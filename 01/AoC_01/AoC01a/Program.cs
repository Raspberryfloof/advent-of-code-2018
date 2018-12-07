using System;
using System.IO;

namespace AoC01a
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
                int frequency = 0, count = 1;

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
                    int lineFreq = 0;
                    string line = inputFile.ReadLine();

                    try
                    {
                        lineFreq = int.Parse(line);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error parsing line #{0}:", count.ToString());
                        Console.WriteLine(ex.Message);
                    }

                    frequency += lineFreq;
                    Console.WriteLine("Line {0}: frequency change {1}, new frequency {2}", count, lineFreq, frequency);
                    count++;
                }

                Console.WriteLine("Final frequency: {0}{1}", frequency, Environment.NewLine);
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
