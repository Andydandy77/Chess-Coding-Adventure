using Chess.Core;
using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.HelpersTests;

public class FenUtilityTests
{

	[Fact]
    public void CurrentFen_WithInitialPosition_ShouldMatchStartPositionFEN()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;

        // Act
        string fen = FenUtility.CurrentFen(board);

        // Assert
        Assert.Equal(FenUtility.StartPositionFEN, fen);
    }
    [Fact]
    public void PositionFromFen_WithStartPositionFEN_ShouldReturnExpectedPositionInfo()
    {
        // Arrange
        string startFen = FenUtility.StartPositionFEN;

        // Act
        var positionInfo = FenUtility.PositionFromFen(startFen);

        // Assert
        Assert.Equal(startFen, positionInfo.fen);
    }
    // Failed to flip Fen
    [Fact]
    public void FlipFen_WithValidFen_ShouldReturnFlippedFen()
    {
        // Arrange
        string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        string expectedFlippedFen = "8/pppppppp/rnbqkbnr/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1";

        // Act
        string flippedFen = FenUtility.FlipFen(fen);

        // Assert
        Assert.Equal(expectedFlippedFen, flippedFen);
    }
}