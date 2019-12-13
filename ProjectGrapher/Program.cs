﻿using System;
using System.IO;
using System.Threading;

namespace ProjectGrapher
{
    internal class Program
    {
        public const int graphWidth = 40;
        public const int graphHeight = 25;

        public static int nodeCount = 0;
        public static char[] nodeNames = new char[1];

        public static int[,] rMatrix = new int[1, 1];

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

        private static char[] findNeighbours(int row, int col, char[,] graph, bool diagonal = true)
        {
            char[] neighbours = new char[8];
            // neighbours are added like that
            // 0 1 2
            // 3 N 4
            // 5 6 7

            int counter = 0;
            for (int k = -1; k < 2; k++)
            {
                for (int l = -1; l < 2; l++)
                {
                    if (diagonal)
                    {
                        if (!(k == 0 && l == 0) && row + k > 0 && row + k < graph.GetLength(0) && col + l > 0 && col + l < graph.GetLength(1))
                        {
                            neighbours[counter] = graph[row + k, col + l];
                            if (counter < neighbours.Length - 1)
                            {
                                counter++;
                            }
                        }
                    }
                    else
                    {
                        if (!(k == 0 && l == 0) && row + k > 0 && row + k < graph.GetLength(0) && col + l > 0 && col + l < graph.GetLength(1))
                        {
                            if ((k == -1 && l == -1) || (k == -1 && l == 1) || (k == 1 && l == -1) || (k == 1 && l == 1))
                            {
                                neighbours[counter] = '0';
                            }
                            else
                            {
                                neighbours[counter] = graph[row + k, col + l];
                            }
                            if (counter < neighbours.Length - 1)
                            {
                                counter++;
                            }
                        }
                    }
                }
            }
            return neighbours;
        }

        private static int[,] findRelation(char[,] graph)
        {
            int[,] rMatrix = new int[nodeCount, nodeCount];

            // filling the matrix with zeros
            for (int j = 0; j < rMatrix.GetLength(0); j++)
            {
                for (int k = 0; k < rMatrix.GetLength(1); k++)
                {
                    rMatrix[j, k] = 0;
                }
            }

            int i = 0;
            char node;
            char[] nodeNeighbours = new char[8];
            char[] breakPointNeighbours = new char[8];

            bool[,] isVisited = new bool[graph.GetLength(0), graph.GetLength(1)];
            // filling matrix all not visited initially
            for (int k = 0; k < isVisited.GetLength(0); k++)
            {
                for (int l = 0; l < isVisited.GetLength(1); l++)
                {
                    
                    if (graph[k,l] == '.')
                    {
                        isVisited[k, l] = true;
                    }
                    else
                    {
                        isVisited[k, l] = false;
                    }
                }
            }

            int indexX;
            int indexY;

            int xDiff;
            int yDiff;

            bool trace;


            for (int row = 0; row < graph.GetLength(0); row++)
            {
                for (int col = 0; col < graph.GetLength(1); col++)
                {
                    if (graph[row, col] == nodeNames[i])
                    {
                        node = nodeNames[i];

                        nodeNeighbours = findNeighbours(row, col, graph);

                        int prevX;
                        int prevY;

                        indexX = col;
                        indexY = row;

                        xDiff = 0;
                        yDiff = 0;

                        trace = true;

                        // tracing algorithm
                        // TODO: tracing algorithm works fine for direct lines, now it should detect breakpoints and change direction to find more complex relations

                        while (trace)
                        {
                            
                            if (graph[indexY + yDiff, indexX + xDiff] != '+')
                            {
                                nodeNeighbours = findNeighbours(indexY, indexX, graph);
                                // we can count number of +'s to give an error, if more than 2, than exception
                            }

                            for (int k = 0; k < nodeNeighbours.Length; k++)
                            {
                                if (nodeNeighbours[k] <= 'I' && nodeNeighbours[k] >= 'A')
                                {
                                    rMatrix[i, k] = 1;
                                    trace = false;
                                    break;
                                }
                            }

                            for (int k = 0; k < nodeNeighbours.Length; k++)
                            {
                                prevX = xDiff;
                                prevY = yDiff;
                                if (nodeNeighbours[k] == '+')
                                {
                                    if (k == 0)
                                    {
                                        xDiff = -1;
                                        yDiff = -1;
                                    }
                                    else if (k == 1)
                                    {
                                        xDiff = 0;
                                        yDiff = -1;
                                    }
                                    else if (k == 2)
                                    {
                                        xDiff = 1;
                                        yDiff = -1;
                                    }
                                    else if (k == 3)
                                    {
                                        xDiff = -1;
                                        yDiff = 0;
                                    }
                                    else if (k == 4)
                                    {
                                        xDiff = 1;
                                        yDiff = 0;
                                    }
                                    else if (k == 5)
                                    {
                                        xDiff = -1;
                                        yDiff = 1;
                                    }
                                    else if (k == 6)
                                    {
                                        xDiff = 0;
                                        yDiff = 1;
                                    }
                                    else if (k == 7)
                                    {
                                        xDiff = 1;
                                        yDiff = 1;
                                    }
                                    if (isVisited[indexY + yDiff, indexX + xDiff])
                                    {
                                        xDiff = prevX;
                                        yDiff = prevY;
                                    }
                                }
                            }
                            //Console.Clear();
                            isVisited[indexY, indexX] = true;
                            indexX += xDiff;
                            indexY += yDiff;

                            //drawGraph(0, 0, graph);
                            //drawText(indexX, indexY, "!");
                            //Thread.Sleep(400);
                        }

                        if (i < nodeNames.Length - 1)
                        {
                            i++;
                            row = 0;
                            col = 0;
                        }
                        break;
                    }
                }
            }
            return rMatrix;
        }

