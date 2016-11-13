using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace Visualisation
{
    // enum used for radiobutton selection of problems loaded
    enum tour { berlin, lin };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int multiplier = 5; // scale for points drawn to canvas

        // two structs to store data from algorithms for faster visualisation
        private Data near;
        private Data twoOpt;

        // instance of solver
        private TSPSolver solver;

        // constructor of main window to intialse and load problems
        public MainWindow()
        {
            // initialise main window
            InitializeComponent();

            // initialise solver
            solver = new TSPSolver();
        }
    
        // event handler for nearestneighbour btn click, displays stored results and draws graph
        private void nnBtn_Click(object sender, RoutedEventArgs e)
        {
            typeLbl.Content = "Nearest\nNeighbour"; // change title to nn
            UpdateResults(near);                    // update graph
        }

        // event handler for twoopt click, displays new results
        private void twoOptBtn_Click(object sender, RoutedEventArgs e)
        {
            typeLbl.Content = "Two Opt";    // change title
            UpdateResults(twoOpt);          // update gui
        }

        // method to update gui with results. Displays graph and time/length
        private void UpdateResults(Data results)
        {
            // update labels
            timeLbl.Content = "Time: " + results.time + "ms";
            lengthLbl.Content = "Length: " + results.length;


            mCanvas.Children.Clear();   // ensure canvas is clear before drawing

            // for every city(point) stored in the tour list, create a line and add it to the canvas as a child
            for (int i = 0; i < results.tour.Count; ++i)
            {
                Line l = new Line();

                // ensure visible
                l.Visibility = System.Windows.Visibility.Visible;
                l.StrokeThickness = 2;
                l.Stroke = System.Windows.Media.Brushes.Black;

                // first point of line
                l.X1 = results.tour[i].X / multiplier;
                l.Y1 = results.tour[i].Y / multiplier;

                // second point of line (if not last point, draw line between next point) 
                if (i < results.tour.Count - 1)
                {
                    l.X2 = results.tour[i + 1].X / multiplier;
                    l.Y2 = results.tour[i + 1].Y / multiplier;
                }
                else // else if last point, draw between that and starting point in list
                {
                    l.X2 = results.tour[0].X / multiplier;
                    l.Y2 = results.tour[0].Y / multiplier;
                }

                // add line to canvas
                mCanvas.Children.Add(l);
            }
        }

        // event handler for rb, changes choice of data set and runs solutions, shows nearest neighbour first by default
        private void berlinRBtn_Checked(object sender, RoutedEventArgs e)
        {
            // calculate selected tour nn and twoOpt
            solver.Selected(tour.berlin);
            near = solver.NN();
            twoOpt = solver.TwoOpt();

            // change line multiplier to fit lines to canvas
            multiplier = 5;

            // show nearest neighbour results first
            nnBtn_Click(this, new RoutedEventArgs());
        }

        // event handler for rb lin, changes choice of data set and runs solutions, shows nearest neighbour first by default
        private void linRBtn_Checked(object sender, RoutedEventArgs e)
        {
            // calculate selected tour nn and twoOpt
            solver.Selected(tour.lin);
            near = solver.NN();
            twoOpt = solver.TwoOpt();


            // change line multiplier to fit lines to canvas
            multiplier = 10;

            // show nearest neighbour results first
            nnBtn_Click(this, new RoutedEventArgs());
        }
    }
}
