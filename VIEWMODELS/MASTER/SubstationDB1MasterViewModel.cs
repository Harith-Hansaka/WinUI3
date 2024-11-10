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

public class SubstationDB1MasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    SubstationDB1MasterModel newSubstationDB1MasterModel;
    MainPageMasterViewModel _mainPageMasterViewModel;
    int substationDB1MasterModelCount = 0;
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

    public SubstationDB1MasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        SubstationDB1MasterModel = new ObservableCollection<SubstationDB1MasterModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        DBExportCommand = new AsyncRelayCommand(() => ExportStationDBPageModelMasterToCsvAsync(substationDB1MasterModel));
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

    private ObservableCollection<SubstationDB1MasterModel> substationDB1MasterModel;
    public ObservableCollection<SubstationDB1MasterModel> SubstationDB1MasterModel
    {
        get { return substationDB1MasterModel; }
        set
        {
            substationDB1MasterModel = value;
            OnPropertyChanged(nameof(SubstationDB1MasterModel));
        }
    }

    private SubstationDB1MasterModel _selectedSubstationDB1MasterModel;
    public SubstationDB1MasterModel SelectedSubstationDB1MasterModel
    {
        get { return _selectedSubstationDB1MasterModel; }
        set
        {
            _selectedSubstationDB1MasterModel = value;
            if (SelectedSubstationDB1MasterModel != null)
            {
                SelectedSlave1Name = SelectedSubstationDB1MasterModel.Name;
                SelectedLatitudeSlave1 = SelectedSubstationDB1MasterModel.Latitude;
                SelectedLongitudeSlave1 = SelectedSubstationDB1MasterModel.Longitude;
                SelectedElevationSlave1 = SelectedSubstationDB1MasterModel.Elevation;
                SelectedPoleHeight = SelectedSubstationDB1MasterModel.PoleLength;
                SelectedSlave1AntennaIPAddress = SelectedSubstationDB1MasterModel.Slave1AntennaIPAddress;
                SelectedSlave1AntennaName = SelectedSubstationDB1MasterModel.Slave1AntennaName;
            }
            OnPropertyChanged(nameof(SelectedSubstationDB1MasterModel));
        }
    }

    public void AddStationDBPage
    (
        string Slave1AntennaIPAddress,
        string Slave1AntennaName,
        string Name,
        string Latitude,
        string Longitude,
        string Elevation,
        string PoleLength,
        string Date,
        String Time
    )
    {
        if (substationDB1MasterModelCount < 15 || substationDB1MasterModel.Count < 15)
        {
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave1AntennaName == "" &
                Slave1AntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                newSubstationDB1MasterModel = new SubstationDB1MasterModel
                {
                    ID = null,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave1AntennaIPAddress = Slave1AntennaIPAddress,
                    Slave1AntennaName = Slave1AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB1MasterModelCount++;
                SubstationDB1MasterModel.Add(newSubstationDB1MasterModel);
            }
            else
            {
                newSubstationDB1MasterModel = new SubstationDB1MasterModel
                {
                    ID = substationDB1MasterModelCount + 1,
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave1AntennaIPAddress = Slave1AntennaIPAddress,
                    Slave1AntennaName = Slave1AntennaName,
                    Date = Date,
                    Time = Time
                };

                substationDB1MasterModelCount++;
                SubstationDB1MasterModel.Add(newSubstationDB1MasterModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = substationDB1MasterModelCount % 15;
            if
            (
                Name == "" &
                Latitude == "" &
                Longitude == "" &
                Elevation == "" &
                PoleLength == "" &
                Slave1AntennaName == "" &
                Slave1AntennaIPAddress == "" &
                Date == "" &
                Time == ""
            )
            {
                SubstationDB1MasterModel[indexToUpdate] = new SubstationDB1MasterModel
                {
                    ID = null,  // Update ID to match the new entry position
                    Name = Name,
                    Latitude = Latitude,
                    Longitude = Longitude,
                    Elevation = Elevation,
                    PoleLength = PoleLength,
                    Slave1AntennaIPAddress = Slave1AntennaIPAddress,
                    Slave1AntennaName = Slave1AntennaName,
                    Date = Date,
                    Time = Time
                };
            }
            else
            {
                int objCount1 = 0;
                foreach (SubstationDB1MasterModel _substationDB1MasterModel in substationDB1MasterModel)
                {
                    if (IsObjectEmptyOrNull(_substationDB1MasterModel))
                    {
                        break;
                    }
                    objCount1++;
                }
                if (objCount1 < 15)
                {
                    SubstationDB1MasterModel[objCount1] = new SubstationDB1MasterModel
                    {
                        ID = objCount1 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave1AntennaIPAddress = Slave1AntennaIPAddress,
                        Slave1AntennaName = Slave1AntennaName,
                        Date = Date,
                        Time = Time
                    };
                }
                else
                {
                    SubstationDB1MasterModel[objCount % 15] = new SubstationDB1MasterModel
                    {
                        ID = objCount % 15 + 1,  // Update ID to match the new entry position
                        Name = Name,
                        Latitude = Latitude,
                        Longitude = Longitude,
                        Elevation = Elevation,
                        PoleLength = PoleLength,
                        Slave1AntennaIPAddress = Slave1AntennaIPAddress,
                        Slave1AntennaName = Slave1AntennaName,
                        Date = Date,
                        Time = Time
                    };
                    objCount++;
                }
                if (canAdd2DB)
                {
                    try
                    {
                        IPAddress.Parse(Slave1AntennaIPAddress);
                        SaveDataBase();
                    }
                    catch (FormatException ex)
                    {
                    }
                }
                canAdd2DB = false;
            }
            substationDB1MasterModelCount++;
        }
    }

    public void DeleteSelectedItem()
    {
        if (SelectedSubstationDB1MasterModel != null)
        {
            SubstationDB1MasterModel.Remove(SelectedSubstationDB1MasterModel);
            AddStationDBPage
            (
                "", "", "", "", "", "", "", "", ""
            );
            SaveDataBase();
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedSubstationDB1MasterModel != null;
    }

    public void SaveDataBase()
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{Slave1AntennaIPAddress}.txt";

        // Clear the contents of the file by writing an empty string
        File.WriteAllText(filePath, string.Empty);

        foreach (var station in substationDB1MasterModel)
        {
            if (!IsObjectEmptyOrNull(station as SubstationDB1MasterModel))
            {
                try
                {
                    IPAddress.Parse(Slave1AntennaIPAddress);
                    File.AppendAllText(filePath,
                        $"{station.Slave1AntennaIPAddress}," +
                        $"{station.Slave1AntennaName}," +
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

    public bool IsObjectEmptyOrNull(SubstationDB1MasterModel model)
    {
        // Get all the public properties of the model
        var properties = typeof(SubstationDB1MasterModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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

    public async Task ExportStationDBPageModelMasterToCsvAsync(ObservableCollection<SubstationDB1MasterModel> substationDB1MasterModel)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = baseDirectory + "SubstationDB1Export.csv";
        var sb = new StringBuilder();

        // Define the column headers based on the properties of SubstationDB1MasterModel
        sb.AppendLine
            (
                "ID," +
                "Slave1AntennaIPAddress," +
                "Slave1AntennaName," +
                "Name," +
                "Latitude," +
                "Longitude," +
                "Elevation," +
                "PoleLength," +
                "Date," +
                "Time"
            );

        // Iterate over the collection to get the row data
        foreach (var station in substationDB1MasterModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Slave1AntennaIPAddress}," +
                    $"{station.Slave1AntennaName}," +
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
            "S1," +
            SelectedSlave1AntennaIPAddress +
            "," +
            SelectedSlave1AntennaName +
            "," +
            SelectedSlave1Name +
            "," +
            SelectedLatitudeSlave1 +
            "," +
            SelectedLongitudeSlave1 +
            "," +
            SelectedElevationSlave1 +
            "," +
            SelectedPoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            SelectedLatitudeSlave1 == "" || SelectedLatitudeSlave1 == null ||
            SelectedLongitudeSlave1 == "" || SelectedLongitudeSlave1 == null ||
            SelectedElevationSlave1 == "" || SelectedElevationSlave1 == null ||
            SelectedPoleHeight == "" || SelectedPoleHeight == null ||
            SelectedSlave1Name == "" || SelectedSlave1Name == null ||
            SelectedSlave1AntennaIPAddress == "" || SelectedSlave1AntennaIPAddress == null
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
        else if
        (!MainPageMasterModel.IsValidIPAddress(SelectedSlave1AntennaIPAddress))
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
            "S1," +
            Slave1AntennaIPAddress +
            "," +
            Slave1AntennaName +
            "," +
            Slave1Name +
            "," +
            LatitudeSlave1 +
            "," +
            LongitudeSlave1 +
            "," +
            ElevationSlave1 +
            "," +
            PoleHeight +
            "," +
            DateTime.Now.Date.ToString("yyyy-MM-dd") +
            "," +
            DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss") +
            ",\n";

        if
        (
            LatitudeSlave1 == "" || LatitudeSlave1 == null ||
            Slave1Name == "" || Slave1Name == null ||
            LongitudeSlave1 == "" || LongitudeSlave1 == null ||
            ElevationSlave1 == "" || ElevationSlave1 == null ||
            PoleHeight == "" || PoleHeight == null ||
            Slave1AntennaIPAddress == "" || Slave1AntennaIPAddress == null
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
        (!MainPageMasterModel.IsValidIPAddress(Slave1AntennaIPAddress))
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
            SubstationRegistrationMasterNavigateCommand.Execute(null);
        }
    }

    void CheckIPAvailability(string _slave1AntennaIPAddress)
    {
        // Specify the file path for the CSV output
        string baseDirectory = AppContext.BaseDirectory;
        string filePath = $"{baseDirectory}{_slave1AntennaIPAddress}.txt";
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

    private string latitudeSlave1 = "";
    public string LatitudeSlave1
    {
        get
        {
            return latitudeSlave1;
        }

        set
        {
            latitudeSlave1 = value;
            OnPropertyChanged(nameof(LatitudeSlave1));
            OnPropertyChanged(nameof(ElevationSlave1));
        }
    }

    private string longitudeSlave1 = "";
    public string LongitudeSlave1
    {
        get
        {
            return longitudeSlave1;
        }
        set
        {
            longitudeSlave1 = value;
            OnPropertyChanged(nameof(LongitudeSlave1));
            OnPropertyChanged(nameof(ElevationSlave1));
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
            OnPropertyChanged(nameof(ElevationSlave1));
        }
    }

    private string elevationInabilityShow;
    public string ElevationInabilityShow
    {
        get => IsElevationEnabled ? "#FFFFFF" : "#50FFFFFF";
    }

    private string elevationSlave1 = "";
    public string ElevationSlave1
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
                        LatitudeSlave1.Length >= 2 &&
                        LongitudeSlave1.Length >= 3 &&
                        float.Parse(LatitudeSlave1) >= 20.25f &&
                        float.Parse(LatitudeSlave1) <= 45.35f &&
                        float.Parse(LongitudeSlave1) >= 122.523f &&
                        float.Parse(LongitudeSlave1) <= 153.523f
                    )
                    {
                        //elevationSlave1 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave1), float.Parse(LongitudeSlave1)).ToString();
                    }
                    else
                    {
                        elevationSlave1 = "-999.9";
                    }
                }
            }
            return elevationSlave1;
        }
        set
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
                        LatitudeSlave1.Length >= 2 &&
                        LongitudeSlave1.Length >= 3 &&
                        float.Parse(LatitudeSlave1) >= 20.25f &&
                        float.Parse(LatitudeSlave1) <= 45.35f &&
                        float.Parse(LongitudeSlave1) >= 122.523f &&
                        float.Parse(LongitudeSlave1) <= 153.523f
                    )
                    {
                        //elevationSlave1 = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave1), float.Parse(LongitudeSlave1)).ToString();
                    }
                    else
                    {
                        elevationSlave1 = "-999.9";
                    }
                }
                else
                {
                    elevationSlave1 = value;
                }
            }
            OnPropertyChanged(nameof(ElevationSlave1));
        }
    }

    private string slave1Name = "";
    public string Slave1Name
    {
        get
        {
            return slave1Name;
        }
        set
        {
            slave1Name = value;
            OnPropertyChanged(nameof(Slave1Name));
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

    private string slave1AntennaIPAddress = "";
    public string Slave1AntennaIPAddress
    {
        get => slave1AntennaIPAddress;
        set
        {
            slave1AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave1AntennaIPAddress));
            CheckIPAvailability(Slave1AntennaIPAddress);
        }
    }

    private string slave1AntennaName = "*";
    public string Slave1AntennaName
    {
        get => slave1AntennaName;
        set
        {
            slave1AntennaName = value;
            OnPropertyChanged(nameof(Slave1AntennaName));
        }
    }

    private string selectedSlave1Name = "";
    public string SelectedSlave1Name
    {
        get
        {
            return selectedSlave1Name;
        }
        set
        {
            selectedSlave1Name = value;
            OnPropertyChanged(nameof(SelectedSlave1Name));
        }
    }

    private string selectedElevationSlave1 = "";
    public string SelectedElevationSlave1
    {
        get
        {
            return selectedElevationSlave1;
        }
        set
        {
            selectedElevationSlave1 = value;
            OnPropertyChanged(nameof(SelectedElevationSlave1));
        }
    }

    private string selectedLongitudeSlave1 = "";
    public string SelectedLongitudeSlave1
    {
        get
        {
            return selectedLongitudeSlave1;
        }
        set
        {
            selectedLongitudeSlave1 = value;
            OnPropertyChanged(nameof(SelectedLongitudeSlave1));
        }
    }

    private string selectedLatitudeSlave1 = "";
    public string SelectedLatitudeSlave1
    {
        get
        {
            return selectedLatitudeSlave1;
        }
        set
        {
            selectedLatitudeSlave1 = value;
            OnPropertyChanged(nameof(SelectedLatitudeSlave1));
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

    private string selectedSlave1AntennaIPAddress = "";
    public string SelectedSlave1AntennaIPAddress
    {
        get
        {
            return selectedSlave1AntennaIPAddress;
        }
        set
        {
            selectedSlave1AntennaIPAddress = value;
            OnPropertyChanged(nameof(SelectedSlave1AntennaIPAddress));
        }
    }

    private string selectedSlave1AntennaName = "";
    public string SelectedSlave1AntennaName
    {
        get
        {
            return selectedSlave1AntennaName;
        }
        set
        {
            selectedSlave1AntennaName = value;
            OnPropertyChanged(nameof(SelectedSlave1AntennaName));
        }
    }

    public bool IsIPAddressShowSub1
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
                    return _mainPageMasterViewModel.IsIPAddressShowSub1;
                }
            }
            return false;
        }
        set
        {
            if (Slave1AntennaIPAddress == _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub1 = value;
                IsIPAddressShowSubPosition = 1;
            }
            else if (Slave1AntennaIPAddress == _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub2 = value;
                IsIPAddressShowSubPosition = 2;
            }
            else if (Slave1AntennaIPAddress == _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub3 = value;
                IsIPAddressShowSubPosition = 3;
            }
            else if (Slave1AntennaIPAddress == _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText.Split(',')[1])
            {
                _mainPageMasterViewModel.IsIPAddressShowSub4 = value;
                IsIPAddressShowSubPosition = 4;
            }
            else
            {
                _mainPageMasterViewModel.IsIPAddressShowSub1 = value;
            }
            OnPropertyChanged(nameof(IsIPAddressShowSub1));
        }
    }

    public bool IsNameShowSub1
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
            if (Slave1AntennaName == _mainPageMasterViewModel.Slave1JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub1 = value;
                IsJapaneseNameShowSubPosition = 1;
            }
            else if (Slave1AntennaName == _mainPageMasterViewModel.Slave2JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub2 = value;
                IsJapaneseNameShowSubPosition = 2;
            }
            else if (Slave1AntennaName == _mainPageMasterViewModel.Slave3JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub3 = value;
                IsJapaneseNameShowSubPosition = 3;
            }
            else if (Slave1AntennaName == _mainPageMasterViewModel.Slave4JapaneseNameBackupText.Split(',')[0])
            {
                _mainPageMasterViewModel.IsNameShowSub4 = value;
                IsJapaneseNameShowSubPosition = 4;
            }
            OnPropertyChanged(nameof(IsNameShowSub1));
        }
    }
}
