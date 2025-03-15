namespace Tetris.Model;

public class Tetromino(int x, int y, Shape shape)
{
    public double XAbsolute { get; set; } = x;
    public double YAbsolute { get; set; } = y;

    public Mino[] Minoes { get; set; } = SetMinoes(shape);

    public void Rotate(bool right = true)
    {
        for (int i = 0; i < Minoes.Length; i++)
        {
            Mino mino = Minoes[i];

            int x = mino.XRelative;
            int y = mino.YRelative;

            int newX;
            int newY;

            if (right)
            {
                newX = -y - 1;
                newY = x;
            }
            else
            {
                newX = y;
                newY = -x - 1;
            }

            mino.XRelative = newX;
            mino.YRelative = newY;
        }
    }

    public static Mino[] SetMinoes(Shape shape)
    {
        MinoColor minoColor = MinoColor.Cyan;
        (int, int)[] coords = new (int, int)[4];

        // Shapes are designed with the console in mind, so top left is (0,0)
        switch (shape)
        {
            case Shape.I:
                // ####
                minoColor = MinoColor.Cyan;
                coords[0] = (-2, -1);
                coords[1] = (-1, -1);
                coords[2] = (0, -1);
                coords[3] = (1, -1);
                break;

            case Shape.J:
                // #
                // ###
                minoColor = MinoColor.Blue;
                coords[0] = (-2, -2);
                coords[1] = (-2, -1);
                coords[2] = (-1, -1);
                coords[3] = (0, -1);
                break;

            case Shape.L:
                //   #
                // ###
                minoColor = MinoColor.Orange;
                coords[0] = (-2, -1);
                coords[1] = (-1, -1);
                coords[2] = (0, -1);
                coords[3] = (0, -2);
                break;

            case Shape.O:
                // ##
                // ##
                minoColor = MinoColor.Yellow;
                coords[0] = (-1, -1);
                coords[1] = (0, -1);
                coords[2] = (-1, 0);
                coords[3] = (0, 0);
                break;

            case Shape.S:
                //  ##
                // ##
                minoColor = MinoColor.Green;
                coords[0] = (-2, -1);
                coords[1] = (-1, -1);
                coords[2] = (-1, -2);
                coords[3] = (0, -2);
                break;

            case Shape.T:
                // ###
                //  #
                minoColor = MinoColor.Purple;
                coords[0] = (-1, -2);
                coords[1] = (-2, -1);
                coords[2] = (-1, -1);
                coords[3] = (0, -1);
                break;

            case Shape.Z:
                // ##
                //  ##
                minoColor = MinoColor.Red;
                coords[0] = (-2, -2);
                coords[1] = (-1, -2);
                coords[2] = (-1, -1);
                coords[3] = (0, -1);
                break;
        }

        Mino[] minoes = new Mino[4];

        for (int i = 0; i < 4; i++)
        {
            minoes[i] = new Mino(coords[i].Item1, coords[i].Item2, minoColor);
        }

        return minoes;
    }
}