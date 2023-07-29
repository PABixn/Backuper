using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using Dapper;
using System.Reflection;
using System.Windows.Markup;
using System.Globalization;

#pragma warning disable

namespace DBManager
{
    public static class DB
    {

        public static void SavePlan(string planName, string planDescription, TimeSpan interval, List<DayOfWeek> allowedDays, List<string> sourceFolders, List<string> destinationFolders)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            string sourceFoldersString = DBHelper.Combine(sourceFolders);
            string destinationFoldersString = DBHelper.Combine(destinationFolders);
            string allowedDaysString = DBHelper.CombineDays(allowedDays);

            conn.Execute($"REPLACE INTO Plans Values ('{planName}', '{planDescription}', '{sourceFoldersString}', '{destinationFoldersString}', '{allowedDaysString}', '{interval}')");
        }

        public static void DeletePlan(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            conn.Execute($"DELETE FROM Plans WHERE planName='{planName}'");
        }

        public static void SaveBackup(string planName, DateTime startDate, DateTime endDate)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            conn.Execute($"INSERT INTO ExecutedBackups VALUES (NULL, '{planName}', '{startDate}', '{endDate}')");
        }

        public static (string planDescription, TimeSpan interval, List<DayOfWeek> allowedDays, List<string> sourceFolders, List<string> destinationFolders) GetPlan(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            string planDescription = conn.Query<string>($"SELECT planDescription FROM Plans WHERE planName='{planName}'").FirstOrDefault();
            string sourceFolders = conn.Query<string>($"SELECT sourceFolders FROM Plans WHERE planName='{planName}'").FirstOrDefault();
            string destinationFolders = conn.Query<string>($"SELECT destinationFolders FROM Plans WHERE planName='{planName}'").FirstOrDefault();
            string allowedDays = conn.Query<string>($"SELECT allowedDays FROM Plans WHERE planName='{planName}'").FirstOrDefault();
            string interval = conn.Query<string>($"SELECT interval FROM Plans WHERE planName='{planName}'").FirstOrDefault();

            return (planDescription, TimeSpan.Parse(interval), DBHelper.DivideDays(allowedDays), DBHelper.Divide(sourceFolders), DBHelper.Divide(destinationFolders));
        }

        public static (List<string> planName, List<DateTime> startDate, List<DateTime> endDate) GetBackups()
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            List<string> planNames = conn.Query<string>($"SELECT planName FROM ExecutedBackups").ToList();
            List<string> startDates = conn.Query<string>($"SELECT startDate FROM ExecutedBackups").ToList();
            List<string> endDates = conn.Query<string>($"SELECT endDate FROM ExecutedBackups").ToList();

            return (planNames, DBHelper.ToDateTimes(startDates), DBHelper.ToDateTimes(endDates));
        }

        public static void SaveFile(string filePath, string planName, DateTime modifyDate)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            conn.Execute($"REPLACE INTO Files VALUES ('{filePath}', '{planName}', '{modifyDate}')");
        }

        public static (List<string>, List<string>) GetFolders(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            string sourceFolders = conn.Query<string>($"SELECT sourceFolders FROM Plans WHERE planName='{planName}'").FirstOrDefault();
            string destinationFolders = conn.Query<string>($"SELECT destinationFolders FROM Plans WHERE planName='{planName}'").FirstOrDefault();


            return (DBHelper.Divide(sourceFolders), DBHelper.Divide(destinationFolders));
        }

        public static (List<string>, List<DateTime>, List<TimeSpan>) GetAll()
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            List<string> plans = new List<string>();
            List<DateTime> lastDates = new List<DateTime>();
            List<TimeSpan> intervals = new List<TimeSpan>();

            plans = conn.Query<string>($"SELECT planName FROM Plans").ToList();

            for (int i = 0; i < plans.Count; i++)
                lastDates.Add(GetLastDate(plans[i]));

            for (int i = 0; i < plans.Count; i++)
                intervals.Add(GetInterval(plans[i]));

            return (plans, lastDates, intervals);
        }

        public static List<string> GetAllPlans()
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            return conn.Query<string>($"SELECT planName FROM Plans").ToList();
        }

        public static DateTime GetModifyDate(string planName, string filePath)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            SQLiteConnection conn = new SQLiteConnection(Connect());

            string date = conn.Query<string>($"SELECT modifyDate FROM Files WHERE filePath='{filePath}' AND planName='{planName}'").FirstOrDefault();

            return DateTime.Parse(date ?? "1/1/1111 1:11:11");
        }

        public static TimeSpan GetInterval(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            return TimeSpan.Parse(conn.Query<string>($"SELECT interval FROM Plans WHERE planName='{planName}'").FirstOrDefault());
        }

        public static List<string> GetSourceFolders(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            return conn.Query<string>($"SELECT sourceFolders FROM Plans WHERE planName='{planName}'").ToList();
        }

        public static DateTime GetLastDate(string planName)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            SQLiteConnection conn = new SQLiteConnection(Connect());

            List<string> lastDates = conn.Query<string>($"SELECT endDate FROM ExecutedBackups WHERE planName='{planName}'").ToList();

            return DBHelper.ToDateTimes(lastDates).OrderByDescending(x => x).FirstOrDefault();
        }

        public static bool PlanExists(string planName)
        {
            SQLiteConnection conn = new SQLiteConnection(Connect());

            return conn.Query<bool>($"SELECT EXISTS(SELECT 1 FROM Plans WHERE planName='{planName}')").FirstOrDefault();
        }

        private static string Connect()
        {
            //return $"Data Source={Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Backuper\backuper.db"};Version=3;";
            return "Data Source=backuper.db;Version=3;";
        }
    }
}
