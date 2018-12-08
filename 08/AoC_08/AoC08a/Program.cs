using System;
using System.Collections.Generic;
using System.IO;

namespace AoC08a
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
                int sum;

                if (File.Exists(args[0]))
                {
                    inputFile = new StreamReader(args[0]);
                }
                else
                {
                    throw new FileNotFoundException("File not found", args[0]);
                }

                data = new List<int>(Array.ConvertAll(inputFile.ReadLine().Split(' '), int.Parse));

                sum = TreeSum(ref data);

                Console.WriteLine("Sum of all metadata: {0}{1}", sum, Environment.NewLine);
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fatal error:");
                Console.WriteLine(ex.ToString());
                Environment.Exit(1);
            }
        }

        public static int TreeSum(ref List<int> data)
        {
            int sum = 0;

            int children = data[0];
            data.RemoveAt(0);

            int metadata = data[0];
            data.RemoveAt(0);

            for (int c = 0; c < children; c++)
            {
                sum += TreeSum(ref data);
            }

            for (int m = 0; m < metadata; m++)
            {
                sum += data[0];
                data.RemoveAt(0);
            }

            return sum;
        }
    }
}
