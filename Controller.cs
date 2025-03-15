using System.Diagnostics;
using System.Windows.Input;
using Tetris.Model;
using Tetris.View;

namespace Tetris;

public class Controller
{
    public Game Game { get; }
    public ConsoleView View { get; }
    public Stopwatch GameDeltatime { get; set; } = new();
    public Stopwatch ViewDeltatime { get; set; } = new();
    public int GameTickRate { get; set; } = 25;
    public int ViewTickRate { get; set; } = 25;

    public Controller(string[] args)
    {
        (bool scalable, bool offsetable) = ParseArgs(args);
        
        Game = new Game();
        View = new ConsoleView(Game, scalable, offsetable);
    }

    public void Start()
    {
        GameDeltatime.Start();
        ViewDeltatime.Start();

        do
        {
            if (GameDeltatime.ElapsedMilliseconds >= GameTickRate)
            {
                (int, int, int) input = GetPlayerInput();

                Game.Step(input.Item1, input.Item2, input.Item3);
                GameDeltatime.Restart();
            }
            if (ViewDeltatime.ElapsedMilliseconds >= ViewTickRate)
            {
                View.Step();
                ViewDeltatime.Restart();
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
                    GameTickRate = ticks;
                    ViewTickRate = ticks;
                    i += 1;
                }
            }
            else if (args[i].Equals("-gt") && i + 1 < args.Length)
            {
                if (int.TryParse(args[i + 1], out int ticks))
                {
                    GameTickRate = ticks;
                    i += 1;
                }
            }
            else if (args[i].Equals("-vt") && i + 1 < args.Length)
            {
                if (int.TryParse(args[i + 1], out int ticks))
                {
                    ViewTickRate = ticks;
                    i += 1;
                }
            }
        }

        return (scalable, offsetable);
    }
}