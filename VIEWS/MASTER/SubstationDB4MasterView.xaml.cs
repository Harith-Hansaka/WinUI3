using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SubstationDB4MasterView : Page
    {
        public SubstationDB4MasterViewModel ViewModel { get; }
        public SubstationDB4MasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).substationDB4MasterViewModel;
        }
    }
}
