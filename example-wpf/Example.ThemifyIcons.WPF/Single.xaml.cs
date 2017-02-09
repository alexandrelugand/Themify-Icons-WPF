using System.Windows;
using System.Windows.Media;
using ThemifyIcons.WPF;

namespace Example.ThemifyIcons.WPF
{
    /// <summary>
    /// Interaction logic for Single.xaml
    /// </summary>
    public partial class Single : Window
    {
        public Single()
        {
            InitializeComponent();

            Icon = ImageThemify.CreateImageSource(ThemifyIconsIcon.Flag, Brushes.BlueViolet);
        }

        public ThemifyIconsIcon ThemifyIconsIcon { get { return ThemifyIconsIcon.Flag;} }
    }
}
