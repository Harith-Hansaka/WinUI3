using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.MASTER;

public class SubstationDB4MasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    MainPageMasterViewModel? _mainPageMasterViewModel;
    int substationDB4MasterModelCount = 0;
    App app;
    string _message;
    public bool canAdd2DB = false;
    int objCount = 0;
    int IsIPAddressShowSubPosition = 0;
    int IsJapaneseNameShowSubPosition = 0;

    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand DBExportCommand { get; }
    public ICommand EditDataGridCommand { get; }
    public ICommand RegistrationDataGridCommand { get; }
    public ICommand MainPageMasterNavigateCommand { get; }
    public ICommand SubstationRegistrationMasterNavigateCommand { get; }

    public SubstationDB4MasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        SubstationDB4MasterModel = new ObservableCollection<SubstationDB4MasterModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(() => ExportStationDBPageModelMasterToCsvAsync(substationDB4MasterModel));
        EditDataGridCommand = new AsyncRelayCommand(EditedDataGridSendAsync);
        RegistrationDataGridCommand = new AsyncRelayCommand(EditedDataGridSendAsync);
        app = (App)Application.Current;
        CreateAppClassAfterDelay();
        for (int i = 0; i < 15; i++)
        {
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
        }

        _message = string.Empty;
    }

    private async void CreateAppClassAfterDelay()
    {
        await Task.Delay(50);
        if (_mainPageMasterViewModel == null)
        {
            // Access properties directly
            _mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
    }

    private ObservableCollection<SubstationDB4MasterModel> substationDB4MasterModel;
    public ObservableCollection<SubstationDB4MasterModel> SubstationDB4MasterModel
    {
        get { return substationDB4MasterModel; }
        set
        {
            substationDB4MasterModel = value;
            OnPropertyChanged(nameof(SubstationDB4MasterModel));
        }
    }

    private SubstationDB4MasterModel _selectedSubstationDB4MasterModel;
    public SubstationDB4MasterModel SelectedSubstationDB4MasterModel
    {
        get { return _selectedSubstationDB4MasterModel; }
        set
        {
            _selectedSubstationDB4MasterModel = value;
            if (SelectedSubstationDB4MasterModel != null)
            {
                SelectedSlave4Name = SelectedSubstationDB4MasterModel.Name;
                SelectedLatitudeSlave4 = SelectedSubstationDB4MasterModel.Latitude;
                SelectedLongitudeSlave4 = SelectedSubstationDB4MasterModel.Longitude;
                SelectedElevationSlave4 = SelectedSubstationDB4MasterModel.Elevation;
                SelectedPoleHeight = SelectedSubstationDB4MasterModel.PoleLength;
                SelectedSlave4AntennaIPAddress = SelectedSubstationDB4MasterModel.Slave4AntennaIPAddress;
                SelectedSlave4AntennaName = SelectedSubstationDB4MasterModel.Slave4AntennaName;
            }
            OnPropertyChanged(nameof(SelectedSubstationDB4MasterModel));
        }
    }

    public void AddStationDBPage
    (
        string Slave4AntennaIPAddress,
        string Slave4AntennaName,
        string Name,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string Date,
        String Time
    )
    {
        if (substationDB4MasterModelCount < 15 || substationDB4MasterModel.Count < 15)
        {
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave4AntennaIPAddress == "" &
                Slave4AntennaName == "" &
                Date == "" &
                Time == ""
            )
            {
                var newSubstationDB4MasterModel = new SubstationDB4MasterModel
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave4AntennaIPAddress = Slave4AntennaIPAddress,
                    Slave4AntennaName = Slave4AntennaName
                };

                substationDB4MasterModelCount++;
                SubstationDB4MasterModel.Add(newSubstationDB4MasterModel);
            }
            else
            {
                var newSubstationDB4MasterModel = new SubstationDB4MasterModel
                {
                    ID = substationDB4MasterModelCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave4AntennaIPAddress = Slave4AntennaIPAddress,
                    Slave4AntennaName = Slave4AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB4MasterModelCount++;
                SubstationDB4MasterModel.Add(newSubstationDB4MasterModel);
            }
        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = substationDB4MasterModelCount % 15;
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave4AntennaName == "" &
                Slave4AntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                SubstationDB4MasterModel[indexToUpdate] = new SubstationDB4MasterModel
                {
                    ID = null,  // Update ID to match the new entry position
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave4AntennaIPAddress = Slave4AntennaIPAddress,
                    Slave4AntennaName = Slave4AntennaName,
                    Date = Date,
                    Time = Time
                };
            }
            else
            {
                int objCount1 = 0;
                foreach (SubstationDB4MasterModel _substationDB4MasterModel in substationDB4MasterModel)
                {
                    if (IsObjectEmptyOrNull(_substationDB4MasterModel))
                    {
                        break;
                    }
                    objCount1++;
                }
                if (objCount1 < 15)
                {
                    SubstationDB4MasterModel[objCount1] = new SubstationDB4MasterModel
                    {
                        ID = objCount1 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave4AntennaIPAddress = Slave4AntennaIPAddress,
                        Slave4AntennaName = Slave4AntennaName,
                        Date = Date,
                        Time = Time
                    };
                }
                else
                {
                    SubstationDB4MasterModel[objCount % 15] = new SubstationDB4MasterModel
                    {
                        ID = objCount % 15 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave4AntennaIPAddress = Slave4AntennaIPAddress,
                        Slave4AntennaName = Slave4AntennaName,
                        Date = Date,
                        Time = Time
                    };
                    objCount++;
                }
                if (canAdd2DB)
                {
                    try
                    {
                        IPAddress.Parse(Slave4AntennaIPAddress);
                        SaveDataBase();
                    }
                    catch (FormatException ex)
                    {
                        if (_mainPageMasterViewModel == null)
                        {
                            CreateAppClassAfterDelay();
                        }
                        if (_mainPageMasterViewModel != null)
                        {
                            _mainPageMasterViewModel.MasterAlarmData = ex.ToString();
                        }
                    }
                }
                canAdd2DB = false;
            }
            substationDB4MasterModelCount++;
        }
    }

    public void SaveDataBase()
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{Slave4AntennaIPAddress}.txt";

        // Clear the contents of the file by writing an empty string
        File.WriteAllText(filePath, string.Empty);

        foreach (var station in substationDB4MasterModel)
        {
            if (!IsObjectEmptyOrNull(station as SubstationDB4MasterModel))
            {
                try
                {
                    IPAddress.Parse(Slave4AntennaIPAddress);
                    File.AppendAllText(filePath,
                        $"{station.Slave4AntennaIPAddress}," +
                        $"{station.Slave4AntennaName}," +
                        $"{station.Name}," +
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

    public bool IsObjectEmptyOrNull(SubstationDB4MasterModel model)
    {
        // Get all the public properties of the model
        var properties = typeof(SubstationDB4MasterModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

    void CheckIPAvailability(string _slave4AntennaIPAddress)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{_slave4AntennaIPAddress}.txt";
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

    public void DeleteSelectedItem()
    {
        if (SelectedSubstationDB4MasterModel != null)
        {
            SubstationDB4MasterModel.Remove(SelectedSubstationDB4MasterModel);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
            SaveDataBase();
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedSubstationDB4MasterModel != null;
    }

    public async Task ExportStationDBPageModelMasterToCsvAsync(ObservableCollection<SubstationDB4MasterModel> substationDB4MasterModel)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "SubstationDB4Export.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of SubstationDB4MasterModel
        sb.AppendLine
            (
                "ID," +
                "Slave4AntennaIPAddress," +
                "Slave4AntennaName," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in substationDB4MasterModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Slave4AntennaIPAddress}," +
                    $"{station.Slave4AntennaName}," +
                    $"{station.Name}," +
                    $"{station.Latitude}," +
                    $"{station.Longitude}," +
                    $"{station.Elevation}," +
                    $"{station.PoleLength}," +
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
        if (_mainPageMasterViewModel == null)
        {
            // Access properties directly
            _mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
        _message =
            "S4," +
            SelectedSlave4AntennaIPAddress +
            "," +
            SelectedSlave4AntennaName +
            "," +
            SelectedSlave4Name +
            "," +
            SelectedLatitudeSlave4 +
            "," +
            SelectedLongitudeSlave4 +
            "," +
            SelectedElevationSlave4 +
            "," +
            SelectedPoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedLatitudeSlave4 == "" || SelectedLatitudeSlave4 == null ||
            SelectedLongitudeSlave4 == "" || SelectedLongitudeSlave4 == null ||
            SelectedElevationSlave4 == "" || SelectedElevationSlave4 == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedSlave4Name == "" || SelectedSlave4Name == null ||
            SelectedSlave4AntennaIPAddress == "" || SelectedSlave4AntennaIPAddress == null
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
        else if (!MainPageMasterModel.IsValidIPAddress(SelectedSlave4AntennaIPAddress))
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
        else
        {
            _mainPageMasterViewModel._mainPageMasterModel.MessageSend(_message);
        }
        SubstationRegistrationMasterNavigateCommand.Execute(null);
    }

    public async Task RegistrationDataGridSendAsync()
    {
        if (_mainPageMasterViewModel == null)
        {
            // Access properties directly
            _mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
        _message =
            "S4," +
            Slave4AntennaIPAddress +
            "," +
            Slave4AntennaName +
            "," +
            Slave4Name +
            "," +
            LatitudeSlave4 +
            "," +
            LongitudeSlave4 +
            "," +
            ElevationSlave4 +
            "," +
            PoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            LatitudeSlave4 == "" || LatitudeSlave4 == null ||
            LongitudeSlave4 == "" || LongitudeSlave4 == null ||
            ElevationSlave4 == "" || ElevationSlave4 == null ||
            Slave4Name == "" || Slave4Name == null ||
            PoleHeight == "" || PoleHeight == null ||
            Slave4AntennaIPAddress == "" || Slave4AntennaIPAddress == null
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
        else if (!MainPageMasterModel.IsValidIPAddress(Slave4AntennaIPAddress))
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
        else
        {
            _mainPageMasterViewModel._mainPageMasterModel.MessageSend(_message);
        }
        SubstationRegistrationMasterNavigateCommand.Execute(null);
    }

    // Properties for Slave4
    private string latitudeSlave4 = "";
    public string LatitudeSlave4
    {
        get => latitudeSlave4;
        set
        {
            latitudeSlave4 = value;
            OnPropertyChanged(nameof(LatitudeSlave4));
            OnPropertyChanged(nameof(ElevationSlave4));
        }
    }

    private string longitudeSlave4 = "";
    public string LongitudeSlave4
    {
        get => longitudeSlave4;
        set
        {
            longitudeSlave4 = value;
            OnPropertyChanged(nameof(LongitudeSlave4));
            OnPropertyChanged(nameof(ElevationSlave4));
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
            OnPropertyChanged(nameof(ElevationSlave4));
        }
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private string elevationSlave4 = "";
    public string ElevationSlave4
    {
        get
        {
            if (_mainPageMasterViewModel == null)
            {
                // Access properties directly
                _mainPageMasterViewModel = app.mainPageMasterViewModel;
            }
            if (_mainPageMasterViewModel != null)
            {
                if (!IsElevationEnabled)
                {
                    if
                    (
                        LatitudeSlave4.Length >= 2 &&
                        LongitudeSlave4.Length >= 3 &&
                        float.Parse(LatitudeSlave4) >= 20.25f &&
                        float.Parse(LatitudeSlave4) <= 45.35f &&
                        float.Parse(LongitudeSlave4) >= 122.523f &&
                        float.Parse(LongitudeSlave4) <= 153.523f
                    )
                    {
                        //elevationSlave4 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave4), float.Parse(LongitudeSlave4)).ToString();
                    }
                    else
                    {
                        elevationSlave4 = "-999.9";
                    }
                }
            }
            return elevationSlave4;
        }
        set
        {
            if (!IsElevationEnabled)
            {
                if
                (
                    LatitudeSlave4.Length >= 2 &&
                    LongitudeSlave4.Length >= 3 &&
                    float.Parse(LatitudeSlave4) >= 20.25f &&
                    float.Parse(LatitudeSlave4) <= 45.35f &&
                    float.Parse(LongitudeSlave4) >= 122.523f &&
                    float.Parse(LongitudeSlave4) <= 153.523f
                )
                {
                    //elevationSlave4 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave4), float.Parse(LongitudeSlave4)).ToString();
                }
                else
                {
                    elevationSlave4 = "-999.9";
                }
            }
            else
            {
                elevationSlave4 = value;
            }
            OnPropertyChanged(nameof(ElevationSlave4));
        }
    }

    private string slave4Name = "";
    public string Slave4Name
    {
        get => slave4Name;
        set
        {
            slave4Name = value;
            OnPropertyChanged(nameof(Slave4Name));
        }
    }

    private string poleHeight = "";
    public string PoleHeight
    {
        get => poleHeight;
        set
        {
            poleHeight = value;
            OnPropertyChanged(nameof(PoleHeight));
        }
    }

    private string slave4AntennaIPAddress = "";
    public string Slave4AntennaIPAddress
    {
        get => slave4AntennaIPAddress;
        set
        {
            slave4AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave4AntennaIPAddress));
            CheckIPAvailability(Slave4AntennaIPAddress);
        }
    }

    private string slave4AntennaName = "*";
    public string Slave4AntennaName
    {
        get => slave4AntennaName;
        set
        {
            slave4AntennaName = value;
            OnPropertyChanged(nameof(Slave4AntennaName));
        }
    }

    // Selected properties for Slave4
    private string selectedSlave4Name = "";
    public string SelectedSlave4Name
    {
        get => selectedSlave4Name;
        set
        {
            selectedSlave4Name = value;
            OnPropertyChanged(nameof(SelectedSlave4Name));
        }
    }

    private string selectedLatitudeSlave4 = "";
    public string SelectedLatitudeSlave4
    {
        get => selectedLatitudeSlave4;
        set
        {
            selectedLatitudeSlave4 = value;
            OnPropertyChanged(nameof(SelectedLatitudeSlave4));
        }
    }

    private string selectedLongitudeSlave4 = "";
    public string SelectedLongitudeSlave4
    {
        get => selectedLongitudeSlave4;
        set
        {
            selectedLongitudeSlave4 = value;
            OnPropertyChanged(nameof(SelectedLongitudeSlave4));
        }
    }

    private string selectedElevationSlave4 = "";
    public string SelectedElevationSlave4
    {
        get => selectedElevationSlave4;
        set
        {
            selectedElevationSlave4 = value;
            OnPropertyChanged(nameof(SelectedElevationSlave4));
        }
    }

    private string selectedPoleHeight = "";
    public string SelectedPoleHeight
    {
        get => selectedPoleHeight;
        set
        {
            selectedPoleHeight = value;
            OnPropertyChanged(nameof(SelectedPoleHeight));
        }
    }

    private string selectedSlave4AntennaIPAddress = "";
    public string SelectedSlave4AntennaIPAddress
    {
        get => selectedSlave4AntennaIPAddress;
        set
        {
            selectedSlave4AntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedSlave4AntennaIPAddress));
        }
    }

    private string selectedSlave4AntennaName = "";
    public string SelectedSlave4AntennaName
    {
        get => selectedSlave4AntennaName;
        set
        {
            selectedSlave4AntennaName = value;
            OnPropertyChanged(nameof(SelectedSlave4AntennaName));
        }
    }

    public bool IsIPAddressShowSub4
    {
        get
        {
            if (_mainPageMasterViewModel == null)
            {
                CreateAppClassAfterDelay();
            }
            if (_mainPageMasterViewModel != null)
            {
                if (IsIPAddressShowSubPosition == 1)
                {
                    return _mainPageMasterViewModel.IsIPAddressShowSub1;
                }
                else if (IsIPAddressShowSubPosition == 2)
                {
                    return _mainPageMasterViewModel.IsIPAddressShowSub2;
                }
                else if (IsIPAddressShowSubPosition == 3)
                {
                    return _mainPageMasterViewModel.IsIPAddressShowSub3;
                }
                else if (IsIPAddressShowSubPosition == 4)
                {
                    return _mainPageMasterViewModel.IsIPAddressShowSub4;
                }
                else
                {
                    return _mainPageMasterViewModel.IsIPAddressShowSub4;
                }
            }
            return false;
        }
        set
        {
            if (_mainPageMasterViewModel == null)
            {
                CreateAppClassAfterDelay();
            }
            if (_mainPageMasterViewModel != null)
            {
                if (Slave4AntennaIPAddress == _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText.Split(',')[1])
                {
                    _mainPageMasterViewModel.IsIPAddressShowSub1 = value;
                    IsIPAddressShowSubPosition = 1;
                }
                else if (Slave4AntennaIPAddress == _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText.Split(',')[1])
                {
                    _mainPageMasterViewModel.IsIPAddressShowSub2 = value;
                    IsIPAddressShowSubPosition = 2;
                }
                else if (Slave4AntennaIPAddress == _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText.Split(',')[1])
                {
                    _mainPageMasterViewModel.IsIPAddressShowSub3 = value;
                    IsIPAddressShowSubPosition = 3;
                }
                else if (Slave4AntennaIPAddress == _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText.Split(',')[1])
                {
                    _mainPageMasterViewModel.IsIPAddressShowSub4 = value;
                    IsIPAddressShowSubPosition = 4;
                }
                else
                {
                    _mainPageMasterViewModel.IsIPAddressShowSub4 = value;
                }
            }
            OnPropertyChanged(nameof(IsIPAddressShowSub4));
        }
    }

    public bool IsNameShowSub4
    {
        get
        {
            if (_mainPageMasterViewModel == null)
            {
                CreateAppClassAfterDelay();
            }
            if (_mainPageMasterViewModel != null)
            {
                if (IsJapaneseNameShowSubPosition == 1)
                {
                    return _mainPageMasterViewModel.IsNameShowSub1;
                }
                else if (IsJapaneseNameShowSubPosition == 2)
                {
                    return _mainPageMasterViewModel.IsNameShowSub2;
                }
                else if (IsJapaneseNameShowSubPosition == 3)
                {
                    return _mainPageMasterViewModel.IsNameShowSub3;
                }
                else if (IsJapaneseNameShowSubPosition == 4)
                {
                    return _mainPageMasterViewModel.IsNameShowSub4;
                }
            }
            return false;
        }
        set
        {
            if (Slave4AntennaName == _mainPageMasterViewModel.Slave1JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub1 = value;
                IsJapaneseNameShowSubPosition = 1;
            }
            else if (Slave4AntennaName == _mainPageMasterViewModel.Slave2JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub2 = value;
                IsJapaneseNameShowSubPosition = 2;
            }
            else if (Slave4AntennaName == _mainPageMasterViewModel.Slave3JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub3 = value;
                IsJapaneseNameShowSubPosition = 3;
            }
            else if (Slave4AntennaName == _mainPageMasterViewModel.Slave4JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub4 = value;
                IsJapaneseNameShowSubPosition = 4;
            }
            OnPropertyChanged(nameof(IsNameShowSub4));
        }
    }
}
