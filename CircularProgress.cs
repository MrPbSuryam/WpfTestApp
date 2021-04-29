using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfTestApp
{
    public class CircularProgress : Shape
    {
        static CircularProgress()
        {
            Brush brush = new SolidColorBrush(Colors.Red);
            brush.Freeze();

            FillProperty.OverrideMetadata(
                typeof(CircularProgress),
                new FrameworkPropertyMetadata(brush));
        }

        public int CurrentCount
        {
            get { return (int)GetValue(CurrentCountProperty); }
            set { SetValue(CurrentCountProperty, value); }
        }

        private static FrameworkPropertyMetadata CurrentCountMetadata = new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,null);

        public static readonly DependencyProperty CurrentCountProperty = DependencyProperty.Register("CurrentCount", typeof(int), typeof(CircularProgress), CurrentCountMetadata);
        
        protected override Geometry DefiningGeometry
        {
            get
            {
                double startAngle = 90.0;
                double endAngle = 90.0 + ((CurrentCount / 100.0) * 360.0);

                double maxWidth = Math.Max(0.0, RenderSize.Width - StrokeThickness);
                double maxHeight = Math.Max(0.0, RenderSize.Height - StrokeThickness);

                double xStart = maxWidth / 2.0 * Math.Cos(startAngle * Math.PI / 180.0);
                double yStart = maxHeight / 2.0 * Math.Sin(startAngle * Math.PI / 180.0);

                double xEnd = maxWidth / 2.0 * Math.Cos(endAngle * Math.PI / 180.0);
                double yEnd = maxHeight / 2.0 * Math.Sin(endAngle * Math.PI / 180.0);

                if (CurrentCount > 25 && CurrentCount <= 50)
                {
                    base.Fill = new SolidColorBrush(Colors.Yellow);
                }
                else if (CurrentCount > 50)
                {
                    base.Fill = new SolidColorBrush(Colors.Green);
                }

                StreamGeometry streamGeometry = new StreamGeometry();
                using (StreamGeometryContext streamGeometryContext = streamGeometry.Open())
                {
                    streamGeometryContext.BeginFigure(
                        new Point((RenderSize.Width / 2.0) + xStart,
                                  (RenderSize.Height / 2.0) - yStart),
                        true,
                        true);
                    streamGeometryContext.ArcTo(
                        new Point((RenderSize.Width / 2.0) + xEnd,
                                  (RenderSize.Height / 2.0) - yEnd),
                        new Size(maxWidth / 2.0, maxHeight / 2),
                        0.0,
                        (endAngle - startAngle) > 180,
                        SweepDirection.Counterclockwise,
                        true,
                        false);
                    streamGeometryContext.LineTo(new Point((RenderSize.Width / 2.0), (RenderSize.Height / 2.0)), true, false);
                }

                return streamGeometry;
            }
        }
    }
}
