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
using System.Timers;
using System.Threading;
using Main.Pages;
using DBManager;
using BackupManager;

namespace Main
{
    public class CreateNewPlan
    {
        public string planName, planDescription;
        public List<string> sourceFolders, destinationFolders;
        public List<DayOfWeek> allowedDays;
        public TimeSpan interval;

        public CreateNewPlan()
        {
            planName = CreateNewPlanPage.planName;
            planDescription = CreateNewPlanPage.planDescription;
            sourceFolders = AddSourceFoldersPage.sourceFolders;
            destinationFolders = AddDestinationFoldersPage.destinationFolders;
            allowedDays = ScheduleBackupsPage.allowedDays;
            interval = ScheduleBackupsPage.interval;
        }

        public void ManageNewPlan()
        {
            DB.SavePlan(planName, planDescription, interval, allowedDays, sourceFolders, destinationFolders);

            if (interval == TimeSpan.Zero)
            {
                new Thread(() => Backup.ExecuteBackup(planName)).Start();
            }
        }
    }
}
