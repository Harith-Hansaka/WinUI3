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

public class SubstationDB2MasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    SubstationDB2MasterModel newSubstationDB2MasterModel;
    MainPageMasterViewModel _mainPageMasterViewModel;
    int substationDB2MasterModelCount = 0;
    App app;
    string _message;
    public bool canAdd2DB = false;
    int objCount = 0;
    int IsIPAddressShowSubPosition = 0;
    int IsJapaneseNameShowSubPosition = 0;

    public ICommand MainPageMasterNavigateCommand { get; }
    public ICommand SubstationRegistrationMasterNavigateCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand DBExportCommand { get; }
    public ICommand EditDataGridCommand { get; }
    public ICommand RegistrationDataGridCommand { get; }

    public SubstationDB2MasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        SubstationDB2MasterModel = new ObservableCollection<SubstationDB2MasterModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(() => ExportStationDBPageModelMasterToCsvAsync(substationDB2MasterModel));
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
        if (_mainPageMasterViewModel == null)
        {
            // Access properties directly
            _mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
    }

    private ObservableCollection<SubstationDB2MasterModel> substationDB2MasterModel;
    public ObservableCollection<SubstationDB2MasterModel> SubstationDB2MasterModel
    {
        get { return substationDB2MasterModel; }
        set
        {
            substationDB2MasterModel = value;
            OnPropertyChanged(nameof(SubstationDB2MasterModel));
        }
    }

    private SubstationDB2MasterModel _selectedSubstationDB2MasterModel;
    public SubstationDB2MasterModel SelectedSubstationDB2MasterModel
    {
        get { return _selectedSubstationDB2MasterModel; }
        set
        {
            _selectedSubstationDB2MasterModel = value;
            if (SelectedSubstationDB2MasterModel != null)
            {
                SelectedSlave2Name = SelectedSubstationDB2MasterModel.Name;
                SelectedLatitudeSlave2 = SelectedSubstationDB2MasterModel.Latitude;
                SelectedLongitudeSlave2 = SelectedSubstationDB2MasterModel.Longitude;
                SelectedElevationSlave2 = SelectedSubstationDB2MasterModel.Elevation;
                SelectedPoleHeight = SelectedSubstationDB2MasterModel.PoleLength;
                SelectedSlave2AntennaIPAddress = SelectedSubstationDB2MasterModel.Slave2AntennaIPAddress;
                SelectedSlave2AntennaName = SelectedSubstationDB2MasterModel.Slave2AntennaName;
            }
            OnPropertyChanged(nameof(SelectedSubstationDB2MasterModel));
        }
    }

    public void AddStationDBPage
    (
        string Slave2AntennaIPAddress,
        string Slave2AntennaName,
        string Name,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string Date,
        String Time
    )
    {
        if (substationDB2MasterModelCount < 15 || substationDB2MasterModel.Count < 15)
        {
            if (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave2AntennaIPAddress == "" &
                Slave2AntennaName == "" &
                Date == "" &
                Time == ""
            )
            {
                newSubstationDB2MasterModel = new SubstationDB2MasterModel
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave2AntennaIPAddress = Slave2AntennaIPAddress,
                    Slave2AntennaName = Slave2AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB2MasterModelCount++;
                SubstationDB2MasterModel.Add(newSubstationDB2MasterModel);
            }
            else
            {
                newSubstationDB2MasterModel = new SubstationDB2MasterModel
                {
                    ID = substationDB2MasterModelCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave2AntennaIPAddress = Slave2AntennaIPAddress,
                    Slave2AntennaName = Slave2AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB2MasterModelCount++;
                SubstationDB2MasterModel.Add(newSubstationDB2MasterModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = substationDB2MasterModelCount % 15;
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave2AntennaName == "" &
                Slave2AntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                SubstationDB2MasterModel[indexToUpdate] = new SubstationDB2MasterModel
                {
                    ID = null,  // Update ID to match the new entry position
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave2AntennaIPAddress = Slave2AntennaIPAddress,
                    Slave2AntennaName = Slave2AntennaName,
                    Date = Date,
                    Time = Time
                };
            }
            else
            {
                int objCount1 = 0;
                foreach (SubstationDB2MasterModel _substationDB2MasterModel in substationDB2MasterModel)
                {
                    if (IsObjectEmptyOrNull(_substationDB2MasterModel))
                    {
                        break;
                    }
                    objCount1++;
                }
                if (objCount1 < 15)
                {
                    SubstationDB2MasterModel[objCount1] = new SubstationDB2MasterModel
                    {
                        ID = objCount1 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave2AntennaIPAddress = Slave2AntennaIPAddress,
                        Slave2AntennaName = Slave2AntennaName,
                        Date = Date,
                        Time = Time
                    };
                }
                else
                {
                    SubstationDB2MasterModel[objCount % 15] = new SubstationDB2MasterModel
                    {
                        ID = objCount % 15 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave2AntennaIPAddress = Slave2AntennaIPAddress,
                        Slave2AntennaName = Slave2AntennaName,
                        Date = Date,
                        Time = Time
                    };
                    objCount++;
                }
                if (canAdd2DB)
                {
                    try
                    {
                        IPAddress.Parse(Slave2AntennaIPAddress);
                        SaveDataBase();
                    }
                    catch (FormatException ex)
                    {
                    }
                }
                canAdd2DB = false;
            }
            substationDB2MasterModelCount++;
        }
    }

    public void SaveDataBase()
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{Slave2AntennaIPAddress}.txt";

        // Clear the contents of the file by writing an empty string
        File.WriteAllText(filePath, string.Empty);

        foreach (var station in substationDB2MasterModel)
        {
            if (!IsObjectEmptyOrNull(station as SubstationDB2MasterModel))
            {
                try
                {
                    IPAddress.Parse(Slave2AntennaIPAddress);
                    File.AppendAllText(filePath,
                        $"{station.Slave2AntennaIPAddress}," +
                        $"{station.Slave2AntennaName}," +
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

    public bool IsObjectEmptyOrNull(SubstationDB2MasterModel model)
    {
        // Get all the public properties of the model
        var properties = typeof(SubstationDB2MasterModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

    void CheckIPAvailability(string _slave2AntennaIPAddress)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{_slave2AntennaIPAddress}.txt";
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
        if (SelectedSubstationDB2MasterModel != null)
        {
            SubstationDB2MasterModel.Remove(SelectedSubstationDB2MasterModel);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
            SaveDataBase();
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedSubstationDB2MasterModel != null;
    }

    public async Task ExportStationDBPageModelMasterToCsvAsync(ObservableCollection<SubstationDB2MasterModel> substationDB2MasterModel)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "SubstationDB2Export.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of SubstationDB2MasterModel
        sb.AppendLine
            (
                "ID," +
                "Slave2AntennaIPAddress," +
                "Slave2AntennaName," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in substationDB2MasterModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Slave2AntennaIPAddress}," +
                    $"{station.Slave2AntennaName}," +
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
            "S2," +
            SelectedSlave2AntennaIPAddress +
            "," +
            SelectedSlave2AntennaName +
            "," +
            SelectedSlave2Name +
            "," +
            SelectedLatitudeSlave2 +
            "," +
            SelectedLongitudeSlave2 +
            "," +
            SelectedElevationSlave2 +
            "," +
            SelectedPoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedLatitudeSlave2 == "" || SelectedLatitudeSlave2 == null ||
            SelectedLongitudeSlave2 == "" || SelectedLongitudeSlave2 == null ||
            SelectedElevationSlave2 == "" || SelectedElevationSlave2 == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedSlave2Name == "" || SelectedSlave2Name == null ||
            SelectedSlave2AntennaIPAddress == "" || SelectedSlave2AntennaIPAddress == null
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
        else if (!MainPageMasterModel.IsValidIPAddress(SelectedSlave2AntennaIPAddress))
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
            "S2," +
            Slave2AntennaIPAddress +
            "," +
            Slave2AntennaName +
            "," +
            Slave2Name +
            "," +
            LatitudeSlave2 +
            "," +
            LongitudeSlave2 +
            "," +
            ElevationSlave2 +
            "," +
            PoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            LatitudeSlave2 == "" || LatitudeSlave2 == null ||
            Slave2Name == "" || Slave2Name == null ||
            LongitudeSlave2 == "" || LongitudeSlave2 == null ||
            ElevationSlave2 == "" || ElevationSlave2 == null ||
            PoleHeight == "" || PoleHeight == null ||
            Slave2AntennaIPAddress == "" || Slave2AntennaIPAddress == null
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
        else if (!MainPageMasterModel.IsValidIPAddress(Slave2AntennaIPAddress))
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


    // Properties for Slave2
    private string latitudeSlave2 = "";
    public string LatitudeSlave2
    {
        get => latitudeSlave2;
        set
        {
            latitudeSlave2 = value;
            OnPropertyChanged(nameof(LatitudeSlave2));
            OnPropertyChanged(nameof(ElevationSlave2));
        }
    }

    private string longitudeSlave2 = "";
    public string LongitudeSlave2
    {
        get => longitudeSlave2;
        set
        {
            longitudeSlave2 = value;
            OnPropertyChanged(nameof(LongitudeSlave2));
            OnPropertyChanged(nameof(ElevationSlave2));
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
            OnPropertyChanged(nameof(ElevationSlave2));
        }
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private string elevationSlave2 = "";
    public string ElevationSlave2
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
                        LatitudeSlave2.Length >= 2 &&
                        LongitudeSlave2.Length >= 3 &&
                        float.Parse(LatitudeSlave2) >= 20.25f &&
                        float.Parse(LatitudeSlave2) <= 45.35f &&
                        float.Parse(LongitudeSlave2) >= 122.523f &&
                        float.Parse(LongitudeSlave2) <= 153.523f
                    )
                    {
                        //elevationSlave2 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave2), float.Parse(LongitudeSlave2)).ToString();
                    }
                    else
                    {
                        elevationSlave2 = "-999.9";
                    }
                }
            }
            return elevationSlave2;
        }
        set
        {
            if (!IsElevationEnabled)
            {
                if
                (
                    LatitudeSlave2.Length >= 2 &&
                    LongitudeSlave2.Length >= 3 &&
                    float.Parse(LatitudeSlave2) >= 20.25f &&
                    float.Parse(LatitudeSlave2) <= 45.35f &&
                    float.Parse(LongitudeSlave2) >= 122.523f &&
                    float.Parse(LongitudeSlave2) <= 153.523f
                )
                {
                    //elevationSlave2 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave2), float.Parse(LongitudeSlave2)).ToString();
                }
                else
                {
                    elevationSlave2 = "-999.9";
                }
            }
            else
            {
                elevationSlave2 = value;
            }
            OnPropertyChanged(nameof(ElevationSlave2));
        }
    }

    private string slave2Name = "";
    public string Slave2Name
    {
        get => slave2Name;
        set
        {
            slave2Name = value;
            OnPropertyChanged(nameof(Slave2Name));
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

    private string slave2AntennaIPAddress = "";
    public string Slave2AntennaIPAddress
    {
        get => slave2AntennaIPAddress;
        set
        {
            slave2AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave2AntennaIPAddress));
            CheckIPAvailability(Slave2AntennaIPAddress);
        }
    }

    private string slave2AntennaName = "*";
    public string Slave2AntennaName
    {
        get => slave2AntennaName;
        set
        {
            slave2AntennaName = value;
            OnPropertyChanged(nameof(Slave2AntennaName));
        }
    }

    // Selected properties for Slave2
    private string selectedSlave2Name = "";
    public string SelectedSlave2Name
    {
        get => selectedSlave2Name;
        set
        {
            selectedSlave2Name = value;
            OnPropertyChanged(nameof(SelectedSlave2Name));
        }
    }

    private string selectedLatitudeSlave2 = "";
    public string SelectedLatitudeSlave2
    {
        get => selectedLatitudeSlave2;
        set
        {
            selectedLatitudeSlave2 = value;
            OnPropertyChanged(nameof(SelectedLatitudeSlave2));
        }
    }

    private string selectedLongitudeSlave2 = "";
    public string SelectedLongitudeSlave2
    {
        get => selectedLongitudeSlave2;
        set
        {
            selectedLongitudeSlave2 = value;
            OnPropertyChanged(nameof(SelectedLongitudeSlave2));
        }
    }

    private string selectedElevationSlave2 = "";
    public string SelectedElevationSlave2
    {
        get => selectedElevationSlave2;
        set
        {
            selectedElevationSlave2 = value;
            OnPropertyChanged(nameof(SelectedElevationSlave2));
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

    private string selectedSlave2AntennaIPAddress = "";
    public string SelectedSlave2AntennaIPAddress
    {
        get => selectedSlave2AntennaIPAddress;
        set
        {
            selectedSlave2AntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedSlave2AntennaIPAddress));
        }
    }

    private string selectedSlave2AntennaName = "";
    public string SelectedSlave2AntennaName
    {
        get => selectedSlave2AntennaName;
        set
        {
            selectedSlave2AntennaName = value;
            OnPropertyChanged(nameof(SelectedSlave2AntennaName));
        }
    }

    public bool IsIPAddressShowSub2
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
                    return _mainPageMasterViewModel.IsIPAddressShowSub2;
                }
            }
            return false;
        }
        set
        {
            if (Slave2AntennaIPAddress == _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub1 = value;
                IsIPAddressShowSubPosition = 1;
            }
            else if (Slave2AntennaIPAddress == _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub2 = value;
                IsIPAddressShowSubPosition = 2;
            }
            else if (Slave2AntennaIPAddress == _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub3 = value;
                IsIPAddressShowSubPosition = 3;
            }
            else if (Slave2AntennaIPAddress == _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub4 = value;
                IsIPAddressShowSubPosition = 4;
            }
            else
            {
                _mainPageMasterViewModel.IsIPAddressShowSub2 = value;
            }
            OnPropertyChanged(nameof(IsIPAddressShowSub2));
        }
    }

    public bool IsNameShowSub2
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
            if (Slave2AntennaName == _mainPageMasterViewModel.Slave1JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub1 = value;
                IsJapaneseNameShowSubPosition = 1;
            }
            else if (Slave2AntennaName == _mainPageMasterViewModel.Slave2JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub2 = value;
                IsJapaneseNameShowSubPosition = 2;
            }
            else if (Slave2AntennaName == _mainPageMasterViewModel.Slave3JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub3 = value;
                IsJapaneseNameShowSubPosition = 3;
            }
            else if (Slave2AntennaName == _mainPageMasterViewModel.Slave4JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub4 = value;
                IsJapaneseNameShowSubPosition = 4;
            }
            OnPropertyChanged(nameof(IsNameShowSub2));
        }
    }
}
