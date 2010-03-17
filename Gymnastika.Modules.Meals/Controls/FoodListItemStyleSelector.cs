using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace Gymnastika.Modules.Meals.Controls
{
    public class FoodListItemStyleSelector : StyleSelector
    {
        public Style FirstItemStyle { get; set; }

        public Style SecondItemStyle { get; set; }

        public Style ThirdItemStyle { get; set; }

        public Style FourthItemStyle { get; set; }

        public Style FifthItemStyle { get; set; }

        public Style SixthItemStyle { get; set; }

        public Style SeventhItemStyle { get; set; }

        public Style EighthItemStyle { get; set; }

        public Style NinthItemStyle { get; set; }

        public Style TenthItemStyle { get; set; }

        private int row = 0;

        public override Style SelectStyle(object item, DependencyObject container)
        {
            row++;
            switch (row)
            {
                case 1:
                    return FirstItemStyle;
                case 2:
                    return SecondItemStyle;
                case 3:
                    return ThirdItemStyle;
                case 4:
                    return FourthItemStyle;
                case 5:
                    return FifthItemStyle;
                case 6:
                    return SixthItemStyle;
                case 7:
                    return SeventhItemStyle;
                case 8:
                    return EighthItemStyle;
                case 9:
                    return NinthItemStyle;
                case 10:
                    return TenthItemStyle;
                default:
                    return base.SelectStyle(item, container);
            }
        }
    }
}
