using System.Windows.Input;
using Tetris.Model;
using Tetris.View;

namespace Tetris;

public class Controller
{
    public Game Game { get; set; }
    public ConsoleView View { get; set; }

    public Controller()
    {
        Game = new Game();
        View = new ConsoleView(Game);
    }

    public void Start()
    {
        int tickRate = Game.TickRate;

        do
        {
            Step();
            Thread.Sleep(tickRate);
        } while (!Keyboard.IsKeyDown(Key.Escape) && !Game.GameOver);

        View.DrawGameOver();

        do { } while (!Keyboard.IsKeyDown(Key.Escape));

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

        if (Game.GravityConstant == 0)
            yVector += Keyboard.IsKeyDown(Key.Up) ? -1 : 0; // For testing
        yVector += Keyboard.IsKeyDown(Key.Down) ? 1 : 0;

        rotation += Keyboard.IsKeyDown(Key.OemComma) ? -1 : 0;
        rotation += Keyboard.IsKeyDown(Key.OemPeriod) ? 1 : 0;

        return (xVector, yVector, rotation);
    }
}