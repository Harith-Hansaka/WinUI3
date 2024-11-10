using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.MASTER;

public class SystemSettingMasterViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    public SystemSettingMasterModel _systemSettingMasterModel;
    public MainPageMasterViewModel mainPageMasterViewModel;
    public ConnectionMaster _connectionMaster;
    App app;

    public bool PINKeyboardClose = false;
    public bool continuousOperationTimerStart = false;
    private TimeSpan timeLeft;

    public ICommand BtnUpArrowMainPageMouseDownCommand { get; }
    public ICommand BtnUpArrowMainPageMouseUpCommand { get; }
    public ICommand BtnDownArrowMainPageMouseDownCommand { get; }
    public ICommand BtnDownArrowMainPageMouseUpCommand { get; }
    public ICommand BtnCCWMainPageMouseDownCommand { get; }
    public ICommand BtnCCWMainPageMouseUpCommand { get; }
    public ICommand BtnCWMainPageMouseDownCommand { get; }
    public ICommand BtnCWMainPageMouseUpCommand { get; }
    public ICommand ElevationEncorderResetCommand { get; }
    public ICommand AzimuthEncorderResetCommand { get; }
    public ICommand UndaiIPAddressCommand { get; }
    public ICommand OriginOffsetAzimuthCommand { get; }
    public ICommand OriginOffsetElevationCommand { get; }
    public ICommand HighSpeedSetAzimuthCommand { get; }
    public ICommand HighSpeedSetElevationCommand { get; }
    public ICommand LowSpeedSetAzimuthCommand { get; }
    public ICommand LowSpeedSetElevationCommand { get; }
    public ICommand InchingSpeedSetAzimuthCommand { get; }
    public ICommand InchingSpeedSetElevationCommand { get; }
    public ICommand PeakSearchSpeedCommand { get; }
    public ICommand PeakSearchAzimuthCommand { get; }
    public ICommand PeakSearchElevationCommand { get; }
    public ICommand PeakSearchRSSILevelCommand { get; }
    public ICommand DetailedPeakSearchStepAngleCommand { get; }
    public ICommand DetailedPeakSearchRSSILevelCommand { get; }
    public ICommand SearchDirectionSwitchingCommand { get; }
    public ICommand ContinuousOperationTimeCommand { get; }
    public ICommand SystemSettingRegister { get; }
    public ICommand AlarmHistoryMasterNavigateCommand { get; }
    public ICommand AlarmResetCommand { get; }
    public ICommand ContinuousOperationTimerStopCommand { get; }
    public ICommand MainPageSlaveNavigateCommand;
    public ICommand MainPageMasterNavigateCommand;


    private ICommand mainPageNavigateCommand;
    public ICommand MainPageNavigateCommand
    {
        get
        {
            SystemSettingUnlock = false;
            GridVisibility = "Hidden";
            UndaiIPAddress = _connectionMaster.currentServerIP;
            if (SelectedMode == 1)
            {
                MasterChecked = true;
                mainPageNavigateCommand = MainPageMasterNavigateCommand;
            }
            if (SelectedMode == 2)
            {
                SlaveChecked = true;
                mainPageNavigateCommand = MainPageSlaveNavigateCommand;
            }
            if (!continuousOperationTimerStart)
            {
                ContinuousOperationTime = "";
            }

            return mainPageNavigateCommand;
        }
        set
        {
            mainPageNavigateCommand = value;
        }
    }

    public SystemSettingMasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connection
    )
    {
        _navigationService = navigationService;
        _connectionMaster = connection;
        _systemSettingMasterModel = new SystemSettingMasterModel(this);
        BtnUpArrowMainPageMouseDownCommand   = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(1));
        BtnUpArrowMainPageMouseUpCommand     = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(2));
        BtnDownArrowMainPageMouseDownCommand = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(3));
        BtnDownArrowMainPageMouseUpCommand   = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(4));
        BtnCCWMainPageMouseDownCommand       = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(5));
        BtnCCWMainPageMouseUpCommand         = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(6));
        BtnCWMainPageMouseDownCommand        = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(7));
        BtnCWMainPageMouseUpCommand          = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(8));
        ElevationEncorderResetCommand        = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(9));
        AzimuthEncorderResetCommand          = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(10));
        SystemSettingRegister                = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(11));
        ContinuousOperationTimeCommand       = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(12));
        ContinuousOperationTimerStopCommand  = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(13));
        AlarmResetCommand                    = new RelayCommand(() => _systemSettingMasterModel.ButtonClick(14));
        app = (App)Application.Current;
        CreateAppClassAfterDelay();
    }

    private async void CreateAppClassAfterDelay()
    {
        await Task.Delay(50);
        if (mainPageMasterViewModel == null)
        {
            // Access properties directly
            mainPageMasterViewModel = app.mainPageMasterViewModel;
        }
    }

    private string[] receivedDataArray;
    private string receivedData = string.Empty;
    public string ReceivedData
    {
        get { return receivedData; }
        set
        {
            receivedData = string.Empty;
            receivedDataArray = value.Split(',');
            for (int i = 0; i < receivedDataArray.Length / 2; i++)
            {
                if (i == 0)
                {
                    receivedData += $"0 - " + receivedDataArray[i] + "\n";
                }
                else if (i == 1)
                {
                    receivedData += $"1 - " + receivedDataArray[i] + "\n";
                }
                else if (i == 2)
                {
                    receivedData += $"2 - " + receivedDataArray[i] + "\n";
                }
                else
                {
                    receivedData += $"{i + 97} - {receivedDataArray[i]}\n";
                }
            }
            OnPropertyChanged(nameof(ReceivedData));
        }
    }

    private string[] receivedDataArray2;
    private string receivedData2 = string.Empty;
    public string ReceivedData2
    {
        get { return receivedData2; }
        set
        {
            receivedData2 = string.Empty;
            receivedDataArray2 = value.Split(',');
            for (int i = receivedDataArray2.Length / 2; i < receivedDataArray2.Length; i++)
            {
                receivedData2 += $"{i + 97} - {receivedDataArray2[i]}\n";
            }
            OnPropertyChanged(nameof(ReceivedData2));
        }
    }

    private string systemUnlockPIN = "****";
    public string SystemUnlockPIN
    {
        get { return systemUnlockPIN; }
        set
        {
            systemUnlockPIN = value;
            OnPropertyChanged(nameof(SystemUnlockPIN));
        }
    }

    private string gridVisibility = "Hidden";
    public string GridVisibility
    {
        get { return gridVisibility; }
        set
        {
            gridVisibility = value;
            OnPropertyChanged(nameof(GridVisibility));
        }
    }

    private string undaiIPAddress = "169.254.1.110";
    public string UndaiIPAddress
    {
        get { return undaiIPAddress; }
        set
        {
            undaiIPAddress = value;
            OnPropertyChanged(nameof(UndaiIPAddress));
        }
    }

    private string undaiSubnet = "255.255.0.0";
    public string UndaiSubnet
    {
        get { return undaiSubnet; }
        set
        {
            undaiSubnet = value;
            OnPropertyChanged(nameof(UndaiSubnet));
        }
    }

    private string elevationAngleInput100 = "";
    public string ElevationAngleInput100
    {
        get => elevationAngleInput100;
        set
        {
            elevationAngleInput100 = value;
            OnPropertyChanged(nameof(ElevationAngleInput100));
        }
    }

    private string azimuthAngleInput101 = "";
    public string AzimuthAngleInput101
    {
        get { return azimuthAngleInput101; }
        set
        {
            azimuthAngleInput101 = value;
            OnPropertyChanged(nameof(AzimuthAngleInput101));
        }
    }

    private bool systemSettingUnlock = false;
    public bool SystemSettingUnlock
    {
        get { return systemSettingUnlock; }
        set
        {
            systemSettingUnlock = value;
            OnPropertyChanged(nameof(SystemSettingUnlock));
        }
    }

    private string originOffsetAzimuth = "";
    public string OriginOffsetAzimuth
    {
        get { return originOffsetAzimuth; }
        set
        {
            originOffsetAzimuth = value;
            OnPropertyChanged(nameof(OriginOffsetAzimuth));
        }
    }

    private string defaultGateway = "";
    public string DefaultGateway
    {
        get { return defaultGateway; }
        set
        {
            defaultGateway = value;
            OnPropertyChanged(nameof(DefaultGateway));
        }
    }

    private string masterAntennaIPAddress = "";
    public string MasterAntennaIPAddress
    {
        get { return masterAntennaIPAddress; }
        set
        {
            masterAntennaIPAddress = value;
            OnPropertyChanged(nameof(MasterAntennaIPAddress));
        }
    }

    private string originOffsetElevation = "";
    public string OriginOffsetElevation
    {
        get { return originOffsetElevation; }
        set
        {
            originOffsetElevation = value;
            OnPropertyChanged(nameof(OriginOffsetElevation));
        }
    }

    private string highSpeedSetAzimuth = "";
    public string HighSpeedSetAzimuth
    {
        get { return highSpeedSetAzimuth; }
        set
        {
            highSpeedSetAzimuth = value;
            OnPropertyChanged(nameof(HighSpeedSetAzimuth));
        }
    }

    private string highSpeedSetElevation = "";
    public string HighSpeedSetElevation
    {
        get { return highSpeedSetElevation; }
        set
        {
            highSpeedSetElevation = value;
            OnPropertyChanged(nameof(HighSpeedSetElevation));
        }
    }

    private string lowSpeedSetAzimuth = "";
    public string LowSpeedSetAzimuth
    {
        get { return lowSpeedSetAzimuth; }
        set
        {
            lowSpeedSetAzimuth = value;
            OnPropertyChanged(nameof(LowSpeedSetAzimuth));
        }
    }

    private string lowSpeedSetElevation = "";
    public string LowSpeedSetElevation
    {
        get { return lowSpeedSetElevation; }
        set
        {
            lowSpeedSetElevation = value;
            OnPropertyChanged(nameof(LowSpeedSetElevation));
        }
    }

    private string inchingSpeedSetAzimuth = "";
    public string InchingSpeedSetAzimuth
    {
        get { return inchingSpeedSetAzimuth; }
        set
        {
            inchingSpeedSetAzimuth = value;
            OnPropertyChanged(nameof(InchingSpeedSetAzimuth));
        }
    }

    private string inchingSpeedSetElevation = "";
    public string InchingSpeedSetElevation
    {
        get { return inchingSpeedSetElevation; }
        set
        {
            inchingSpeedSetElevation = value;
            OnPropertyChanged(nameof(InchingSpeedSetElevation));
        }
    }

    private string peakSearchSpeed = "";
    public string PeakSearchSpeed
    {
        get { return peakSearchSpeed; }
        set
        {
            peakSearchSpeed = value;
            OnPropertyChanged(nameof(PeakSearchSpeed));
        }
    }

    private string peakSearchAzimuth = "";
    public string PeakSearchAzimuth
    {
        get { return peakSearchAzimuth; }
        set
        {
            peakSearchAzimuth = value;
            OnPropertyChanged(nameof(PeakSearchAzimuth));
        }
    }

    private string peakSearchElevation = "";
    public string PeakSearchElevation
    {
        get { return peakSearchElevation; }
        set
        {
            peakSearchElevation = value;
            OnPropertyChanged(nameof(PeakSearchElevation));
        }
    }

    private string peakSearchRSSILevel = "";
    public string PeakSearchRSSILevel
    {
        get { return peakSearchRSSILevel; }
        set
        {
            peakSearchRSSILevel = value;
            OnPropertyChanged(nameof(PeakSearchRSSILevel));
        }
    }

    private string detailedPeakSearchStepAngle = "";
    public string DetailedPeakSearchStepAngle
    {
        get { return detailedPeakSearchStepAngle; }
        set
        {
            detailedPeakSearchStepAngle = value;
            OnPropertyChanged(nameof(DetailedPeakSearchStepAngle));
        }
    }

    private string detailedPeakSearchRSSILevel = "";
    public string DetailedPeakSearchRSSILevel
    {
        get { return detailedPeakSearchRSSILevel; }
        set
        {
            detailedPeakSearchRSSILevel = value;
            OnPropertyChanged(nameof(DetailedPeakSearchRSSILevel));
        }
    }

    private string stepStability = "";
    public string StepStability
    {
        get { return stepStability; }
        set
        {
            stepStability = value;
            OnPropertyChanged(nameof(StepStability));
        }
    }

    private string rSSITurnbackThresholdLevel = "";
    public string RSSITurnbackThresholdLevel
    {
        get { return rSSITurnbackThresholdLevel; }
        set
        {
            rSSITurnbackThresholdLevel = value;
            OnPropertyChanged(nameof(RSSITurnbackThresholdLevel));
        }
    }

    private string continuousOperationTime = "2";
    public string ContinuousOperationTime
    {
        get
        {
            if (string.IsNullOrEmpty(continuousOperationTime))
            {
                return "0";
            }
            return continuousOperationTime;
        }
        set
        {
            continuousOperationTime = value;
            OnPropertyChanged(nameof(ContinuousOperationTime));
        }
    }

    private string peakSearchRSSIDelay = "";
    public string PeakSearchRSSIDelay
    {
        get { return peakSearchRSSIDelay; }
        set
        {
            peakSearchRSSIDelay = value;
            OnPropertyChanged(nameof(PeakSearchRSSIDelay));
        }
    }

    private string peakSearchRSSITurnLevel = "";
    public string PeakSearchRSSITurnLevel
    {
        get { return peakSearchRSSITurnLevel; }
        set
        {
            peakSearchRSSITurnLevel = value;
            OnPropertyChanged(nameof(PeakSearchRSSITurnLevel));
        }
    }

    private string upDownSearchAngle = "";
    public string UpDownSearchAngle
    {
        get { return upDownSearchAngle; }
        set
        {
            upDownSearchAngle = value;
            OnPropertyChanged(nameof(UpDownSearchAngle));
        }
    }

    private string continuousOperationTimerStopCommandAccess = "Visible";
    public string ContinuousOperationTimerStopCommandAccess
    {
        get
        {
            if (!continuousOperationTimerStart)
            {
                continuousOperationTimerStopCommandAccess = "Visible";
            }
            else
            {
                continuousOperationTimerStopCommandAccess = "Hidden";
            }
            return continuousOperationTimerStopCommandAccess;
        }
        set
        {
            if (!continuousOperationTimerStart)
            {
                continuousOperationTimerStopCommandAccess = "Visible";
            }
            else
            {
                continuousOperationTimerStopCommandAccess = "Hidden";
            }
            OnPropertyChanged(nameof(ContinuousOperationTimerStopCommandAccess));
        }
    }

    private string continuousOperationTimeCommandAccess = "Visible";
    public string ContinuousOperationTimeCommandAccess
    {
        get
        {
            if (continuousOperationTimerStart)
            {
                continuousOperationTimeCommandAccess = "Visible";
            }
            else
            {
                continuousOperationTimeCommandAccess = "Hidden";
            }
            return continuousOperationTimeCommandAccess;
        }
        set
        {
            if (continuousOperationTimerStart)
            {
                continuousOperationTimeCommandAccess = "Visible";
            }
            else
            {
                continuousOperationTimeCommandAccess = "Hidden";
            }
            OnPropertyChanged(nameof(ContinuousOperationTimeCommandAccess));
        }
    }

    private string systemControllerAccess = "Visible";
    public string SystemControllerAccess
    {
        get
        {
            if (SelectedMode != 2)
            {
                systemControllerAccess = "Visible";
            }
            else
            {
                systemControllerAccess = "Hidden";
            }
            return systemControllerAccess;
        }
        set
        {
            if (SelectedMode != 2)
            {
                systemControllerAccess = "Visible";
            }
            else
            {
                systemControllerAccess = "Hidden";
            }
            OnPropertyChanged(nameof(SystemControllerAccess));
        }
    }

    public async void ContinuousOperationTimerStart(string timeInMinutes)
    {
        if (double.TryParse(timeInMinutes, out double minutes))
        {
            continuousOperationTimerStart = true;
            timeLeft = TimeSpan.FromMinutes(minutes);
            while (timeLeft.TotalSeconds > 0 && continuousOperationTimerStart && mainPageMasterViewModel.Connected != "CONNECT")
            {
                ContinuousOperationTime = timeLeft.ToString(@"mm\.ss");  // Update the display every second (minutes and seconds)
                await Task.Delay(1000);  // Wait for 1 second
                timeLeft = timeLeft.Subtract(TimeSpan.FromSeconds(1));  // Decrease the time by 1 second
                OnPropertyChanged(nameof(ContinuousOperationTimeCommandAccess));
                OnPropertyChanged(nameof(ContinuousOperationTimerStopCommandAccess));
            }
            if (timeLeft.TotalSeconds <= 0)
            {
                continuousOperationTimerStart = false;  // Stop the timer when finished
                ContinuousOperationTime = "00.00";  // Set to zero when the timer ends

                if (mainPageMasterViewModel == null)
                {
                    // Access properties directly
                    CreateAppClassAfterDelay();
                }
                if (mainPageMasterViewModel != null)
                {
                    mainPageMasterViewModel._mainPageMasterModel.MessageSend("I," + (11).ToString() + ",\n");
                }
            }
            if (mainPageMasterViewModel == null)
            {
                // Access properties directly
                CreateAppClassAfterDelay();
            }
            if (mainPageMasterViewModel != null)
            {
                if (mainPageMasterViewModel.Connected == "CONNECT")
                {
                    continuousOperationTimerStart = false;  // Stop the timer when finished
                }
            }
            OnPropertyChanged(nameof(ContinuousOperationTimeCommandAccess));
            OnPropertyChanged(nameof(ContinuousOperationTimerStopCommandAccess));
        }
    }

    private int selectedMode = 1;
    public int SelectedMode
    {
        get { return selectedMode; }
        set
        {
            selectedMode = value;
            OnPropertyChanged(nameof(SelectedMode));
            OnPropertyChanged(nameof(MasterChecked));
            OnPropertyChanged(nameof(SlaveChecked));
            OnPropertyChanged(nameof(MainPageNavigateCommand));
        }
    }

    private bool masterChecked = true;
    public bool MasterChecked
    {
        get
        {
            if (mainPageMasterViewModel == null)
            {
                // Access properties directly
                mainPageMasterViewModel = app.mainPageMasterViewModel;
            }
            masterChecked = mainPageMasterViewModel._masterChecked;
            return masterChecked;
        }
        set
        {
            if (masterChecked == value)
            {
                return;
            }
            masterChecked = value;
            mainPageMasterViewModel._masterChecked = masterChecked;
            if (masterChecked)
            {
                SlaveChecked = false;
                mainPageMasterViewModel._slaveChecked = SlaveChecked;
            }
            OnPropertyChanged(nameof(MasterChecked));
            OnPropertyChanged(nameof(SystemControllerAccess));
        }
    }

    private bool slaveChecked = false;
    public bool SlaveChecked
    {
        get
        {
            slaveChecked = mainPageMasterViewModel._slaveChecked;
            return slaveChecked;
        }
        set
        {
            if (slaveChecked == value)
            {
                return;
            }
            slaveChecked = value;
            mainPageMasterViewModel._slaveChecked = slaveChecked;
            if (slaveChecked)
            {
                MasterChecked = false;
                mainPageMasterViewModel._masterChecked = MasterChecked;
            }
            OnPropertyChanged(nameof(SlaveChecked));
        }
    }
}
