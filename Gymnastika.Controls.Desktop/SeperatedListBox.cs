using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Gymnastika.Controls.Desktop
{
    public class SeperatedListBox : ListBox
    {
        static SeperatedListBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(SeperatedListBox), new FrameworkPropertyMetadata(typeof(SeperatedListBox)));
        }
    }
}
