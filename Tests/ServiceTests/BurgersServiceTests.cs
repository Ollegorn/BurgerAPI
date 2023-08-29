using AutoFixture;
using Moq;
using RepositoryContracts;
using ServiceContracts.Interfaces;
using Xunit.Abstractions;
using Serilog;
using Services;
using Microsoft.Extensions.Logging;
using Entities;
using ServiceContracts.BurgerDto;
using FluentAssertions;
using System.Security.Permissions;

namespace Tests.ServiceTests
{
    public class BurgersServiceTests
    {
        private readonly IBurgerGetterService _getterService;
        private readonly IBurgerAdderService _adderService;
        private readonly IBurgerDeleterService _deleterService;
        private readonly IBurgerUpdaterService _updaterService;

        private readonly Mock<IBurgerRepository> _repositoryMock;
        private readonly IBurgerRepository _repository;

        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        public BurgersServiceTests(ITestOutputHelper testOutputHelper)
        {
            _fixture = new Fixture();
            _repositoryMock = new Mock<IBurgerRepository>();
            _repository = _repositoryMock.Object;

            var loggerMock = new Mock<ILogger<BurgerGetterService>>();

            _getterService = new BurgerGetterService(_repository, loggerMock.Object);
            _adderService = new BurgerAdderService(_repository, loggerMock.Object);

        }

        #region GetAllBurgers

