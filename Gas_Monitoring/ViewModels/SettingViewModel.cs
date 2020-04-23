using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using Gas_Monitoring.Models;
using System.Windows;
using System.Collections.Generic;
using DevExpress.Mvvm.POCO;
using System.Threading;
using System.ComponentModel;
using Gas_Monitoring.Views;
using System.Windows.Threading;

namespace Gas_Monitoring.ViewModels
{
    [POCOViewModel]
    public class SettingViewModel:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public static ObservableCollection<Device> Dev_list { get; set; }
        public virtual ObservableCollection<Device> Selected_list { get; set; }
        public SettingViewModel()
        {
            
        }

        public void init()
        {
            Dev_list = new ObservableCollection<Device>();
            Selected_list = new ObservableCollection<Device>();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();

            Dev_list.Clear();
            Selected_list.Clear();
            foreach (DeviceView v in MainViewModel.Devicev)
            {
                Device dev = (v.DataContext as DeviceViewModel).Dev;
                Dev_list.Add(dev);
                if (dev.Enable == true)
                {
                    Selected_list.Add(dev);
                }
            }
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            foreach (DeviceView v in MainViewModel.Devicev)
            {
                DeviceViewModel vm = v.DataContext as DeviceViewModel;
                Dev_list[vm.Dev_num-1].Conn = vm.Dev.Conn;
            }
        }
        //protected void RaisePropertyChanged(string propertyName)
        //{
        //    // take a copy to prevent thread issues
        //    PropertyChangedEventHandler handler = PropertyChanged;
        //    if (handler != null)
        //    {
        //        handler(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }

}