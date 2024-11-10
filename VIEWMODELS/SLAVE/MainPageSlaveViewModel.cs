using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.MODELS.SLAVE;
using UNDAI.SERVICES;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.VIEWMODELS.SLAVE;

public class MainPageSlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    private int commandNo;
    public int ReceivingDataCount1;
    public string backupMasterIPAddress;
    public bool PINKeyboardClose = false;
    public string MasterNameBackup;
    public string MasterJapaneseNameBackup;
    public string SlaveLatitudeTextBackup;
    public string SlaveLongitudeTextBackup;
    public string SlaveElevationTextBackup;

    TimeSpan _time;
    private DateTime sendDataPressedTimer;
    public DateTime SendDataPressedTimer
    {
        get { return sendDataPressedTimer; }
        set { sendDataPressedTimer = value; }
    }

    private bool isSendDataLongPress;
    public bool IsSendDataLongPress
    {
        get { return isSendDataLongPress; }
        set
        {
            isSendDataLongPress = value;
            OnPropertyChanged(nameof(IsSendDataLongPress));
        }
    }

    private string connected = "CONNECT";
    public string Connected
    {
        get { return connected; }
        set
        {
            connected = value;
            OnPropertyChanged(nameof(Connected));
            OnPropertyChanged(nameof(ConnectBtnColor));
            OnPropertyChanged(nameof(ConnectedButtonText));
            OnPropertyChanged(nameof(ControllerAccess));
            OnPropertyChanged(nameof(LoadingScreenImgVisibility));
            Thread.Sleep(300);
        }
    }

    private string connectedButtonText = "雲台通信";
    public string ConnectedButtonText
    {
        get
        {
            if (Connected == "CONNECT")
            {
                connectedButtonText = "雲台通信";
            }
            else
            {
                connectedButtonText = "雲台接続";
            }
            return connectedButtonText;
        }
        set
        {
            if (Connected == "CONNECT")
            {
                connectedButtonText = "雲台通信";
            }
            else
            {
                connectedButtonText = "雲台接続";
            }
            OnPropertyChanged(nameof(ConnectedButtonText));
        }
    }

    private string controllerAccess = "Visible";
    public string ControllerAccess
    {
        get
        {
            if (Connected == "CONNECT" || _connectionSlave.loadingImg)
            {
                controllerAccess = "Visible";
            }
            else
            {
                controllerAccess = "Hidden";
            }
            return controllerAccess;
        }
        set
        {
            if (Connected == "CONNECT" || _connectionSlave.loadingImg)
            {
                controllerAccess = "Visible";
            }
            else
            {
                controllerAccess = "Hidden";
            }
            OnPropertyChanged(nameof(ControllerAccess));
        }
    }

    private string loadingScreenImgVisibility = "Hidden";
    public string LoadingScreenImgVisibility
    {
        get
        {
            if (_connectionSlave.loadingImg && Connected != "CONNECT")
            {
                loadingScreenImgVisibility = "Visible";
            }
            else
            {
                loadingScreenImgVisibility = "Hidden";
            }
            return loadingScreenImgVisibility;
        }
        set
        {

            if (_connectionSlave.loadingImg && Connected != "CONNECT")
            {
                loadingScreenImgVisibility = "Visible";
            }
            else
            {
                loadingScreenImgVisibility = "Hidden";
            }
            OnPropertyChanged(nameof(LoadingScreenImgVisibility));
            OnPropertyChanged(nameof(ControllerAccess));
        }
    }

    private string connectBtnColor = "#ED524F";
    public string ConnectBtnColor
    {
        get
        {
            return Connected == "CONNECT" ? "#ED524F" : "#3785c3";
        }
    }

    private int speedSelect = 0;
    public int SpeedSelect
    {
        get { return speedSelect; }
        set
        {
            speedSelect = value;
            OnPropertyChanged(nameof(SpeedSelect));
            OnPropertyChanged(nameof(LowSpeedBtnColor));
            OnPropertyChanged(nameof(HighSpeedBtnColor));
            OnPropertyChanged(nameof(InchingSpeedBtnColor));
        }
    }

    private string lowSpeedBtnColor = "#0077b6";
    public string LowSpeedBtnColor
    {
        get
        {
            return SpeedSelect == 1 ? "#0077b6" : "White";
        }
    }

    private string highSpeedBtnColor = "White";
    public string HighSpeedBtnColor
    {
        get
        {
            return SpeedSelect == 2 ? "#0077b6" : "White";
        }
    }

    private string inchingSpeedBtnColor = "White";
    public string InchingSpeedBtnColor
    {
        get
        {
            return SpeedSelect == 0 ? "#0077b6" : "White";
        }
    }

    private int homePeakSelect = 0;
    public int HomePeakSelect
    {
        get => homePeakSelect;
        set
        {
            homePeakSelect = value;
            OnPropertyChanged(nameof(HomeBtnColor));
            OnPropertyChanged(nameof(DirectionSetBtnColor));
            OnPropertyChanged(nameof(PeakBtnColor));
            OnPropertyChanged(nameof(PeakSearchButtonTextFirstLine));
            OnPropertyChanged(nameof(PeakSearchButtonTextSecondLine));
            OnPropertyChanged(nameof(PeakSearchButtonTextFirstLineFontSize));
            OnPropertyChanged(nameof(PeakSearchButtonTextSecondLineFontSize));
        }
    }

    private string homeBtnColor = "White";
    public string HomeBtnColor
    {
        get
        {
            if (HomePeakSelect == 1)
            {
                return "#0077b6";
            }
            else if (HomePeakSelect == 2)
            {
                return "#fdcd75";
            }
            return "White";
        }
    }

    private string directionSetBtnColor = "White";
    public string DirectionSetBtnColor
    {
        get
        {
            if (HomePeakSelect == 3)
            {
                return "#0077b6";
            }
            else if (HomePeakSelect == 4)
            {
                return "#fdcd75";
            }
            return "White";
        }
    }

    private string peakBtnColor = "White";
    public string PeakBtnColor
    {
        get
        {
            if (HomePeakSelect == 5)
            {
                return "#0077b6";
            }
            else if (HomePeakSelect == 6)
            {
                return "#fdcd75";
            }
            return "White";
        }
    }

    private string peakSearchButtonTextFirstLine = "ピーク";
    public string PeakSearchButtonTextFirstLine
    {
        get
        {
            if (HomePeakSelect == 5)
            {
                peakSearchButtonTextFirstLine = "ピークサーチ";
            }
            else if (HomePeakSelect == 6)
            {
                peakSearchButtonTextFirstLine = "ピークサーチ";
            }
            else
            {
                peakSearchButtonTextFirstLine = "ピーク";
            }
            return peakSearchButtonTextFirstLine;
        }
        set
        {
            peakSearchButtonTextFirstLine = value;
            OnPropertyChanged(nameof(PeakSearchButtonTextFirstLine));
        }
    }

    private int peakSearchButtonTextFirstLineFontSize = 40;
    public int PeakSearchButtonTextFirstLineFontSize
    {
        get
        {
            if (HomePeakSelect == 5)
            {
                peakSearchButtonTextFirstLineFontSize = 30;
            }
            else if (HomePeakSelect == 6)
            {
                peakSearchButtonTextFirstLineFontSize = 30;
            }
            else
            {
                peakSearchButtonTextFirstLineFontSize = 40;
            }
            return peakSearchButtonTextFirstLineFontSize;
        }
        set
        {
            peakSearchButtonTextFirstLineFontSize = value;
            OnPropertyChanged(nameof(PeakSearchButtonTextFirstLineFontSize));
        }
    }

    private string peakSearchButtonTextSecondLine = "サーチ";
    public string PeakSearchButtonTextSecondLine
    {
        get
        {
            if (HomePeakSelect == 5)
            {
                peakSearchButtonTextSecondLine = "中";
            }
            else if (HomePeakSelect == 6)
            {
                peakSearchButtonTextSecondLine = "完了";
            }
            else
            {
                peakSearchButtonTextSecondLine = "サーチ";
            }
            return peakSearchButtonTextSecondLine;
        }
        set
        {
            peakSearchButtonTextSecondLine = value;
            OnPropertyChanged(nameof(peakSearchButtonTextSecondLine));
        }
    }

    private int peakSearchButtonTextSecondLineFontSize = 40;
    public int PeakSearchButtonTextSecondLineFontSize
    {
        get
        {
            if (HomePeakSelect == 5)
            {
                peakSearchButtonTextSecondLineFontSize = 40;
            }
            else if (HomePeakSelect == 6)
            {
                peakSearchButtonTextSecondLineFontSize = 40;
            }
            else
            {
                peakSearchButtonTextSecondLineFontSize = 40;
            }
            return peakSearchButtonTextSecondLineFontSize;
        }
        set
        {
            peakSearchButtonTextSecondLineFontSize = value;
            OnPropertyChanged(nameof(PeakSearchButtonTextSecondLineFontSize));
        }
    }

    private bool isNameShowMaster = false;
    public bool IsNameShowMaster
    {
        get
        {
            return isNameShowMaster;
        }
        set
        {
            isNameShowMaster = value;
            OnPropertyChanged(nameof(IsNameShowMaster));
            MasterName = "";
        }
    }

    private string masterName = "Master";
    public string MasterName
    {
        get
        {
            if (IsNameShowMaster)
            {
                masterName = MasterJapaneseNameBackup;
            }
            else
            {
                masterName = MasterNameBackup;
            }
            return masterName;
        }
        set
        {
            if (IsNameShowMaster)
            {
                masterName = MasterJapaneseNameBackup;
            }
            else
            {
                masterName = MasterNameBackup;
            }
            OnPropertyChanged(nameof(MasterName));
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

    private int gPSStatus;
    public int GPSStatus
    {
        get { return gPSStatus; }
        set
        {
            gPSStatus = value;
            OnPropertyChanged(nameof(GPSStatus));
            OnPropertyChanged(nameof(GPSStatusDisplay));
            OnPropertyChanged(nameof(GPSStatusName));
        }
    }

    private string gPSStatusDisplay = "#ED524F";
    public string GPSStatusDisplay
    {
        get
        {
            return GPSStatus == 1 ? "#3785c3" : "#ED524F";
        }
    }

    private string gPSStatusName = "GPS捕捉中";
    public string GPSStatusName
    {
        get
        {
            if (GPSStatus == 1)
            {
                gPSStatusName = "GPS捕捉";
            }
            else
            {
                gPSStatusName = "GPS捕捉中";
            }
            return gPSStatusName;
        }
        set
        {
            if (GPSStatus == 1)
            {
                gPSStatusName = "GPS捕捉";
            }
            else
            {
                gPSStatusName = "GPS捕捉中";
            }
            OnPropertyChanged(nameof(GPSStatusName));
        }
    }

    private string elevationAngleInputSlave = "";
    public string ElevationAngleInputSlave
    {
        get { return elevationAngleInputSlave; }
        set
        {
            elevationAngleInputSlave = value;
            OnPropertyChanged(nameof(ElevationAngleInputSlave));
            OnPropertyChanged(nameof(UpDownArrowPositionSetMainPageEnabled));
        }
    }

    private string azimuthAngleInputSlave = "";
    public string AzimuthAngleInputSlave
    {
        get { return azimuthAngleInputSlave; }
        set
        {
            azimuthAngleInputSlave = value;
            OnPropertyChanged(nameof(AzimuthAngleInputSlave));
            OnPropertyChanged(nameof(RotatePositionSetMainPageEnabled));
        }
    }

    private string rotatePositionSetMainPageEnabled = "Visible";
    public string RotatePositionSetMainPageEnabled
    {
        get
        {
            if (AzimuthAngleInputSlave == "" || AzimuthAngleInputSlave == null)
            {
                rotatePositionSetMainPageEnabled = "Visible";
            }
            else
            {
                rotatePositionSetMainPageEnabled = "Hidden";
            }
            return rotatePositionSetMainPageEnabled;
        }
        set
        {
            if (AzimuthAngleInputSlave == "" || AzimuthAngleInputSlave == null)
            {
                rotatePositionSetMainPageEnabled = "Visible";
            }
            else
            {
                rotatePositionSetMainPageEnabled = "Hidden";
            }
        }
    }

    private string upDownArrowPositionSetMainPageEnabled = "Visible";
    public string UpDownArrowPositionSetMainPageEnabled
    {
        get
        {
            if (ElevationAngleInputSlave == "" || ElevationAngleInputSlave == null)
            {
                upDownArrowPositionSetMainPageEnabled = "Visible";
            }
            else
            {
                upDownArrowPositionSetMainPageEnabled = "Hidden";
            }
            return upDownArrowPositionSetMainPageEnabled;
        }
        set
        {
            if (ElevationAngleInputSlave == "" || ElevationAngleInputSlave == null)
            {
                upDownArrowPositionSetMainPageEnabled = "Visible";
            }
            else
            {
                upDownArrowPositionSetMainPageEnabled = "Hidden";
            }
        }
    }

    private int receivingDataCount;
    public int ReceivingDataCount
    {
        get => receivingDataCount;
        set
        {
            receivingDataCount = value;
            OnPropertyChanged(nameof(ReceivingDataCount));
            if (ReceivingDataCount % 3 == 0)
            {
                ReceivedData = "RECEIVING" + new string('.', ReceivingDataCount1);
                SlaveAlarmData = "";
                ReceivingDataCount1++;
                if (ReceivingDataCount1 > 3) { ReceivingDataCount1 = 0; }
            }
        }
    }

    private string receivedData;
    public string ReceivedData
    {
        get { return receivedData; }
        set
        {
            receivedData = value;
            if (receivedData != "")
            {
                SlaveAlarmData = "";
            }
            OnPropertyChanged(nameof(ReceivedData));
        }
    }

    private string slaveAlarmData;
    public string SlaveAlarmData
    {
        get { return slaveAlarmData; }
        set
        {
            slaveAlarmData = value;
            if (slaveAlarmData != "")
            {
                _alarmHistorySlaveViewModel.AddAlarmHistory(DateTime.Now.Date.ToString("yyyy-MM-dd"), DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"), slaveAlarmData);
            }
            OnPropertyChanged(nameof(SlaveAlarmData));
        }
    }

    private string txtBlockTime;
    public string TxtBlockTime
    {
        get => txtBlockTime;
        set
        {
            txtBlockTime = value;
            OnPropertyChanged(nameof(TxtBlockTime));
        }
    }

    private string txtBlockDate;
    public string TxtBlockDate
    {
        get => txtBlockDate;
        set
        {
            txtBlockDate = value;
            OnPropertyChanged(nameof(TxtBlockDate));
        }
    }

    private string latitude102 = "";
    public string Latitude102
    {
        get => latitude102;
        set
        {
            if (Connected == "CONNECT")
            {
                latitude102 = "";
            }
            else
            {
                if (UnitMainSlave == "DMS")
                {
                    int degrees = 0, wholeMinutes = 0, seconds = 0;
                    double fractional = 0, minutes = 0;
                    if (double.TryParse(value, out double dd))
                    {
                        degrees = (int)dd;
                        fractional = dd - degrees;
                        minutes = fractional * 60;
                        wholeMinutes = (int)minutes;
                        seconds = (int)(minutes - wholeMinutes) * 60;
                    }
                    latitude102 = $"{degrees}° {wholeMinutes}' {seconds}\"";
                }
                else
                {
                    latitude102 = value;
                }
            }
            OnPropertyChanged(nameof(Latitude102));
        }
    }

    private string longitude103 = "";
    public string Longitude103
    {
        get => longitude103;
        set
        {
            if (Connected == "CONNECT")
            {
                longitude103 = "";
            }
            else
            {
                if (UnitMainSlave == "DMS")
                {
                    int degrees = 0, wholeMinutes = 0, seconds = 0;
                    double fractional = 0, minutes = 0;
                    if (double.TryParse(value, out double dd))
                    {
                        degrees = (int)dd;
                        fractional = dd - degrees;
                        minutes = fractional * 60;
                        wholeMinutes = (int)minutes;
                        seconds = (int)(minutes - wholeMinutes) * 60;
                    }
                    longitude103 = $"{degrees}° {wholeMinutes}' {seconds}\"";
                }
                else
                {
                    longitude103 = value;
                }
            }
            OnPropertyChanged(nameof(Longitude103));
        }
    }

    private string elevation104 = "";
    public string Elevation104
    {
        get => elevation104;
        set
        {
            elevation104 = value;
            OnPropertyChanged(nameof(Elevation104));
        }
    }

    private string installationDirection105 = "";
    public string InstallationDirection105
    {
        get => installationDirection105;
        set
        {
            if (Connected == "CONNECT")
            {
                installationDirection105 = "";
            }
            else
            {
                installationDirection105 = value;
            }
            OnPropertyChanged(nameof(InstallationDirection105));
        }
    }

    private string mODMaster;
    public string MODMaster
    {
        get => mODMaster;
        set
        {
            if (Connected == "CONNECT")
            {
                mODMaster = "";
            }
            else
            {
                mODMaster = value;
            }
            OnPropertyChanged(nameof(MODMaster));
        }
    }

    private string rSSIRateMaster;
    public string RSSIRateMaster
    {
        get => rSSIRateMaster;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSIRateMaster = "";
            }
            else
            {
                rSSIRateMaster = (float.Parse(value) / 1000).ToString();
            }
            OnPropertyChanged(nameof(RSSIRateMaster));
        }
    }

    private bool masterStatus = true;
    public string MasterStatusColor
    {
        get { return masterStatus ? "#00FFFF" : "#ed524f"; }
        set
        {
            if (Connected == "CONNECT")
            {
                masterStatus = false;
            }
            else
            {
                masterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(MasterStatusColor));
            OnPropertyChanged(nameof(MasterStatusText));
        }
    }

    public string MasterStatusText
    {
        get { return masterStatus ? "LINK UP" : "LINK DOWN"; }
        set
        {
            if (Connected == "CONNECT")
            {
                masterStatus = false;
            }
            else
            {
                masterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(MasterStatusText));
        }
    }

    private string unitMainSlave = "DD";
    public string UnitMainSlave
    {
        get => unitMainSlave;
        set
        {
            unitMainSlave = value;
            OnPropertyChanged(nameof(UnitMainSlave));
        }
    }

    private string currentValueMasterRSSI;
    public string CurrentValueMasterRSSI
    {
        get { return currentValueMasterRSSI; }
        set
        {
            currentValueMasterRSSI = value.ToString();
            OnPropertyChanged(nameof(CurrentValueMasterRSSI));
        }
    }

    private string currentValueMasterElevation;
    public string CurrentValueMasterElevation
    {
        get { return currentValueMasterElevation; }
        set
        {
            currentValueMasterElevation = value.ToString();
            OnPropertyChanged(nameof(CurrentValueMasterElevation));
        }
    }

    private string currentValueMasterAzimuth;
    public string CurrentValueMasterAzimuth
    {
        get { return currentValueMasterAzimuth; }
        set
        {
            currentValueMasterAzimuth = value.ToString();
            OnPropertyChanged(nameof(CurrentValueMasterAzimuth));
        }
    }

    private string peakValueMasterRSSI;
    public string PeakValueMasterRSSI
    {
        get { return peakValueMasterRSSI; }
        set
        {
            peakValueMasterRSSI = value.ToString();
            OnPropertyChanged(nameof(PeakValueMasterRSSI));
        }
    }

    private string peakValueMasterElevation;
    public string PeakValueMasterElevation
    {
        get { return peakValueMasterElevation; }
        set
        {
            peakValueMasterElevation = value.ToString();
            OnPropertyChanged(nameof(PeakValueMasterElevation));
        }
    }

    private string peakValueMasterAzimuth;
    public string PeakValueMasterAzimuth
    {
        get { return peakValueMasterAzimuth; }
        set
        {
            peakValueMasterAzimuth = value.ToString();
            OnPropertyChanged(nameof(PeakValueMasterAzimuth));
        }
    }

    private string slaveNameTxt;
    public string SlaveNameTxt
    {
        get
        {
            return slaveNameTxt;
        }
        set
        {
            slaveNameTxt = value;
            OnPropertyChanged(nameof(SlaveNameTxt));
        }
    }

    private string slaveIPAddressTxt;
    public string SlaveIPAddressTxt
    {
        get
        {
            return slaveIPAddressTxt;
        }
        set
        {
            if (IsSlaveIPAddressShow)
            {
                slaveIPAddressTxt = _stationDBSlaveViewModel.SlaveAntennaIPAddress;
            }
            else
            {
                slaveIPAddressTxt = "";
            }
            OnPropertyChanged(nameof(SlaveIPAddressTxt));
        }
    }

    private bool isSlaveIPAddressShow = false;
    public bool IsSlaveIPAddressShow
    {
        get
        {
            return isSlaveIPAddressShow;
        }
        set
        {
            isSlaveIPAddressShow = value;
            OnPropertyChanged(nameof(IsSlaveIPAddressShow));
            SlaveIPAddressTxt = "";
        }
    }

    private string masterIPAddressTxt;
    public string MasterIPAddressTxt
    {
        get
        {
            return masterIPAddressTxt;
        }
        set
        {
            if (IsMasterIPAddressShow)
            {
                masterIPAddressTxt = _baseStationRegSlaveViewModel.MasterAntennaIPAddress;
            }
            else
            {
                masterIPAddressTxt = "";
            }
            OnPropertyChanged(nameof(MasterIPAddressTxt));
        }
    }

    private bool isMasterIPAddressShow = false;
    public bool IsMasterIPAddressShow
    {
        get
        {
            return isMasterIPAddressShow;
        }
        set
        {
            isMasterIPAddressShow = value;
            OnPropertyChanged(nameof(IsMasterIPAddressShow));
            MasterIPAddressTxt = "";
        }
    }

    public MainPageSlaveModel mainPageSlaveModel;
    public ConnectionSlave _connectionSlave;
    public SelfRegSlaveViewModel _selfRegSlaveViewModel;
    public SystemSettingSlaveViewModel _systemSettingSlaveViewModel;
    public StationDBSlaveViewModel _stationDBSlaveViewModel;
    public AlarmHistorySlaveViewModel _alarmHistorySlaveViewModel;
    public BaseStationRegSlaveViewModel _baseStationRegSlaveViewModel;
    public SystemSettingMasterViewModel _systemSettingMasterViewModel;
    //public ElevationCalculationModel _elevationCalculationModel;
    App app;
    public bool _masterChecked = false;
    public bool _slaveChecked = true;

    public ICommand ConnectCommandMouseDownCommand { get; }
    public ICommand ConnectCommandMouseUpCommand { get; }
    public ICommand SendCommand { get; }
    public ICommand SecondPageCommand { get; }
    public ICommand BtnUpArrowMainPage { get; }
    public ICommand BtnUpArrowMainPageMouseDownCommand { get; }
    public ICommand BtnUpArrowMainPageMouseUpCommand { get; }
    public ICommand BtnDownArrowMainPageMouseDownCommand { get; }
    public ICommand BtnDownArrowMainPageMouseUpCommand { get; }
    public ICommand BtnCCWMainPageMouseDownCommand { get; }
    public ICommand BtnCCWMainPageMouseUpCommand { get; }
    public ICommand BtnCWMainPageMouseDownCommand { get; }
    public ICommand BtnCWMainPageMouseUpCommand { get; }
    public ICommand ToMasterCommand { get; }
    public ICommand SelfRegSlaveCommand { get; }
    public ICommand StationDBSlaveCommand { get; }
    public ICommand BaseStationRegistrationSlaveCommand { get; }
    public ICommand LowSpeedBtnSlaveCommand { get; }
    public ICommand HighSpeedBtnSlaveCommand { get; }
    public ICommand InchingSpeedBtnSlaveCommand { get; }
    public ICommand ElevationAngleSetSlaveCommand { get; }
    public ICommand AzimuthAngleSetSlaveCommand { get; }
    public ICommand SaveAngleMainPageSlaveCommand { get; }
    public ICommand LoadLoadAngleMainPageSlaveCommand { get; }
    public ICommand HomePositionDownCommand { get; }
    public ICommand HomePositionUpCommand { get; }
    public ICommand DirectionSearchDownCommand { get; }
    public ICommand DirectionSearchUPCommand { get; }
    public ICommand PeakSearchDownCommand { get; }
    public ICommand PeakSearchUpCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand UnitMainSlaveCommand { get; }
    ICommand SystemResetSettingSlavePage { get; }
    public ICommand SystemSettingSlavePage { get; }
    public ICommand NullCommand { get; }

    public ICommand systemSettingSlaveCommand;
    public ICommand? SystemSettingSlaveCommand
    {
        get
        {
            if (Connected == "CONNECT")
            {
                systemSettingSlaveCommand = SystemResetSettingSlavePage;
            }
            else
            {
                systemSettingSlaveCommand = SystemSettingSlavePage;
            }
            return systemSettingSlaveCommand;
        }
        set
        {
            if (Connected == "CONNECT")
            {
                systemSettingSlaveCommand = SystemResetSettingSlavePage;
            }
            else
            {
                systemSettingSlaveCommand = SystemSettingSlavePage;
            }
            OnPropertyChanged(nameof(SystemSettingSlaveCommand));
        }
    }


    public MainPageSlaveViewModel
    (
        NavigationService navigationService,
        ConnectionSlave connectionSlave

    )
    {
        _navigationService = navigationService;
        _connectionSlave = connectionSlave;
        //_elevationCalculationModel = new ElevationCalculationModel();

        app = (App)Application.Current;
        //MethodChange();
    }


    private async void MethodChange()
    {
        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _connectionSlave.fileName);
        await Task.Delay(200);
        try
        {
            int selectedMode = int.Parse(File.ReadLines(_filePath).First().Split(',')[0]);
            if (_systemSettingSlaveViewModel == null || _systemSettingMasterViewModel == null)
            {
                // Access properties directly
                _systemSettingSlaveViewModel = app.systemSettingSlaveViewModel;
                _systemSettingMasterViewModel = app.systemSettingMasterViewModel;
            }
            _systemSettingMasterViewModel.SelectedMode = selectedMode;
            _systemSettingSlaveViewModel.SelectedMode = selectedMode;
            if (selectedMode == 1)
            {
                ToMasterCommand.Execute(null);
            }
        }
        catch
        {
            // Write newIPAddress to the file
            try
            {
                using (StreamWriter writer = new StreamWriter(_filePath, false))
                {
                    writer.WriteLine("2,169.254.1.110");
                }
                if (_systemSettingSlaveViewModel == null || _systemSettingMasterViewModel == null)
                {
                    // Access properties directly
                    _systemSettingSlaveViewModel = app.systemSettingSlaveViewModel;
                    _systemSettingMasterViewModel = app.systemSettingMasterViewModel;
                }
                int selectedMode = int.Parse(File.ReadLines(_filePath).First().Split(',')[0]);
                _systemSettingMasterViewModel.SelectedMode = selectedMode;
                _systemSettingSlaveViewModel.SelectedMode = selectedMode;
            }
            catch (Exception ex)
            {
                // Handle any errors that may occur
                SlaveAlarmData = $"Error saving Seleced method address: {ex.Message}";
            }
        }
    }
}
