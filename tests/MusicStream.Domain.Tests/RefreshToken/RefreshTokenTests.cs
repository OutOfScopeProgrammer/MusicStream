namespace MusicStream.Domain.Tests.RefreshToken;

public class RefreshTokenTests
{
    [Fact]
    public void IsValid_ReturnsFalse_WhenExpired()
    {
        // Arrange
        var refToken = Entities.RefreshToken.Create("teststring");
        refToken.ExpirationTime = DateTime.UtcNow.AddDays(1);
        // Act
        var result = refToken.IsValid();
        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    [InlineData(-7)]
    public void IsValid_ReturnsTrue_WhenValid(int days)
    {
        // Arrange
        var refToken = Entities.RefreshToken.Create("teststring");
        refToken.ExpirationTime = DateTime.UtcNow.AddDays(days);
        // Act
        var result = refToken.IsValid();
        // Assert
        Assert.True(result);
    }
}
