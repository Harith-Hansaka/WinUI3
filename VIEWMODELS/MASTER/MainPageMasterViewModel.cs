using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Threading;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;

namespace UNDAI.VIEWMODELS.MASTER;

public class MainPageMasterViewModel : ViewModelBase
{
    private int commandNo;
    public int ReceivingDataCount1;
    public bool PINKeyboardClose = false;
    public string Slave1AntennaIPAddressBackupText = " , ";
    public string Slave2AntennaIPAddressBackupText = " , ";
    public string Slave3AntennaIPAddressBackupText = " , ";
    public string Slave4AntennaIPAddressBackupText = " , ";
    public string Slave1JapaneseNameBackupText = "*,*";
    public string Slave2JapaneseNameBackupText = "*,*";
    public string Slave3JapaneseNameBackupText = "*,*";
    public string Slave4JapaneseNameBackupText = "*,*";
    public string MasterNameTextBackup;
    public string MasterAntennaNameTextBackup;
    public string MasterLatitudeTextBackup;
    public string MasterLongitudeTextBackup;
    public string MasterElevationTextBackup;
    public string MasterInstallationOrientationTextBackup = "0";
    public bool _masterChecked = true;
    public bool _slaveChecked = false;
    public bool isPINClickedSys = false;

    TimeSpan _time;
    private DateTime sendDataPressedTimer;
    public DateTime SendDataPressedTimer
    {
        get { return sendDataPressedTimer; }
        set { sendDataPressedTimer = value; }
    }

    private string slave1Name = "Slave1";
    public string Slave1Name
    {
        get
        {
            if (isNameShowSub1)
            {
                slave1Name = Slave1JapaneseName;
            }
            else
            {
                slave1Name = Slave1AntennaName;
            }
            return slave1Name;
        }
        set
        {
            if (isNameShowSub1)
            {
                slave1Name = Slave1JapaneseName;
            }
            else
            {
                slave1Name = Slave1AntennaName;
            }
            OnPropertyChanged(nameof(Slave1Name));
        }
    }

    private string slave2Name = "Slave2";
    public string Slave2Name
    {
        get
        {
            if (isNameShowSub2)
            {
                slave2Name = Slave2JapaneseName;
            }
            else
            {
                slave2Name = Slave2AntennaName;
            }
            return slave2Name;
        }
        set
        {
            if (isNameShowSub2)
            {
                slave2Name = Slave2JapaneseName;
            }
            else
            {
                slave2Name = Slave2AntennaName;
            }
            OnPropertyChanged(nameof(Slave2Name));
        }
    }

    private string slave3Name = "Slave3";
    public string Slave3Name
    {
        get
        {
            if (isNameShowSub3)
            {
                slave3Name = Slave3JapaneseName;
            }
            else
            {
                slave3Name = Slave3AntennaName;
            }
            return slave3Name;
        }
        set
        {
            if (isNameShowSub3)
            {
                slave3Name = Slave3JapaneseName;
            }
            else
            {
                slave3Name = Slave3AntennaName;
            }
            OnPropertyChanged(nameof(Slave3Name));
        }
    }

    private string slave4Name = "Slave4";
    public string Slave4Name
    {
        get
        {
            if (isNameShowSub4)
            {
                slave4Name = Slave4JapaneseName;
            }
            else
            {
                slave4Name = Slave4AntennaName;
            }
            return slave4Name;
        }
        set
        {
            if (isNameShowSub4)
            {
                slave4Name = Slave4JapaneseName;
            }
            else
            {
                slave4Name = Slave4AntennaName;
            }
            OnPropertyChanged(nameof(Slave4Name));
        }
    }

    private string slave1AntennaName = "N/C";
    public string Slave1AntennaName
    {
        get { return slave1AntennaName; }
        set
        {
            slave1AntennaName = value;
            OnPropertyChanged(nameof(Slave1AntennaName));
        }
    }

