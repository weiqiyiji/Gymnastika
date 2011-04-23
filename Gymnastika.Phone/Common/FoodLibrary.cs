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
using System.Runtime.Serialization;
using System.Text;

namespace Gymnastika.Phone.Common
{
    [DataContract(Namespace = "")]
    public class FoodInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Protein { get; set; }
        [DataMember]
        public double Carbohydrate { get; set; }
        [DataMember]
        public double Fat { get; set; }
        [DataMember]
        public double Calories { get; set; }
        [DataMember]
        public string Barcode { get; set; }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            if (Fat > 0)
                sb.AppendLine("脂肪：" + Fat);
            if (Calories > 0)
                sb.AppendLine("卡路里：" + Calories);
            if (Protein > 0)
                sb.AppendLine("蛋白质:" + Protein);
            if (Carbohydrate > 0)
                sb.AppendLine("碳水化合物：" + Carbohydrate);
            return sb.ToString();
        }
    }
    public class GetFoodInfoCompeletedArgs : EventArgs
    {
        public FoodInfo Info { get; set; }
        public string Barcode { get; set; }
    }
    public class FoodLibrary
    {
        private Uri repositoryUri;
        public event EventHandler<GetFoodInfoCompeletedArgs> GetFoodInfoCompeleted;
        public FoodLibrary(Uri repository)
        {
            this.repositoryUri = repository;
        }
        public void GetFoodByBarcodeAsync(string barcode)
        {
            WebClient wc = new WebClient();
            wc.DownloadStringCompleted += new DownloadStringCompletedEventHandler(
                (s, e) =>
                {
                    if (GetFoodInfoCompeleted != null)
                    {
                        if (e.Error == null && !e.Cancelled)
                        {
                            try
                            {
                                GetFoodInfoCompeleted(this, new GetFoodInfoCompeletedArgs()
                                {
                                    Info = DataContractHelper.Decontract<FoodInfo>(e.Result),
                                    Barcode = e.UserState as string
                                });
                                return;
                            }
                            catch { }
                        }
                        GetFoodInfoCompeleted(this, new GetFoodInfoCompeletedArgs() { Barcode = e.UserState as string });
                    }
                });
            wc.DownloadStringAsync(
                new Uri(repositoryUri, "Info/?code=" + barcode), barcode);
        }
    }
}
