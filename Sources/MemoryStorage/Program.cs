using System;
using System.Diagnostics;
using System.IO;

namespace MemoryTest
{
    class Program
    {
        private static Random _rnd = new Random(DateTime.Now.Millisecond);
        private const int c_totalMovies = 17770;
        private const int c_customersPerMovie = 5882;
        private const int c_moviesPerCustomer = 209;
        private const int c_totalCustomers = 480000;

        static void Main(string[] args)
        {
            TestPacker();

            string moviesFilename = @"C:\Tests\tmp\movies.dat";
            string customersFilename = @"C:\Tests\tmp\customers.dat";
            
            DateTime start = DateTime.Now;
            DataHolder movies = GetMovies(moviesFilename);
            DataHolder customers = GetCustomers(customersFilename);

            Console.WriteLine(DateTime.Now - start);

            CheckPerformance(movies);
            CheckPerformance(customers);
            Console.ReadKey();
        }

        private static DataHolder GetCustomers(string customersFilename)
        {
            return GetDataSet(customersFilename, c_moviesPerCustomer * 2, c_totalCustomers, c_totalMovies);
        }

        private static DataHolder GetMovies(string moviesFilename)
        {
            return GetDataSet(moviesFilename, c_customersPerMovie * 2, c_totalMovies, c_totalCustomers);
        }

        private static DataHolder GetDataSet(string filename, int slotSize, int slotsCount, int maxValue)
        {
            DataHolder result;
            bool exists = File.Exists(filename);
            if (exists)
            {
                result = DataHolder.Load(filename);
            }
            else
            {
                result = DataHolder.GenerateRandomDataset(slotSize, slotsCount, maxValue);
                result.Save(filename);
            }
            
            return result;
        }

        private static void CheckPerformance(DataHolder dataset)
        {
            DateTime start = DateTime.Now;
            PerformanceCheck(dataset);
            Console.WriteLine(DateTime.Now - start);
        }

        private static void PerformanceCheck(DataHolder dataset)
        {
            for (int i = 0; i < 1000000; i++)
            {
                int slot = _rnd.Next(c_totalMovies);
                int id = _rnd.Next(c_totalCustomers);

                byte rating = dataset.GetRating(slot, id);
            }
        }

        private static void TestPacker()
        {
            int packed = PackedInt.Pack(c_totalCustomers, 3);

            int id = PackedInt.GetId(packed);
            byte rating = PackedInt.GetRating(packed);
            
            int[] arr = new int[3] 
            {
                PackedInt.Pack(0, 2),
                PackedInt.Pack(5, 5),
                PackedInt.Pack(10, 3)
            };
            
            
            int index = Array.BinarySearch(arr, 5, new PackedInt());
            Debug.Assert(1 == index);
        }

    }
}
