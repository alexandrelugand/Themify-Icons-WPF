using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Example.ThemifyIcons.WPF.ViewModel;
using ThemifyIcons.WPF;

namespace Example.ThemifyIcons.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Icon = ImageThemify.CreateImageSource(ThemifyIconsIcon.ThemifyFavicon, Brushes.White);

            Closed += (sender, args) => Application.Current.Shutdown();
        }

        private void IconSource_OnFilter(object sender, FilterEventArgs e)
        {
            var icon = e.Item as IconDescription;

            if (icon == null) return;

            e.Accepted = icon.Description.ToLower().Contains(FilterText.Text.ToLower());
        }

        private void FilterText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(FaListView.ItemsSource).Refresh();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var single = new Single();
            single.Show();
        }

        private void ButtonBase_OnClickAnimated(object sender, RoutedEventArgs e)
        {
            var singleRotating = new SingleAnimation();
            singleRotating.Show();
        }


        private void ButtonBase_OnClickSized(object sender, RoutedEventArgs e)
        {
            new SizedWindow().Show();
        }
    }
}
