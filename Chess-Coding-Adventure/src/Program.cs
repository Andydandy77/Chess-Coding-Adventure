namespace CodingAdventureBot;

public static class Program
{
	public static void Main(string[] args)
	{
		EngineUCI engine = new(new Bot());

		string command = String.Empty;
		while (command != "quit")
		{
			command = Console.ReadLine();
			engine.ReceiveCommand(command);
		}

	}

}