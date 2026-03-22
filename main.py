"""
Agentic AI CI/CD Pipeline — Day CICD
Full pipeline: Code → PR → Review → HITL → Merge → QA → UAT → HITL → Prod

Agents:
  1. code_generator    — Groq: writes code, commits to GitHub branch
  2. pr_creator        — Groq: raises PR on GitHub
  3. code_reviewer     — OpenAI: reviews PR diff, scores quality
  4. human_review_gate — HITL: human approves or rejects
  5. merge_agent       — merges approved PR
  6. qa_test_agent     — Groq: generates + runs tests
  7. uat_deploy_agent  — Docker: builds + deploys to UAT
  8. prod_approval_gate— HITL: human approves prod deploy
  9. prod_deploy_agent — Docker: deploys to production
"""

from typing import TypedDict, List, Optional
from langgraph.graph import StateGraph, START, END
from langgraph.checkpoint.memory import MemorySaver
from langgraph.types import Command
from dotenv import load_dotenv
import os, uuid

load_dotenv()

# Import all agents
from agents.code_generator   import code_generator
from agents.pr_creator        import pr_creator
from agents.code_reviewer     import code_reviewer
from agents.review_merge_qa   import (
    human_review_gate, merge_agent, qa_test_agent
)
from agents.deploy_agents     import (
    uat_deploy_agent, prod_approval_gate,
    prod_deploy_agent, pipeline_complete
)


# ── State ────────────────────────────────────────────────────────────────────

class PipelineState(TypedDict):
    # Input
    requirement:        str

    # Code generation (Agent 1)
    code:               str
    branch_name:        str
    iteration:          int

    # PR (Agent 2)
    pr_number:          int
    pr_url:             str

    # Code review (Agent 3)
    review_feedback:    str
    review_score:       float
    review_approved:    bool

    # HITL Gate 1
    human_decision:     str

    # Merge (Agent 4)
    merged:             bool
    merge_sha:          str

    # QA (Agent 5)
    test_results:       str
    tests_passed:       bool
    test_summary:       str

    # UAT Deploy (Agent 6)
    uat_url:            str
    uat_status:         str
    uat_image_tag:      str
    smoke_test_results: str

    # HITL Gate 2
    prod_approved:      bool

    # Prod Deploy (Agent 7)
    prod_url:           str
    prod_status:        str
    prod_image_tag:     str

    # Audit
    audit_log:          List[str]


# ── Routers ──────────────────────────────────────────────────────────────────

MAX_ITERATIONS = 3

def route_after_qa(state: PipelineState) -> str:
    """After QA — always proceed to UAT regardless of test results."""
    if state.get("tests_passed"):
        print("All tests passed — deploying to UAT")
    else:
        print("Some tests failed — proceeding to UAT anyway (check logs)")
    return "uat_deploy_agent"

def route_after_uat(state: PipelineState) -> str:
    """After UAT — go to prod approval gate."""
    return "prod_approval_gate"


# ── Build Graph ───────────────────────────────────────────────────────────────

def build_pipeline():
    graph = StateGraph(PipelineState)

    # Register all nodes
    graph.add_node("code_generator",    code_generator)
    graph.add_node("pr_creator",        pr_creator)
    graph.add_node("code_reviewer",     code_reviewer)
    graph.add_node("human_review_gate", human_review_gate)
    graph.add_node("merge_agent",       merge_agent)
    graph.add_node("qa_test_agent",     qa_test_agent)
    graph.add_node("uat_deploy_agent",  uat_deploy_agent)
    graph.add_node("prod_approval_gate",prod_approval_gate)
    graph.add_node("prod_deploy_agent", prod_deploy_agent)
    graph.add_node("pipeline_complete", pipeline_complete)

    # ── Fixed edges ───────────────────────────────────────────────────────────
    graph.add_edge(START,               "code_generator")
    graph.add_edge("code_generator",    "pr_creator")
    graph.add_edge("pr_creator",        "code_reviewer")
    graph.add_edge("code_reviewer",     "human_review_gate")
    # human_review_gate uses Command(goto=) → merge_agent OR code_generator
    graph.add_edge("merge_agent",       "qa_test_agent")

    graph.add_conditional_edges(
        "qa_test_agent",
        route_after_qa,
        {"uat_deploy_agent": "uat_deploy_agent"}
    )

    graph.add_conditional_edges(
        "uat_deploy_agent",
        route_after_uat,
        {"prod_approval_gate": "prod_approval_gate"}
    )

    # prod_approval_gate uses Command(goto=) → prod_deploy_agent OR pipeline_complete
    graph.add_edge("prod_deploy_agent", "pipeline_complete")
    graph.add_edge("pipeline_complete", END)

    checkpointer = MemorySaver()
    return graph.compile(checkpointer=checkpointer)


