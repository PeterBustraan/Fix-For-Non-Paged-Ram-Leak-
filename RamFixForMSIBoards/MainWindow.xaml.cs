using System;
using System.Windows;
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
        private int resetNetStart;
        private int resetNonStart;
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
                int newSize;
                if (int.TryParse(nonPagedSize.Text, out newSize))
                {
                    Registry.SetValue(nonPath, "NonPagedPoolSize", newSize);
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

        private void setNetworkStartVar(object sender, RoutedEventArgs e)
        { 
            checkElevation();
            if (ElevationStatus)
            {
                int newStart;
                if (int.TryParse(networkStart.Text, out newStart))
                {
                    Registry.SetValue(netPath, "Start", newStart);
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

        private void resetValues(object sender, RoutedEventArgs e)
        {
            nonPagedSize.Text = Convert.ToString(resetNonStart);
            Registry.SetValue(nonPath, "NonPagedPoolSize", resetNonStart);

            networkStart.Text = Convert.ToString(resetNetStart);
            Registry.SetValue(netPath, "Start", resetNetStart);
        }

        private void getDefaults(){ getCurrentNonPagedSize(); getCurrentNetworkStartSetting();}

        private void getCurrentNonPagedSize()
        {
            string path = nonPath;
            int startingNumber = Convert.ToInt32(Registry.GetValue(path, "NonPagedPoolSize", -1));
            resetNonStart = startingNumber;

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
            string path = netPath;
            int startingNumber = Convert.ToInt32(Registry.GetValue(path, "Start", -1));
            resetNetStart = startingNumber;

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
