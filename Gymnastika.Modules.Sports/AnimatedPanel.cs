using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Gymnastika.Modules.Sports
{
	public class AnimatedPanel : Panel
	{
		public int ItemSize
		{
			get { return (int)GetValue(ItemSizeProperty); }
			set { SetValue(ItemSizeProperty, value); }
		}

		// Using a DependencyProperty as the backing store for ItemSize.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemSizeProperty =
			DependencyProperty.Register("ItemSize",
				typeof(int),
				typeof(AnimatedPanel));

		private Dictionary<int, Point> currentMap =
			new Dictionary<int, Point>();

		private Dictionary<int, Point> previousMap =
			new Dictionary<int, Point>();

		private Dictionary<UIElement, Point> previousChildren;

		private const int itemsDistance = 10; //px

		protected override Size MeasureOverride(Size availableSize)
		{
			var currentLeft = 10d;
			var currentTop = 10d;
			var width = ItemSize;
			var height = ItemSize;

			foreach (UIElement child in InternalChildren)
			{
				child.Measure(new Size(ItemSize, ItemSize));
				if ((currentLeft + width) > availableSize.Width)
				{
					currentTop += (height + itemsDistance);
					currentLeft = 10;
				}
				currentLeft += (width + itemsDistance);
			}

			currentTop += (height + itemsDistance);
			var resSize = new Size();
			resSize.Width = double.IsPositiveInfinity(availableSize.Width)
								? currentLeft
								: availableSize.Width;

			resSize.Height = double.IsPositiveInfinity(availableSize.Height)
								? currentTop
								: availableSize.Height;

			return resSize;
		}

		protected override Size ArrangeOverride(Size arrangeSize)
		{
			previousMap = currentMap;
			currentMap = new Dictionary<int, Point>();

			var currentLeft = 10d;
			var currentTop = 10d;
			var startPoint = new Point(currentLeft, currentTop);
			var width = ItemSize;
			var height = ItemSize;
			
			if (IsItemsHost)
			{
				foreach (ContentControl child in InternalChildren)
				{
					
					var childContent = child.Content;
					var hashCode = childContent.GetHashCode();
					if ((currentLeft + width) > arrangeSize.Width)
					{
						currentTop += (height + itemsDistance);
						currentLeft = 10;
					}

					var point = new Point(currentLeft, currentTop);
					currentMap.Add(childContent.GetHashCode(), point);

					var from = new Vector(double.NaN, double.NaN);

					var isNew = false;
					var combinedTransform = new TransformGroup();
					if (child.RenderTransform != null)
					{
						if (child.RenderTransform is TransformGroup)
						{
							combinedTransform = child.RenderTransform as TransformGroup;
						}
						else
						{
							combinedTransform.Children.Add(child.RenderTransform);
						}
					}
					var trans = GetTranslateTransform(combinedTransform);
					if (trans == null)
					{
						child.RenderTransformOrigin = new Point(0, 0);
						trans = new TranslateTransform();
						combinedTransform.Children.Add(trans);
						child.RenderTransform = combinedTransform;

						Point previousPoint;
						if (previousMap.TryGetValue(hashCode, out previousPoint))
						{
							from = previousPoint - point;
							previousMap.Remove(hashCode);
						}
						else
						{
							isNew = true;
						}
					}
					else
					{
						var fromPoint = child.TranslatePoint(new Point(), this);
						from = fromPoint - point;
					}
					child.Arrange(new Rect(point, child.DesiredSize));
					if (isNew)
					{
						var fromPoint = child.TranslatePoint(new Point(), this);
						from = startPoint - fromPoint;
					}

					if (!double.IsNaN(from.X))
					{
						trans.BeginAnimation(TranslateTransform.XProperty,
											 new DoubleAnimation(from.X, 0.0d, duration),
											 HandoffBehavior.SnapshotAndReplace);
					}
					if (!double.IsNaN(from.Y))
					{
						trans.BeginAnimation(TranslateTransform.YProperty,
											 new DoubleAnimation(from.Y, 0.0d, duration),
											 HandoffBehavior.SnapshotAndReplace);
					}

					currentLeft += (width + itemsDistance);
				}
			}
			else
			{
				var currentChildren = previousChildren ?? new Dictionary<UIElement, Point>();
				previousChildren = new Dictionary<UIElement, Point>();

				foreach (UIElement child in InternalChildren)
				{
					if ((currentLeft + width) > arrangeSize.Width)
					{
						currentTop += (height + itemsDistance);
						currentLeft = 10;
					}

					var point = new Point(currentLeft, currentTop);

					var from = new Vector(double.NaN, double.NaN);

					var isNew = false;
					var combinedTransform = new TransformGroup();
					if (child.RenderTransform != null)
					{
						if (child.RenderTransform is TransformGroup)
						{
							combinedTransform = child.RenderTransform as TransformGroup;
						}
						else
						{
							combinedTransform.Children.Add(child.RenderTransform);
						}
						Point previousPoint;
						if (currentChildren.TryGetValue(child, out previousPoint))
						{
							from = previousPoint - point;
							//currentChildren.Remove(hashCode);
						}
						else
						{
							isNew = true;
						}
					}

					var trans = GetTranslateTransform(combinedTransform);
					if (trans == null)
					{
						child.RenderTransformOrigin = new Point(0, 0);
						trans = new TranslateTransform();
						combinedTransform.Children.Add(trans);
						child.RenderTransform = combinedTransform;
						isNew = true;
					}
					else
					{
						var fromPoint = child.TranslatePoint(new Point(), this);
						from = fromPoint - point;
					}
					child.Arrange(new Rect(point, child.DesiredSize));

					if (isNew)
					{
						var fromPoint = child.TranslatePoint(new Point(), this);
						from = startPoint - fromPoint;
					}

					if (!double.IsNaN(from.X))
					{
						trans.BeginAnimation(TranslateTransform.XProperty,
											 new DoubleAnimation(from.X, 0.0d, duration),
											 HandoffBehavior.SnapshotAndReplace);
					}
					if (!double.IsNaN(from.Y))
					{
						trans.BeginAnimation(TranslateTransform.YProperty,
											 new DoubleAnimation(from.Y, 0.0d, duration),
											 HandoffBehavior.SnapshotAndReplace);
					}

					currentLeft += (width + itemsDistance);
					previousChildren.Add(child, point);
				}
			}

			return arrangeSize;
		}

		private TranslateTransform GetTranslateTransform(TransformGroup combinedTransform)
		{
			var result = combinedTransform.Children
							.Where(transform => transform is TranslateTransform).FirstOrDefault() as TranslateTransform;

			return result;
		}

		private readonly Duration duration = new Duration(TimeSpan.FromSeconds(0.5));
	}
}