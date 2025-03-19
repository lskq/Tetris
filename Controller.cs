using System.Diagnostics;
using System.Windows.Input;
using Tetris.Model;
using Tetris.View;

namespace Tetris;

public class Controller
{
    public Game Game { get; }
    public ConsoleView View { get; }

    public CancellationTokenSource Source { get; } = new();

    public Stopwatch Deltatime { get; set; } = new();
    public int TickRate { get; set; } = 100;

    public Controller()
    {
        Game = new Game();
        View = new ConsoleView(Game, Source.Token);

        var keyTask = Task.Run(() => ConsoleKeyIntercept(), Source.Token);
    }

    public void Start()
    {
        Deltatime.Start();

        do
        {
            Step();
        } while (!Keyboard.IsKeyDown(Key.Escape));

        Source.Cancel();
        Console.Write("\x1b[0m");
        Console.Clear();
        Console.CursorVisible = true;
    }

    public void Step()
    {
        if (Deltatime.ElapsedMilliseconds >= TickRate)
        {
            (int xInput, int yInput, int rotation) = ReadPlayerInput();

            Game.Step(xInput, yInput, rotation);
            View.Step();
            Deltatime.Restart();
        }
    }

    public void ConsoleKeyIntercept()
    {
        while (true)
        {
            if (Console.KeyAvailable)
                Console.ReadKey(true);
        }
    }

    public (int, int, int) ReadPlayerInput()
    {
        int xVector = 0;
        int yVector = 0;
        int rotation = 0;

        xVector += Keyboard.IsKeyDown(Key.Left) ? -1 : 0;
        xVector += Keyboard.IsKeyDown(Key.Right) ? 1 : 0;
        yVector += Keyboard.IsKeyDown(Key.Down) ? 1 : 0;

        rotation += Keyboard.IsKeyDown(Key.Z) ? -1 : 0;
        rotation += Keyboard.IsKeyDown(Key.X) ? 1 : 0;

        return (xVector, yVector, rotation);
    }

    public static bool ConsoleIsKeyDown(ConsoleKey key)
    {
        return Console.KeyAvailable && Console.ReadKey(true).Key == key;
    }

    public (bool scalable, bool offsetable) ParseArgs(string[] args)
    {
        bool offsetable = false;
        bool scalable = false;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("-o"))
                offsetable = true;
            else if (args[i].Equals("-s"))
                scalable = true;
            else if (args[i].Equals("-t") && i + 1 < args.Length)
            {
                if (int.TryParse(args[i + 1], out int ticks))
                {
                    TickRate = ticks;
                    i += 1;
                }
            }
        }

        return (scalable, offsetable);
    }
}