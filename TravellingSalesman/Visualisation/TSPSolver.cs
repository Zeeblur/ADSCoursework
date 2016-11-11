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
    class TSPSolver
    {
        TSPInstance berlin;
        TSPInstance ali535;

        public TSPSolver()
        {
            berlin = new TSPInstance("berlin52.tsp");
            ali535 = new TSPInstance("ali535.tsp");
        }

        public static List<PointF> NN()
        {
            TSPInstance test = new TSPInstance("a280.tsp");

          
            List<PointF> near = test.NearestNeighbour(test.originalCitiesData);

            return near;
        }

        public static List<PointF> TwoOpt()
        {
            TSPInstance test = new TSPInstance("a280.tsp");

            List<PointF> near = test.NearestNeighbour(test.originalCitiesData);

            List<PointF> twoOpt = test.TwoOpt(near);
            return twoOpt;
        }


    }
}
