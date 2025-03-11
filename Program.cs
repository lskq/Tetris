﻿namespace Tetris;

class Program
{

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            Controller controller = new();
            controller.Start();
        }
        catch (Exception e)
        {
            // For testing
            Console.Clear();
            Console.CursorVisible = true;
            Console.WriteLine(e.ToString());
        }
    }
}
