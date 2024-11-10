using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class BaseStationRegSlaveView : Page
    {
        public BaseStationRegSlaveViewModel ViewModel { get; }
        public BaseStationRegSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).baseStationRegSlaveViewModel;
        }
    }
}
