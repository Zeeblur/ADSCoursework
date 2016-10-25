using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace TravellingSalesman
{
    class TSPInstance
    {
        private string filename;
        private List<PointF> cities;
        private double lengthOfTour;

        public TSPInstance(String fn)
        {
            filename = fn;
        } 

        public double LengthOfTour
        {
            get { return lengthOfTour; }
        }

        public void LoadTSPLib()
        {
            List<PointF> result = new List<PointF>();

            StreamReader reader;
            
            try
            {
                // create instance of stream reader to read from a file
                reader = new StreamReader(filename);
               
                bool readingNodes = false; // flag to check for End of Field
                int dimension = 0;         // dimension is number of points within problem

                // using closes stream when complete
                using (reader)
                {
                    string line;
                    // while more lines to read, print out
                    while ((line = reader.ReadLine()) != null)
                    {
                        // Read file until end of field
                        if (line.Contains("EOF"))
                        {
                            // set finished flag and check if dimension is correct
                            readingNodes = false;

                            if (result.Count != dimension)
                            {
                                // close app if dimension isn't correct
                                Console.WriteLine("Error loading cities");
                                Environment.Exit(-1);
                            }
                        }

                        // parse nodes
                        if (readingNodes)
                        {
                            // get rid of spaces at start of line
                            line = line.TrimStart();

                            // split at any number of spaces (1 or more)
                            string[] tokens = Regex.Split(line, @"\s+").ToArray();

                            tokens[1].Trim();
                            tokens[2].Trim();

                            // token[0] is city ID
                            float x = float.Parse(tokens[1].Trim());
                            float y = float.Parse(tokens[2].Trim());

                            // add to list of cities
                            PointF city = new PointF(x, y);
                            result.Add(city);
                        }

                        // read dimension
                        if (line.Contains("DIMENSION"))
                        {
                            // save expected problem ( number of cities)
                            String[] tokens = line.Split(':');
                            dimension = Int32.Parse(tokens[1].Trim());
                        }

                        // find node data
                        if (line.Contains("NODE_COORD_SECTION"))
                            readingNodes = true;

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no" + e.Message);
            }

            cities = result;
        }

        public void CalculateLength()
        {
            double result = 0;

            // set previous city to last city in the list to measure the length of entire loop
            PointF previousCity = cities.ElementAt(cities.Count - 1);

            foreach(PointF city in cities)
            {
                // go through each city in turn summing length between neighbouring points
                result += Distance(city, previousCity);
                previousCity = city;
            }

            lengthOfTour = result;
        }

        private double Distance(PointF p1, PointF p2)
        {
            // method to calculate distance between two points

            double result = 0;

            PointF difference = new PointF(p1.X - p2.X, p1.Y - p2.Y);

            result = Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y);

            return result;
        }
    }
}
