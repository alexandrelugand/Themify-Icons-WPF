using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ThemifyIcons.WIN81
{
    /// <summary>
    /// Represents ann icon that uses the ThemifyIcons font
    /// </summary>
    public class ThemifyIcons
        : FontIcon
    {
        private static readonly FontFamily ThemifyIconsFontFamily = new FontFamily("ms-appx:///ThemifyIcons.WIN81/themify.ttf#themify");

        public static readonly DependencyProperty IconProperty = 
            DependencyProperty.Register("Icon", typeof(ThemifyIconsIcon), typeof(ThemifyIcons),
                new PropertyMetadata(ThemifyIconsIcon.None, Icon_PropertyChangedCallback));

        private static void Icon_PropertyChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var themifyIcons = (ThemifyIcons) dependencyObject;

            var fontToSet = ThemifyIconsIcon.None;
            
            if (dependencyPropertyChangedEventArgs.NewValue != null)
                fontToSet = (ThemifyIconsIcon)dependencyPropertyChangedEventArgs.NewValue;

            themifyIcons.SetValue(FontFamilyProperty, ThemifyIconsFontFamily);
            themifyIcons.SetValue(GlyphProperty, char.ConvertFromUtf32((int)fontToSet));
        }

        public ThemifyIcons()
        {
            FontFamily = ThemifyIconsFontFamily;
        }

        /// <summary>
        /// Gets or sets the ThemifyIcons icon
        /// </summary>
        public ThemifyIconsIcon Icon
        {
            get { return (ThemifyIconsIcon)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
    }
}
