using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;
using Movie.Service.Interfaces;
using MovieEntity = Movie.Domain.Entities.Movie;

namespace Movie.Service.Implementations;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MovieService(IMovieRepository movieRepository, IUnitOfWork unitOfWork)
    {
        _movieRepository = movieRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<MovieDTO>> GetAllMoviesAsync()
    {
        var movies = await _movieRepository.GetAllMoviesAsync();

        return movies.Select(MapToDto).ToList();
    }

    public async Task<MovieDTO> GetMovieByIdAsync(int id)
    {
        ValidateId(id);

        var movie = await _movieRepository.GetMovieByIdAsync(id);

        if (movie == null)
        {
            throw new ArgumentException("Movie not found.", nameof(id));
        }

        return MapToDto(movie);
    }

    public async Task AddMovieAsync(CreateMovieDTO movieDto)
    {
        ArgumentNullException.ThrowIfNull(movieDto);
        ValidateMovie(movieDto.Title, movieDto.ReleaseYear, movieDto.StudioId);

        var movie = new MovieEntity
        {
            Title = movieDto.Title,
            ReleaseYear = movieDto.ReleaseYear,
            StudioId = movieDto.StudioId
        };

        await _movieRepository.AddMovieAsync(movie);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateMovieAsync(int id, UpdateMovieDTO movieDto)
    {
        ValidateId(id);
        ArgumentNullException.ThrowIfNull(movieDto);
        ValidateMovie(movieDto.Title, movieDto.ReleaseYear, movieDto.StudioId);

        var movie = new MovieEntity
        {
            Title = movieDto.Title,
            ReleaseYear = movieDto.ReleaseYear,
            StudioId = movieDto.StudioId
        };

        await _movieRepository.UpdateMovieAsync(id, movie);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteMovieAsync(int id)
    {
        ValidateId(id);

        await _movieRepository.DeleteMovieAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<ICollection<MovieDTO>> SearchMoviesByStudioAsync(
        int year,
        string studioName,
        int minimumActorCount)
    {
        if (year < 0)
        {
            throw new ArgumentException("Year cannot be negative.", nameof(year));
        }
        if (string.IsNullOrWhiteSpace(studioName))
        {
            throw new ArgumentException("Studio name cannot be null or empty.", nameof(studioName));
        }
        if (minimumActorCount < 0)
        {
            throw new ArgumentException("Minimum actor count cannot be negative.", nameof(minimumActorCount));
        }

        var movies = await _movieRepository.SearchMoviesByStudioAsync(year, studioName, minimumActorCount);

        return movies.Select(MapToDto).ToList();
    }

    public async Task<ICollection<MovieDTO>> SearchMoviesByCountryAsync(
        string countryName,
        int minimumYear,
        int maximumActorCount)
    {
        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new ArgumentException("Country name cannot be null or empty.", nameof(countryName));
        }
        if (minimumYear < 0)
        {
            throw new ArgumentException("Minimum year cannot be negative.", nameof(minimumYear));
        }
        if (maximumActorCount < 0)
        {
            throw new ArgumentException("Maximum actor count cannot be negative.", nameof(maximumActorCount));
        }

        var movies = await _movieRepository.SearchMoviesByCountryAsync(countryName, minimumYear, maximumActorCount);

        return movies.Select(MapToDto).ToList();
    }

    public async Task<ICollection<MovieDTO>> SearchMoviesAdvancedAsync(
        int fromYear,
        int toYear,
        string countryName,
        string titleText,
        int minimumActorCount)
    {
        if (fromYear < 0)
        {
            throw new ArgumentException("From year cannot be negative.", nameof(fromYear));
        }
        if (toYear < fromYear)
        {
            throw new ArgumentException("To year cannot be less than from year.", nameof(toYear));
        }
        if (string.IsNullOrWhiteSpace(countryName))
        {
            throw new ArgumentException("Country name cannot be null or empty.", nameof(countryName));
        }
        if (titleText == null)
        {
            throw new ArgumentNullException(nameof(titleText));
        }
        if (minimumActorCount < 0)
        {
            throw new ArgumentException("Minimum actor count cannot be negative.", nameof(minimumActorCount));
        }

        var movies = await _movieRepository.SearchMoviesAdvancedAsync(
            fromYear,
            toYear,
            countryName,
            titleText,
            minimumActorCount);

        return movies.Select(MapToDto).ToList();
    }

    private static MovieDTO MapToDto(MovieEntity movie)
    {
        return new MovieDTO
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseYear = movie.ReleaseYear,
            StudioName = movie.Studio.Name
        };
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Movie ID must be a positive integer.", nameof(id));
        }
    }

    private static void ValidateMovie(string title, int releaseYear, int studioId)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Movie title cannot be null or empty.", nameof(title));
        }
        if (releaseYear < 0)
        {
            throw new ArgumentException("Movie release year cannot be negative.", nameof(releaseYear));
        }
        if (releaseYear > DateTime.Now.Year)
        {
            throw new ArgumentException("Movie release year cannot be from future.", nameof(releaseYear));
        }
        if (studioId <= 0)
        {
            throw new ArgumentException("Movie studio ID must be a positive integer.", nameof(studioId));
        }
    }
}