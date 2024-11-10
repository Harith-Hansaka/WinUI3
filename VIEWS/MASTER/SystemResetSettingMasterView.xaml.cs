using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;
namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SystemResetSettingMasterView : Page
    {
        public SystemResetSettingMasterViewModel ViewModel { get; }
        public SystemResetSettingMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).systemResetSettingMasterViewModel;
        }
    }
}
