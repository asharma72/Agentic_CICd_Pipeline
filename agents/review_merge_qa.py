from langgraph.types import interrupt, Command
from tools.github_tools import merge_pr, post_pr_comment
from langchain_groq import ChatGroq
from langchain_core.messages import SystemMessage, HumanMessage
from datetime import datetime
import os, subprocess

groq_llm = ChatGroq(
    model="llama-3.3-70b-versatile",
    api_key=os.getenv("GROQ_API_KEY")
)


# ── HITL Gate 1: Human Review ────────────────────────────────────────────────

def human_review_gate(state: dict) -> Command:
    """
    HITL Gate 1 — Shows LLM review to human reviewer.
    Pauses pipeline until human approves or requests changes.
    """
    print("\n" + "="*50)
    print("HITL GATE 1 — HUMAN REVIEW")
    print("="*50)

    audit_log = state.get("audit_log", [])

    decision = interrupt({
        "message":         "PR requires human review decision",
        "pr_url":          state.get("pr_url", ""),
        "pr_number":       state.get("pr_number", ""),
        "review_score":    state.get("review_score", 0),
        "review_feedback": state.get("review_feedback", ""),
        "instruction":     "Type 'approve' to merge or 'reject' to send back to developer"
    })

    audit_log.append(
        f"[{datetime.now()}] Human review decision: {decision} "
        f"(PR #{state.get('pr_number')})"
    )

    if decision.strip().lower() == "approve":
        post_pr_comment(
            state["pr_number"],
            "PR approved by human reviewer. Proceeding to merge."
        )
        return Command(
            update={"human_decision": "approve", "audit_log": audit_log},
            goto="merge_agent"
        )
    else:
        post_pr_comment(
            state["pr_number"],
            f"Changes requested by human reviewer. Sending back to developer.\n\n"
            f"Feedback: {decision}"
        )
        return Command(
            update={
                "human_decision":  "reject",
                "review_feedback": state.get("review_feedback", "") + f"\n\nHuman reviewer: {decision}",
                "audit_log":       audit_log
            },
            goto="code_generator"
        )


# ── Merge Agent ──────────────────────────────────────────────────────────────

def merge_agent(state: dict) -> dict:
    """
    Agent 4 — Merges approved PR to release branch.
    """
    print("\n" + "="*50)
    print("MERGE AGENT")
    print("="*50)

    audit_log  = state.get("audit_log", [])
    pr_number  = state["pr_number"]

    print(f"Merging PR #{pr_number} to {os.getenv('GITHUB_BASE_BRANCH', 'main')}...")

    result = merge_pr(
        pr_number=pr_number,
        commit_message=f"feat: {state['requirement'][:50]} [approved by human reviewer]"
    )

    if result["success"]:
        audit_log.append(f"[{datetime.now()}] PR #{pr_number} merged successfully")
        print(f"PR #{pr_number} merged successfully!")
        return {"merged": True, "merge_sha": result.get("sha", ""), "audit_log": audit_log}
    else:
        audit_log.append(f"[{datetime.now()}] Merge failed: {result}")
        print(f"Merge failed: {result}")
        return {"merged": False, "audit_log": audit_log}


# ── QA Test Agent ────────────────────────────────────────────────────────────

def qa_test_agent(state: dict) -> dict:
    """
    Agent 5 — Runs automated tests using Groq.
    Generates and runs test cases against the code.
    """
    print("\n" + "="*50)
    print("QA TEST AGENT (Groq)")
    print("="*50)

    audit_log = state.get("audit_log", [])
    code      = state.get("code", "")

    # Generate tests using Groq
    prompt = f"""Write Python pytest unit tests for this code.
Include at least 3 test functions.
Use only standard library — no external mocks needed.
Do NOT import the app module directly — test logic inline.
Return ONLY the test code. No explanation. No markdown.

Code to test:
{code[:2000]}"""

    print("Generating test cases...")
    result = groq_llm.invoke([
        SystemMessage("You are a QA engineer. Write thorough pytest tests."),
        HumanMessage(content=prompt)
    ])

    test_code = result.content.strip()
    if test_code.startswith("```"):
        test_code = test_code.split("```")[1]
        if test_code.startswith("python"):
            test_code = test_code[6:]
        test_code = test_code.strip()
    if test_code.endswith("```"):
        test_code = test_code[:-3].strip()

    # ── Windows-safe temp file ────────────────────────────────
    import tempfile
    test_file = os.path.join(tempfile.gettempdir(), "test_generated.py")
    print(f"Writing tests to: {test_file}")

    with open(test_file, "w", encoding="utf-8") as f:
        f.write(test_code)

    print("Running tests...")
    result = subprocess.run(
        ["python", "-m", "pytest", test_file, "-v", "--tb=short"],
        capture_output=True, text=True, timeout=60
    )

    test_output  = result.stdout + result.stderr
    tests_passed = result.returncode == 0

    # Count results
    passed = test_output.count(" PASSED")
    failed = test_output.count(" FAILED")
    errors = test_output.count(" ERROR")

    summary = f"{passed} passed, {failed} failed, {errors} errors"
    print(f"Test results: {summary}")

    audit_log.append(
        f"[{datetime.now()}] QA tests complete: {summary}, "
        f"overall={'PASSED' if tests_passed else 'FAILED'}"
    )

    return {
        "test_results": test_output[:2000],
        "tests_passed": tests_passed,
        "test_summary": summary,
        "audit_log":    audit_log
    }