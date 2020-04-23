using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using MySql.Data.MySqlClient;
using Gas_Monitoring.ViewModels;
using Gas_Monitoring.Models;
using Gas_Monitoring.Views;
using DevExpress.Mvvm.POCO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace Gas_Monitoring.ViewModels 
{
    [POCOViewModel]
    public class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<DeviceView> deviceview = new ObservableCollection<DeviceView>();
        public ObservableCollection<DeviceView> Deviceview { get => deviceview; set => deviceview = value; }
        public HomeViewModel()
        {
        }
        public void Init()
        {
            Deviceview.Clear();
            foreach (DeviceView v in MainViewModel.Devicev)
            {
                if((v.DataContext as DeviceViewModel).Dev.Enable == true)
                {
                    Deviceview.Add(v);
                }
            }
        }
        public void Pop_Setting()
        {
            try
            {
                Messenger.Default.Send<string, MainViewModel>("dev_set");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void Help()
        {
            System.Diagnostics.Process.Start(Path.GetFullPath("../../help.html"));
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