using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager;

namespace BackupManager
{
    public class Backup
    {
        private static DateTime startDate;
        private static string backupDirectoryName = @"\Backups\";

        public static void ExecuteBackup(string planName)
        {
            startDate = DateTime.UtcNow;

            (List<string> sourceFolders, List<string> destinationFolders) = DB.GetFolders(planName);

            BackupUtility bu = new BackupUtility(planName, startDate);

            bool isLastElement = false;

            foreach(string destination in destinationFolders)
            {
                isLastElement = destination.Equals(destinationFolders.Last());

                foreach (string source in sourceFolders)
                {
                    bu.MakeBackup(source, destination + backupDirectoryName, isLastElement);
                }
            }

            DB.SaveBackup(planName, startDate, DateTime.UtcNow);
        }
    }
}
