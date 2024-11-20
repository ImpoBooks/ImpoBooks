using Moq;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.DataAccess.Interfaces;
using ImpoBooks.Infrastructure.Providers;
using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.Infrastructure;

namespace ImpoBooks.Tests.Unit.BusinessTests;

public class AuthServiceTests
{
    private readonly Mock<IUsersRepository> _usersRepositoryMock;
    private readonly Mock<IPasswordHasher> _passwordHasherMock;
    private readonly Mock<IJwtProvider> _jwtProviderMock;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _usersRepositoryMock = new Mock<IUsersRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _jwtProviderMock = new Mock<IJwtProvider>();
        _authService = new AuthService(
            _usersRepositoryMock.Object,
            _passwordHasherMock.Object,
            _jwtProviderMock.Object);
    }

    [Fact]
    public async Task Register_UserIsNull_ReturnsIsNullError()
    {
        // Act
        ErrorOr<Success> result = await _authService.RegisterAsync(null);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.IsNull, result.FirstError);
    }

    [Theory]
    [InlineData("albom2004q@gmail.com")]
    public async Task Register_UserAlreadyExists_ReturnsAlreadyExistsError(string email)
    {
        // Arrange
        User existingUser = new User { Email = email };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(existingUser.Email))
            .ReturnsAsync(existingUser);

        // Act
        ErrorOr<Success> result = await _authService.RegisterAsync(existingUser);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.AlreadyExists, result.FirstError);
    }

    [Fact]
    public async Task Register_ValidUser_ReturnsSuccess()
    {
        // Arrange
        User newUser = new User { Email = "test@example.com" };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(newUser.Email))
            .ReturnsAsync(null as User);

        // Act
        ErrorOr<Success> result = await _authService.RegisterAsync(newUser);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(Result.Success, result.Value);
        _usersRepositoryMock.Verify(repo => repo.CreateAsync(newUser), Times.Once);
    }

    [Fact]
    public async Task Login_UserNotFound_ReturnsNotFoundByEmailError()
    {
        // Arrange
        string email = "test@example.com";
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(null as User);

        // Act
        ErrorOr<string> result = await _authService.LoginAsync(email, "password");

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.NotFoundByEmail, result.FirstError);
    }

    [Fact]
    public async Task Login_WrongPassword_ReturnsWrongPasswordError()
    {
        // Arrange
        string email = "test@example.com";
        User dbUser = new User { Email = email, HashedPassword = "hashedPassword" };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(dbUser);
        _passwordHasherMock
            .Setup(hasher => hasher.Verify("wrongPassword", dbUser.HashedPassword))
            .Returns(false);

        // Act
        ErrorOr<string> result = await _authService.LoginAsync(email, "wrongPassword");

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.WrongPassword, result.FirstError);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsJwtToken()
    {
        // Arrange
        string email = "test@example.com";
        User dbUser = new User { Email = email, HashedPassword = "hashedPassword" };
        string token = "jwtToken";
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(dbUser);
        _passwordHasherMock
            .Setup(hasher => hasher.Verify("password", dbUser.HashedPassword))
            .Returns(true);
        _jwtProviderMock
            .Setup(provider => provider.GenerateToken(dbUser))
            .Returns(token);

        // Act
        ErrorOr<string> result = await _authService.LoginAsync(email, "password");

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(token, result.Value);
    }

    [Fact]
    public void GenerateJwt_ValidUser_ReturnsJwtToken()
    {
        // Arrange
        User dbUser = new User { Email = "test@example.com" };
        string token = "jwtToken";
        _jwtProviderMock
            .Setup(provider => provider.GenerateToken(dbUser))
            .Returns(token);

        // Act
        ErrorOr<string> result = _authService.GenerateJwt(dbUser);

        // Assert
        Assert.False(result.IsError);
        Assert.Equal(token, result.Value);
    }

    [Fact]
    public async Task Register_UserEmailIsEmpty_ReturnsIsNullOrEmptyError()
    {
        // Arrange
        User user = new User { Email = "" };

        // Act
        ErrorOr<Success> result = await _authService.RegisterAsync(user);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.EmailIsNullOrEmpty, result.FirstError);
    }

    [Fact]
    public async Task Register_RepositoryThrowsException_ReturnsFailure()
    {
        // Arrange
        User user = new User { Email = "test@example.com" };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(user.Email))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.RegisterAsync(user));
    }

    [Fact]
    public async Task Login_RepositoryThrowsException_ReturnsFailure()
    {
        // Arrange
        string email = "test@example.com";
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(email, "password"));
    }

    [Fact]
    public async Task Login_EmptyEmail_ReturnsNotFoundByEmailError()
    {
        // Arrange
        string email = string.Empty;

        // Act
        ErrorOr<string> result = await _authService.LoginAsync(email, "password");

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.NotFoundByEmail, result.FirstError);
    }

    [Fact]
    public async Task Login_EmptyPassword_ReturnsWrongPasswordError()
    {
        // Arrange
        string email = "test@example.com";
        User dbUser = new User { Email = email, HashedPassword = "hashedPassword" };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(dbUser);

        // Act
        ErrorOr<string> result = await _authService.LoginAsync(email, string.Empty);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.WrongPassword, result.FirstError);
    }

    [Fact]
    public async Task Login_PasswordHasherThrowsException_ReturnsFailure()
    {
        // Arrange
        string email = "test@example.com";
        User dbUser = new User { Email = email, HashedPassword = "hashedPassword" };
        _usersRepositoryMock
            .Setup(repo => repo.GetByEmailAsync(email))
            .ReturnsAsync(dbUser);
        _passwordHasherMock
            .Setup(hasher => hasher.Verify(It.IsAny<string>(), dbUser.HashedPassword))
            .Throws(new Exception("Hashing error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _authService.LoginAsync(email, "password"));
    }

    [Fact]
    public void GenerateJwt_EmptyUser_ReturnsFailure()
    {
        // Arrange
        User? dbUser = null;

        // Act
        ErrorOr<string> result = _authService.GenerateJwt(dbUser);

        // Assert
        Assert.True(result.IsError);
        Assert.Equal(UserErrors.IsNull, result.FirstError);
    }

    [Fact]
    public void GenerateJwt_JwtProviderThrowsException_ReturnsFailure()
    {
        // Arrange
        User dbUser = new User { Email = "test@example.com" };
        _jwtProviderMock
            .Setup(provider => provider.GenerateToken(dbUser))
            .Throws(new Exception("JWT generation error"));

        // Act & Assert
        Assert.Throws<Exception>(() => _authService.GenerateJwt(dbUser));
    }
}