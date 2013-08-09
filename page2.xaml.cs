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
using System.IO;
using System.Xml.Linq;
using System.Device.Location;

namespace GPSalarm
{
    public partial class Page2 : PhoneApplicationPage
    {
        
        private IsolatedStorageSettings appdata = IsolatedStorageSettings.ApplicationSettings;
       
        
        public Page2()
        {
            InitializeComponent();
            
            if (appdata.Contains("exact"))
            {

                string destinationPoint = appdata["exact"].ToString();
                WebClient webClient = new WebClient();
                webClient.OpenReadAsync(new Uri("http://maps.googleapis.com/maps/api/geocode/xml?sensor=true&address=" + destinationPoint));
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadComplete);

               


            }
           
             
            
        }

       
     

        void webClient_OpenReadComplete(object sender, OpenReadCompletedEventArgs e)
        {
            try
            {

                var reader = new StreamReader(e.Result);

                var xmlElm = XElement.Parse(reader.ReadToEnd());

                var status = (from elm in xmlElm.Descendants()
                              where elm.Name == "status"
                              select elm).FirstOrDefault();

                if (status.Value.ToLower() == "ok")
                {
                    var latitude = (from elm in xmlElm.Descendants().Descendants().Descendants()
                                    where elm.Name == "lat"
                                    select elm).FirstOrDefault();
                    var longitude = (from elm in xmlElm.Descendants().Descendants().Descendants()
                                     where elm.Name == "lng"
                                     select elm).FirstOrDefault();

                    if (appdata.Contains("latitide") && appdata.Contains("longitude"))
                    {
                        appdata.Remove("latitude");
                        appdata.Remove("longitude");

                        appdata.Add("latitude", latitude.Value);
                        appdata.Add("longitude", longitude.Value);
                    }
                    else
                    {
                        appdata.Remove("latitude");
                        appdata.Remove("longitude");
                        appdata.Add("latitude", latitude.Value);
                        appdata.Add("longitude", longitude.Value);
                    }
                    NavigationService.Navigate(new Uri("/page3.xaml", UriKind.Relative));



                }
                else
                {

                }
            }
            catch
            {
                MessageBox.Show("Please Try Again..Something Went wrong");
            }
           
        }
     
    }
}
