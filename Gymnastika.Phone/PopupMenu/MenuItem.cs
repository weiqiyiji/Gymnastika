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

namespace Gymnastika.Phone.PopupMenu
{
    public class MenuItem
    {
        internal delegate void PopMenuItemClick(object sender);
        public ImageSource Icon{get;set;}
        public string Text { get; set; }

    }
}
