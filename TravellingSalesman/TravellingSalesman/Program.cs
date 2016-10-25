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
            test.CalculateLength(); // calc tour length

            Console.WriteLine("length of tour = " + test.LengthOfTour);

            Console.ReadLine();
        }
    }
}
