using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class StationDBMasterView : Page
    {
        public StationDBMasterViewModel ViewModel { get; }
        public StationDBMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).stationDBMasterViewModel;
        }
    }
}
