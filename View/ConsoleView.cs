using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public const char Space = ' ';

    public Game Game { get; set; }
    public bool[,] Grid { get; set; }

    public ConsoleView(Game game)
    {
        Game = game;
        Grid = game.Grid;

        Console.CursorVisible = false;
        Console.Clear();
    }

    public void Step()
    {
        Tetromino tetromino = Game.Tetromino;

        string emptyString = "\x1b[0m" + Space;

        for (int x = 0; x < Game.Width; x++)
        {
            for (int y = 0; y < Game.Height; y++)
            {
                if (!Grid[x, y])
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(emptyString);
                }
            }
        }

        string blockString = GetColorCode(tetromino.Color) + Space;

        foreach ((int, int) block in tetromino.Blocks)
        {
            int x = tetromino.X + block.Item1;
            int y = tetromino.Y + block.Item2;

            Console.SetCursorPosition(x, y);
            Console.Write(blockString);
        }
    }

    public static string GetColorCode(Color color)
    {
        return color switch
        {
            Color.Cyan => "\x1b[48;2;0;255;255m",
            Color.Blue => "\x1b[48;2;0;0;255m",
            Color.Orange => "\x1b[48;2;255;165;0m",
            Color.Yellow => "\x1b[48;2;255;255;0m",
            Color.Green => "\x1b[48;2;0;255;0m",
            Color.Purple => "\x1b[48;2;255;0;255m",
            Color.Red => "\x1b[48;2;255;0;0m",
            _ => "\x1b[39;49m",
        };
    }
}