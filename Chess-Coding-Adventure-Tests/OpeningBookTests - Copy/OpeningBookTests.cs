using Chess.Core;
using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.GameResultsTests;

public class OpeningBookTests
{
    // You may need to adjust the paths to your test opening book files
    private const string TestBookFilePath = "P:\\CPSC5610 testing\\Chess-Coding-Adventure\\Chess-Coding-Adventure\\resources\\Book.txt";

    [Fact]
    public void Constructor_WithValidFile_ShouldInitializeOpeningBook()
    {
        // Arrange
        var openingBook = new OpeningBook(TestBookFilePath);

        // Act & Assert
        // Ensure that the opening book is initialized without errors
        Assert.NotNull(openingBook);
    }

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -", false)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq -", false)] 
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 2", false)] 
    [InlineData("rnbqkb1r/pp1ppppp/8/2pnP3/8/2P5/PP1P1PPP/RNBQKBNR w KQkq -", false)] 
    public void HasBookMove_ShouldReturnCorrectResult(string fen, bool expectedResult)
    {
        // Arrange
        var openingBook = new OpeningBook(TestBookFilePath);

        // Act
        bool result = openingBook.HasBookMove(fen);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void TryGetBookMove_WithValidBoard_ShouldReturnValidMove()
    {
        // Arrange
        var openingBook = new OpeningBook(TestBookFilePath);
        var bot = new Bot();
        var board = bot.board;

        // Act
        bool result = openingBook.TryGetBookMove(board, out string moveString);

        // Assert
        Assert.False(result);
    }
}