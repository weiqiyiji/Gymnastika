using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Controls;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Gymnastika.Common.Utils
{
    public class ImageHelper
    {
        private static Tuple<int, int> CalculateScaledSize(int originWidth, int originHeight, int width, int height)
        {
            double scaleFactor = 0.0;

            if (originWidth > originHeight)
            {
                scaleFactor = (double)originWidth / width;
            }
            else if (originWidth == originHeight)
            {
                scaleFactor = (double)originWidth / (width > height ? height : width);
            }
            else
            {
                scaleFactor = (double)originHeight / height;
            }

            return new Tuple<int, int>((int)(originWidth / scaleFactor), (int)(originHeight / scaleFactor));
        }

        public static void SaveBitmap(string originPath, string targetPath, int width, int height)
        {
            System.Drawing.Image originImage = null;
            Bitmap savedBitmap = null;
            Graphics g;

            try
            {
                originImage = Bitmap.FromFile(originPath);
                Tuple<int, int> actualSize = CalculateScaledSize(originImage.Width, originImage.Height, width, height);

                savedBitmap = new Bitmap(actualSize.Item1, actualSize.Item2);
                g = Graphics.FromImage(savedBitmap);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(
                    originImage,
                    new Rectangle(0, 0, actualSize.Item1, actualSize.Item2),
                    new Rectangle(0, 0, originImage.Width, originImage.Height),
                    GraphicsUnit.Pixel);

                g.Dispose();
                originImage.Dispose();

                if (File.Exists(targetPath))
                    File.Delete(targetPath);

                savedBitmap.Save(targetPath);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                savedBitmap.Dispose();
            }
        }
    }
}
