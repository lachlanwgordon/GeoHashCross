using System;
using System.Windows.Input;
using GeohashCross.Models;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using Plugin.Jobs;
using Plugin.LocalNotifications;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeohashCross.ViewModels
{
    public class NotificationsViewModel : BaseViewModel
    {
        public bool ShowEmptyView
        {
            get
            {
                return Subscriptions.Count == 0;
            }
        }


        public NotificationsViewModel()
        {
            if(!DesignMode.IsDesignModeEnabled)
                Init();
        }

        private async void Init()
        {
            await DB.Initialize();
            var subs = DB.Connection.Table<NotificationSubscription>();
            if (await subs.CountAsync() > 0)
            {
                Subscriptions.AddRange(await subs.ToListAsync());
            }
            OnPropertyChanged(nameof(ShowEmptyView));
        }


        public ObservableRangeCollection<NotificationSubscription> Subscriptions
        {
            get;
            set;
        } = new ObservableRangeCollection<NotificationSubscription>();

  
        public INotificationPermission NotificationPermission => DependencyService.Get<INotificationPermission>();

        public ICommand AddSubscriptionCommand => new Command(AddSubscription);

        private async void AddSubscription()
        {
            try
            {
                var notificationsAllowed = await GetNotificationsPermissions();
                if(!notificationsAllowed)
                {
                    await Shell.Current.DisplayAlert("Notification Permission Needed", "You can only subscribe to notifications if you have the permission enabled", "Okay");
                    return;
                }
                 await DB.Subscribe();

                var loc = await Xamarin.Essentials.Geolocation.GetLocationAsync();
                var subscription = new NotificationSubscription
                {
                    Latitude = loc.Latitude,
                    Longitude = loc.Longitude,
                    Radius = 20,
                    AlarmTime = new TimeSpan(8,0,0),//new DateTime(1, 1, 1, 8, 0, 0)

                };
                Subscriptions.Add(subscription);
                OnPropertyChanged(nameof(ShowEmptyView));

                var id = await DB.Connection.InsertAsync(subscription);
                subscription.Id = id;
                var itemsBeingEdited = Subscriptions.FirstOrDefault(x => x.IsEditing);//Should always be one

                itemsBeingEdited?.SaveCommand.Execute(null);
                subscription.IsEditing = true;

            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
        }

        private async Task<bool> GetNotificationsPermissions()
        {
            var allowed = await NotificationPermission.GetPermission();
            return allowed;
        }

        public async Task Delete(NotificationSubscription sub)
        {
            Subscriptions.Remove(sub);
            await DB.Connection.DeleteAsync(sub);
            OnPropertyChanged(nameof(ShowEmptyView));
        }

    }
}

