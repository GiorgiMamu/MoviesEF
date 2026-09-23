using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;
using MovieEntity = Movie.Domain.Entities.Movie;

namespace Movie.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MovieDbContext _movieDbContext;

    public MovieRepository(MovieDbContext movieDbContext)
    {
        _movieDbContext = movieDbContext;
    }

    public async Task<ICollection<MovieEntity>> GetAllMoviesAsync()
    {
        return await _movieDbContext.Movies
            .Include(m => m.Studio)
            .ToListAsync();
    }

    public async Task<MovieEntity?> GetMovieByIdAsync(int id)
    {
        return await _movieDbContext.Movies
            .Include(m => m.Studio)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddMovieAsync(MovieEntity movie)
    {
        await _movieDbContext.Movies.AddAsync(movie);
    }

    public async Task UpdateMovieAsync(int id, MovieEntity movie)
    {
        var existingMovie = await _movieDbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null)
        {
            throw new ArgumentException("Movie not found");
        }

        existingMovie.Title = movie.Title;
        existingMovie.ReleaseYear = movie.ReleaseYear;
        existingMovie.StudioId = movie.StudioId;
    }

    public async Task DeleteMovieAsync(int id)
    {
        var existingMovie = await _movieDbContext.Movies
            .FirstOrDefaultAsync(m => m.Id == id);

        if (existingMovie == null)
        {
            throw new ArgumentException("Movie not found");
        }

        _movieDbContext.Movies.Remove(existingMovie);
    }

    public async Task<ICollection<MovieEntity>> SearchMoviesByStudioAsync(
        int year,
        string studioName,
        int minimumActorCount)
    {
        return await _movieDbContext.Movies
            .Include(m => m.Studio)
            .Include(m => m.Actors)
            .Where(m => m.ReleaseYear >= year
                        && m.Studio.Name == studioName
                        && m.Actors.Count >= minimumActorCount)
            .OrderByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<ICollection<MovieEntity>> SearchMoviesByCountryAsync(
        string countryName,
        int minimumYear,
        int maximumActorCount)
    {
        return await _movieDbContext.Movies
            .Include(m => m.Studio)
            .Include(m => m.Actors)
            .Where(m => m.Studio.Country.Name == countryName
                        && m.ReleaseYear >= minimumYear
                        && m.Actors.Count <= maximumActorCount)
            .OrderBy(m => m.Actors.Count)
            .ThenByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<ICollection<MovieEntity>> SearchMoviesAdvancedAsync(
        int fromYear,
        int toYear,
        string countryName,
        string titleText,
        int minimumActorCount)
    {
        return await _movieDbContext.Movies
            .Include(m => m.Studio)
            .Include(m => m.Actors)
            .Where(m => m.ReleaseYear >= fromYear
                        && m.ReleaseYear <= toYear
                        && m.Studio.Country.Name == countryName
                        && m.Title.Contains(titleText)
                        && m.Actors.Count >= minimumActorCount)
            .OrderByDescending(m => m.Actors.Count)
            .ThenByDescending(m => m.ReleaseYear)
            .ThenBy(m => m.Studio.Name)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }
}