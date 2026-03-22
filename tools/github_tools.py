import requests
import os
import base64
from datetime import datetime

GITHUB_TOKEN = os.getenv("GITHUB_TOKEN")
GITHUB_REPO  = os.getenv("GITHUB_REPO")
BASE_BRANCH  = os.getenv("GITHUB_BASE_BRANCH", "main")

HEADERS = {
    "Authorization": f"token {GITHUB_TOKEN}",
    "Accept": "application/vnd.github.v3+json"
}
BASE_URL = f"https://api.github.com/repos/{GITHUB_REPO}"


def create_branch(branch_name: str) -> dict:
    """Create a new branch from base branch."""

    # ── Debug: print exactly what we are calling ──────────────
    print(f"\nDEBUG GitHub:")
    print(f"  GITHUB_REPO    = '{GITHUB_REPO}'")
    print(f"  BASE_BRANCH    = '{BASE_BRANCH}'")
    print(f"  GITHUB_TOKEN   = '{str(GITHUB_TOKEN)[:8]}...' (first 8 chars)")
    print(f"  URL = {BASE_URL}/git/ref/heads/{BASE_BRANCH}")

    # Get base branch SHA
    r = requests.get(
        f"{BASE_URL}/git/ref/heads/{BASE_BRANCH}",
        headers=HEADERS
    )

    print(f"  Response status: {r.status_code}")
    print(f"  Response body:   {r.json()}")

    # ── Handle errors ──────────────────────────────────────────
    if r.status_code == 401:
        return {"success": False, "error": "GITHUB_TOKEN is invalid or expired — regenerate it"}

    if r.status_code == 404:
        return {"success": False, "error": f"Repo '{GITHUB_REPO}' or branch '{BASE_BRANCH}' not found — check .env"}

    if r.status_code != 200:
        return {"success": False, "error": f"GitHub API error {r.status_code}: {r.json()}"}

    data = r.json()
    if "object" not in data:
        return {"success": False, "error": f"Unexpected response structure: {data}"}

    sha = data["object"]["sha"]

    # Create new branch
    payload = {"ref": f"refs/heads/{branch_name}", "sha": sha}
    r2 = requests.post(f"{BASE_URL}/git/refs", json=payload, headers=HEADERS)

    if r2.status_code == 201:
        return {"success": True, "branch": branch_name, "sha": sha}
    if r2.status_code == 422:
        # Branch already exists — still ok
        return {"success": True, "branch": branch_name, "sha": sha}

    return {"success": False, "error": f"Branch create failed {r2.status_code}: {r2.json()}"}


def commit_file(branch: str, file_path: str, content: str, message: str) -> dict:
    """Commit a file to a branch."""
    encoded = base64.b64encode(content.encode()).decode()

    r = requests.get(
        f"{BASE_URL}/contents/{file_path}?ref={branch}",
        headers=HEADERS
    )
    payload = {"message": message, "content": encoded, "branch": branch}
    if r.status_code == 200:
        payload["sha"] = r.json()["sha"]

    r = requests.put(
        f"{BASE_URL}/contents/{file_path}",
        json=payload, headers=HEADERS
    )
    if r.status_code in [200, 201]:
        return {"success": True, "file": file_path}
    return {"success": False, "error": r.json()}


def create_pr(branch: str, title: str, body: str) -> dict:
    """Create a pull request."""
    payload = {
        "title": title,
        "body":  body,
        "head":  branch,
        "base":  BASE_BRANCH
    }
    r = requests.post(f"{BASE_URL}/pulls", json=payload, headers=HEADERS)
    if r.status_code == 201:
        data = r.json()
        return {
            "success":   True,
            "pr_number": data["number"],
            "pr_url":    data["html_url"],
            "pr_title":  data["title"]
        }
    return {"success": False, "error": r.json()}


def get_pr_diff(pr_number: int) -> str:
    """Get the diff of a PR for review."""
    headers = {**HEADERS, "Accept": "application/vnd.github.v3.diff"}
    r = requests.get(f"{BASE_URL}/pulls/{pr_number}", headers=headers)
    return r.text[:4000] if r.status_code == 200 else "Could not fetch diff"


def get_pr_files(pr_number: int) -> list:
    """Get list of files changed in a PR."""
    r = requests.get(f"{BASE_URL}/pulls/{pr_number}/files", headers=HEADERS)
    if r.status_code == 200:
        return [{"filename": f["filename"], "additions": f["additions"],
                 "deletions": f["deletions"], "patch": f.get("patch", "")[:500]}
                for f in r.json()]
    return []


def post_pr_comment(pr_number: int, comment: str) -> dict:
    """Post a comment on a PR."""
    payload = {"body": comment}
    r = requests.post(
        f"{BASE_URL}/issues/{pr_number}/comments",
        json=payload, headers=HEADERS
    )
    return {"success": r.status_code == 201}


def merge_pr(pr_number: int, commit_message: str = "Approved and merged by CI/CD agent") -> dict:
    """Merge an approved PR."""
    payload = {"commit_title": commit_message, "merge_method": "squash"}
    r = requests.put(
        f"{BASE_URL}/pulls/{pr_number}/merge",
        json=payload, headers=HEADERS
    )
    if r.status_code == 200:
        return {"success": True, "sha": r.json().get("sha")}
    return {"success": False, "error": r.json()}


def get_repo_info() -> dict:
    """Get basic repo info."""
    r = requests.get(BASE_URL, headers=HEADERS)
    if r.status_code == 200:
        data = r.json()
        return {
            "name":           data["name"],
            "default_branch": data["default_branch"],
            "language":       data.get("language", "Unknown")
        }
    return {}