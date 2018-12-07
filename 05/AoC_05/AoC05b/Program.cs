using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC05b
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
                string initialPolymer = "", input = "", finalPolymer = "", bestPolymer = "", bestCharacter = "";
                List<string> loupReplacements = new List<string>();
                List<string> uploReplacements = new List<string>();
                Regex loupRegex, uploRegex;

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                for (char c = 'a'; c <= 'z'; c++)
                {
                    string loupKey = c.ToString() + c.ToString().ToUpper();
                    string uploKey = c.ToString().ToUpper() + c.ToString();
                    loupReplacements.Add(loupKey);
                    uploReplacements.Add(uploKey);
                }

                loupRegex = new Regex(string.Join("|", loupReplacements));
                uploRegex = new Regex(string.Join("|", uploReplacements));

                initialPolymer = inputFile.ReadLine();
                bestPolymer = initialPolymer; // Since it will never be worse than the unreacted input

                for (char c = 'a'; c <= 'z'; c++)
                {
                    Console.WriteLine("Testing with {0} removed:", c);
                    Regex charRemover = new Regex(string.Format("{0}|{1}", c, c.ToString().ToUpper()));
                    finalPolymer = charRemover.Replace(initialPolymer, "");
                    do
                    {
                        input = finalPolymer;
                        finalPolymer = loupRegex.Replace(input, "");
                        finalPolymer = uploRegex.Replace(finalPolymer, "");
                    } while (finalPolymer != input);
                    Console.WriteLine("Polymer length: {0}", finalPolymer.Length);
                    if (finalPolymer.Length < bestPolymer.Length)
                    {
                        bestPolymer = finalPolymer;
                        bestCharacter = c.ToString();
                    }
                }

                Console.WriteLine("Best polymer:{0}{1}{2}", Environment.NewLine, bestPolymer, Environment.NewLine);
                Console.WriteLine("Removed unit: {0}{1}", bestCharacter, Environment.NewLine);
                Console.WriteLine("Best polymer length: {0}{1}", bestPolymer.Length, Environment.NewLine);
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
