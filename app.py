from fastapi import FastAPI
from fastapi.responses import JSONResponse
from fastapi.requests import Request
from fastapi.exceptions import HTTPException
import uvicorn
import logging

# Initialize the FastAPI application
app = FastAPI()

# Initialize the logger
logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Health check endpoint
@app.get("/health")
async def health_check():
    try:
        # Check the application's health
        # Replace this with your own health check logic
        health_status = {"status": "ok"}
        return JSONResponse(content=health_status, status_code=200)
    except Exception as e:
        # Log the error and return a 500 error response
        logger.error(f"Error in health check: {str(e)}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

# Error handler for unhandled exceptions
@app.exception_handler(Exception)
async def handle_exception(request: Request, exc: Exception):
    # Log the error and return a 500 error response
    logger.error(f"Unhandled exception: {str(exc)}")
    return JSONResponse(status_code=500, content={"error": "Internal Server Error"})

# Run the application
if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)