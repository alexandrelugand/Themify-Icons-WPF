using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ThemifyIcons.WPF.Converters
{
    /// <summary>
    /// Converts the CSS class name to a FontAwesomIcon and vice-versa.
    /// </summary>
    public class CssClassNameConverter
        : MarkupExtension, IValueConverter
    {
        private static readonly IDictionary<string, ThemifyIconsIcon> ClassNameLookup = new Dictionary<string, ThemifyIconsIcon>();
        private static readonly IDictionary<ThemifyIconsIcon, string> IconLookup = new Dictionary<ThemifyIconsIcon, string>();

        static CssClassNameConverter()
        {
            foreach (var value in Enum.GetValues(typeof(ThemifyIconsIcon)))
            {
                var memInfo = typeof(ThemifyIconsIcon).GetMember(value.ToString());
                var attributes = memInfo[0].GetCustomAttributes(typeof(IconIdAttribute), false);

                if (attributes.Length == 0) continue; // alias

                var id = ((IconIdAttribute)attributes[0]).Id;

                if (ClassNameLookup.ContainsKey(id)) continue;

                ClassNameLookup.Add(id, (ThemifyIconsIcon)value);
                IconLookup.Add((ThemifyIconsIcon)value, id);
            }
        }

        /// <summary>
        /// Gets or sets the mode of the converter
        /// </summary>
        public CssClassConverterMode Mode { get; set; }

        private static ThemifyIconsIcon FromStringToIcon(object value)
        {
            var icon = value as string;

            if (string.IsNullOrEmpty(icon)) return ThemifyIconsIcon.None;

            ThemifyIconsIcon rValue;

            if (!ClassNameLookup.TryGetValue(icon, out rValue))
            {
                rValue = ThemifyIconsIcon.None;
            }

            return rValue;
        }

        private static string FromIconToString(object value)
        {
            if (!(value is ThemifyIconsIcon)) return null;

            string rValue = null;

            IconLookup.TryGetValue((ThemifyIconsIcon) value, out rValue);
            
            return rValue;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Mode == CssClassConverterMode.FromStringToIcon)
                return FromStringToIcon(value);
            
            return FromIconToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Mode == CssClassConverterMode.FromStringToIcon)
                return FromIconToString(value);

            return FromStringToIcon(value);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// Defines the CssClassNameConverter mode. 
    /// </summary>
    public enum CssClassConverterMode
    {
        /// <summary>
        /// Default mode. Expects a string and converts to a ThemifyIconsIcon.
        /// </summary>
        FromStringToIcon = 0,
        /// <summary>
        /// Expects a ThemifyIconsIcon and converts it to a string.
        /// </summary>
        FromIconToString
    }
}
