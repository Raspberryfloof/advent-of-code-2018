using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC09b
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
                Regex inputRegex = new Regex(@"(?<players>\d+) players; last marble is worth (?<target>\d+) points");
                int players = 0, nextPlayer = 0;
                long marbleScore = 0;
                long[] playerScores;
                int nextMarble = 1, targetMarble = 0, lastMarbleID = 0, marbleCount = 1;
                List<long> marbles = new List<long>();

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                Match inputMatch = inputRegex.Match(inputFile.ReadLine());
                players = int.Parse(inputMatch.Groups["players"].Value);
                targetMarble = int.Parse(inputMatch.Groups["target"].Value) * 100;

                playerScores = new long[players];
                marbles.Add(0);

                do
                {
                    if (nextMarble + 22 > targetMarble)
                    {
                        Console.WriteLine("High score: {0}, by player {1}{2}", playerScores.Max(), Array.IndexOf(playerScores, playerScores.Max()) + 1, Environment.NewLine);
                        Environment.Exit(0);
                    }
                    for (int t = 0; t < 22; t++)
                    {
                        int nextMarbleID = (lastMarbleID + 2) % marbleCount;
                        marbles.Insert((int)nextMarbleID, nextMarble);
                        marbleCount++;
                        lastMarbleID = nextMarbleID;
                        nextMarble++;
                        nextPlayer = (nextPlayer + 1) % players;
                    }
                    marbleScore = nextMarble;
                    int removedMarbleID = (lastMarbleID + marbleCount - 7) % marbleCount;
                    long removedMarble = marbles[removedMarbleID];
                    marbleScore += removedMarble;
                    marbles.RemoveAt(removedMarbleID);
                    marbleCount--;
                    lastMarbleID = removedMarbleID;
                    playerScores[nextPlayer] += marbleScore;

                    Console.WriteLine("Player {0} scored {1} points on marble #{2}", nextPlayer + 1, marbleScore, nextMarble);

                    nextMarble++;
                    nextPlayer = (nextPlayer + 1) % players;
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
