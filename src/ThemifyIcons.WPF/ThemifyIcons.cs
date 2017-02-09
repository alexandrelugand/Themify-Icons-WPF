using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemifyIcons.WPF
{
    /// <summary>
    /// Provides a lightweight control for displaying a ThemifyIcons icon as text.
    /// </summary>
    public class ThemifyIcons
        : TextBlock, ISpinable, IRotatable, IFlippable
    {
        /// <summary>
        /// ThemifyIcons FontFamily.
        /// </summary>
        private static readonly FontFamily ThemifyIconsFontFamily = new FontFamily(new Uri("pack://application:,,,/ThemifyIcons.WPF;component/"), "./#themify");
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ThemifyIcons.Icon dependency property.
        /// </summary>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(ThemifyIconsIcon), typeof(ThemifyIcons), new PropertyMetadata(ThemifyIconsIcon.None, OnIconPropertyChanged));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ThemifyIcons.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty =
            DependencyProperty.Register("Spin", typeof(bool), typeof(ThemifyIcons), new PropertyMetadata(false, OnSpinPropertyChanged, SpinCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ThemifyIcons.Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinDurationProperty =
            DependencyProperty.Register("SpinDuration", typeof(double), typeof(ThemifyIcons), new PropertyMetadata(1d, SpinDurationChanged, SpinDurationCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ThemifyIcons.Rotation dependency property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty =
            DependencyProperty.Register("Rotation", typeof(double), typeof(ThemifyIcons), new PropertyMetadata(0d, RotationChanged, RotationCoerceValue));
        /// <summary>
        /// Identifies the ThemifyIcons.WPF.ThemifyIcons.FlipOrientation dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipOrientationProperty =
            DependencyProperty.Register("FlipOrientation", typeof(FlipOrientation), typeof(ThemifyIcons), new PropertyMetadata(FlipOrientation.Normal, FlipOrientationChanged));

        static ThemifyIcons()
        {
            OpacityProperty.OverrideMetadata(typeof(ThemifyIcons), new UIPropertyMetadata(1.0, OpacityChanged));
        }

        public ThemifyIcons()
        {
            IsVisibleChanged += (s, a) => CoerceValue(SpinProperty);
        }

        private static void OpacityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.CoerceValue(SpinProperty);
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

        private static void OnIconPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if NET40
            d.SetValue(TextOptions.TextRenderingModeProperty, TextRenderingMode.ClearType);
#endif
            d.SetValue(FontFamilyProperty, ThemifyIconsFontFamily);
            d.SetValue(TextAlignmentProperty, TextAlignment.Center);
            d.SetValue(TextProperty, char.ConvertFromUtf32((int)e.NewValue));
        }

        private static void OnSpinPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var themifyIcons = d as ThemifyIcons;

            if (themifyIcons == null) return;

            if ((bool)e.NewValue)
                themifyIcons.BeginSpin();
            else
            {
                themifyIcons.StopSpin();
                themifyIcons.SetRotation();
            }
        }

        private static object SpinCoerceValue(DependencyObject d, object basevalue)
        {
            var themifyIcons = (ThemifyIcons)d;

            if (!themifyIcons.IsVisible || themifyIcons.Opacity == 0.0 || themifyIcons.SpinDuration == 0.0)
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
            var themifyIcons = d as ThemifyIcons;

            if (null == themifyIcons || !themifyIcons.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;

            themifyIcons.StopSpin();
            themifyIcons.BeginSpin();
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
            var themifyIcons = d as ThemifyIcons;

            if (null == themifyIcons || themifyIcons.Spin || !(e.NewValue is double) || e.NewValue.Equals(e.OldValue)) return;

            themifyIcons.SetRotation();
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
            var themifyIcons = d as ThemifyIcons;

            if (null == themifyIcons || !(e.NewValue is FlipOrientation) || e.NewValue.Equals(e.OldValue)) return;

            themifyIcons.SetFlipOrientation();
        }
    }
}
