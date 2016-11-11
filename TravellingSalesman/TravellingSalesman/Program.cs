using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows;

namespace TravellingSalesman
{
    class Program
    {

        static void Main(string[] args)
        {
            // initialise TSP instance and load file
            TSPInstance berlin = new TSPInstance("a280.tsp");

            //for (int i = 0; i < 10; ++i)
                RunNearestNeighbour(berlin);


            // for (int i = 0; i < 10; ++i)
                RunTwoOpt(berlin);

            // stop console window from closing
            Console.ReadLine();
        }

        // Method to run, time and print results from nearest neighbour test
        public static void RunNearestNeighbour(TSPInstance test)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<PointF> nn = test.NearestNeighbour(test.originalCitiesData);
            stopwatch.Stop();
            long elaspedTime = stopwatch.ElapsedMilliseconds;
            Console.WriteLine("time taken = " + elaspedTime + "ms");
            Console.WriteLine("length of tour = " + test.CalculateLength(nn));
            Console.WriteLine("is valid solution: " + test.Correct(nn) + "\n");
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

            // print results
            Console.WriteLine("time taken = " + elaspedTime + "ms");

            // calculate total length of tour
            Console.WriteLine("length of tour = " + test.CalculateLength(twoOpt));

            // check if solution is correct (no duplicates/dimensions are correct/everything exists in the list)
            Console.WriteLine("is valid solution: " + test.Correct(twoOpt) + "\n");
        }


    }
}
