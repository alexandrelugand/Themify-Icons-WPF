using System.Windows;
using System.Windows.Media;
using ThemifyIcons.WPF;

namespace Example.ThemifyIcons.WPF
{
    /// <summary>
    /// Interaction logic for SingleAnimation.xaml
    /// </summary>
    public partial class SingleAnimation : Window
    {
        public SingleAnimation()
        {
            InitializeComponent();

            Icon = ImageThemify.CreateImageSource(ThemifyIconsIcon.Reload, Brushes.BlueViolet);
        }
    }
}
