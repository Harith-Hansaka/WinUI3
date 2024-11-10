using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class MainPageSlaveView : Page
    {
        public MainPageSlaveViewModel ViewModel { get; }
        public MainPageSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).mainPageSlaveViewModel;
        }
    }
}
