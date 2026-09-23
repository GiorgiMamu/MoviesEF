using Movie.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

using Movie.Domain.Entities;

namespace Movie.Domain.Interfaces;

public interface IActorRepository
{
    Task<ICollection<Actor>> GetAllActorsAsync();
    Task<Actor?> GetActorByIdAsync(int id);
    Task AddActorAsync(Actor actor);
    Task UpdateActorAsync(int id, Actor actor);
    Task DeleteActorAsync(int id);
    Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds);
}