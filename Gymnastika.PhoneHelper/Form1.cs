using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace Gymnastika.PhoneHelper
{
    public partial class Form1 : Form
    {
        HttpListener listener = new HttpListener();
        Image CurrentBitmap;
        private long frame = 0;
        private byte[] picture;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            IPAddress[] addrs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (var addr in addrs)
            {
                lblAddresses.Text = addr.ToString() + "\r\n";
            }
            listener.Prefixes.Add("http://*:998/");
            listener.Start();
            this.DragDrop += new DragEventHandler(DisplayHolder_DragDrop);
            this.DragOver += new DragEventHandler(DisplayHolder_DragOver);
            GetHttpContext();
            DisplayHolder.DoubleClick += new EventHandler(DisplayHolder_DoubleClick);
            //DisplayHolder.Invalidated += new InvalidateEventHandler(DisplayHolder_Invalidated);
            //DisplayHolder.Paint += new PaintEventHandler(DisplayHolder_Paint);
        }

        void DisplayHolder_Paint(object sender, PaintEventArgs e)
        {
            if (CurrentBitmap != null)
                e.Graphics.DrawImageUnscaledAndClipped(CurrentBitmap, DisplayHolder.ClientRectangle);
        }

        void DisplayHolder_Invalidated(object sender, InvalidateEventArgs e)
        {
            if (CurrentBitmap != null)
            {
                Graphics g = DisplayHolder.CreateGraphics();
                g.DrawImageUnscaledAndClipped(CurrentBitmap, DisplayHolder.ClientRectangle);
                g.Dispose();
            }


        }
        private void SetImage(Image image)
        {
            if (image == null)
                return;
            if (CurrentBitmap != null)
            {
                DisplayHolder.Image = null;
                CurrentBitmap.Dispose();
                CurrentBitmap = null;
            }
            //int dw, dh;
            //dw = 320;
            //dh = (int)(320.0 / image.Width * image.Height);
            //image = image.GetThumbnailImage(dw,dh, null, IntPtr.Zero);

            //CurrentBitmap = new Bitmap(dw , dh );
            //Graphics g = Graphics.FromImage(CurrentBitmap);
            //int w = CurrentBitmap.Width, h = CurrentBitmap.Height;
            //g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            //g.DrawImage(image, new Rectangle(0, 0, w, h)
            //    , new Rectangle(0, 0 , image.Width, image.Height ), GraphicsUnit.Pixel);
            //g.Flush();
            //g.Dispose();
            //image.Save("d:\\t1.jpg");
            //CurrentBitmap.Save("D:\\t2.jpg");
            CurrentBitmap = image;
            DisplayHolder.Image = CurrentBitmap;
            using (MemoryStream ms = new MemoryStream())
            {
                CurrentBitmap.Save(ms, ImageFormat.Png);
                picture = ms.ToArray();
            }
            ++frame;
        }
        void DisplayHolder_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                Image img = Clipboard.GetImage();

                SetImage(img);
            }
            catch { }
        }

        void DisplayHolder_DragOver(object sender, DragEventArgs e)
        {
            foreach (var item in e.Data.GetFormats())
            {
                if (item == DataFormats.FileDrop)
                {
                    string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
                    if (files.Length == 1)
                    {
                        try
                        {
                            Image bmp = Bitmap.FromFile(files[0]);
                            bmp.Dispose();
                            e.Effect = DragDropEffects.Link;
                            break;
                        }
                        catch { }
                    }
                }
            }
        }

        void DisplayHolder_DragDrop(object sender, DragEventArgs e)
        {
            foreach (var item in e.Data.GetFormats())
            {
                if (item == DataFormats.FileDrop)
                {
                    string filename = (e.Data.GetData(DataFormats.FileDrop) as string[])[0];
                    try
                    {
                        SetImage(Image.FromFile(filename));
                    }
                    catch { }

                    break;
                }
            }
        }
        private delegate void SendPictureDelegate(HttpListenerContext conetxt);
        private void SendPictureAsync(HttpListenerContext context)
        {
            int frame;
            if (!int.TryParse(context.Request.QueryString["frame"], out frame))
            {
                frame = 0;
            }
            while (frame >= this.frame || picture == null)
                Thread.Sleep(10);
            context.Response.OutputStream.Write(picture, 0, picture.Length);
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Close();
        }
        private void SendPicture(HttpListenerContext context)
        {
            SendPictureDelegate del = new SendPictureDelegate(SendPictureAsync);
            del.BeginInvoke(context, null, null);
        }
        public static void Contract(Stream target, object o)
        {
            DataContractSerializer ser = new DataContractSerializer(o.GetType());
            ser.WriteObject(target, o);
        }
        private void SendInfo(HttpListenerContext context)
        {
            string code = context.Request.QueryString["code"];
            if (code == null)
            {
                BadRequest(context);
                return;
            }
            FoodLibraryDataContext foodLibrary = new FoodLibraryDataContext();
            try
            {
                var food = foodLibrary.Foods.Where(f => f.Barcode == code).Single();

                context.Response.StatusCode = (int)HttpStatusCode.OK;
                Contract(context.Response.OutputStream, new FoodInfo(food));
            }
            catch
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            context.Response.Close();
        }
        private void PostPicture(HttpListenerContext context)
        {
            try
            {
                byte[] buffer = new byte[context.Request.ContentLength64];
                context.Request.InputStream.Read(buffer, 0, buffer.Length);
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                MemoryStream stream = new MemoryStream(buffer);
                File.WriteAllBytes("D:\\ttt.jpg", buffer);
                SetImage(new Bitmap(stream));
            }
            catch { context.Response.StatusCode = (int)HttpStatusCode.BadRequest; }


            context.Response.Close();


        }
        private void BadRequest(HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.StatusDescription = "BAD REQEUST";
            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                writer.Write("Bad request.");
            }
            context.Response.Close();
        }
        private void HandleContext(HttpListenerContext context)
        {

            string path = context.Request.Url.AbsolutePath.ToLower();
            if (context.Request.HttpMethod == "GET" && path == "/image/")
            {
                SendPicture(context);
            }
            else if (context.Request.HttpMethod == "GET" && path == "/info/")
            {
                SendInfo(context);
            }
            else if (context.Request.HttpMethod == "POST" && path == "/image/")
            {
                this.Invoke(new Action(() => { PostPicture(context); }));

            }
            else
            {
                BadRequest(context);
            }
        }
        private void GetHttpContext()
        {
            listener.BeginGetContext(
           new AsyncCallback((r) =>
           {
               HttpListenerContext context = listener.EndGetContext(r);
               GetHttpContext();
               HandleContext(context);

           }), listener);
        }

        private void DisplayHolder_Click(object sender, EventArgs e)
        {

        }
    }
}
