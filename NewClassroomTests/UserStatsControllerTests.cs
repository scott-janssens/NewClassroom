using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewClassroom.Controllers;
using NewClassroom.Models;
using NewClassroom.Services;
using NewClassroom.Wrappers;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;


namespace NewClassroomTests;

[ExcludeFromCodeCoverage]
internal class UserStatsControllerTests
{
    private UserStatsController _controller;
    private Mock<IUserStatsService> _serviceMock;
    private Mock<IHttpClient> _httpClientMock;
    private Mock<ILogger<UserStatsController>> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _serviceMock = new Mock<IUserStatsService>();
        _httpClientMock = new Mock<IHttpClient>();
        _loggerMock = new Mock<ILogger<UserStatsController>>();
        _controller = new UserStatsController(_serviceMock.Object, _httpClientMock.Object, _loggerMock.Object);
    }

    [Test]
    public void Put()
    {
        var input = new RandomUserResults() { Results = new List<User> { TestData.UserFaker.Generate() } };
        var response = _controller.Put(input) as OkObjectResult;

        _serviceMock.Verify(x => x.AddDefaultQueries(), Times.Once);
        _serviceMock.Verify(x => x.GetStatistics(It.IsAny<IEnumerable<User>>()), Times.Once);
        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public void Put_BadInput()
    {
        var nullResponse = _controller.Put(new RandomUserResults()) as BadRequestResult;
        var emptyResponse = _controller.Put(new RandomUserResults() { Results = new List<User>() }) as BadRequestResult;

        Assert.Multiple(() =>
        {
            Assert.That(nullResponse, Is.Not.Null);
            Assert.That(nullResponse!.StatusCode, Is.EqualTo(400));
            Assert.That(emptyResponse, Is.Not.Null);
            Assert.That(emptyResponse!.StatusCode, Is.EqualTo(400));
        });
    }

    [Test]
    public async Task Get()
    {
        var user = TestData.UserFaker.Generate();
        var randomUserResults = new RandomUserResults()
        {
            Results = new List<User> { user }
        };
        var randomUserResultsJson = JsonSerializer.Serialize(randomUserResults, TestData.JsonOptions);
        var dataResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(randomUserResultsJson)
        };

        _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(dataResponse);

        var response = await _controller.Get(12) as OkObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(200));
        });
    }

    [Test]
    public async Task Get_EmptyResponse()
    {
        var randomUserResultsJson = JsonSerializer.Serialize(new RandomUserResults(), TestData.JsonOptions);
        var dataResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(randomUserResultsJson)
        };

        _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(dataResponse);

        var response = await _controller.Get(12) as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(500));
        });
    }
    [Test]
    public async Task Get_NullResponse()
    {
        _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync((HttpResponseMessage?)null);
        var response = await _controller.Get(12) as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(500));
        });
    }

    [Test]
    public async Task Get_FailResponse()
    {
        _httpClientMock.Setup(x => x.GetAsync(It.IsAny<string>()))
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError));
        
        var response = await _controller.Get(12) as ObjectResult;

        Assert.Multiple(() =>
        {
            Assert.That(response, Is.Not.Null);
            Assert.That(response!.StatusCode, Is.EqualTo(500));
        });
    }
}
