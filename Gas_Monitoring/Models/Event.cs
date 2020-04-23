using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas_Monitoring.Models
{
    public class Event
    {
        private int idx;
        private int dev_num;
        private string gas;
        private int ppm;
        private string date;
        private string time;
        public int Idx { get => idx; set => idx = value; }
        public int Dev_num { get => dev_num; set => dev_num = value; }
        public string Gas { get => gas; set => gas = value; }
        public int Ppm { get => ppm; set => ppm = value; }
        public string Date { get => date; set => date = value; }
        public string Time { get => time; set => time = value; }

        public Event(int idx, int dev_num, string gas, int ppm, string date, string time)
        {
            Idx = idx;
            Dev_num = dev_num;
            Gas = gas;
            Ppm = ppm;
            Date = date;
            Time = time;
        }
    }
}
