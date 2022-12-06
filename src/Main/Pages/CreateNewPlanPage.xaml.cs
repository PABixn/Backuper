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
using DBManager;

namespace Main.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy CreateNewPlanPage.xaml
    /// </summary>
    public partial class CreateNewPlanPage : Page
    {
        public static string planName, planDescription;

        public CreateNewPlanPage()
        {
            InitializeComponent();

            if(planName != "")
                PlanNameTextBox.Text = planName;

            if(planDescription != "")
                DescriptionTextBox.Text = planDescription;
        }

        private void PlanNameTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (PlanNameTextBox.Text == "Name your plan")
                PlanNameTextBox.Text = "";
        }

        private void PlanNameTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (PlanNameTextBox.Text == String.Empty)
                PlanNameTextBox.Text = "Name your plan";
        }

        private void DescriptionTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (DescriptionTextBox.Text == "Description")
                DescriptionTextBox.Text = "";
        }

        private void DescriptionTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (DescriptionTextBox.Text == String.Empty)
                DescriptionTextBox.Text = "Description";
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            planName = PlanNameTextBox.Text;
            planDescription = DescriptionTextBox.Text;

            if (DB.PlanExists(planName))
                PlanExistsTextBlock.Visibility = Visibility.Visible;
            else
                this.NavigationService.Navigate(new AddSourceFoldersPage());
        }
    }
}
