using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class SelfRegSlaveView : Page
    {
        public SelfRegSlaveViewModel ViewModel { get; }
        public SelfRegSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).selfRegSlaveViewModel;
        }
    }
}
