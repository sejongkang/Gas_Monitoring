using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using Gas_Monitoring.Models;
using Gas_Monitoring.Views;
using MySql.Data.MySqlClient;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Core;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using DevExpress.Xpf.WindowsUI;
using static DevExpress.Xpf.Core.WindowedDocumentUIService;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace Gas_Monitoring.ViewModels
{
    [POCOViewModel]
    public class MainViewModel : INotifyPropertyChanged
    {
        public static DB db;
        private List<int> dev_list;
        private List<int> sel_list;
        public SettingViewModel setting_vm;
        private DXTabItem tabitem;
        private HomeView home_view;
        private ObservableCollection<int> remove_list = new ObservableCollection<int>();
        private HomeViewModel home_viewmodel;
        private ObservableCollection<DXTabItem> tabItems;
        private static ObservableCollection<DeviceView> devicev;
        private static ObservableCollection<TabView> tabViews;
        private ObservableCollection<DXTabItem> init_tabs;
        private static string main_loading;
        private static string network_loading;
        private static string network_loading2;
        private string binding;
        private string binding2;

        public event PropertyChangedEventHandler PropertyChanged;

        [ServiceProperty(SearchMode = ServiceSearchMode.PreferParents)]
        public virtual IDocumentManagerService DocumentManagerService { get { return null; } }

        public HomeViewModel Home_viewmodel { get => home_viewmodel; set => home_viewmodel = value; }
        public HomeView Home_view { get => home_view; set => home_view = value; }
        public ObservableCollection<DXTabItem> TabItems { get => tabItems; set => tabItems = value; }
        public DXTabItem Tabitem { get => tabitem; set => tabitem = value; }
        public static ObservableCollection<DeviceView> Devicev { get => devicev; set => devicev = value; }
        public ObservableCollection<int> Remove_list { get => remove_list; set => remove_list = value; }
        public List<int> Dev_list { get => dev_list; set => dev_list = value; }
        public List<int> Sel_list { get => sel_list; set => sel_list = value; }
        public ObservableCollection<TabView> TabViews { get => tabViews; set => tabViews = value; }
        public ObservableCollection<DXTabItem> Init_tabs { get => init_tabs; set => init_tabs = value; }
        public Thread checknet_th { get; private set; }
        public static string Network_loading { get => network_loading; set => network_loading = value; }
        public static string Network_loading2 { get => network_loading2; set => network_loading2 = value; }
        public static string Main_loading { get => main_loading; set => main_loading = value; }
        public string Binding { get => binding; set => binding = value; }
        public string Binding2 { get => binding2; set => binding2 = value; }
        Process[] procs = Process.GetProcessesByName("Gas_Monitoring");
        public MainViewModel()
        {
            if (procs.Length > 1)
            {
                DXMessageBox.Show("프로그램이 이미 실행 중입니다.", "악취 및 유해가스 모니터링 시스템 v1.0");
                System.Environment.Exit(0);
            }
            else
            {
                Network_loading = "False";
                Network_loading2 = "Hidden";

                Binding = Network_loading;
                Binding2 = Network_loading2;

                Main_loading = "False";
                TabItems = new ObservableCollection<DXTabItem>();
                Init_tabs = new ObservableCollection<DXTabItem>();
                Devicev = new ObservableCollection<DeviceView>();
                Dev_list = new List<int>(new int[] { 1, 2, 3, 4, 5 });
                Sel_list = new List<int>();
                db = new DB();
                if (db.DBConn() == false)
                {
                    DBError(true);
                }

                //Messenger.Default.Register<bool>(this, DBError);
                Messenger.Default.Register<int>(this, ClickMessage);
                Messenger.Default.Register<string>(this, PopSetting);

                String sql = "SELECT idx,serial FROM gas.gas_module ORDER BY idx;";
                MySqlDataReader rdr = db.select(sql);
                while (rdr.Read())
                {
                    DeviceView dev_v = new DeviceView();
                    DXTabItem tab_tmp = new DXTabItem();
                    TabView tab_v = new TabView();
                    DeviceViewModel vm = new DeviceViewModel();
                    int num = rdr.GetInt16(0);
                    string serial = rdr.GetString(1);
                    vm.init(num, serial);

                    dev_v.DataContext = vm;
                    Devicev.Add(dev_v);
                    tab_v.DataContext = vm;
                    tab_tmp.Header = vm.Dev.Dev_name;
                    tab_tmp.Content = tab_v;
                    Init_tabs.Add(tab_tmp);
                }
                rdr.Close();
                Home_view = new HomeView();
                (Home_view.DataContext as HomeViewModel).Init();
                Tabitem = new DXTabItem();
                Tabitem.Header = "홈";
                Tabitem.Content = Home_view;
                Tabitem.AllowHide = DevExpress.Utils.DefaultBoolean.False;
                TabItems.Add(Tabitem);

                Console.WriteLine("Main 생성자 끝");
                checknet_th = new Thread(new ThreadStart(CheckNet));
                checknet_th.IsBackground = true;
                checknet_th.Start();
            }
        }
        public void CheckNet()
        {
            Ping pingSender = new Ping();
            while (true)
            {
                try
                {
                    PingReply reply = pingSender.Send(db.Address);
                    if (reply.Status == IPStatus.Success)
                    {
                        Console.WriteLine("Ping Success");
                        Network_loading = "False";
                        Network_loading2 = "Hidden";

                    }
                    else
                    {
                        Console.WriteLine("Ping Fail");
                        Network_loading = "True";
                        Network_loading2 = "Visible";
                    }
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                    Console.WriteLine("Ping Fail");
                    Network_loading = "True";
                    Network_loading2 = "Visible";
                }
                Binding = Network_loading;
                Binding2 = Network_loading2;
                Thread.Sleep(1000);
            }
        }
        public void DBError(bool iserror)
        {

            if (DXMessageBox.Show("DB 네트워크에 연결할 수 없습니다. 확인 후 다시 실행해주세요.", "DB 접속 불가", MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                System.Windows.Application.Current.Shutdown();
            }
        }
        public void PopSetting(string bang)
        {
            if(bang == "dev_set")
            {
                setting_vm = new SettingViewModel();
                setting_vm.init();

                UICommand okCommand = new UICommand()
                {
                    Caption = "OK",
                    IsCancel = false,
                    IsDefault = true
                };

                UICommand cancelCommand = new UICommand()
                {
                    Id = MessageBoxResult.Cancel,
                    Caption = "Cancel",
                    IsCancel = true,
                    IsDefault = false,
                };

                IDialogService service = this.GetService<IDialogService>();

                UICommand result = service.ShowDialog(
                    dialogCommands: new List<UICommand>() { okCommand, cancelCommand },
                    title: "디바이스 선택",
                    viewModel: setting_vm);

                if (result == okCommand)
                {
                    int home_idx=0;
                    ObservableCollection<Device>tmp = setting_vm.Selected_list;
                    Sel_list.Clear();
                    Remove_list.Clear();
                    
                    foreach (DeviceView v in Devicev)
                    {
                        (v.DataContext as DeviceViewModel).Dev.Enable = false;
                    }
                    foreach (Device dev in tmp)
                    {
                        dev.Enable = true;
                    }
                    ObservableCollection<DXTabItem> tmp_list = new ObservableCollection<DXTabItem>();

                    for(int i=0; i<TabItems.Count;i++)
                    {
                        if (TabItems[i].Header.ToString() == "홈")
                        {
                            tmp_list.Add(TabItems[i]);
                            home_idx = tmp_list.Count;
                            continue;
                        }

                        for (int j = 0; j< tmp.Count; j++)
                        {
                            if(TabItems[i].Header.ToString() == tmp[j].Dev_name)
                            {
                                tmp_list.Add(TabItems[i]);
                            }
                        }
                    }
                    TabItems.Clear();
                    TabItems = tmp_list;
                    (Home_view.DataContext as HomeViewModel).Init();
                    Tabitem = TabItems[home_idx-1];
                }
            }
        }

        public void ClickMessage(int dev_num)
        {
            for(int i=0; i<TabItems.Count; i++)
            {
                DXTabItem tmp = TabItems[i];
                if (tmp.Header.ToString() != "홈")
                {
                    if(((tmp.Content as TabView).DataContext as DeviceViewModel).Dev_num == dev_num)
                    {
                        Tabitem = tmp;
                        return;
                    }
                }
            }
            ((Init_tabs[dev_num - 1].Content as TabView).DataContext as DeviceViewModel).init();
            Init_tabs[dev_num - 1].Visibility = Visibility.Visible;
            
            TabItems.Add(Init_tabs[dev_num - 1]);
            Tabitem = Init_tabs[dev_num - 1];
        }
        
        public void Hiding(CancelEventArgs e)
        {

            if (DXMessageBox.Show(Application.Current.MainWindow,"탭을 닫으시겠습니까?", "탭 닫기 확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        public void Closing(CancelEventArgs e)
        {
            if(DXMessageBox.Show(Application.Current.MainWindow, "정말로 종료하시겠습니까?","프로그램 종료 확인",MessageBoxButton.YesNo)==MessageBoxResult.Yes)
            {
                e.Cancel=false;
            }
            else
            {
                e.Cancel = true;
            }
        }
        public void Help()
        {
            System.Diagnostics.Process.Start(Path.GetFullPath("../../help.html"));
        }
        public static MainViewModel Create()
        {
            return ViewModelSource.Create(() => new MainViewModel());
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