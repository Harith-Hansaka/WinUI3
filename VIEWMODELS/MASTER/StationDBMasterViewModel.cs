using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.MASTER;

public class StationDBMasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;

    StationDBPageModelMaster newStationDBPageModelMaster;
    MainPageMasterViewModel mainPageMasterViewModel;
    SelfRegMasterViewModel selfRegMasterViewModel;
    int stationDBPageModelMasterCount = 0;
    App app;
    string _message;

    public ICommand NavigateToMainPageMasterCommand { get; }
    public ICommand mainPageMasterNavigateCommand { get; }
    public ICommand stationDBPageMasterNavigateCommand { get; }
    public ICommand DBExportCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand EditDataGridCommand { get; }
    public ICommand RegistrationDataGridCommand { get; }

    public StationDBMasterViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        mainPageMasterNavigateCommand = new NavigateCommand(_navigationService, "MainPageMasterView");
        stationDBPageMasterNavigateCommand = new NavigateCommand(_navigationService, "StationDBMasterView");
        StationDBPageModelMaster = new ObservableCollection<StationDBPageModelMaster>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(() => ExportStationDBPageModelMasterToCsvAsync(stationDBPageModelMaster));
        EditDataGridCommand = new AsyncRelayCommand(EditedDataGridSendAsync);
        RegistrationDataGridCommand = new AsyncRelayCommand(RegistrationDataGridSendAsync);
        app = (App)Application.Current;
        CreateAppClassAfterDelay();
        for (int i = 0; i < 15; i++)
        {
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", "", ""
            );
        }
    }

    private async void CreateAppClassAfterDelay()
    {
        await Task.Delay(50);
        if (mainPageMasterViewModel == null || selfRegMasterViewModel == null)
        {
            // Access properties directly
            mainPageMasterViewModel = app.mainPageMasterViewModel;
            selfRegMasterViewModel = app.selfRegMasterViewModel;
        }
    }

    private ObservableCollection<StationDBPageModelMaster> stationDBPageModelMaster;
    public ObservableCollection<StationDBPageModelMaster> StationDBPageModelMaster
    {
        get { return stationDBPageModelMaster; }
        set
        {
            stationDBPageModelMaster = value;
            OnPropertyChanged(nameof(StationDBPageModelMaster));
        }
    }

    private StationDBPageModelMaster _selectedStationDBPageModelMaster;
    public StationDBPageModelMaster SelectedStationDBPageModelMaster
    {
        get { return _selectedStationDBPageModelMaster; }
        set
        {
            _selectedStationDBPageModelMaster = value;
            if (SelectedStationDBPageModelMaster != null)
            {
                SelectedMasterName = SelectedStationDBPageModelMaster.Name;
                SelectedLatitudeMaster = SelectedStationDBPageModelMaster.Latitude;
                SelectedLongitudeMaster = SelectedStationDBPageModelMaster.Longitude;
                SelectedElevationMaster = SelectedStationDBPageModelMaster.Elevation;
                SelectedPoleHeight = SelectedStationDBPageModelMaster.PoleLength;
                SelectedInstallationOrientation = SelectedStationDBPageModelMaster.InstallationOrientation;
                SelectedHeadIPAddress = SelectedStationDBPageModelMaster.MasterIPAddress;
                selectedMasterAntennaIPAddress = SelectedStationDBPageModelMaster.MasterAntennaIPAddress;
            }
            OnPropertyChanged(nameof(SelectedStationDBPageModelMaster));
        }
    }

    private bool isLatLongEnabled;
    public bool IsLatLongEnabled
    {
        get => isLatLongEnabled;
        set
        {
            isLatLongEnabled = value;
            LatLongEnable = value;
            if (!isLatLongEnabled)
            {
                LatitudeMaster = mainPageMasterViewModel.Latitude102;
                LongitudeMaster = mainPageMasterViewModel.Longitude103;
            }
            OnPropertyChanged(nameof(IsLatLongEnabled));
            OnPropertyChanged(nameof(LatLongInabilityShow));
        }
    }

    private string latLongInabilityShow;
    public string LatLongInabilityShow
    {
        get => IsLatLongEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private bool isElevationEnabled;
    public bool IsElevationEnabled
    {
        get => isElevationEnabled;
        set
        {
            isElevationEnabled = value;
            ElevationEnable = value;
            if (!isElevationEnabled)
            {
                ElevationMaster = mainPageMasterViewModel.Elevation104;
            }
            OnPropertyChanged(nameof(IsElevationEnabled));
            OnPropertyChanged(nameof(ElevationInabilityShow));
        }
    }

    private bool latLongEnable;
    public bool LatLongEnable
    {
        get => latLongEnable;
        set
        {
            latLongEnable = value;
            OnPropertyChanged(nameof(LatLongEnable));
        }
    }

    private bool elevationEnable;
    public bool ElevationEnable
    {
        get => elevationEnable;
        set
        {
            elevationEnable = value;
            OnPropertyChanged(nameof(ElevationEnable));
        }
    }

    private string latitudeMaster = "";
    public string LatitudeMaster
    {
        get
        {
            return latitudeMaster;
        }

        set
        {
            latitudeMaster = value;
            OnPropertyChanged(nameof(LatitudeMaster));
        }
    }

    private string longitudeMaster = "";
    public string LongitudeMaster
    {
        get
        {
            return longitudeMaster;
        }
        set
        {
            longitudeMaster = value;
            OnPropertyChanged(nameof(LongitudeMaster));
        }
    }

    private string elevationMaster = "";
    public string ElevationMaster
    {
        get
        {
            return elevationMaster;
        }
        set
        {
            elevationMaster = value;
            OnPropertyChanged(nameof(ElevationMaster));
        }
    }

    private string masterName = "";
    public string MasterName
    {
        get
        {
            return masterName;
        }
        set
        {
            masterName = value;
            OnPropertyChanged(nameof(MasterName));
        }
    }

    private string poleHeight = "";
    public string PoleHeight
    {
        get
        {
            return poleHeight;
        }
        set
        {
            poleHeight = value;
            OnPropertyChanged(nameof(PoleHeight));
        }
    }

    private string installationOrientation = "";
    public string InstallationOrientation
    {
        get
        {
            return installationOrientation;
        }
        set
        {
            installationOrientation = value;
            OnPropertyChanged(nameof(InstallationOrientation));
        }
    }

    private string headIPAddress = "";
    public string HeadIPAddress
    {
        get
        {
            if (mainPageMasterViewModel == null || selfRegMasterViewModel == null)
            {
                // Access properties directly
                mainPageMasterViewModel = app.mainPageMasterViewModel;
                selfRegMasterViewModel = app.selfRegMasterViewModel;
            }
            headIPAddress = mainPageMasterViewModel._systemSettingMasterViewModel.UndaiIPAddress;
            return headIPAddress;
        }
        set
        {
            headIPAddress = value;
            OnPropertyChanged(nameof(HeadIPAddress));
        }
    }

    private string masterAntennaIPAddress = "";
    public string MasterAntennaIPAddress
    {
        get
        {
            if (mainPageMasterViewModel == null || selfRegMasterViewModel == null)
            {
                // Access properties directly
                mainPageMasterViewModel = app.mainPageMasterViewModel;
                selfRegMasterViewModel = app.selfRegMasterViewModel;
            }
            masterAntennaIPAddress = mainPageMasterViewModel._systemSettingMasterViewModel.MasterAntennaIPAddress;
            return masterAntennaIPAddress;
        }
        set
        {
            masterAntennaIPAddress = value;
            OnPropertyChanged(nameof(MasterAntennaIPAddress));
        }
    }

    private string selectedMasterName = "";
    public string SelectedMasterName
    {
        get
        {
            return selectedMasterName;
        }
        set
        {
            selectedMasterName = value;
            OnPropertyChanged(nameof(SelectedMasterName));
        }
    }

    private string selectedElevationMaster = "";
    public string SelectedElevationMaster
    {
        get
        {
            return selectedElevationMaster;
        }
        set
        {
            selectedElevationMaster = value;
            OnPropertyChanged(nameof(SelectedElevationMaster));
        }
    }

    private string selectedLongitudeMaster = "";
    public string SelectedLongitudeMaster
    {
        get
        {
            return selectedLongitudeMaster;
        }
        set
        {
            selectedLongitudeMaster = value;
            OnPropertyChanged(nameof(SelectedLongitudeMaster));
        }
    }

    private string selectedLatitudeMaster = "";
    public string SelectedLatitudeMaster
    {
        get
        {
            return selectedLatitudeMaster;
        }
        set
        {
            selectedLatitudeMaster = value;
            OnPropertyChanged(nameof(SelectedLatitudeMaster));
        }
    }

    private string selectedPoleHeight = "";
    public string SelectedPoleHeight
    {
        get
        {
            return selectedPoleHeight;
        }
        set
        {
            selectedPoleHeight = value;
            OnPropertyChanged(nameof(SelectedPoleHeight));
        }
    }

    private string selectedInstallationOrientation = "";
    public string SelectedInstallationOrientation
    {
        get
        {
            return selectedInstallationOrientation;
        }
        set
        {
            selectedInstallationOrientation = value;
            OnPropertyChanged(nameof(SelectedInstallationOrientation));
        }
    }

    private string selectedHeadIPAddress = "";
    public string SelectedHeadIPAddress
    {
        get
        {
            return selectedHeadIPAddress;
        }
        set
        {
            selectedHeadIPAddress = value;
            OnPropertyChanged(nameof(SelectedHeadIPAddress));
        }
    }

    private string selectedMasterAntennaIPAddress = "";
    public string SelectedMasterAntennaIPAddress
    {
        get
        {
            return selectedMasterAntennaIPAddress;
        }
        set
        {
            selectedMasterAntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedMasterAntennaIPAddress));
        }
    }

    public bool IsIPAddressShow
    {
        get
        {
            return mainPageMasterViewModel.IsIPAddressShow;
        }
        set
        {
            mainPageMasterViewModel.IsIPAddressShow = value;
            OnPropertyChanged(nameof(IsIPAddressShow));
        }
    }

    public void AddStationDBPage
    (
        string Name,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string InstallationOrientation,
        string MasterIPAddress,
        string MasterAntennaIPAddress,
        string Date,
        String Time
    )
    {
        if (stationDBPageModelMasterCount < 15 || stationDBPageModelMaster.Count < 15)
        {
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                InstallationOrientation == "" &
                MasterIPAddress == "" &
                MasterAntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                newStationDBPageModelMaster = new StationDBPageModelMaster
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    InstallationOrientation = InstallationOrientation,
                    MasterIPAddress = MasterIPAddress,
                    MasterAntennaIPAddress = MasterAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                stationDBPageModelMasterCount++;
                StationDBPageModelMaster.Add(newStationDBPageModelMaster);
            }
            else
            {
                newStationDBPageModelMaster = new StationDBPageModelMaster
                {
                    ID = stationDBPageModelMasterCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    InstallationOrientation = InstallationOrientation,
                    MasterIPAddress = MasterIPAddress,
                    MasterAntennaIPAddress = MasterAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                stationDBPageModelMasterCount++;
                StationDBPageModelMaster.Add(newStationDBPageModelMaster);
            }
        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = stationDBPageModelMasterCount % 15;

            StationDBPageModelMaster[indexToUpdate] = new StationDBPageModelMaster
            {
                ID = indexToUpdate + 1,  // Update ID to match the new entry position
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Elevation = Elevation,
                PoleLength = PoleLength,
                InstallationOrientation = InstallationOrientation,
                MasterIPAddress = MasterIPAddress,
                MasterAntennaIPAddress = MasterAntennaIPAddress,
                Date = Date,
                Time = Time
            };

            stationDBPageModelMasterCount++;
        }
    }

    public void DeleteSelectedItem()
    {
        if (SelectedStationDBPageModelMaster != null)
        {
            StationDBPageModelMaster.Remove(SelectedStationDBPageModelMaster);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", "", ""
            );
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedStationDBPageModelMaster != null;
    }

    public async Task ExportStationDBPageModelMasterToCsvAsync(ObservableCollection<StationDBPageModelMaster> stationDBPageModelMasters)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "MasterStationDBExport.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of StationDBPageModelMaster
        sb.AppendLine
            (
                "ID," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "InstallationOrientation," +
                "MasterIPAddress," +
                "MasterAntennaIPAddress," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in stationDBPageModelMasters)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Name}," +
                    $"{station.Latitude}," +
                    $"{station.Longitude}," +
                    $"{station.Elevation}," +
                    $"{station.PoleLength}," +
                    $"{station.InstallationOrientation}," +
                    $"{station.MasterIPAddress}," +
                    $"{station.MasterAntennaIPAddress}," +
                    $"{station.Date}," +
                    $"{station.Time}"
                );
        }

        // Write the CSV content to a file
        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);

        ContentDialog dialog = new ContentDialog()
        {
            Title = "EXPORT SUCCESSFUL",
            Content = "Database export successful",
            PrimaryButtonText = "OK",  // Only primary button
            DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
            XamlRoot = app.xamlRoot
        };

        ContentDialogResult result = await dialog.ShowAsync();

        if (result == ContentDialogResult.Primary)
        { 
        }
    }

    public async Task EditedDataGridSendAsync()
    {
        if (mainPageMasterViewModel == null || selfRegMasterViewModel == null)
        {
            // Access properties directly
            mainPageMasterViewModel = app.mainPageMasterViewModel;
            selfRegMasterViewModel = app.selfRegMasterViewModel;
        }
        _message =
            "DM," +
            SelectedMasterName +
            "," +
            SelectedLatitudeMaster +
            "," +
            SelectedLongitudeMaster +
            "," +
            SelectedElevationMaster +
            "," +
            SelectedPoleHeight +
            "," +
            SelectedInstallationOrientation +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedMasterName == "" || SelectedMasterName == null ||
            SelectedLatitudeMaster == "" || SelectedLatitudeMaster == null ||
            SelectedLongitudeMaster == "" || SelectedLongitudeMaster == null ||
            SelectedElevationMaster == "" || SelectedElevationMaster == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedInstallationOrientation == "" || SelectedInstallationOrientation == null ||
            SelectedHeadIPAddress == "" || SelectedHeadIPAddress == null ||
            SelectedMasterAntennaIPAddress == "" || SelectedMasterAntennaIPAddress == null
        )
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Not Complete",
                Content = "Please reenter, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else if (!MainPageMasterModel.IsValidIPAddress(SelectedHeadIPAddress))
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "INVALID UNDAI IPADDRESS",
                Content = "Please reenter, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else if (!MainPageMasterModel.IsValidIPAddress(SelectedMasterAntennaIPAddress))
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "INVALID ANTENNA IPADDRESS",
                Content = "Please reenter, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else
        {
            mainPageMasterViewModel._mainPageMasterModel.MessageSend(_message);
        }
        //mainPageMasterNavigateCommand.Execute(null);

    }

    public async Task RegistrationDataGridSendAsync()
    {
        _message =
            "DM," +
            MasterName +
            "," +
            LatitudeMaster +
            "," +
            LongitudeMaster +
            "," +
            ElevationMaster +
            "," +
            PoleHeight +
            "," +
            InstallationOrientation +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            LatitudeMaster == "" || LatitudeMaster == null ||
            LongitudeMaster == "" || LongitudeMaster == null ||
            ElevationMaster == "" || ElevationMaster == null ||
            PoleHeight == "" || PoleHeight == null ||
            InstallationOrientation == "" || InstallationOrientation == null ||
            HeadIPAddress == "" || HeadIPAddress == null ||
            MasterAntennaIPAddress == "" || MasterAntennaIPAddress == null
        )
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "Not Complete",
                Content = "Please fill all, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else if
        (!MainPageMasterModel.IsValidIPAddress(HeadIPAddress))
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "INVALID UNDAI IPADDRESS",
                Content = "Please reenter, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else if
        (!MainPageMasterModel.IsValidIPAddress(MasterAntennaIPAddress))
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "INVALID ANTENNA IPADDRESS",
                Content = "Please reenter, try again?",
                PrimaryButtonText = "OK",  // Only primary button
                DefaultButton = ContentDialogButton.Primary,  // Optional: makes Enter key trigger OK
                XamlRoot = app.xamlRoot
            };

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
            }
        }
        else
        {
            mainPageMasterViewModel._mainPageMasterModel.MessageSend(_message);
        }
    }
}
