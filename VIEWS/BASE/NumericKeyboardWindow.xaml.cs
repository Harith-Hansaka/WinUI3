using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using Microsoft.UI.Xaml.Controls;

namespace UNDAI.VIEWS.BASE;

public sealed partial class NumericKeyboardWindow : Window
{
    string _backupString;

    public NumericKeyboardWindow(string backupString)
    {
        this.InitializeComponent();

        // Get the window handle
        var windowHandle = WinRT.Interop.WindowNative.GetWindowHandle(this);

        // Get AppWindow
        Microsoft.UI.WindowId windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(windowHandle);
        var appWindow = Microsoft.UI.Windowing.AppWindow.GetFromWindowId(windowId);

        // Set size and prevent resizing
        var preferences = new Windows.Graphics.SizeInt32(350, 525);
        appWindow.Resize(preferences);

        // Disable maximize button and resizing
        var presenter = appWindow.Presenter as OverlappedPresenter;
        if (presenter != null)
        {
            presenter.IsResizable = false;
            presenter.IsMaximizable = false;
        }

        // Optional: Set initial position (centered on screen)
        var displayArea = Microsoft.UI.Windowing.DisplayArea.Primary;
        var centerX = ((displayArea.WorkArea.Width - 350) / 2);
        var centerY = ((displayArea.WorkArea.Height - 525) / 2);
        appWindow.Move(new Windows.Graphics.PointInt32(centerX, centerY));

        _backupString = backupString;
    }

    private void NumericButton_Click(object sender, RoutedEventArgs e)
    {
        if(sender is Button button)
        {
            KeyPad.Text += button.Content.ToString();
        }
    }

    private void BackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        // Check if KeyPad.Text is not empty
        if (!string.IsNullOrEmpty(KeyPad.Text))
        {
            // Remove the last character
            KeyPad.Text = KeyPad.Text.Remove(KeyPad.Text.Length - 1);
        }
    }

    private void SignChangeButton_Click(object sender, RoutedEventArgs e)
    {
        if (!string.IsNullOrEmpty(KeyPad.Text))
        {
            if (KeyPad.Text[0] == '-')
            {
                // Remove the minus sign
                KeyPad.Text = KeyPad.Text.Substring(1);
            }
            else
            {
                // Add minus sign
                KeyPad.Text = "-" + KeyPad.Text;
            }
        }
    }

    private void EnterButton_Click(object sender, RoutedEventArgs e)
    {
        _backupString = KeyPad.Text;
    }
}