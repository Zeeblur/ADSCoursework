using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
using System.Windows;

namespace Visualisation
{
    // struct to store data/results from algorithms
    struct Data
    {
        public long time;            // time taken for algorithm to complete
        public double length;        // length of completed tour
        public List<PointF> tour;    // tour (order of cities to visit)
    }

    class TSPSolver
    {
        // vars to store instances of different tsp routes
        private TSPInstance berlin;
        private TSPInstance lin;
        private TSPInstance selected;

        // constructor to load two tsp instances and store cities in list
        public TSPSolver()
        {
            // load instances of tsp for two routes
            berlin = new TSPInstance("berlin52");
            lin = new TSPInstance("lin318");
        }

        // setter for selected tour from gui radiobtn
        public void Selected(tour value)
        {
            // selected reference tsp instance to perform calulations on
            if (value == tour.berlin)
                selected = berlin;
            else
                selected = lin;
        }

        // performs then returns nearest neighbour results
        public Data NN()
        {
            // struct to store data from alg to use in GUI
            Data nearest = new Data();
           
            // time and complete nearest neighbour alg
            Stopwatch watch = new Stopwatch();
            watch.Start();
            nearest.tour = selected.NearestNeighbour(selected.originalCitiesData);
            watch.Stop();

            // store time and length
            nearest.time = watch.ElapsedMilliseconds;
            nearest.length = selected.CalculateLength(nearest.tour);

            return nearest;
        }        

        // performs then returns two opt results
        public Data TwoOpt()
        {
            // struct to store results from twoopt
            Data twoOpt = new Data();

            // start timer, perform nn then two opt from nn
            Stopwatch watch = new Stopwatch();
            watch.Start();
            List<PointF> near = selected.NearestNeighbour(selected.originalCitiesData);
            twoOpt.tour = selected.TwoOpt(near);
            watch.Stop();

            // store time and length
            twoOpt.time = watch.ElapsedMilliseconds;
            twoOpt.length = selected.CalculateLength(twoOpt.tour);

            return twoOpt;
        }      

    }
}
