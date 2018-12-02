using System;
using System.IO;
using System.Linq;

namespace AoC02a
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
                int doubleLetter = 0, tripleLetter = 0, checksum = 0;
                int count = 1;

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
                    bool hasDouble = false, hasTriple = false;
                    string boxID = inputFile.ReadLine();
                    foreach(var map in boxID.ToCharArray().GroupBy(c => c))
                    {
                        if (map.Count() == 2)
                        {
                            hasDouble = true;
                        }
                        if (map.Count() == 3)
                        {
                            hasTriple = true;
                        }
                    }
                    if (hasDouble)
                    {
                        doubleLetter++;
                    }
                    if (hasTriple)
                    {
                        tripleLetter++;
                    }

                    Console.WriteLine(string.Format("Line {0}: {1}, {2}", count, hasDouble ? "double" : "no double", hasTriple ? "triple" : "no triple"));

                    count++;
                }
                Console.WriteLine();

                checksum = doubleLetter * tripleLetter;

                Console.WriteLine(string.Format("Doubles: {0}; Triples: {1}", doubleLetter, tripleLetter));
                Console.WriteLine(string.Format("Checksum: {0}", checksum));
                Console.WriteLine();

                Environment.Exit(0);
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
