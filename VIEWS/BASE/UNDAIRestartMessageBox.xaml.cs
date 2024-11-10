using Microsoft.UI.Xaml;

namespace UNDAI.VIEWS.BASE;

public sealed partial class UNDAIRestartMessageBox : Window
{
    public bool Result { get; private set; }
    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        Result = true;
        this.Close();
    }
    public UNDAIRestartMessageBox()
    {
        this.InitializeComponent();
        // Get the window handle
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);
        var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);
        // Set size
        appWindow.Resize(new Windows.Graphics.SizeInt32(800, 800));
        // Create dialog presenter
        var presenter = Microsoft.UI.Windowing.OverlappedPresenter.CreateForDialog();
        presenter.IsModal = true;
        presenter.IsResizable = false;
        // Set the presenter
        appWindow.SetPresenter(presenter);
        // Center on screen
        var displayArea = Microsoft.UI.Windowing.DisplayArea.Primary;
        var centerX = ((displayArea.WorkArea.Width - 800) / 2);
        var centerY = ((displayArea.WorkArea.Height - 800) / 2);
        appWindow.Move(new Windows.Graphics.PointInt32(centerX, centerY));
        Title = "RESET UNDAI";
    }
}
