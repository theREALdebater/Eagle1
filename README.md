# Eagle1
Submission for Eagle Eye

## To Run

 1. Build using MSBUILD
 2. Open a shell and CD to the output directory
 3. Check that the CSV files are in subdirectory "Data" of the current directory
 4. Execute "./Eagle1.exe" to run the server (Kestrel)
 5. Use e.g. Postman to test

The HTTP port number is: 5000
The HTTPS port number is: 5001

E.g.:

GET http://localhost:5000/metadata/3

GET http://localhost:5000/movies/stats

POST http://localhost:5000/metadata
with the body:
{
    "movieId": 10,
    "title": "Friday 21st",
    "language": "EN",
    "duration": "3:19:01",
    "releaseYear": 2023
}



