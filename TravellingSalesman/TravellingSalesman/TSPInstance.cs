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
    // TSP Instance class contains methods for reading in, and creating tours for a specific tsp problem set
    class TSPInstance
    {
        private string filename;                    // store filename of dataset
        public List<PointF> originalCitiesData;     // list to store the original cities read from the data
        private int dimension;                      // dimension stores problem size

        // Constructor, takes in file name, adds path to resource folder, stores a reference to it, and runs the file Loader
        public TSPInstance(String fn)
        {
            // relative path for resource folder
            string path = "..\\..\\Resources\\";
            filename = path + fn + ".tsp";

            LoadTSPLib();
        } 

        // Load reads from the given file. Checks for errors, parses data. Returns list of points (cities to visit on tour) and size of the problem
        public void LoadTSPLib()
        {
            List<PointF> result = new List<PointF>(); // for storing result

            StreamReader reader;
            
            try
            {
                // create instance of stream reader to read from a file
                reader = new StreamReader(filename);
               
                bool readingNodes = false; // flag to check for End of Field
                dimension = 0;             // dimension is number of points within problem

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
                                Console.ReadLine();
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

                            // trim any space from values
                            tokens[1].Trim();
                            tokens[2].Trim();

                            // token[0] is city ID and can be ignored.

                            // token[1] is x coord, 2 is y coordinate of city
                            float x = float.Parse(tokens[1]);
                            float y = float.Parse(tokens[2]);

                            // create a new point and add to list of cities
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
            catch (Exception e) // catch all exceptions, and print message.
            {
                Console.WriteLine("Error reading file: " + e.Message);
            }

            // store the result
            originalCitiesData = result;
            
        }

        //Nearest Neighbour alg from pseudocode
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

                    // using distance sq as a comparison for a small optimization  
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

        // TwoOpt Algorithm: From a starting permutation, swap cities, if better, keep result
        public List<PointF> TwoOpt(List<PointF> citiesIn)
        {
            // deep copy of list to store result (if no swaps can improve, this is result)
            List<PointF> result = new List<PointF>(citiesIn);

            int improvement = 0;

            while (improvement < 5)
            {
                // calculate distance of current tour.
                double bestDistance = CalculateLength(result);                

                for (int i = 0; i < dimension -1; ++i)
                {
                    for (int k = i + 1; k < dimension; ++k)
                    {
                        List<PointF> newTour = Swap(result, i, k);

                        double new_distance = CalculateLength(newTour);

                        if (new_distance < bestDistance)
                        {
                            improvement = 0;
                            result = newTour;
                            bestDistance = new_distance;

                        }
                    }
                }
                improvement++;
            }

            
            return result;
        }

        public List<PointF> Swap(List<PointF> tour, int i, int k)
        {
            List<PointF> result = new List<PointF>();

            // first part of route add in order, tour[0] to tour[i-1]
            for (int c =0; c <= i -1; ++c)
            {
                result.Add(tour[c]);
            }

            int dec = 0;
            for (int c = i; c <= k; ++c)
            {
                result.Add(tour[k - dec]);
                dec++;
            }

            for (int c = k + 1; c < dimension; ++c)
            {
                result.Add(tour[c]);
            }

            return result;
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
