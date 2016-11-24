using Serwisy.ServiceReference1;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace Serwisy
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

        private async void TemperaturaZInternetu()
        {
            Windows.Web.Http.HttpClient hc = new Windows.Web.Http.HttpClient();
            var result = await hc.GetStringAsync(new Uri("http://ktostemperature.azurewebsites.net/external2.txt"));
            TempConvertSoapClient f = new TempConvertSoapClient();
            var resultf = await f.CelsiusToFahrenheitAsync(result);
            await f.CloseAsync();
            temp.Text = $"{result} C, {resultf} F";

        }
        private async void ConvertTempToF()
        {
            TempConvertSoapClient f = new TempConvertSoapClient();
            var resultf = await f.CelsiusToFahrenheitAsync(textbox.Text);
            await f.CloseAsync();
            tempf.Text = resultf;
        }
        private async void ConvertTempToC()
        {
            TempConvertSoapClient f = new TempConvertSoapClient();
            var resultc = await f.FahrenheitToCelsiusAsync(textbox.Text);
            await f.CloseAsync();
            tempc.Text = resultc;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TemperaturaZInternetu();
        }

        private void TempCButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertTempToF();
        }

        private void TempFButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertTempToC();
        }
    }
}
