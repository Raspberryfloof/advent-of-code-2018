using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC03a
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
                int[,] sqftAllotments = new int[1024, 1024];
                Dictionary<int, clipValues> claims = new Dictionary<int, clipValues>();
                int dupClaims = 0;

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
                    string line = inputFile.ReadLine();
                    int[] parseIntArray = new Regex(@"\d+").Matches(line).Cast<Match>().Select(num => Int32.Parse(num.Value)).ToArray();

                    // Note that if this fails, the input was in an incorrect format somehow
                    claims.Add(parseIntArray[0], new clipValues(parseIntArray[1], parseIntArray[2], parseIntArray[3], parseIntArray[4]));
                }

                foreach (KeyValuePair<int, clipValues> thisClaim in claims)
                {
                    for (int cx = thisClaim.Value.x; cx < (thisClaim.Value.x + thisClaim.Value.width); cx++)
                    {
                        for (int cy = thisClaim.Value.y; cy < (thisClaim.Value.y + thisClaim.Value.height); cy++)
                        {
                            sqftAllotments[cx, cy]++;
                        }
                    }

                    Console.WriteLine("Claim #{0} @ {1},{2}: {3}x{4}", thisClaim.Key, thisClaim.Value.x, thisClaim.Value.y, thisClaim.Value.width, thisClaim.Value.height);
                }

                for (int x = 0; x < 1024; x++)
                {
                    for (int y = 0; y < 1024; y++)
                    {
                        if (sqftAllotments[x,y] > 1)
                        {
                            dupClaims++;
                        }
                    }
                }
                
                Console.WriteLine("Duplicate claim area: {0} sq.ft{1}", dupClaims, Environment.NewLine);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public struct clipValues
        {
            public int x, y, width, height;

            public clipValues(int a, int b, int c, int d)
            {
                x = a;
                y = b;
                width = c;
                height = d;
            }
        }
    }
}
