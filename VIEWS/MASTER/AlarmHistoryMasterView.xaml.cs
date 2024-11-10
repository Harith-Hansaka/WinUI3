using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class AlarmHistoryMasterView : Page
    {
        public AlarmHistoryMasterViewModel ViewModel { get; }
        public AlarmHistoryMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).alarmHistoryMasterViewModel;
        }
    }
}
