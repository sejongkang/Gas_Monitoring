using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm.POCO;
using DevExpress.Xpf.Charts;
using DevExpress.Xpf.Core;
using Gas_Monitoring.Models;
using MySql.Data.MySqlClient;

namespace Gas_Monitoring.ViewModels
{
    [POCOViewModel]
    public class DeviceViewModel : INotifyPropertyChanged
    {
        public int test_idx = 0;
        public Thread realtime_th;
        private Device dev;
        private DelegateCommand buttonCommand;
        private string device_name;
        private int dev_num;
        private string dev_serial;
        private DateTime date_time;
        private string loading;
        private DateTime selected_date;
        private string selected_term;
        private string selected_type;
        private string measureunit;
        private string multi;
        private string aggreate;
        private DelegateCommand pickcommand;
        private DelegateCommand termcommand;
        private DelegateCommand typecommand;
        private Range range;
        private bool is_loading;
        private string conn_img;
        private string conn_stk;
        private string conn_text;
        private string date_str;
        private DateTime max_date;
        private bool selected;


        public event PropertyChangedEventHandler PropertyChanged;

        public Device Dev { get => dev; set => dev = value; }
        public string Device_name { get => device_name; set => device_name = value; }
        public int Dev_num { get => dev_num; set => dev_num = value; }
        public virtual Result Dev_result { get; set; }
        public virtual string Date_time_str { get; set; }

