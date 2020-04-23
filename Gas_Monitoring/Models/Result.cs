using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas_Monitoring.Models
{
    public class Result
    {
        private double gas_ppm;
        private double gas_ppm2;
        private double gas_ppm3;
        private double gas_ppm4;
        private double gas_ppm5;
        private double gas_ppm6;

        public double Gas_ppm { get => gas_ppm; set => gas_ppm = value; }
        public double Gas_ppm2 { get => gas_ppm2; set => gas_ppm2 = value; }
        public double Gas_ppm3 { get => gas_ppm3; set => gas_ppm3 = value; }
        public double Gas_ppm4 { get => gas_ppm4; set => gas_ppm4 = value; }
        public double Gas_ppm5 { get => gas_ppm5; set => gas_ppm5 = value; }
        public double Gas_ppm6 { get => gas_ppm6; set => gas_ppm6 = value; }

        public Result(double gas_ppm=0, double gas_ppm2 = 0, double gas_ppm3 = 0, double gas_ppm4 = 0, double gas_ppm5 = 0, double gas_ppm6 = 0)
        {
            Gas_ppm = gas_ppm;
            Gas_ppm2 = gas_ppm2;
            Gas_ppm3 = gas_ppm3;
            Gas_ppm4 = gas_ppm4;
            Gas_ppm5 = gas_ppm5;
            Gas_ppm6 = gas_ppm6;
        }
    }
}
