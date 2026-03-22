from tools.github_tools import create_pr, get_repo_info
from langchain_groq import ChatGroq
from langchain_core.messages import HumanMessage
from datetime import datetime
import os

groq_llm = ChatGroq(
    model="llama-3.3-70b-versatile",
    api_key=os.getenv("GROQ_API_KEY")
)


def pr_creator(state: dict) -> dict:
    """
    Agent 2 — Creates a GitHub Pull Request.
    Writes a professional PR description automatically.
    """
    print("\n" + "="*50)
    print("PR CREATOR AGENT")
    print("="*50)

    audit_log = state.get("audit_log", [])

    # Generate PR description using LLM
    prompt = f"""Write a professional GitHub Pull Request description.
Include: Summary, Changes Made, Testing Notes, and Checklist.
Keep it concise and technical.

Requirement: {state['requirement']}
Branch: {state['branch_name']}"""

    result  = groq_llm.invoke([HumanMessage(content=prompt)])
    pr_body = result.content.strip()

    pr_title = f"feat: {state['requirement'][:60]}"

    print(f"Creating PR: {pr_title}")

    pr_result = create_pr(
        branch=state["branch_name"],
        title=pr_title,
        body=pr_body
    )

    if pr_result["success"]:
        audit_log.append(
            f"[{datetime.now()}] PR created: #{pr_result['pr_number']} "
            f"- {pr_result['pr_url']}"
        )
        print(f"PR created: #{pr_result['pr_number']}")
        print(f"PR URL: {pr_result['pr_url']}")
        return {
            "pr_number":  pr_result["pr_number"],
            "pr_url":     pr_result["pr_url"],
            "audit_log":  audit_log
        }
    else:
        audit_log.append(f"[{datetime.now()}] PR creation failed: {pr_result}")
        print(f"PR creation failed: {pr_result}")
        return {"audit_log": audit_log}
