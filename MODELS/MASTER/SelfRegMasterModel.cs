using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Windows;
using UNDAI.COMMANDS.BASE;
using UNDAI.SERVICES;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.MODELS.MASTER
{
    public class SelfRegMasterModel
    {
        private SelfRegMasterViewModel _selfRegMasterViewModel;
        private MainPageMasterViewModel _mainPageMasterViewModel;
        private ConnectionMaster _connectionMaster;
        private int _commandNo;
        public MainPageMasterModel _mainPageMasterModel;
        string _message;
        public NavigationService _navigationService;
        XamlRoot _xamlRoot;

        public SelfRegMasterModel
        (
            NavigationService navigationService,
            ConnectionMaster connectionMaster,
            SelfRegMasterViewModel selfRegMasterViewModel,
            MainPageMasterViewModel mainPageMasterViewModel,
            XamlRoot xamlRoot

        )

        {
            _selfRegMasterViewModel = selfRegMasterViewModel;
            _connectionMaster = connectionMaster;
            _mainPageMasterViewModel = mainPageMasterViewModel;
            _mainPageMasterModel = _mainPageMasterViewModel._mainPageMasterModel;
            _navigationService = navigationService;
            _xamlRoot = xamlRoot;
        }

        public async void Button_Click()
        {
            _message =
                "DM," +
                _selfRegMasterViewModel.MasterName + "," +
                _selfRegMasterViewModel.LatitudeMaster + "," +
                _selfRegMasterViewModel.LongitudeMaster + "," +
                _selfRegMasterViewModel.ElevationMaster + "," +
                _selfRegMasterViewModel.PoleHeight + "," +
                _selfRegMasterViewModel.InstallationOrientation + "," +
                DateTime.Now.Date.ToString("yyyy-MM-dd") + "," +
                DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") + ",\n";

            NavigateCommand navigateCommand = new NavigateCommand(_navigationService, "MainPageMasterView");

            if
            (
                _selfRegMasterViewModel.MasterName == "" || 
                _selfRegMasterViewModel.PoleHeight == "" ||
                _selfRegMasterViewModel.InstallationOrientation== "" ||
                _selfRegMasterViewModel.HeadIPAddress== "" ||
                _selfRegMasterViewModel.LatitudeMaster == "" ||
                _selfRegMasterViewModel.LongitudeMaster == "" ||
                _selfRegMasterViewModel.ElevationMaster == "" || 
                _selfRegMasterViewModel.MasterAntennaIPAddress == ""
            )
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "Not Complete",
                    Content = "Please fill all, try again?",
                    PrimaryButtonText = "Yes",
                    SecondaryButtonText = "No",
                    XamlRoot = _xamlRoot  // Get XamlRoot from window
                };

                ContentDialogResult result = await dialog.ShowAsync();
                
                if (result == ContentDialogResult.Primary)
                {
                    // Optionally, do something here if the user clicks "Yes"

                }
                else
                {
                    // Optionally, do something here if the user clicks "No"
                    navigateCommand.Execute(null);
                }
            }
            else if
            (!MainPageMasterModel.IsValidIPAddress(_selfRegMasterViewModel.HeadIPAddress))
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "INVALID UNDAI IPADDRESS",
                    Content = "Please reenter, try again?",
                    PrimaryButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = _xamlRoot  // Get XamlRoot from window
                };

                ContentDialogResult result = await dialog.ShowAsync();

                if(result == ContentDialogResult.Primary)
                {
                    // Optionally, do something here if the user clicks "OK"
                }
            }
            else if
            (!MainPageMasterModel.IsValidIPAddress(_selfRegMasterViewModel.MasterAntennaIPAddress))
            {
                ContentDialog dialog = new ContentDialog()
                {
                    Title = "INVALID ANTENNA IPADDRESS",
                    Content = "Please reenter, try again?",
                    PrimaryButtonText = "Ok",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = _xamlRoot  // Get XamlRoot from window
                };

                ContentDialogResult result = await dialog.ShowAsync();

                if (result == ContentDialogResult.Primary)
                {
                    // Optionally, do something here if the user clicks "OK"
                }
            }
            else
            {
                _mainPageMasterModel.MessageSend(_message);
                navigateCommand.Execute(null);
            }
        }
    }
}