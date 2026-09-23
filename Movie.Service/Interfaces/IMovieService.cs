using Movie.Domain.DTOs;

namespace Movie.Service.Interfaces;

public interface IMovieService
{
    Task<ICollection<MovieDTO>> GetAllMoviesAsync();
    Task<MovieDTO> GetMovieByIdAsync(int id);
    Task AddMovieAsync(CreateMovieDTO movieDto);
    Task UpdateMovieAsync(int id, UpdateMovieDTO movieDto);
    Task DeleteMovieAsync(int id);

    Task<ICollection<MovieDTO>> SearchMoviesByStudioAsync(
        int year,
        string studioName,
        int minimumActorCount);

    Task<ICollection<MovieDTO>> SearchMoviesByCountryAsync(
        string countryName,
        int minimumYear,
        int maximumActorCount);

    Task<ICollection<MovieDTO>> SearchMoviesAdvancedAsync(
        int fromYear,
        int toYear,
        string countryName,
        string titleText,
        int minimumActorCount);
}