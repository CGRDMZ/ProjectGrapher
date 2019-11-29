using System;

namespace ProjectGrapher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            char[,] graph = new char[25, 40];
            int option;
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    graph[i, j] = '.';
                }
            }

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    Console.Write(graph[i, j]);
                }
                Console.Write("\n");
            }

            int cursorX = 20;
            int cursorY = 24;

            ConsoleKeyInfo cki;

            // drawing the graph

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);

                    // going up
                    if (cki.Key == ConsoleKey.UpArrow)
                    {
                        cursorY--;
                    }
                    //going down
                    if (cki.Key == ConsoleKey.DownArrow)
                    {
                        cursorY++;
                    }
                    //going left
                    if (cki.Key == ConsoleKey.LeftArrow)
                    {
                        cursorX--;
                    }
                    //going down
                    if (cki.Key == ConsoleKey.RightArrow)
                    {
                        cursorX++;
                        // yorum bu
                    }
                }
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }

                graph[cursorY, cursorX] = '+';

                // draw graph every loop
                Console.SetCursorPosition(0, 0);
                Console.CursorVisible = false;
                for (int i = 0; i < graph.GetLength(0); i++)
                {
                    for (int j = 0; j < graph.GetLength(1); j++)
                    {
                        Console.Write(graph[i, j]);
                    }
                    Console.Write("\n");
                }
                Console.CursorVisible = true;
                Console.SetCursorPosition(cursorX, cursorY);
            }

            Console.ReadLine();
        }
    }
}