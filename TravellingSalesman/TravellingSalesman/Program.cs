using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TravellingSalesman
{
    class Program
    {
        static void Main(string[] args)
        {
            TSPInstance test = new TSPInstance("a280.tsp");

            test.LoadTSPLib();      // load library

            Console.WriteLine("length of tour = " + test.LengthOfTour);

            List<PointF> nn = test.NearestNeighbour(test.originalCitiesData);

            Console.WriteLine("nn =" + test.CalculateLength(nn));
            Console.ReadLine();
        }


    }
}
