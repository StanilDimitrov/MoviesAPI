## Movies Api

----

### Project Purpose

Simple web api application which allows creating, reading, updating and deleting of movies.  


#### Endpoints

  - HttpPost["api/movies/"] CreateMovieAsync
  
  - HttpPut["api/movies/{id}"] UpdateMovieAsync

  - HttpGet["api/movies"] GetMovieGridAsync
  
  - HttpGet["api/movies/{id}"] GetMovieDetailsAsync

  - HttpDelete["api/movies/{id}"] DeleteMovieAsync


#### Technologies and other important information

The General technologies used in the development of Movies Api are the following:
  - ASP.NET Core 3.1
  - Entity Framework Core to access database (used in-memory database)
  - Used object caching for movie details response model
  - Used GitHub advantages to work with branches during development ot features
