using System;
using MvvmHelpers;
using Xamarin.Essentials;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using System.Diagnostics;
using GeohashCross.Services;
using Microsoft.AppCenter.Crashes;
using GeohashCross.ViewModels;
using System.Linq;
using Xamarin.Forms.Internals;

namespace GeohashCross.Models
{
    public class NotificationSubscription : ObservableObject
    {
        public ICommand SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        IsEditing = !IsEditing;
                        OnPropertyChanged(nameof(IsEditing));
                        await DB.Connection.UpdateAsync(this);
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }

        private int radius;
        private TimeSpan alarmTime;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Radius
        {
            get => radius;
            set
            {
                radius = value;
                OnPropertyChanged(nameof(Radius));
            }

        }
        public TimeSpan AlarmTime
        {
            get => alarmTime;
            set
            {
                alarmTime = value;
                OnPropertyChanged(nameof(AlarmTime));

            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                return new Xamarin.Forms.Command(async (object sender) =>
                {
                    try
                    {
                        var view = sender as StackLayout;
                        if (view != null)
                        {
                            var vm = view.BindingContext as NotificationsViewModel;
                            await vm.Delete(this);
                            


                        }
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }

        bool isEditing;
        public bool IsEditing
        {
            get
            {
                return isEditing;
            }
            set
            {
                isEditing = value;
                OnPropertyChanged(nameof(IsEditing));
            }
        }


        public ICommand EditCommand
        {
            get
            {
                return new Xamarin.Forms.Command((object sender) =>
                {
                    try
                    {


                        if (sender is StackLayout view)
                        {
                            var vm = view.BindingContext as NotificationsViewModel;
                            var items = vm.Subscriptions;// as ObservableRangeCollection<NotificationSubscription>;

                            var itemsBeingEdited = items.FirstOrDefault(x => x.IsEditing);//Should always be one

                            itemsBeingEdited?.SaveCommand.Execute(null);

                        }
                        IsEditing = !IsEditing;
                    }
                    catch (Exception ex)
                    {
                        Crashes.TrackError(ex);
                    }
                });
            }
        }
    }
}
