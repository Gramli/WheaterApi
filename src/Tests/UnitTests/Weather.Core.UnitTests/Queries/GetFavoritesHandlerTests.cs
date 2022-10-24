﻿using FluentResults;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Weather.Core.Abstractions;
using Weather.Core.Queries;
using Weather.Domain.Dtos;
using Weather.Domain.Http;

namespace Weather.Core.UnitTests.Queries
{
    public class GetFavoritesHandlerTests
    {
        private readonly Mock<IWeatherQueriesRepository> _weatherRepositoryMock;
        private readonly Mock<IWeatherService> _weatherServiceMock;
        private readonly Mock<ILogger<IGetFavoritesHandler>> _loggerMock;

        private readonly IGetFavoritesHandler _uut;
        public GetFavoritesHandlerTests() 
        { 
            _weatherRepositoryMock = new Mock<IWeatherQueriesRepository>();
            _weatherServiceMock = new Mock<IWeatherService>();
            _loggerMock = new Mock<ILogger<IGetFavoritesHandler>>();

            _uut = new GetFavoritesHandler(_weatherRepositoryMock.Object, _weatherServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetFavorites_Failed()
        {
            //Arrange
            var failMessage = "Some fail message";

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>())).ReturnsAsync(Result.Fail(failMessage));

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(failMessage, result.Errors.Single());
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFavorites_Empty()
        {
            //Arrange

            _weatherRepositoryMock.Setup(x => x.GetFavorites(It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result.Ok<IEnumerable<LocationDto>>(new List<LocationDto>()));

            //Act
            var result = await _uut.HandleAsync(EmptyRequest.Instance, CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
            Assert.Empty(result.Errors);
            _weatherRepositoryMock.Verify(x => x.GetFavorites(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Result_Empty()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public async Task One_Of_GetCurrentWeather_Failed()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact]
        public async Task Success()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}