        public virtual ObservableCollection<double> Ppm_list { get; set; }
        public virtual ObservableCollection<DataPoint> his_tmp { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his2 { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his3 { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his4 { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his5 { get; set; }
        public virtual ObservableCollection<DataPoint> Gas_his6 { get; set; }
        public virtual ObservableCollection<ResultSetting> Setting { get; set; }
        public virtual ObservableCollection<String> Term_list { get; set; }
        public virtual ObservableCollection<String> Type_list { get; set; }
        public virtual DateTime Selected_date { get => selected_date; set => selected_date = value; }
        public DelegateCommand ButtonCommand { get => buttonCommand; set => buttonCommand = value; }
        public DelegateCommand Pickcommand { get => pickcommand; set => pickcommand = value; }
        public Range Binding_Range { get => range; set => range = value; }
        public DelegateCommand Termcommand { get => termcommand; set => termcommand = value; }
        public DelegateCommand Typecommand { get => typecommand; set => typecommand = value; }
        public string Selected_term { get => selected_term; set => selected_term = value; }
        public string Selected_type { get => selected_type; set => selected_type = value; }
        public string Measureunit { get => measureunit; set => measureunit = value; }
        public string Multi { get => multi; set => multi = value; }
        public string Aggreate { get => aggreate; set => aggreate = value; }
        public string Dev_serial { get => dev_serial; set => dev_serial = value; }
        public string Loading { get => loading; set => loading = value; }
        public bool Is_loading { get => is_loading; set => is_loading = value; }
        public DateTime Date_time { get => date_time; set => date_time = value; }
        public virtual string Conn_img { get => conn_img; set => conn_img = value; }
        public string Conn_stk { get => conn_stk; set => conn_stk = value; }
        public string Conn_text { get => conn_text; set => conn_text = value; }
        public string Date_str { get => date_str; set => date_str = value; }
        public DateTime Max_date { get => max_date; set => max_date = value; }
        public bool Selected { get => selected; set => selected = value; }

        public List<string> Src_enable = new List<string> {
                Path.GetFullPath("../../images/nose.png"),
                Path.GetFullPath("../../images/hazard.png"),
            };
        public List<string> Src_disable = new List<string> {
                Path.GetFullPath("../../images/nose2.png"),
                Path.GetFullPath("../../images/hazard2.png"),
            };
        public List<string> Src_conn = new List<string> {
                Path.GetFullPath("../../images/conn.png"),
                Path.GetFullPath("../../images/disconn.png"),
            };
        
        public DeviceViewModel()
        {
            Console.WriteLine(test_idx.ToString() + "." + "DeviceVIewModel 기본생성자");
        }
        public void init(int num, string serial)
        {
            Console.WriteLine(test_idx.ToString() + "." + "DeviceVIewModel init 2 param");
            Selected = true;
            Binding_Range = new Range();
            Ppm_list = new ObservableCollection<double>();
            
            Gas_his = new ObservableCollection<DataPoint>();
            Gas_his2 = new ObservableCollection<DataPoint>();
            Gas_his3 = new ObservableCollection<DataPoint>();
            Gas_his4 = new ObservableCollection<DataPoint>();
            Gas_his5 = new ObservableCollection<DataPoint>();
            Gas_his6 = new ObservableCollection<DataPoint>();
            Term_list = new ObservableCollection<string> { "10분", "30분", "1시간", "3시간", "6시간" };
            Type_list = new ObservableCollection<string> { "Average", "Minimum", "Maximum"};
            Setting = new ObservableCollection<ResultSetting>
            {
                new ResultSetting(),
                new ResultSetting(),
                new ResultSetting(),
                new ResultSetting(),
                new ResultSetting(),
                new ResultSetting()
            };
            Pickcommand = new DelegateCommand(Pick);
            ButtonCommand = new DelegateCommand(Click);
            Termcommand = new DelegateCommand(Pick_Term);
            Typecommand = new DelegateCommand(Pick_Type);

            realtime_th = new Thread(new ThreadStart(Refresh));

            Dev_num = num;
            Device_name = "디바이스 " + num;
            Dev_serial = serial;
            //Selected_date = DateTime.Now;
            Console.WriteLine(test_idx.ToString() + "." + "생성자, " + Selected_date.ToString());
            Selected_term = "6시간";
            Selected_type = "Maximum";
            Measureunit = "Hour";
            Multi = "6";
            Date_time_str = "-";
            Loading = "Hidden";
            Aggreate = Selected_type;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMinutes(5);
            timer.Tick += new EventHandler(Timer_Tick);
        
            Max_date = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
            init_device();
            realtime_th.IsBackground = true;
            realtime_th.Start();
            timer.Start();
            
            Console.WriteLine(test_idx.ToString() + "." + Device_name);
        }
        public void init()
        {
            Console.WriteLine(test_idx.ToString() + "." + "DeviceVIewModel init");
            Selected_date = DateTime.Now;
            Selected_term = "6시간";
            Selected_type = "Maximum";
            Measureunit = "Hour";
            Multi = "6";
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            Max_date = DateTime.Now.AddDays(1).Date.AddSeconds(-1);
        }
        public void Pick_Term()
        {
            if (Selected_term == Term_list[0])
            {
                Measureunit = "Minute";
                Multi = "10";
            }
            else if (Selected_term == Term_list[1])
            {
                Measureunit = "Minute";
                Multi = "30";
            }
            else if (Selected_term == Term_list[2])
            {
                Measureunit = "Hour";
                Multi = "1";
            }
            else if (Selected_term == Term_list[3])
            {
                Measureunit = "Hour";
                Multi = "3";
            }
            else
            {
                Measureunit = "Hour";
                Multi = "6";
            }
        }  
        public void Pick_Type()
        {
            Aggreate = Selected_type;
        }
        public void Click()
        {
            MainViewModel.Main_loading = "True";
            Messenger.Default.Send<int, MainViewModel>(Dev_num);
        }
        public void init_device()
        {
            DB init_db = new DB();
            if (init_db.DBConn() == false)
            {
                //Messenger.Default.Send<bool, MainViewModel>(true);
            }
            string sql = "SELECT * FROM gas.gas_module WHERE idx = " + Dev_num + ";";
            MySqlDataReader rdr = init_db.select(sql);
            while (rdr.Read())
            {
                string date = string.Format("{0:yyyy-MM-dd}", rdr.GetDateTime(4));
                Dev = new Device(rdr.GetInt32(0),"디바이스"+rdr.GetInt32(0).ToString(), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), date, rdr.GetBoolean(5), rdr.GetString(6));
            }
            rdr.Close();
        }

        public void Refresh()
        {
            int compare = 0;
            DateTime tmp_now;
            while (true)
            {
                if(MainViewModel.Network_loading == "False")
                {
                    try
                    {
                        DB refresh_db = new DB();
                        if (refresh_db.DBConn() == false)
                        {
                            Messenger.Default.Send<bool, MainViewModel>(true);
                        }
                        String sql = "SELECT ppm,ppm2,ppm3,ppm4,ppm5,ppm6,regdate,loading FROM gas.result_info INNER JOIN gas.gas_log ON gas.result_info.log_idx = gas.gas_log.idx " +
                            "WHERE gas_module_idx = '" + Dev_serial + "' ORDER BY regdate DESC limit 1;";
                        MySqlDataReader rdr = refresh_db.select(sql);
                        Dev_result = new Result();
                        Ppm_list.Clear();
                        while (rdr.Read())
                        {
                            for(int i=0;i<6;i++)
                            {
                                double tmp = Math.Truncate(rdr.GetDouble(i));
                                if (tmp < 0)
                                {
                                    Ppm_list.Add(0);
                                }
                                else
                                {
                                    if (i == 0 || i == 2)
                                    {
                                        if (tmp > 30)
                                        {
                                            Ppm_list.Add(30);
                                        }
                                        else
                                        {
                                            Ppm_list.Add(tmp);
                                        }
                                    }
                                    else if (i == 1 || i == 3)
                                    {
                                        if (tmp > 300)
                                        {
                                            Ppm_list.Add(300);
                                        }
                                        else
                                        {
                                            Ppm_list.Add(tmp);
                                        }
                                    }
                                    else if (i == 4)
                                    {
                                        if (tmp > 3000)
                                        {
                                            Ppm_list.Add(3000);
                                        }
                                        else
                                        {
                                            Ppm_list.Add(tmp);
                                        }
                                    }
                                    else
                                    {
                                        if (tmp > 10000)
                                        {
                                            Ppm_list.Add(10000);
                                        }
                                        else
                                        {
                                            Ppm_list.Add(tmp);
                                        }
                                    }
                                }
                            }
                            Dev_result = new Result(Ppm_list[0], Ppm_list[1], Ppm_list[2], Ppm_list[3], Ppm_list[4], Ppm_list[5]);
                            Date_time = rdr.GetDateTime(6);
                            Date_str = String.Format("{0:yyyy-MM-dd}", Date_time);
                            Date_time_str = String.Format("{0:yyyy-MM-dd HH:mm:ss}", Date_time);
                            Is_loading = rdr.GetBoolean(7);
                        }
                        rdr.Close();
                        tmp_now = DateTime.Now.AddMinutes(-1);
                        compare = DateTime.Compare(tmp_now, Date_time);

                        if (compare < 0)
                        {
                            Dev.Conn = "ON";
                        }
                        else
                        {
                            Dev.Conn = "OFF";
                        }
                        sql = "UPDATE gas.gas_module SET conn = '" + Dev.Conn + "' WHERE idx=" + Dev.Dev_idx + ";";
                        refresh_db.update(sql);
                        Result_Set();
                        refresh_db.Conn.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else
                {
                    Console.WriteLine("Refresh pass");
                    Dev.Conn = "OFF";
                    Result_Set();
                }
                Thread.Sleep(4000);
            }
        }

        public void Result_Set()
        {
            if (Dev.Conn == "ON")
            {
                Conn_img = Src_conn[0];
                Conn_stk = "#FF64C54F";
                Conn_text = "Hidden";
            }
            else
            {
                Conn_img = Src_conn[1];
                Conn_stk = "#FFB2B2B2";
                Conn_text = "Visible";
                Is_loading = false;
            }
            if (Is_loading == true)
            {
                Loading = "Visible";
            }
            else
            {
                Loading = "Hidden";
            }

            if (Dev_result.Gas_ppm == 0)
            {
                Setting[0].Set("황화수소", "H2S", "#FFDAE5E6", "{x:Null}", Src_disable[0], "White", "White", "Hidden");
            }
            else if(Dev_result.Gas_ppm == 30)
            {
                Setting[0].Set("황화수소", "H2S", "#FF63ADB8", "Black", Src_enable[0], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[0].Set("황화수소", "H2S", "#FF63ADB8", "Black", Src_enable[0], "Black", "Black", "Hidden");
            }
            if (Dev_result.Gas_ppm2 == 0)
            {
                Setting[1].Set("암모니아", "NH3", "#FFF5FBF3", "{x:Null}", Src_disable[0], "White", "White", "Hidden");
            }
            else if (Dev_result.Gas_ppm2 == 300)
            {
                Setting[1].Set("암모니아", "NH3", "#FF93A68D", "Black", Src_enable[0], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[1].Set("암모니아", "NH3", "#FF93A68D", "Black", Src_enable[0], "Black", "Black", "Hidden");
            }
            if (Dev_result.Gas_ppm3 == 0)
            {
                Setting[2].Set("메틸메르캅탄", "CH3SH", "#FFF2ECF7", "{x:Null}", Src_disable[0], "White", "White", "Hidden");
            }
            else if (Dev_result.Gas_ppm3 == 30)
            {
                Setting[2].Set("메틸메르캅탄", "CH3SH", "#FF85788F", "Black", Src_enable[0], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[2].Set("메틸메르캅탄", "CH3SH", "#FF85788F", "Black", Src_enable[0], "Black", "Black", "Hidden");
            }
            if (Dev_result.Gas_ppm4 == 0)
            {
                Setting[3].Set("일산화탄소", "CO", "#FFFFF8EF", "{x:Null}", Src_disable[1], "White", "White", "Hidden");
            }
            else if (Dev_result.Gas_ppm4 == 300)
            {
                Setting[3].Set("일산화탄소", "CO", "#FFC59F71", "Black", Src_enable[1], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[3].Set("일산화탄소", "CO", "#FFC59F71", "Black", Src_enable[1], "Black", "Black", "Hidden");
            }
            if (Dev_result.Gas_ppm5 == 0)
            {
                Setting[4].Set("이산화탄소", "CO2", "#FFFBF0F1", "{x:Null}", Src_disable[1], "White", "White", "Hidden");
            }
            else if (Dev_result.Gas_ppm5 == 3000)
            {
                Setting[4].Set("이산화탄소", "CO2", "#FFA27076", "Black", Src_enable[1], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[4].Set("이산화탄소", "CO2", "#FFA27076", "Black", Src_enable[1], "Black", "Black", "Hidden");
            }
            if (Dev_result.Gas_ppm6 == 0)
            {
                Setting[5].Set("메탄", "CH4", "#FFF3F0E9", "{x:Null}", Src_disable[1], "White", "White", "Hidden");
            }
            else if (Dev_result.Gas_ppm6 == 10000)
            {
                Setting[5].Set("메탄", "CH4", "#FF918262", "Black", Src_enable[1], "Black", "#FF2C3FF3", "Visible");
            }
            else
            {
                Setting[5].Set("메탄", "CH4", "#FF918262", "Black", Src_enable[1], "Black", "Black", "Hidden");
            }
        }
        public void Pick()
        {
            if (string.Format("{0:yyyy-MM-dd}", Selected_date) == "0001-01-01") //자체 초기화 값 
            {
                return;
            }
            MainViewModel.Main_loading = "True";
            Get_ppm();

            Gas_his = his_tmp; //얕은 복사 필요-한번에 교체
            Gas_his2 = his_tmp;
            Gas_his3 = his_tmp;
            Gas_his4 = his_tmp;
            Gas_his5 = his_tmp;
            Gas_his6 = his_tmp;
            MainViewModel.Main_loading = "False";
        }
        public void Get_ppm()
        {
            string sql;
            try
            {
                DB tmp_db = new DB();

                if (tmp_db.DBConn() == false)
                {
                    Messenger.Default.Send<bool, MainViewModel>(true);
                }
                his_tmp = new ObservableCollection<DataPoint>();
                sql = "SELECT regdate, ppm, ppm2, ppm3, ppm4, ppm5, ppm6 FROM gas.result_info INNER JOIN gas.gas_log ON gas.result_info.log_idx = gas.gas_log.idx " +
                    "WHERE gas_module_idx = '" + Dev_serial + "' AND regdate LIKE '" + String.Format("{0:yyyy-MM-dd}", Selected_date) + "%';";
                MySqlDataReader rdr = tmp_db.select(sql);
                while (rdr.Read())
                {
                    his_tmp.Add(new DataPoint(rdr.GetDateTime(0), rdr.GetDouble(1), rdr.GetDouble(2), rdr.GetDouble(3), rdr.GetDouble(4), rdr.GetDouble(5), rdr.GetDouble(6)));
                }
                rdr.Close(); ;
                tmp_db.Conn.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            Console.WriteLine("갱신끝");
        }
        protected void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}