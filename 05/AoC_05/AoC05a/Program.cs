using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC05a
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
                string input = "", finalPolymer = "";
                List<string> loupReplacements = new List<string>();
                List<string> uploReplacements = new List<string>();
                Regex loupRegex, uploRegex;
                int count = 1;

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

                finalPolymer = inputFile.ReadLine();
                Console.WriteLine(string.Format("Initial polymer size: {0}", finalPolymer.Length));

                do
                {
                    input = finalPolymer;
                    finalPolymer = loupRegex.Replace(input, "");
                    finalPolymer = uploRegex.Replace(finalPolymer, "");
                    Console.WriteLine(string.Format("After {0} reactions, size = {1}", count, finalPolymer.Length));
                    count++;
                } while (finalPolymer != input);

                Console.WriteLine(string.Format("Final polymer:{0}{1}{2}", Environment.NewLine, finalPolymer, Environment.NewLine));
                Console.WriteLine(string.Format("Final polymer length: {0}{1}", finalPolymer.Length, Environment.NewLine));
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
