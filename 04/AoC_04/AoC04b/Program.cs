using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC04b
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
                SortedList<DateTime, guardEvent> guardEventLog = new SortedList<DateTime, guardEvent>();
                Dictionary<int, int[]> history = new Dictionary<int, int[]>();
                int[] curGuardAsleepMins = new int[60];
                int currentGuard = -1, fallAsleepMinute = -1, wakeUpMinute = -1;
                int highestMinute = -1, minuteIndex = -1;

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
                    Regex lineRegex = new Regex(@"\[(?<timestamp>.*)\] (?<event>.*)");
                    MatchCollection matches = lineRegex.Matches(line);
                    DateTime timestamp = DateTime.Parse(matches[0].Groups["timestamp"].Value);
                    guardEvent thisEvent = parseGuardEvent(matches[0].Groups["event"].Value.ToString());
                    guardEventLog.Add(timestamp, thisEvent);
                }

                foreach (KeyValuePair<DateTime, guardEvent> thisEvent in guardEventLog)
                {
                    if (thisEvent.Value.eventType == guardEventType.StartShift)
                    {
                        if ((fallAsleepMinute != -1) && (wakeUpMinute == -1))
                        {
                            wakeUpMinute = 60;
                            for (int m = fallAsleepMinute; m < wakeUpMinute; m++)
                            {
                                curGuardAsleepMins[m]++;
                            }
                        }

                        if (currentGuard != -1)
                        {
                            if (history.ContainsKey(currentGuard))
                            {
                                for (int m = 0; m < 60; m++)
                                {
                                    history[currentGuard][m] += curGuardAsleepMins[m];
                                }
                            }
                            else
                            {
                                history.Add(currentGuard, curGuardAsleepMins);
                            }
                        }

                        currentGuard = thisEvent.Value.guardNumber;
                        fallAsleepMinute = -1;
                        wakeUpMinute = -1;
                        curGuardAsleepMins = new int[60];
                    }
                    else if (thisEvent.Value.eventType == guardEventType.FallAsleep)
                    {
                        fallAsleepMinute = thisEvent.Key.Minute;
                    }
                    else if (thisEvent.Value.eventType == guardEventType.WakeUp)
                    {
                        wakeUpMinute = thisEvent.Key.Minute;
                        for (int m = fallAsleepMinute; m < wakeUpMinute; m++)
                        {
                            curGuardAsleepMins[m]++;
                        }

                        fallAsleepMinute = wakeUpMinute = -1;
                    }
                }

                foreach (KeyValuePair<int, int[]> guardData in history)
                {
                    for (int m = 0; m < 60; m++)
                    {
                        if (guardData.Value[m] > highestMinute)
                        {
                            currentGuard = guardData.Key;
                            minuteIndex = m;
                            highestMinute = guardData.Value[m];
                        }
                    }
                }

                Console.WriteLine(string.Format("Guard #{0} slept most at 00:{1,2:00} (was asleep {2} times)", currentGuard, minuteIndex, highestMinute));
                Console.WriteLine(string.Format("Multiplied number: {0}{1}", currentGuard * minuteIndex, Environment.NewLine));
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public enum guardEventType { StartShift, FallAsleep, WakeUp };

        public struct guardEvent
        {
            public guardEventType eventType;
            public int guardNumber;
            public guardEvent(guardEventType t, int n)
            {
                eventType = t;
                guardNumber = n;
            }
        }

        public static guardEvent parseGuardEvent(string s)
        {
            Regex shiftBeginRegex = new Regex(@"Guard #(?<guardNumber>\d+) begins shift");
            Match match = shiftBeginRegex.Match(s);
            if (match.Success)
            {
                return new guardEvent(guardEventType.StartShift, int.Parse(match.Groups["guardNumber"].Value));
            }
            else
            {
                if (s == "falls asleep")
                {
                    return new guardEvent(guardEventType.FallAsleep, -1);
                }
                else if (s == "wakes up")
                {
                    return new guardEvent(guardEventType.WakeUp, -1);
                }
                else
                {
                    throw new Exception("Input string was not in a correct format");
                }
            }
        }
    }
}
