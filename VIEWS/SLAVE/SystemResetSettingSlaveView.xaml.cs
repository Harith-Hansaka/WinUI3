using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class SystemResetSettingSlaveView : Page
    {
        public SystemResetSettingSlaveViewModel ViewModel { get; }
        public SystemResetSettingSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).systemResetSettingSlaveViewModel;
        }
    }
}
