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

namespace Gymnastika.Phone.Common
{
    public static class MicroBlog
    {
        public delegate void OnPublishCompeletedHandler(Guid guid, bool Seccessful);
        public static event OnPublishCompeletedHandler OnPublishCompeleted;
        public class MicroBlogReply
        {
        }
        public class MicroBlogItem
        {
            public Guid Guid { get; internal set; }
            public string Message { get; internal set; }
            public DateTime PostTime { get; internal set; }
            public int ReplyCount { get; internal set; }
            public MicroBlogReply[] Replies { get; internal set; }
            public void MoreReply()
            {

            }
        }
        public  static void BeginPublish(Guid guid,string Message)
        {
        }
    }
}
