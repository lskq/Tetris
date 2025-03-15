using System.Windows;
using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public Game Game { get; }
    public Mino[][] Grid { get; }
    public int ScoreTracker { get; set; }

    // ScreenMath
    public bool Scalable { get; set; }
    public bool Offsetable { get; set; }
    public int ScreenWidth { get; set; } = Console.WindowWidth;
    public int ScreenHeight { get; set; } = Console.WindowHeight;
    public int YScale { get; set; } = 1;
    public int XScale { get; set; } = 2;
    public int GridXOffset { get; set; } = 0;
    public int GridYOffset { get; set; } = 0;

    public ConsoleView(Game game, bool scalable, bool offsetable)
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
            
            if (!Game.GameOver)
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
            bool loop = true;
            while (loop)
            {
                try
                {
                    ValidateScreenSize();
                    DrawScreen();
                    loop = false;
                }
                catch (ArgumentOutOfRangeException)
                {
                    // Try again
                }
            }
            
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
            color = ConsoleColor.Grid;
        }
        else
        {
            color = GetMinoColor(Grid[y][x].MinoColor);
        }

        DrawPixel(GridXOffset + x * XScale, GridYOffset + y * YScale, color);
    }

    public void DrawGameOver()
    {
        string message = "Game Over";

        int x = GridXOffset + Game.Width * XScale / 2 - message.Length / 2;
        int y = GridYOffset + Game.Height * YScale / 2;

        Console.SetCursorPosition(x, y);
        Console.Write(ConsoleColor.Grid + message);
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
        string color = ConsoleColor.Grid;
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
        string color = ConsoleColor.Background;
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

    public static string GetMinoColor(MinoColor minoColor)
    {
        return minoColor switch
        {
            MinoColor.Cyan => ConsoleColor.Cyan,
            MinoColor.Blue => ConsoleColor.Blue,
            MinoColor.Orange => ConsoleColor.Orange,
            MinoColor.Yellow => ConsoleColor.Yellow,
            MinoColor.Green => ConsoleColor.Green,
            MinoColor.Purple => ConsoleColor.Purple,
            MinoColor.Red => ConsoleColor.Red,
            _ => ConsoleColor.Default,
        };
    }
}