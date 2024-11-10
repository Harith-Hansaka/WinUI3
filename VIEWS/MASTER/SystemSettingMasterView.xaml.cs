using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using UNDAI.VIEWMODELS.MASTER;
using UNDAI.VIEWS.BASE;

namespace UNDAI.VIEWS.MASTER
{
    public sealed partial class SystemSettingMasterView : Page
    {
        NumericKeyboardWindow _numericKeyboardWindow;
        public SystemSettingMasterViewModel ViewModel { get; }
        public SystemSettingMasterView()
        {
            this.InitializeComponent();
            ViewModel = ((App)Application.Current).systemSettingMasterViewModel;
        }

        private void UndaiIPAddress_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            _numericKeyboardWindow = new NumericKeyboardWindow(ViewModel.UndaiIPAddress);
        }

        private void RSSITurnbackThresholdLevel_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void ContinuousOperationTime_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void StepStability_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void DetailedPeakSearchRSSILevel_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void UpDownSearchAngle_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void DetailedPeakSearchStepAngle_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchRSSILevel_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchRSSITurnLevel_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchElevation_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchAzimuth_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchSpeed_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void InchingSpeedSetElevation_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void InchingSpeedSetAzimuth_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void LowSpeedSetElevation_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void LowSpeedSetAzimuth_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void HighSpeedSetElevation_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void HighSpeedSetAzimuth_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void OriginOffsetElevation_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void OriginOffsetAzimuth_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void MasterAntennaIPAddress_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void DefaultGateway_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void UndaiSubnet_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void btnUpArrowMainPage_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnUpArrowMainPageMouseDownCommand.Execute(null);
        }

        private void btnUpArrowMainPage_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnUpArrowMainPageMouseUpCommand.Execute(null);
        }

        private void btnDownArrowMainPage_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnDownArrowMainPageMouseDownCommand.Execute (null);
        }

        private void btnDownArrowMainPage_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnDownArrowMainPageMouseUpCommand.Execute(null);
        }

        private void btnCCWArrowMainPage_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnCCWMainPageMouseDownCommand.Execute(null);
        }

        private void btnCCWArrowMainPage_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnCCWMainPageMouseUpCommand.Execute(null);
        }

        private void btnCWArrowMainPage_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnCWMainPageMouseUpCommand.Execute(null);
        }

        private void btnCWArrowMainPage_PointerReleased(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ViewModel.BtnCWMainPageMouseUpCommand.Execute(null);
        }

        private void SystemUnlockPIN_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }

        private void PeakSearchRSSDelay_PointerPressed(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e)
        {

        }
    }
}
