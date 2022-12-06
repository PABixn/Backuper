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
    /// Logika interakcji dla klasy AddSourceFoldersPage.xaml
    /// </summary>
    public partial class AddSourceFoldersPage : Page
    {
        public static List<string> sourceFolders = new List<string>();

        public AddSourceFoldersPage()
        {
            InitializeComponent();

            if(sourceFolders.Count > 0)
            {
                foreach(string s in sourceFolders)
                {
                    SourceFoldersList.Items.Add(s);
                }
            }
        }

        private void DeleteSourceFolderButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = SourceFoldersList.SelectedIndex;

            SourceFoldersList.Items.RemoveAt(selectedIndex);
            sourceFolders.RemoveAt(selectedIndex);
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddDestinationFoldersPage());
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog fbd = new VistaFolderBrowserDialog();

            fbd.Multiselect = true;
            fbd.Description = "Select folders...";

            if(fbd.ShowDialog() == true)
            {
                foreach(string s in fbd.SelectedPaths)
                {
                    if(!sourceFolders.Contains(s))
                    {
                        SourceFoldersList.Items.Add(s);
                        sourceFolders.Add(s);
                    }
                }
            }
        }

        private void GoBackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new CreateNewPlanPage());
        }
    }
}
