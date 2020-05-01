## Movies Api

----

### Project Purpose

Simple web api application which allows creating, reading, updating and deleting of movies.  


#### Endpoints

  - HttpGet["api/movies/{id}"] GetMovieDetailsAsync

  - HttpGet["api/movies"] GetMovieGridAsync

  - HttpPut["api/movies/{id}"] UpdateMovieAsync

  - HttpPost["api/movies/"] CreateMovieAsync

  - HttpDelete["api/movies/{id}"] DeleteMovieAsync


#### Technologies and other important information

The General technologies used in the development of Movies Api are the following:
  - ASP.NET Core 3.1
  - Entity Framework Core to access database (used in-memory database)
  - Used object caching for movie details response model
  - Used GitHub advantages to work with branches during development ot features
