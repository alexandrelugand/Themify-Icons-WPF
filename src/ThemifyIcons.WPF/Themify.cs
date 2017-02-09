using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ThemifyIcons.WPF
{
    /// <summary>
    /// Provides attached properties to set ThemifyIcons icons on controls.
    /// </summary>
    public static class Themify
    {

        /// <summary>
        /// ThemifyIcons FontFamily.
        /// </summary>
        private static readonly FontFamily ThemifyIconsFontFamily = new FontFamily(new Uri("pack://application:,,,/ThemifyIcons.WPF;component/"), "./#themify");

        /// <summary>
        /// Identifies the ThemifyIcons.WPF.Themify.Content attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.RegisterAttached(
                "Content",
                typeof(ThemifyIconsIcon),
                typeof(Themify),
                new PropertyMetadata(DEFAULT_CONTENT, ContentChanged));

        /// <summary>
        /// Gets the content of a ContentControl, expressed as a ThemifyIcons icon.
        /// </summary>
        /// <param name="target">The ContentControl subject of the query</param>
        /// <returns>ThemifyIcons icon found as content</returns>
        public static ThemifyIconsIcon GetContent(DependencyObject target)
        {
            return (ThemifyIconsIcon)target.GetValue(ContentProperty);
        }
        
        /// <summary>
        /// Sets the content of a ContentControl expressed as a ThemifyIcons icon. This will cause the content to be redrawn.
        /// </summary>
        /// <param name="target">The ContentControl where to set the content</param>
        /// <param name="value">ThemifyIcons icon to set as content</param>
        public static void SetContent(DependencyObject target, ThemifyIconsIcon value)
        {
            target.SetValue(ContentProperty, value);
        }

        private static void ContentChanged(DependencyObject sender, DependencyPropertyChangedEventArgs evt)
        {
            // If target is not a ContenControl just ignore: Themify.Content property can only be set on a ContentControl element
            if (!(sender is ContentControl)) return;

            ContentControl target = (ContentControl) sender;

            // If value is not a ThemifyIconsIcon just ignore: Themify.Content property can only be set to a ThemifyIconsIcon value
            if (!(evt.NewValue is ThemifyIconsIcon)) return;

            ThemifyIconsIcon symbolIcon = (ThemifyIconsIcon)evt.NewValue;
            int symbolCode = (int)symbolIcon;
            char symbolChar = (char)symbolCode;

            target.FontFamily = ThemifyIconsFontFamily;
            target.Content = symbolChar;
        }

        private const ThemifyIconsIcon DEFAULT_CONTENT = ThemifyIconsIcon.None;
    }
}
