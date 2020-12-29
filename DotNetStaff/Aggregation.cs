using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PluralsightCourses
{
    
    public class CarStatistics {
        public CarStatistics()
        {
            Max = Int32.MinValue;
            Min = Int32.MaxValue;
        }

        public int Max { get; set; }
        public int Min { get; set; }
        public double Average { get; set;  }
        public int Total { get; set;  }
        public int Count { get; set;  }

        public CarStatistics Accumulate(Car car)
        {
            Count += 1;
            Total += car.Combined;
            Max = Math.Max(Max, car.Combined);
            Min = Math.Max(Min, car.Combined);
            return this;
        }

        public CarStatistics Compute()
        {
            return this;
        }
    }
    
    public class Aggregation
    {
        static void Main3(string[] args)
        {
            var cars = FileProcessor.ProcessCars("fuel.csv");
            var manu = FileProcessor.ProcessManufacturers("manufacturers.csv");

            var query = cars.Join(manu, c => c.Manufacturer, m => m.Name, (c, m) => new
            {
                c.Manufacturer,
                c.Name,
                c.Combined,
                m.Headquarters
            })
                .OrderByDescending(c => c.Combined)
                .GroupBy(c => c.Headquarters);

            foreach (var carGroup in query.ToList())
            {
                var result = carGroup;
                Console.WriteLine($"{carGroup.Key} : {carGroup.Count()}");
                foreach (var car in carGroup.Take(3))
                {
                    Console.WriteLine($"\t{car.Name}, {car.Combined}");
                }
                
            }
            
            var aggregationResult = cars.Aggregate(new CarStatistics(), (acc, c) => acc.Accumulate(c), acc => acc.Compute());

            Console.WriteLine(aggregationResult.Average);
            Console.WriteLine(aggregationResult.Max);
            Console.WriteLine(aggregationResult.Min);
            

        }
    }
}