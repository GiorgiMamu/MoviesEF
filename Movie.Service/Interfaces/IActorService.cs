using Movie.Domain.DTOs;

namespace Movie.Service.Interfaces;

public interface IActorService
{
    Task<ICollection<ActorDTO>> GetAllActorsAsync();
    Task<ActorDTO> GetActorAsync(int id);
    Task AddActorAsync(CreateActorDTO actorDto);
    Task UpdateActorAsync(int id, UpdateActorDTO actorDto);
    Task DeleteActorAsync(int id);
    Task UpdateActorMoviesAsync(int actorId, UpdateActorMovieDTO updateActorMovieDto);
}