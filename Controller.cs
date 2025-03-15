using System.Diagnostics;
using System.Windows.Input;
using Tetris.Model;
using Tetris.View;

namespace Tetris;

public class Controller
{
    public Game Game { get; }
    public ConsoleView View { get; }
    public Stopwatch Deltatime { get; set; } = new();
    public int TickRate { get; set; } = 100;

    public Controller(string[] args)
    {
        (bool scalable, bool offsetable) = ParseArgs(args);

        Game = new Game();
        View = new ConsoleView(Game, scalable, offsetable);
    }

    public void Start()
    {
        Deltatime.Start();

        do
        {
            if (Deltatime.ElapsedMilliseconds >= TickRate)
            {
                (int, int, int) input = GetPlayerInput();

                Game.Step(input.Item1, input.Item2, input.Item3);
                View.Step();
                Deltatime.Restart();
            }
        } while (!Keyboard.IsKeyDown(Key.Q));

        throw new Exception(); //For testing
    }

    public void Step()
    {
        (int, int, int) input = GetPlayerInput();

        Game.Step(input.Item1, input.Item2, input.Item3);
        View.Step();
    }

    public (int, int, int) GetPlayerInput()
    {
        int xVector = 0;
        int yVector = 0;
        int rotation = 0;

        xVector += Keyboard.IsKeyDown(Key.Left) ? -1 : 0;
        xVector += Keyboard.IsKeyDown(Key.Right) ? 1 : 0;
        yVector += Keyboard.IsKeyDown(Key.Down) ? 1 : 0;

        rotation += Keyboard.IsKeyDown(Key.OemComma) ? -1 : 0;
        rotation += Keyboard.IsKeyDown(Key.OemPeriod) ? 1 : 0;

        return (xVector, yVector, rotation);
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