using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.MODELS.SLAVE;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace UNDAI.VIEWMODELS.SLAVE;

public class StationDBSlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    StationDBModelSlave newStationDBPageModelSlave;
    MainPageSlaveViewModel mainPageSlaveViewModel;
    SelfRegSlaveViewModel selfRegSlaveViewModel;
    int stationDBModelSlaveCount = 0;
    App app;
    string _message;

    public ICommand mainPageSlaveNavigateCommand { get; }
    public ICommand stationDBSlaveNavigateCommand { get; }
    public ICommand DBExportCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand EditDataGridCommand { get; }
    public ICommand RegistrationDataGridCommand { get; }

    public StationDBSlaveViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        mainPageSlaveNavigateCommand = new NavigateCommand(_navigationService, "MainPageSlaveView");
        stationDBSlaveNavigateCommand = new NavigateCommand(_navigationService, "StationDBSlaveView");
        StationDBModelSlave = new ObservableCollection<StationDBModelSlave>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(() => ExportStationDBPageModelSlaveToCsvAsync(stationDBModelSlave));
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
        if (mainPageSlaveViewModel == null || selfRegSlaveViewModel == null)
        {
            // Access properties directly
            mainPageSlaveViewModel = app.mainPageSlaveViewModel;
            selfRegSlaveViewModel = app.selfRegSlaveViewModel;
        }
    }

    private ObservableCollection<StationDBModelSlave> stationDBModelSlave;
    public ObservableCollection<StationDBModelSlave> StationDBModelSlave
    {
        get { return stationDBModelSlave; }
        set
        {
            stationDBModelSlave = value;
            OnPropertyChanged(nameof(StationDBModelSlave));
        }
    }

    private StationDBModelSlave _selectedStationDBPageModelSlave;
    public StationDBModelSlave SelectedStationDBPageModelSlave
    {
        get { return _selectedStationDBPageModelSlave; }
        set
        {
            _selectedStationDBPageModelSlave = value;
            if (SelectedStationDBPageModelSlave != null)
            {
                SelectedSlaveName = SelectedStationDBPageModelSlave.Name;
                SelectedLatitudeSlave = SelectedStationDBPageModelSlave.Latitude;
                SelectedLongitudeSlave = SelectedStationDBPageModelSlave.Longitude;
                SelectedElevationSlave = SelectedStationDBPageModelSlave.Elevation;
                SelectedPoleHeight = SelectedStationDBPageModelSlave.PoleLength;
                SelectedInstallationOrientation = SelectedStationDBPageModelSlave.InstallationOrientation;
                SelectedHeadIPAddress = SelectedStationDBPageModelSlave.SlaveIPAddress;
                SelectedSlaveAntennaIPAddress = SelectedStationDBPageModelSlave.SlaveAntennaIPAddress;
            }
            OnPropertyChanged(nameof(SelectedStationDBPageModelSlave));
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
                LatitudeSlave = mainPageSlaveViewModel.Latitude102;
                LongitudeSlave = mainPageSlaveViewModel.Longitude103;
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
                ElevationSlave = mainPageSlaveViewModel.Elevation104;
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

    private string latitudeSlave = "";
    public string LatitudeSlave
    {
        get
        {
            return latitudeSlave;
        }

        set
        {
            latitudeSlave = value;
            OnPropertyChanged(nameof(LatitudeSlave));
        }
    }

    private string longitudeSlave = "";
    public string LongitudeSlave
    {
        get
        {
            return longitudeSlave;
        }
        set
        {
            longitudeSlave = value;
            OnPropertyChanged(nameof(LongitudeSlave));
        }
    }

    private string elevationSlave = "";
    public string ElevationSlave
    {
        get
        {
            return elevationSlave;
        }
        set
        {
            elevationSlave = value;
            OnPropertyChanged(nameof(ElevationSlave));
        }
    }

    private string slaveName = "";
    public string SlaveName
    {
        get
        {
            return slaveName;
        }
        set
        {
            slaveName = value;
            OnPropertyChanged(nameof(SlaveName));
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
            if (headIPAddress == null || headIPAddress == null)
            {
                // Access properties directly
                mainPageSlaveViewModel = app.mainPageSlaveViewModel;
                selfRegSlaveViewModel = app.selfRegSlaveViewModel;
            }
            headIPAddress = mainPageSlaveViewModel._systemSettingSlaveViewModel.UndaiIPAddress;
            return headIPAddress;
        }
        set
        {
            headIPAddress = value;
            OnPropertyChanged(nameof(HeadIPAddress));
        }
    }

    private string slaveAntennaIPAddress = "";
    public string SlaveAntennaIPAddress
    {
        get
        {
            if (slaveAntennaIPAddress == null || slaveAntennaIPAddress == null)
            {
                // Access properties directly
                mainPageSlaveViewModel = app.mainPageSlaveViewModel;
                selfRegSlaveViewModel = app.selfRegSlaveViewModel;
            }
            slaveAntennaIPAddress = mainPageSlaveViewModel._systemSettingSlaveViewModel.SlaveAntennaIPAddress;
            return slaveAntennaIPAddress;
        }
        set
        {
            slaveAntennaIPAddress = value;
            OnPropertyChanged(nameof(SlaveAntennaIPAddress));
        }
    }

    private string selectedSlaveName = "";
    public string SelectedSlaveName
    {
        get
        {
            return selectedSlaveName;
        }
        set
        {
            selectedSlaveName = value;
            OnPropertyChanged(nameof(SelectedSlaveName));
        }
    }

    private string selectedElevationSlave = "";
    public string SelectedElevationSlave
    {
        get
        {
            return selectedElevationSlave;
        }
        set
        {
            selectedElevationSlave = value;
            OnPropertyChanged(nameof(SelectedElevationSlave));
        }
    }

    private string selectedLongitudeSlave = "";
    public string SelectedLongitudeSlave
    {
        get
        {
            return selectedLongitudeSlave;
        }
        set
        {
            selectedLongitudeSlave = value;
            OnPropertyChanged(nameof(SelectedLongitudeSlave));
        }
    }

    private string selectedLatitudeSlave = "";
    public string SelectedLatitudeSlave
    {
        get
        {
            return selectedLatitudeSlave;
        }
        set
        {
            selectedLatitudeSlave = value;
            OnPropertyChanged(nameof(SelectedLatitudeSlave));
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

    private string selectedSlaveAntennaIPAddress = "";
    public string SelectedSlaveAntennaIPAddress
    {
        get
        {
            return selectedSlaveAntennaIPAddress;
        }
        set
        {
            selectedSlaveAntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedSlaveAntennaIPAddress));
        }
    }

    public bool IsIPAddressShow
    {
        get
        {
            return mainPageSlaveViewModel.IsSlaveIPAddressShow;
        }
        set
        {
            mainPageSlaveViewModel.IsSlaveIPAddressShow = value;
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
        string SlaveIPAddress,
        string SlaveAntennaIPAddress,
        string Date,
        string Time
    )
    {
        if (stationDBModelSlaveCount < 15 || stationDBModelSlave.Count < 15)
        {
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                InstallationOrientation == "" &
                SlaveIPAddress == "" &
                SlaveAntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                newStationDBPageModelSlave = new StationDBModelSlave
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    InstallationOrientation = InstallationOrientation,
                    SlaveIPAddress = SlaveIPAddress,
                    SlaveAntennaIPAddress = SlaveAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                stationDBModelSlaveCount++;
                StationDBModelSlave.Add(newStationDBPageModelSlave);
            }
            else
            {
                newStationDBPageModelSlave = new StationDBModelSlave
                {
                    ID = stationDBModelSlaveCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    InstallationOrientation = InstallationOrientation,
                    SlaveIPAddress = SlaveIPAddress,
                    SlaveAntennaIPAddress = SlaveAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                stationDBModelSlaveCount++;
                StationDBModelSlave.Add(newStationDBPageModelSlave);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = stationDBModelSlaveCount % 15;

            StationDBModelSlave[indexToUpdate] = new StationDBModelSlave
            {
                ID = indexToUpdate + 1,  // Update ID to match the new entry position
                Name = Name,
                Latitude = Latitude,
                Longitude = Longitude,
                Elevation = Elevation,
                PoleLength = PoleLength,
                InstallationOrientation = InstallationOrientation,
                SlaveIPAddress = SlaveIPAddress,
                SlaveAntennaIPAddress = SlaveAntennaIPAddress,
                Date = Date,
                Time = Time
            };

            stationDBModelSlaveCount++;
        }
    }

    public void DeleteSelectedItem()
    {
        if (SelectedStationDBPageModelSlave != null)
        {
            StationDBModelSlave.Remove(newStationDBPageModelSlave);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", "", ""
            );
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedStationDBPageModelSlave != null;
    }

    public async Task ExportStationDBPageModelSlaveToCsvAsync(ObservableCollection<StationDBModelSlave> stationDBPageModelSlave)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "SlaveStationDBExport.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of StationDBPageModelSlave
        sb.AppendLine
            (
                "ID," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "InstallationOrientation," +
                "SlaveIPAddress," +
                "SlaveAntennaIPAddress" +
                "Date" +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in stationDBPageModelSlave)
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
                    $"{station.SlaveIPAddress}," +
                    $"{station.SlaveAntennaIPAddress}" +
                    $"{station.Date}" +
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
        if (mainPageSlaveViewModel == null)
        {
            // Access properties directly
            mainPageSlaveViewModel = app.mainPageSlaveViewModel;
        }
        _message =
            "DM," +
            SelectedSlaveName +
            "," +
            SelectedLatitudeSlave +
            "," +
            SelectedLongitudeSlave +
            "," +
            SelectedElevationSlave +
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
            SelectedSlaveName == "" || SelectedSlaveName == null ||
            SelectedLatitudeSlave == "" || SelectedLatitudeSlave == null ||
            SelectedLongitudeSlave == "" || SelectedLongitudeSlave == null ||
            SelectedElevationSlave == "" || SelectedElevationSlave == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedInstallationOrientation == "" || SelectedInstallationOrientation == null
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
        else
        {
            mainPageSlaveViewModel.mainPageSlaveModel.MessageSend(_message);
        }

    }

    public async Task RegistrationDataGridSendAsync()
    {
        _message =
            "DM," +
            SlaveName +
            "," +
            LatitudeSlave +
            "," +
            LongitudeSlave +
            "," +
            ElevationSlave +
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
            SlaveName == "" || SlaveName == null ||
            LatitudeSlave == "" || LatitudeSlave == null ||
            LongitudeSlave == "" || LongitudeSlave == null ||
            ElevationSlave == "" || ElevationSlave == null ||
            PoleHeight == "" || PoleHeight == null ||
            InstallationOrientation == "" || InstallationOrientation == null ||
            HeadIPAddress == "" || HeadIPAddress == null
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
        (!MainPageMasterModel.IsValidIPAddress(SlaveAntennaIPAddress))
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
            mainPageSlaveViewModel.mainPageSlaveModel.MessageSend(_message);
        }
    }
}
