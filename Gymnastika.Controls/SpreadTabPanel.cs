using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Gymnastika.Controls
{
    public class SpreadTabPanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size();
            foreach (UIElement child in InternalChildren)
            {
                child.Measure(availableSize);
                size.Width += child.DesiredSize.Width;
                size.Height = Math.Max(size.Height, child.DesiredSize.Height);
            }

            return double.IsPositiveInfinity(availableSize.Width) || double.IsPositiveInfinity(availableSize.Height)
                ? size : availableSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (InternalChildren == null || InternalChildren.Count == 0) return finalSize;
            if (InternalChildren.Count == 1)
            {
                InternalChildren[0].Arrange(new Rect(0, 0, finalSize.Width, finalSize.Height));
               return finalSize;
            }

            int endIndex = InternalChildren.Count / 2 + (InternalChildren.Count % 2 == 0 ? 0 : 1);
            double averageChildWidth = finalSize.Width / InternalChildren.Count;
            double widthSoFarFromLeft = 0.0;
            double widthSoFarFromRight = 0.0;

            for (int i = 0; i < endIndex; i++)
            {
                UIElement leftChild = InternalChildren[i];
                double leftChildWidth = GetChildWidth(leftChild, averageChildWidth);
                leftChild.Arrange(
                    new Rect(
                        widthSoFarFromLeft, 
                        0,
                        leftChildWidth, 
                        finalSize.Height));
                widthSoFarFromLeft += leftChildWidth;

                int rightChildIndex = InternalChildren.Count - i - 1;
                UIElement rightChild = InternalChildren[rightChildIndex];
                double rightChildWidth = GetChildWidth(rightChild, averageChildWidth);
                rightChild.Arrange(
                    new Rect(
                        finalSize.Width - widthSoFarFromRight - rightChildWidth, 
                        0,
                        rightChildWidth, 
                        finalSize.Height));
                widthSoFarFromRight += rightChildWidth;
            }

            return finalSize;
        }

        private double GetChildWidth(UIElement child, double averageChildWidth)
        {
            return child.DesiredSize.Width < averageChildWidth ? child.DesiredSize.Width : averageChildWidth;
        }
    }
}
