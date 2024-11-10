using Microsoft.UI.Xaml;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.MODELS.MASTER;

namespace UNDAI.VIEWMODELS.MASTER;

public class SubstationMasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    App app;
    public MainPageMasterViewModel? _mainPageMasterViewModel;
    //ElevationProfileGenerateModel _elevationProfileGenerateModel;
    //LineSeries _lineSeries;
    //LinearAxis xAxis;
    //LinearAxis yAxis;
    //public bool plotGenerated = false;
    //List<float> elevation;
    //public float distanceBetweenTwoPoints;

    public ICommand MainPageMasterNavigateCommand { get; }
    public ICommand SubstationDB1PageMasterNavigateCommand { get; }
    public ICommand SubstationDB2PageMasterNavigateCommand { get; }
    public ICommand SubstationDB3PageMasterNavigateCommand { get; }
    public ICommand SubstationDB4PageMasterNavigateCommand { get; }
    public ICommand SlaveDataRegisterMasterCommand { get; }
    public ICommand Slave1ElevationProfileCommand { get; }

    string _message;
    string _message1;
    string _message2;
    string _message3;
    string _message4;

    public string firstLatitude = "";
    public string firstLongitude = "";
    public string secondLatitude = "";
    public string secondLongitude = "";
    public string backupFirstLatitude;
    public string backupFirstLongitude;
    public string backupSecondLatitude;
    public string backupSecondLongitude;

    public SubstationMasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster
    )
    {
        _navigationService = navigationService;
        app = (App)Application.Current;
        CreateAppClassAfterDelay();

        _message = string.Empty;  // Initialize to an empty string or default value
        _message1 = string.Empty; // Initialize to an empty string or default value
        _message2 = string.Empty; // Initialize to an empty string or default value
        _message3 = string.Empty; // Initialize to an empty string or default value
        _message4 = string.Empty; // Initialize to an empty string or default value
    }

    private async void CreateAppClassAfterDelay()
    {
        await Task.Delay(100);
        if (_mainPageMasterViewModel == null)
        {
            // Access properties directly
            _mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
    }

    // Slave Name Properties
    private string slave1Name = "";
    public string Slave1Name
    {
        get { return slave1Name; }
        set
        {
            slave1Name = value;
            OnPropertyChanged(nameof(Slave1Name));
        }
    }

    private string slave2Name = "";
    public string Slave2Name
    {
        get { return slave2Name; }
        set
        {
            slave2Name = value;
            OnPropertyChanged(nameof(Slave2Name));
        }
    }

    private string slave3Name = "";
    public string Slave3Name
    {
        get { return slave3Name; }
        set
        {
            slave3Name = value;
            OnPropertyChanged(nameof(Slave3Name));
        }
    }

    private string slave4Name = "";
    public string Slave4Name
    {
        get { return slave4Name; }
        set
        {
            slave4Name = value;
            OnPropertyChanged(nameof(Slave4Name));
        }
    }

    // Latitude Properties for Each Slave
    private string latitudeSlave1 = "";
    public string LatitudeSlave1
    {
        get { return latitudeSlave1; }
        set
        {
            latitudeSlave1 = value;
            OnPropertyChanged(nameof(LatitudeSlave1));
        }
    }

    private string latitudeSlave2 = "";
    public string LatitudeSlave2
    {
        get { return latitudeSlave2; }
        set
        {
            latitudeSlave2 = value;
            OnPropertyChanged(nameof(LatitudeSlave2));
        }
    }

    private string latitudeSlave3 = "";
    public string LatitudeSlave3
    {
        get { return latitudeSlave3; }
        set
        {
            latitudeSlave3 = value;
            OnPropertyChanged(nameof(LatitudeSlave3));
        }
    }

    private string latitudeSlave4 = "";
    public string LatitudeSlave4
    {
        get { return latitudeSlave4; }
        set
        {
            latitudeSlave4 = value;
            OnPropertyChanged(nameof(LatitudeSlave4));
        }
    }

    // Longitude Properties for Each Slave
    private string longitudeSlave1 = "";
    public string LongitudeSlave1
    {
        get { return longitudeSlave1; }
        set
        {
            longitudeSlave1 = value;
            OnPropertyChanged(nameof(LongitudeSlave1));
        }
    }

    private string longitudeSlave2 = "";
    public string LongitudeSlave2
    {
        get { return longitudeSlave2; }
        set
        {
            longitudeSlave2 = value;
            OnPropertyChanged(nameof(LongitudeSlave2));
        }
    }

    private string longitudeSlave3 = "";
    public string LongitudeSlave3
    {
        get { return longitudeSlave3; }
        set
        {
            longitudeSlave3 = value;
            OnPropertyChanged(nameof(LongitudeSlave3));
        }
    }

    private string longitudeSlave4 = "";
    public string LongitudeSlave4
    {
        get { return longitudeSlave4; }
        set
        {
            longitudeSlave4 = value;
            OnPropertyChanged(nameof(LongitudeSlave4));
        }
    }

    // Elevation Properties for Each Slave
    private string elevationSlave1 = "";
    public string ElevationSlave1
    {
        get { return elevationSlave1; }
        set
        {
            elevationSlave1 = value;
            OnPropertyChanged(nameof(ElevationSlave1));
        }
    }

    private string elevationSlave2 = "";
    public string ElevationSlave2
    {
        get { return elevationSlave2; }
        set
        {
            elevationSlave2 = value;
            OnPropertyChanged(nameof(ElevationSlave2));
        }
    }

    private string elevationSlave3 = "";
    public string ElevationSlave3
    {
        get { return elevationSlave3; }
        set
        {
            elevationSlave3 = value;
            OnPropertyChanged(nameof(ElevationSlave3));
        }
    }

    private string elevationSlave4 = "";
    public string ElevationSlave4
    {
        get { return elevationSlave4; }
        set
        {
            elevationSlave4 = value;
            OnPropertyChanged(nameof(ElevationSlave4));
        }
    }

    // Pole Length Properties for Each Slave
    private string poleLengthSlave1 = "";
    public string PoleLengthSlave1
    {
        get { return poleLengthSlave1; }
        set
        {
            poleLengthSlave1 = value;
            OnPropertyChanged(nameof(PoleLengthSlave1));
        }
    }

    private string poleLengthSlave2 = "";
    public string PoleLengthSlave2
    {
        get { return poleLengthSlave2; }
        set
        {
            poleLengthSlave2 = value;
            OnPropertyChanged(nameof(PoleLengthSlave2));
        }
    }

    private string poleLengthSlave3 = "";
    public string PoleLengthSlave3
    {
        get { return poleLengthSlave3; }
        set
        {
            poleLengthSlave3 = value;
            OnPropertyChanged(nameof(PoleLengthSlave3));
        }
    }

    private string poleLengthSlave4 = "";
    public string PoleLengthSlave4
    {
        get { return poleLengthSlave4; }
        set
        {
            poleLengthSlave4 = value;
            OnPropertyChanged(nameof(PoleLengthSlave4));
        }
    }

    // Antenna IPAddress Properties for Each Slave
    private string slave1AntennaIPAddress = "";
    public string Slave1AntennaIPAddress
    {
        get { return slave1AntennaIPAddress; }
        set
        {
            slave1AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave1AntennaIPAddress));
        }
    }

    private string slave2AntennaIPAddress = "";
    public string Slave2AntennaIPAddress
    {
        get { return slave2AntennaIPAddress; }
        set
        {
            slave2AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave2AntennaIPAddress));
        }
    }

    private string slave3AntennaIPAddress = "";
    public string Slave3AntennaIPAddress
    {
        get { return slave3AntennaIPAddress; }
        set
        {
            slave3AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave3AntennaIPAddress));
        }
    }

    private string slave4AntennaIPAddress = "";
    public string Slave4AntennaIPAddress
    {
        get { return slave4AntennaIPAddress; }
        set
        {
            slave4AntennaIPAddress = value;
            OnPropertyChanged(nameof(Slave4AntennaIPAddress));
        }
    }

    // Antenna IP Address Properties for Each Slave
    private string antennaNameSlave1 = "";
    public string AntennaNameSlave1
    {
        get { return antennaNameSlave1; }
        set
        {
            antennaNameSlave1 = value;
            OnPropertyChanged(nameof(AntennaNameSlave1));
        }
    }

    private string antennaNameSlave2 = "";
    public string AntennaNameSlave2
    {
        get { return antennaNameSlave2; }
        set
        {
            antennaNameSlave2 = value;
            OnPropertyChanged(nameof(AntennaNameSlave2));
        }
    }

    private string antennaNameSlave3 = "";
    public string AntennaNameSlave3
    {
        get { return antennaNameSlave3; }
        set
        {
            antennaNameSlave3 = value;
            OnPropertyChanged(nameof(AntennaNameSlave3));
        }
    }

    private string antennaNameSlave4 = "";
    public string AntennaNameSlave4
    {
        get { return antennaNameSlave4; }
        set
        {
            antennaNameSlave4 = value;
            OnPropertyChanged(nameof(AntennaNameSlave4));
        }
    }

    public void AddDataToSlave1
    (
        string slave1AntennaIPAddress,
        string slave1AntennaName,
        string slave1Latitude,
        string slave1Longitude,
        string slave1Elevation,
        string slave1PoleLength,
        string slave1Name = "UNKNOWN"
    )
    {
        Slave1Name = slave1Name;
        LatitudeSlave1 = slave1Latitude;
        LongitudeSlave1 = slave1Longitude;
        ElevationSlave1 = slave1Elevation;
        PoleLengthSlave1 = slave1PoleLength;
        Slave1AntennaIPAddress = slave1AntennaIPAddress;
        AntennaNameSlave1 = slave1AntennaName;
    }

    public void AddDataToSlave2
    (
        string slave2AntennaIPAddress,
        string slave2AntennaName,
        string slave2Latitude,
        string slave2Longitude,
        string slave2Elevation,
        string slave2PoleLength,
        string slave2Name = "UNKNOWN"
    )
    {
        Slave2Name = slave2Name;
        LatitudeSlave2 = slave2Latitude;
        LongitudeSlave2 = slave2Longitude;
        ElevationSlave2 = slave2Elevation;
        PoleLengthSlave2 = slave2PoleLength;
        Slave2AntennaIPAddress = slave2AntennaIPAddress;
        AntennaNameSlave2 = slave2AntennaName;
    }

    public void AddDataToSlave3
    (
        string slave3AntennaIPAddress,
        string slave3AntennaName,
        string slave3Latitude,
        string slave3Longitude,
        string slave3Elevation,
        string slave3PoleLength,
        string slave3Name = "UNKNOWN"
    )
    {
        Slave3Name = slave3Name;
        LatitudeSlave3 = slave3Latitude;
        LongitudeSlave3 = slave3Longitude;
        ElevationSlave3 = slave3Elevation;
        PoleLengthSlave3 = slave3PoleLength;
        Slave3AntennaIPAddress = slave3AntennaIPAddress;
        AntennaNameSlave3 = slave3AntennaName;
    }

    public void AddDataToSlave4
    (
        string slave4AntennaIPAddress,
        string slave4AntennaName,
        string slave4Latitude,
        string slave4Longitude,
        string slave4Elevation,
        string slave4PoleLength,
        string slave4Name = "UNKNOWN"
    )
    {
        Slave4Name = slave4Name;
        LatitudeSlave4 = slave4Latitude;
        LongitudeSlave4 = slave4Longitude;
        ElevationSlave4 = slave4Elevation;
        PoleLengthSlave4 = slave4PoleLength;
        Slave4AntennaIPAddress = slave4AntennaIPAddress;
        AntennaNameSlave4 = slave4AntennaName;
    }

    public void ShowElevationProfile(string lat1, string long1, string lat2, string long2)
    {
        firstLatitude = lat1;
        firstLongitude = long1;
        secondLatitude = lat2;
        secondLongitude = long2;
    }

    public void GPSMapper(string lat1, string long1, string lat2, string long2)
    {
        firstLatitude = lat1;
        firstLongitude = long1;
        secondLatitude = lat2;
        secondLongitude = long2;
    }
}
