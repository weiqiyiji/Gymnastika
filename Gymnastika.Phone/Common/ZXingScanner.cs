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
using ZXing = com.google.zxing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Threading;
namespace Gymnastika.Phone.Common
{
    public class BarcodeScanedEventArgs : EventArgs
    {
        public bool Successful { get; set; }
        public string Code { get; set; }
        public ZXing.ResultPoint[] DetectedPoints { get; set; }
        public Point Point1 { get; set; }
        public Point Point2 { get; set; }
        public BitmapSource Image { get; set; }
    }
    public class BarcodeImageCaputredEventArgs : EventArgs
    {
        public Stream ImageStream { get; set; }
    }
    public class ZXingScanner
    {
        public event EventHandler<BarcodeScanedEventArgs> Scanned;
        public event EventHandler<BarcodeImageCaputredEventArgs> Captured;
        private long CaptureFrame = 0;
        private bool isCaputuring = false;
        public void CaptureBitmap()
        {
            if (isCaputuring)
                return;
            Random rnd = new Random();
            WebRequest request = WebRequest.CreateHttp("http://localhost:998/image/?frame=" + CaptureFrame);
            isCaputuring = true;
            request.BeginGetResponse(
                new AsyncCallback((r) =>
                {
                    isCaputuring = false;
                    try
                    {
                        WebResponse response = request.EndGetResponse(r);
                        Stream stream = response.GetResponseStream();
                        MemoryStream ms = new MemoryStream();
                        byte[] buffer = new byte[1024];
                        int size;
                        while ((size = (stream.Read(buffer, 0, 1024))) > 0)
                        {
                            ms.Write(buffer, 0, size);
                        }
                        ++CaptureFrame;
                        if (Captured != null)
                            Captured(this, new BarcodeImageCaputredEventArgs() { ImageStream = ms });
                    }
                    catch
                    {
                        if (Captured != null)
                            Captured(this, new BarcodeImageCaputredEventArgs());
                    }

                }), null);
        }
        public void Scan(BitmapSource Image)
        {
            ZXing.ResultPoint[] points = null;

            try
            {
                WriteableBitmap bmp = new WriteableBitmap(Image);

                RGBLuminanceSource luminance = new RGBLuminanceSource(bmp, bmp.PixelWidth, bmp.PixelHeight);
                ZXing.common.HybridBinarizer binarizer = new ZXing.common.HybridBinarizer(luminance);
                ZXing.BinaryBitmap binaryBitmap = new ZXing.BinaryBitmap(binarizer);

                ZXing.oned.EAN13Reader reader = new ZXing.oned.EAN13Reader();
                ZXing.Result result = reader.decode(binaryBitmap);

                if (Scanned != null)
                    Scanned(this, new BarcodeScanedEventArgs()
                    {
                        Code = result.Text,
                        Successful = true,
                        Image = Image,
                        Point1 = new Point(result.ResultPoints[0].X, result.ResultPoints[0].Y),
                        Point2 = new Point(result.ResultPoints[1].X, result.ResultPoints[1].Y),
                        DetectedPoints = points
                    });
            }
            catch
            {
                if (Scanned != null)
                    Scanned(this, new BarcodeScanedEventArgs() { Successful = false, Image = Image, DetectedPoints = points });
            }

        }

    }
}
