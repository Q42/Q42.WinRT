using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Q42.WinRT.Controls
{
    /// <summary>
    /// WrapPanel implementation ported from Silverlight
    /// http://www.codeproject.com/Articles/24141/WrapPanel-for-Silverlight-2-0
    /// </summary>
   // [Obsolete("Check the WinRT XAML Toolkit for a better implementation of a WrapPanel. http://winrtxamltoolkit.codeplex.com")]
    public class WrapPanel : Panel
    {
        /// <summary>
        /// Orientation
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// OrientationProperty
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(WrapPanel), null);

        /// <summary>
        /// Simple WrapPanel implementation ported from Silverlight
        /// </summary>
        public WrapPanel()
        {
            // default orientation
            Orientation = Orientation.Horizontal;
        }

        /// <summary>
        /// MeasureOverride
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Point point = new Point(0, 0);
            int i = 0;

            foreach (UIElement child in Children)
            {
                child.Measure(new Size(availableSize.Width, availableSize.Height));
            }

            if (Orientation == Orientation.Horizontal)
            {
                double largestHeight = 0.0;

                foreach (UIElement child in Children)
                {
                    if (child.DesiredSize.Height > largestHeight)
                        largestHeight = child.DesiredSize.Height;


                    point.X = point.X + child.DesiredSize.Width;


                    if ((i + 1) < Children.Count)
                    {
                        if ((point.X + Children[i + 1].DesiredSize.Width) > availableSize.Width)
                        {
                            point.X = 0;
                            point.Y = point.Y + largestHeight;
                            largestHeight = 0.0;
                        }
                    }
                    else
                    {
                        point.X = availableSize.Width;
                        point.Y = point.Y + largestHeight;
                    }
                    i++;
                }
            }
            else
            {
                double largestWidth = 0.0;
                foreach (UIElement child in Children)
                {
                    if (child.DesiredSize.Width > largestWidth)
                        largestWidth = child.DesiredSize.Width;


                    point.Y = point.Y + child.DesiredSize.Height;


                    if ((i + 1) < Children.Count)
                    {
                        if ((point.Y + Children[i + 1].DesiredSize.Height) > availableSize.Height)
                        {
                            point.Y = 0;
                            point.X = point.X + largestWidth;
                            largestWidth = 0.0;
                        }
                    }
                    else
                    {
                        point.X = point.X + largestWidth;
                        point.Y = availableSize.Height;
                    }


                    i++;
                }
            }

            return new Size(point.X, point.Y);
        }

        /// <summary>
        /// ArrangeOverride
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Point point = new Point(0, 0);

            int i = 0;

            if (Orientation == Orientation.Horizontal)
            {
                double largestHeight = 0.0;

                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(point, new Point(point.X + child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Height > largestHeight)
                        largestHeight = child.DesiredSize.Height;

                    point.X = point.X + child.DesiredSize.Width;

                    if ((i + 1) < Children.Count)
                    {
                        if ((point.X + Children[i + 1].DesiredSize.Width) > finalSize.Width)
                        {
                            point.X = 0;
                            point.Y = point.Y + largestHeight;
                            largestHeight = 0.0;
                        }
                    }

                    i++;
                }
            }
            else
            {
                double largestWidth = 0.0;

                foreach (UIElement child in Children)
                {
                    child.Arrange(new Rect(point, new Point(point.X + child.DesiredSize.Width, point.Y + child.DesiredSize.Height)));

                    if (child.DesiredSize.Width > largestWidth)
                        largestWidth = child.DesiredSize.Width;

                    point.Y = point.Y + child.DesiredSize.Height;

                    if ((i + 1) < Children.Count)
                    {
                        if ((point.Y + Children[i + 1].DesiredSize.Height) > finalSize.Height)
                        {
                            point.Y = 0;
                            point.X = point.X + largestWidth;
                            largestWidth = 0.0;
                        }
                    }

                    i++;
                }
            }

            return base.ArrangeOverride(finalSize);
        }
    }
}
