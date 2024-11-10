using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.SLAVE;

namespace UNDAI.VIEWS.SLAVE
{
    public sealed partial class AlarmHistorySlaveView : Page
    {
        public AlarmHistorySlaveViewModel ViewModel { get; }
        public AlarmHistorySlaveView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).alarmHistorySlaveViewModel;
        }
    }
}
