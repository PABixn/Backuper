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
using System.IO;
using System.Windows.Forms;
using Ookii.Dialogs.Wpf;

namespace Main.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy AddDestinationFoldersPage.xaml
    /// </summary>
    public partial class AddDestinationFoldersPage : Page
    {
        public static List<string> destinationFolders = new List<string>();

        public AddDestinationFoldersPage()
        {
            InitializeComponent();
        }

        private void DeleteSourceFolderButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = DestinationFoldersList.SelectedIndex;

            DestinationFoldersList.Items.RemoveAt(selectedIndex);
            destinationFolders.RemoveAt(selectedIndex);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ScheduleBackupsPage());
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();

            fbd.Multiselect = true;
            fbd.Description = "Select folders...";

            if (fbd.ShowDialog() == true)
            {
                foreach (string s in fbd.SelectedPaths)
                {
                    if(!destinationFolders.Contains(s))
                    {
                        DestinationFoldersList.Items.Add(s);
                        destinationFolders.Add(s);
                    }
                }
            }
        }
    }
}
