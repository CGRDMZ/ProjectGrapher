﻿using System;
using System.IO;
using System.Threading;

namespace ProjectGrapher
{
    internal class Program
    {
        public const int graphWidth = 40;
        public const int graphHeight = 25;
        public static int graphX = 2;
        public static int graphY = 2;
        private const int MillisecondsTimeout = 100;
        public static int nodeCount = 0;
        public static char[] nodeNames = new char[1];
        public static int[,] rMatrix = new int[1, 1];

        private static void design(int graphHeight, int graphWidth)
        {
            int num = 0;
            for (int k = 0; k < graphHeight; k++)
            {
                Console.SetCursorPosition(0, 2 + k);
                Console.Write(num);
                num++;
                if (num == 10)
                    num = 0;
            }            
            num = 0;
            for (int l = 0; l < graphWidth; l++)
            {
                Console.SetCursorPosition(2 + l, 0);
                Console.Write(num);
                num++;
                if (num == 10)
                {
                    num = 0;
                }
                
            }
            for (int i = 0; i < graphHeight; i++)
            {
                Console.SetCursorPosition(1, 2 + i);
                Console.Write("#");
            }
            for (int i = 0; i < graphHeight; i++)
            {
                Console.SetCursorPosition(42, 2 + i);
                Console.Write("#");
            }
            for (int m = 0; m < graphWidth+2; m++)
            {
                Console.SetCursorPosition(1 + m, 1);
                Console.Write("#");
            }
            for (int m = 0; m < graphWidth+2; m++)
            {
                Console.SetCursorPosition(1 + m, 27);
                Console.Write("#");
            }


        }

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

            char[] initialNeighbours = new char[8];

            char[] breakPointNeighbours = new char[8];

