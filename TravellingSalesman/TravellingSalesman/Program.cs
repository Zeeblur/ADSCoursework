using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TravellingSalesman
{
    class Program
    {
        static void Main(string[] args)
        {
            TSPInstance test = new TSPInstance("berlin52.tsp");

            test.LoadTSPLib();      // load library

            Console.WriteLine("length of tour = " + test.LengthOfTour);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            List<PointF> nn = test.NearestNeighbour(test.originalCitiesData);
            stopwatch.Stop();
            long elapsed_time = stopwatch.ElapsedMilliseconds;

            Console.WriteLine("time taken = " + elapsed_time + "ms");
            
            Console.WriteLine("is valid: " + test.Correct(nn));

            Console.WriteLine("nn =" + test.CalculateLength(nn));

            List<PointF> twoOpt = test.TwoOpt(nn);

            Console.WriteLine("two opt valid: " + test.Correct(twoOpt));
            Console.WriteLine("two = " + test.CalculateLength(twoOpt));

            Console.ReadLine();
        }


    }
}
