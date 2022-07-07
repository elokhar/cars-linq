using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Car> myCars = new List<Car>() {
                new Car("E250", new Engine(1.8, 204, "CGI"), 2009),
                new Car("E350", new Engine(3.5, 292, "CGI"), 2009),
                new Car("A6", new Engine(2.5, 187, "FSI"), 2012),
                new Car("A6", new Engine(2.8, 220, "FSI"), 2012),
                new Car("A6", new Engine(3.0, 295, "TFSI"), 2012),
                new Car("A6", new Engine(2.0, 175, "TDI"), 2011),
                new Car("A6", new Engine(3.0, 309, "TDI"), 2011),
                new Car("S6", new Engine(4.0, 414, "TFSI"), 2012),
                new Car("S8", new Engine(4.0, 513, "TFSI"), 2012)
            };

            var query1 = from e in myCars
                         where e.model == "A6"
                         select new
                         {
                             engineType = e.motor.model == "TDI" ? "diesel" : "petrol",
                             hppl = e.motor.horsePower / e.motor.displacement
                         };

            var query2 = from e in query1
                         group e by e.engineType;
                         
                         

            foreach (var e in query1)
            {
                Console.WriteLine(e);
            }

            Console.WriteLine();

            foreach (var myGroup in query2)
            {
                Console.Write("{0}: ", myGroup.Key);
                var avg = (from e in myGroup
                           select e.hppl).Average();
                Console.WriteLine(avg);                   
            }

            var root = new XElement("cars");
            foreach(var car in myCars)
            {
                var carElement = new XElement("car");
                carElement.Add(new XElement("model", car.model));
                var engineElement = new XElement("engine");
                engineElement.Add(new XAttribute("model", car.motor.model));
                engineElement.Add(new XElement("displacement", car.motor.displacement));
                engineElement.Add(new XElement("horsePower", car.motor.horsePower));
                carElement.Add(engineElement);
                carElement.Add(new XElement("year", car.year));
                root.Add(carElement);
            }

            root.Save("CarsCollection.xml");

            root = XElement.Load("CarsCollection.xml");
            myCars.Clear();

            foreach(var child in root.Elements())
            {
                var model = (string)child.Element("model");
                var engModel = (string)child.Element("engine").Attribute("model");
                var displacement = (double)child.Element("engine").Element("displacement");
                var horsePower = (double)child.Element("engine").Element("horsePower");
                var year = (int)child.Element("year");

                var engine = new Engine(displacement, horsePower, engModel);
                var car = new Car(model, engine, year);

                myCars.Add(car);               
            }

            XElement rootNode = XElement.Load("CarsCollection.xml");
            var firstXPathExpression = "sum(/car[engine/@model!='TDI']/engine/horsePower) div count(/car[engine/@model!='TDI'])";
            double avgHP = (double)rootNode.XPathEvaluate(firstXPathExpression);

            Console.WriteLine();
            Console.WriteLine("Srednia moc samochodow o silnikach innych niz TDI: {0}", avgHP);

            var secondXPathExpression = "/car/model[not(. = ../following-sibling::car/model)]";
            IEnumerable<XElement> models = rootNode.XPathSelectElements(secondXPathExpression);

            Console.WriteLine();
            Console.WriteLine("Modele samochodow bez powtorzen:");
            foreach(var model in models)
            {
                Console.WriteLine(model);
            }

            createXmlFromLinq(myCars);

            var htmlRoot = XElement.Load("Template.html");

            var table = new XElement("table");

            foreach(var car in myCars)
            {
                
                var row = new XElement("tr");
      
                row.Add(new XElement("td", car.model));
                row.Add(new XElement("td", car.motor.model));
                row.Add(new XElement("td", car.motor.displacement));
                row.Add(new XElement("td", car.motor.horsePower));
                row.Add(new XElement("td", car.year));
                table.Add(row);
            }

            var body = new XElement("body", table);
            htmlRoot.Add(body);

            htmlRoot.Save("Cars.html");
           
        }

        private static void createXmlFromLinq(List<Car> myCars)
        {
            IEnumerable<XElement> nodes = from car in myCars
                                          select new XElement("car",
                                            new XElement("model", car.model),
                                            new XElement("engine",
                                                new XAttribute("model", car.motor.model),
                                                new XElement("displacement", car.motor.displacement),
                                                new XElement("horsePower", car.motor.horsePower)
                                                ),
                                            new XElement("year", car.year));

                XElement rootNode = new XElement("cars",nodes); //create a root node to contain the query results
            rootNode.Save("CarsFromLinq.xml");
        }
    }
}
