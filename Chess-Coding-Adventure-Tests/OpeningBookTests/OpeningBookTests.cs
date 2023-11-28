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

    // Failed to return Correct Result
    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq -", true)]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq -", true)] 
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 2", true)] 
    [InlineData("rnbqkb1r/pp1ppppp/8/2pnP3/8/2P5/PP1P1PPP/RNBQKBNR w KQkq -", true)] 
    public void HasBookMove_ShouldReturnCorrectResult(string fen, bool expectedResult)
    {
        // Arrange
        var openingBook = new OpeningBook(TestBookFilePath);

        // Act
        bool result = openingBook.HasBookMove(fen);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    // Failed to return Valid Move 
    [Fact]
    public void TryGetBookMove_WithValidBoard_ShouldReturnValidMove()
    {
        // Arrange
        var openingBook = new OpeningBook(TestBookFilePath);
        var bot = new Bot();
        var board = bot.board;
        // Set up the board with a position that is present in the opening book
        board.LoadPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        // Act
        bool result = openingBook.TryGetBookMove(board, out string moveString);

        // Assert
        Assert.True(result);
        Assert.NotEqual("Null", moveString);
    }
}