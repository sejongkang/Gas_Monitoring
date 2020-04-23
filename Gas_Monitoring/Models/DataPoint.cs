using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;


namespace Gas_Monitoring.Models
{
    public class DataPoint
    {
        public DateTime Argument { get; set; }
        public double Ppm { get; set; }
        public double Ppm2 { get; set; }
        public double Ppm3 { get; set; }
        public double Ppm4 { get; set; }
        public double Ppm5 { get; set; }
        public double Ppm6 { get; set; }
        //public static ObservableCollection<DataPoint> GetDataPoints()
        //{
        //    return new ObservableCollection<DataPoint> {
        //            new DataPoint { Argument = "Asia", Value = 5.289D},
        //            new DataPoint { Argument = "Australia", Value = 2.2727D},
        //            new DataPoint { Argument = "Europe", Value = 3.7257D},
        //            new DataPoint { Argument = "North America", Value = 4.1825D},
        //            new DataPoint { Argument = "South America", Value = 2.1172D}
        //           };
        //}
        public DataPoint(DateTime argument, double ppm=0, double ppm2=0, double ppm3=0, double ppm4=0, double ppm5=0, double ppm6=0)
        {
            Argument = argument;
            if(ppm>1000000)
            {
                Ppm = 1000000;
            }
            else
            {
                Ppm = ppm;
            }
            if (ppm2 > 1000000)
            {
                Ppm2 = 1000000;
            }
            else
            {
                Ppm2 = ppm2;
            }
            if (ppm3 > 1000000)
            {
                Ppm3 = 1000000;
            }
            else
            {
                Ppm3 = ppm3;
            }
            if (ppm4 > 1000000)
            {
                Ppm4 = 1000000;
            }
            else
            {
                Ppm4 = ppm4;
            }
            if (ppm5 > 1000000)
            {
                Ppm5 = 1000000;
            }
            else
            {
                Ppm5 = ppm5;
            }
            if (ppm6 > 1000000)
            {
                Ppm6 = 1000000;
            }
            else
            {
                Ppm6 = ppm6;
            }
        }
    }
}
