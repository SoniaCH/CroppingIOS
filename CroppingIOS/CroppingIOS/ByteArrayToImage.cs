using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Xamarin.Forms;

namespace CroppingIOS
{
    public class ByteArrayToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource retSource = null;
            try
            {
                if (value != null)
                {
                    byte[] imageAsBytes = (byte[])value;
                    //var stream = new MemoryStream(imageAsBytes);
                    retSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                    GC.Collect();
                }
                return retSource;
            }
            catch (Exception e)
            {

                if (Debugger.IsAttached)
                {
                    Debug.WriteLine(e);
                }
                else
                {
                    Console.WriteLine(e);
                }
            }

            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
