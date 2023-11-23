using Chess.Core;

namespace Chess_Coding_Adventure_Tests.GameResultsTests;

public class ArbiterTests
{
	[Theory]
	[InlineData(GameResult.DrawByArbiter, true)]
	[InlineData(GameResult.FiftyMoveRule, true)]
	[InlineData(GameResult.Repetition, true)]
	[InlineData(GameResult.Stalemate, true)]
	[InlineData(GameResult.InsufficientMaterial, true)]
	[InlineData(GameResult.WhiteIsMated, false)]
	public void IsDrawResult_ShouldReturnCorrectValue(GameResult result, bool expected)
	{
		Assert.Equal(expected, Arbiter.IsDrawResult(result));
	}
	
	[Theory]
	[InlineData(GameResult.WhiteIsMated, true)]
	[InlineData(GameResult.BlackIsMated, true)]
	[InlineData(GameResult.BlackTimeout, true)]
	[InlineData(GameResult.WhiteTimeout, true)]
	public void IsWinResult_ShouldReturnCorrectValue(GameResult result, bool expected)
	{
		Assert.Equal(expected, Arbiter.IsWinResult(result));
	}
	
	[Theory]
	[InlineData(GameResult.BlackIsMated, true)]
	[InlineData(GameResult.BlackTimeout, true)]
	[InlineData(GameResult.BlackIllegalMove, true)]
	[InlineData(GameResult.WhiteIsMated, false)]
	public void IsWhiteWinsResult_ShouldReturnCorrectValue(GameResult result, bool expected)
	{
		Assert.Equal(expected, Arbiter.IsWhiteWinsResult(result));
	}
	
	[Theory]
	[InlineData(GameResult.WhiteIsMated, true)]
	[InlineData(GameResult.WhiteTimeout, true)]
	[InlineData(GameResult.WhiteIllegalMove, true)]
	[InlineData(GameResult.BlackIsMated, false)]
	public void IsBlackWinsResult_ShouldReturnCorrectValue(GameResult result, bool expected)
	{
		Assert.Equal(expected, Arbiter.IsBlackWinsResult(result));
	}
	
	[Fact]
	public void GetGameState_ShouldReturnCheckmate_WhenBlackKingIsCheckmated()
	{
		// Arrange
		var board = new Board();
		board.LoadPosition("3k4/3Q4/8/1B6/8/8/8/3K4 b - - 0 1"); // FEN string representing a checkmate position
		var expected = GameResult.BlackIsMated; // or WhiteIsMated depending on the position

		// Act
		var actual = Arbiter.GetGameState(board);

		// Assert
		Assert.Equal(expected, actual);
	}
	
	[Fact]
	public void GetGameState_ShouldReturnCheckmate_WhenWhitekKingIsCheckmated()
	{
		// Arrange
		var board = new Board();
		board.LoadPosition("6r1/8/8/8/8/8/6q1/7K w - - 0 1"); // FEN string representing a checkmate position
		var expected = GameResult.WhiteIsMated; // or WhiteIsMated depending on the position

		// Act
		var actual = Arbiter.GetGameState(board);

		// Assert
		Assert.Equal(expected, actual);
	}
	
	[Theory]
	[InlineData("8/8/8/8/8/8/8/K6k w - - 0 1", GameResult.InsufficientMaterial)] // King vs. King
	[InlineData("8/8/8/8/8/6n1/8/K6k w - - 0 1", GameResult.InsufficientMaterial)] // King and Knight vs. King
	[InlineData("8/8/8/8/8/6b1/8/K6k w - - 0 1", GameResult.InsufficientMaterial)] // King and Bishop vs. King
	public void GetGameState_ShouldReturnInsufficientMaterial(string fen, GameResult expected)
	{
		var board = new Board();
		board.LoadPosition(fen);
		var actual = Arbiter.GetGameState(board);
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void GetGameState_ShouldReturnStalemate()
	{
		var board = new Board();
		board.LoadPosition("7k/5Q2/5K2/8/8/8/8/8 b - - 0 1"); // FEN for a stalemate position
		var expected = GameResult.Stalemate;

		var actual = Arbiter.GetGameState(board);

		Assert.Equal(expected, actual);
	}
	
	[Fact]
	public void GetGameState_ShouldReturnFiftyMoveRule()
	{
		var board = new Board();
		board.LoadPosition("8/8/8/8/8/5N2/8/5K1k w - - 0 1"); // FEN with White Knight on f3, White King on f1, Black King on h1

		// Calculate the indices for the squares involved in the moves
		int f3Index = BoardHelper.IndexFromCoord(5, 2);
		int g1Index = BoardHelper.IndexFromCoord(6, 0);
		int h1Index = BoardHelper.IndexFromCoord(7, 0);
		int h2Index = BoardHelper.IndexFromCoord(7, 1);

		// Create moves for the White Knight and Black King
		Move knightToF3 = new Move(g1Index, f3Index);
		Move knightToG1 = new Move(f3Index, g1Index);
		Move blackKingToH2 = new Move(h1Index, h2Index);
		Move blackKingToH1 = new Move(h2Index, h1Index);

		// Apply 50 moves in total, alternating between White and Black
		for (int i = 0; i < 25; i++)
		{
			board.MakeMove(knightToG1);
			board.MakeMove(blackKingToH2);
			board.MakeMove(knightToF3);
			board.MakeMove(blackKingToH1);
		}

		var expected = GameResult.FiftyMoveRule;

		var actual = Arbiter.GetGameState(board);

		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", GameResult.InProgress)] // Starting position, game in progress
	[InlineData("rnb1kbnr/pppp1ppp/8/4p3/6Pq/5P2/PPPPP2P/RNBQKBNR w KQkq - 1 3", GameResult.WhiteIsMated)] // Fool's mate, Black is checkmated
	public void GetGameState_ShouldIdentifyCheckmateCorrectly(string fen, GameResult expected)
	{
		var board = new Board();
		board.LoadPosition(fen);
		var actual = Arbiter.GetGameState(board);
		Assert.Equal(expected, actual);
	}
}