using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SelfRegMasterView : Page
    {
        public SelfRegMasterViewModel ViewModel { get; }
        public SelfRegMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).selfRegMasterViewModel;
        }

        private void LatitudeMaster_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void InstallationOrientationMaster_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void ElevationMaster_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void LongitudeMaster_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PoleHeightMaster_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }
    }
}
