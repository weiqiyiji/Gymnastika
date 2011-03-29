using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Phone.Controls;
using System.Xml.Linq;

namespace Gymnastika.Phone.Common
{
    public class Util
    {
        private static string[] WeekDays;
        public static Network.LongAlive LA;
        static Util()
        {
            WeekDays = new string[] { "Sunday", "Monday", "Thuesday", "Wednesday", "Thursday ", "Friday", "Satuarday" };
        }
        public static string GetWeekDay()
        {
            return WeekDays[(int)DateTime.Now.DayOfWeek];
        }
        public BitmapImage ImageFromBytes(byte[] data)
        {
            BitmapImage img = new BitmapImage();
            using (MemoryStream ms = new MemoryStream(data))
            {
                img.SetSource(ms);
            }
            return img;
        }
        public BitmapImage ImageFromStream(Stream stream)
        {
            BitmapImage img = new BitmapImage();
            img.SetSource(stream);
            return img;
        }
        public int[] GetImageBufferInt(BitmapSource source, out int width, out int height)
        {
            WriteableBitmap bmp = new WriteableBitmap(source);
            width = bmp.PixelWidth;
            height = bmp.PixelHeight;
            return bmp.Pixels;
        }
        public int[] GetImageBufferInt(BitmapImage source, out int width, out int height)
        {
            WriteableBitmap bmp = new WriteableBitmap(source);
            width = bmp.PixelWidth;
            height = bmp.PixelHeight;
            return bmp.Pixels;
        }
        public static Size GetRootVisualSize()
        {
            return GetRootVisualSize(false);
        }
        public static Size GetRootVisualSize(bool IngoreOrientation)
        {
            var _rootVisual = App.Current.RootVisual as PhoneApplicationFrame;
            if (IngoreOrientation)
            {
                return new Size(_rootVisual.ActualWidth, _rootVisual.ActualHeight);
            }
            else
            {
                bool portrait = PageOrientation.Portrait == (PageOrientation.Portrait & _rootVisual.Orientation);
                double width = portrait ? _rootVisual.ActualWidth : _rootVisual.ActualHeight;
                double height = portrait ? _rootVisual.ActualHeight : _rootVisual.ActualWidth;
                return new Size(width, height);
            }
        }
        public static string GetXmlPureString(string input)
        {
            return XElement.Parse(input).Value;
        }
    }
}
