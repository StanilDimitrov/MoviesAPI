using MoviesApi.Dal.Data.Models;
using System;

namespace MoviesApi.Dal
{
    public static class DataSeedHelper
    {
        public static Movie TestMovie1()
        {
            return new Movie
            {
                Id = 1,
                Title = "TestMovie1",
                Description = "TestDescription1",
                ReleaseDate = DateTime.Now
            };
        }

        public static Movie TestMovie2()
        {
            return new Movie
            {
                Id = 2,
                Title = "TestMovie2",
                Description = "TestDescription2",
                ReleaseDate = DateTime.Now
            };
        }

        public static Movie TestMovie3()
        {
            return new Movie
            {
                Id = 3,
                Title = "TestMovie3",
                Description = "TestDescription3",
                ReleaseDate = DateTime.Now
            };
        }

        public static Movie TestMovie4()
        {
            return new Movie
            {
                Id = 4,
                Title = "TestMovie4",
                Description = "TestDescription4",
                ReleaseDate = DateTime.Now
            };
        }
    }
}
