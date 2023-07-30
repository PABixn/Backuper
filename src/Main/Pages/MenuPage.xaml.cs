using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DBManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Main.Pages
{
    public partial class MenuPage : System.Windows.Controls.Page
    {
        System.Timers.Timer timer;

        public MenuPage()
        {
            InitializeComponent();
        }

        private void MenuPage1_Loaded(object sender, RoutedEventArgs e)
        {
            (List<string> plans, List<DateTime> lastDates, List<TimeSpan> intervals) = DB.GetAll();

            for (int i = 0; i < plans.Count; i++)
                PlansList.Items.Add(new Plan { PlanName = plans[i], NextBackup = "Next backup: " + GetNextBackupDate(lastDates[i], intervals[i]).ToLocalTime().ToString(CultureInfo.InstalledUICulture) });

            timer = new System.Timers.Timer(1000);

            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();

            LoadPlanData(((Plan)PlansList.Items[0]).PlanName);
        }

        private void LoadPlanData(string planName)
        {
            (string planDescription, TimeSpan interval, List<DayOfWeek> allowedDays, List<string> sourceFolders, List<string> destinationFolders) = DB.GetPlan(planName);

            SourceFoldersTextBlock.Text = "";
            DestinationFoldersTextBlock.Text = "";
            LastBackupTextBlock.Text = "";
            NextBackupTextBlock.Text = "";
            PlanNameTextBlock.Text = "";

            PlanNameTextBlock.Text = planName;

            foreach (string folder in sourceFolders)
                SourceFoldersTextBlock.Text += folder + "   ";

            foreach (string folder in destinationFolders)
                DestinationFoldersTextBlock.Text += folder + "   ";

            LastBackupTextBlock.Text = "Last backup: " + DB.GetLastDate(planName).ToLocalTime().ToString(CultureInfo.InstalledUICulture);

            NextBackupTextBlock.Text = "Next backup: " + GetNextBackupDate(DB.GetLastDate(planName), interval).ToLocalTime().ToString(CultureInfo.InstalledUICulture);

            HideAll.Visibility = Visibility.Hidden;

            //PlansList.SelectedItem = PlansList.Items.Cast<Plan>().Where(x => x.PlanName == planName).FirstOrDefault();
        }

        private void UpdateUI(DateTime lastDate, TimeSpan interval, bool executing)
        {
            this.Dispatcher.Invoke(() =>
            {
                LastBackupTextBlock.Text = "";
                NextBackupTextBlock.Text = "";

                LastBackupTextBlock.Text = "Last backup: " + lastDate.ToLocalTime().ToString(CultureInfo.InstalledUICulture);

                if (executing == false)
                    NextBackupTextBlock.Text = "Next backup: " + GetNextBackupDate(lastDate, interval).ToLocalTime().ToString(CultureInfo.InstalledUICulture);
                else
                    NextBackupTextBlock.Text = "Executing now";

                (List<string> plans, List<DateTime> lastDates, List<TimeSpan> intervals) = DB.GetAll();

            for (int i = 0; i < plans.Count; i++)
            {
                if (PlansList.Items.Count-1 < i)
                    PlansList.Items.Add(new Plan { PlanName = plans[i], NextBackup = "Next backup: " + GetNextBackupDate(lastDates[i], intervals[i]).ToLocalTime().ToString(CultureInfo.InstalledUICulture) });

                if (PlanHandler.IsExecuting.ContainsKey(plans[i]) && PlanHandler.IsExecuting[plans[i]] == true)
                    PlansList.Items[i] = new Plan { PlanName = plans[i], NextBackup = "Executing" };
                else
                    PlansList.Items[i] = new Plan { PlanName = plans[i], NextBackup = "Next backup: " + GetNextBackupDate(lastDates[i], intervals[i]).ToLocalTime().ToString(CultureInfo.InstalledUICulture) };
                }
            });
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                if(PlansList.Items.Count > 0)
                {
                    string planName = ((Plan)PlansList.SelectedItem)?.PlanName ?? ((Plan)PlansList.Items[0]).PlanName;

                    if (PlanHandler.IsExecuting.ContainsKey(planName))
                        UpdateUI(DB.GetLastDate(planName), DB.GetInterval(planName), PlanHandler.IsExecuting[planName]);
                }
            });
        }

        private DateTime GetNextBackupDate(DateTime lastDate, TimeSpan interval)
        {
            return lastDate.Add(interval);
        }

        private void ListViewPlan_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            LoadPlanData(((Plan)PlansList.SelectedItem ?? (Plan)PlansList.Items[0]).PlanName);
        }

        private void NewPlan_Click(object sender, RoutedEventArgs e)
        {
            ClearPages();
            this.NavigationService.Navigate(new CreateNewPlanPage());
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string plan = ((Plan)PlansList.SelectedItem ?? (Plan)PlansList.Items[0]).PlanName;

            if (PlanHandler.IsExecuting.ContainsKey(plan))
            {
                if (PlanHandler.IsExecuting[plan] == false)
                {
                    PlanExecutingTextBlock.Visibility = Visibility.Hidden;
                    new Thread(() => PlanHandler.RunBackup(plan)).Start();
                }
                else
                {
                    PlanExecutingTextBlock.Visibility = Visibility.Visible;
                }
            }
        }

        private void DeletePlan_Click(object sender, RoutedEventArgs e)
        {
            if(PlansList.SelectedItem != null)
            {
                DB.DeletePlan(((Plan)PlansList.SelectedItem).PlanName);
                PlanHandler.IsExecuting.Remove(((Plan)PlansList.SelectedItem).PlanName);
                PlansList.Items.Remove(PlansList.SelectedItem);
                if(PlansList.Items.Count <= 0)
                {
                    PlanHandler.StopHandling();
                    timer.Stop();
                    this.NavigationService.Navigate(new CreateNewPlanPage());
                }
                else
                {
                    LoadPlanData(((Plan)PlansList.Items[0]).PlanName); 
                }
            }
        }

        private void ClearPages()
        {
            CreateNewPlanPage.planName = "";
            CreateNewPlanPage.planDescription = "";
            AddSourceFoldersPage.sourceFolders.Clear();
            AddDestinationFoldersPage.destinationFolders.Clear();
            ScheduleBackupsPage.allowedDays.Clear();
            ScheduleBackupsPage.interval = new TimeSpan(1, 0, 0, 0);
        }
    }

    class Plan
    {
        public string PlanName { get; set; }
        public string NextBackup { get; set; }

    }
}
