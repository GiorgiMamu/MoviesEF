using Movie.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Movie.Domain.Interfaces;
using Movie.Domain.DTOs;


namespace Movie.Service.Implementations
{
    public class MovieService : IMovieService
    {

        private readonly IMovieRepository _movieRepository;


        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<ICollection<MovieDTO>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllMoviesAsync();

            var movieDto = movies.Select(m => new MovieDTO
            {
                Id = m.Id,
                Title = m.Title,
                ReleaseYear = m.ReleaseYear,
                StudioName = m.Studio?.Name,
            }).ToList();

            return movieDto;
        }

        public async Task<MovieDTO?> GetMovieByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Movie ID must be a positive integer.", nameof(id));
            }
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return null;
            }
            return new MovieDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseYear,
                StudioName = movie.Studio?.Name,
            };
        }


        public async Task AddMovieAsync(CreateMovieDTO movieDto)
        {
            if(movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto));
            }
            if(string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be null or whitespace.", nameof(movieDto.Title));
            }
            if(movieDto.ReleaseYear <= 0)
            {
                throw new ArgumentException("Release year must be a positive integer.", nameof(movieDto.ReleaseYear));
            }
            if(movieDto.StudioId <= 0)
            {
                throw new ArgumentException("Studio ID must be a positive integer.", nameof(movieDto.StudioId));
            }
            if (movieDto.ReleaseYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Release year must be a valid year.", nameof(movieDto.ReleaseYear));
            }


            var movie = new Domain.Entities.Movie
            {
                Title = movieDto.Title,
                ReleaseYear = movieDto.ReleaseYear,
                StudioId = movieDto.StudioId
            };

            await _movieRepository.AddMovieAsync(movie);

        }
        public async Task UpdateMovieAsync(UpdateMovieDTO movieDto)
        {
            if (movieDto == null)
            {
                throw new ArgumentNullException(nameof(movieDto));
            }
            if (movieDto.Id <= 0)
            {
                throw new ArgumentException("Movie ID must be a positive integer.", nameof(movieDto.Id));
            }
            if (string.IsNullOrWhiteSpace(movieDto.Title))
            {
                throw new ArgumentException("Movie title cannot be null or whitespace.", nameof(movieDto.Title));
            }
            if (movieDto.ReleaseYear <= 0)
            {
                throw new ArgumentException("Release year must be a positive integer.", nameof(movieDto.ReleaseYear));
            }
            if (movieDto.StudioId <= 0)
            {
                throw new ArgumentException("Studio ID must be a positive integer.", nameof(movieDto.StudioId));
            }
            if (movieDto.ReleaseYear > DateTime.Now.Year)
            {
                throw new ArgumentException("Release year must be a valid year.", nameof(movieDto.ReleaseYear));
            }

            var existingMovie = await _movieRepository.GetMovieByIdAsync(movieDto.Id);
            if (existingMovie == null)
            {
                throw new KeyNotFoundException($"Movie with ID {movieDto.Id} was not found.");
            }

            existingMovie.Title = movieDto.Title;
            existingMovie.ReleaseYear = movieDto.ReleaseYear;
            existingMovie.StudioId = movieDto.StudioId;

            await _movieRepository.UpdateMovieAsync(existingMovie);
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Movie ID must be a positive integer.", nameof(id));
            }

            return await _movieRepository.DeleteMovieAsync(id);
        }
    }
}
