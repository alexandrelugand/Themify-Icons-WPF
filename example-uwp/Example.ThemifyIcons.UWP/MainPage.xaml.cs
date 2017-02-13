using Windows.UI.Xaml.Controls;
using Example.ThemifyIcons.UWP.ViewModel;

namespace Example.ThemifyIcons.UWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel => (MainViewModel) DataContext;

        private void FilterText_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.Filter(FilterText.Text);
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 0)
                return;

            ViewModel.CurrentIcon = (IconDescription)e.AddedItems[0];
        }
    }
}
