from fastapi import FastAPI, HTTPException
from fastapi.responses import JSONResponse
from fastapi.requests import Request
import logging
import uvicorn

app = FastAPI(
    title="Health Check API",
    description="A simple API to check the health of the service",
    version="1.0.0"
)

@app.get("/health")
async def read_health():
    try:
        # Simulating a database connection check
        # Replace this with your actual database connection check
        # For demonstration purposes, this will always return True
        db_connection = True
        
        if db_connection:
            return {"status": "ok"}
        else:
            raise HTTPException(status_code=500, detail="Database connection failed")
    except Exception as e:
        logging.error(f"Error in health check: {str(e)}")
        raise HTTPException(status_code=500, detail="Internal Server Error")

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)

# requirements.txt
# fastapi
# uvicorn
# logging