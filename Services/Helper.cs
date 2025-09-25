using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SCT.Services
{
    public class Helper
    {
        public static async Task DateNotify(int courseId, string? courseTitle, DateTime courseStartDate, DateTime courseEndDate)
        {
            LocalNotificationCenter.Current.Cancel(new[] { courseId + 1000, courseId + 2000 });
            if (courseStartDate.Date.AddHours(9) >= DateTime.Today)
            {
                var notifyStart = new NotificationRequest
                {
                    NotificationId = courseId + 1000,
                    Title = "Course Start Date",
                    Description = $"Your assessment for {courseTitle} starts today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        //NotifyTime = DateTime.Now.AddMinutes(2)
                        NotifyTime = courseStartDate.Date.AddHours(9)
                    }
                };
                await LocalNotificationCenter.Current.Show(notifyStart);
            }
            if (courseEndDate.Date.AddHours(9) >= DateTime.Today)
            {
                var notifyEnd = new NotificationRequest
                {
                    NotificationId = courseId + 2000,
                    Title = "Course End Date",
                    Description = $"Your assessment for {courseTitle} ends today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        //NotifyTime = DateTime.Now.AddMinutes(2)
                        NotifyTime = courseEndDate.Date.AddHours(9)
                    }
                };
                await LocalNotificationCenter.Current.Show(notifyEnd);
            }
        }
        public static async Task DateAssessmentNotify(int id, string? title, DateTime startDate, DateTime endDate)
        {
            LocalNotificationCenter.Current.Cancel(new[] { id + 10000, id + 20000 });
            if (startDate.Date.AddHours(9) >= DateTime.Today)
            {
                var notifyStart = new NotificationRequest
                {
                    NotificationId = id + 10000,
                    Title = "Assessment Start Date",
                    Description = $"Your assessment for {title} starts today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        //NotifyTime = DateTime.Now.AddMinutes(2)
                        NotifyTime = startDate.Date.AddHours(9)
                    }
                };
                await LocalNotificationCenter.Current.Show(notifyStart);
            }
            if (endDate.Date.AddHours(9) >= DateTime.Today)
            {
                var notifyEnd = new NotificationRequest
                {
                    NotificationId = id + 20000,
                    Title = "Assessment End Date",
                    Description = $"Your assessment for {title} ends today!",
                    Schedule = new NotificationRequestSchedule
                    {
                        //NotifyTime = DateTime.Now.AddMinutes(2)
                        NotifyTime = endDate.Date.AddHours(9)
                    }
                };
                await LocalNotificationCenter.Current.Show(notifyEnd);
            }
        }

        public static string Sanitize(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Trim()
                .Replace("<", "")
                .Replace(">", "")
                .Replace(";", "")
                .Replace("--", "");
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }

        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            string pattern = @"^(\+?\d{1,2}\s?)?(\(?\d{3}\)?[\s-]?)?\d{3}[\s-]?\d{4}$";
            return Regex.IsMatch(phone, pattern);
        }
    }
}
