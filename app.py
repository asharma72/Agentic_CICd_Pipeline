from fastapi import FastAPI, HTTPException
from fastapi.responses import JSONResponse
from fastapi.requests import Request
import logging

app = FastAPI()

@app.get("/health", tags=["health"])
async def health_check():
    try:
        # Check if the service is up and running
        return JSONResponse(content={"status": "ok"}, status_code=200)
    except Exception as e:
        logging.error(f"Error during health check: {str(e)}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

@app.exception_handler(Exception)
async def catch_all_exception_handler(request: Request, exc: Exception):
    logging.error(f"Request: {request.method} {request.url.path} - Exception: {str(exc)}")
    return JSONResponse(status_code=500, content={"detail": "Internal Server Error"})

@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    logging.error(f"Request: {request.method} {request.url.path} - HTTP Exception: {exc.detail} - Status Code: {exc.status_code}")
    return JSONResponse(status_code=exc.status_code, content={"detail": exc.detail})