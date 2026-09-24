using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Movie.Domain.Entities;
using Movie.Infrastructure.Data;
using Movie.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Test.Repositories
{
    public class MovieRepositoryTest
    {

        private MovieDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<MovieDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new MovieDbContext(options);
        }



        [Fact]
        public async Task GetAllMoviesAsync_ReturnsAllMovies()
        {
            //Arrange
            var context = CreateContext();

            var country = new Country { Id = 1, Name = "USA" };

            var studio = new Studio { Id = 1, Name = "Warner Bros", CountryId = country.Id, Country = country };

            context.Countries.Add(country);
            context.Studios.Add(studio);

            var movie1 = new Movie.Domain.Entities.Movie { Id = 1, Title = "Inception", ReleaseYear = 2010, StudioId = studio.Id };
            var movie2 = new Movie.Domain.Entities.Movie { Id = 2, Title = "Inception2", ReleaseYear = 2008, StudioId = studio.Id };

            context.Movies.AddRange(movie1, movie2);
            await context.SaveChangesAsync();

            var sut = new MovieRepository(context);


            //Act
            var result = await sut.GetAllMoviesAsync();




            // Assert

            Assert.Equal(2, result.Count);


            Assert.Contains(result, m => m.Title == "Inception" && m.ReleaseYear == 2010);

            var singleFilm = result.First(m => m.Title == "Inception");
            Assert.Equal(1, singleFilm.Id);

            Assert.NotNull(singleFilm.Studio);
            Assert.Equal(1, singleFilm.Studio.Id);
            Assert.Equal("Warner Bros", singleFilm.Studio.Name);
            Assert.Equal(1, singleFilm.Studio.CountryId);
            Assert.Equal("USA", singleFilm.Studio.Country.Name);


        }


        [Fact]
        public async Task AddMovieAsync_AddsMovie()
        {

            var token =   //TestContext.Current.CancellationToken;    XUNIT V3 
                new CancellationToken();  /// ამ ვერსიაზე ასე ვტოვებთ

            //Arrange
            var context = CreateContext();

            var repository = new MovieRepository(context);


            var movie = new Movie.Domain.Entities.Movie
            { Title = "Inception", ReleaseYear = 2010, StudioId = 1 };


            //Act
            await repository.AddMovieAsync(movie);
            await context.SaveChangesAsync(token);



            // Assert
            var result = await context.Movies.FirstOrDefaultAsync(token);
            Assert.NotNull(result);
            //Assert.Equal(1, result.Id);
            Assert.Equal("Inception", result.Title);
            Assert.Equal(2010, result.ReleaseYear);
            Assert.Equal(1, result.StudioId);


        }


    }
}