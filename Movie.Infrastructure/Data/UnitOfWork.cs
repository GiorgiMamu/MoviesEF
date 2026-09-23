using Movie.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly MovieDbContext _context;

        public UnitOfWork(MovieDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