        [Fact]
        public async Task GetAllBurgers_ToBeEmptyList()
        {
            //Arrange
            var burgers = new List<Burger>();
            _repositoryMock.Setup(repository => repository.GetAllBurgers()).ReturnsAsync(burgers);

            //Act
            List<BurgerResponseDto> burgers_from_get = await _getterService.GetAllBurgers();

            //Assert
            burgers_from_get.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllBurgers_WithAFewBurgers_ToBeSuccessfull()
        {
            //Arrange
            List<Burger> burgers = new List<Burger> {
            _fixture.Build<Burger>().With(burg => burg.Price, 15).Create(), //using custom price because autofixure breaks otherwise
            _fixture.Build<Burger>().With(burg => burg.Price, 10).Create(),
            _fixture.Build<Burger>().With(burg => burg.Price, 12).Create(),
            _fixture.Build<Burger>().With(burg => burg.Price, 11).Create()
            };

            List<BurgerResponseDto> burgers_responseList_expected = burgers.Select(temp => temp.ToBurgerResponseDto()).ToList();
            _repositoryMock.Setup(repository => repository.GetAllBurgers()).ReturnsAsync(burgers);

            //Act
            List<BurgerResponseDto> burgers_list_from_get = await _getterService.GetAllBurgers();

            //Assert
            burgers_list_from_get.Should().BeEquivalentTo(burgers_responseList_expected);

        }

        [Fact]
        public async Task GetAllBurges_ThrowsException_ReturnsNull()
        {
            //Arrange
            _repositoryMock.Setup(repository => repository.GetAllBurgers()).ThrowsAsync(new Exception("Test"));

            //Act
            var burgersResponse = await _getterService.GetAllBurgers();

            //Assert
            burgersResponse.Should().BeNull();

        }

        #endregion

        #region GetBurgerById

        [Fact]
        public async Task GetBurgersById_WithNullId_ReturnsNull()
        {
            //Arrange
            int? id = null;

            //Act
            BurgerResponseDto? burgerResponse = await _getterService.GetBurgerById(id);

            //assert
            burgerResponse?.Should().BeNull();
        }

        [Fact]
        public async Task GetBurgerById_WithValidId_ReturnsBurgerResponseDto()
        {
            //Arrange
            Burger burger = _fixture.Build<Burger>().With(burg => burg.Price, 11).Create();

            BurgerResponseDto burgerResponse_expected = burger.ToBurgerResponseDto();
            _repositoryMock.Setup(repository => repository.GetBurgerById(It.IsAny<int>())).ReturnsAsync(burger);

            //Act
            BurgerResponseDto burgerResponse = await _getterService.GetBurgerById(burger.Id);

            //Assert
            burgerResponse.Should().Be(burgerResponse_expected);
        }

        [Fact]
        public async Task GetBurgerById_WithInvalidId_ReturnsNull()
        {
            //Arrange
            int nonExistentId = int.MaxValue;

            _repositoryMock.Setup(repository => repository.GetBurgerById(nonExistentId)).ReturnsAsync((Burger)null);

            //Act
            var burgerResponse = await _getterService.GetBurgerById(nonExistentId);

            //Assert
            burgerResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetBurgerById_ThrowsException_ReturnsNull()
        {
            //Arrange
            int id = 3;

            _repositoryMock.Setup(repository => repository.GetBurgerById(id)).ThrowsAsync(new Exception("Test"));

            //Act
            var burgerResponse = await _getterService.GetBurgerById(id); 
            
            //Assert
            burgerResponse.Should().BeNull();
        }

        #endregion

        #region GetBurgersByIds

        [Fact]
        public async Task GetBurgersByIds_WithNullIds_ReturnsNull()
        {
            //Arrange
            List<int>? ids = null;

            //Act
            var burgersResponse = await _getterService.GetBurgersByIds(ids);

            //Assert
            burgersResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetBurgersByIds_WithEmptyIds_ReturnsEmptyList()
        {
            //Arrange
            List<int> ids = new List<int>();
            List<int> expectedResponse = new List<int>();

            _repositoryMock.Setup(repo => repo.GetBurgersByIds(ids)).ReturnsAsync(new List<Burger>());

            //Act
            var burgersResponse = await _getterService.GetBurgersByIds(ids);

            //Assert
            burgersResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetBurgersByIds_WithValidIds_ReturnsList()
        {
            //Arrange
            var ids = _fixture.CreateMany<int>().ToList();
            var expectedResponse = _fixture.Build<Burger>().With(burg => burg.Price, 10).CreateMany().ToList();

            _repositoryMock.Setup(repo => repo.GetBurgersByIds(ids)).ReturnsAsync(expectedResponse);

            var expectedBurgerDtoResponses = expectedResponse.Select(burg => burg.ToBurgerResponseDto()).ToList();

            //Act
            var burgerResponses = await _getterService.GetBurgersByIds(ids);

            //Assert
            burgerResponses.Should().BeEquivalentTo(expectedBurgerDtoResponses);

        }

        [Fact]
        public async Task GetBurgersByIds_ThrowsException_ReturnsNull()
        {
            // Arrange
            var ids = new List<int>(); 

            _repositoryMock.Setup(repo => repo.GetBurgersByIds(ids)).ThrowsAsync(new Exception("Test"));

            // Act
            var result = await _getterService.GetBurgersByIds(ids);

            // Assert
            result.Should().BeNull();
        }

        #endregion

        #region AddBurger
        [Fact]
        public async Task AddBurger_Null_ToBeArgumentNullException()
        {
            // Arrange
            BurgerAddRequestDto? nullRequest = null; 
            Burger burger = _fixture.Build<Burger>().With(burg => burg.Price, 10).Create();

            _repositoryMock.Setup(repo => repo.AddBurger(It.IsAny<Burger>())).ReturnsAsync(burger);

            //Act
            var addBurgerAction = async () => await _adderService.AddBurger(nullRequest);

            //Assert
            await addBurgerAction.Should().ThrowAsync<ArgumentNullException>();

        }

        [Fact]
        public async Task AddBurger_Successfull_AddsNewBurger()
        {
            //Arrange
            var newBurger = _fixture.Build<BurgerAddRequestDto>().With(burg => burg.BurgerPrice, 10).Create();
            var expectedBurger = _fixture.Build<Burger>().With(burg => burg.Price, 11).Create();

            _repositoryMock.Setup(repo => repo.AddBurger(It.IsAny<Burger>()))
                           .ReturnsAsync(expectedBurger);


            //Act
            var addedBurger = await _adderService.AddBurger(newBurger);

            //Assert
            addedBurger.Should().BeEquivalentTo(expectedBurger);
            _repositoryMock.Verify(repo => repo.AddBurger(It.IsAny<Burger>()), Times.Once);
        }

        [Fact]
        public async Task AddBurger_ThrowsException_ReturnsNull()
        {
            // Arrange
            BurgerAddRequestDto burgerRequest = _fixture.Build<BurgerAddRequestDto>().With(burg => burg.BurgerPrice, 10).Create();

            _repositoryMock.Setup(repository => repository.AddBurger(It.IsAny<Burger>())).ThrowsAsync(new Exception("Test"));

            // Act & Assert
            Func<Task> addBurgerAction = async () => await _adderService.AddBurger(burgerRequest);
            await addBurgerAction.Should().ThrowAsync<Exception>().WithMessage("Test");
        }






        #endregion
    }
}
