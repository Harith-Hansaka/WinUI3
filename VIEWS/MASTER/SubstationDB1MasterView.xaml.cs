using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SubstationDB1MasterView : Page
    {
        public SubstationDB1MasterViewModel ViewModel { get; }
        public SubstationDB1MasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).substationDB1MasterViewModel;
        }
    }
}
