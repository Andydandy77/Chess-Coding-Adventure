using Chess.Core;
using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.HelpersTests;

public class PGNCreatorTests
{
    [Fact]
    public void CreatePGN_WithMovesAndResult_ShouldReturnCorrectPGN()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;

        var moves = new Move[]
        {
        MoveUtility.GetMoveFromUCIName("e2e4", board),
        MoveUtility.GetMoveFromUCIName("e7e5", board),
        MoveUtility.GetMoveFromUCIName("g1f3", board),
        MoveUtility.GetMoveFromUCIName("b8c6", board),
        MoveUtility.GetMoveFromUCIName("f1c4", board),
        MoveUtility.GetMoveFromUCIName("g8f6", board),
        };

        // Act
        string pgn = PGNCreator.CreatePGN(moves);

        // Assert
        string expectedPGN = "[Result \"InProgress\"]\r\n1. " + MoveUtility.GetMoveNameSAN(moves[0], board) + " " + MoveUtility.GetMoveNameSAN(moves[1], board) + " ";
        expectedPGN += "2. " + MoveUtility.GetMoveNameSAN(moves[2], board) + " " + MoveUtility.GetMoveNameSAN(moves[3], board) + " ";
        expectedPGN += "3. " + MoveUtility.GetMoveNameSAN(moves[4], board) + " " + MoveUtility.GetMoveNameSAN(moves[5], board) + " ";
        Assert.Equal(expectedPGN.Trim(), pgn.Trim());
    }

    [Fact]
    public void CreatePGN_WithEmptyMoves_ShouldReturnCorrectPGN()
    {
        // Act
        var bot = new Bot();
        var board = bot.board;
        string pgn = PGNCreator.CreatePGN(board, GameResult.NotStarted, "Player1", "Player2");

        // Assert
        string expectedPGN = "[White \"Player1\"]\r\n[Black \"Player2\"]";
        Assert.Equal(expectedPGN.Trim(), pgn.Trim());
    }

    [Fact]
    public void CreatePGN_WithPlayersAndFEN_ShouldReturnCorrectPGN()
    {
        // Arrange
        var bot = new Bot();
        var board = bot.board;

        var moves = new Move[]
        {
        MoveUtility.GetMoveFromUCIName("e2e4", board),
        MoveUtility.GetMoveFromUCIName("d7d5", board),
        MoveUtility.GetMoveFromUCIName("g1f3", board),
        MoveUtility.GetMoveFromUCIName("c7c6", board),
        };

        // Act
        string pgn = PGNCreator.CreatePGN(moves, GameResult.InProgress, FenUtility.StartPositionFEN, "Player1", "Player2");

        // Assert
        string expectedPGN = "[White \"Player1\"]\r\n[Black \"Player2\"]\r\n[Result \"InProgress\"]\r\n";
        expectedPGN += "1. " + MoveUtility.GetMoveNameSAN(moves[0], board) + " " + MoveUtility.GetMoveNameSAN(moves[1], board) + " ";
        expectedPGN += "2. " + MoveUtility.GetMoveNameSAN(moves[2], board) + " " + MoveUtility.GetMoveNameSAN(moves[3], board) + " ";
        Assert.Equal(expectedPGN.Trim(), pgn.Trim());
    }

    
}