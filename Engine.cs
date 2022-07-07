using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
    public class Engine
    {
        public Engine() { }
        public Engine(double displacement, double horsePower, string model)
        {
            this.displacement = displacement;
            this.horsePower = horsePower;
            this.model = model;
        }
        public double displacement { get; }
        public double horsePower { get; }
        public string model { get; }
    }
}
