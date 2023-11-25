using CodingAdventureBot;

namespace Chess_Coding_Adventure_Tests.IntegrationTests;

public class EngineUciTests
{
	[Fact]
	public void TestOpeningBookResponses()
	{
		var engine = new EngineUCI(new Bot());
		var output = new StringWriter();
		Console.SetOut(output);
    
		engine.ReceiveCommand("position startpos");
		engine.ReceiveCommand("go wtime 60000 btime 60000");

		var consoleOutput = output.ToString();
		var possibleMoves = new[] { "e2e4", "d2d4", "g1f3", "c2c4", "f2f4" };
		var moveFound = possibleMoves.Any(move => consoleOutput.Contains($"bestmove {move}"));

		Assert.True(moveFound, "Output does not contain a valid move.");

		Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
	}
	
	[Fact]
	public void TestPositionCommandChangesBoardState()
	{
		var bot = new Bot();
		var engine = new EngineUCI(bot);
		engine.ReceiveCommand("position fen rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 moves e2e4 e7e5"); //nonstard fen
		Assert.NotEqual(bot.board.GameStartFEN, bot.board.CurrentFEN);
	}
	
	[Fact]
	public async Task TestStopGeneratesBestMoveWithLongThinkTime()
	{
		var bot = new Bot();
		var engine = new EngineUCI(bot);
		var output = new StringWriter();
		Console.SetOut(output);

		try
		{
			engine.ReceiveCommand("position fen r1bqkb1r/p2n2pp/3p1np1/1pp1p3/4P3/1PPB1Q2/PB1P1PPP/RN2K1NR - 0 1");    
			engine.ReceiveCommand("go movetime 5000000"); // a more reasonable think time
			await Task.Delay(1000); // Allow some time for the bot to start processing
			engine.ReceiveCommand("stop");
			await Task.Delay(100); // Allow a little time for the stop to take effect

			var consoleOutput = output.ToString();
			Assert.Contains("bestmove", consoleOutput);
		}
		finally
		{
			Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
		}
	}
	
	[Fact]
	public void TestFullGameSimulation()
	{
		var engine = new EngineUCI(new Bot());
		var output = new StringWriter();
		Console.SetOut(output);
		engine.ReceiveCommand("position startpos moves f2f3 e7e6 g2g4 d8h4");
		engine.ReceiveCommand("go wtime 60000 btime 60000");
		Assert.Equal("WhiteIsMated", output.ToString());

	}

}