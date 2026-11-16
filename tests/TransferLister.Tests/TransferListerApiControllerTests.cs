using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Transfer_ListerAPI.Controllers;
using Transfer_ListerAPI.Models;
using Transfer_ListerAPI.Models.DTOs;
using Transfer_ListerAPI.Repository.IRepository;

namespace TransferLister.Tests;

public class TransferListerApiControllerTests
{
    private readonly Mock<IPlayerRepository> _playerRepository = new();
    private readonly Mock<IPositionRepository> _positionRepository = new();
    private readonly Mock<IMapper> _mapper = new();

    private TransferListerAPIController CreateController()
    {
        return new TransferListerAPIController(
            _playerRepository.Object,
            _positionRepository.Object,
            _mapper.Object);
    }

    [Fact]
    public async Task GetPlayer_WithZeroId_ReturnsBadRequest()
    {
        var controller = CreateController();

        var result = await controller.GetPlayer(0);

        Assert.IsType<BadRequestResult>(result.Result);
    }

    [Fact]
    public async Task GetPlayer_WhenPlayerDoesNotExist_ReturnsNotFound()
    {
        _playerRepository
            .Setup(repository => repository.GetSingleAsync(
                It.IsAny<Expression<Func<Player, bool>>>(),
                "Positions"))
            .ReturnsAsync((Player)null!);

        var controller = CreateController();

        var result = await controller.GetPlayer(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetPlayers_WhenNoPlayersExist_ReturnsNoContent()
    {
        _playerRepository
            .Setup(repository => repository.GetAllAsync(null, "Positions"))
            .ReturnsAsync(new List<Player>());

        var controller = CreateController();

        var result = await controller.GetPlayers();

        Assert.IsType<NoContentResult>(result.Result);
    }

    [Fact]
    public async Task CreatePlayer_WithNegativePrice_ReturnsBadRequest()
    {
        var dto = new CreatePlayerDTO
        {
            Price = -1,
            PositionIds = new List<string> { "LW" }
        };

        var controller = CreateController();

        var result = await controller.CreatePlayer(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task CreatePlayer_WithoutPositions_ReturnsBadRequest()
    {
        var dto = new CreatePlayerDTO
        {
            Price = 10,
            PositionIds = new List<string>()
        };

        var controller = CreateController();

        var result = await controller.CreatePlayer(dto);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}