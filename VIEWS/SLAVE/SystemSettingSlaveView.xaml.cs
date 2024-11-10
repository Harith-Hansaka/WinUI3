using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class SystemSettingSlaveView : Page
    {
        public SystemSettingSlaveViewModel ViewModel { get; }
        public SystemSettingSlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).systemSettingSlaveViewModel;
        }
    }
}
