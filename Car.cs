using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Car
    {
        public Car() { }
        public Car(string model, Engine engine, int year) 
        {
            this.model = model;
            this.year = year;
            motor = engine;
        }
        public string model { get; }
        public int year { get; }
        public Engine motor { get; }
    }
}
