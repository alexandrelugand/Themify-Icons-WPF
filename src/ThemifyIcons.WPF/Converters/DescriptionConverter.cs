using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemifyIcons.WPF.Converters
{
    /// <summary>
    /// Converts a FontAwesomIcon to its description.
    /// </summary>
    public class DescriptionConverter
        : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ThemifyIconsIcon)) return null;

            var icon = (ThemifyIconsIcon) value;

            var memInfo = typeof(ThemifyIconsIcon).GetMember(icon.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes.Length == 0) return null; // alias

            return ((DescriptionAttribute)attributes[0]).Description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
