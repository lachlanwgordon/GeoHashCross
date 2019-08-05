using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GeohashCross.Models;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Plugin.Jobs;
using SQLite;

namespace GeohashCross.Services
{
    public class DB
    {
        public DB()
        {
        }
        static string Folder => Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "gc");
        static string DatabaseName => System.IO.Path.Combine(Folder, "gc.db");

        public static SQLiteAsyncConnection Connection { get; private set; }// = new SQLiteAsyncConnection(DatabaseName);
        public static SQLiteConnection ConnectionSync { get; set; }// = new SQLiteConnection(DatabaseName);
        internal static async Task Initialize()
        {
            var folder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "gc");

            Directory.CreateDirectory(folder);

            string databaseFileName = System.IO.Path.Combine(folder, "gc.db");

            Connection = new SQLiteAsyncConnection(databaseFileName);
            ConnectionSync = new SQLiteConnection(databaseFileName);

            await Connection.CreateTableAsync<NotificationSubscription>();
            Subscribe();

        }

        public static async Task Subscribe()
        {

            if (await Connection.Table<NotificationSubscription>().CountAsync() == 0)
            {
                return;
            }

            var job = new JobInfo
            {
                Name = "Distance",
                Type = typeof(LocationNotificationJob),
                BatteryNotLow = false,
                DeviceCharging = false,
                RequiredNetwork = NetworkType.Any,
            };
            await CrossJobs.Current.Schedule(job);
            var subs = await Connection.Table<NotificationSubscription>().ToListAsync();
        }
    }
}
