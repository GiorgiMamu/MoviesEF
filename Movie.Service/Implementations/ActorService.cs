using Movie.Domain.DTOs;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Service.Interfaces;

namespace Movie.Service.Implementations;

public class ActorService : IActorService
{
    private readonly IActorRepository _actorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActorService(IActorRepository actorRepository, IUnitOfWork unitOfWork)
    {
        _actorRepository = actorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<ActorDTO>> GetAllActorsAsync()
    {
        var actors = await _actorRepository.GetAllActorsAsync();

        return actors.Select(MapToDto).ToList();
    }

    public async Task<ActorDTO> GetActorAsync(int id)
    {
        ValidateId(id);

        var actor = await _actorRepository.GetActorByIdAsync(id);

        if (actor == null)
        {
            throw new ArgumentException("Actor not found.", nameof(id));
        }

        return MapToDto(actor);
    }

    public async Task AddActorAsync(CreateActorDTO actorDto)
    {
        ArgumentNullException.ThrowIfNull(actorDto);
        ValidateName(actorDto.FirstName, nameof(actorDto.FirstName));
        ValidateName(actorDto.LastName, nameof(actorDto.LastName));

        var actor = new Actor
        {
            FirstName = actorDto.FirstName,
            LastName = actorDto.LastName
        };

        await _actorRepository.AddActorAsync(actor);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActorAsync(int id, UpdateActorDTO actorDto)
    {
        ValidateId(id);
        ArgumentNullException.ThrowIfNull(actorDto);
        ValidateName(actorDto.FirstName, nameof(actorDto.FirstName));
        ValidateName(actorDto.LastName, nameof(actorDto.LastName));

        var actor = new Actor
        {
            FirstName = actorDto.FirstName,
            LastName = actorDto.LastName
        };

        await _actorRepository.UpdateActorAsync(id, actor);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeleteActorAsync(int id)
    {
        ValidateId(id);

        await _actorRepository.DeleteActorAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdateActorMoviesAsync(int actorId, UpdateActorMovieDTO updateActorMovieDto)
    {
        ValidateId(actorId);
        ArgumentNullException.ThrowIfNull(updateActorMovieDto);

        await _actorRepository.UpdateActorMoviesAsync(actorId, updateActorMovieDto.MovieIds);
        await _unitOfWork.SaveChangesAsync();
    }

    private static ActorDTO MapToDto(Actor actor)
    {
        return new ActorDTO
        {
            Id = actor.Id,
            FirstName = actor.FirstName,
            LastName = actor.LastName,
            MovieTitles = actor.Movies.Select(m => m.Title).ToList()
        };
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid actor ID.", nameof(id));
        }
    }

    private static void ValidateName(string name, string paramName)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Actor name cannot be null or empty.", paramName);
        }
        if (name.Length > 100)
        {
            throw new ArgumentException("Actor name cannot exceed 100 characters.", paramName);
        }
    }
}