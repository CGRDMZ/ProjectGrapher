using System;
using System.IO;

namespace ProjectGrapher
{
    internal class Program
    {
        public const int graphWidth = 40;
        public const int graphHeight = 25;

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

        private static void drawText(int x, int y, string text)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }

        private static char[,] loadGraph(string path)
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            char[,] graph = new char[graphHeight, graphWidth];
            string line = "";
            for (int row = 0; row < graphHeight; row++)
            {
                line = sr.ReadLine();
                for (int col = 0; col < graphWidth; col++)
                {
                    graph[row, col] = line[col];
                }
            }
            return graph;
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
            char[,] graph = new char[graphHeight, graphWidth];

            int nodeCount = 0;

            int option;

            int cursorX = 20;
            int cursorY = 24;

            int graphXOffset = 0;
            int graphYOffset = 0;

            int[,] rMatrix = new int[1, 1];
            int[,] nthMatrix = new int[1, 1];
            int nthMatrixInput = 1;

            Random rand = new Random();

            bool flag = true;

            ConsoleKeyInfo cki;
            ConsoleKeyInfo ckiDraw;
            ConsoleKeyInfo ckiCalc;
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
                Console.WriteLine("choose menu (1- draw, 2- calculation, 3- load)");
                cki = Console.ReadKey(true);
                option = (int)cki.Key;
                nthMatrixInput = 1;

                if (option == (int)ConsoleKey.D1)
                {
                    Console.Clear();

                    // drawing a graph
                    while (true)
                    {
                        drawGraph(graphXOffset, graphYOffset, graph);

                        // draws the cursor
                        Console.SetCursorPosition(graphXOffset + cursorX, graphYOffset + cursorY);
                        Console.Write("?");

                        ckiDraw = Console.ReadKey(true);

                        // going up
                        if (ckiDraw.Key == ConsoleKey.UpArrow)
                        {
                            cursorY--;
                        }
                        else
                        //going down
                        if (ckiDraw.Key == ConsoleKey.DownArrow)
                        {
                            cursorY++;
                        }
                        else
                        //going left
                        if (ckiDraw.Key == ConsoleKey.LeftArrow)
                        {
                            cursorX--;
                        }
                        else
                        //going down
                        if (ckiDraw.Key == ConsoleKey.RightArrow)
                        {
                            cursorX++;
                        }
                        else
                        //put an  arrow
                        if (ckiDraw.Key == ConsoleKey.Spacebar)
                        {
                            graph[cursorY, cursorX] = '+';
                        }
                        else
                        //put X which means the head of the arrow
                        if (ckiDraw.Key == ConsoleKey.X)
                        {
                            graph[cursorY, cursorX] = 'X';
                        }
                        else if (ckiDraw.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        else
                        {
                            graph[cursorY, cursorX] = char.ToUpper(ckiDraw.KeyChar);
                        }
                    }
                }
                else if (option == (int)ConsoleKey.D2) // calculations andd stuff
                {
                    Console.Clear();
                    flag = true;
                    while (true)
                    {
                        drawGraph(graphXOffset, graphYOffset, graph);
                        drawText(50, 4, $"r{nthMatrixInput} matrix is:");
                        drawGraph(50, 5, nthMatrix, 1);

                        ckiCalc = Console.ReadKey(true);

                        if (ckiCalc.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        else
                        {
                            nthMatrixInput = Convert.ToInt32(Int32.Parse(ckiCalc.KeyChar.ToString()));
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
                        if (flag)
                        {
                            // defining the r matricies
                            rMatrix = new int[nodeCount, nodeCount];
                            for (int row = 0; row < rMatrix.GetLength(0); row++)
                            {
                                for (int col = 0; col < rMatrix.GetLength(1); col++)
                                {
                                    // assigns one and zeros randomly for debugging
                                    rMatrix[row, col] = rand.Next(2);
                                }
                            }
                            flag = false;
                        }

                        nthMatrix = rMatrix;
                        // calculate the nth matrix
                        for (int i = 1; i < nthMatrixInput; i++)
                        {
                            nthMatrix = matrixMultipicator2D(nthMatrix, rMatrix);
                        }
                    }
                }
                else if (option == (int)ConsoleKey.D3)
                {
                    Console.Clear();
                    Console.WriteLine("please enter the name of the graph:");
                    string name = Console.ReadLine();
                    graph = loadGraph($"{name}.txt");
                }
            }
        }
    }
}