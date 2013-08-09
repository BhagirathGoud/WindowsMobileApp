using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using GPSalarm.Resources;
using System.IO.IsolatedStorage;

namespace GPSalarm
{
    public partial class MainPage : PhoneApplicationPage
    {

        private IsolatedStorageSettings  appdata = IsolatedStorageSettings.ApplicationSettings;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void set_Click(object sender, RoutedEventArgs e)
        {
            if (destText.Text == "" || distText.Text == "" || title.Text == "")
            {
                MessageBox.Show("Please Fill All The Fields");
            }
            else
            {

                if (appdata.Contains("data1") || appdata.Contains("data2") || appdata.Contains("data3"))
                {
                    appdata.Remove("data1");
                    appdata.Remove("data2");
                    appdata.Remove("data3");
                    
                    appdata.Add("data1", destText.Text);
                    appdata.Add("data2", distText.Text);
                    appdata.Add("data3", title.Text);
                }
                else
                {
                   
                    appdata.Add("data1", destText.Text);
                    appdata.Add("data2", distText.Text);
                    appdata.Add("data3", title.Text);
                }
                NavigationService.Navigate(new Uri("/page1.xaml", UriKind.Relative));
            }
            
            
        }

        

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
