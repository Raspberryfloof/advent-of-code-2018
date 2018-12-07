using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC07b
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
                SortedList<string, List<string>> instructions = new SortedList<string, List<string>>();
                int[] workerTimers = new int[5]; // 5 workers
                string[] workerTasks = new string[5]; // 5 workers
                int timer = 0;

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

                while ((instructions.Count > 0) || WorkersRunning(workerTimers))
                {
                    string step = "";
                    bool dispatch = false;
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

                    if (step != "")
                    {
                        // Next step is ready; attempt dispatch
                        int workerID = 0;
                        for (; workerID < workerTimers.Length; workerID++)
                        {
                            if (workerTimers[workerID] == 0)
                            {
                                break;
                            }
                        }
                        if (workerID < workerTimers.Length)
                        {
                            // Worker available; dispatch
                            dispatch = true;
                            Console.WriteLine("Next step: {0}", step);
                            Console.WriteLine("-- Dispatched to worker {0} at time {1}", workerID, timer);

                            workerTimers[workerID] = (char.Parse(step) - 'A') + 61;
                            workerTasks[workerID] = step;

                            // Remove from list of pending tasks
                            instructions.Remove(step);
                        }
                    }

                    if (!dispatch)
                    {
                        // When done dispatching for this tick, tick timers
                        Console.Write("Time: {0}", timer);
                        for (int i = 0; i < workerTimers.Length; i++)
                        {
                            Console.Write(" | {0}", workerTimers[i]);
                            if (workerTimers[i] > 0)
                            {
                                workerTimers[i]--;
                            }
                            if (workerTimers[i] == 0)
                            {
                                // notify dependencies that task is done
                                foreach (List<string> dependencies in instructions.Values)
                                {
                                    dependencies.Remove(workerTasks[i]);
                                }
                                workerTasks[i] = "";
                            }
                        }
                        timer++;
                        Console.Write(Environment.NewLine);
                    }
                }

                Console.WriteLine("Final time: {0}{1}", timer, Environment.NewLine);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public static bool WorkersRunning(int[] workerTimers)
        {
            for (int i = 0; i < workerTimers.Length; i++)
            {
                if (workerTimers[i] > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
