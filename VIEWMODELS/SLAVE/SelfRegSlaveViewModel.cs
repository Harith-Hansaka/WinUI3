using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.MODELS.SLAVE;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.SLAVE;

public class SelfRegSlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private bool isLatLongEnabled;
    public bool IsLatLongEnabled
    {
        get => isLatLongEnabled;
        set
        {
            isLatLongEnabled = value;
            LatLongEnable = value;
            if (!isElevationEnabled)
            {
                LatitudeSlave = _mainPageSlaveViewModel.Latitude102;
                LongitudeSlave = _mainPageSlaveViewModel.Longitude103;
            }
            if (isLatLongEnabled)
            {
                latLongEnabledFirst = true;
                LatitudeSlave = "";
                LongitudeSlave = "";
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

            OnPropertyChanged(nameof(IsElevationEnabled));

            if (!isElevationEnabled)
            {
                ElevationSlave = "";
            }
            if (isElevationEnabled)
            {
                elevationSlaveEnabledFirst = true;
            }
            OnPropertyChanged(nameof(ElevationInabilityShow));
            OnPropertyChanged(nameof(ElevationSlave));
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

    bool latLongEnabledFirst = false;
    private string latitudeSlave = "";
    public string LatitudeSlave
    {
        get
        {
            if (!IsLatLongEnabled)
            {
                latitudeSlave = _mainPageSlaveViewModel.Latitude102;
            }
            return latitudeSlave;
        }
        set
        {
            latitudeSlave = value;
            if (IsLatLongEnabled && latLongEnabledFirst)
            {
                latitudeSlave = _mainPageSlaveViewModel.SlaveLongitudeTextBackup;
            }
            OnPropertyChanged(nameof(LatitudeSlave));
            if (!isElevationEnabled)
            {
                ElevationSlave = "";
            }
        }
    }

    private string longitudeSlave = "";
    public string LongitudeSlave
    {
        get
        {
            if (!IsLatLongEnabled)
            {
                longitudeSlave = _mainPageSlaveViewModel.Longitude103;
            }
            return longitudeSlave;
        }
        set
        {
            longitudeSlave = value;
            if (IsLatLongEnabled && latLongEnabledFirst)
            {
                longitudeSlave = _mainPageSlaveViewModel.SlaveLongitudeTextBackup;
                latLongEnabledFirst = false;
            }
            OnPropertyChanged(nameof(LongitudeSlave));
            if (!isElevationEnabled)
            {
                ElevationSlave = "";
            }
        }
    }

    bool elevationSlaveEnabledFirst = false;
    private string elevationSlave = "";
    public string ElevationSlave
    {
        get
        {
            if (IsElevationEnabled && elevationSlaveEnabledFirst)
            {
                elevationSlave = _mainPageSlaveViewModel.SlaveElevationTextBackup;
                elevationSlaveEnabledFirst = false;
            }
            if (!IsElevationEnabled)
            {
                if
                (
                LatitudeSlave.Length >= 2 &&
                LongitudeSlave.Length >= 3 &&
                float.Parse(LatitudeSlave) >= 20.25f &&
                float.Parse(LatitudeSlave) <= 45.35f &&
                float.Parse(LongitudeSlave) >= 122.523f &&
                float.Parse(LongitudeSlave) <= 153.523f
                )
                {
                    //elevationSlave = _mainPageSlaveViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave), float.Parse(LongitudeSlave)).ToString();
                }
                else
                {
                    elevationSlave = "-999.9";
                }
            }
            return elevationSlave;
        }
        set
        {
            if (IsElevationEnabled && elevationSlaveEnabledFirst)
            {
                elevationSlave = _mainPageSlaveViewModel.SlaveElevationTextBackup;
                elevationSlaveEnabledFirst = false;
            }
            else if (IsElevationEnabled)
            {
                elevationSlave = value;
            }
            else if (!IsElevationEnabled)
            {
                if
                (
                LatitudeSlave.Length >= 2 &&
                LongitudeSlave.Length >= 3 &&
                float.Parse(LatitudeSlave) >= 20.25f &&
                float.Parse(LatitudeSlave) <= 45.35f &&
                float.Parse(LongitudeSlave) >= 122.523f &&
                float.Parse(LongitudeSlave) <= 153.523f
                )
                {
                    //elevationSlave = _mainPageSlaveViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeSlave), float.Parse(LongitudeSlave)).ToString();
                }
                else
                {
                    elevationSlave = "-999.9";
                }
            }
            OnPropertyChanged(nameof(ElevationSlave));
        }
    }

    private string slaveName = "";
    public string SlaveName
    {
        get => slaveName;
        set
        {
            slaveName = value;
            OnPropertyChanged(nameof(SlaveName));
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

    private string installationOrientation = "";
    public string InstallationOrientation
    {
        get => installationOrientation;
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
            headIPAddress = _mainPageSlaveViewModel._systemSettingSlaveViewModel.UndaiIPAddress;
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
            slaveAntennaIPAddress = _mainPageSlaveViewModel._systemSettingSlaveViewModel.SlaveAntennaIPAddress;
            return slaveAntennaIPAddress;
        }
        set
        {
            slaveAntennaIPAddress = value;
            OnPropertyChanged(nameof(SlaveAntennaIPAddress));
        }
    }

    public bool IsIPAddressShow
    {
        get
        {
            return _mainPageSlaveViewModel.IsSlaveIPAddressShow;
        }
        set
        {
            _mainPageSlaveViewModel.IsSlaveIPAddressShow = value;
            OnPropertyChanged(nameof(IsIPAddressShow));
        }
    }

    public SelfRegSlaveModel SelfRegSlaveModel;
    public MainPageSlaveViewModel _mainPageSlaveViewModel;
    App app;

    public ICommand NavigateCommand { get; }
    public ICommand BaseStationDBSlaveNavigateCommand { get; }
    public ICommand SlaveDataRegistrationCommand { get; }

    public SelfRegSlaveViewModel
    (
        NavigationService navigationService,
        ConnectionSlave connectionSlave,
        MainPageSlaveViewModel mainPageSlaveViewModel
    )
    {
        app = (App)Application.Current;
        _navigationService = navigationService;
        _mainPageSlaveViewModel = mainPageSlaveViewModel;
        SelfRegSlaveModel = new SelfRegSlaveModel(navigationService, connectionSlave, this, _mainPageSlaveViewModel, app.xamlRoot);
        NavigateCommand = new NavigateCommand(navigationService, "MainPageSlaveView");
        BaseStationDBSlaveNavigateCommand = new NavigateCommand(navigationService, "StationDBSlaveView");
        SlaveDataRegistrationCommand = new RelayCommand(SelfRegSlaveModel.Button_Click);
    }
}
