using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.BusinessLogic.Services.Extensions;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ImpoBooks.Tests.E2e;

public class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IAuthService> _authServiceMock = new();

    public AuthControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.Development.json", optional: false);
            });
            builder.ConfigureServices(services => { services.AddSingleton(_ => _authServiceMock.Object); });
        }).CreateClient();
    }


    [Fact]
    public async Task Signin_ShouldReturnCreated_WhenCredentialsAreCorrect()
    {
        // Arrange
        var registerRequest = new RegisterUserRequest
        {
            Email = "testuser@example.com",
            FullName = "Test User",
            Password = "StrongPassword123"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/signin", registerRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Signin_ShouldReturnBadRequest_WhenUserAlreadyExists()
    {
        // Arrange
        var registerRequest = new RegisterUserRequest
        {
            Email = "existinguser@example.com",
            FullName = "Test User",
            Password = "Password123"
        };

        _authServiceMock.Setup(s => s.RegisterAsync(It.IsAny<User>()))
            .ReturnsAsync(UserErrors.AlreadyExists);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/signin", registerRequest);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().Contain("User.AlreadyExists");
    }

    [Theory]
    [MemberData(nameof(InvalidRegisterRequests))]
    public async Task Signin_ShouldReturnBadRequest_WhenAnyRequestFieldIsNullOrEmpty(
        string email,
        string fullName,
        string password)
    {
        //Arrange
        RegisterUserRequest request = new()
        {
            Email = email,
            FullName = fullName,
            Password = password,
        };

        // Act
        HttpResponseMessage response = await _client.PostAsJsonAsync("/api/auth/signin", request);
        ProblemDetails problemDetails = await response.GetProblemDetailsAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(problemDetails);
        Assert.NotNull(problemDetails.Extensions["errors"]);
    }


    [Fact]
    public async Task Login_ShouldReturnOk_WhenCredentialsAreValid()
    {
        // Arrange
        var request = new LoginUserRequest
        {
            Email = "test@example.com",
            Password = "Password123"
        };

        _authServiceMock.Setup(service => service.LoginAsync(request.Email, request.Password)).ReturnsAsync("jwt");

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        ProblemDetails problemDetails = await response.GetProblemDetailsAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Null(problemDetails);
    }

    [Theory]
    [MemberData(nameof(InvalidLoginRequests))]
    public async Task Login_ShouldReturnBadRequest_WhenAnyRequestFieldIsNullOrEmpty(string email, string password)
    {
        var request = new LoginUserRequest
        {
            Email = email,
            Password = password,
        };

        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(UserErrors.WrongPassword);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        responseContent.Should().Contain("User.WrongPassword");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenPasswordIsInvalid()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "validuser@example.com",
            Password = "WrongPassword123"
        };

        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(UserErrors.WrongPassword);
        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().Contain("User.WrongPassword");
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenUserDoesNotExist()
    {
        // Arrange
        var loginRequest = new LoginUserRequest
        {
            Email = "invalidemail@example.com",
            Password = "strongPassword123"
        };

        _authServiceMock.Setup(s => s.LoginAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(UserErrors.NotFoundByEmail);

        // Act
        var response = await _client.PostAsJsonAsync("/api/auth/login", loginRequest);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        responseContent.Should().Contain("User.NotFoundByEmail");
    }

    public static IEnumerable<object[]> InvalidRegisterRequests =>
        new List<object[]>
        {
            new object[] { string.Empty, string.Empty, string.Empty },
            new object[] { "test@test.com", string.Empty, string.Empty },
            new object[] { string.Empty, "John Doe", string.Empty },
            new object[] { string.Empty, string.Empty, "StrongPassword123" },
            new object[] { string.Empty, "John Doe", "StrongPassword123" },
            new object[] { "test@test.com", string.Empty, "StrongPassword123" },
            new object[] { "test@test.com", "John Doe", string.Empty },
        };

    public static IEnumerable<object[]> InvalidLoginRequests =>
        new List<object[]>
        {
            new object[] { string.Empty, "StrongPassword123" },
            new object[] { "test@test.com", string.Empty },
            new object[] { string.Empty, string.Empty },
        };
}