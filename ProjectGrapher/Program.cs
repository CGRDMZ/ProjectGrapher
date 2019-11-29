using System;
using System.Threading;

namespace ProjectGrapher
{
    internal class Program
    {
        private static void drawGraph(int x, int y, dynamic array, int emptySpace = 0)
        {
            // draw graph every loop
            for (int row = 0; row < array.GetLength(0); row++)
            {
                Console.SetCursorPosition(x, y + row);
                for (int col = 0; col < array.GetLength(1); col++)
                {
                    Console.Write(array[row, col]);
                    for (int i = 0; i < emptySpace; i++)
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
        }

        private static int[,] matrixMultipicator2D(int[,] matrix1, int[,] matrix2)
        {
            int[,] matrix3 = new int[matrix1.GetLength(0), matrix2.GetLength(1)];

            for (int row = 0; row < matrix3.GetLength(0); row++)
            {
                for (int col = 0; col < matrix3.GetLength(1); col++)
                {
                    for (int i = 0; i < matrix1.GetLength(1); i++)
                    {
                        matrix3[row, col] += matrix1[row, i] * matrix2[i, col];
                        if (matrix3[row, col] > 0)
                        {
                            matrix3[row, col] = 1;
                        }
                    }
                }
            }
            return matrix3;
        }

        private static void Main(string[] args)
        {
            char[,] graph = new char[25, 40];

            int nodeCount = 0;

            int option;

            int cursorX = 20;
            int cursorY = 24;

            int graphXOffset = 0;
            int graphYOffset = 0;

            int nthMatrix;

            Random rand = new Random();

            ConsoleKeyInfo cki;
            Console.CursorVisible = false;

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                for (int j = 0; j < graph.GetLength(1); j++)
                {
                    graph[i, j] = '.';
                }
            }

            //for (int i = 0; i < graph.GetLength(0); i++)
            //{
            //    for (int j = 0; j < graph.GetLength(1); j++)
            //    {
            //        Console.Write(graph[i, j]);
            //    }
            //    Console.Write("\n");
            //}

            while (true)
            {
                Console.Clear();
                Console.WriteLine("choose menu (1- draw, 2- calculation)");
                cki = Console.ReadKey(true);
                option = (int)cki.Key;
                nthMatrix = 1;

                if (option == (int)ConsoleKey.D1)
                {
                    // drawing a graph
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
                            else
                            //going down
                            if (cki.Key == ConsoleKey.DownArrow)
                            {
                                cursorY++;
                            }
                            else
                            //going left
                            if (cki.Key == ConsoleKey.LeftArrow)
                            {
                                cursorX--;
                            }
                            else
                            //going down
                            if (cki.Key == ConsoleKey.RightArrow)
                            {
                                cursorX++;
                            }
                            else
                            //put an  arrow
                            if (cki.Key == ConsoleKey.Spacebar)
                            {
                                graph[cursorY, cursorX] = '+';
                            }
                            else
                            //put X which means the head of the arrow
                            if (cki.Key == ConsoleKey.X)
                            {
                                graph[cursorY, cursorX] = 'X';
                            }
                            else if (cki.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                            else
                            {
                                graph[cursorY, cursorX] = char.ToUpper(cki.KeyChar);
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(true);
                            }
                        }

                        drawGraph(graphXOffset, graphYOffset, graph);

                        // draws the cursor
                        Console.SetCursorPosition(graphXOffset + cursorX, graphYOffset + cursorY);
                        Console.Write("?");

                        Thread.Sleep(100);
                    }
                }
                else if (option == (int)ConsoleKey.D2) // calculations andd stuff
                {
                    while (true)
                    {
                        if (Console.KeyAvailable)
                        {
                            cki = Console.ReadKey(true);

                            if (cki.Key == ConsoleKey.Escape)
                            {
                                break;
                            }
                            else
                            {
                                nthMatrix = (int)cki.KeyChar;
                            }
                            while (Console.KeyAvailable)
                            {
                                Console.ReadKey(true);
                            }
                        }

                        //counting node number
                        nodeCount = 0;
                        for (int row = 0; row < graph.GetLength(0); row++)
                        {
                            for (int col = 0; col < graph.GetLength(1); col++)
                            {
                                if (graph[row, col] != '.' && graph[row, col] != 'X' && graph[row, col] != '+')
                                {
                                    nodeCount++;
                                }
                            }
                        }

                        // defining the r matricies
                        int[,] rMatrix = new int[nodeCount, nodeCount];
                        for (int row = 0; row < rMatrix.GetLength(0); row++)
                        {
                            for (int col = 0; col < rMatrix.GetLength(1); col++)
                            {
                                // assigns one and zeros randomly for debugging
                                if (rand.Next(2) == 0)
                                {
                                    rMatrix[row, col] = 0;
                                }
                                else
                                {
                                    rMatrix[row, col] = 1;
                                }
                            }
                        }




                        drawGraph(50, 5, rMatrix, 1);
                    }
                    
                }
            }
        }
    }
}