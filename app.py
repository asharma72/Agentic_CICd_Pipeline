from fastapi import FastAPI, HTTPException
from fastapi.responses import JSONResponse
from fastapi.requests import Request
import logging
import uvicorn

app = FastAPI()

# Define a custom logger
logger = logging.getLogger(__name__)
logger.setLevel(logging.INFO)

# Create a file handler and a stream handler
file_handler = logging.FileHandler('health_check.log')
stream_handler = logging.StreamHandler()

# Create a formatter and attach it to the handlers
formatter = logging.Formatter('%(asctime)s - %(name)s - %(levelname)s - %(message)s')
file_handler.setFormatter(formatter)
stream_handler.setFormatter(formatter)

# Add the handlers to the logger
logger.addHandler(file_handler)
logger.addHandler(stream_handler)

@app.get("/health-check")
async def health_check():
    try:
        # You can add your custom health check logic here
        # For example, you can check the status of your database connection
        logger.info("Health check endpoint called")
        return {"status": "OK"}
    except Exception as e:
        logger.error(f"Error in health check endpoint: {e}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)

# requirements.txt
# fastapi
# uvicorn
# python 3.9 or higher