using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class DBHelper
    {
        public static string Combine(List<string> input)
        {
            string combined = "";

            foreach(string s in input)
            {
                if (s == input.Last())
                    combined += s;
                else
                    combined += s + "  ";
            }

            return combined;
        }

        public static string CombineDays(List<DayOfWeek> input)
        {
            string combined = "";

            foreach (DayOfWeek s in input)
                combined += s + "  ";

            return combined;
        }

        public static List<string> Divide(string input)
        {
            List<string> divided = new List<string>();

            string[] dividedTab = input.Split("  ");

            divided = dividedTab.ToList();

            return divided;
        }

        public static List<DayOfWeek> DivideDays(string input)
        {
            List<DayOfWeek> divided = new List<DayOfWeek>();

            string[] dividedTab = input.Split("  ");

            divided = ToDays(dividedTab);

            return divided;
        }

        public static List<DateTime> ToDateTimes(List<string> dates)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

            List<DateTime> result = new List<DateTime>();

            foreach(string s in dates)
                result.Add(DateTime.Parse(s));

            return result;
        }

        private static List<DayOfWeek> ToDays(string[] input)
        {
            List<DayOfWeek> days = new List<DayOfWeek>();

            for(int i = 0; i < input.Length; i++)
            {
                switch(input[i])
                {
                    case "Monday": days.Add(DayOfWeek.Monday); break;
                    case "Tuesday": days.Add(DayOfWeek.Tuesday); break;
                    case "Wednesday": days.Add(DayOfWeek.Wednesday); break;
                    case "Thursday": days.Add(DayOfWeek.Thursday); break;
                    case "Friday": days.Add(DayOfWeek.Friday); break;
                    case "Saturday": days.Add(DayOfWeek.Saturday); break;
                    case "Sunday": days.Add(DayOfWeek.Sunday); break;
                }
            }

            return days;
        }
    }
}
