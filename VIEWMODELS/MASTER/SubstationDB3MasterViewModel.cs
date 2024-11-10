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

public class SubstationDB3MasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    SubstationDB3MasterModel newSubstationDB3MasterModel;
    MainPageMasterViewModel _mainPageMasterViewModel;
    int substationDB3MasterModelCount = 0;
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


    public SubstationDB3MasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        SubstationDB3MasterModel = new ObservableCollection<SubstationDB3MasterModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(()=> ExportStationDBPageModelMasterToCsvAsync(substationDB3MasterModel));
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

    private ObservableCollection<SubstationDB3MasterModel> substationDB3MasterModel;
    public ObservableCollection<SubstationDB3MasterModel> SubstationDB3MasterModel
    {
        get { return substationDB3MasterModel; }
        set
        {
            substationDB3MasterModel = value;
            OnPropertyChanged(nameof(SubstationDB3MasterModel));
        }
    }

    private SubstationDB3MasterModel _selectedSubstationDB3MasterModel;
    public SubstationDB3MasterModel SelectedSubstationDB3MasterModel
    {
        get { return _selectedSubstationDB3MasterModel; }
        set
        {
            _selectedSubstationDB3MasterModel = value;
            if (SelectedSubstationDB3MasterModel != null)
            {
                SelectedSlave3Name = SelectedSubstationDB3MasterModel.Name;
                SelectedLatitudeSlave3 = SelectedSubstationDB3MasterModel.Latitude;
                SelectedLongitudeSlave3 = SelectedSubstationDB3MasterModel.Longitude;
                SelectedElevationSlave3 = SelectedSubstationDB3MasterModel.Elevation;
                SelectedPoleHeight = SelectedSubstationDB3MasterModel.PoleLength;
                SelectedSlave3AntennaIPAddress = SelectedSubstationDB3MasterModel.Slave3AntennaIPAddress;
                SelectedSlave3AntennaName = SelectedSubstationDB3MasterModel.Slave3AntennaName;
            }
            OnPropertyChanged(nameof(SelectedSubstationDB3MasterModel));
        }
    }

    public void AddStationDBPage
    (
        string Slave3AntennaIPAddress,
        string Slave3AntennaName,
        string Name,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string Date,
        String Time
    )
    {
        if (substationDB3MasterModelCount < 15 || substationDB3MasterModel.Count < 15)
        {
            if (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave3AntennaIPAddress == "" &
                Slave3AntennaName == "" &
                Date == "" &
                Time == ""
            )
            {
                var newSubstationDB3MasterModel = new SubstationDB3MasterModel
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave3AntennaIPAddress = Slave3AntennaIPAddress,
                    Slave3AntennaName = Slave3AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB3MasterModelCount++;
                SubstationDB3MasterModel.Add(newSubstationDB3MasterModel);
            }
            else
            {
                var newSubstationDB3MasterModel = new SubstationDB3MasterModel
                {
                    ID = substationDB3MasterModelCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave3AntennaIPAddress = Slave3AntennaIPAddress,
                    Slave3AntennaName = Slave3AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB3MasterModelCount++;
                SubstationDB3MasterModel.Add(newSubstationDB3MasterModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = substationDB3MasterModelCount % 15;
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave3AntennaName == "" &
                Slave3AntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                SubstationDB3MasterModel[indexToUpdate] = new SubstationDB3MasterModel
                {
                    ID = null,  // Update ID to match the new entry position
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave3AntennaIPAddress = Slave3AntennaIPAddress,
                    Slave3AntennaName = Slave3AntennaName,
                    Date = Date,
                    Time = Time
                };
            }
            else
            {
                int objCount1 = 0;
                foreach (SubstationDB3MasterModel _substationDB3MasterModel in substationDB3MasterModel)
                {
                    if (IsObjectEmptyOrNull(_substationDB3MasterModel))
                    {
                        break;
                    }
                    objCount1++;
                }
                if (objCount1 < 15)
                {
                    SubstationDB3MasterModel[objCount1] = new SubstationDB3MasterModel
                    {
                        ID = objCount1 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave3AntennaIPAddress = Slave3AntennaIPAddress,
                        Slave3AntennaName = Slave3AntennaName,
                        Date = Date,
                        Time = Time
                    };
                }
                else
                {
                    SubstationDB3MasterModel[objCount % 15] = new SubstationDB3MasterModel
                    {
                        ID = objCount % 15 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave3AntennaIPAddress = Slave3AntennaIPAddress,
                        Slave3AntennaName = Slave3AntennaName,
                        Date = Date,
                        Time = Time
                    };
                    objCount++;
                }
                if (canAdd2DB)
                {
                    try
                    {
                        IPAddress.Parse(Slave3AntennaIPAddress);
                        SaveDataBase();
                    }
                    catch (FormatException ex)
                    {
                    }
                }
                canAdd2DB = false;
            }
            substationDB3MasterModelCount++;
        }
    }

    public void SaveDataBase()
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{Slave3AntennaIPAddress}.txt";

        // Clear the contents of the file by writing an empty string
        File.WriteAllText(filePath, string.Empty);

        foreach (var station in substationDB3MasterModel)
        {
            if (!IsObjectEmptyOrNull(station as SubstationDB3MasterModel))
            {
                try
                {
                    IPAddress.Parse(Slave3AntennaIPAddress);
                    File.AppendAllText(filePath,
                        $"{station.Slave3AntennaIPAddress}," +
                        $"{station.Slave3AntennaName}," +
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

    public bool IsObjectEmptyOrNull(SubstationDB3MasterModel model)
    {
        // Get all the public properties of the model
        var properties = typeof(SubstationDB3MasterModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

    void CheckIPAvailability(string _slave3AntennaIPAddress)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{_slave3AntennaIPAddress}.txt";
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
        if (SelectedSubstationDB3MasterModel != null)
        {
            SubstationDB3MasterModel.Remove(SelectedSubstationDB3MasterModel);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
            SaveDataBase();
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedSubstationDB3MasterModel != null;
    }

    public async Task ExportStationDBPageModelMasterToCsvAsync(ObservableCollection<SubstationDB3MasterModel> substationDB3MasterModel)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "SubstationDB3Export.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of SubstationDB3MasterModel
        sb.AppendLine
            (
                "ID," +
                "Slave3AntennaIPAddress," +
                "Slave3AntennaName," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in substationDB3MasterModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Slave3AntennaIPAddress}," +
                    $"{station.Slave3AntennaName}," +
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
            "S3," +
            SelectedSlave3AntennaIPAddress +
            "," +
            SelectedSlave3AntennaName +
            "," +
            SelectedSlave3Name +
            "," +
            SelectedLatitudeSlave3 +
            "," +
            SelectedLongitudeSlave3 +
            "," +
            SelectedElevationSlave3 +
            "," +
            SelectedPoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedLatitudeSlave3 == "" || SelectedLatitudeSlave3 == null ||
            SelectedLongitudeSlave3 == "" || SelectedLongitudeSlave3 == null ||
            SelectedElevationSlave3 == "" || SelectedElevationSlave3 == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedSlave3Name == "" || SelectedSlave3Name == null ||
            SelectedSlave3AntennaIPAddress == "" || SelectedSlave3AntennaIPAddress == null

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
        else if (!MainPageMasterModel.IsValidIPAddress(SelectedSlave3AntennaIPAddress))
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
            "S3," +
            Slave3AntennaIPAddress +
            "," +
            Slave3AntennaName +
            "," +
            Slave3Name +
            "," +
            LatitudeSlave3 +
            "," +
            LongitudeSlave3 +
            "," +
            ElevationSlave3 +
            "," +
            PoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            LatitudeSlave3 == "" || LatitudeSlave3 == null ||
            LongitudeSlave3 == "" || LongitudeSlave3 == null ||
            Slave3Name == "" || Slave3Name == null ||
            ElevationSlave3 == "" || ElevationSlave3 == null ||
            PoleHeight == "" || PoleHeight == null ||
            Slave3AntennaIPAddress == "" || Slave3AntennaIPAddress == null
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
        else if (!MainPageMasterModel.IsValidIPAddress(Slave3AntennaIPAddress))
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

    // Properties for Slave3
    private string latitudeSlave3 = "";
    public string LatitudeSlave3
    {
        get => latitudeSlave3;
        set
        {
            latitudeSlave3 = value;
            OnPropertyChanged(nameof(LatitudeSlave3));
            OnPropertyChanged(nameof(ElevationSlave3));
        }
    }

    private string longitudeSlave3 = "";
    public string LongitudeSlave3
    {
        get => longitudeSlave3;
        set
        {
            longitudeSlave3 = value;
            OnPropertyChanged(nameof(LongitudeSlave3));
            OnPropertyChanged(nameof(ElevationSlave3));
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
            OnPropertyChanged(nameof(ElevationSlave3));
        }
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private string elevationSlave3 = "";
    public string ElevationSlave3
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
                        LatitudeSlave3.Length >= 2 &&
                        LongitudeSlave3.Length >= 3 &&
                        float.Parse(LatitudeSlave3) >= 20.25f &&
                        float.Parse(LatitudeSlave3) <= 45.35f &&
                        float.Parse(LongitudeSlave3) >= 122.523f &&
                        float.Parse(LongitudeSlave3) <= 153.523f
                    )
                    {
                        //elevationSlave3 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave3), float.Parse(LongitudeSlave3)).ToString();
                    }
                    else
                    {
                        elevationSlave3 = "-999.9";
                    }
                }
            }
            return elevationSlave3;
        }
        set
        {
            if (!IsElevationEnabled)
            {
                if
                (
                    LatitudeSlave3.Length >= 2 &&
                    LongitudeSlave3.Length >= 3 &&
                    float.Parse(LatitudeSlave3) >= 20.25f &&
                    float.Parse(LatitudeSlave3) <= 45.35f &&
                    float.Parse(LongitudeSlave3) >= 122.523f &&
                    float.Parse(LongitudeSlave3) <= 153.523f
                )
                {
                    //elevationSlave3 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave3), float.Parse(LongitudeSlave3)).ToString();
                }
                else
                {
                    elevationSlave3 = "-999.9";
                }
            }
            else
            {
                elevationSlave3 = value;
            }
            OnPropertyChanged(nameof(ElevationSlave3));
        }
    }

    private string slave3Name = "";
    public string Slave3Name
    {
        get => slave3Name;
        set
        {
            slave3Name = value;
            OnPropertyChanged(nameof(Slave3Name));
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

    private string slave3AntennaIPAddress = "";
    public string Slave3AntennaIPAddress
    {
        get => slave3AntennaIPAddress;
        set
        {
            slave3AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave3AntennaIPAddress));
            CheckIPAvailability(Slave3AntennaIPAddress);
        }
    }

    private string slave3AntennaName = "*";
    public string Slave3AntennaName
    {
        get => slave3AntennaName;
        set
        {
            slave3AntennaName = value;
            OnPropertyChanged(nameof(Slave3AntennaName));
        }
    }

    // Selected properties for Slave3
    private string selectedSlave3Name = "";
    public string SelectedSlave3Name
    {
        get => selectedSlave3Name;
        set
        {
            selectedSlave3Name = value;
            OnPropertyChanged(nameof(SelectedSlave3Name));
        }
    }

    private string selectedLatitudeSlave3 = "";
    public string SelectedLatitudeSlave3
    {
        get => selectedLatitudeSlave3;
        set
        {
            selectedLatitudeSlave3 = value;
            OnPropertyChanged(nameof(SelectedLatitudeSlave3));
        }
    }

    private string selectedLongitudeSlave3 = "";
    public string SelectedLongitudeSlave3
    {
        get => selectedLongitudeSlave3;
        set
        {
            selectedLongitudeSlave3 = value;
            OnPropertyChanged(nameof(SelectedLongitudeSlave3));
        }
    }

    private string selectedElevationSlave3 = "";
    public string SelectedElevationSlave3
    {
        get => selectedElevationSlave3;
        set
        {
            selectedElevationSlave3 = value;
            OnPropertyChanged(nameof(SelectedElevationSlave3));
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

    private string selectedSlave3AntennaIPAddress = "";
    public string SelectedSlave3AntennaIPAddress
    {
        get => selectedSlave3AntennaIPAddress;
        set
        {
            selectedSlave3AntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedSlave3AntennaIPAddress));
        }
    }

    private string selectedSlave3AntennaName = "";
    public string SelectedSlave3AntennaName
    {
        get => selectedSlave3AntennaName;
        set
        {
            selectedSlave3AntennaName = value;
            OnPropertyChanged(nameof(SelectedSlave3AntennaName));
        }
    }

    public bool IsIPAddressShowSub3
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
                    return _mainPageMasterViewModel.IsIPAddressShowSub3;
                }
            }
            return false;
        }
        set
        {
            if (Slave3AntennaIPAddress == _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub1 = value;
                IsIPAddressShowSubPosition = 1;
            }
            else if (Slave3AntennaIPAddress == _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub2 = value;
                IsIPAddressShowSubPosition = 2;
            }
            else if (Slave3AntennaIPAddress == _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub3 = value;
                IsIPAddressShowSubPosition = 3;
            }
            else if (Slave3AntennaIPAddress == _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub4 = value;
                IsIPAddressShowSubPosition = 4;
            }
            else
            {
                _mainPageMasterViewModel.IsIPAddressShowSub3 = value;
            }
            OnPropertyChanged(nameof(IsIPAddressShowSub3));
        }
    }

    public bool IsNameShowSub3
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
            if (Slave3AntennaName == _mainPageMasterViewModel.Slave1JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub1 = value;
                IsJapaneseNameShowSubPosition = 1;
            }
            else if (Slave3AntennaName == _mainPageMasterViewModel.Slave2JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub2 = value;
                IsJapaneseNameShowSubPosition = 2;
            }
            else if (Slave3AntennaName == _mainPageMasterViewModel.Slave3JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub3 = value;
                IsJapaneseNameShowSubPosition = 3;
            }
            else if (Slave3AntennaName == _mainPageMasterViewModel.Slave4JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub4 = value;
                IsJapaneseNameShowSubPosition = 4;
            }
            OnPropertyChanged(nameof(IsNameShowSub3));
        }
    }
}
