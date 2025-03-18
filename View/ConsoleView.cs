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
    public int MainPanelXOffset { get; set; } = 0;
    public int MainPanelYOffset { get; set; } = 0;
    public int NextPanelXOffset { get; set; } = 0;
    public int NextPanelYOffset { get; set; } = 0;

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

            if (Game.Switching)
            {
                DrawNextPanel();
            }

            if (Game.Score != ScoreTracker)
            {
                DrawScore();
            }

            if (Game.GameOver)
            {
                DrawGameOver();
            }
            else
            {
                DrawGrid();
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

    public void UpdateNextPanel()
    {
        foreach (Mino mino in Game.NextTetromino.Minoes)
        {
            string color = GetMinoColor(mino.MinoColor);
            DrawPixel(NextPanelXOffset + (2 + mino.XRelative) * XScale, NextPanelYOffset + (2 + mino.YRelative) * YScale, color);
        }
    }

    public void DrawGrid()
    {

        for (int y = 0; y < Game.Height; y++)
        {
            string row = "";

            for (int x = 0; x < Game.Width; x++)
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

                for (int i = 0; i < XScale; i++)
                {
                    row += color + " ";
                }
            }

            for (int i = 0; i < YScale; i++)
            {
                Console.SetCursorPosition(GridXOffset, GridYOffset + i + y * YScale);
                Console.Write(row);
            }
        }
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
        DrawMainPanel();
        DrawNextPanel();
        DrawScore();

        if (Game.GameOver)
        {
            DrawGrid();
        }
    }

    public void DrawScore()
    {
        string score = "Score: " + Game.Score;

        Console.SetCursorPosition(NextPanelXOffset + 2 * XScale - (score.Length / 2), NextPanelYOffset + 6 * YScale - 1);
        Console.Write(ConsoleColor.Panel + score);
    }

    public void DrawNextPanel()
    {
        for (int y = NextPanelYOffset; y < NextPanelYOffset + 4 * YScale; y++)
        {
            Console.SetCursorPosition(NextPanelXOffset, y);
            string row = ConsoleColor.Grid;
            for (int x = 0; x < 4 * XScale; x++)
            {
                row += " ";
            }
            Console.Write(row);
        }

        Console.SetCursorPosition(NextPanelXOffset + 2 * XScale - 2, NextPanelYOffset + 4 * YScale - 1);
        Console.Write("NEXT");

        UpdateNextPanel();
    }

    public void DrawMainPanel()
    {
        for (int y = 0; y < Game.Height * YScale; y++)
        {
            Console.SetCursorPosition(MainPanelXOffset, MainPanelYOffset + y);
            string row = ConsoleColor.Panel;
            for (int x = 0; x < Game.Width * XScale; x++)
            {
                row += " ";
            }
            Console.Write(row);
        }
    }

    public void DrawBackground()
    {
        for (int y = 0; y < Console.WindowHeight; y++)
        {
            Console.SetCursorPosition(0, y);
            string row = ConsoleColor.Background;
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                row += " ";
            }
            Console.Write(row);
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
            int xRatio = Console.WindowWidth / (Game.Width * 2);
            int yRatio = Console.WindowHeight / Game.Height;

            YScale = xRatio < yRatio ? xRatio : yRatio;
            XScale = YScale * 2;
        }

        if (Offsetable)
        {
            GridXOffset = (Console.WindowWidth - 2 * Game.Width * XScale) / 2;
            GridYOffset = (Console.WindowHeight - Game.Height * YScale) / 2;
        }

        MainPanelXOffset = GridXOffset + Game.Width * XScale;
        MainPanelYOffset = GridYOffset;

        NextPanelXOffset = MainPanelXOffset + Game.Width * XScale / 3;
        NextPanelYOffset = MainPanelYOffset + 1 * YScale;
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