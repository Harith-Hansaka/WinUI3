using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SubstationDB3MasterView : Page
    {
        public SubstationDB3MasterViewModel ViewModel { get; }
        public SubstationDB3MasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).substationDB3MasterViewModel;
        }
    }
}
