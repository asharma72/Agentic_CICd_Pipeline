import subprocess
import os
from datetime import datetime

UAT_IMAGE   = os.getenv("UAT_IMAGE_NAME",  "myapp-uat")
PROD_IMAGE  = os.getenv("PROD_IMAGE_NAME", "myapp-prod")
UAT_PORT    = os.getenv("UAT_CONTAINER_PORT",  "8080")
PROD_PORT   = os.getenv("PROD_CONTAINER_PORT", "80")


def run_cmd(cmd: str) -> dict:
    """Run a shell command and return output."""
    result = subprocess.run(
        cmd, shell=True, capture_output=True, text=True, timeout=120
    )
    return {
        "returncode": result.returncode,
        "stdout":     result.stdout.strip(),
        "stderr":     result.stderr.strip(),
        "success":    result.returncode == 0
    }


def generate_dockerfile(language: str = "python") -> str:
    """Generate a basic Dockerfile for the project."""
    if language.lower() == "python":
        return """FROM python:3.11-slim
WORKDIR /app
COPY requirements.txt .
RUN pip install --no-cache-dir -r requirements.txt
COPY . .
EXPOSE 8080
CMD ["python", "app.py"]
"""
    elif language.lower() in ["node", "javascript"]:
        return """FROM node:20-slim
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
EXPOSE 8080
CMD ["node", "index.js"]
"""
    return """FROM ubuntu:22.04
WORKDIR /app
COPY . .
EXPOSE 8080
CMD ["/bin/bash"]
"""


def build_docker_image(image_name: str, tag: str = "latest",
                       context_path: str = ".") -> dict:
    """Build a Docker image."""
    full_tag = f"{image_name}:{tag}"
    print(f"Building Docker image: {full_tag}")
    result = run_cmd(f"docker build -t {full_tag} {context_path}")
    result["image"] = full_tag
    return result


def stop_container(container_name: str) -> dict:
    """Stop and remove an existing container."""
    run_cmd(f"docker stop {container_name} 2>/dev/null || true")
    run_cmd(f"docker rm {container_name} 2>/dev/null || true")
    return {"success": True, "stopped": container_name}


def deploy_uat(image_name: str = None, tag: str = "latest") -> dict:
    """Deploy to UAT environment via Docker."""
    image = image_name or UAT_IMAGE
    full_tag = f"{image}:{tag}"
    container_name = f"{image}-container"

    print(f"Deploying to UAT: {full_tag}")

    # Stop existing container
    stop_container(container_name)

    # Run new container
    cmd = (f"docker run -d "
           f"--name {container_name} "
           f"-p {UAT_PORT}:8080 "
           f"--env-file .env "
           f"{full_tag}")

    result = run_cmd(cmd)
    result["container"] = container_name
    result["url"] = f"http://localhost:{UAT_PORT}"
    return result


def deploy_prod(image_name: str = None, tag: str = "latest") -> dict:
    """Deploy to Production environment via Docker."""
    image = image_name or PROD_IMAGE
    full_tag = f"{image}:{tag}"
    container_name = f"{image}-container"

    print(f"Deploying to PRODUCTION: {full_tag}")

    stop_container(container_name)

    cmd = (f"docker run -d "
           f"--name {container_name} "
           f"-p {PROD_PORT}:8080 "
           f"--restart always "
           f"--env-file .env "
           f"{full_tag}")

    result = run_cmd(cmd)
    result["container"] = container_name
    result["url"] = f"http://localhost:{PROD_PORT}"
    return result


def run_smoke_tests(base_url: str) -> dict:
    """Run basic smoke tests against deployed container."""
    import time
    time.sleep(3)  # wait for container to start

    tests = []

    # Test 1: health check
    r = run_cmd(f"curl -sf {base_url}/health -o /dev/null -w '%{{http_code}}'")
    tests.append({
        "name":    "health_check",
        "passed":  r["stdout"] in ["200", "201"],
        "details": f"HTTP {r['stdout']}"
    })

    # Test 2: container running
    r = run_cmd(f"docker ps --filter name={base_url.split('/')[-1]} --format '{{{{.Status}}}}'")
    tests.append({
        "name":    "container_running",
        "passed":  "Up" in r["stdout"],
        "details": r["stdout"] or "Container not found"
    })

    passed = sum(1 for t in tests if t["passed"])
    return {
        "tests":        tests,
        "total":        len(tests),
        "passed":       passed,
        "failed":       len(tests) - passed,
        "all_passed":   passed == len(tests),
        "summary":      f"{passed}/{len(tests)} smoke tests passed"
    }


def get_container_logs(container_name: str, lines: int = 50) -> str:
    """Get recent logs from a container."""
    r = run_cmd(f"docker logs --tail {lines} {container_name}")
    return r["stdout"] or r["stderr"]
