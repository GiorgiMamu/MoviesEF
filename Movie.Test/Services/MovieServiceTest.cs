using Moq;
using Movie.Domain.DTOs;
using Movie.Domain.Interfaces;
using Movie.Service.Implementations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Movie.Test.Services
{
    public class MovieServiceTest
    {

        [Fact]

        public async Task AddMovieAsync_ValidMovie_CallsRepo()
        {
            //Arrange
            var repoMock = new Mock<IMovieRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            repoMock.Setup(r => r.AddMovieAsync(It.IsAny<Movie.Domain.Entities.Movie>()))
                .Returns(Task.CompletedTask);

            var movieService = new MovieService(repoMock.Object, unitOfWorkMock.Object);


            var dto = new CreateMovieDTO()
            {
                Title = "Test Movie",
                ReleaseYear = 2000,
                StudioId = 1

            };


            //Act
            await movieService.AddMovieAsync(dto);



            //Assert


            repoMock.Verify(repo => repo.AddMovieAsync(It.IsAny<Movie.Domain.Entities.Movie>()),
                Times.Once());


        }


        [Fact]

        public async Task AddMovieAsync_NotValidMovie_CallsRepoZero()
        {
            //Arrange
            var repoMock = new Mock<IMovieRepository>();
            var unitOfWorkMock = new Mock<IUnitOfWork>();

            repoMock.Setup(r => r.AddMovieAsync(It.IsAny<Movie.Domain.Entities.Movie>()))
                .Returns(Task.CompletedTask);

            var movieService = new MovieService(repoMock.Object, unitOfWorkMock.Object);


            var dto = new CreateMovieDTO()
            {
                Title = null,
                ReleaseYear = 2000,
                StudioId = 1

            };


            //Act
            //await movieService.AddMovieAsync(dto);

            var res = await Assert.ThrowsAsync<ArgumentException>(() => movieService.AddMovieAsync(dto));

            //Assert

            // Equal არ მუშაობდა იმიტომ რომ მესიჯი არის  "Movie title cannot be null or empty. (Parameter 'Title')
            //Contains  მუშაობს

            Assert.Contains("Movie title cannot be null or empty", res.Message);  // TODo exception message should be diff

            repoMock.Verify(repo => repo.AddMovieAsync(It.IsAny<Movie.Domain.Entities.Movie>()),
                Times.Never());


        }

    }
}