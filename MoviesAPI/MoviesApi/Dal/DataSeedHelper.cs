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

        public static Movie TestMovie5()
        {
            return new Movie
            {
                Id = 5,
                Title = "TestMovie5",
                Description = "TestDescription5",
                ReleaseDate = DateTime.Now.AddYears(-2)
            };
        }

        public static Movie TestMovie6()
        {
            return new Movie
            {
                Id = 6,
                Title = "TestMovie6",
                Description = "TestDescription6",
                ReleaseDate = DateTime.Now.AddYears(-1)
            };
        }

        public static Movie TestMovie7()
        {
            return new Movie
            {
                Id = 7,
                Title = "TestMovie7",
                Description = "TestDescription7",
                ReleaseDate = DateTime.Now.AddYears(-1)
            };
        }

        public static Movie TestMovie8()
        {
            return new Movie
            {
                Id = 8,
                Title = "TestMovie8",
                Description = "TestDescription8",
                ReleaseDate = DateTime.Now.AddYears(-1)
            };
        }

        public static Movie TestMovie9()
        {
            return new Movie
            {
                Id = 9,
                Title = "TestMovie9",
                Description = "TestDescription9",
                ReleaseDate = DateTime.Now.AddYears(-1)
            };
        }

        public static Movie TestMovie10()
        {
            return new Movie
            {
                Id = 10,
                Title = "TestMovie10",
                Description = "TestDescription10",
                ReleaseDate = DateTime.Now.AddYears(-1)
            };
        }
    }
}
