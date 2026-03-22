from langchain_groq import ChatGroq
from langchain_core.messages import SystemMessage, HumanMessage
from tools.github_tools import create_branch, commit_file
from datetime import datetime
import os, re

groq_llm = ChatGroq(
    model="llama-3.3-70b-versatile",
    api_key=os.getenv("GROQ_API_KEY")
)


def code_generator(state: dict) -> dict:
    """
    Agent 1 — Generates code based on requirement.
    Uses Groq (fast) for code generation.
    Commits code to a new GitHub branch.
    """
    print("\n" + "="*50)
    print("CODE GENERATOR AGENT")
    print("="*50)

    requirement  = state["requirement"]
    iteration    = state.get("iteration", 0)
    feedback     = state.get("review_feedback", "")
    audit_log    = state.get("audit_log", [])

    # Build prompt — include feedback if this is a revision
    feedback_section = ""
    if feedback and iteration > 0:
        feedback_section = f"""
REVIEWER FEEDBACK TO ADDRESS:
{feedback}

Fix all the issues mentioned above in your revised code.
"""

    prompt = f"""You are an expert software engineer.
Write clean, production-ready code for this requirement.
Include proper error handling, comments, and a requirements.txt.

REQUIREMENT:
{requirement}
{feedback_section}
Return ONLY the Python code. No explanation. No markdown fences."""

    print(f"Generating code (iteration {iteration + 1})...")
    result = groq_llm.invoke([
        SystemMessage("You are an expert Python developer. Write clean production code."),
        HumanMessage(content=prompt)
    ])

    code = result.content.strip()
    if code.startswith("```"):
        code = code.split("```")[1]
        if code.startswith("python"): code = code[6:]
        code = code.strip()
    if code.endswith("```"):
        code = code[:-3].strip()

    # Generate requirements.txt
    reqs_prompt = f"""List only the pip package names needed for this code.
One package per line. No versions. No comments.
Code: {code[:500]}"""

    reqs_result = groq_llm.invoke([HumanMessage(content=reqs_prompt)])
    requirements = reqs_result.content.strip()

    # Create branch name
    timestamp   = datetime.now().strftime("%Y%m%d%H%M%S")
    safe_name   = re.sub(r'[^a-z0-9]', '-', requirement[:30].lower())
    branch_name = f"feature/{safe_name}-{timestamp}"

    print(f"Creating branch: {branch_name}")

    # Push to GitHub
    branch_result = create_branch(branch_name)
    if not branch_result["success"]:
        audit_log.append(f"[{datetime.now()}] Branch creation failed: {branch_result}")
        return {"audit_log": audit_log, "error": "Branch creation failed"}

    # Commit the main code file
    commit_result = commit_file(
        branch=branch_name,
        file_path="app.py",
        content=code,
        message=f"feat: {requirement[:50]} (iteration {iteration + 1})"
    )

    # Commit requirements
    commit_file(
        branch=branch_name,
        file_path="requirements.txt",
        content=requirements,
        message="chore: add requirements"
    )

    # Commit a basic Dockerfile
    dockerfile = f"""FROM python:3.11-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
EXPOSE 8080
CMD ["python", "app.py"]
"""
    commit_file(
        branch=branch_name,
        file_path="Dockerfile",
        content=dockerfile,
        message="chore: add Dockerfile for Docker deploy"
    )

    audit_log.append(
        f"[{datetime.now()}] Code generated and committed to {branch_name} "
        f"(iteration {iteration + 1})"
    )

    print(f"Code committed to branch: {branch_name}")
    print(f"Code length: {len(code)} chars")

    return {
        "code":        code,
        "branch_name": branch_name,
        "iteration":   iteration + 1,
        "audit_log":   audit_log
    }
