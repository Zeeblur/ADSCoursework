using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TravellingSalesman
{

    class TSPInstance
    {
        private string filename;
        public List<PointF> originalCitiesData;
        private double lengthOfTour;

        public TSPInstance(String fn)
        {
            // relative path for resource folder
            string path = "..\\..\\Resources\\";
            filename = path + fn;
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


            originalCitiesData = result;

            lengthOfTour = CalculateLength(originalCitiesData);
        }

        //Nearest Neighbour alg
        public List<PointF> NearestNeighbour(List<PointF> citiesIn)
        {
            // deep copy of given list
            List<PointF> cities = new List<PointF>(citiesIn);

            // Create new empty list to store re-ordered tour
            List<PointF> newTour = new List<PointF>();

            // reference to closest city
            PointF closestCity = new PointF();

            // get first city as staring point and remove from list as its been used 
            PointF current = cities.ElementAt(0);
            cities.RemoveAt(0);

            double closestDistance;

            while (cities.Count > 0)
            {
                newTour.Add(current);   // add current city
                        
                closestDistance = double.PositiveInfinity;

                // find closest city to current
                foreach (PointF possCity in cities)
                {
                    double pointDistance = DistanceSQ(current, possCity);

                    // if distance is closer, update vars
                    if (pointDistance < closestDistance)
                    {
                        closestCity = possCity;
                        closestDistance = pointDistance;
                    }
                }

                // remove closest city from the list, add to tour, and set as current to loop and find closest to that
                cities.Remove(closestCity);
                current = closestCity;
                
            }

            newTour.Add(current);


            return newTour;
        }

        // Calculate length of tour
        public double CalculateLength(List<PointF> cities)
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

            return result;
        }

        // calculate distance between two points
        private double Distance(PointF p1, PointF p2)
        {
            // method to calculate distance between two points

            double result = 0;

            PointF difference = new PointF(p1.X - p2.X, p1.Y - p2.Y);

            result = Math.Sqrt(difference.X * difference.X + difference.Y * difference.Y);

            return result;
        }

        // calculate squared distance between two points
        private double DistanceSQ(PointF p1, PointF p2)
        {
            // method to calculate distance between two points squared

            double result = 0;

            PointF difference = new PointF(p1.X - p2.X, p1.Y - p2.Y);

            result = (difference.X * difference.X) + (difference.Y * difference.Y);


            // distance squared is used to save on computing expensive sqrt for nearest neighbour check
            return result;
        }

        // check if correct
        public bool Correct(List<PointF> toCheck)
        {
            // compare sizes. If wrong don't calculate anything
            if (toCheck.Count != originalCitiesData.Count)
                return false;

            foreach (PointF p in originalCitiesData)
            {
                // foreach original city, check if it is within the new permutation
                if (!toCheck.Contains(p))
                    return false;
            }

            // create new hashSet to check for duplicates. Add each point into set and if it can't then it is a duplicate
            HashSet<PointF> hashSet = new HashSet<PointF>();
            
            for (int i = 0; i < toCheck.Count; ++i)
            {
                if (!hashSet.Add(toCheck[i]))
                    return false;
            }
            

            // all checks passed return true
            return true;

        }
    }
}
