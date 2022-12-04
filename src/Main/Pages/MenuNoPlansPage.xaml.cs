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

namespace Main.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy MenuNoPlansPage.xaml
    /// </summary>
    public partial class MenuNoPlansPage : Page
    {
        public MenuNoPlansPage()
        {
            InitializeComponent();
        }

        private void CreateNewPlanButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateNewPlanPage());
        }

        private void NewPlan_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateNewPlanPage());
        }
    }
}
