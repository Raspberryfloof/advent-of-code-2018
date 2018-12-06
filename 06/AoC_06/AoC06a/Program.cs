using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC06a
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine(string.Format("Usage: {0} <input>", Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)));
                Console.WriteLine("<input>: input file path");
                Environment.Exit(-1);
            }

            // Assume the first argument is a file name.
            try
            {
                StreamReader inputFile;
                int[,] coordGrid;
                List<coord> coords = new List<coord>();
                int minX = 0, minY = 0, maxX = 0, maxY = 0, greatestArea = 0, greatestIndex = -1;
                List<int> edgeIndices = new List<int>();

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                while (!inputFile.EndOfStream)
                {
                    string input = inputFile.ReadLine();
                    Regex inputRegex = new Regex(@"(?<x>\d+), (?<y>\d+)");
                    Match inputMatch = inputRegex.Match(input);
                    coords.Add(new coord(int.Parse(inputMatch.Groups["x"].Value), int.Parse(inputMatch.Groups["y"].Value)));
                }

                foreach (coord c in coords)
                {
                    if (c.x < minX)
                    {
                        minX = c.x;
                    }
                    if (c.y < minY)
                    {
                        minY = c.y;
                    }
                    if (c.x > maxX)
                    {
                        maxX = c.x;
                    }
                    if (c.y > maxY)
                    {
                        maxY = c.y;
                    }
                }

                coordGrid = new int[maxX + 1, maxY + 1];

                for (int gx = minX; gx <= maxX; gx++)
                {
                    for (int gy = minY; gy <= maxY; gy++)
                    {
                        coord closestCoord = new coord(-1, -1); // intentionally out of range
                        int manhattanDist = maxX + maxY;
                        foreach (coord c in coords)
                        {
                            int m = Manhattan(new coord(gx, gy), c);
                            if (m < manhattanDist)
                            {
                                manhattanDist = m;
                                closestCoord = c;
                            }
                            else if (m == manhattanDist)
                            {
                                closestCoord = new coord(-1, -1); // intentionally out of range
                            }
                        }
                        coordGrid[gx, gy] = coords.IndexOf(closestCoord);
                    }
                }

                for (int gx = minX; gx <= maxX; gx++)
                {
                    if ((!(coordGrid[gx, minY] == -1)) && (!edgeIndices.Contains(coordGrid[gx, minY])))
                    {
                        edgeIndices.Add(coordGrid[gx, minY]);
                    }
                    if ((!(coordGrid[gx, maxY] == -1)) && (!edgeIndices.Contains(coordGrid[gx, maxY])))
                    {
                        edgeIndices.Add(coordGrid[gx, maxY]);
                    }
                }
                for (int gy = minY; gy <= maxY; gy++)
                {
                    if ((!(coordGrid[minX, gy] == -1)) && (!edgeIndices.Contains(coordGrid[minX, gy])))
                    {
                        edgeIndices.Add(coordGrid[minX, gy]);
                    }
                    if ((!(coordGrid[maxX, gy] == -1)) && (!edgeIndices.Contains(coordGrid[maxX, gy])))
                    {
                        edgeIndices.Add(coordGrid[maxX, gy]);
                    }
                }

                foreach (coord c in coords)
                {
                    if (edgeIndices.Contains(coords.IndexOf(c)))
                    {
                        Console.WriteLine(string.Format("Point #{0} ({1},{2}): area was infinite (edge point)", coords.IndexOf(c), c.x, c.y));
                        // skip; area is infinite
                    }
                    else
                    {
                        int count = 0;
                        for (int gx = minX + 1; gx < maxX; gx++)
                        {
                            for (int gy = minY + 1; gy < maxY; gy++)
                            {
                                if (coordGrid[gx, gy] == coords.IndexOf(c))
                                {
                                    count++;
                                }
                            }
                        }
                        if (count > greatestArea)
                        {
                            greatestArea = count;
                            greatestIndex = coords.IndexOf(c);
                        }
                        Console.WriteLine(string.Format("Point #{0} ({1},{2}): area = {3}", coords.IndexOf(c), c.x, c.y, count));
                    }
                }

                Console.WriteLine(string.Format("Greatest area = {3}, around point #{0} ({1},{2}){4}", greatestIndex, coords[greatestIndex].x, coords[greatestIndex].y, greatestArea, Environment.NewLine));
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public struct coord
        {
            public int x, y;
            public coord(int inx, int iny)
            {
                x = inx;
                y = iny;
            }
        }

        public static int Manhattan(coord a, coord b)
        {
            return (Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y));
        }
    }
}
