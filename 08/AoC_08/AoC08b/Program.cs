using System;
using System.Collections.Generic;
using System.IO;

namespace AoC08b
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
                List<int> data;

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                data = new List<int>(Array.ConvertAll(inputFile.ReadLine().Split(' '), int.Parse));

                Console.WriteLine("Root node value: {0}{1}", RootValue(data, 0), Environment.NewLine);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public static int RootValue(List<int> data, int nodeLocation)
        {
            int rootLocation = nodeLocation;
            int value = 0;
            int children = data[nodeLocation];
            int metadata = data[nodeLocation + 1];
            Tuple<List<int>, int> dataLocations = LocateChildren(data, ref rootLocation);

            if (children == 0)
            {
                for (int m = 0; m < metadata; m++)
                {
                    value += data[dataLocations.Item2 + m];
                }
            }
            else
            {
                for (int m = 0; m < metadata; m++)
                {
                    int metadataValue = data[dataLocations.Item2 + m];
                    if ((metadataValue > 0) && (metadataValue <= dataLocations.Item1.Count))
                    {
                        value += RootValue(data, dataLocations.Item1[metadataValue - 1]);
                    }
                }
            }
            
            return value;
        }

        public static Tuple<List<int>, int> LocateChildren(List<int> data, ref int nodeLocation)
        {
            List<int> locations = new List<int>();
            int children = data[nodeLocation++];
            int metadata = data[nodeLocation++];

            for (int c = 0; c < children; c++)
            {
                locations.Add(nodeLocation);
                LocateChildren(data, ref nodeLocation);
            }

            int metadataLocation = nodeLocation;

            for (int m = 0; m < metadata; m++)
            {
                nodeLocation++;
            }

            return new Tuple<List<int>, int>(locations, metadataLocation);
        }
    }
}
