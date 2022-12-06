using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class ScheduleBackupsPage : Page
    {
        public static List<DayOfWeek> allowedDays = new List<DayOfWeek>();
        public static TimeSpan interval;

        public ScheduleBackupsPage()
        {
            InitializeComponent();
        }

        private void RunAutoCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (RunAutoCheckBox.IsChecked == true)
                HideOptionsPanel.Visibility = Visibility.Hidden;
            else if(RunAutoCheckBox.IsChecked == false)
                HideOptionsPanel.Visibility = Visibility.Visible;
        }

        private void GetAllowedDays()
        {
            if(RunAutoCheckBox.IsChecked == true)
            {
                if (AllowMonday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Monday);
                if (AllowTuesday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Tuesday);
                if (AllowWednesday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Wednesday);
                if (AllowThursday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Thursday);
                if (AllowFriday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Friday);
                if (AllowSaturday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Saturday);
                if (AllowSunday.IsChecked == true)
                    allowedDays.Add(DayOfWeek.Sunday);
            }
        }

        private void GetInterval()
        {
            if(RunAutoCheckBox.IsChecked == false)
                interval = TimeSpan.Zero;
            else if (IntervalUnit.SelectedValue.ToString() == "Days")
                interval = TimeSpan.FromDays(double.Parse(IntervalValue.Text));
            else if (IntervalUnit.SelectedValue.ToString() == "Minutes")
                interval = TimeSpan.FromMinutes(double.Parse(IntervalValue.Text));
            else if (IntervalUnit.SelectedValue.ToString() == "Hours")
                interval = TimeSpan.FromHours(double.Parse(IntervalValue.Text));
            else if (IntervalUnit.SelectedValue.ToString() == "Weeks")
                interval = TimeSpan.FromDays(7 * double.Parse(IntervalValue.Text));
            else if (IntervalUnit.SelectedValue.ToString() == "Months")
                interval = TimeSpan.FromDays(30 * double.Parse(IntervalValue.Text));
            else if (IntervalUnit.SelectedValue.ToString() == "Minutes")
                interval = TimeSpan.FromDays(365 * double.Parse(IntervalValue.Text));
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            GetAllowedDays();
            GetInterval();

            this.NavigationService.Navigate(new MenuPage());

            CreateNewPlan cnp = new CreateNewPlan();
            cnp.ManageNewPlan();
        }

        private void IntervalValue_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void GoBackButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new AddDestinationFoldersPage());
        }
    }
}
