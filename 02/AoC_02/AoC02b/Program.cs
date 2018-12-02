using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC02b
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
                List<string> boxIDs = new List<string>();

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
                    boxIDs.Add(inputFile.ReadLine());
                }

                // Assume every box ID is an identical length.
                for (int count = 0; count < boxIDs[0].Length; count++)
                {
                    List<string> checkIDs = new List<string>();
                    foreach(string id in boxIDs)
                    {
                        checkIDs.Add(id.Remove(count, 1));
                    }

                    var duplicates = checkIDs.GroupBy(id => id).Where(id => id.Count() > 1).Select(id => id.Key).ToList();
                    if ((duplicates == null) || (duplicates.Count < 1))
                    {
                        Console.WriteLine(string.Format("Testing character {0}: no valid IDs found", count));
                    }
                    else
                    {
                        Console.WriteLine(string.Format("Testing character {0}: valid ID found!", count));
                        Console.WriteLine(string.Format("Remaining characters: {0}", duplicates[0].ToString()));
                        Environment.Exit(0);
                    }
                }

                Console.WriteLine("Error: No valid IDs found?!");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }
    }
}
