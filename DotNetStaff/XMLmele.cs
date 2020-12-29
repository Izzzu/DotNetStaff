using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Schema;

namespace PluralsightCourses
{
    public class XMLmele
    {
        static XNamespace ns = "http://pluralsight.com/cars/2016";
        static XNamespace ex = "http://pluralsight.com/cars/2016/ex";

        public static void Main()
        {
            CreateXml(records);
            QueryXml();
        }


        private static void QueryXml()
        {
            //with huge files consider using older API XReader - stream, XDocument loads the whole file into a memory
            var document = XDocument.Load("fuel.xml");
            
            // for smaller xml can use document.Descendants("Cars)
            var query = from element in document.Element(ns + "Cars")?.Elements(ex + "Car") ??
                                        Enumerable.Empty<XElement>()
                where element.Attribute("Manufacturer")?.Value == "BMW"
                select element.Attribute("Name").Value;

            foreach (var name in query)
            {
                Console.WriteLine(name);
            }
        }

        private static void CreateXml()
        {
            var records = ProcessCars("fuel.csv");

            var document = new XDocument();
            var cars = new XElement(ns + "Cars",
                from record in records
                select new XElement(ex + "Car",
                    new XAttribute("Name", record.Name),
                    new XAttribute("Manufacturer", record.Manufacturer),
                    new XAttribute("Combined", record.Combined))
            );
            cars.Add(new XAttribute(XNamespace.Xmlns + "ex", ex));
            document.Add(cars);
            document.Save("fuel.xml");
        }

        
    }
}