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
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;



namespace GPSalarm
{
    public partial class Page1 : PhoneApplicationPage
    {
        List<String> StringsList = new List<string>();
        private IsolatedStorageSettings appdata = IsolatedStorageSettings.ApplicationSettings;
        public Page1()
        {
            InitializeComponent();
            textBlock.Text = "Please Wait...";
            if (appdata.Contains("data1") && appdata.Contains("data2") )
            {
                
                string destinationPoint = appdata["data1"].ToString();
               
                
                getPage(destinationPoint);
               
               // textBlock1.Text = destPoint2;
               // textBlock2.Text = distance.ToString();
               }
            
        }
        private void getPage(string des)
           
        { string get = des;
        WebClient webClient = new WebClient();
        webClient.OpenReadAsync(new Uri("http://maps.googleapis.com/maps/api/geocode/xml?sensor=true&address=" + get));
        webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadComplete);
        }
         


void webClient_OpenReadComplete(object sender ,OpenReadCompletedEventArgs e)
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
        var res = (from elm in xmlElm.Descendants()
                   where elm.Name == "formatted_address"
                   select elm).OrderByDescending(elm => elm.Name);

       
        foreach (var item in res)
        {
            textBlock.Text = "";
            string it = item.Value.ToString();
           
          StringsList.Add(it.ToString());
         
            longList.ItemsSource = StringsList;
            
        }
        
    }
    else
    {
        textBlock.Text = "";
        StringsList.Add("No Place found with Specified name..Please try with Specified Format");
        longList.ItemsSource = StringsList;
    }
        
    }
    catch
    {
        MessageBox.Show("Please Check Internet Connection");
    }
}

static IEnumerable<string> Split(string str, int chunksize)
{
    return Enumerable.Range(0, str.Length / chunksize)
        .Select(i => str.Substring(i * chunksize, chunksize));
}
     

        private void longList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (longList.SelectedItem != null)
            {
                var item = longList.SelectedItem;

                if (appdata.Contains("exact"))
                {
                    appdata.Remove("exact");
                    


                    appdata.Add("exact",item);
                    
                }
                else
                {

                    appdata.Add("exact",item);
                }
                NavigationService.Navigate(new Uri("/page2.xaml", UriKind.Relative));
            }
        }

    
    

        
    }
}
