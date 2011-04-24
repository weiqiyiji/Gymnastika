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
                    if (DisplayHolder.Image != null)
                    {
                        DisplayHolder.Image.Dispose();
                        DisplayHolder.Image = null;
                    }
                    DisplayHolder.Image = Bitmap.FromFile(filename);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        DisplayHolder.Image.Save(ms, ImageFormat.Png);
                        picture = ms.ToArray();
                    }
                    ++frame;
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
                Bitmap bmp = new Bitmap(460, 800);

                BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppRgb);
                IntPtr ptr = data.Scan0;
                for (int i = 0; i < buffer.Length; ++i)
                {
                    Marshal.WriteByte(ptr, i, buffer[i]);
                }
                bmp.UnlockBits(data);
                DisplayHolder.Image = bmp;
                ++frame;
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
                PostPicture(context);
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
               HandleContext(listener.EndGetContext(r));
               GetHttpContext();
           }), listener);
        }
    }
}
