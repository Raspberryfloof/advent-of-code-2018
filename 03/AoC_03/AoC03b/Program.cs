using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC03b
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
                List<int>[,] sqftAllotments = new List<int>[1024, 1024];
                Dictionary<int, clipValues> claims = new Dictionary<int, clipValues>();

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

                for (int x = 0; x < 1024; x++)
                {
                    for (int y = 0; y < 1024; y++)
                    {
                        sqftAllotments[x, y] = new List<int>();
                    }
                }

                foreach (KeyValuePair<int, clipValues> thisClaim in claims)
                {
                    for (int cx = thisClaim.Value.x; cx < (thisClaim.Value.x + thisClaim.Value.width); cx++)
                    {
                        for (int cy = thisClaim.Value.y; cy < (thisClaim.Value.y + thisClaim.Value.height); cy++)
                        {
                            sqftAllotments[cx, cy].Add(thisClaim.Key);
                            if (sqftAllotments[cx,cy].Count > 1)
                            {
                                foreach (int claimID in sqftAllotments[cx,cy])
                                {
                                    claims[claimID].overlapping = true;
                                }
                            }
                        }
                    }

                    Console.WriteLine(string.Format("Claim #{0} @ {1},{2}: {3}x{4}", thisClaim.Key, thisClaim.Value.x, thisClaim.Value.y, thisClaim.Value.width, thisClaim.Value.height));
                }

                foreach (KeyValuePair<int, clipValues> thisClaim in claims)
                {
                    if (!(thisClaim.Value.overlapping))
                    {
                        Console.WriteLine(string.Format("Non-overlapping claim: {0} (@ {1},{2}: {3}x{4}){5}", thisClaim.Key, thisClaim.Value.x, thisClaim.Value.y, thisClaim.Value.width, thisClaim.Value.height, Environment.NewLine));
                        Environment.Exit(0);
                    }
                }

                Console.WriteLine("No overlapping claims?!");
                Console.WriteLine();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public class clipValues
        {
            public int x { get; }
            public int y { get; }
            public int width { get; }
            public int height { get; }
            public bool overlapping { get; set; }

            public clipValues(int a, int b, int c, int d)
            {
                x = a;
                y = b;
                width = c;
                height = d;
                overlapping = false;
            }
        }
    }
}
