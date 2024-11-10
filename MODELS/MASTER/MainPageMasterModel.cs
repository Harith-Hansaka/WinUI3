using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using UNDAI.COMMANDS.MASTER;
using UNDAI.VIEWMODELS.MASTER;
using UNDAI.VIEWMODELS.SLAVE;
using UNDAI.VIEWS.BASE;
using Timer = System.Timers.Timer;

namespace UNDAI.MODELS.MASTER
{
    public class MainPageMasterModel
    {
        MainPageMasterViewModel _mainPageMasterViewModel;
        public LongPressHandleMaster _longPressHandleMaster;
        MessageSendMasterCommand _messageSendMasterCommand;
        StationDBMasterViewModel _stationDBPageMasterViewModel;
        ConnectionMaster _connectionMaster;
        SubstationMasterViewModel _substationMasterViewModel;
        SubstationDB1MasterViewModel _substationDB1MasterViewModel;
        SubstationDB2MasterViewModel _substationDB2MasterViewModel;
        SubstationDB3MasterViewModel _substationDB3MasterViewModel;
        SubstationDB4MasterViewModel _substationDB4MasterViewModel;
        SystemSettingMasterViewModel _systemSettingMasterViewModel;
        SystemSettingSlaveViewModel _systemSettingSlaveViewModel;
        AlarmHistoryMasterViewModel _alarmHistoryMasterViewModel;
        SelfRegMasterViewModel _selfRegMasterViewModel;
        App app;

        private Timer _timer;
        string _message;
        List<string> messageList;
        int dataCount;
        List<int> dataCountList;
        public int randomNumber = 0;
        public int messageReceivedCount = 0;
        public bool isBtnUnitNoUpLongPress = false;
        public bool isBtnUnitNoDownLongPress = false;
        DateTime pressedTime;
        bool isBtnPointerPressed;
        bool isBtnPointerReleased = true;
        bool isLongPressed = false;
        string newIPAddress = "";

        public MainPageMasterModel(MainPageMasterViewModel mainPageMasterViewModel, ConnectionMaster connectionMaster)
        {
            _mainPageMasterViewModel = mainPageMasterViewModel;
            _connectionMaster = connectionMaster;
            _stationDBPageMasterViewModel = mainPageMasterViewModel._stationDBMasterViewModel;
            _substationMasterViewModel = mainPageMasterViewModel._substationMasterViewModel;
            _substationDB1MasterViewModel = mainPageMasterViewModel._substationDB1MasterViewModel;
            _substationDB2MasterViewModel = mainPageMasterViewModel._substationDB2MasterViewModel;
            _substationDB3MasterViewModel = mainPageMasterViewModel._substationDB3MasterViewModel;
            _substationDB4MasterViewModel = mainPageMasterViewModel._substationDB4MasterViewModel;

            _longPressHandleMaster = new LongPressHandleMaster(_mainPageMasterViewModel, this);
            _messageSendMasterCommand = new MessageSendMasterCommand(_connectionMaster, _mainPageMasterViewModel, string.Empty);
            _timer = new Timer(1000); // Initialize timer
            _message = string.Empty; // Initialize message
            messageList = new List<string>(); // Initialize message list
            dataCountList = new List<int>(); // Initialize data count list
            UpdateDateTime();
            StartTimer();
            app = (App)Application.Current;
            CreateAppClassAfterDelay();
        }

        private void CreateAppClassAfterDelay()
        {
            Thread.Sleep(1);
            if (_systemSettingMasterViewModel == null || _systemSettingSlaveViewModel == null)
            {
                // Access properties directly
                _systemSettingMasterViewModel = app.systemSettingMasterViewModel;
                _systemSettingSlaveViewModel = app.systemSettingSlaveViewModel;
            }
            if (_alarmHistoryMasterViewModel == null)
            {
                // Access properties directly
                _alarmHistoryMasterViewModel = app.alarmHistoryMasterViewModel;
            }
            if (_selfRegMasterViewModel == null)
            {
                // Access properties directly
                _substationMasterViewModel = app.substationMasterViewModel;
            }
        }

        public void MessageSend(string message)
        {
            _messageSendMasterCommand = new MessageSendMasterCommand(_connectionMaster, _mainPageMasterViewModel, message);
            _messageSendMasterCommand.Execute(this);
        }

        public void PINCheck(string EnteredPIN)
        {
            string PIN = "1234";
            if (_mainPageMasterViewModel.PINKeyboardClose)
            {
                if (EnteredPIN == PIN)
                {
                    _mainPageMasterViewModel.SystemSettingUnlock = true;
                    _mainPageMasterViewModel.PINKeyboardClose = false;
                    _mainPageMasterViewModel.SystemSettingMasterCommand = _mainPageMasterViewModel.SystemSettingMasterPage;
                    _mainPageMasterViewModel.SystemSettingMasterCommand.Execute(null);
                }
                else
                {
                    _mainPageMasterViewModel.SystemSettingUnlock = false;
                    _mainPageMasterViewModel.PINKeyboardClose = false;
                }
            }
        }

