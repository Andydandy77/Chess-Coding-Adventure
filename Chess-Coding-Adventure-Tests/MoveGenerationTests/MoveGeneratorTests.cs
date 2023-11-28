using Chess.Core;
using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.GameResultsTests;

public class MoveGeneratorTests
{
    [Fact]
    public void GenerateMoves_ShouldGenerateValidMoves()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;
        MoveGenerator moveGenerator = new MoveGenerator();
        board.LoadPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"); //initial position

        // Act
        Span <Move> moves = moveGenerator.GenerateMoves(board);

        // Assert
        Assert.True(moves.Length > 0);
        Assert.Equal(20, moves.Length); // Initial position has 20 legal moves
    }

    [Fact]
    public void InCheck_ShouldReturnFalseCheckStatus()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;
        MoveGenerator moveGenerator = new MoveGenerator();
        board.LoadPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1"); //initial position

        // Act
        moveGenerator.GenerateMoves(board);

        // Assert
        Assert.False(moveGenerator.InCheck(), "Initial position should not be in check");
    }

    [Fact]
    public void InCheck_ShouldReturnTrueCheckStatus()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;
        MoveGenerator moveGenerator = new MoveGenerator();
        board.LoadPosition("6r1/8/8/8/8/8/6q1/7K w - - 0 1"); // a checkmate position

        // Act
        moveGenerator.GenerateMoves(board);

        // Assert
        Assert.True(moveGenerator.InCheck(), "Checkmate position should be in check");
    }

    [Fact]
    public void GenerateMoves_WithCapturesOnly_ReturnsOnlyCaptureMoves()
    {
        // Arrange
        var board = new Board(); // Set up the board state for testing
        var moveGenerator = new MoveGenerator();
        board.LoadPosition("6r1/8/8/8/8/8/6q1/7K w - - 0 1"); // a checkmate position

        // Act
        var moves = moveGenerator.GenerateMoves(board, capturesOnly: true);

        // Assert
        // Checks whether the target square is occupied 
        // If the target square is occupied, it implies that the move is a capture.
        // The test asserts that all the generated moves are indeed captures.
        foreach (var move in moves.ToArray())
        {
            int targetSquare = move.TargetSquare;
            bool isCapture = board.Square[targetSquare] != Piece.None;

            Assert.True(isCapture);
        }
    }

}