using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC07a
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
                SortedList<string, List<string>> instructions = new SortedList<string, List<string>>();
                string instructionOrder = "";

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
                    Regex inputRegex = new Regex(@"Step (?<dependency>.) must be finished before step (?<step>.) can begin.");
                    Match inputMatch = inputRegex.Match(input);
                    if (!(instructions.ContainsKey(inputMatch.Groups["step"].Value)))
                    {
                        instructions.Add(inputMatch.Groups["step"].Value, new List<string>());
                    }
                    if (!(instructions.ContainsKey(inputMatch.Groups["dependency"].Value)))
                    {
                        instructions.Add(inputMatch.Groups["dependency"].Value, new List<string>());
                    }
                    instructions[inputMatch.Groups["step"].Value].Add(inputMatch.Groups["dependency"].Value);
                }

                while (instructions.Count > 0)
                {
                    string step = "";
                    foreach (KeyValuePair<string, List<string>> item in instructions)
                    {
                        if (item.Value.Count > 0)
                        {
                            // still have dependencies; don't run this step
                            continue;
                        }
                        else
                        {
                            // no dependencies left; run this step
                            step = item.Key;
                            break;
                        }
                    }
                    if (step == "")
                    {
                        throw new Exception("Deadlocked!!");
                    }

                    Console.WriteLine("Next step: {0}", step);
                    foreach (List<string> dependencies in instructions.Values)
                    {
                        dependencies.Remove(step);
                    }
                    instructions.Remove(step);
                    instructionOrder += step;
                }

                Console.WriteLine("Full step order: {0}{1}", instructionOrder, Environment.NewLine);
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
