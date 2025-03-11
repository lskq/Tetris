namespace Tetris.Model;

public class Tetromino
{
    public Color Color { get; set; }
    public (int, int)[] Blocks { get; set; }

    public Tetromino(Shape shape)
    {
        Blocks = new (int, int)[4];

        // Shapes are designed with the console in mind, so top left is (0,0)
        switch (shape)
        {
            case Shape.I:
                // ####
                Color = Color.Cyan;
                Blocks[0] = (-2, -1);
                Blocks[1] = (-1, -1);
                Blocks[2] = (0, -1);
                Blocks[3] = (1, -1);
                break;

            case Shape.J:
                // #
                // ###
                Color = Color.Blue;
                Blocks[0] = (-2, -2);
                Blocks[1] = (-2, -1);
                Blocks[2] = (-1, -1);
                Blocks[3] = (0, -1);
                break;

            case Shape.L:
                //   #
                // ###
                Color = Color.Orange;
                Blocks[0] = (-2, -1);
                Blocks[1] = (-1, -1);
                Blocks[2] = (0, -1);
                Blocks[3] = (0, -2);
                break;

            case Shape.O:
                // ##
                // ##
                Color = Color.Yellow;
                Blocks[0] = (-1, -1);
                Blocks[1] = (0, -1);
                Blocks[2] = (-1, 0);
                Blocks[3] = (0, 0);
                break;

            case Shape.S:
                //  ##
                // ##
                Color = Color.Green;
                Blocks[0] = (-2, -1);
                Blocks[1] = (-1, -1);
                Blocks[2] = (-1, -2);
                Blocks[3] = (0, -2);
                break;

            case Shape.T:
                // ###
                //  #
                Color = Color.Purple;
                Blocks[0] = (-1, -2);
                Blocks[1] = (-2, -1);
                Blocks[2] = (-1, -1);
                Blocks[3] = (0, -1);
                break;

            case Shape.Z:
                // ##
                //  ##
                Color = Color.Red;
                Blocks[0] = (-2, -2);
                Blocks[1] = (-1, -2);
                Blocks[2] = (-1, -1);
                Blocks[3] = (0, -1);
                break;
        }
    }
    public void Rotate()
    {
        for (int i = 0; i < Blocks.Length; i++)
        {
            int x = Blocks[i].Item1;
            int y = Blocks[i].Item2;

            if (x < 0 && y < 0 || x >= 0 && y >= 0)
            {
                x *= -1;
            }
            else
            {
                y *= -1;
            }

            Blocks[i] = (x, y);
        }
    }

}