using Movie.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Service.Interfaces
{
    public interface IMovieService
    {
        Task<ICollection<MovieDTO>> GetAllMoviesAsync();
        Task AddMovieAsync(CreateMovieDTO movieDto);
        Task<MovieDTO?> GetMovieByIdAsync(int id);
        Task UpdateMovieAsync(UpdateMovieDTO movieDto);
        Task<bool> DeleteMovieAsync(int id);

    }
}
