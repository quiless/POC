using System;
using System.Text.RegularExpressions;

namespace POC.Artifacts.Helpers
{
	public static class DateHelper
	{
        public static int GetYearDiff(this DateTime date)
        {
            int years = DateTime.Now.Year - date.Year;

            if ((date.Month == DateTime.Now.Month && DateTime.Now.Day < date.Day)
                || DateTime.Now.Month < date.Month)
            {
                years--;
            }

            return years;
        }

        
        public static DateTime AbsoluteStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static DateTime AbsoluteEnd(this DateTime dateTime)
        {
            return AbsoluteStart(dateTime).AddDays(1).AddTicks(-1).AddMilliseconds(-999);
        }

    }
}

