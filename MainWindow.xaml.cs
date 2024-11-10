using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using UNDAI.SERVICES;

namespace UNDAI
{
    public sealed partial class MainWindow : Window
    {
        App app;
        private readonly INavigationService _navigationService;
        public MainWindow()
        {
            app = (App)Application.Current;
            this.InitializeComponent();
            Title = "UNDAI";
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(TitleBar);

            // Set window size
            AppWindow.Resize(new Windows.Graphics.SizeInt32(1920, 1200));
            
            _navigationService = ((App)Application.Current).NavigationService;
            _navigationService.Initialize(RootFrame);
            _navigationService.NavigateTo("MainPageMasterView");
        }

        private async Task MainWindow_ClosingAsync(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (app != null)
            {
                if (app.mainPageMasterViewModel.Connected != "CONNECT")
                {
                    if (app.mainPageMasterViewModel.HomePeakSelect != 2)
                    {
                        // Show a confirmation message
                        ContentDialog dialog = new ContentDialog()
                        {
                            Content = "UNDAI is not it's origin, Are you sure you want to exit?",
                            Title = "Confirmation",
                            PrimaryButtonText = "Yes",
                            SecondaryButtonText = "No",
                            XamlRoot = this.Content.XamlRoot  // Important for WinUI 3
                        };

                        ContentDialogResult result = await dialog.ShowAsync();

                        // Check the user's choice
                        if (result == ContentDialogResult.Secondary)
                        {
                            // Cancel the closing if the user selects No
                            e.Cancel = true;
                        }
                        else
                        {
                            Application.Current.Exit(); // Ensure the application closes completely
                        }
                    }
                    else
                    {
                        Application.Current.Exit(); // Ensure the application closes completely
                    }
                }
                else if (app.mainPageSlaveViewModel.Connected != "CONNECT")
                {
                    if (app.mainPageSlaveViewModel.HomePeakSelect != 2)
                    {
                        // Show a confirmation message
                        ContentDialog dialog = new ContentDialog() {
                            Content = "UNDAI is not it's origin, Are you sure you want to exit?",
                            Title = "Confirmation",
                            PrimaryButtonText = "Yes",
                            SecondaryButtonText = "No",
                            XamlRoot = this.Content.XamlRoot  // Important for WinUI 3
                        };

                        ContentDialogResult result = await dialog.ShowAsync();

                        // Check the user's choice
                        if (result == ContentDialogResult.Secondary)
                        {
                            // Cancel the closing if the user selects No
                            e.Cancel = true;
                        }
                        else
                        {
                            Application.Current.Exit(); // Ensure the application closes completely
                        }
                    }
                    else
                    {
                        Application.Current.Exit(); // Ensure the application closes completely
                    }
                }
                else
                {
                    Application.Current.Exit(); // Ensure the application closes completely
                }
            }
            else
            {
                Application.Current.Exit(); // Ensure the application closes completely
            }
        }
    }
}
