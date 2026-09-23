using System;
using System.Collections.Generic;
using Movie.Domain.Entities;
using System.Text;

using MovieEntity = Movie.Domain.Entities.Movie;

namespace Movie.Domain.Interfaces;

public interface IMovieRepository
{
    Task<ICollection<MovieEntity>> GetAllMoviesAsync();
    Task<MovieEntity?> GetMovieByIdAsync(int id);
    Task AddMovieAsync(MovieEntity movie);
    Task UpdateMovieAsync(int id, MovieEntity movie);
    Task DeleteMovieAsync(int id);

    Task<ICollection<MovieEntity>> SearchMoviesByStudioAsync(
        int year,
        string studioName,
        int minimumActorCount);

    Task<ICollection<MovieEntity>> SearchMoviesByCountryAsync(
        string countryName,
        int minimumYear,
        int maximumActorCount);

    Task<ICollection<MovieEntity>> SearchMoviesAdvancedAsync(
        int fromYear,
        int toYear,
        string countryName,
        string titleText,
        int minimumActorCount);
}