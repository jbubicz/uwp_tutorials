using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace TrainingPlatform.Models
{
    public class StringToCurrencyConverter : IValueConverter
    {
        private double p;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string v = value.ToString();
            if (Double.TryParse(v, out p))
            {
                v = String.Format(System.Globalization.CultureInfo.CurrentCulture, "{0:C2}", p);
            }
            else
            {
                v = String.Empty;
            }
            return v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
