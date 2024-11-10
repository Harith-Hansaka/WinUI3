using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.MODELS.MASTER;

namespace UNDAI.VIEWMODELS.MASTER;

public class SystemResetSettingMasterViewModel : ViewModelBase
{
    public SystemResetSettingMasterModel _systemResetSettingMasterModel;
    public ConnectionMaster _connectionMaster;
    private readonly NavigationService _navigationService;
    bool defaultBtnPressed = false;
    App app;

    public ICommand SystemSettingReset { get; }

    public ICommand mainPageNavigateCommand;
    public ICommand MainPageNavigateCommand
    {
        get
        {
            defaultBtnPressed = false;
            OnPropertyChanged(nameof(ResetUndaiImg));
            return mainPageNavigateCommand;
        }
    }

    public SystemResetSettingMasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        _connectionMaster = connectionMaster;
        _systemResetSettingMasterModel = new SystemResetSettingMasterModel(this);
        SystemSettingReset = new RelayCommand(() => _systemResetSettingMasterModel.ButtonClick(1));
        app = (App)Application.Current;
    }

    private string currentIPAddress;
    public string CurrentIPAddress
    {
        get
        {
            currentIPAddress = _connectionMaster.currentServerIP;
            return currentIPAddress;
        }
        set
        {
            currentIPAddress = value;
            OnPropertyChanged(nameof(CurrentIPAddress));
            defaultBtnPressed = true;
            OnPropertyChanged(nameof(ResetUndaiImg));

            _ = ShowImg();
            
        }
    }

    private async Task ShowImg()
    {
        ContentDialog dialog = new ContentDialog()
        {
            Title = "RESET UNDAI",
            Content = "RESET UNDAI AS WELL!",
            PrimaryButtonText = "OK",  // Only primary button
            DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
            XamlRoot = app.xamlRoot
        };

        ContentDialogResult result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        {
        }
    }

    private string defaultIPAddress;
    public string DefaultIPAddress
    {
        get
        {
            defaultIPAddress = _connectionMaster.defaultServerIP;
            return defaultIPAddress;
        }
    }

    private string resetUndaiImg = "Hidden";
    public string ResetUndaiImg
    {
        get
        {
            if (defaultBtnPressed)
            {
                resetUndaiImg = "Visible";
            }
            else
            {
                resetUndaiImg = "Hidden";
            }
            return resetUndaiImg;
        }
        set
        {
            if (defaultBtnPressed)
            {
                resetUndaiImg = "Visible";
            }
            else
            {
                resetUndaiImg = "Hidden";
            }
            OnPropertyChanged(nameof(ResetUndaiImg));
        }
    }
}
