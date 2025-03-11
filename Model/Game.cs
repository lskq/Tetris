namespace Tetris.Model;

public class Game
{
    public const int Width = 10;
    public const int Height = 10;
    public const int XInit = Width / 2;
    public const int YInit = Height * 3 / 4;
    public const int Gravity = 1;

    public Random Random { get; }

    public bool[,] Grid { get; set; }
    public Tetromino Tetromino { get; set; }

    public Game()
    {
        Random = new();

        Grid = new bool[Width, Height];
        Tetromino = GetRandomTetromino();

        UpdateGrid(true);
    }

    public void Step(int xInput, int yInput)
    {
        UpdateGrid(false);

        if (yInput < 0)
        {
            Tetromino.Rotate();
            UpdateGrid(true);
            return;
        }

        Tetromino.X += xInput;
        // Tetromino.Y += (yInput > 0) ? Gravity + yInput : Gravity;
        UpdateGrid(true);
    }

    public void UpdateGrid(bool state)
    {
        foreach ((int, int) block in Tetromino.Blocks)
        {
            int x = Tetromino.X + block.Item1;
            int y = Tetromino.Y + block.Item2;

            Grid[x, y] = state;
        }
    }

    public Tetromino GetRandomTetromino()
    {
        int numShapes = Enum.GetNames(typeof(Shape)).Length;

        int rand = Random.Next(numShapes);

#pragma warning disable CS8605 // Unboxing a possibly null value.
        Shape shape = (Shape)Enum.GetValues(typeof(Shape)).GetValue(rand);
#pragma warning restore CS8605 // Unboxing a possibly null value.

        return new Tetromino(XInit, YInit, shape);
    }
}