# ── Runner ────────────────────────────────────────────────────────────────────

def run_pipeline(requirement: str):
    """Run the full CI/CD pipeline for a given requirement."""
    app       = build_pipeline()
    thread_id = str(uuid.uuid4())
    config    = {"configurable": {"thread_id": thread_id}}

    initial_state = {
        "requirement":        requirement,
        "code":               "",
        "branch_name":        "",
        "iteration":          0,
        "pr_number":          0,
        "pr_url":             "",
        "review_feedback":    "",
        "review_score":       0.0,
        "review_approved":    False,
        "human_decision":     "",
        "merged":             False,
        "merge_sha":          "",
        "test_results":       "",
        "tests_passed":       False,
        "test_summary":       "",
        "uat_url":            "",
        "uat_status":         "",
        "uat_image_tag":      "",
        "smoke_test_results": "",
        "prod_approved":      False,
        "prod_url":           "",
        "prod_status":        "",
        "prod_image_tag":     "",
        "audit_log":          []
    }

    print("\n" + "="*60)
    print("AGENTIC AI CI/CD PIPELINE")
    print("="*60)
    print(f"Requirement: {requirement}")
    print(f"Thread ID:   {thread_id}")
    print("="*60)

    while True:
        result = app.invoke(initial_state, config=config)
        state  = app.get_state(config)

        # Check if paused at a HITL gate
        if state.next:
            next_node = state.next[0] if state.next else ""

            if "human_review_gate" in str(state.next):
                # Gate 1: Code review
                print("\n" + "="*60)
                print("HITL GATE 1 — CODE REVIEW DECISION")
                print("="*60)
                print(f"PR URL:       {result.get('pr_url', 'N/A')}")
                print(f"Review Score: {result.get('review_score', 0)}/10")
                print("\nREVIEW FEEDBACK:")
                print(result.get("review_feedback", "No feedback")[:800])
                print("="*60)
                print("\nOptions:")
                print("  approve         — merge PR and continue")
                print("  reject          — send back to developer")
                print("  reject: <notes> — reject with specific feedback")

                decision = input("\nYour decision: ").strip()
                initial_state = Command(resume=decision)

            elif "prod_approval_gate" in str(state.next):
                # Gate 2: Prod deploy
                print("\n" + "="*60)
                print("HITL GATE 2 — PRODUCTION DEPLOYMENT APPROVAL")
                print("="*60)
                print(f"UAT URL:     {result.get('uat_url', 'N/A')}")
                print(f"UAT Status:  {result.get('uat_status', 'N/A')}")
                print(f"Tests:       {result.get('test_summary', 'N/A')}")
                print("="*60)
                print("\nOptions:")
                print("  approve — deploy to production")
                print("  reject  — abort production deploy")

                decision = input("\nApprove production deploy? ").strip()
                initial_state = Command(resume=decision)

            else:
                print(f"Pipeline paused at: {state.next}")
                break
        else:
            # Pipeline completed
            print("\n" + "="*60)
            print("PIPELINE FINISHED")
            print(f"Prod URL:    {result.get('prod_url', 'N/A')}")
            print(f"Prod Status: {result.get('prod_status', 'N/A')}")
            print("="*60)
            break

    return result


# ── Entry Point ────────────────────────────────────────────────────────────────

if __name__ == "__main__":
    print("="*60)
    print("AGENTIC AI CI/CD PIPELINE")
    print("Powered by LangGraph + GitHub + Docker")
    print("="*60)

    requirement = input("\nEnter your requirement: ").strip()

    if not requirement:
        requirement = "Create a FastAPI health check endpoint that returns status and timestamp"

    run_pipeline(requirement)
