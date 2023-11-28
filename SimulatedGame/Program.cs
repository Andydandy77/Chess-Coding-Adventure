namespace CodingAdventureBot;

public class SimulatedGame
{
    public static async Task Main(string[] args)
    {
        var botWhite = new Bot();
        var botBlack = new Bot();
        var engineWhite = new EngineUCI(botWhite);
        var engineBlack = new EngineUCI(botBlack);

        var isWhiteTurn = true;
        var gameOver = false;
        var currentFen = botWhite.board.CurrentFEN;
        var whiteTimeLeft = 60000;
        var blackTimeLeft = 60000; 

        var whiteThinkTime = 100;
        var blackThinkTime = 10;

        while (true)
        {
            var currentEngine = isWhiteTurn ? engineWhite : engineBlack;
            var timeForThisMove = isWhiteTurn ? whiteThinkTime : blackThinkTime; // time spent for this move in milliseconds

            if (isWhiteTurn)
            {
                whiteTimeLeft -= timeForThisMove;
            }
            else
            {
                blackTimeLeft -= timeForThisMove;
            }

            // Instruct the engine to calculate the best move
            var output = new StringWriter();
            Console.SetOut(output);
            currentEngine.ReceiveCommand("position fen " + currentFen);
            currentEngine.ReceiveCommand($"go movetime {(isWhiteTurn ? whiteThinkTime : blackThinkTime)}");

            await Task.Delay(500); // Simulating the engine's thinking time
            if (output.ToString().Contains("IsMated"))
            {
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
                Console.WriteLine(output.ToString());
                return;
            }
            // Capture the output from the engine and extract the best move
            var bestMove = GetBestMoveFromOutput(output.ToString());
            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

            // Print the time remaining for each player
            Console.WriteLine($"White time left (ms): {whiteTimeLeft}");
            Console.WriteLine($"Black time left (ms): {blackTimeLeft}");

            // Apply the best move to both bots and get new FEN
            var currentBot = isWhiteTurn ? botWhite : botBlack;
            currentBot.MakeMove(bestMove);
            currentFen = currentBot.board.CurrentFEN;

            // Toggle turn
            isWhiteTurn = !isWhiteTurn;
        }
    }

    private static string GetBestMoveFromOutput(string engineOutput)
    {
        var bestMove = engineOutput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
        return bestMove;
    }
}