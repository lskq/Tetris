using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public const char SpaceSymbol = ' ';
    public const char MinoSymbol = ' ';
    public const char BorderSymbol = '#';
    public const string DefaultCode = "\x1b[0m";

    public Game Game { get; }
    public Mino[][] Grid { get; }

    public int ScoreTracker { get; set; }

    public ConsoleView(Game game)
    {
        Game = game;
        Grid = game.Grid;
        ScoreTracker = game.Score;

        Console.CursorVisible = false;
        Console.Clear();

        DrawBorder();
        DrawScore();
    }

    public void Step()
    {
        for (int x = 0; x < Game.Width; x++)
        {
            for (int y = 0; y < Game.Height; y++)
            {
                Console.SetCursorPosition(x, y);

                if (Grid[y][x] == null)
                {
                    Console.Write(DefaultCode + SpaceSymbol);
                }
                else
                {
                    string colorCode = GetColorCode(Grid[y][x].Color);
                    Console.Write(colorCode + MinoSymbol);
                }
            }
        }

        if (ScoreTracker != Game.Score)
            DrawScore();
    }

    public void DrawScore()
    {
        Console.SetCursorPosition(Game.Width + 2, 0);
        Console.Write(DefaultCode + Game.Score);
    }

    public void DrawBorder()
    {
        int width = Game.Width;
        int height = Game.Height;

        for (int y = 0; y < height; y++)
        {
            Console.SetCursorPosition(width, y);
            Console.Write(BorderSymbol);
        }

        Console.SetCursorPosition(0, height);

        for (int x = 0; x < width; x++)
        {
            Console.Write(BorderSymbol);
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