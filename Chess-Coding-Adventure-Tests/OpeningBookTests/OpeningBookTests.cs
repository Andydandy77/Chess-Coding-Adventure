using Chess.Core;
using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.GameResultsTests;

public class OpeningBookTests
{
    // You may need to adjust the paths to your test opening book files

    private static readonly string bookContent = File.ReadAllText("../../../../Chess-Coding-Adventure/resources/Book.txt");


    [Fact]
    public void Constructor_WithValidFile_ShouldInitializeOpeningBook()
    {
        // Arrange
        var openingBook = new OpeningBook(bookContent);

        // Act & Assert
        // Ensure that the opening book is initialized without errors
        Assert.NotNull(openingBook);
    }

    [Theory]
    [InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
    [InlineData("rnbqkbnr/pppppppp/8/8/3P4/8/PPP1PPPP/RNBQKBNR b KQkq - 0 1")] 
    [InlineData("rnbqkb1r/pppppppp/5n2/8/3P4/8/PPP1PPPP/RNBQKBNR w KQkq - 0 1")] 
    [InlineData("rnbqkb1r/pppppppp/5n2/8/2PP4/8/PP2PPPP/RNBQKBNR b KQkq - 0 1")] 
    public void HasBookMove_ShouldReturnCorrectResult(string fen)
    {
        // Arrange
        var openingBook = new OpeningBook(bookContent);

        // Assert
        Assert.True(openingBook.HasBookMove(fen));
    }

    [Fact]
    public void TryGetBookMove_WithValidBoard_ShouldReturnValidMove()
    {
        // Arrange
        var openingBook = new OpeningBook(bookContent);
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