﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Diagnostics;

namespace Gymnastika.Controls
{
	public class AnimatedListPanel : Panel
	{
        public AnimatedListPanel()
        {
            Delay = 0.1;
            XDuration = 0.6;
            YDuration = 1;
        }


        public static readonly DependencyProperty ItemsDistanceProperty =
            DependencyProperty.Register("ItemsDistance",
            typeof(double),
            typeof(AnimatedListPanel),
            new PropertyMetadata(10d, DistanceChanged));
        
        protected static void DistanceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sender = d as AnimatedListPanel;
            sender.InvalidateArrange();
        }


        //Item间隔
        public double ItemsDistance
        {
            get { return (double)GetValue(ItemsDistanceProperty); }
            set { SetValue(ItemsDistanceProperty, value); }
        }


        //加载延迟
        public double Delay { get; set; }


        //上下移动速度
        public double XDuration { get; set; }

        //左右移动速度
        public double YDuration { get; set; }



        public static DependencyProperty IsNewProperty =
            DependencyProperty.RegisterAttached(
            "IsNew", 
            typeof(bool), 
            typeof(AnimatedListPanel), 
            new PropertyMetadata(true));


        protected override Size MeasureOverride(Size constraint)
        {
            double width = 0;
            double height = 0;
            double distance = ItemsDistance;
            foreach (ContentControl child in InternalChildren)
            {
                child.Measure(constraint);
                Size ds = child.DesiredSize;
                width = Math.Max(width, ds.Width);
                height += distance + ds.Height;
            }
            height = Math.Max(height - distance, 0);
            height = Math.Min(height, constraint.Height);
            width = Math.Min(width, constraint.Width);
            //Height += 100;
            //return base.MeasureOverride(new Size(width,height));
            return new Size(width, height);
        }



        protected override Size ArrangeOverride(Size arrangeSize)
        {
            //return base.ArrangeOverride(arrangeSize);
            //start point
            var itemsDistance = ItemsDistance;
            var currentTop = 0d;
            var width = arrangeSize.Width;
            var index = 0;
            
            foreach (ContentControl child in InternalChildren)
            {

                child.Arrange(new Rect(0, 0, width, child.DesiredSize.Height));
                
                //Get transform
                var trans = GetTranslateTransform(child);

                //IsNew?
                bool isNew = IsNewItem(child);


                if (isNew)
                {
                    child.SetValue(IsNewProperty, false);
                    ++index;

                    //Begin X Animation
                    var startLeft = -width - Margin.Left;
                    trans.SetValue(TranslateTransform.YProperty, currentTop);

                    double offset = index * 5;
                    if (offset + startLeft < 0)
                    {
                        trans.SetValue(TranslateTransform.XProperty, startLeft);
                        var animation = new DoubleAnimation(startLeft + offset, 0, TimeSpan.FromSeconds(XDuration));
                        animation.BeginTime = TimeSpan.FromSeconds(index * Delay);
                        animation.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 8 };
                        trans.BeginAnimation(TranslateTransform.XProperty,
                                animation,
                               HandoffBehavior.SnapshotAndReplace);
                    }
                    else
                    {
                        trans.SetValue(TranslateTransform.XProperty, 0d);
                    }
                }
                
                //Begin Y Animation
                var animationY = new DoubleAnimation(currentTop, TimeSpan.FromSeconds(YDuration));
                animationY.EasingFunction = new ElasticEase() { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 8 };
                trans.BeginAnimation(TranslateTransform.YProperty,
                                            animationY,
                                            HandoffBehavior.SnapshotAndReplace);


                currentTop += child.DesiredSize.Height + itemsDistance;
            
            
            }
            return arrangeSize;
            //return base.ArrangeOverride(arrangeSize);
        }

		private TranslateTransform GetTranslateTransformFromGroup(TransformGroup combinedTransform)
		{
			var result = combinedTransform.Children
							.Where(transform => transform is TranslateTransform).FirstOrDefault() as TranslateTransform;

			return result;
		}

        private TranslateTransform GetTranslateTransform(ContentControl child)
        {
            //Get Group
            TransformGroup combinedTrans = new TransformGroup();
            if (child.RenderTransform == null)
            {
                child.RenderTransform = combinedTrans;
            }
            else if (child.RenderTransform is TransformGroup)
            {
                combinedTrans = child.RenderTransform as TransformGroup;
            }
            else
            {
                combinedTrans.Children.Add(child.RenderTransform);
                child.RenderTransform = combinedTrans;
            }

            var trans = GetTranslateTransformFromGroup(combinedTrans);

            //Get TranslateTransForm

            if (trans == null)
            {
                trans = new TranslateTransform();
                combinedTrans.Children.Add(trans);
            }
            return trans;
        }

        private bool IsNewItem(ContentControl child)
        {
           return (bool)child.GetValue(IsNewProperty);
        }
	}
}