using System.Windows;
using Tetris.Model;

namespace Tetris.View;

public class ConsoleView
{
    public const string DefaultCode = "\x1b[0m";

    public Game Game { get; }
    public Mino[][] Grid { get; }

    public int ScoreTracker { get; set; }
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    
    public int Scale => (Console.WindowWidth / (Game.Width * 2) < Console.WindowHeight / Game.Height) ?
                            Console.WindowWidth / (Game.Width * 2) : Console.WindowHeight / Game.Height;
    public int GridWidthOffset => (Console.WindowWidth - Game.Width * 2 * Scale) / 2;
    public int GridHeightOffset => (Console.WindowHeight - Game.Height * Scale) / 2;

    public ConsoleView(Game game)
    {
        Game = game;
        Grid = game.Grid;
        ScoreTracker = game.Score;
        ScreenWidth = Console.WindowWidth;
        ScreenHeight = Console.WindowHeight;

        Console.CursorVisible = false;

        ValidateScreenSize();
        DrawScreen();
    }

    public void Step()
    {
        if (ScreenWidth != Console.WindowWidth || ScreenHeight != Console.WindowHeight)
        {
            ScreenWidth = Console.WindowWidth;
            ScreenHeight = Console.WindowHeight;
            
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
        
        DrawPixel(GridWidthOffset + x * 2 * Scale, GridHeightOffset + y * Scale, color);
    }

    public void DrawGameOver()
    {
        string message = "Game Over";

        int x = GridWidthOffset + (Game.Width * 2 * Scale) / 2 - message.Length / 2;
        int y = GridHeightOffset + (Game.Height * Scale) / 2;
        
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
        for (int y = 0; y < Game.Height * Scale; y++)
        {
            Console.SetCursorPosition(GridWidthOffset, GridHeightOffset + y);
            
            for (int x = 0; x < Game.Width * Scale; x++)
            {
                Console.Write(color + "  ");
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
        for (int y = 0; y < Scale; y++)
        {
            for (int x = 0; x < Scale; x++)
            {
                Console.SetCursorPosition(left + x, top + y);
                Console.Write(colorCode + "  ");
            }
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