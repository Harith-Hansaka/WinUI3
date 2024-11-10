using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.SLAVE;
using UNDAI.SERVICES;
using CommunityToolkit.Mvvm.Input;
using UNDAI.MODELS.MASTER;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace UNDAI.VIEWMODELS.SLAVE;

public class SystemResetSettingSlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    public SystemResetSettingSlaveModel _systemResetSettingSlaveModel;
    public ConnectionSlave _connectionSlave;
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

    public SystemResetSettingSlaveViewModel
    (
        NavigationService navigationService,
        ConnectionSlave connectionSlave
    )
    {
        _navigationService = navigationService;
        _connectionSlave = connectionSlave;
        _systemResetSettingSlaveModel = new SystemResetSettingSlaveModel(this);
        SystemSettingReset = new RelayCommand(() => _systemResetSettingSlaveModel.ButtonClick(1));
        app = (App)Application.Current;
    }

    private string currentIPAddress;
    public string CurrentIPAddress
    {
        get
        {
            currentIPAddress = _connectionSlave.currentServerIP;
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
            defaultIPAddress = _connectionSlave.defaultServerIP;
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
