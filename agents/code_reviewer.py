from langchain_openai import ChatOpenAI
from langchain_core.messages import SystemMessage, HumanMessage
from tools.github_tools import get_pr_diff, get_pr_files, post_pr_comment
from datetime import datetime
import os, json

openai_llm = ChatOpenAI(
    model="gpt-4o-mini",
    api_key=os.getenv("OPENAI_API_KEY")
)


def code_reviewer(state: dict) -> dict:
    """
    Agent 3 — Reviews the PR code using OpenAI GPT-4o-mini.
    Posts review feedback as a PR comment.
    Scores code quality 1-10.
    """
    print("\n" + "="*50)
    print("CODE REVIEWER AGENT (OpenAI GPT-4o-mini)")
    print("="*50)

    audit_log  = state.get("audit_log", [])
    pr_number  = state["pr_number"]

    # Get the actual code diff from GitHub
    diff  = get_pr_diff(pr_number)
    files = get_pr_files(pr_number)

    files_summary = "\n".join(
        [f"- {f['filename']}: +{f['additions']} -{f['deletions']} lines"
         for f in files]
    ) if files else "Could not fetch file list"

    prompt = f"""You are a senior software engineer conducting a code review.
Review this pull request thoroughly and provide structured feedback.

REQUIREMENT BEING IMPLEMENTED:
{state['requirement']}

FILES CHANGED:
{files_summary}

CODE DIFF:
{diff}

Provide your review in this exact JSON format:
{{
  "score": 7.5,
  "approved": false,
  "summary": "Overall assessment in 2 sentences",
  "issues": [
    {{"severity": "critical", "description": "Issue description", "line": "approximate location"}},
    {{"severity": "minor", "description": "Minor issue"}}
  ],
  "positives": ["Good thing 1", "Good thing 2"],
  "suggestions": ["Suggestion 1", "Suggestion 2"],
  "security_concerns": ["Any security issues or 'None found'"]
}}

Score guide: 9-10=excellent, 7-8=good, 5-6=needs work, 1-4=major issues
Set approved=true only if score >= 7 and no critical issues."""

    print(f"Reviewing PR #{pr_number}...")
    result = openai_llm.invoke([
        SystemMessage("You are a senior engineer. Review code strictly but fairly."),
        HumanMessage(content=prompt)
    ])

    raw = result.content.strip()
    if raw.startswith("```"):
        raw = raw.split("```")[1]
        if raw.startswith("json"): raw = raw[4:]
        raw = raw.strip()
    if raw.endswith("```"):
        raw = raw[:-3].strip()

    try:
        review_data = json.loads(raw)
    except json.JSONDecodeError:
        review_data = {
            "score":            6.0,
            "approved":         False,
            "summary":          raw[:200],
            "issues":           [],
            "positives":        [],
            "suggestions":      [],
            "security_concerns":["Could not parse review"]
        }

    score    = float(review_data.get("score", 5.0))
    approved = bool(review_data.get("approved", False))

    # Format feedback for developer
    issues_text = "\n".join(
        [f"- [{i['severity'].upper()}] {i['description']}"
         for i in review_data.get("issues", [])]
    ) or "No issues found"

    suggestions_text = "\n".join(
        [f"- {s}" for s in review_data.get("suggestions", [])]
    ) or "No suggestions"

    feedback = f"""## AI Code Review Summary

**Score: {score}/10** | **Status: {'APPROVED' if approved else 'CHANGES REQUESTED'}**

### Summary
{review_data.get('summary', 'No summary')}

### Issues Found
{issues_text}

### Suggestions
{suggestions_text}

### Security Concerns
{chr(10).join(['- ' + s for s in review_data.get('security_concerns', ['None'])])}

### Positives
{chr(10).join(['- ' + p for p in review_data.get('positives', [])])}

---
*Reviewed by AI Code Reviewer (GPT-4o-mini)*"""

    # Post review as PR comment
    post_pr_comment(pr_number, feedback)

    audit_log.append(
        f"[{datetime.now()}] Code review completed: score={score}/10, "
        f"approved={approved}"
    )

    print(f"Review score: {score}/10")
    print(f"Auto-approved: {approved}")
    print(f"Issues found: {len(review_data.get('issues', []))}")

    return {
        "review_feedback": feedback,
        "review_score":    score,
        "review_approved": approved,
        "audit_log":       audit_log
    }
