from fastapi import FastAPI
from fastapi.responses import JSONResponse
from fastapi.requests import Request
from fastapi.exceptions import HTTPException
import logging

app = FastAPI()

@app.get("/healthcheck")
async def healthcheck():
    try:
        return JSONResponse(content={"status": "OK"}, status_code=200)
    except Exception as e:
        logging.error(f"Error in healthcheck endpoint: {str(e)}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

@app.exception_handler(HTTPException)
async def http_exception_handler(request: Request, exc: HTTPException):
    return JSONResponse(status_code=exc.status_code, content={"error": exc.detail})

@app.exception_handler(Exception)
async def generic_exception_handler(request: Request, exc: Exception):
    logging.error(f"Generic error: {str(exc)}")
    return JSONResponse(status_code=500, content={"error": "Internal Server Error"})