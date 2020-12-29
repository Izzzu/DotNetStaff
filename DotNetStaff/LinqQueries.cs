using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PluralsightCourses
{
    class LinqQueries
    {
        static void Main2(string[] args)
        {
            // ---------------------------------------------------------------------------------------------
            var cars = FileProcessor.ProcessCars("fuel.csv");

            var top = cars
                .OrderByDescending(c => c.Combined)
                .ThenBy(c => c.Name)
                .FirstOrDefault(car => car.Manufacturer == "BMW" && car.Year == 2016);

            Console.WriteLine(top?.Name);

            var anyCar = cars.All(c => c.Manufacturer == "Ford");
            Console.WriteLine(anyCar);

            var flattingQuery = cars.SelectMany(c => c.Name).Where(c => c != ' ');
            foreach (var character in flattingQuery)
            {
                Console.Write(character);
            }


            // ---------------------------------------------------------------------------------------------
            var manu = FileProcessor.ProcessManufacturers("manufacturers.csv");

            var joiningQuery = from car in cars
                join manufacturer in manu
                    on new {car.Manufacturer, car.Year} equals new {Manufacturer = manufacturer.Name, manufacturer.Year}
                orderby car.Combined descending, car.Name
                select new
                {
                    manufacturer.Headquarters,
                    car.Name,
                    car.Combined
                };
            foreach (var r in joiningQuery.ToList())
            {
                Console.WriteLine(r);
            }


            var queryGroup = from car in cars
                group car by car.Manufacturer.ToUpper()
                into newManufacturer
                orderby newManufacturer.Key
                select newManufacturer;

            foreach (var res in queryGroup)
            {
                Console.WriteLine($"{res.Key} has {res.Count()} cars");
                foreach (var c in res.OrderByDescending(c => c.Name))
                {
                    Console.WriteLine($"\t{c.Name}, {c.Combined}");
                }
            }
        }
    }
}