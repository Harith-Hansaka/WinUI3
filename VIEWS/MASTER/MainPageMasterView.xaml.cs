using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class MainPageMasterView : Page
    {
        public MainPageMasterViewModel ViewModel { get; }
        public MainPageMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).mainPageMasterViewModel;
        }
    }
}
