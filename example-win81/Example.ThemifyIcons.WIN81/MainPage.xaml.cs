using Windows.UI.Xaml.Controls;
using Example.ThemifyIcons.WIN81.ViewModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Example.ThemifyIcons.WIN81
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        public MainViewModel ViewModel => (MainViewModel)DataContext;

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
