from langgraph.types import interrupt, Command
from tools.docker_tools import (
    build_docker_image, deploy_uat, deploy_prod,
    run_smoke_tests, get_container_logs
)
from datetime import datetime
import os


# ── UAT Deploy Agent ─────────────────────────────────────────────────────────

def uat_deploy_agent(state: dict) -> dict:
    """
    Agent 6 — Builds Docker image and deploys to UAT.
    Runs smoke tests after deployment.
    """
    print("\n" + "="*50)
    print("UAT DEPLOY AGENT")
    print("="*50)

    audit_log  = state.get("audit_log", [])
    image_name = os.getenv("UAT_IMAGE_NAME", "myapp-uat")
    tag        = f"uat-{datetime.now().strftime('%Y%m%d%H%M%S')}"

    # Build Docker image
    print(f"Building Docker image: {image_name}:{tag}")
    build_result = build_docker_image(image_name, tag)

    if not build_result["success"]:
        audit_log.append(f"[{datetime.now()}] UAT build failed: {build_result['stderr']}")
        print(f"Build failed: {build_result['stderr'][:200]}")
        return {
            "uat_status":  "build_failed",
            "uat_url":     "",
            "audit_log":   audit_log
        }

    print(f"Build successful! Deploying to UAT...")

    # Deploy to UAT
    deploy_result = deploy_uat(image_name, tag)

    if not deploy_result["success"]:
        audit_log.append(f"[{datetime.now()}] UAT deploy failed: {deploy_result['stderr']}")
        print(f"Deploy failed: {deploy_result['stderr'][:200]}")
        return {
            "uat_status":  "deploy_failed",
            "uat_url":     "",
            "audit_log":   audit_log
        }

    uat_url = deploy_result["url"]
    print(f"UAT deployed at: {uat_url}")

    # Run smoke tests
    print("Running smoke tests...")
    smoke_results = run_smoke_tests(uat_url)
    print(f"Smoke tests: {smoke_results['summary']}")

    # Get container logs for audit
    logs = get_container_logs(deploy_result["container"], lines=20)

    uat_status = "healthy" if smoke_results["all_passed"] else "unhealthy"

    audit_log.append(
        f"[{datetime.now()}] UAT deploy complete: {uat_url}, "
        f"smoke tests: {smoke_results['summary']}, status: {uat_status}"
    )

    return {
        "uat_url":          uat_url,
        "uat_status":       uat_status,
        "uat_image_tag":    f"{image_name}:{tag}",
        "smoke_test_results": str(smoke_results),
        "audit_log":        audit_log
    }


# ── HITL Gate 2: Prod Approval ───────────────────────────────────────────────

def prod_approval_gate(state: dict) -> Command:
    """
    HITL Gate 2 — Shows UAT results to human.
    Requires explicit human approval before prod deploy.
    """
    print("\n" + "="*50)
    print("HITL GATE 2 — PROD APPROVAL")
    print("="*50)

    audit_log = state.get("audit_log", [])

    decision = interrupt({
        "message":        "UAT complete — approve production deployment?",
        "uat_url":        state.get("uat_url", ""),
        "uat_status":     state.get("uat_status", ""),
        "test_summary":   state.get("test_summary", ""),
        "smoke_tests":    state.get("smoke_test_results", ""),
        "requirement":    state.get("requirement", ""),
        "instruction":    "Type 'approve' to deploy to prod or 'reject' to abort"
    })

    audit_log.append(
        f"[{datetime.now()}] Prod approval decision: {decision}"
    )

    if decision.strip().lower() == "approve":
        return Command(
            update={"prod_approved": True, "audit_log": audit_log},
            goto="prod_deploy_agent"
        )
    else:
        return Command(
            update={"prod_approved": False, "audit_log": audit_log},
            goto="pipeline_complete"
        )


# ── Prod Deploy Agent ─────────────────────────────────────────────────────────

def prod_deploy_agent(state: dict) -> dict:
    """
    Agent 7 — Deploys to production Docker container.
    Only runs after explicit human approval.
    """
    print("\n" + "="*50)
    print("PROD DEPLOY AGENT")
    print("="*50)

    audit_log  = state.get("audit_log", [])
    image_name = os.getenv("PROD_IMAGE_NAME", "myapp-prod")

    # Re-tag UAT image as prod
    uat_tag   = state.get("uat_image_tag", "")
    prod_tag  = f"prod-{datetime.now().strftime('%Y%m%d%H%M%S')}"

    print(f"Deploying to PRODUCTION: {image_name}:{prod_tag}")

    # Build fresh prod image
    build_result = build_docker_image(image_name, prod_tag)

    if not build_result["success"]:
        audit_log.append(f"[{datetime.now()}] PROD build failed")
        return {
            "prod_url":    "",
            "prod_status": "build_failed",
            "audit_log":   audit_log
        }

    # Deploy to prod
    deploy_result = deploy_prod(image_name, prod_tag)

    if deploy_result["success"]:
        prod_url = deploy_result["url"]
        audit_log.append(
            f"[{datetime.now()}] PRODUCTION deploy successful: {prod_url} "
            f"image={image_name}:{prod_tag}"
        )
        print(f"PRODUCTION live at: {prod_url}")
        return {
            "prod_url":        prod_url,
            "prod_status":     "live",
            "prod_image_tag":  f"{image_name}:{prod_tag}",
            "audit_log":       audit_log
        }
    else:
        audit_log.append(f"[{datetime.now()}] PROD deploy failed: {deploy_result['stderr']}")
        print(f"Prod deploy failed: {deploy_result['stderr'][:200]}")
        return {
            "prod_url":    "",
            "prod_status": "deploy_failed",
            "audit_log":   audit_log
        }


# ── Pipeline Complete ─────────────────────────────────────────────────────────

def pipeline_complete(state: dict) -> dict:
    """Final node — prints audit trail and summary."""
    print("\n" + "="*60)
    print("PIPELINE COMPLETE")
    print("="*60)

    audit_log = state.get("audit_log", [])

    print(f"\nRequirement:   {state.get('requirement', '')}")
    print(f"PR:            #{state.get('pr_number', 'N/A')} — {state.get('pr_url', '')}")
    print(f"Tests:         {state.get('test_summary', 'N/A')}")
    print(f"UAT:           {state.get('uat_url', 'N/A')} ({state.get('uat_status', 'N/A')})")
    print(f"Prod:          {state.get('prod_url', 'N/A')} ({state.get('prod_status', 'N/A')})")
    print(f"Iterations:    {state.get('iteration', 0)}")

    print("\nAUDIT TRAIL:")
    for entry in audit_log:
        print(f"  {entry}")

    audit_log.append(f"[{datetime.now()}] Pipeline complete")
    return {"audit_log": audit_log}
