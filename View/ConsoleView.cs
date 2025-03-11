using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public const char BlockChar = '#';
    public const char EmptyChar = ' ';

    public Game Game { get; set; }
    public bool[,] Grid { get; set; }

    public ConsoleView(Game game)
    {
        Game = game;
        Grid = game.Grid;

        Console.CursorVisible = false;
    }

    public void Step()
    {
        Tetromino tetromino = Game.Tetromino;

        string colorCode = GetColorCode(tetromino.Color);

        for (int x = 0; x < Game.Width; x++)
        {
            for (int y = 0; y < Game.Height; y++)
            {
                if (!Grid[x, y])
                {
                    Console.SetCursorPosition(x, y);
                    Console.WriteLine(EmptyChar);
                }
            }
        }

        foreach ((int, int) block in tetromino.Blocks)
        {
            int x = tetromino.X + block.Item1;
            int y = tetromino.Y + block.Item2;

            Console.SetCursorPosition(x, y);
            Console.WriteLine(colorCode + BlockChar);
        }
    }

    public static string GetColorCode(Color color)
    {
        return color switch
        {
            Color.Cyan => "\x1b[0;255;255m",
            Color.Blue => "\x1b[0;0;255m",
            Color.Orange => "\x1b[255;165;0m",
            Color.Yellow => "\x1b[255;255;0m",
            Color.Green => "\x1b[0;255;0m",
            Color.Purple => "\x1b[255;0;255m",
            Color.Red => "\x1b[255;0;0m",
            _ => "\x1b[39;49m",
        };
    }
}