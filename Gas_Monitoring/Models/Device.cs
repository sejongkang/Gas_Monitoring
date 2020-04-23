using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gas_Monitoring.Models
{
    public class Device: BindableBase
    {
        private int dev_idx;
        private string dev_name;
        private string dev_type;
        private string serial;
        private string local_name;
        private string dev_purch_date;
        private bool enable;
        private string conn;

        public string Dev_type { get => dev_type; set => dev_type = value; }
        public string Serial { get => serial; set => serial = value; }
        public string Local_name { get => local_name; set => local_name = value; }
        public string Dev_purch_date { get => dev_purch_date; set => dev_purch_date = value; }
        public bool Enable { get => enable; set => enable = value; }
        public string Conn { get => conn; set => conn = value; }
        public int Dev_idx { get => dev_idx; set => dev_idx = value; }
        public string Dev_name { get => dev_name; set => dev_name = value; }

        public Device(int dev_idx=0, string dev_name = "디바이스", string dev_type="Mems", string serial ="None", string local_name="None", string dev_purch_date="2019-08-09",bool enable=true, string conn="Off")
        {
            Dev_idx = dev_idx;
            Dev_name = dev_name;
            Dev_type = dev_type;
            Serial = serial;
            Local_name = local_name;
            Dev_purch_date = dev_purch_date;
            Enable = enable;
            Conn = conn;
        }
    }
}
