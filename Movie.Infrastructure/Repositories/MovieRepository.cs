using Microsoft.EntityFrameworkCore;
using Movie.Domain.Interfaces;
using Movie.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {

        private readonly MovieDbContext _movieDbContext;
        public MovieRepository(MovieDbContext movieDbContext)
        {
            _movieDbContext = movieDbContext;
        }


        public async Task AddMovieAsync(Domain.Entities.Movie movie)
        {
            await _movieDbContext.Movies.AddAsync(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<ICollection<Domain.Entities.Movie>> GetAllMoviesAsync()
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .ToListAsync();
        }
        public async Task<Domain.Entities.Movie?> GetMovieByIdAsync(int id)
        {
            return await _movieDbContext.Movies
                .Include(m => m.Studio)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task UpdateMovieAsync(Domain.Entities.Movie movie)
        {
            _movieDbContext.Movies.Update(movie);
            await _movieDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieDbContext.Movies.FindAsync(id);

            if (movie == null)
            {
                return false;
            }

            _movieDbContext.Movies.Remove(movie);
            await _movieDbContext.SaveChangesAsync();
            return true;
        }
    }
}