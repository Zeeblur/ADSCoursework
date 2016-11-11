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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int multiplier = 1;
        List<PointF> near;
        List<PointF> twoOpt;

        public MainWindow()
        {
            InitializeComponent();

            near = TSPSolver.NN();
            twoOpt = TSPSolver.TwoOpt();
        }

        private void nnBtn_Click(object sender, RoutedEventArgs e)
        {
            mCanvas.Children.Clear();

            for (int i = 0; i < near.Count; ++i)
            {
                Line l = new Line();
                
                l.Visibility = System.Windows.Visibility.Visible;
                l.StrokeThickness = 2;
                l.Stroke = System.Windows.Media.Brushes.Black;
                l.X1 = near[i].X / multiplier;
                l.Y1 = near[i].Y / multiplier;

                if (i < near.Count - 1)
                {
                    l.X2 = near[i + 1].X / multiplier;
                    l.Y2 = near[i + 1].Y / multiplier;
                }
                else
                {
                    l.X2 = near[0].X / multiplier;
                    l.Y2 = near[0].Y / multiplier;
                }

                mCanvas.Children.Add(l);
            }
        }

        private void twoOptBtn_Click(object sender, RoutedEventArgs e)
        {
            mCanvas.Children.Clear();

            for (int i = 0; i < twoOpt.Count; ++i)
            {
                Line l = new Line();

                l.Visibility = System.Windows.Visibility.Visible;
                l.StrokeThickness = 2;
                l.Stroke = System.Windows.Media.Brushes.Black;
                l.X1 = twoOpt[i].X / multiplier;
                l.Y1 = twoOpt[i].Y / multiplier;

                if (i < twoOpt.Count - 1)
                {
                    l.X2 = twoOpt[i + 1].X / multiplier;
                    l.Y2 = twoOpt[i + 1].Y / multiplier;
                }
                else
                {
                    l.X2 = twoOpt[0].X / multiplier;
                    l.Y2 = twoOpt[0].Y / multiplier;
                }

                mCanvas.Children.Add(l);
            }
        }
    }
}
