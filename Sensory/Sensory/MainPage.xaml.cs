using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Sensory
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void GetLocation_Click(object sender, RoutedEventArgs e)
        {
            getLocation();
        }

        private async void getLocation()
        {
            Geolocator gl = new Geolocator();
            var r = await gl.GetGeopositionAsync();

            lat.Text = r.Coordinate.Point.Position.Latitude.ToString();
            lon.Text = r.Coordinate.Point.Position.Longitude.ToString();

            myMap.Center = r.Coordinate.Point;
            myMap.ZoomLevel = 12;

            //gl.PositionChanged += (s, e) =>
            //{
            //    e.Position.Coordinate.Point.Position.Latitude; // current latitude
            //    e.Position.VenueData; // represents a venue, such as a shopping mall or office building
            //};
        }

        private void Compas_Click(object sender, RoutedEventArgs e)
        {
            Compass c = Compass.GetDefault();
            if (c != null)
            {
                c.ReadingChanged += C_ReadingChanged;
            }

            //Com.Text = c.GetCurrentReading().HeadingTrueNorth.ToString() ?? "error";
        }

        private void C_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Com.Text = args.Reading.HeadingTrueNorth.ToString();
            });
        }

        private void GetAcc_Click(object sender, RoutedEventArgs e)
        {
            Accelerometer a = Accelerometer.GetDefault();
            if (a != null)
            {
                a.ReadingChanged += A_ReadingChanged; 
            }
        }

        private void A_ReadingChanged(Accelerometer sender, AccelerometerReadingChangedEventArgs args)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                AccelerometerReading reading = args.Reading;
                ScenarioOutput_X.Text = String.Format("{0,5:0.00}", reading.AccelerationX);
                ScenarioOutput_Y.Text = String.Format("{0,5:0.00}", reading.AccelerationY);
                ScenarioOutput_Z.Text = String.Format("{0,5:0.00}", reading.AccelerationZ);
            });
        }
    }
}
