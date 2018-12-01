using System;
using System.Collections.Generic;
using System.IO;

namespace AoC01b
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
                int frequency = 0, count = 1;
                List<int> freqHistory = new List<int>();

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                do
                {
                    // Reset stream if we are at the end
                    if (inputFile.EndOfStream)
                    {
                        inputFile.BaseStream.Position = 0;
                        inputFile.DiscardBufferedData();
                    }

                    int lineFreq = 0;
                    string line = inputFile.ReadLine();

                    try
                    {
                        lineFreq = int.Parse(line);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(string.Format("Error parsing line #{0}:", count.ToString()));
                        Console.WriteLine(ex.Message);
                    }

                    frequency += lineFreq;
                    Console.WriteLine(string.Format("Line {0}: frequency change {1}, new frequency {2}", count, lineFreq, frequency));
                    count++;

                    if (freqHistory.Contains(frequency))
                    {
                        Console.WriteLine(string.Format("First match found: frequency {0}", frequency));
                        Environment.Exit(0);
                    }
                    else
                    {
                        freqHistory.Add(frequency);
                    }
                } while (true);
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
