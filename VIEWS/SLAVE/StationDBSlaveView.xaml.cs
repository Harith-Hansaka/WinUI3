using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class StationDBSlaveView : Page
    {
        public StationDBSlaveViewModel ViewModel { get; }
        public StationDBSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).stationDBSlaveViewModel;
        }
    }
}