            bool[,] isVisited = new bool[graph.GetLength(0), graph.GetLength(1)];
            // filling matrix all not visited initially
            for (int k = 0; k < isVisited.GetLength(0); k++)
            {
                for (int l = 0; l < isVisited.GetLength(1); l++)
                {
                    if (graph[k, l] == '.')
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

            bool found;

            int plusCount;
            int relationCount = 0;

            for (int row = 0; row < graph.GetLength(0); row++)
            {
                for (int col = 0; col < graph.GetLength(1); col++)
                {
                    if (graph[row, col] == nodeNames[i])
                    {
                        initialNeighbours = findNeighbours(row, col, graph);

                        trace = true;

                        for (int j = 0; j < initialNeighbours.Length; j++)
                        {
                            indexX = col;
                            indexY = row;

                            plusCount = 0;
                            for (int k = 0; k < initialNeighbours.Length; k++)
                            {
                                if (initialNeighbours[k] == '+')
                                {
                                    plusCount++;
                                }
                            }




                            while (trace)
                            {



                                found = false;

                                isVisited[indexY, indexX] = true;

                                if (graph[indexY, indexX + 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexX += 1;
                                    found = true;
                                }
                                if (graph[indexY - 1, indexX] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY -= 1;
                                    found = true;
                                }
                                if (graph[indexY, indexX - 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexX -= 1;
                                    found = true;
                                }
                                if (graph[indexY + 1, indexX] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY += 1;
                                    found = true;
                                }
                                if (graph[indexY - 1, indexX + 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY -= 1;
                                    indexX += 1;
                                    found = true;
                                }
                                if (graph[indexY - 1, indexX - 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY -= 1;
                                    indexX -= 1;
                                    found = true;
                                }
                                if (graph[indexY + 1, indexX + 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY += 1;
                                    indexX += 1;
                                    found = true;
                                }
                                if (graph[indexY + 1, indexX - 1] == 'X' && graph[indexY, indexX] != 'X' && !(graph[indexY, indexX] <= 'I' && graph[indexY, indexX] >= 'A'))
                                {
                                    indexY += 1;
                                    indexX -= 1;
                                    found = true;
                                }

                                drawGraph(graphX, graphY, graph);
                                drawGraph(80, 6, rMatrix, 1);
                                drawText(indexX + 2, indexY + 2, "!");
                                Thread.Sleep(MillisecondsTimeout);

                                if (found)
                                {
                                    nodeNeighbours = findNeighbours(indexY, indexX, graph);

                                    // add to r matrix
                                    for (int k = 0; k < nodeNeighbours.Length; k++)
                                    {
                                        for (int m = 0; m < nodeNames.Length; m++)
                                        {
                                            if (nodeNeighbours[k] <= 'I' && nodeNeighbours[k] >= 'A' && nodeNeighbours[k] == nodeNames[m])
                                            {
                                                rMatrix[i, m] = 1;
                                                //trace = false;
                                                break;
                                            }
                                        }
                                    }
                                    break;
                                }
                                relationCount = 0;
                                for (int k = 0; k < rMatrix.GetLength(1); k++)
                                {
                                    if (rMatrix[i, k] == 1)
                                    {
                                        relationCount++;
                                    }
                                }

                                if (!isVisited[indexY, indexX + 1])
                                {
                                    while (graph[indexY, indexX + 1] == '+') // going right
                                    {
                                        indexX += 1;
                                        isVisited[indexY, indexX] = true;

                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY - 1, indexX])
                                {
                                    while (graph[indexY - 1, indexX] == '+') // going up
                                    {
                                        indexY -= 1;
                                        isVisited[indexY, indexX] = true;

                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY, indexX - 1])
                                {
                                    while (graph[indexY, indexX - 1] == '+') // going left
                                    {
                                        indexX -= 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY + 1, indexX])
                                {
                                    while (graph[indexY + 1, indexX] == '+') // going down
                                    {
                                        indexY += 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY - 1, indexX + 1])
                                {
                                    while (graph[indexY - 1, indexX + 1] == '+') // going north east
                                    {
                                        indexY -= 1;
                                        indexX += 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY - 1, indexX - 1])
                                {
                                    while (graph[indexY - 1, indexX - 1] == '+') // going north west
                                    {
                                        indexY -= 1;
                                        indexX -= 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }

                                if (!isVisited[indexY + 1, indexX + 1])
                                {
                                    while (graph[indexY + 1, indexX + 1] == '+') // going south east
                                    {
                                        indexY += 1;
                                        indexX += 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }


                                if (!isVisited[indexY + 1, indexX - 1])
                                {
                                    while (graph[indexY + 1, indexX - 1] == '+' && graph[indexY, indexX - 1] != 'X') // going north west
                                    {
                                        indexY += 1;
                                        indexX -= 1;
                                        isVisited[indexY, indexX] = true;
                                        drawGraph(graphX, graphY, graph);
                                        drawGraph(80, 6, rMatrix, 1);
                                        drawText(indexX + 2, indexY + 2, "!");
                                        Thread.Sleep(MillisecondsTimeout);
                                    }
                                }


                                if (relationCount == plusCount)
                                {
                                    trace = false;
                                }


                                for (int k = 0; k < nodeNames.Length; k++)
                                {
                                    Console.SetCursorPosition(80 + k * 2, 5);
                                    Console.Write(nodeNames[k]);
                                }

                                for (int k = 0; k < nodeNames.Length; k++)
                                {
                                    Console.SetCursorPosition(78, 6 + k);
                                    Console.Write(nodeNames[k]);
                                }
                                drawGraph(graphX, graphY, graph);
                                drawText(indexX + 2, indexY + 2, "!");

                                Thread.Sleep(MillisecondsTimeout);
                            }
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

            int graphXOffset = 2;
            int graphYOffset = 2;

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

                    design(graphHeight, graphWidth);
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
                        char temp='~';
                        // we will make a function to achieve this.
                        //Array.Sort(nodeNames);
                        for (int i = 0; i < nodeNames.Length; i++)
                        {
                            if (nodeNames[i] != temp && nodeNames[i] < temp)
                            {
                                temp = nodeNames[i];
                                
                            }
                        }

                        if (flag)
                        {
                            rMatrix = findRelation(graph);
                            flag = false;
                        }


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