using Movie.Domain.DTOs;
using Movie.Domain.Entities;

using Movie.Service.Implementations;
using Movie.Service.Interfaces;
using Movie.Infrastructure.Repositories;
using Movie.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Movie.Domain.Interfaces;


namespace Movie.UI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            #region without DI container
            //var dbContext = new MovieDbContext();
            //var movieRepository = new MovieRepository(dbContext);
            //var movieService = new MovieService(movieRepository);
            #endregion

            var services = new ServiceCollection();

            services.AddDbContext<MovieDbContext>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieService, MovieService>();

            var serviceProvider = services.BuildServiceProvider();

            var movieService = serviceProvider.GetRequiredService<IMovieService>();

            //var studio = new Studio
            //{
            //    Name = "Warner Bros",
            //    CountryId = 1 // Assuming the country with ID 1 exists
            //};
            //dbContext.Studios.Add(studio);
            //await dbContext.SaveChangesAsync();

            //var createMovieDto = new CreateMovieDTO
            //{
            //    Title = "Home Alone",
            //    ReleaseYear = 1999,
            //    StudioId = 1
            //};
            //await movieService.AddMovieAsync(createMovieDto);
            //await dbContext.SaveChangesAsync();

            //var movies = await movieService.GetAllMoviesAsync();
            //foreach (var movie in movies)
            //{
            //    Console.WriteLine(movie);
            //}

            var movieById = await movieService.GetMovieByIdAsync(3);
            if (movieById != null)
            {
                Console.WriteLine(movieById);
            }
            else
            {
                Console.WriteLine("Movie not found");
            }
        }
    }
}
