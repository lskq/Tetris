using System.Windows;
using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public const string DefaultCode = "\x1b[0m";

    public Game Game { get; }
    public Mino[][] Grid { get; }
    public int ScoreTracker { get; set; }

    public bool Scalable { get; }
    public bool Offsetable { get; }

    // The following are set using UpdateScreenMath() in DrawScreen(). Expressions were too performance-intensive
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public int YScale { get; set; } = 1;
    public int XScale { get; set; } = 2;
    public int GridXOffset { get; set; }
    public int GridYOffset { get; set; }

    public ConsoleView(Game game, bool scalable = false, bool offsetable = false)
    {
        Game = game;
        Grid = game.Grid;
        ScoreTracker = game.Score;

        Scalable = scalable;
        Offsetable = offsetable;

        Console.CursorVisible = false;

        ValidateScreenSize();
        DrawScreen();
    }

    public void Step()
    {
        try
        {
            if (ScreenWidth != Console.WindowWidth || ScreenHeight != Console.WindowHeight)
            {
                ValidateScreenSize();
                DrawScreen();
            }
            else if (!Game.GameOver)
            {
                DrawMinoesInGrid();
            }
            else
            {
                DrawGameOver();
            }
        }
        catch (ArgumentOutOfRangeException)
        {
            ValidateScreenSize();
            DrawScreen();
        }
    }

    public void DrawMinoesInGrid()
    {
        for (int x = 0; x < Game.Width; x++)
        {
            for (int y = 0; y < Game.Height; y++)
            {
                DrawMino(x, y);
            }
        }
    }

    public void DrawMino(int x, int y)
    {
        string color;

        if (Grid[y][x] == null)
        {
            color = GetColor(ConsoleColor.Grid);
        }
        else
        {
            color = GetColor(Grid[y][x].MinoColor);
        }

        DrawPixel(GridXOffset + x * XScale, GridYOffset + y * YScale, color);
    }

    public void DrawGameOver()
    {
        string message = "Game Over";

        int x = GridXOffset + (Game.Width * XScale) / 2 - message.Length / 2;
        int y = GridYOffset + (Game.Height * YScale) / 2;

        Console.SetCursorPosition(x, y);
        Console.Write(message);
    }

    public void ValidateScreenSize()
    {
        while (Console.WindowWidth < Game.Width * 2 || Console.WindowHeight < Game.Height)
        {
            DrawBackground();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Game suspended. Please increase window size to continue.");

            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight;

            do { } while (currentWidth == Console.WindowWidth && currentHeight == Console.WindowHeight);
        }
    }

    public void DrawScreen()
    {
        UpdateScreenMath();
        DrawBackground();
        DrawGrid();

        if (Game.GameOver)
        {
            DrawMinoesInGrid();
        }
    }

    public void DrawGrid()
    {
        string color = GetColor(ConsoleColor.Grid);
        for (int y = 0; y < Game.Height * YScale; y++)
        {
            Console.SetCursorPosition(GridXOffset, GridYOffset + y);

            for (int x = 0; x < Game.Width * XScale; x++)
            {
                Console.Write(color + " ");
            }
        }
    }

    public void DrawBackground()
    {
        string color = GetColor(ConsoleColor.Background);
        for (int y = 0; y < Console.WindowHeight; y++)
        {
            Console.SetCursorPosition(0, y);
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                Console.Write(color + " ");
            }
        }
    }

    public void DrawPixel(int left, int top, string colorCode)
    {
        for (int y = 0; y < YScale; y++)
        {
            Console.SetCursorPosition(left, top + y);
            for (int x = 0; x < XScale; x++)
            {
                Console.Write(colorCode + " ");
            }
        }
    }

    public void UpdateScreenMath()
    {
        ScreenWidth = Console.WindowWidth;
        ScreenHeight = Console.WindowHeight;

        if (Scalable)
        {
            YScale = (Console.WindowWidth / (Game.Width * 2) < Console.WindowHeight / Game.Height) ?
                        Console.WindowWidth / (Game.Width * 2) : Console.WindowHeight / Game.Height;
            XScale = YScale * 2;
        }

        if (Offsetable)
        {
            GridXOffset = (Console.WindowWidth - Game.Width * XScale) / 2;
            GridYOffset = (Console.WindowHeight - Game.Height * YScale) / 2;
        }
    }

    public static string GetColor(ConsoleColor consoleColor)
    {
        return consoleColor switch
        {
            ConsoleColor.Default => "\x1b[39;49m",
            ConsoleColor.Background => "\x1b[48;2;63;63;63m",
            ConsoleColor.Grid => "\x1b[48;2;223;223;223m",
            _ => "\x1b[39;49m",
        };
    }

    public static string GetColor(MinoColor minoColor)
    {
        return minoColor switch
        {
            MinoColor.Cyan => "\x1b[48;2;0;255;255m",
            MinoColor.Blue => "\x1b[48;2;0;0;255m",
            MinoColor.Orange => "\x1b[48;2;255;165;0m",
            MinoColor.Yellow => "\x1b[48;2;255;255;0m",
            MinoColor.Green => "\x1b[48;2;0;255;0m",
            MinoColor.Purple => "\x1b[48;2;255;0;255m",
            MinoColor.Red => "\x1b[48;2;255;0;0m",
            _ => "\x1b[39;49m",
        };
    }
}