        private static void Main(string[] args)
        {
            char[,] graph = new char[graphHeight, graphWidth];

            int option;

            int cursorX = 20;
            int cursorY = 24;

            int graphXOffset = 0;
            int graphYOffset = 0;

            int[,] nthMatrix = new int[1, 1];
            int[,] rStarMatrix = new int[1, 1];

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
                        //counting node number
                        nodeCount = 0;
                        for (int row = 0; row < graph.GetLength(0); row++)
                        {
                            for (int col = 0; col < graph.GetLength(1); col++)
                            {
                                if (graph[row, col] != '.' && graph[row, col] != 'X' && graph[row, col] != '+' && graph[row, col] <= 'I' && graph[row, col] >= 'A')
                                {
                                    nodeCount++;
                                }
                            }
                        }

                        // find nodes
                        nodeNames = new char[nodeCount];
                        int counter = 0;
                        for (int row = 0; row < graph.GetLength(0); row++)
                        {
                            for (int col = 0; col < graph.GetLength(1); col++)
                            {
                                if (graph[row, col] != '.' && graph[row, col] != 'X' && graph[row, col] != '+' && graph[row, col] <= 'I' && graph[row, col] >= 'A')
                                {
                                    nodeNames[counter] = graph[row, col];
                                    counter++;
                                }
                            }
                        }

                        // we will make a function to achieve this.
                        Array.Sort(nodeNames);

                        //if (flag)
                        //{
                        //    // defining the r matricies
                        //    rMatrix = new int[nodeCount, nodeCount];
                        //    for (int row = 0; row < rMatrix.GetLength(0); row++)
                        //    {
                        //        for (int col = 0; col < rMatrix.GetLength(1); col++)
                        //        {
                        //            // assigns one and zeros randomly for debugging
                        //            rMatrix[row, col] = rand.Next(2);
                        //        }
                        //    }
                        //    flag = false;
                        //}

                        rMatrix = findRelation(graph);
                        flag = false;

                        // calculate r star
                        rStarMatrix = new int[nodeCount, nodeCount];
                        nthMatrix = new int[nodeCount, nodeCount];

                        for (int i = 0; i < rMatrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < rMatrix.GetLength(1); j++)
                            {
                                nthMatrix[i, j] = rMatrix[i, j];
                            }
                        }
                        for (int i = 0; i < nodeCount; i++)
                        {
                            for (int row = 0; row < rStarMatrix.GetLength(0); row++)
                            {
                                for (int col = 0; col < rStarMatrix.GetLength(1); col++)
                                {
                                    if (nthMatrix[row, col] == 1)
                                    {
                                        rStarMatrix[row, col] = 1;
                                    }
                                }
                            }
                            nthMatrix = matrixMultipicator2D(nthMatrix, rMatrix);
                        }

                        // calculate the nth matrix
                        for (int i = 0; i < rMatrix.GetLength(0); i++)
                        {
                            for (int j = 0; j < rMatrix.GetLength(1); j++)
                            {
                                nthMatrix[i, j] = rMatrix[i, j];
                            }
                        }
                        for (int i = 1; i < nthMatrixInput; i++)
                        {
                            nthMatrix = matrixMultipicator2D(nthMatrix, rMatrix);
                        }

                        drawGraph(graphXOffset, graphYOffset, graph);

                        for (int i = 0; i < nodeNames.Length; i++)
                        {
                            Console.SetCursorPosition(80 + i * 2, 5);
                            Console.Write(nodeNames[i]);
                        }

                        for (int i = 0; i < nodeNames.Length; i++)
                        {
                            Console.SetCursorPosition(78, 6 + i);
                            Console.Write(nodeNames[i]);
                        }

                        for (int i = 0; i < nodeNames.Length; i++)
                        {
                            Console.SetCursorPosition(80 + i * 2, 16);
                            Console.Write(nodeNames[i]);
                        }

                        for (int i = 0; i < nodeNames.Length; i++)
                        {
                            Console.SetCursorPosition(78, 17 + i);
                            Console.Write(nodeNames[i]);
                        }

                        drawText(80, 4, $"r{nthMatrixInput} matrix is:");
                        drawGraph(80, 6, nthMatrix, 1);

                        drawText(80, 15, "r* matrix is:");
                        drawGraph(80, 17, rStarMatrix, 1);

                        ckiCalc = Console.ReadKey(true);

                        if (ckiCalc.Key == ConsoleKey.Escape)
                        {
                            break;
                        }
                        else
                        {
                            nthMatrixInput = Convert.ToInt32(int.Parse(ckiCalc.KeyChar.ToString()));
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