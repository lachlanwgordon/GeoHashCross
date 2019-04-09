using System;
using MvvmHelpers;
using Xamarin.Essentials;
using System.Windows.Input;
using SQLite;
using Xamarin.Forms;
using System.Diagnostics;
using GeohashCross.Services;

namespace GeohashCross.Models
{
    public class NotificationSubscription : ObservableObject
    {
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
                        var view = sender as CollectionView;
                        if (view != null)
                        {
                            var items = view.ItemsSource as ObservableRangeCollection<NotificationSubscription>;
                            items.Remove(this);
                            await DB.Connection.DeleteAsync(this);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Oject disposed exception here if you navigate to the ListView page then back to the CollectionViewPage and click add or delete. Android only, iOS is fine. CollectionViewOnly, listview is fine. \n{ex}\n{ex.StackTrace}");
                    }
                });
            }
        }
        public bool IsEditing { get; set; }
        public double Height
        {
            get => IsEditing ? 100 : 50;
        }

        public ICommand EditCommand
        {
            get
            {
                return new Xamarin.Forms.Command((object sender) =>
                {
                    try
                    {
                        var view = sender as CollectionView;
                        if (view != null)
                        {
                            var items = view.ItemsSource as ObservableRangeCollection<NotificationSubscription>;
                            var index = items.IndexOf(this);
                            items.Remove(this);

                            IsEditing = !IsEditing;
                            items.Insert(index, this);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Oject disposed exception here if you navigate to the ListView page then back to the CollectionViewPage and click add or delete. Android only, iOS is fine. CollectionViewOnly, listview is fine. \n{ex}\n{ex.StackTrace}");
                    }
                });
            }
        }
    }
}
