using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC06b
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: {0} <input>", Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName));
                Console.WriteLine("<input>: input file path");
                Environment.Exit(-1);
            }

            // Assume the first argument is a file name.
            try
            {
                StreamReader inputFile;
                int[,] coordGrid;
                List<coord> coords = new List<coord>();
                int maxX = 0, maxY = 0, area = 0;
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
                    if (c.x > maxX)
                    {
                        maxX = c.x;
                    }
                    if (c.y > maxY)
                    {
                        maxY = c.y;
                    }
                }

                maxX *= 2;
                maxY *= 2;
                coordGrid = new int[maxX, maxY];

                for (int gx = 0; gx < maxX; gx++)
                {
                    for (int gy = 0; gy < maxY; gy++)
                    {
                        foreach (coord c in coords)
                        {
                            coordGrid[gx, gy] += Manhattan(new coord(gx, gy), c);
                        }
                    }
                }

                for (int gx = 0; gx < maxX; gx++)
                {
                    if ((coordGrid[gx, 0] < 10000) || (coordGrid[gx, maxY - 1] < 10000))
                    {
                        throw new Exception("Error: area not bounded by grid size!!");
                    }
                }
                for (int gy = 0; gy < maxY; gy++)
                {
                    if ((coordGrid[0, gy] < 10000) || (coordGrid[maxX - 1, gy] < 10000))
                    {
                        throw new Exception("Error: area not bounded by grid size!!");
                    }
                }

                for (int gx = 0; gx < maxX; gx++)
                {
                    for (int gy = 0; gy < maxY; gy++)
                    {
                        if (coordGrid[gx, gy] < 10000)
                        {
                            area++;
                        }
                    }
                }

                Console.WriteLine("Total area under 10000 units from all points: {0}{1}", area, Environment.NewLine);
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
