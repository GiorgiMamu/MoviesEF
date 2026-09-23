using Microsoft.EntityFrameworkCore;
using Movie.Domain.Entities;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;

namespace Movie.Infrastructure.Repositories;

public class ActorRepository : IActorRepository
{
    private readonly MovieDbContext _movieDbContext;

    public ActorRepository(MovieDbContext movieDbContext)
    {
        _movieDbContext = movieDbContext;
    }

    public async Task<ICollection<Actor>> GetAllActorsAsync()
    {
        return await _movieDbContext.Actors
            .Include(a => a.Movies)
            .ToListAsync();
    }

    public async Task<Actor?> GetActorByIdAsync(int id)
    {
        return await _movieDbContext.Actors
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddActorAsync(Actor actor)
    {
        await _movieDbContext.Actors.AddAsync(actor);
    }

    public async Task UpdateActorAsync(int id, Actor actor)
    {
        var existingActor = await _movieDbContext.Actors
            .FirstOrDefaultAsync(a => a.Id == id);

        if (existingActor == null)
        {
            throw new ArgumentException("Actor not found");
        }

        existingActor.FirstName = actor.FirstName;
        existingActor.LastName = actor.LastName;
    }

    public async Task DeleteActorAsync(int id)
    {
        var existingActor = await _movieDbContext.Actors
            .FirstOrDefaultAsync(a => a.Id == id);

        if (existingActor == null)
        {
            throw new ArgumentException("Actor not found");
        }

        _movieDbContext.Actors.Remove(existingActor);
    }

    public async Task UpdateActorMoviesAsync(int actorId, ICollection<int> movieIds)
    {
        var actor = await _movieDbContext.Actors
            .Include(a => a.Movies)
            .FirstOrDefaultAsync(a => a.Id == actorId);

        if (actor == null)
        {
            throw new ArgumentException("Actor not found");
        }

        var distinctMovieIds = movieIds.Distinct().ToList();

        var movies = await _movieDbContext.Movies
            .Where(m => distinctMovieIds.Contains(m.Id))
            .ToListAsync();

        if (movies.Count != distinctMovieIds.Count)
        {
            throw new ArgumentException("One or more movies were not found");
        }

        actor.Movies.Clear();

        foreach (var movie in movies)
        {
            actor.Movies.Add(movie);
        }
    }
}