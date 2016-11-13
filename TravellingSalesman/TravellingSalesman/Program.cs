using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows;
using System.IO;

namespace TravellingSalesman
{

    // Execution of the program is handled in this class
    class Program
    {
       
        static StreamWriter writer;  // declaration of streamwriter to write data to a csv file
        static string delim = ",";   // delimiter for csv

        static void Main(string[] args)
        {
            // file name for data set
            string fn = "berlin52";

            // initialise TSP instance and load file
            TSPInstance berlin = new TSPInstance(fn);
            
            // Initialise CSV file. Create and open streamwriter for writing, and create table headings
            InitialiseCSV(fn);

            // Loop for running tests n times
            for (int i = 0; i < 5; ++i)
            {
                RunNearestNeighbour(berlin);
                RunTwoOpt(berlin);
                writer.WriteLine();
            }

            // close writer connection to file and dispose of it
            writer.Close();
            writer.Dispose();

            // stop console window from closing
            //Console.ReadLine();
        }

        // Method to run, time and print results from nearest neighbour test
        public static void RunNearestNeighbour(TSPInstance test)
        {
            // Start timer
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // create new tour from original read-in data, using nearest neighbour algorithm
            List<PointF> nn = test.NearestNeighbour(test.originalCitiesData);

            // Stop timer
            stopwatch.Stop();
            long elaspedTime = stopwatch.ElapsedMilliseconds;

            // print results
            // calculate total length of tour
            // check if solution is correct (no duplicates/dimensions are correct/everything exists in the list)
            PrintResult(elaspedTime, test.CalculateLength(nn), test.Correct(nn));
        }

        // Method to run, time and print results from TwoOpt test
        public static void RunTwoOpt(TSPInstance test)
        {
            // start stopwatch
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // create new tour from NearestNeighbour.
            List<PointF> twoOpt = test.TwoOpt(test.NearestNeighbour(test.originalCitiesData));

            // Stop timer
            stopwatch.Stop();
            long elaspedTime = stopwatch.ElapsedMilliseconds;

            // Print results
            PrintResult(elaspedTime, test.CalculateLength(twoOpt), test.Correct(twoOpt));

        }

        // Method to print results in same format and add to file
        public static void PrintResult(long time, double length, bool correct)
        {
            // print results to console
            Console.WriteLine("Time taken = " + time + "ms");
            Console.WriteLine("Length of tour = " + length);
            Console.WriteLine("Is valid solution: " + correct + "\n");

            // write to file, length and time within table, separated by commas
            writer.Write(length + delim + time + delim);
        }

        // Method to create csv file to store data, and create table headings.
        public static void InitialiseCSV(string fn)
        {
            try
            {
                // create new Streamwriter connection to new file
                writer = new StreamWriter("..\\..\\Solutions\\DataSet-"+ fn +"TEST.csv");

                // write table headings in file
                writer.Write("NN Length" + delim);

                writer.Write("NN Time (ms)" + delim);

                writer.Write("Two-Opt Length" + delim);

                writer.Write("Two-Opt Time (ms)");

                // new line
                writer.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem in writing to file: " + e);
            }
        }

    }
}
