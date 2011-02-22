using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections;

namespace Gymnastika.Controls
{
    public class CheckButtonGroup : ListBox
    {
        private int _index;

        static CheckButtonGroup()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CheckButtonGroup), new FrameworkPropertyMetadata(typeof(CheckButtonGroup)));
        }

        public CheckButtonGroup()
        {
            _index = 0;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new CheckButtonGroupItem(this) { Index = _index++ };
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is CheckButtonGroupItem);
        }
    }
}
