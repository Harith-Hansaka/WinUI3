using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SubstationMasterView : Page
    {
        public SubstationMasterViewModel ViewModel { get; }
        public SubstationMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).substationMasterViewModel;
        }
    }
}