        public async Task ReceivedMessageAsync(string message)
        {
            if (_systemSettingMasterViewModel != null && 
                _systemSettingSlaveViewModel != null && 
                _selfRegMasterViewModel != null && 
                _alarmHistoryMasterViewModel != null) { }
            else
            {
                CreateAppClassAfterDelay();
            }
            
            _message = message;
            //_mainPageMasterViewModel.ReceivedData = _message;
            messageReceivedCount++;
            if (_systemSettingMasterViewModel == null)
            {
                // Access properties directly
                _systemSettingMasterViewModel = app.systemSettingMasterViewModel;
            }
            try
            {
                messageList = new List<string>(_message.Split(','));

            }

            catch (Exception ex)
            {
                _mainPageMasterViewModel.ElevationAngleInput100 = ex.ToString();
                _mainPageMasterViewModel.AzimuthAngleInput101 = ex.ToString();
            }
            if (messageList[0] == "A")
            {
                try
                {
                    dataCountList = new List<int>();
                    int dataCount = int.Parse(messageList[2]);
                    int dataCountStartingDataLocation = int.Parse(messageList[1]);
                    randomNumber = int.Parse(messageList[3 + dataCount - 1]);
                    for (int i = 0; i < dataCount; i++)
                    {
                        dataCountList.Add(dataCountStartingDataLocation + i);
                    }
                    _connectionMaster.loadingImg = false;
                    _mainPageMasterViewModel.LoadingScreenImgVisibility = "";
                    for (int i = 0; i < dataCountList.Count; i++)
                    {
                        switch (dataCountList[i])
                        {
                            // 100 - Elevation Angle
                            case 100:
                                _mainPageMasterViewModel.ElevationAngleInput100 = messageList[3 + i];
                                _systemSettingMasterViewModel.ElevationAngleInput100 = messageList[3 + i];
                                break;
                            // 101 - Azimuth Angle
                            case 101:
                                _mainPageMasterViewModel.AzimuthAngleInput101 = messageList[3 + i];
                                _systemSettingMasterViewModel.AzimuthAngleInput101 = messageList[3 + i];
                                break;
                            // 102 - Latitude
                            case 102:
                                _mainPageMasterViewModel.Latitude102 = messageList[3 + i];
                                break;
                            // 103 - Longitude
                            case 103:
                                _mainPageMasterViewModel.Longitude103 = messageList[3 + i];
                                break;
                            // 104 - Elevation
                            case 104:
                                _mainPageMasterViewModel.Elevation104 = messageList[3 + i];
                                break;
                            // 105 - Installation Direction
                            case 105:
                                _mainPageMasterViewModel.InstallationDirection105 = messageList[3 + i];
                                break;
                            // 106 - Speed Select
                            case 106:
                                _mainPageMasterViewModel.SpeedSelect = int.Parse(messageList[3 + i]);
                                break;
                            // 107 - LINK UP/ DOWN
                            case 107:
                                string binaryString = Convert.ToString(int.Parse(messageList[3 + i]), 2).PadLeft(4, '0');
                                if (binaryString[3] == '0')
                                {
                                    _mainPageMasterViewModel.SubstationDB1MasterStatusColor = "false";
                                }
                                else if (binaryString[3] == '1')
                                {
                                    _mainPageMasterViewModel.SubstationDB1MasterStatusColor = "true";
                                }
                                if (binaryString[2] == '0')
                                {
                                    _mainPageMasterViewModel.SubstationDB2MasterStatusColor = "false";
                                }
                                else if (binaryString[2] == '1')
                                {
                                    _mainPageMasterViewModel.SubstationDB2MasterStatusColor = "true";
                                }
                                if (binaryString[1] == '0')
                                {
                                    _mainPageMasterViewModel.SubstationDB3MasterStatusColor = "false";
                                }
                                else if (binaryString[1] == '1')
                                {
                                    _mainPageMasterViewModel.SubstationDB3MasterStatusColor = "true";
                                }
                                if (binaryString[0] == '0')
                                {
                                    _mainPageMasterViewModel.SubstationDB4MasterStatusColor = "false";
                                }
                                else if (binaryString[0] == '1')
                                {
                                    _mainPageMasterViewModel.SubstationDB4MasterStatusColor = "true";
                                }
                                break;
                            // 108 - RSSISubstation1Master
                            case 108:
                                _mainPageMasterViewModel.RSSISubstation1Master = $"-{(int.Parse(messageList[3 + i]) / 10.0):F1}";
                                break;
                            // 109 - MODSubstation1Master
                            case 109:
                                if (messageList[3 + i] == "0") { _mainPageMasterViewModel.MODSubstation1Master = "ACQUISITION"; }
                                else if (messageList[3 + i] == "1") { _mainPageMasterViewModel.MODSubstation1Master = "BPSK 0.63"; }
                                else if (messageList[3 + i] == "2") { _mainPageMasterViewModel.MODSubstation1Master = "QPSK 0.63 (SINGLE)"; }
                                else if (messageList[3 + i] == "3") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT1"; }
                                else if (messageList[3 + i] == "4") { _mainPageMasterViewModel.MODSubstation1Master = "QPSK 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "5") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT2"; }
                                else if (messageList[3 + i] == "6") { _mainPageMasterViewModel.MODSubstation1Master = "16QAM 0.63 (SINGLEA)"; }
                                else if (messageList[3 + i] == "7") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT3"; }
                                else if (messageList[3 + i] == "8") { _mainPageMasterViewModel.MODSubstation1Master = "16QAM 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "9") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT4"; }
                                else if (messageList[3 + i] == "10") { _mainPageMasterViewModel.MODSubstation1Master = "64QAM 0.75 (SINGLE)"; }
                                else if (messageList[3 + i] == "11") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT5"; }
                                else if (messageList[3 + i] == "12") { _mainPageMasterViewModel.MODSubstation1Master = "64QAM 0.92 (SINGLE)"; }
                                else if (messageList[3 + i] == "13") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT6"; }
                                else if (messageList[3 + i] == "14") { _mainPageMasterViewModel.MODSubstation1Master = "256QAM 0.81 (SINGLE)"; }
                                else if (messageList[3 + i] == "15") { _mainPageMasterViewModel.MODSubstation1Master = "16QAM 0.63 (SINGLEB)"; }
                                else if (messageList[3 + i] == "16") { _mainPageMasterViewModel.MODSubstation1Master = "16QAM 0.63 (DUAL)"; }
                                else if (messageList[3 + i] == "17") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT7"; }
                                else if (messageList[3 + i] == "18") { _mainPageMasterViewModel.MODSubstation1Master = "16QAM 0.87 (DUAL)"; }
                                else if (messageList[3 + i] == "19") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT8"; }
                                else if (messageList[3 + i] == "20") { _mainPageMasterViewModel.MODSubstation1Master = "64QAM 0.75 (DUAL)"; }
                                else if (messageList[3 + i] == "21") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT9"; }
                                else if (messageList[3 + i] == "22") { _mainPageMasterViewModel.MODSubstation1Master = "64QAM 0.92 (DUAL)"; }
                                else if (messageList[3 + i] == "23") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT10"; }
                                else if (messageList[3 + i] == "24") { _mainPageMasterViewModel.MODSubstation1Master = "256QAM 0.81 (DUAL)"; }
                                else if (messageList[3 + i] == "25") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT11"; }
                                else if (messageList[3 + i] == "26") { _mainPageMasterViewModel.MODSubstation1Master = "256QAM 0.94 (SINGLE)"; }
                                else if (messageList[3 + i] == "27") { _mainPageMasterViewModel.MODSubstation1Master = "TRANSIENT12"; }
                                else if (messageList[3 + i] == "28") { _mainPageMasterViewModel.MODSubstation1Master = "256QAM 0.94 (DUAL)"; }
                                else _mainPageMasterViewModel.MODSubstation1Master = (messageList[3 + i]);
                                break;
                            // 110 - RSSIRate1Master
                            case 110:
                                _mainPageMasterViewModel.RSSIRate1Master = messageList[3 + i];
                                break;
                            // 111 - RSSISubstation2Master
                            case 111:
                                _mainPageMasterViewModel.RSSISubstation2Master = $"-{(int.Parse(messageList[3 + i]) / 10.0):F1}";
                                break;
                            // 112 - MODSubstation2Master
                            case 112:
                                if (messageList[3 + i] == "0") { _mainPageMasterViewModel.MODSubstation2Master = "ACQUISITION"; }
                                else if (messageList[3 + i] == "1") { _mainPageMasterViewModel.MODSubstation2Master = "BPSK 0.63"; }
                                else if (messageList[3 + i] == "2") { _mainPageMasterViewModel.MODSubstation2Master = "QPSK 0.63 (SINGLE)"; }
                                else if (messageList[3 + i] == "3") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT1"; }
                                else if (messageList[3 + i] == "4") { _mainPageMasterViewModel.MODSubstation2Master = "QPSK 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "5") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT2"; }
                                else if (messageList[3 + i] == "6") { _mainPageMasterViewModel.MODSubstation2Master = "16QAM 0.63 (SINGLEA)"; }
                                else if (messageList[3 + i] == "7") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT3"; }
                                else if (messageList[3 + i] == "8") { _mainPageMasterViewModel.MODSubstation2Master = "16QAM 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "9") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT4"; }
                                else if (messageList[3 + i] == "10") { _mainPageMasterViewModel.MODSubstation2Master = "64QAM 0.75 (SINGLE)"; }
                                else if (messageList[3 + i] == "11") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT5"; }
                                else if (messageList[3 + i] == "12") { _mainPageMasterViewModel.MODSubstation2Master = "64QAM 0.92 (SINGLE)"; }
                                else if (messageList[3 + i] == "13") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT6"; }
                                else if (messageList[3 + i] == "14") { _mainPageMasterViewModel.MODSubstation2Master = "256QAM 0.81 (SINGLE)"; }
                                else if (messageList[3 + i] == "15") { _mainPageMasterViewModel.MODSubstation2Master = "16QAM 0.63 (SINGLEB)"; }
                                else if (messageList[3 + i] == "16") { _mainPageMasterViewModel.MODSubstation2Master = "16QAM 0.63 (DUAL)"; }
                                else if (messageList[3 + i] == "17") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT7"; }
                                else if (messageList[3 + i] == "18") { _mainPageMasterViewModel.MODSubstation2Master = "16QAM 0.87 (DUAL)"; }
                                else if (messageList[3 + i] == "19") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT8"; }
                                else if (messageList[3 + i] == "20") { _mainPageMasterViewModel.MODSubstation2Master = "64QAM 0.75 (DUAL)"; }
                                else if (messageList[3 + i] == "21") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT9"; }
                                else if (messageList[3 + i] == "22") { _mainPageMasterViewModel.MODSubstation2Master = "64QAM 0.92 (DUAL)"; }
                                else if (messageList[3 + i] == "23") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT10"; }
                                else if (messageList[3 + i] == "24") { _mainPageMasterViewModel.MODSubstation2Master = "256QAM 0.81 (DUAL)"; }
                                else if (messageList[3 + i] == "25") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT11"; }
                                else if (messageList[3 + i] == "26") { _mainPageMasterViewModel.MODSubstation2Master = "256QAM 0.94 (SINGLE)"; }
                                else if (messageList[3 + i] == "27") { _mainPageMasterViewModel.MODSubstation2Master = "TRANSIENT12"; }
                                else if (messageList[3 + i] == "28") { _mainPageMasterViewModel.MODSubstation2Master = "256QAM 0.94 (DUAL)"; }
                                else _mainPageMasterViewModel.MODSubstation2Master = (messageList[3 + i]);
                                break;
                            // 113 - RSSIRate2Master
                            case 113:
                                _mainPageMasterViewModel.RSSIRate2Master = messageList[3 + i];
                                break;
                            // 114 - RSSISubstation3Master
                            case 114:
                                _mainPageMasterViewModel.RSSISubstation3Master = $"-{(int.Parse(messageList[3 + i]) / 10.0):F1}";
                                break;
                            // 115 - MODSubstation3Master
                            case 115:
                                if (messageList[3 + i] == "0") { _mainPageMasterViewModel.MODSubstation3Master = "ACQUISITION"; }
                                else if (messageList[3 + i] == "1") { _mainPageMasterViewModel.MODSubstation3Master = "BPSK 0.63"; }
                                else if (messageList[3 + i] == "2") { _mainPageMasterViewModel.MODSubstation3Master = "QPSK 0.63 (SINGLE)"; }
                                else if (messageList[3 + i] == "3") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT1"; }
                                else if (messageList[3 + i] == "4") { _mainPageMasterViewModel.MODSubstation3Master = "QPSK 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "5") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT2"; }
                                else if (messageList[3 + i] == "6") { _mainPageMasterViewModel.MODSubstation3Master = "16QAM 0.63 (SINGLEA)"; }
                                else if (messageList[3 + i] == "7") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT3"; }
                                else if (messageList[3 + i] == "8") { _mainPageMasterViewModel.MODSubstation3Master = "16QAM 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "9") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT4"; }
                                else if (messageList[3 + i] == "10") { _mainPageMasterViewModel.MODSubstation3Master = "64QAM 0.75 (SINGLE)"; }
                                else if (messageList[3 + i] == "11") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT5"; }
                                else if (messageList[3 + i] == "12") { _mainPageMasterViewModel.MODSubstation3Master = "64QAM 0.92 (SINGLE)"; }
                                else if (messageList[3 + i] == "13") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT6"; }
                                else if (messageList[3 + i] == "14") { _mainPageMasterViewModel.MODSubstation3Master = "256QAM 0.81 (SINGLE)"; }
                                else if (messageList[3 + i] == "15") { _mainPageMasterViewModel.MODSubstation3Master = "16QAM 0.63 (SINGLEB)"; }
                                else if (messageList[3 + i] == "16") { _mainPageMasterViewModel.MODSubstation3Master = "16QAM 0.63 (DUAL)"; }
                                else if (messageList[3 + i] == "17") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT7"; }
                                else if (messageList[3 + i] == "18") { _mainPageMasterViewModel.MODSubstation3Master = "16QAM 0.87 (DUAL)"; }
                                else if (messageList[3 + i] == "19") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT8"; }
                                else if (messageList[3 + i] == "20") { _mainPageMasterViewModel.MODSubstation3Master = "64QAM 0.75 (DUAL)"; }
                                else if (messageList[3 + i] == "21") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT9"; }
                                else if (messageList[3 + i] == "22") { _mainPageMasterViewModel.MODSubstation3Master = "64QAM 0.92 (DUAL)"; }
                                else if (messageList[3 + i] == "23") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT10"; }
                                else if (messageList[3 + i] == "24") { _mainPageMasterViewModel.MODSubstation3Master = "256QAM 0.81 (DUAL)"; }
                                else if (messageList[3 + i] == "25") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT11"; }
                                else if (messageList[3 + i] == "26") { _mainPageMasterViewModel.MODSubstation3Master = "256QAM 0.94 (SINGLE)"; }
                                else if (messageList[3 + i] == "27") { _mainPageMasterViewModel.MODSubstation3Master = "TRANSIENT12"; }
                                else if (messageList[3 + i] == "28") { _mainPageMasterViewModel.MODSubstation3Master = "256QAM 0.94 (DUAL)"; }
                                else _mainPageMasterViewModel.MODSubstation3Master = (messageList[3 + i]);
                                break;
                            // 116 - RSSIRate3Master
                            case 116:
                                _mainPageMasterViewModel.RSSIRate3Master = messageList[3 + i];
                                break;
                            // 117 - RSSISubstation4Master
                            case 117:
                                _mainPageMasterViewModel.RSSISubstation4Master = $"-{(int.Parse(messageList[3 + i]) / 10.0):F1}";
                                break;
                            // 118 - MODSubstation4Master
                            case 118:
                                if (messageList[3 + i] == "0") { _mainPageMasterViewModel.MODSubstation4Master = "ACQUISITION"; }
                                else if (messageList[3 + i] == "1") { _mainPageMasterViewModel.MODSubstation4Master = "BPSK 0.63"; }
                                else if (messageList[3 + i] == "2") { _mainPageMasterViewModel.MODSubstation4Master = "QPSK 0.63 (SINGLE)"; }
                                else if (messageList[3 + i] == "3") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT1"; }
                                else if (messageList[3 + i] == "4") { _mainPageMasterViewModel.MODSubstation4Master = "QPSK 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "5") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT2"; }
                                else if (messageList[3 + i] == "6") { _mainPageMasterViewModel.MODSubstation4Master = "16QAM 0.63 (SINGLEA)"; }
                                else if (messageList[3 + i] == "7") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT3"; }
                                else if (messageList[3 + i] == "8") { _mainPageMasterViewModel.MODSubstation4Master = "16QAM 0.87 (SINGLE)"; }
                                else if (messageList[3 + i] == "9") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT4"; }
                                else if (messageList[3 + i] == "10") { _mainPageMasterViewModel.MODSubstation4Master = "64QAM 0.75 (SINGLE)"; }
                                else if (messageList[3 + i] == "11") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT5"; }
                                else if (messageList[3 + i] == "12") { _mainPageMasterViewModel.MODSubstation4Master = "64QAM 0.92 (SINGLE)"; }
                                else if (messageList[3 + i] == "13") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT6"; }
                                else if (messageList[3 + i] == "14") { _mainPageMasterViewModel.MODSubstation4Master = "256QAM 0.81 (SINGLE)"; }
                                else if (messageList[3 + i] == "15") { _mainPageMasterViewModel.MODSubstation4Master = "16QAM 0.63 (SINGLEB)"; }
                                else if (messageList[3 + i] == "16") { _mainPageMasterViewModel.MODSubstation4Master = "16QAM 0.63 (DUAL)"; }
                                else if (messageList[3 + i] == "17") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT7"; }
                                else if (messageList[3 + i] == "18") { _mainPageMasterViewModel.MODSubstation4Master = "16QAM 0.87 (DUAL)"; }
                                else if (messageList[3 + i] == "19") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT8"; }
                                else if (messageList[3 + i] == "20") { _mainPageMasterViewModel.MODSubstation4Master = "64QAM 0.75 (DUAL)"; }
                                else if (messageList[3 + i] == "21") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT9"; }
                                else if (messageList[3 + i] == "22") { _mainPageMasterViewModel.MODSubstation4Master = "64QAM 0.92 (DUAL)"; }
                                else if (messageList[3 + i] == "23") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT10"; }
                                else if (messageList[3 + i] == "24") { _mainPageMasterViewModel.MODSubstation4Master = "256QAM 0.81 (DUAL)"; }
                                else if (messageList[3 + i] == "25") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT11"; }
                                else if (messageList[3 + i] == "26") { _mainPageMasterViewModel.MODSubstation4Master = "256QAM 0.94 (SINGLE)"; }
                                else if (messageList[3 + i] == "27") { _mainPageMasterViewModel.MODSubstation4Master = "TRANSIENT12"; }
                                else if (messageList[3 + i] == "28") { _mainPageMasterViewModel.MODSubstation4Master = "256QAM 0.94 (DUAL)"; }
                                else _mainPageMasterViewModel.MODSubstation4Master = (messageList[3 + i]);
                                break;
                            // 119 - RSSIRate4Master
                            case 119:
                                _mainPageMasterViewModel.RSSIRate4Master = messageList[3 + i];
                                break;
                            // 120 - GPSStatus
                            case 120:
                                _mainPageMasterViewModel.GPSStatus = int.Parse(messageList[3 + i]);
                                break;
                            // 121 - HomePeakSelect
                            case 121:
                                _mainPageMasterViewModel.HomePeakSelect = int.Parse(messageList[3 + i]);
                                break;
                            // 122 - Slave1Name
                            case 122:
                                _mainPageMasterViewModel.Slave1AntennaName = (messageList[3 + i]);
                                break;
                            // 123 - Slave2Name
                            case 123:
                                _mainPageMasterViewModel.Slave2AntennaName = (messageList[3 + i]);
                                break;
                            // 124 - Slave3Name
                            case 124:
                                _mainPageMasterViewModel.Slave3AntennaName = (messageList[3 + i]);
                                break;
                            // 125 - Slave4Name
                            case 125:
                                _mainPageMasterViewModel.Slave4AntennaName = (messageList[3 + i]);
                                break;
                        }

                    }
                }
                catch
                {

                }
            }

            else if (messageList[0] == "DM")
            {
                try
                {
                    _mainPageMasterViewModel.MasterNameTxt = messageList[2];
                    _mainPageMasterViewModel.MasterIPAddressTxt = messageList[9];
                    _mainPageMasterViewModel.MasterInstallationOrientationTextBackup = messageList[7];

                    randomNumber = int.Parse(messageList[12]);
                    _stationDBPageMasterViewModel.MasterName = (messageList[2]);
                    _stationDBPageMasterViewModel.LatitudeMaster = (messageList[3]);
                    _stationDBPageMasterViewModel.LongitudeMaster = (messageList[4]);
                    _stationDBPageMasterViewModel.ElevationMaster = (messageList[5]);
                    _stationDBPageMasterViewModel.PoleHeight = (messageList[6]);
                    _stationDBPageMasterViewModel.InstallationOrientation = (messageList[7]);

                    _selfRegMasterViewModel.MasterName = (messageList[2]);
                    _selfRegMasterViewModel.LatitudeMaster = (messageList[3]);
                    _selfRegMasterViewModel.LongitudeMaster = (messageList[4]);
                    _selfRegMasterViewModel.ElevationMaster = (messageList[5]);
                    _selfRegMasterViewModel.PoleHeight = (messageList[6]);
                    _selfRegMasterViewModel.InstallationOrientation = (messageList[7]);

                    _mainPageMasterViewModel.MasterLatitudeTextBackup = (messageList[3]);
                    _mainPageMasterViewModel.MasterLongitudeTextBackup = (messageList[4]);
                    _mainPageMasterViewModel.MasterElevationTextBackup = (messageList[5]);

                    _stationDBPageMasterViewModel.AddStationDBPage
                    (
                        messageList[2],
                        messageList[3],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8],
                        messageList[9],
                        messageList[10],
                        messageList[11]
                    );

                    if (_selfRegMasterViewModel == null)
                    {
                        CreateAppClassAfterDelay();
                    }
                    if (_selfRegMasterViewModel != null)
                    {
                        _selfRegMasterViewModel.MasterName = (messageList[2]);
                        _selfRegMasterViewModel.LatitudeMaster = (messageList[3]);
                        _selfRegMasterViewModel.LongitudeMaster = (messageList[4]);
                        _selfRegMasterViewModel.ElevationMaster = (messageList[5]);
                        _selfRegMasterViewModel.PoleHeight = (messageList[6]);
                        _selfRegMasterViewModel.InstallationOrientation = (messageList[7]);
                    }
                }
                catch
                {

                }
            }
            else if (messageList[0] == "S1")
            {
                if ((messageList[2]) != "")
                {
                    _substationDB1MasterViewModel.Slave1AntennaIPAddress = (messageList[1]);
                    _substationDB1MasterViewModel.Slave1AntennaName = (messageList[2]);
                    _substationDB1MasterViewModel.LatitudeSlave1 = (messageList[4]);
                    _substationDB1MasterViewModel.LongitudeSlave1 = (messageList[5]);
                    _substationDB1MasterViewModel.ElevationSlave1 = (messageList[6]);
                    _substationDB1MasterViewModel.PoleHeight = (messageList[7]);

                    _substationDB1MasterViewModel.canAdd2DB = true;
                    _substationDB1MasterViewModel.AddStationDBPage
                    (
                        messageList[1],
                        messageList[2],
                        messageList[3],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8],
                        messageList[9]
                    );
                    _substationDB1MasterViewModel.canAdd2DB = false;
                    _substationMasterViewModel.AddDataToSlave1
                    (
                        messageList[1],
                        messageList[2],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8]
                    );
                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }

                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                }
            }
            else if (messageList[0] == "S2")
            {
                if ((messageList[2]) != "")
                {
                    _substationDB2MasterViewModel.Slave2AntennaIPAddress = (messageList[1]);
                    _substationDB2MasterViewModel.Slave2AntennaName = (messageList[2]);
                    _substationDB2MasterViewModel.LatitudeSlave2 = (messageList[4]);
                    _substationDB2MasterViewModel.LongitudeSlave2 = (messageList[5]);
                    _substationDB2MasterViewModel.ElevationSlave2 = (messageList[6]);
                    _substationDB2MasterViewModel.PoleHeight = (messageList[7]);

                    _substationDB2MasterViewModel.canAdd2DB = true;
                    _substationDB2MasterViewModel.AddStationDBPage
                    (
                        messageList[1],
                        messageList[2],
                        messageList[3],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8],
                        messageList[9]
                    );
                    _substationDB2MasterViewModel.canAdd2DB = false;
                    _substationMasterViewModel.AddDataToSlave2
                    (
                        messageList[1],
                        messageList[2],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8]
                    );
                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }

                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                }
            }
            else if (messageList[0] == "S3")
            {
                if ((messageList[2]) != "")
                {
                    _substationDB3MasterViewModel.Slave3AntennaIPAddress = (messageList[1]);
                    _substationDB3MasterViewModel.Slave3AntennaName = (messageList[2]);
                    _substationDB3MasterViewModel.LatitudeSlave3 = (messageList[4]);
                    _substationDB3MasterViewModel.LongitudeSlave3 = (messageList[5]);
                    _substationDB3MasterViewModel.ElevationSlave3 = (messageList[6]);
                    _substationDB3MasterViewModel.PoleHeight = (messageList[7]);

                    _substationDB3MasterViewModel.canAdd2DB = true;
                    _substationDB3MasterViewModel.AddStationDBPage
                    (
                        messageList[1],
                        messageList[2],
                        messageList[3],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8],
                        messageList[9]
                    );
                    _substationDB3MasterViewModel.canAdd2DB = false;
                    _substationMasterViewModel.AddDataToSlave3
                    (
                        messageList[1],
                        messageList[2],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8]
                    );
                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }

                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                }
            }
            else if (messageList[0] == "S4")
            {
                if ((messageList[2]) != "")
                {
                    _substationDB4MasterViewModel.Slave4AntennaIPAddress = (messageList[1]);
                    _substationDB4MasterViewModel.Slave4AntennaName = (messageList[2]);
                    _substationDB4MasterViewModel.LatitudeSlave4 = (messageList[4]);
                    _substationDB4MasterViewModel.LongitudeSlave4 = (messageList[5]);
                    _substationDB4MasterViewModel.ElevationSlave4 = (messageList[6]);
                    _substationDB4MasterViewModel.PoleHeight = (messageList[4]);

                    _substationDB4MasterViewModel.canAdd2DB = true;
                    _substationDB4MasterViewModel.AddStationDBPage
                    (
                        messageList[1],
                        messageList[2],
                        messageList[3],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8],
                        messageList[9]
                    );
                    _substationDB4MasterViewModel.canAdd2DB = false;
                    _substationMasterViewModel.AddDataToSlave4
                    (
                        messageList[1],
                        messageList[2],
                        messageList[4],
                        messageList[5],
                        messageList[6],
                        messageList[7],
                        messageList[8]
                    );
                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4AntennaIPAddressBackupText = (messageList[2] + "," + messageList[1]);
                        _mainPageMasterViewModel.Slave1AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave2AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave3AntennaIPAddress = "";
                        _mainPageMasterViewModel.Slave4AntennaIPAddress = "";
                    }

                    if (_mainPageMasterViewModel.Slave1AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave1JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave2AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave2JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave3AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave3JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                    else if (_mainPageMasterViewModel.Slave4AntennaName == messageList[2])
                    {
                        _mainPageMasterViewModel.Slave4JapaneseNameBackupText = (messageList[2] + "," + messageList[3]);
                    }
                }
            }
            else if (messageList[0] == "SY")
            {
                try
                {
                    int dataCount = messageList.Count;
                    if (dataCount < 5)
                    {
                        for (int i = 1; i < dataCount; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    if (int.Parse(messageList[i]) != 1)
                                    {
                                        //_systemSettingMasterViewModel.SelectedMode = int.Parse(messageList[i]);
                                        _connectionMaster.CloseConnection();

                                        UNDAIRestartMessageBox messageBox = new UNDAIRestartMessageBox();
                                        var tcs = new TaskCompletionSource<bool>();

                                        messageBox.Closed += (s, e) =>
                                        {
                                            tcs.SetResult(messageBox.Result);
                                        };
                                        messageBox.Activate();
                                        bool result = await tcs.Task;

                                        if (result)
                                        {
                                            _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                        }
                                        else
                                        {
                                            _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                        }
                                        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _connectionMaster.fileName);
                                        string[] savedData = File.ReadLines(_filePath).First().Split(',');
                                        savedData[0] = messageList[i];
                                        // Write newIPAddress to the file
                                        try
                                        {
                                            using (StreamWriter writer = new StreamWriter(_filePath, false))
                                            {
                                                writer.WriteLine(string.Join(",", savedData));
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle any errors that may occur
                                            _mainPageMasterViewModel.MasterAlarmData = $"Error saving IP address: {ex.Message}";
                                        }
                                    }
                                    break;
                                case 2:
                                    newIPAddress = messageList[i];
                                    if (newIPAddress != _connectionMaster.currentServerIP)
                                    {
                                        // Define the file path in the application installation directory
                                        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _connectionMaster.fileName);
                                        string[] savedData = File.ReadLines(_filePath).First().Split(',');
                                        savedData[1] = newIPAddress;
                                        // Write newIPAddress to the file
                                        try
                                        {
                                            using (StreamWriter writer = new StreamWriter(_filePath, false))
                                            {
                                                writer.WriteLine(string.Join(",", savedData));
                                            }
                                            // Optionally, update the currentServerIP with the newIPAddress
                                            _connectionMaster.currentServerIP = newIPAddress;
                                            _connectionMaster.CloseConnection();

                                            UNDAIRestartMessageBox messageBox = new UNDAIRestartMessageBox();
                                            var tcs = new TaskCompletionSource<bool>();

                                            messageBox.Closed += (s, e) =>
                                            {
                                                tcs.SetResult(messageBox.Result);
                                            };
                                            messageBox.Activate();
                                            bool result = await tcs.Task;

                                            if (result)
                                            {
                                                _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                            }
                                            else
                                            {
                                                _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle any errors that may occur
                                            _mainPageMasterViewModel.MasterAlarmData = $"Error saving Seleced method address: {ex.Message}";
                                        }
                                    }
                                    break;
                                case 3:
                                    if (int.Parse(messageList[i]) != 1)
                                    {
                                        _connectionMaster.CloseConnection();

                                        UNDAIRestartMessageBox messageBox = new UNDAIRestartMessageBox();
                                        var tcs = new TaskCompletionSource<bool>();

                                        messageBox.Closed += (s, e) =>
                                        {
                                            tcs.SetResult(messageBox.Result);
                                        };
                                        messageBox.Activate();
                                        bool result = await tcs.Task;

                                        if (result)
                                        {
                                            _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                        }
                                        else
                                        {
                                            _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < dataCount; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    if (int.Parse(messageList[i]) != 1)
                                    {
                                        ContentDialog dialog = new ContentDialog
                                        {
                                            Title = "DEVICE RECONNECTION REQUEST",
                                            Content = "The operation is different. Please reconnect the device.",
                                            PrimaryButtonText = "OK",
                                            XamlRoot = app.xamlRoot
                                        };

                                        ContentDialogResult result = await dialog.ShowAsync();
                                        if (result == ContentDialogResult.Primary)
                                        {
                                            // Optionally, do something here if the user clicks "OK"
                                            _connectionMaster.CloseConnection();
                                            _systemSettingMasterViewModel.SelectedMode = int.Parse(messageList[i]);
                                            _systemSettingSlaveViewModel.SelectedMode = int.Parse(messageList[i]);
                                            _mainPageMasterViewModel.ToSlaveCommand.Execute(null);
                                            string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _connectionMaster.fileName);
                                            string[] savedData = File.ReadLines(_filePath).First().Split(',');
                                            savedData[0] = messageList[i];
                                            // Write newIPAddress to the file
                                            try
                                            {
                                                using (StreamWriter writer = new StreamWriter(_filePath, false))
                                                {
                                                    writer.WriteLine(string.Join(",", savedData));
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                // Handle any errors that may occur
                                                _mainPageMasterViewModel.MasterAlarmData = $"Error saving Seleced method address: {ex.Message}";
                                            }
                                        }
                                    }
                                    break;
                                case 2:
                                    newIPAddress = messageList[i];
                                    if (newIPAddress != _connectionMaster.currentServerIP)
                                    {
                                        // Define the file path in the application installation directory
                                        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _connectionMaster.fileName);
                                        string[] savedData = File.ReadLines(_filePath).First().Split(',');
                                        savedData[1] = newIPAddress;
                                        // Write newIPAddress to the file
                                        try
                                        {
                                            using (StreamWriter writer = new StreamWriter(_filePath, false))
                                            {
                                                writer.WriteLine(string.Join(",", savedData));
                                            }

                                            // Optionally, update the currentServerIP with the newIPAddress
                                            _connectionMaster.currentServerIP = newIPAddress;
                                            _connectionMaster.CloseConnection();
                                            _systemSettingMasterViewModel.MainPageNavigateCommand.Execute(null);
                                        }
                                        catch (Exception ex)
                                        {
                                            // Handle any errors that may occur
                                            _mainPageMasterViewModel.MasterAlarmData = $"Error saving IP address: {ex.Message}";
                                        }
                                    }
                                    break;
                                case 3:
                                    _systemSettingMasterViewModel.UndaiSubnet = messageList[i];
                                    break;
                                case 4:
                                    _systemSettingMasterViewModel.DefaultGateway = messageList[i];
                                    break;
                                case 5:
                                    _systemSettingMasterViewModel.MasterAntennaIPAddress = messageList[i];
                                    break;
                                case 6:
                                    _systemSettingMasterViewModel.OriginOffsetAzimuth = messageList[i];
                                    break;
                                case 7:
                                    _systemSettingMasterViewModel.OriginOffsetElevation = messageList[i];
                                    break;
                                case 8:
                                    _systemSettingMasterViewModel.HighSpeedSetAzimuth = messageList[i];
                                    break;
                                case 9:
                                    _systemSettingMasterViewModel.HighSpeedSetElevation = messageList[i];
                                    break;
                                case 10:
                                    _systemSettingMasterViewModel.LowSpeedSetAzimuth = messageList[i];
                                    break;
                                case 11:
                                    _systemSettingMasterViewModel.LowSpeedSetElevation = messageList[i];
                                    break;
                                case 12:
                                    _systemSettingMasterViewModel.InchingSpeedSetAzimuth = messageList[i];
                                    break;
                                case 13:
                                    _systemSettingMasterViewModel.InchingSpeedSetElevation = messageList[i];
                                    break;
                                case 14:
                                    _systemSettingMasterViewModel.PeakSearchSpeed = messageList[i];
                                    break;
                                case 15:
                                    _systemSettingMasterViewModel.PeakSearchAzimuth = messageList[i];
                                    break;
                                case 16:
                                    _systemSettingMasterViewModel.PeakSearchElevation = messageList[i];
                                    break;
                                case 17:
                                    _systemSettingMasterViewModel.PeakSearchRSSILevel = messageList[i];
                                    break;
                                case 18:
                                    _systemSettingMasterViewModel.PeakSearchRSSITurnLevel = messageList[i];
                                    break;
                                case 19:
                                    _systemSettingMasterViewModel.PeakSearchRSSIDelay = messageList[i];
                                    break;
                                case 20:
                                    _systemSettingMasterViewModel.DetailedPeakSearchStepAngle = messageList[i];
                                    break;
                                case 21:
                                    _systemSettingMasterViewModel.UpDownSearchAngle = messageList[i];
                                    break;
                                case 22:
                                    _systemSettingMasterViewModel.DetailedPeakSearchRSSILevel = messageList[i];
                                    break;
                                case 23:
                                    _systemSettingMasterViewModel.StepStability = messageList[i];
                                    break;
                                case 24:
                                    _systemSettingMasterViewModel.RSSITurnbackThresholdLevel = messageList[i];
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _mainPageMasterViewModel.MasterAlarmData = ex.Message;
                }
            }
            else if (messageList[0] == "SA")
            {
                if (messageList.Count == 3)
                {
                    _alarmHistoryMasterViewModel.AddAlarmHistory
                    (
                        messageList[1],
                        messageList[2],
                        messageList[3]
                    );
                }
                else
                {
                    _alarmHistoryMasterViewModel.AddAlarmHistory
                    (
                        DateTime.Now.Date.ToString("yyyy-MM-dd"),
                        DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"),
                        messageList[1]
                    );
                }
            }
            else if (messageList[0] == "SI")
            {
            }
            
        }

        public async Task Button_Click(int Identifier)
        {
            switch (Identifier)
            {
                case 1:
                    // UP ARROW MOUSE PRESSED - I,1\n
                    MessageSend("I," + (1).ToString() + "\n");
                    break;

                case 2:
                    // UP ARROW MOUSE RELEASED - I,2\n
                    MessageSend("I," + (2).ToString() + "\n");
                    break;

                case 3:
                    // DOWN ARROW MOUSE PRESSED - I,3\n
                    MessageSend("I," + (3).ToString() + "\n");
                    break;

                case 4:
                    // DOWN ARROW MOUSE RELEASED - I,4\n
                    MessageSend("I," + (4).ToString() + "\n");
                    break;

                case 5:
                    // CCW ARROW MOUSE PRESSED - I,5\n
                    MessageSend("I," + (5).ToString() + "\n");
                    break;

                case 6:
                    // CCW ARROW MOUSE RELEASED - I,6\n
                    MessageSend("I," + (6).ToString() + "\n");
                    break;

                case 7:
                    // CW ARROW MOUSE PRESSED - I,7\n
                    MessageSend("I," + (7).ToString() + "\n");
                    break;

                case 8:
                    // CW ARROW MOUSE RELEASED - I,8\n
                    MessageSend("I," + (8).ToString() + "\n");
                    break;


                case 9:
                    // HIGH SPEED COMMAND - I,18\n
                    MessageSend("I," + (18).ToString() + "\n");
                    break;

                case 10:
                    // LOW SPEED COMMAND - I,19\n
                    MessageSend("I," + (19).ToString() + "\n");
                    break;

                case 11:
                    // INCHING SPEED COMMAND - I,20\n
                    MessageSend("I," + (20).ToString() + "\n");
                    break;

                case 12:
                    // ELEVATION ANGLE SET COMMAND - R,1,Value\n
                    if(_mainPageMasterViewModel.ElevationAngleInputMaster != "")
                    {
                        MessageSend("R," + (1).ToString() +"," + _mainPageMasterViewModel.ElevationAngleInputMaster + "\n");
                    }
                    break;

                case 13:
                    // AZIMUTH ANGLE SET COMMAND - R,2,Value\n
                    if(_mainPageMasterViewModel.AzimuthAngleInputMaster != "")
                    {
                        MessageSend("R," + (2).ToString() +"," + _mainPageMasterViewModel.AzimuthAngleInputMaster + "\n");
                    }
                    break;

                case 14:
                    // SAVE ANGLE COMMAND - I,21\n
                    MessageSend("I," + (21).ToString() + "\n");
                    break;

                case 15:
                    // LOAD ANGLE COMMAND - I,22\n
                    MessageSend("I," + (22).ToString() + "\n");
                    break;

                case 16:
                    // UNIT CHANGE COMMAND - DMS/ DD
                    if (_mainPageMasterViewModel.UnitMainMaster == "DMS")
                    {
                        _mainPageMasterViewModel.UnitMainMaster = "DD";
                    }
                    else
                    {
                        _mainPageMasterViewModel.UnitMainMaster = "DMS";
                    }
                    break;

                case 17:
                    // STOP COMMAND - I,30\n
                    MessageSend("I," + (30).ToString() + "\n");
                    break;
                case 18:
                    // HOME POSITION COMMAND PRESS - I,13\n
                    pressedTime = DateTime.Now;
                    isBtnPointerPressed = true;
                    await LongPressCheck(pressedTime);
                    if (isLongPressed)
                    {
                        MessageSend("I," + (13).ToString() + "\n");
                        isLongPressed = false;
                    }
                    break;
                case 19:
                    // HOME POSITION COMMAND RELEASE - I,13\n
                    isBtnPointerPressed = false;
                    break;
                case 20:
                    // PEAK SEARCH COMMAND PRESS - I,29\n
                    pressedTime = DateTime.Now;
                    isBtnPointerPressed = true;
                    await LongPressCheck(pressedTime);
                    if (isLongPressed)
                    {
                        MessageSend("I," + (29).ToString() + "\n");
                        isLongPressed = false;
                    }
                    break;
                case 21:
                    // PEAK SEARCH COMMAND RELEASE - I,29\n
                    isBtnPointerPressed = false;
                    break;
                case 22:
                    // Direction SEARCH COMMAND PRESS - I,17\n
                    pressedTime = DateTime.Now;
                    isBtnPointerPressed = true;
                    await LongPressCheck(pressedTime);
                    if (isLongPressed)
                    {
                        MessageSend("I," + (17).ToString() + "\n");
                        isLongPressed = false;
                    }
                    break;
                case 23:
                    // PEAK SEARCH COMMAND RELEASE - I,17\n
                    isBtnPointerPressed = false;
                    break;
            }
        }
        // Date and Time
        private void StartTimer()
        {
            _timer = new Timer(1000); // Update every second
            _timer.Elapsed += OnTimerElapsed;
            _timer.Start();
        }

        // Date and Time
        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            UpdateDateTime();
        }

        // Date and Time
        private void UpdateDateTime()
        {
            _mainPageMasterViewModel.TxtBlockTime = DateTime.Now.ToString("HH:mm:ss");
            _mainPageMasterViewModel.TxtBlockDate = DateTime.Now.ToShortDateString();
        }

        public async Task LongPressCheck(DateTime pressedTime)
        {
            while (isBtnPointerPressed)
            {
                if ((DateTime.Now - pressedTime).TotalMilliseconds > 1000)
                {
                    isLongPressed = true;
                    isBtnPointerPressed = false;
                }
                await Task.Delay(50);
            }
        }
        public static bool IsValidIPAddress(string ipAddress)
        {
            // Use IPAddress.TryParse to check if the input string is a valid IP address
            if (!ipAddress.Contains('.'))
            {
                return false;
            }
            if((ipAddress.Split('.').Length - 1) != 3)
            {
                return false;
            }
            return IPAddress.TryParse(ipAddress, out _);
        }
        public static bool IsValidSubnetMask(string subnetMask)
        {
            if (IPAddress.TryParse(subnetMask, out IPAddress address))
            {
                byte[] bytes = address.GetAddressBytes();

                // Check if it's a valid subnet mask (between 255.0.0.0 and 255.255.255.255)
                // A valid subnet mask must start with 255 and follow CIDR mask rules (continuously filled bits)
                return IsSubnetMask(bytes);
            }

            return false; // Invalid IP format
        }

        private static bool IsSubnetMask(byte[] bytes)
        {
            uint mask = 0;

            // Convert subnet mask to an integer by shifting and OR-ing
            for (int i = 0; i < bytes.Length; i++)
            {
                mask = (mask << 8) | bytes[i];
                if (bytes[i]<0 || bytes[i] > 255)
                {
                    return false;
                }
            }

            // Subnet masks are a contiguous sequence of 1s followed by 0s, e.g. 11111111 11111111 00000000 00000000
            // If we OR mask and mask + 1, the result must be all 1s for a valid subnet mask
            return true;
        }
    }
}