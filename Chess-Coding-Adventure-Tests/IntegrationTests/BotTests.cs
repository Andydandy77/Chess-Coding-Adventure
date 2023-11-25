using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.IntegrationTests;

public class BotTests
{
	[Fact]
	public void EmptyFenStringThrowsException()
	{
		var bot = new Bot();
		Assert.ThrowsAny<Exception>(() => bot.SetPosition(""));
	}
	
	[Fact]
	public void CanSetPositionFromValidFen()
	{
		var bot = new Bot(); 
		bot.SetPosition("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR 0 1");
		Assert.Equal("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b - - 0 1",bot.board.CurrentFEN);
	}
	
	[Fact]
	public void CanMakeMove()
	{
		var bot = new Bot(); 
		bot.MakeMove("e2e4");
		Assert.Equal("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq e3 0 1",bot.board.CurrentFEN);
	}
}