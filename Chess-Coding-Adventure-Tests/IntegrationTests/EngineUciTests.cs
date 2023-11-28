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
	public void TestCheckmate()
	{
		var engine = new EngineUCI(new Bot());
		var output = new StringWriter();
		Console.SetOut(output);
		engine.ReceiveCommand("position startpos moves f2f3 e7e6 g2g4 d8h4");
		engine.ReceiveCommand("go wtime 60000 btime 60000");
		Assert.Equal("WhiteIsMated", output.ToString());
		
	}
	
	[Fact]
	public async void TestFullGameSimulation()
	{
		var botWhite = new Bot();
        var botBlack = new Bot();
        var engineWhite = new EngineUCI(botWhite);
        var engineBlack = new EngineUCI(botBlack);

        var isWhiteTurn = true;
        var currentFen = botWhite.board.CurrentFEN;

        const int whiteThinkTime = 100;
        const int blackThinkTime = 10;

        while (true)
        {
	        var currentEngine = isWhiteTurn ? engineWhite : engineBlack;

	        // Instruct the engine to calculate the best move
	        var output = new StringWriter();
	        Console.SetOut(output);
	        currentEngine.ReceiveCommand("position fen " + currentFen);
	        currentEngine.ReceiveCommand($"go movetime {(isWhiteTurn ? whiteThinkTime : blackThinkTime)}");

	        await Task.Delay(200); // Simulating the engine's thinking time
	        if (output.ToString().Contains("IsMated"))
	        {
		        Assert.Equal("BlackIsMated",output.ToString());
		        return;
	        }

	        // Capture the output from the engine and extract the best move
	        var bestMove = GetBestMoveFromOutput(output.ToString());
	        // Apply the best move to both bots and get new FEN
	        var currentBot = isWhiteTurn ? botWhite : botBlack;
	        currentBot.MakeMove(bestMove);
	        currentFen = currentBot.board.CurrentFEN;

	        // Toggle turn
	        isWhiteTurn = !isWhiteTurn;

        }
        
	}
	[Fact]
	public void TestBoard()
	{
        var engine = new EngineUCI(new Bot());
        var output = new StringWriter();
        Console.SetOut(output);
        engine.ReceiveCommand("position startpos moves f2f3");
		Assert.Equal("+---+---+---+---+---+---+---+---+\r\n| r | n | b | q | k | b | n | r | 8\r\n+---+---+---+---+---+---+---+---+\r\n| p | p | p | p | p | p | p | p | 7\r\n+---+---+---+---+---+---+---+---+\r\n|   |   |   |   |   |   |   |   | 6\r\n+---+---+---+---+---+---+---+---+\r\n|   |   |   |   |   |   |   |   | 5\r\n+---+---+---+---+---+---+---+---+\r\n|   |   |   |   |   |   |   |   | 4\r\n+---+---+---+---+---+---+---+---+\r\n|   |   |   |   |   |(P)|   |   | 3\r\n+---+---+---+---+---+---+---+---+\r\n| P | P | P | P | P |   | P | P | 2\r\n+---+---+---+---+---+---+---+---+\r\n| R | N | B | Q | K | B | N | R | 1\r\n+---+---+---+---+---+---+---+---+\r\n  a   b   c   d   e   f   g   h  \r\n\r\n\r\n", output.ToString());
    	}

	private string GetBestMoveFromOutput(string engineOutput)
	{
		return engineOutput.Split(" ")[1];
	}

}
