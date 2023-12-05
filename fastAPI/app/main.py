from typing import Union
from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware

app = FastAPI()
origins = [
    "http://localhost",
    "http://localhost:8080",
]
app.add_middleware(
    CORSMiddleware,
    allow_origins=origins,
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)
@app.get("/")
def read_root():
    return {"Hello": "World"}

@app.get("/makerecommendation/{pl_ids}/{rec_id}")
def read_item(pl_id: str,rec_id: str, q: Union[str, None] = None):

    return {"pl_id": pl_id, "q": q, "rec_id": rec_id}
