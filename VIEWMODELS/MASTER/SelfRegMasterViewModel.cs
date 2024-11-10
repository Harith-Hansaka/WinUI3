using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.MASTER;

public class SelfRegMasterViewModel : ViewModelBase
{
    private bool isLatLongEnabled;
    public bool IsLatLongEnabled
    {
        get => isLatLongEnabled;
        set
        {
            isLatLongEnabled = value;
            LatLongEnable = value;
            OnPropertyChanged(nameof(IsLatLongEnabled));
            OnPropertyChanged(nameof(LatLongInabilityShow));
            if (!isLatLongEnabled)
            {
                LatitudeMaster = _mainPageMasterViewModel.Latitude102;
                LongitudeMaster = _mainPageMasterViewModel.Longitude103;
            }
            if (isLatLongEnabled)
            {
                latLongEnabledFirst = true;
                LatitudeMaster = "";
                LongitudeMaster = "";
            }
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
                ElevationMaster = "";
            }
            if (isElevationEnabled)
            {
                elevationMasterEnabledFirst = true;
            }
            OnPropertyChanged(nameof(ElevationInabilityShow));
            OnPropertyChanged(nameof(ElevationMaster));
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
            if (!IsLatLongEnabled)
            {
                latitudeMaster = _mainPageMasterViewModel.Latitude102;
            }
            return latitudeMaster;
        }
        set
        {
            latitudeMaster = value;
            if (IsLatLongEnabled && latLongEnabledFirst)
            {
                latitudeMaster = _mainPageMasterViewModel.MasterLatitudeTextBackup;
            }
            OnPropertyChanged(nameof(LatitudeMaster));
            if (!isElevationEnabled)
            {
                ElevationMaster = "";
            }
        }
    }

    bool latLongEnabledFirst = false;
    private string longitudeMaster = "";
    public string LongitudeMaster
    {
        get
        {
            if (!IsLatLongEnabled)
            {
                longitudeMaster = _mainPageMasterViewModel.Longitude103;
            }
            return longitudeMaster;
        }
        set
        {
            longitudeMaster = value;
            if (IsLatLongEnabled && latLongEnabledFirst)
            {
                longitudeMaster = _mainPageMasterViewModel.MasterLongitudeTextBackup;
                latLongEnabledFirst = false;
            }
            OnPropertyChanged(nameof(LongitudeMaster));
            if (!isElevationEnabled)
            {
                ElevationMaster = "";
            }
        }
    }

    bool elevationMasterEnabledFirst = false;
    private string elevationMaster = "";
    public string ElevationMaster
    {
        get
        {
            if (IsElevationEnabled && elevationMasterEnabledFirst)
            {
                elevationMaster = _mainPageMasterViewModel.MasterElevationTextBackup;
                elevationMasterEnabledFirst = false;
            }
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
                    //elevationMaster = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeMaster), float.Parse(LongitudeMaster)).ToString();
                }
                else
                {
                    elevationMaster = "-999.9";
                }
            }
            return elevationMaster;
        }
        set
        {
            if (IsElevationEnabled && elevationMasterEnabledFirst)
            {
                elevationMaster = _mainPageMasterViewModel.MasterElevationTextBackup;
                elevationMasterEnabledFirst = false;
            }
            else if (IsElevationEnabled)
            {
                elevationMaster = value;
            }
            else if (!IsElevationEnabled)
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
                    //elevationMaster = _mainPageMasterViewModel._elevationCalculationModel.ElevationCalculator(float.Parse(LatitudeMaster), float.Parse(LongitudeMaster)).ToString();
                }
                else
                {
                    elevationMaster = "-999.9";
                }
            }
            OnPropertyChanged(nameof(ElevationMaster));
        }
    }

    private string masterName = "";
    public string MasterName
    {
        get => masterName;
        set
        {
            masterName = value;
            OnPropertyChanged(nameof(MasterName));
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
            //headIPAddress = _mainPageMasterViewModel._systemSettingMasterViewModel.UndaiIPAddress;
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
            //masterAntennaIPAddress = _mainPageMasterViewModel._systemSettingMasterViewModel.MasterAntennaIPAddress;
            return masterAntennaIPAddress;
        }
        set
        {
            masterAntennaIPAddress = value;
            OnPropertyChanged(nameof(MasterAntennaIPAddress));
        }
    }

    public bool IsIPAddressShow
    {
        get
        {
            return _mainPageMasterViewModel.IsIPAddressShow;
        }
        set
        {
            _mainPageMasterViewModel.IsIPAddressShow = value;
            OnPropertyChanged(nameof(IsIPAddressShow));
        }
    }

    public SelfRegMasterModel SelfRegMasterModel;
    public MainPageMasterViewModel _mainPageMasterViewModel;
    App app;

    public ICommand NavigateCommand { get; }
    public ICommand StationDBPageMasterNavigateCommand { get; }
    public ICommand MasterDataRegistrationCommand { get; }


    private readonly NavigationService _navigationService;
    public SelfRegMasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster,
        MainPageMasterViewModel mainPageMasterViewModel
    )

    {
        app = (App)Application.Current;
        _navigationService = navigationService;
        _mainPageMasterViewModel = mainPageMasterViewModel;
        SelfRegMasterModel = new SelfRegMasterModel(_navigationService, connectionMaster, this, _mainPageMasterViewModel, app.xamlRoot);
        NavigateCommand = new NavigateCommand(navigationService, "MainPageMasterView");
        StationDBPageMasterNavigateCommand = new NavigateCommand(navigationService, "StationDBMasterView");
        MasterDataRegistrationCommand = new RelayCommand(SelfRegMasterModel.Button_Click);
    }
}
