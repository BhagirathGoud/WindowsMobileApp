using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Device.Location;
using Microsoft.Phone.Scheduler;


namespace GPSalarm
{
    public partial class Page3 : PhoneApplicationPage
    {
        GeoCoordinateWatcher geoWatcher;
        private IsolatedStorageSettings appdata = IsolatedStorageSettings.ApplicationSettings;
        string status;
        public Page3()
        {
            InitializeComponent();
            get();
            
        }

        private void get()
        {
            geoWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
            geoWatcher.MovementThreshold = 20;
            geoWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(geoWatcher_PositionChanged);
            geoWatcher.Start();
        }
        void geoWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate currentLoc = e.Position.Location;
            double minDis = Double.Parse(appdata["data2"].ToString());
            double minDistance = minDis*1000 ;
            double currentLongitude = e.Position.Location.Longitude;
            double currnetLatitude = e.Position.Location.Latitude;
            double latitude = Double.Parse(appdata["latitude"].ToString());
            double longitude = Double.Parse(appdata["longitude"].ToString());
            GeoCoordinate location = new GeoCoordinate(latitude, longitude);
            double distanceInMeter = currentLoc.GetDistanceTo(location);
            double disInKm = distanceInMeter / 1000;
            if (distanceInMeter < minDistance)
            { 
              if (!App.RunningInBackground)
               {
                   Dispatcher.BeginInvoke(() =>
                 {
                     textBlock2.Text = disInKm.ToString();
               
                      status = "You have reached your destination !!!!!!";
                      CreateAlarm(15);
                 });
                }
              else
               {
                   CreateAlarm(15);
                Microsoft.Phone.Shell.ShellToast toast = new Microsoft.Phone.Shell.ShellToast();
                toast.Content = status;
                toast.Title = "Status: ";
                toast.NavigationUri = new Uri("/Page4.xaml", UriKind.Relative);
                toast.Show();

                }
            }
            else
             {
               textBlock2.Text = disInKm.ToString();
               status = "Not at Reached the Destination...";
             }
                textBlock3.Text = status;
            }

      private void CreateAlarm(int p)
         {
             string alarmName = Guid.NewGuid().ToString();
             var alarm = new Alarm(alarmName)
                 {
                     Content =  appdata["data3"].ToString(),
                     BeginTime = DateTime.Now.AddSeconds(p)
                 };
                 ScheduledActionService.Add(alarm);
            
         }

     
           
            
      }

       
   }