    private string slave1AntennaIPAddress = "";
    public string Slave1AntennaIPAddress
    {
        get { return slave1AntennaIPAddress; }
        set
        {
            if (IsIPAddressShowSub1)
            {
                if (Slave1AntennaName == Slave1AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave1AntennaIPAddress = Slave1AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave2AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave1AntennaIPAddress = Slave2AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave3AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave1AntennaIPAddress = Slave3AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave4AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave1AntennaIPAddress = Slave4AntennaIPAddressBackupText.Split(',')[1];
                }
            }
            else
            {
                slave1AntennaIPAddress = "";
            }
            OnPropertyChanged(nameof(Slave1AntennaIPAddress));
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

    private bool isIPAddressShowSub1 = false;
    public bool IsIPAddressShowSub1
    {
        get
        {
            return isIPAddressShowSub1;
        }
        set
        {
            isIPAddressShowSub1 = value;
            OnPropertyChanged(nameof(IsIPAddressShowSub1));
            Slave1AntennaIPAddress = "";

        }
    }

    private string slave2AntennaName = "N/C";
    public string Slave2AntennaName
    {
        get { return slave2AntennaName; }
        set
        {
            slave2AntennaName = value;
            OnPropertyChanged(nameof(Slave2AntennaName));
        }
    }

    private string slave2AntennaIPAddress = "";
    public string Slave2AntennaIPAddress
    {
        get { return slave2AntennaIPAddress; }
        set
        {
            if (IsIPAddressShowSub2)
            {
                if (Slave2AntennaName == Slave1AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave2AntennaIPAddress = Slave1AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave2AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave2AntennaIPAddress = Slave2AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave3AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave2AntennaIPAddress = Slave3AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave4AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave2AntennaIPAddress = Slave4AntennaIPAddressBackupText.Split(',')[1];
                }
            }
            else
            {
                slave2AntennaIPAddress = "";
            }
            OnPropertyChanged(nameof(Slave2AntennaIPAddress));
        }
    }

    private bool isIPAddressShowSub2 = false;
    public bool IsIPAddressShowSub2
    {
        get
        {
            return isIPAddressShowSub2;
        }
        set
        {
            isIPAddressShowSub2 = value;
            OnPropertyChanged(nameof(IsIPAddressShowSub2));
            Slave2AntennaIPAddress = "";
        }
    }

    private string slave3AntennaName = "N/C";
    public string Slave3AntennaName
    {
        get { return slave3AntennaName; }
        set
        {
            slave3AntennaName = value;
            OnPropertyChanged(nameof(Slave3AntennaName));
        }
    }

    private string slave3AntennaIPAddress = "";
    public string Slave3AntennaIPAddress
    {
        get { return slave3AntennaIPAddress; }
        set
        {
            if (IsIPAddressShowSub3)
            {
                if (Slave3AntennaName == Slave1AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave3AntennaIPAddress = Slave1AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave2AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave3AntennaIPAddress = Slave2AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave3AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave3AntennaIPAddress = Slave3AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave4AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave3AntennaIPAddress = Slave4AntennaIPAddressBackupText.Split(',')[1];
                }
            }
            else
            {
                slave3AntennaIPAddress = "";
            }
            OnPropertyChanged(nameof(Slave3AntennaIPAddress));
        }
    }

    private bool isIPAddressShowSub3 = false;
    public bool IsIPAddressShowSub3
    {
        get
        {
            return isIPAddressShowSub3;
        }
        set
        {
            isIPAddressShowSub3 = value;
            OnPropertyChanged(nameof(IsIPAddressShowSub3));
            Slave3AntennaIPAddress = "";
        }
    }

    private string slave4AntennaName = "N/C";
    public string Slave4AntennaName
    {
        get { return slave4AntennaName; }
        set
        {
            slave4AntennaName = value;
            OnPropertyChanged(nameof(Slave4AntennaName));
        }
    }

    private string slave4AntennaIPAddress = "";
    public string Slave4AntennaIPAddress
    {
        get { return slave4AntennaIPAddress; }
        set
        {
            if (IsIPAddressShowSub4)
            {
                if (Slave4AntennaName == Slave1AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave4AntennaIPAddress = Slave1AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave2AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave4AntennaIPAddress = Slave2AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave3AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave4AntennaIPAddress = Slave3AntennaIPAddressBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave4AntennaIPAddressBackupText.Split(',')[0])
                {
                    slave4AntennaIPAddress = Slave4AntennaIPAddressBackupText.Split(',')[1];
                }
            }
            else
            {
                slave4AntennaIPAddress = "";
            }
            OnPropertyChanged(nameof(Slave4AntennaIPAddress));
        }
    }

    private bool isIPAddressShowSub4 = false;
    public bool IsIPAddressShowSub4
    {
        get
        {
            return isIPAddressShowSub4;
        }
        set
        {
            isIPAddressShowSub4 = value;
            OnPropertyChanged(nameof(IsIPAddressShowSub4));
            Slave4AntennaIPAddress = "";
        }
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

    private bool isNameShowSub1 = false;
    public bool IsNameShowSub1
    {
        get
        {
            return isNameShowSub1;
        }
        set
        {
            isNameShowSub1 = value;
            OnPropertyChanged(nameof(IsNameShowSub1));
            Slave1JapaneseName = "";

        }
    }

    private string slave1JapaneseName = "";
    public string Slave1JapaneseName
    {
        get { return slave1JapaneseName; }
        set
        {
            if (IsNameShowSub1)
            {
                if (Slave1AntennaName == Slave1JapaneseNameBackupText.Split(',')[0])
                {
                    slave1JapaneseName = Slave1JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave2JapaneseNameBackupText.Split(',')[0])
                {
                    slave1JapaneseName = Slave2JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave3JapaneseNameBackupText.Split(',')[0])
                {
                    slave1JapaneseName = Slave3JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave1AntennaName == Slave4JapaneseNameBackupText.Split(',')[0])
                {
                    slave1JapaneseName = Slave4JapaneseNameBackupText.Split(',')[1];
                }
            }
            OnPropertyChanged(nameof(Slave1JapaneseName));
            Slave1Name = "";
        }
    }

    private bool isNameShowSub2 = false;
    public bool IsNameShowSub2
    {
        get
        {
            return isNameShowSub2;
        }
        set
        {
            isNameShowSub2 = value;
            OnPropertyChanged(nameof(IsNameShowSub2));
            Slave2JapaneseName = "";

        }
    }

    private string slave2JapaneseName = "";
    public string Slave2JapaneseName
    {
        get { return slave2JapaneseName; }
        set
        {
            if (IsNameShowSub2)
            {
                if (Slave2AntennaName == Slave1JapaneseNameBackupText.Split(',')[0])
                {
                    slave2JapaneseName = Slave1JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave2JapaneseNameBackupText.Split(',')[0])
                {
                    slave2JapaneseName = Slave2JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave3JapaneseNameBackupText.Split(',')[0])
                {
                    slave2JapaneseName = Slave3JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave2AntennaName == Slave4JapaneseNameBackupText.Split(',')[0])
                {
                    slave2JapaneseName = Slave4JapaneseNameBackupText.Split(',')[1];
                }
            }
            OnPropertyChanged(nameof(Slave2JapaneseName));
            Slave2Name = "";
        }
    }

    private bool isNameShowSub3 = false;
    public bool IsNameShowSub3
    {
        get
        {
            return isNameShowSub3;
        }
        set
        {
            isNameShowSub3 = value;
            OnPropertyChanged(nameof(IsNameShowSub3));
            Slave3JapaneseName = "";

        }
    }

    private string slave3JapaneseName = "";
    public string Slave3JapaneseName
    {
        get { return slave3JapaneseName; }
        set
        {
            if (IsNameShowSub3)
            {
                if (Slave3AntennaName == Slave1JapaneseNameBackupText.Split(',')[0])
                {
                    slave3JapaneseName = Slave1JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave2JapaneseNameBackupText.Split(',')[0])
                {
                    slave3JapaneseName = Slave2JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave3JapaneseNameBackupText.Split(',')[0])
                {
                    slave3JapaneseName = Slave3JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave3AntennaName == Slave4JapaneseNameBackupText.Split(',')[0])
                {
                    slave3JapaneseName = Slave4JapaneseNameBackupText.Split(',')[1];
                }
            }
            OnPropertyChanged(nameof(Slave3JapaneseName));
            Slave3Name = "";
        }
    }

    private bool isNameShowSub4 = false;
    public bool IsNameShowSub4
    {
        get
        {
            return isNameShowSub4;
        }
        set
        {
            isNameShowSub4 = value;
            OnPropertyChanged(nameof(IsNameShowSub4));
            Slave4JapaneseName = "";
        }
    }

    private string slave4JapaneseName = "";
    public string Slave4JapaneseName
    {
        get { return slave4JapaneseName; }
        set
        {
            if (IsNameShowSub4)
            {
                if (Slave4AntennaName == Slave1JapaneseNameBackupText.Split(',')[0])
                {
                    slave4JapaneseName = Slave1JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave2JapaneseNameBackupText.Split(',')[0])
                {
                    slave4JapaneseName = Slave2JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave3JapaneseNameBackupText.Split(',')[0])
                {
                    slave4JapaneseName = Slave3JapaneseNameBackupText.Split(',')[1];
                }
                else if (Slave4AntennaName == Slave4JapaneseNameBackupText.Split(',')[0])
                {
                    slave4JapaneseName = Slave4JapaneseNameBackupText.Split(',')[1];
                }
            }
            OnPropertyChanged(nameof(Slave4JapaneseName));
            Slave4Name = "";
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
            if (connected == "CONNECT")
            {
                RSSISubstation1Master = "";
                RSSISubstation2Master = "";
                RSSISubstation3Master = "";
                RSSISubstation4Master = "";
                RSSIRate1Master = "";
                RSSIRate2Master = "";
                RSSIRate3Master = "";
                RSSIRate4Master = "";
                Latitude102 = "";
                Longitude103 = "";
                SubstationDB1MasterStatusColor = "";
                SubstationDB2MasterStatusColor = "";
                SubstationDB3MasterStatusColor = "";
                SubstationDB4MasterStatusColor = "";
                MasterNameTxt = "";
                MasterIPAddressTxt = "";
                Elevation104 = "";
                InstallationDirection105 = "";
                ElevationAngleInput100 = "";
                AzimuthAngleInput101 = "";
                ElevationAngleInputMaster = "";
                AzimuthAngleInputMaster = "";
                Slave1AntennaName = "N/C";
                Slave1AntennaIPAddress = "N/C";
                Slave2AntennaName = "N/C";
                Slave2AntennaIPAddress = "N/C";
                Slave3AntennaName = "N/C";
                Slave3AntennaIPAddress = "N/C";
                Slave4AntennaName = "N/C";
                Slave4AntennaIPAddress = "N/C";
                MODSubstation1Master = "";
                MODSubstation2Master = "";
                MODSubstation3Master = "";
                MODSubstation4Master = "";

            }
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
            if (Connected == "CONNECT")
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
            if (Connected == "CONNECT")
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
            if (Connected != "CONNECT")
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

            if (Connected != "CONNECT")
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

    private bool substationDB1MasterStatus;
    public string SubstationDB1MasterStatusColor
    {
        get { return substationDB1MasterStatus ? "#00FFFF" : "#ed524f"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB1MasterStatus = false;
            }
            else
            {
                substationDB1MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB1MasterStatusColor));
            OnPropertyChanged(nameof(SubstationDB1MasterStatusText));
            OnPropertyChanged(nameof(ControllerAccessSub1));
        }
    }

    public string SubstationDB1MasterStatusText
    {
        get { return substationDB1MasterStatus ? "LINK UP" : "LINK DOWN"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB1MasterStatus = false;
            }
            else
            {
                substationDB1MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB1MasterStatusText));
        }
    }

    private string controllerAccessSub1 = "Visible";
    public string ControllerAccessSub1
    {
        get
        {
            if (!substationDB1MasterStatus)
            {
                controllerAccessSub1 = "Visible";
            }
            else
            {
                controllerAccessSub1 = "Hidden";
            }
            return controllerAccessSub1;
        }
        set
        {
            if (!substationDB1MasterStatus)
            {
                controllerAccessSub1 = "Visible";
            }
            else
            {
                controllerAccessSub1 = "Hidden";
            }
            OnPropertyChanged(nameof(ControllerAccessSub1));
        }
    }

    private bool substationDB2MasterStatus;
    public string SubstationDB2MasterStatusColor
    {
        get { return substationDB2MasterStatus ? "#00FFFF" : "#ed524f"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB2MasterStatus = false;
            }
            else
            {
                substationDB2MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB2MasterStatusColor));
            OnPropertyChanged(nameof(SubstationDB2MasterStatusText));
            OnPropertyChanged(nameof(ControllerAccessSub2));
        }
    }

    public string SubstationDB2MasterStatusText
    {
        get { return substationDB2MasterStatus ? "LINK UP" : "LINK DOWN"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB2MasterStatus = false;
            }
            else
            {
                substationDB2MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB2MasterStatusText));
        }
    }

    private string controllerAccessSub2 = "Visible";
    public string ControllerAccessSub2
    {
        get
        {
            if (!substationDB2MasterStatus)
            {
                controllerAccessSub2 = "Visible";
            }
            else
            {
                controllerAccessSub2 = "Hidden";
            }
            return controllerAccessSub2;
        }
        set
        {
            if (!substationDB2MasterStatus)
            {
                controllerAccessSub2 = "Visible";
            }
            else
            {
                controllerAccessSub2 = "Hidden";
            }
            OnPropertyChanged(nameof(ControllerAccessSub2));
        }
    }

    private bool substationDB3MasterStatus;
    public string SubstationDB3MasterStatusColor
    {
        get { return substationDB3MasterStatus ? "#00FFFF" : "#ed524f"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB3MasterStatus = false;
            }
            else
            {
                substationDB3MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB3MasterStatusColor));
            OnPropertyChanged(nameof(SubstationDB3MasterStatusText));
            OnPropertyChanged(nameof(ControllerAccessSub3));
        }
    }

    public string SubstationDB3MasterStatusText
    {
        get { return substationDB3MasterStatus ? "LINK UP" : "LINK DOWN"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB3MasterStatus = false;
            }
            else
            {
                substationDB3MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB3MasterStatusText));
        }
    }

    private string controllerAccessSub3 = "Visible";
    public string ControllerAccessSub3
    {
        get
        {
            if (!substationDB3MasterStatus)
            {
                controllerAccessSub3 = "Visible";
            }
            else
            {
                controllerAccessSub3 = "Hidden";
            }
            return controllerAccessSub3;
        }
        set
        {
            if (!substationDB3MasterStatus)
            {
                controllerAccessSub3 = "Visible";
            }
            else
            {
                controllerAccessSub3 = "Hidden";
            }
            OnPropertyChanged(nameof(ControllerAccessSub3));
        }
    }

    private bool substationDB4MasterStatus;

    public string SubstationDB4MasterStatusColor
    {
        get { return substationDB4MasterStatus ? "#00FFFF" : "#ed524f"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB4MasterStatus = false;
            }
            else
            {
                substationDB4MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB4MasterStatusColor));
            OnPropertyChanged(nameof(SubstationDB4MasterStatusText));
            OnPropertyChanged(nameof(ControllerAccessSub4));
        }
    }

    public string SubstationDB4MasterStatusText
    {
        get { return substationDB4MasterStatus ? "LINK UP" : "LINK DOWN"; }
        set
        {
            if (Connected == "CONNECT")
            {
                substationDB4MasterStatus = false;
            }
            else
            {
                substationDB4MasterStatus = (bool.Parse(value));
            }
            OnPropertyChanged(nameof(SubstationDB4MasterStatusText));
        }
    }

    private string controllerAccessSub4 = "Visible";
    public string ControllerAccessSub4
    {
        get
        {
            if (!substationDB4MasterStatus)
            {
                controllerAccessSub4 = "Visible";
            }
            else
            {
                controllerAccessSub4 = "Hidden";
            }
            return controllerAccessSub4;
        }
        set
        {
            if (!substationDB4MasterStatus)
            {
                controllerAccessSub4 = "Visible";
            }
            else
            {
                controllerAccessSub4 = "Hidden";
            }
            OnPropertyChanged(nameof(ControllerAccessSub4));
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
                ReceivingDataCount1++;
                MasterAlarmData = "";
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
                MasterAlarmData = "";
            }
            OnPropertyChanged(nameof(ReceivedData));
        }
    }

    private string masterAlarmData;
    public string MasterAlarmData
    {
        get { return masterAlarmData; }
        set
        {
            masterAlarmData = value;
            OnPropertyChanged(nameof(MasterAlarmData));
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

    private string elevationAngleInput100 = "";
    public string ElevationAngleInput100
    {
        get => elevationAngleInput100;
        set
        {
            if (Connected == "CONNECT")
            {
                elevationAngleInput100 = "";
            }
            else
            {
                elevationAngleInput100 = value;
            }
            OnPropertyChanged(nameof(ElevationAngleInput100));
        }
    }

    private string azimuthAngleInput101 = "";
    public string AzimuthAngleInput101
    {
        get => azimuthAngleInput101;
        set
        {
            if (Connected == "CONNECT")
            {
                azimuthAngleInput101 = "";
            }
            else
            {
                azimuthAngleInput101 = value;
            }
            OnPropertyChanged(nameof(AzimuthAngleInput101));
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
                if (UnitMainMaster == "DMS")
                {
                    double fractional = 0.0;
                    double minutes = 0.0;
                    int degrees = 0, wholeMinutes = 0, seconds = 0;
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
                if (UnitMainMaster == "DMS")
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
            if (Connected == "CONNECT")
            {
                elevation104 = "";
            }
            else
            {
                elevation104 = value;
            }
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

    private string stillConnectionCheckCount;
    public string StillConnectionCheckCount
    {
        get { return stillConnectionCheckCount; }
        set
        {
            stillConnectionCheckCount = value.ToString();
            OnPropertyChanged(nameof(StillConnectionCheckCount));
        }
    }

    private string stillConnectionCheckCount1;
    public string StillConnectionCheckCount1
    {
        get { return stillConnectionCheckCount1; }
        set
        {
            stillConnectionCheckCount1 = value.ToString();
            OnPropertyChanged(nameof(StillConnectionCheckCount1));
        }
    }

    private string stillConnectionCheckCount2;
    public string StillConnectionCheckCount2
    {
        get { return stillConnectionCheckCount2; }
        set
        {
            stillConnectionCheckCount2 = value.ToString();
            OnPropertyChanged(nameof(StillConnectionCheckCount2));
        }
    }

    private string elevationAngleInputMaster = "";
    public string ElevationAngleInputMaster
    {
        get { return elevationAngleInputMaster; }
        set
        {
            elevationAngleInputMaster = value;
            OnPropertyChanged(nameof(ElevationAngleInputMaster));
            OnPropertyChanged(nameof(UpDownArrowPositionSetMainPageEnabled));
        }
    }

    private string azimuthAngleInputMaster = "";
    public string AzimuthAngleInputMaster
    {
        get { return azimuthAngleInputMaster; }
        set
        {
            azimuthAngleInputMaster = value;
            OnPropertyChanged(nameof(AzimuthAngleInputMaster));
            OnPropertyChanged(nameof(RotatePositionSetMainPageEnabled));
        }
    }

    private string rotatePositionSetMainPageEnabled = "Visible";
    public string RotatePositionSetMainPageEnabled
    {
        get
        {
            if (AzimuthAngleInputMaster == "" || AzimuthAngleInputMaster == null)
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
            if (AzimuthAngleInputMaster == "" || AzimuthAngleInputMaster == null)
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
            if (ElevationAngleInputMaster == "" || ElevationAngleInputMaster == null)
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
            if (ElevationAngleInputMaster == "" || ElevationAngleInputMaster == null)
            {
                upDownArrowPositionSetMainPageEnabled = "Visible";
            }
            else
            {
                upDownArrowPositionSetMainPageEnabled = "Hidden";
            }
        }
    }

    private string unitMainMaster = "DD";
    public string UnitMainMaster
    {
        get => unitMainMaster;
        set
        {
            unitMainMaster = value;
            OnPropertyChanged(nameof(UnitMainMaster));
        }
    }

    private string rSSISubstation1Master;
    public string RSSISubstation1Master
    {
        get => rSSISubstation1Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSISubstation1Master = "";
            }
            else
            {
                rSSISubstation1Master = value;
            }
            OnPropertyChanged(nameof(RSSISubstation1Master));
        }
    }

    private string rSSISubstation2Master;
    public string RSSISubstation2Master
    {
        get => rSSISubstation2Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSISubstation2Master = "";
            }
            else
            {
                rSSISubstation2Master = value;
            }
            OnPropertyChanged(nameof(RSSISubstation2Master));
        }
    }

    private string rSSISubstation3Master;
    public string RSSISubstation3Master
    {
        get => rSSISubstation3Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSISubstation3Master = "";
            }
            else
            {
                rSSISubstation3Master = value;
            }
            OnPropertyChanged(nameof(RSSISubstation3Master));
        }
    }

    private string rSSISubstation4Master;
    public string RSSISubstation4Master
    {
        get => rSSISubstation4Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSISubstation4Master = "";
            }
            else
            {
                rSSISubstation4Master = value;
            }
            OnPropertyChanged(nameof(RSSISubstation4Master));
        }
    }

    private string mODSubstation1Master;
    public string MODSubstation1Master
    {
        get => mODSubstation1Master;
        set
        {
            if (Connected == "CONNECT")
            {
                mODSubstation1Master = "";
            }
            else
            {
                mODSubstation1Master = value;
            }
            OnPropertyChanged(nameof(MODSubstation1Master));
        }
    }

    private string mODSubstation2Master;
    public string MODSubstation2Master
    {
        get => mODSubstation2Master;
        set
        {
            if (Connected == "CONNECT")
            {
                mODSubstation2Master = "";
            }
            else
            {
                mODSubstation2Master = value;
            }
            OnPropertyChanged(nameof(MODSubstation2Master));
        }
    }

    private string mODSubstation3Master;
    public string MODSubstation3Master
    {
        get => mODSubstation3Master;
        set
        {
            if (Connected == "CONNECT")
            {
                mODSubstation3Master = "";
            }
            else
            {
                mODSubstation3Master = value;
            }
            OnPropertyChanged(nameof(MODSubstation3Master));
        }
    }

    private string mODSubstation4Master;
    public string MODSubstation4Master
    {
        get => mODSubstation4Master;
        set
        {
            if (Connected == "CONNECT")
            {
                mODSubstation4Master = "";
            }
            else
            {
                mODSubstation4Master = value;
            }
            OnPropertyChanged(nameof(MODSubstation4Master));
        }
    }

    private string rSSIRate1Master;
    public string RSSIRate1Master
    {
        get => rSSIRate1Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSIRate1Master = "";
            }
            else
            {
                rSSIRate1Master = (float.Parse(value) / 1000).ToString();
            }
            OnPropertyChanged(nameof(RSSIRate1Master));
        }
    }

    private string rSSIRate2Master;
    public string RSSIRate2Master
    {
        get => rSSIRate2Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSIRate2Master = "";
            }
            else
            {
                rSSIRate2Master = (float.Parse(value) / 1000).ToString();
            }
            OnPropertyChanged(nameof(RSSIRate2Master));
        }
    }

    private string rSSIRate3Master;
    public string RSSIRate3Master
    {
        get => rSSIRate3Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSIRate3Master = "";
            }
            else
            {
                rSSIRate3Master = (float.Parse(value) / 1000).ToString();
            }
            OnPropertyChanged(nameof(RSSIRate3Master));
        }
    }

    private string rSSIRate4Master;
    public string RSSIRate4Master
    {
        get => rSSIRate4Master;
        set
        {
            if (Connected == "CONNECT")
            {
                rSSIRate4Master = "";
            }
            else
            {
                rSSIRate4Master = (float.Parse(value) / 1000).ToString();
            }
            OnPropertyChanged(nameof(RSSIRate4Master));
        }
    }

    private string masterNameTxt;
    public string MasterNameTxt
    {
        get
        {
            return masterNameTxt;
        }
        set
        {
            masterNameTxt = value;
            OnPropertyChanged(nameof(MasterNameTxt));
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
            if (IsIPAddressShow)
            {
                //masterIPAddressTxt = _stationDBPageMasterViewModel.MasterAntennaIPAddress;
            }
            else
            {
                masterIPAddressTxt = "";
            }
            OnPropertyChanged(nameof(MasterIPAddressTxt));
        }
    }

    private bool isIPAddressShow = false;
    public bool IsIPAddressShow
    {
        get
        {
            return isIPAddressShow;
        }
        set
        {
            isIPAddressShow = value;
            OnPropertyChanged(nameof(IsIPAddressShow));
            MasterIPAddressTxt = "";
        }
    }

    private bool isMasterNameShow = true;
    public bool IsMasterNameShow
    {
        get
        {
            return isMasterNameShow;
        }
        set
        {
            isMasterNameShow = value;
            OnPropertyChanged(nameof(IsMasterNameShow));
            MasterNameTxt = "";
        }
    }
    private readonly NavigationService       _navigationService;
    public ConnectionMaster                  _connectionMaster;
    public MainPageMasterModel               _mainPageMasterModel;
    public SelfRegMasterViewModel            _selfRegMasterViewModel;
    public SystemSettingMasterViewModel      _systemSettingMasterViewModel;
    public StationDBMasterViewModel          _stationDBMasterViewModel;
    //public ElevationCalculationModel         _elevationCalculationModel;
    public SubstationMasterViewModel         _substationMasterViewModel;
    public SubstationDB1MasterViewModel      _substationDB1MasterViewModel;
    public SubstationDB2MasterViewModel      _substationDB2MasterViewModel;
    public SubstationDB3MasterViewModel      _substationDB3MasterViewModel;
    public SubstationDB4MasterViewModel      _substationDB4MasterViewModel;
    public AlarmHistoryMasterViewModel       _alarmHistoryMasterViewModel;
    public SystemResetSettingMasterViewModel _systemResetSettingMasterViewModel;
    public App app;

    public ICommand ConnectCommandMouseDownCommand { get; }
    public ICommand ConnectCommandMouseUpCommand { get; }
    public ICommand BtnUpArrowMainPageMouseDownCommand { get; }
    public ICommand BtnUpArrowMainPageMouseUpCommand { get; }
    public ICommand BtnDownArrowMainPageMouseDownCommand { get; }
    public ICommand BtnDownArrowMainPageMouseUpCommand { get; }
    public ICommand BtnCCWMainPageMouseDownCommand { get; }
    public ICommand BtnCCWMainPageMouseUpCommand { get; }
    public ICommand BtnCWMainPageMouseDownCommand { get; }
    public ICommand BtnCWMainPageMouseUpCommand { get; }
    public ICommand ToSlaveCommand { get; }
    public ICommand SelfRegMasterCommand { get; }
    public ICommand StationDBMasterCommand { get; }
    public ICommand SubstationDB1MasterCommand { get; }
    public ICommand SubstationDB2MasterCommand { get; }
    public ICommand SubstationDB3MasterCommand { get; }
    public ICommand SubstationDB4MasterCommand { get; }
    public ICommand SubstationMasterCommand { get; }
    public ICommand LowSpeedBtnMasterCommand { get; }
    public ICommand HighSpeedBtnMasterCommand { get; }
    public ICommand InchingSpeedBtnMasterCommand { get; }
    public ICommand ElevationAngleSetMasterCommand { get; }
    public ICommand AzimuthAngleSetMasterCommand { get; }
    public ICommand SaveAngleMainPageMasterCommand { get; }
    public ICommand LoadLoadAngleMainPageMasterCommand { get; }
    public ICommand UnitMainMasterCommand { get; }
    public ICommand StopCommand { get; }
    public ICommand HomePositionDownCommand { get; }
    public ICommand HomePositionUpCommand { get; }
    public ICommand PeakSearchDownCommand { get; }
    public ICommand PeakSearchUpCommand { get; }
    public ICommand DirectionSearchDownCommand { get; }
    public ICommand DirectionSearchUPCommand { get; }
    public ICommand SystemResetSettingMasterPage { get; }
    public ICommand SystemSettingMasterPage { get; }
    public ICommand NullCommand { get; }

    public ICommand systemSettingMasterCommand;
    public ICommand? SystemSettingMasterCommand
    {
        get
        {
            if (Connected == "CONNECT")
            {
                systemSettingMasterCommand = SystemResetSettingMasterPage;
            }
            else
            {
                systemSettingMasterCommand = SystemSettingMasterPage;
            }
            return systemSettingMasterCommand;
        }
        set
        {
            if (Connected == "CONNECT")
            {
                systemSettingMasterCommand = SystemResetSettingMasterPage;
            }
            else
            {
                systemSettingMasterCommand = SystemSettingMasterPage;
            }
            OnPropertyChanged(nameof(SystemSettingMasterCommand));
        }
    }
    public MainPageMasterViewModel
    (
        NavigationService navigationService,
        ConnectionMaster connectionMaster,
        SystemSettingMasterViewModel systemSettingMasterViewModel,
        StationDBMasterViewModel stationDBMasterViewModel,
        SubstationMasterViewModel substationMasterViewModel,
        SubstationDB1MasterViewModel substationDB1MasterViewModel,
        SubstationDB2MasterViewModel substationDB2MasterViewModel,
        SubstationDB3MasterViewModel substationDB3MasterViewModel,
        SubstationDB4MasterViewModel substationDB4MasterViewModel,
        AlarmHistoryMasterViewModel alarmHistoryMasterViewModel,
        SystemResetSettingMasterViewModel resetSettingMasterViewModel
    )
    {
        _navigationService = navigationService;
        _connectionMaster = connectionMaster;
        _systemResetSettingMasterViewModel = resetSettingMasterViewModel;
        _systemSettingMasterViewModel = systemSettingMasterViewModel;
        _stationDBMasterViewModel = stationDBMasterViewModel;
        _substationMasterViewModel = substationMasterViewModel;
        _substationDB1MasterViewModel = substationDB1MasterViewModel;
        _substationDB2MasterViewModel = substationDB2MasterViewModel;
        _substationDB3MasterViewModel = substationDB3MasterViewModel;
        _substationDB4MasterViewModel = substationDB4MasterViewModel;
        _alarmHistoryMasterViewModel = alarmHistoryMasterViewModel;


        ConnectCommandMouseDownCommand               = new RelayCommand(() => _connectionMaster.ConnectClick(this, 1));
        ConnectCommandMouseUpCommand                 = new RelayCommand(() => _connectionMaster.ConnectClick(this, 2));
        _mainPageMasterModel                         = new MainPageMasterModel(this, connectionMaster);
        BtnUpArrowMainPageMouseDownCommand           = new RelayCommand(() => _mainPageMasterModel.Button_Click(1));
        BtnUpArrowMainPageMouseUpCommand             = new RelayCommand(() => _mainPageMasterModel.Button_Click(2));
        BtnDownArrowMainPageMouseDownCommand         = new RelayCommand(() => _mainPageMasterModel.Button_Click(3));
        BtnDownArrowMainPageMouseUpCommand           = new RelayCommand(() => _mainPageMasterModel.Button_Click(4));
        BtnCCWMainPageMouseDownCommand               = new RelayCommand(() => _mainPageMasterModel.Button_Click(5));
        BtnCCWMainPageMouseUpCommand                 = new RelayCommand(() => _mainPageMasterModel.Button_Click(6));
        BtnCWMainPageMouseDownCommand                = new RelayCommand(() => _mainPageMasterModel.Button_Click(7));
        BtnCWMainPageMouseUpCommand                  = new RelayCommand(() => _mainPageMasterModel.Button_Click(8));
        HighSpeedBtnMasterCommand                    = new RelayCommand(() => _mainPageMasterModel.Button_Click(9));
        LowSpeedBtnMasterCommand                     = new RelayCommand(() => _mainPageMasterModel.Button_Click(10));
        InchingSpeedBtnMasterCommand                 = new RelayCommand(() => _mainPageMasterModel.Button_Click(11));
        ElevationAngleSetMasterCommand               = new RelayCommand(() => _mainPageMasterModel.Button_Click(12));
        SaveAngleMainPageMasterCommand               = new RelayCommand(() => _mainPageMasterModel.Button_Click(14));
        AzimuthAngleSetMasterCommand                 = new RelayCommand(() => _mainPageMasterModel.Button_Click(13));
        LoadLoadAngleMainPageMasterCommand           = new RelayCommand(() => _mainPageMasterModel.Button_Click(15));
        UnitMainMasterCommand                        = new RelayCommand(() => _mainPageMasterModel.Button_Click(16));
        StopCommand                                  = new RelayCommand(() => _mainPageMasterModel.Button_Click(17));
        HomePositionDownCommand                      = new RelayCommand(() => _mainPageMasterModel.Button_Click(18));
        HomePositionUpCommand                        = new RelayCommand(() => _mainPageMasterModel.Button_Click(19));
        PeakSearchDownCommand                        = new RelayCommand(() => _mainPageMasterModel.Button_Click(20));
        PeakSearchUpCommand                          = new RelayCommand(() => _mainPageMasterModel.Button_Click(21));
        DirectionSearchDownCommand                   = new RelayCommand(() => _mainPageMasterModel.Button_Click(22));
        DirectionSearchUPCommand                     = new RelayCommand(() => _mainPageMasterModel.Button_Click(23));

        //_elevationCalculationModel          = new ElevationCalculationModel();
        SubstationMasterCommand             = new NavigateCommand(_navigationService, "StationDBMasterView");
        SystemSettingMasterPage             = new NavigateCommand(_navigationService, "SystemSettingMasterView");
        ToSlaveCommand                      = new NavigateCommand(_navigationService, "MainPageSlaveView");
        SelfRegMasterCommand                = new NavigateCommand(_navigationService, "SelfRegMasterView");
        StationDBMasterCommand              = new NavigateCommand(_navigationService, "StationDBMasterView");
        SubstationDB2MasterCommand          = new NavigateCommand(_navigationService, "SubstationDB1MasterView");
        SubstationDB1MasterCommand          = new NavigateCommand(_navigationService, "SubstationDB2MasterView");
        SubstationDB3MasterCommand          = new NavigateCommand(_navigationService, "SubstationDB3MasterView");
        SubstationDB4MasterCommand          = new NavigateCommand(_navigationService, "SubstationDB4MasterView");
        SubstationMasterCommand             = new NavigateCommand(_navigationService, "SubstationMasterView");
        SystemSettingMasterPage             = new NavigateCommand(_navigationService, "SystemSettingMasterView");
        SystemResetSettingMasterPage        = new NavigateCommand(_navigationService, "SystemResetSettingMasterView");
        app = (App)Application.Current;
    }

}