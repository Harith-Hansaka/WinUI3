using Microsoft.UI.Xaml.Controls;

namespace UNDAI.SERVICES
{
    public interface INavigationService
    {
        void Initialize(Frame frame);
        bool NavigateTo(string pageKey);
        bool GoBack();
        bool CanGoBack { get; }
    }
}
