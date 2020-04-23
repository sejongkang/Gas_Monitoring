using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Threading;
using System.Windows.Forms;

namespace Check
{
    [RunInstaller(true)]
    public class Program : System.Configuration.Install.Installer
    {
        public Program() : base()
        {
            this.BeforeUninstall += Program_BeforeUninstall;
        }

        static int Main(string[] args)
        {
            return 0;
        }   
        private void Program_BeforeUninstall(object sender, InstallEventArgs e)
        {
            Check check = new Check();
            int value = check.check();
            
            if (value == 1)
            {
                
                throw new System.Configuration.Install.InstallException();
            }
            else
            {
                //MessageBox.Show("된다");
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }

    }

    class Check
    {
        Process[] procs = Process.GetProcessesByName("gas_monitoring");
        public int check()
        {

            if (procs.Length >= 1)
            {
                MessageBox.Show(String.Format("{0}", procs.Length));
                Console.WriteLine("실행 중");
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
