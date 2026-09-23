using Microsoft.Extensions.DependencyInjection;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;
using Movie.Infrastructure.Repositories;
using Movie.Service.Implementations;
using Movie.Service.Interfaces;

namespace Movie.UI;

internal class Program
{
    private static async Task Main()
    {
        var services = new ServiceCollection();

        services.AddDbContext<MovieDbContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IActorRepository, ActorRepository>();
        services.AddScoped<IActorService, ActorService>();

        await using var serviceProvider = services.BuildServiceProvider();
        await using var scope = serviceProvider.CreateAsyncScope();

        var movieService = scope.ServiceProvider.GetRequiredService<IMovieService>();
        var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();

        PrintMovies("All movies", await movieService.GetAllMoviesAsync());

        var actors = await actorService.GetAllActorsAsync();
        Console.WriteLine("Actors");
        foreach (var actor in actors)
        {
            Console.WriteLine($"{actor.FirstName} {actor.LastName} - {string.Join(", ", actor.MovieTitles)}");
        }
        Console.WriteLine();

        PrintMovies(
            "Task 1: Warner Bros, from 1990, at least 1 actor",
            await movieService.SearchMoviesByStudioAsync(1990, "Warner Bros", 1));

        PrintMovies(
            "Task 2: USA, from 1990, at most 5 actors",
            await movieService.SearchMoviesByCountryAsync("USA", 1990, 5));

        PrintMovies(
            "Task 3: USA, 1990-2000, title contains 'Home', at least 1 actor",
            await movieService.SearchMoviesAdvancedAsync(1990, 2000, "USA", "Home", 1));
    }

    private static void PrintMovies(string header, ICollection<MovieDTO> movies)
    {
        Console.WriteLine(header);
        foreach (var movie in movies)
        {
            Console.WriteLine(movie);
        }
        Console.WriteLine();
    }
}