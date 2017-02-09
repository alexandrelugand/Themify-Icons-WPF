using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemifyIcons.WPF
{
    /// <summary>
    /// Represents a control that draws an ThemifyIcons icon as an image.
    /// </summary>
    public class ImageThemify
        : Image, ISpinable, IRotatable, IFlippable
    {
        /// <summary>
        /// ThemifyIcons FontFamily.
        /// </summary>
        private static readonly FontFamily ThemifyIconsFontFamily = new FontFamily(new Uri("pack://application:,,,/ThemifyIcons.WPF;component/"), "./#themify");
        /// <summary>
        /// Typeface used to generate ThemifyIcons icon.
        /// </summary>
        private static readonly Typeface ThemifyIconsTypeface = new Typeface(ThemifyIconsFontFamily, FontStyles.Normal,
            FontWeights.Normal, FontStretches.Normal);
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.Foreground dependency property.
        /// </summary>
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(ImageThemify), new PropertyMetadata(Brushes.Black, OnIconPropertyChanged));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ThemifyIconsIcon), typeof(ImageThemify), new PropertyMetadata(ThemifyIconsIcon.None, OnIconPropertyChanged));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool), typeof(ImageThemify), new PropertyMetadata(false, OnSpinPropertyChanged, SpinCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinDurationProperty =
            DependencyProperty.Register("SpinDuration", typeof(double), typeof(ImageThemify), new PropertyMetadata(1d, SpinDurationChanged, SpinDurationCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.Rotation dependency property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(ImageThemify), new PropertyMetadata(0d, RotationChanged, RotationCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ImageThemify.FlipOrientation dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipOrientationProperty =
            DependencyProperty.Register("FlipOrientation", typeof(FlipOrientation), typeof(ImageThemify), new PropertyMetadata(FlipOrientation.Normal, FlipOrientationChanged));

        static ImageThemify()
        {
            OpacityProperty.OverrideMetadata(typeof(ImageThemify), new UIPropertyMetadata(1.0, OpacityChanged));
        }

        public ImageThemify()
        {
            IsVisibleChanged += (s, a) => CoerceValue(SpinProperty);
        }

        private static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SpinProperty);
        }

        /// <summary>
        /// Gets or sets the foreground brush of the icon. Changing this property will cause the icon to be redrawn.
        /// </summary>
        public Brush Foreground
        {
            get { return (Brush)GetValue(ForegroundProperty); }
            set { SetValue(ForegroundProperty, value); }
        }
        /// <summary>
        /// Gets or sets the ThemifyIcons icon. Changing this property will cause the icon to be redrawn.
        /// </summary>
        public ThemifyIconsIcon Icon
        {
            get { return (ThemifyIconsIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        /// <summary>
        /// Gets or sets the current spin (angle) animation of the icon.
        /// </summary>
        public bool Spin
        {
            get { return (bool)GetValue(SpinProperty); }
            set { SetValue(SpinProperty, value); }
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageThemify = d as ImageThemify;

            if (imageThemify == null) return;

            if ((bool)e.NewValue)
                imageThemify.BeginSpin();
            else
            {
                imageThemify.StopSpin();
                imageThemify.SetRotation();
            }
        }

        private static object SpinCoerceValue(DependencyObject d, object basevalue)
        {
            var imageThemify = (ImageThemify)d;

            if (!imageThemify.IsVisible || imageThemify.Opacity == 0.0 || imageThemify.SpinDuration == 0.0)
                return false;

            return basevalue;
        }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will stop and start the spin animation.
        /// </summary>
        public double SpinDuration
        {
            get { return (double)GetValue(SpinDurationProperty); }
            set { SetValue(SpinDurationProperty, value); }
        }

        private static void SpinDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageThemify = d as ImageThemify;

            if (null == imageThemify || !imageThemify.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;

            imageThemify.StopSpin();
            imageThemify.BeginSpin();
        }

        private static object SpinDurationCoerceValue(DependencyObject d, object value)
        {
            double val = (double)value;
            return val < 0 ? 0d : value;
        }

        /// <summary>
        /// Gets or sets the current rotation (angle).
        /// </summary>
        public double Rotation
        {
            get { return (double)GetValue(RotationProperty); }
            set { SetValue(RotationProperty, value); }
        }

     
        private static void RotationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageThemify = d as ImageThemify;

            if (null == imageThemify || imageThemify.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;

            imageThemify.SetRotation();
        }

        private static object RotationCoerceValue(DependencyObject d, object value)
        {
            double val = (double)value;
            return val < 0 ? 0d : (val > 360 ? 360d : value);
        }

        /// <summary>
        /// Gets or sets the current orientation (horizontal, vertical).
        /// </summary>
        public FlipOrientation FlipOrientation
        {
            get { return (FlipOrientation)GetValue(FlipOrientationProperty); }
            set { SetValue(FlipOrientationProperty, value); }
        }

        private static void FlipOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageThemify = d as ImageThemify;

            if (null == imageThemify || !(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue)) return;
            
            imageThemify.SetFlipOrientation();
        }

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageThemify = d as ImageThemify;

            if (imageThemify == null) return;

            imageThemify.SetValue(SourceProperty, CreateImageSource(imageThemify.Icon, imageThemify.Foreground));
        }


        /// <summary>
        /// Creates a new System.Windows.Media.ImageSource of a specified ThemifyIconsIcon and foreground System.Windows.Media.Brush.
        /// </summary>
        /// <param name="icon">The ThemifyIcons icon to be drawn.</param>
        /// <param name="foregroundBrush">The System.Windows.Media.Brush to be used as the foreground.</param>
        /// <returns>A new System.Windows.Media.ImageSource</returns>
        public static ImageSource CreateImageSource(ThemifyIconsIcon icon, Brush foregroundBrush, double emSize = 100)
        {
            var charIcon = char.ConvertFromUtf32((int)icon);

            var visual = new DrawingVisual();
            using (var drawingContext = visual.RenderOpen())
            {
                drawingContext.DrawText(
                    new FormattedText(charIcon, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                        ThemifyIconsTypeface, emSize, foregroundBrush) { TextAlignment = TextAlignment.Center }, new Point(0, 0));
            }
            return new DrawingImage(visual.Drawing);
        }

    }
}
