using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Principal;
using Microsoft.Win32;

namespace RamFixForMSIBoards
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string nonPath = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management";
        private string netPath = "HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\Ndu";
        private bool ElevationStatus;

        public MainWindow()
        {
            InitializeComponent();
            checkElevation();
            getDefaults();
        }

        private void checkElevation()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            ElevationStatus = principal.IsInRole(WindowsBuiltInRole.Administrator);
            //bool ElevationStatus = WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);

            if (!ElevationStatus)
            {
                permStatus.Text = "⛔ This Application needs to be run as an Administrator to function ";
            }
            else
            {
                permStatus.Text = "⚡ Everything is ready to run ";
            }
        }

        private void setNonPagedSize(object sender, RoutedEventArgs e)
        {
            checkElevation();
            if (ElevationStatus)
            {
                //string path = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management";
                //int startingNumber =
                //    Convert.ToInt32(Registry.GetValue(path, "NonPagedPoolSize", -1));
                int newSize;
                if (int.TryParse(nonPagedSize.Text, out newSize))
                {
                    MessageBox.Show(newSize.ToString("X"));
                }
                else
                {
                    MessageBox.Show("Please Input a Valid Number");
                }
            }
            else
            {
                MessageBox.Show("Cannot Set Non Paged Pool Size without admin permissions", "Error");
            }
        }

        private void setNetworkStartVar() { 
            checkElevation();
            if (ElevationStatus)
            {
                //string path = "HKEY_LOCAL_MACHINE\\SYSTEM\\CurrentControlSet\\Control\\Session Manager\\Memory Management";
                //int startingNumber =
                //    Convert.ToInt32(Registry.GetValue(path, "NonPagedPoolSize", -1));
                int newSize;
                if (int.TryParse(nonPagedSize.Text, out newSize))
                {
                    MessageBox.Show(newSize.ToString("X"));
                }
                else
                {
                    MessageBox.Show("Please Input a Valid Number");
                }
            }
            else
            {
                MessageBox.Show("Cannot Set Non Paged Pool Size without admin permissions", "Error");
            }
        }

        private void getDefaults(){ getCurrentNonPagedSize(); getCurrentNetworkStartSetting();}

        private void getCurrentNonPagedSize()
        {
            string path =
            int startingNumber = 
                Convert.ToInt32(Registry.GetValue(path, "NonPagedPoolSize", -1));

            nonPagedSize.Text = "Loading . . .";

            if (startingNumber > 0)
            {
                nonPagedSize.Text = Convert.ToString(startingNumber);
            }
            else
            {
                nonPagedSize.Text = "0";
            }
        }

        private void getCurrentNetworkStartSetting()
        {
            string path = "HKEY_LOCAL_MACHINE\\SYSTEM\\ControlSet001\\Services\\Ndu";
            int startingNumber =
                Convert.ToInt32(Registry.GetValue(path, "Start", -1));

            networkStart.Text = "Loading . . .";

            if (startingNumber > 0)
            {
                networkStart.Text = Convert.ToString(startingNumber);
            }
            else
            {
                networkStart.Text = "0";
            }
        }
    }
}
