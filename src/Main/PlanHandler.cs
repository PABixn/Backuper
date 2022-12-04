using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using DBManager;
using BackupManager;
using System.ComponentModel;
using System.Threading;
using Main.Pages;
using System.Windows;

namespace Main
{
    public class PlanHandler
    {
        public static Dictionary<string, bool> IsExecuting = new Dictionary<string, bool>();
        static System.Timers.Timer timer = new System.Timers.Timer(1000);

        public static void HandleAllPlans()
        {
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Start();
        }

        public static void StopHandling()
        {
            timer.Stop();
        }

        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<string> planNames = DB.GetAllPlans();

            foreach(string plan in planNames)
            {
                if(ShouldBackup(DB.GetLastDate(plan), DB.GetInterval(plan)))
                {
                    if (!IsExecuting.ContainsKey(plan))
                        IsExecuting.Add(plan, false);

                    if (IsExecuting[plan] == false)
                    {
                        new Thread(() => RunBackup(plan)).Start();
                    }
                }
            }
        }

        public static void RunBackup(string plan)
        {
            IsExecuting[plan] = true;

            Backup.ExecuteBackup(plan);

            IsExecuting[plan] = false;
        }

        private static bool ShouldBackup(DateTime lastDate, TimeSpan interval)
        {
            DateTime nextBackup = lastDate.Add(interval);

            if (DateTime.UtcNow >= nextBackup)
                return true;
            else
                return false;
        }
    }
}
