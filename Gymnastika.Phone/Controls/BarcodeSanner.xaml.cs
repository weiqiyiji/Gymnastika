using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.Threading;

namespace Gymnastika.Phone.Controls
{
    public class BarcodeScanBeginArgs : EventArgs
    {

    }
    public class BarcodeScanCompeletedArgs : EventArgs
    {
        public string Code { get; set; }
        public bool Successful { get; set; }
        public ImageSource CodeImage { get; set; }
    }
    public partial class BarcodeSanner : UserControl
    {
        private bool IsScanning = false;
        public string Code { get; private set; }
        public event EventHandler<BarcodeScanBeginArgs> ScanBegin;
        public event EventHandler<BarcodeScanCompeletedArgs> ScanCompeleted;
        GestureListener listener;
        Common.ZXingScanner scanner = new Common.ZXingScanner();
        public BarcodeSanner()
        {
            InitializeComponent();
            listener = GestureService.GetGestureListener(this);
            this.Loaded += new RoutedEventHandler(BarcodeSanner_Loaded);
            scanner.Scanned += new EventHandler<Common.BarcodeScanedEventArgs>(scanner_Scanned);
            scanner.Captured += new EventHandler<Common.BarcodeImageCaputredEventArgs>(scanner_Captured);
        }

        void scanner_Captured(object sender, Common.BarcodeImageCaputredEventArgs e)
        {
            this.Dispatcher.BeginInvoke(
                delegate
                {
                    BitmapImage image = new BitmapImage();
                    image.SetSource(e.ImageStream);
                    ScanImage(image);
                });

        }
        public void TestScan()
        {
            scanner.CaptureBitmap();
        }
        private void CopyImage(WriteableBitmap src, WriteableBitmap to, Rect rcSrc, Rect rcTo)
        {
            double scaleX = (double)rcSrc.Width / rcTo.Width,
                    scaleY = (double)rcSrc.Height / rcTo.Height;

            for (int y = (int)rcTo.Top; y < (int)rcTo.Bottom; ++y)
            {
                int yOffsetSrc = (int)(rcSrc.Top + y * scaleY) * src.PixelWidth;
                int yOffsetTo = (int)(rcTo.Top + y) * to.PixelWidth;
                for (int x = (int)rcTo.Left; x < (int)rcTo.Right; ++x)
                {
                    to.Pixels[yOffsetTo + x] = src.Pixels[yOffsetSrc + (int)(rcSrc.Left + x * scaleX)];
                }
            }
        }
        WriteableBitmap GetBarcodeImage(WriteableBitmap bitmap, int x1, int y1, int x2, int y2)
        {
            WriteableBitmap final;
            int cY = (y1 + y2) / 2;
            int w, h;
            w = (int)(Math.Abs(x2 - x1) * 1.3);
            if ((x2 + x1) / 2 - w / 2 < 0)
                w = (x2 + x1); ;
            h = (int)((w * 0.8 > bitmap.PixelHeight) ? bitmap.PixelHeight : w * 0.8);
            Rect rcSrc = new Rect((x2 + x1) / 2 - w / 2,
                (cY - h / 2 < 0) ? 0 : cY - h / 2, w, h
                );
            Rect rcTo = new Rect(0, 0, BarcodeImage.ActualWidth, BarcodeImage.ActualWidth / w * h);
            final = new WriteableBitmap((int)rcTo.Width, (int)rcTo.Height);
            CopyImage(bitmap, final, rcSrc, rcTo);
            return final;
        }
        void scanner_Scanned(object sender, Common.BarcodeScanedEventArgs e)
        {
            if (ScanCompeleted != null)
            {
                this.Dispatcher.BeginInvoke(delegate
                    {
                        if (e.Successful)
                        {
                            this.Code = e.Code;
                            WriteableBitmap image = new WriteableBitmap(e.Image);
                            BarcodeImage.Source = GetBarcodeImage(image, (int)e.Point1.X, (int)e.Point1.Y,
                                (int)e.Point2.X, (int)e.Point2.Y);
                            ScanCompeleted(this, new BarcodeScanCompeletedArgs()
                            {
                                Successful = true,
                                CodeImage = BarcodeImage.Source,
                                Code = e.Code
                            });
                            txtResult.Text = "扫描结果：" + e.Code;
                        }
                        else
                        {
                            this.Code = "";
                            BarcodeImage.Source = e.Image;
                            txtResult.Text = "没有识别到条形码";
                            ScanCompeleted(this, new BarcodeScanCompeletedArgs() { Successful = false });
                        }
                    });
            }
        }
        void BarcodeSanner_Loaded(object sender, RoutedEventArgs e)
        {
            listener.Tap += new EventHandler<GestureEventArgs>(listener_Tap);
        }
        public void ScanImage(BitmapSource image)
        {
            Scan(image);
        }
        private void Scan(object image)
        {
            this.Dispatcher.BeginInvoke(
                delegate
                {
                    BarcodeImage.Source = (BitmapSource)image;
                    scanner.Scan((BitmapSource)image);
                });

        }
        public void ScanImageAsync(BitmapSource image)
        {
            Thread th = new Thread(new ParameterizedThreadStart(Scan));
            th.Start(image);
        }
        void listener_Tap(object sender, GestureEventArgs e)
        {
            if (ScanBegin != null)
                ScanBegin(this, new BarcodeScanBeginArgs());
        }
    }
}
