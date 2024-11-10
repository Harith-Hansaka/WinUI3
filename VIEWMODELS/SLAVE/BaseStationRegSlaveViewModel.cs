using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.MODELS.SLAVE;

namespace UNDAI.VIEWMODELS.SLAVE;

public class BaseStationRegSlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    BaseStationRegSlaveModel newBaseStationRegSlaveModel;
    MainPageSlaveViewModel _mainPageSlaveViewModel;
    int BaseStationRegSlaveModelCount = 0;
    App app;
    string _message;
    public bool canAdd2DB = false;
    int objCount = 0;

    public ICommand MainPageSlaveNavigateCommand { get; }
    public ICommand SubstationRegistrationSlaveNavigateCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand DBExportCommand { get; }
    public ICommand EditDataGridCommand { get; }
    public ICommand RegistrationDataGridCommand { get; }

    public BaseStationRegSlaveViewModel
    (
        NavigationService navigationService,
        ConnectionSlave connectionSlave
    )
    {
        _navigationService = navigationService;
        BaseStationRegSlaveModel = new ObservableCollection<BaseStationRegSlaveModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new RelayCommand(() => ExportBaseStationRegSlaveModelToCsvAsync(baseStationRegSlaveModel));
        EditDataGridCommand = new AsyncRelayCommand(EditedDataGridSendAsync);
        RegistrationDataGridCommand = new AsyncRelayCommand(RegistrationDataGridSendAsync);
        app = (App)Application.Current;
        CreateAppClassAfterDelay();

        for (int i = 0; i < 15; i++)
        {
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
        }
    }

    private async void CreateAppClassAfterDelay()
    {
        await Task.Delay(50);
        if (_mainPageSlaveViewModel == null)
        {
            // Access properties directly
            _mainPageSlaveViewModel = app.mainPageSlaveViewModel;
        }
    }

    private ObservableCollection<BaseStationRegSlaveModel> baseStationRegSlaveModel;
    public ObservableCollection<BaseStationRegSlaveModel> BaseStationRegSlaveModel
    {
        get { return baseStationRegSlaveModel; }
        set
        {
            baseStationRegSlaveModel = value;
            OnPropertyChanged(nameof(BaseStationRegSlaveModel));
        }
    }

    private BaseStationRegSlaveModel _selectedBaseStationRegSlaveModel;
    public BaseStationRegSlaveModel SelectedBaseStationRegSlaveModel
    {
        get { return _selectedBaseStationRegSlaveModel; }
        set
        {
            _selectedBaseStationRegSlaveModel = value;
            if (SelectedBaseStationRegSlaveModel != null)
            {
                SelectedMasterName = SelectedBaseStationRegSlaveModel.MasterName;
                SelectedLatitudeMaster = SelectedBaseStationRegSlaveModel.Latitude;
                SelectedLongitudeMaster = SelectedBaseStationRegSlaveModel.Longitude;
                SelectedElevationMaster = SelectedBaseStationRegSlaveModel.Elevation;
                SelectedPoleHeight = SelectedBaseStationRegSlaveModel.PoleLength;
                SelectedMasterAntennaName = SelectedBaseStationRegSlaveModel.MasterAntennaName;
                SelectedMasterAntennaIPAddress = SelectedBaseStationRegSlaveModel.MasterAntennaIPAddress;
            }
            OnPropertyChanged(nameof(SelectedBaseStationRegSlaveModel));
        }
    }

    public void AddStationDBPage
    (
        string MasterAntennaIPAddress,
        string MasterAntennaName,
        string MasterName,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string Date,
        string Time
    )
    {
        if (BaseStationRegSlaveModelCount < 15 || baseStationRegSlaveModel.Count < 15)
        {
            if
            (
                MasterName == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                MasterAntennaName == "" &
                MasterAntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                newBaseStationRegSlaveModel = new BaseStationRegSlaveModel
                {
                    ID = null,
                    MasterName = MasterName,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    MasterAntennaName = MasterAntennaName,
                    MasterAntennaIPAddress = MasterAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                BaseStationRegSlaveModelCount++;
                BaseStationRegSlaveModel.Add(newBaseStationRegSlaveModel);
            }
            else
            {
                newBaseStationRegSlaveModel = new BaseStationRegSlaveModel
                {
                    ID = BaseStationRegSlaveModelCount + 1,
                    MasterName = MasterName,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    MasterAntennaName = MasterAntennaName,
                    MasterAntennaIPAddress = MasterAntennaIPAddress,
                    Date = Date,
                    Time = Time
                };

                BaseStationRegSlaveModelCount++;
                BaseStationRegSlaveModel.Add(newBaseStationRegSlaveModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = BaseStationRegSlaveModelCount % 15;
            if
            (
                MasterName == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                MasterAntennaName == "" &
                MasterAntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                BaseStationRegSlaveModel[indexToUpdate] = new BaseStationRegSlaveModel
                {
                    ID = null,  // Update ID to match the new entry position
                    MasterName = MasterName,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    MasterAntennaIPAddress = MasterAntennaIPAddress,
                    MasterAntennaName = MasterAntennaName,
                    Date = Date,
                    Time = Time
                };
            }
            else
            {
                int objCount1 = 0;
                foreach (BaseStationRegSlaveModel _baseStationRegSlaveModel in baseStationRegSlaveModel)
                {
                    if (IsObjectEmptyOrNull(_baseStationRegSlaveModel))
                    {
                        break;
                    }
                    objCount1++;
                }
                if (objCount1 < 15)
                {
                    BaseStationRegSlaveModel[objCount1] = new BaseStationRegSlaveModel
                    {
                        ID = objCount1 + 1,  // Update ID to match the new entry position
                        MasterName = MasterName,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        MasterAntennaIPAddress = MasterAntennaIPAddress,
                        MasterAntennaName = MasterAntennaName,
                        Date = Date,
                        Time = Time
                    };
                }
                else
                {
                    BaseStationRegSlaveModel[objCount % 15] = new BaseStationRegSlaveModel
                    {
                        ID = objCount % 15 + 1,  // Update ID to match the new entry position
                        MasterName = MasterName,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        MasterAntennaIPAddress = MasterAntennaIPAddress,
                        MasterAntennaName = MasterAntennaName,
                        Date = Date,
                        Time = Time
                    };
                    objCount++;
                }
                if (canAdd2DB)
                {
                    try
                    {
                        IPAddress.Parse(MasterAntennaIPAddress);
                        SaveDataBase();
                    }
                    catch (FormatException ex)
                    {
                        if (_mainPageSlaveViewModel == null)
                        {
                            CreateAppClassAfterDelay();
                        }
                        if (_mainPageSlaveViewModel != null)
                        {
                            _mainPageSlaveViewModel.SlaveAlarmData = ex.ToString();
                        }
                    }
                }
                canAdd2DB = false;
            }
            BaseStationRegSlaveModelCount++;
        }
    }

    public void SaveDataBase()
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{MasterAntennaIPAddress}.txt";

        // Clear the contents of the file by writing an empty string
        File.WriteAllText(filePath, string.Empty);

        foreach (var station in baseStationRegSlaveModel)
        {
            if (!IsObjectEmptyOrNull(station as BaseStationRegSlaveModel))
            {
                try
                {
                    IPAddress.Parse(MasterAntennaIPAddress);
                    File.AppendAllText(filePath,
                        $"{station.MasterAntennaIPAddress}," +
                        $"{station.MasterAntennaName}," +
                        $"{station.MasterName}," +
                        $"{station.Latitude}," +
                        $"{station.Longitude}," +
                        $"{station.Elevation}," +
                        $"{station.PoleLength}," +
                        $"{station.Date}," +
                        $"{station.Time}\n");
                }
                catch
                {
                }
            }
        }
    }

    public bool IsObjectEmptyOrNull(BaseStationRegSlaveModel model)
    {
        // Get all the public properties of the model
        var properties = typeof(BaseStationRegSlaveModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        // Check if all properties are either null or empty string
        foreach (var prop in properties)
        {
            // Get the value of the property
            var value = prop.GetValue(model);

            // If value is not null and not an empty string, the object is not "empty"
            if (value != null)
            {
                if (value != "")
                {
                    return false; // Object has a non-null or non-empty value
                }
            }
        }

        return true; // All properties are null or empty
    }

    public void DeleteSelectedItem()
    {
        if (SelectedBaseStationRegSlaveModel != null)
        {
            BaseStationRegSlaveModel.Remove(SelectedBaseStationRegSlaveModel);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
            SaveDataBase();
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedBaseStationRegSlaveModel != null;
    }

    public async Task ExportBaseStationRegSlaveModelToCsvAsync(ObservableCollection<BaseStationRegSlaveModel> baseStationRegSlaveModel)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "BaseStationRegistrationSlaveExport.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of BaseStationRegSlaveModel
        sb.AppendLine
            (
                "ID," +
                "MasterAntennaIPAddress," +
                "MasterAntennaName," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in baseStationRegSlaveModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.MasterAntennaIPAddress}," +
                    $"{station.MasterAntennaName}," +
                    $"{station.MasterName}," +
                    $"{station.Latitude}," +
                    $"{station.Longitude}," +
                    $"{station.Elevation}," +
                    $"{station.PoleLength}," +
                    $"{station.Date}," +
                    $"{station.Time},"
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
        if (_mainPageSlaveViewModel == null)
        {
            // Access properties directly
            _mainPageSlaveViewModel = app.mainPageSlaveViewModel;
        }
        _message =
            "SM," +
            SelectedMasterAntennaIPAddress +
            "," +
            SelectedMasterAntennaName +
            "," +
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
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedLatitudeMaster == "" || SelectedLatitudeMaster == null ||
            SelectedMasterName == "" || SelectedMasterName == null ||
            SelectedLongitudeMaster == "" || SelectedLongitudeMaster == null ||
            SelectedElevationMaster == "" || SelectedElevationMaster == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedMasterAntennaIPAddress == "" || SelectedMasterAntennaIPAddress == null
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
        (!MainPageSlaveModel.IsValidIPAddress(SelectedMasterAntennaIPAddress))
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
            _mainPageSlaveViewModel.mainPageSlaveModel.MessageSend(_message);
        }
        MainPageSlaveNavigateCommand.Execute(null);
    }

    void CheckIPAvailability(string masterAntennaIPAddress)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{masterAntennaIPAddress}.txt";
        if (File.Exists(filePath))
        {
            for (int i = 0; i < 15; i++)
            {
                AddStationDBPage
                (
                    "", "", "", "", "", "", "", "", ""
                );
            }
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null && line != "")
                {
                    string[] elements = line.Split(',');
                    AddStationDBPage(
                        elements[0],
                        elements[1],
                        elements[2],
                        elements[3],
                        elements[4],
                        elements[5],
                        elements[6],
                        elements[7],
                        elements[8]);
                }
            }
        }
        else
        {
            for (int i = 0; i < 15; i++)
            {
                AddStationDBPage
                (
                    "", "", "", "", "", "", "", "", ""
                );
            }
        }
    }

    public async Task RegistrationDataGridSendAsync()
    {
        if (_mainPageSlaveViewModel == null)
        {
            // Access properties directly
            _mainPageSlaveViewModel = app.mainPageSlaveViewModel;
        }
        _message =
            "SM," +
            MasterAntennaIPAddress +
            "," +
            MasterAntennaName +
            "," +
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
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            MasterName == "" || MasterName == null ||
            LatitudeMaster == "" || LatitudeMaster == null ||
            LongitudeMaster == "" || LongitudeMaster == null ||
            ElevationMaster == "" || ElevationMaster == null ||
            PoleHeight == "" || PoleHeight == null ||
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
        (!MainPageSlaveModel.IsValidIPAddress(MasterAntennaIPAddress))
        {
            ContentDialog dialog = new ContentDialog()
            {
                Title = "INVALID ANTENNA IPADDRESS",
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
        else
        {
            _mainPageSlaveViewModel.mainPageSlaveModel.MessageSend(_message);
        }
        MainPageSlaveNavigateCommand.Execute(null);
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
            OnPropertyChanged(nameof(ElevationMaster));
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
            OnPropertyChanged(nameof(ElevationMaster));
        }
    }

    private string elevationMaster = "";
    public string ElevationMaster
    {
        get
        {
            if (_mainPageSlaveViewModel == null)
            {
                // Access properties directly
                CreateAppClassAfterDelay();
            }
            if (_mainPageSlaveViewModel != null)
            {
                if (!IsElevationEnabled)
                {
                    if
                    (
                        LatitudeMaster.Length >= 2 &&
                        LongitudeMaster.Length >= 3 &&
                        float.Parse(LatitudeMaster) >= 20.25f &&
                        float.Parse(LatitudeMaster) <= 45.35f &&
                        float.Parse(LongitudeMaster) >= 122.523f &&
                        float.Parse(LongitudeMaster) <= 153.523f
                    )
                    {
                        //elevationMaster = _mainPageSlaveViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeMaster), float.Parse(LongitudeMaster)).ToString();
                    }
                    else
                    {
                        elevationMaster = "-999.9";
                    }
                }
            }
            return elevationMaster;
        }
        set
        {
            if (_mainPageSlaveViewModel == null)
            {
                // Access properties directly
                CreateAppClassAfterDelay();
            }
            if (_mainPageSlaveViewModel != null)
            {
                if (!IsElevationEnabled)
                {
                    if
                    (
                        LatitudeMaster.Length >= 2 &&
                        LongitudeMaster.Length >= 3 &&
                        float.Parse(LatitudeMaster) >= 20.25f &&
                        float.Parse(LatitudeMaster) <= 45.35f &&
                        float.Parse(LongitudeMaster) >= 122.523f &&
                        float.Parse(LongitudeMaster) <= 153.523f
                    )
                    {
                        //elevationMaster = _mainPageSlaveViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeMaster), float.Parse(LongitudeMaster)).ToString();
                    }
                    else
                    {
                        elevationMaster = "-999.9";
                    }
                }
                else
                {
                    elevationMaster = value;
                }
                OnPropertyChanged(nameof(ElevationMaster));
            }
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

    private string masterAntennaName = "*";
    public string MasterAntennaName
    {
        get => masterAntennaName;
        set
        {
            masterAntennaName = value;
            OnPropertyChanged(nameof(MasterAntennaName));
        }
    }

    private string masterAntennaIPAddress = "";
    public string MasterAntennaIPAddress
    {
        get => masterAntennaIPAddress;
        set
        {
            masterAntennaIPAddress = value;
            OnPropertyChanged(nameof(MasterAntennaIPAddress));
            CheckIPAvailability(MasterAntennaIPAddress);
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

    private string selectedMasterAntennaName = "";
    public string SelectedMasterAntennaName
    {
        get
        {
            return selectedMasterAntennaName;
        }
        set
        {
            selectedMasterAntennaName = value;
            OnPropertyChanged(nameof(SelectedMasterAntennaName));
        }
    }

    private string selectedMasterAntennaIPAddress = "";
    public string SelectedMasterAntennaIPAddress
    {
        get => selectedMasterAntennaIPAddress;
        set
        {
            selectedMasterAntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedMasterAntennaIPAddress));
        }
    }

    public bool IsMasterIPAddressShow
    {
        get
        {
            return _mainPageSlaveViewModel.IsMasterIPAddressShow;
        }
        set
        {
            _mainPageSlaveViewModel.IsMasterIPAddressShow = value;
            OnPropertyChanged(nameof(IsMasterIPAddressShow));
        }
    }

    public bool IsNameShowBase
    {
        get
        {
            if (_mainPageSlaveViewModel == null)
            {
                CreateAppClassAfterDelay();
            }
            if (_mainPageSlaveViewModel != null)
            {
                return _mainPageSlaveViewModel.IsNameShowMaster;
            }
            return false;
        }
        set
        {
            _mainPageSlaveViewModel.IsNameShowMaster = value;
            OnPropertyChanged(nameof(IsNameShowBase));
        }
    }

    private bool isElevationEnabled;
    public bool IsElevationEnabled
    {
        get => isElevationEnabled;
        set
        {
            isElevationEnabled = value;
            OnPropertyChanged(nameof(IsElevationEnabled));
            OnPropertyChanged(nameof(ElevationInabilityShow));
            OnPropertyChanged(nameof(ElevationMaster));
        }
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }
}
