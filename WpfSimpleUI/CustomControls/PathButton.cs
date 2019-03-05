using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfSimpleUI.CustomControls
{
    public class PathButton : Button
    {
        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));

            BackgroundProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Transparent)));
        }

        public Geometry PathGeometry { get { return (Geometry)GetValue(PathGeometryProperty); } set { SetValue(PathGeometryProperty, value); } }
        public static readonly DependencyProperty PathGeometryProperty = DependencyProperty.Register(nameof(PathGeometry), typeof(Geometry), typeof(PathButton),
            new FrameworkPropertyMetadata(null));

        public Brush PathDefaultBrush { get { return (Brush)GetValue(PathDefaultBrushProperty); } set { SetValue(PathDefaultBrushProperty, value); } }
        public static readonly DependencyProperty PathDefaultBrushProperty = DependencyProperty.Register(nameof(PathDefaultBrush), typeof(Brush), typeof(PathButton),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black)));

        public Brush PathMouseOverBrush { get { return (Brush)GetValue(PathMouseOverBrushProperty); } set { SetValue(PathMouseOverBrushProperty, value); } }
        public static readonly DependencyProperty PathMouseOverBrushProperty = DependencyProperty.Register(nameof(PathMouseOverBrush), typeof(Brush), typeof(PathButton),
            new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Silver)));
    }
}
