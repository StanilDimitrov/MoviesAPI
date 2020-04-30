## Movies Api

----

### Project Purpose

Simple web api application allows creating, reading, updating, deleting of movies.  


#### Endpoints

  - HttpGet["api/movies/{id}"] GetMovieDetailsAsync

  - HttpGet["api/movies"] GetMovieGridAsync

  - HttpPut["api/movies/{id}"] UpdateMovieAsync

  - HttpPost["api/movies/"] CreateMovieAsync

  - HttpDelete["api/movies/{id}"] DeleteMovieAsync

The database used in the project is in-memory. At the same time the endpoints are implemented as async methods
which provides the opportunity to easily replace it with а relational or non relational database.


#### Technologies and other important information

The General technologies used in the development of Movies Api are the following:
  - ASP.NET Core 3.1
  - Entity Framework Core to access database (used in-memory database)
  - Used object caching for movie details response model
  - Used GitHub advantages to work with branches during development ot features
