using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.IO;
using UNDAI.MODELS.MASTER;
using UNDAI.MODELS.SLAVE;
using UNDAI.VIEWMODELS.MASTER;
using UNDAI.VIEWMODELS.SLAVE;
using UNDAI.VIEWS.SLAVE;

namespace UNDAI
{
    public partial class App : Application
    {
        public XamlRoot xamlRoot;
        public static Window MainWindow { get; private set; }
        public ConnectionMaster connectionMaster { get; private set; }
        public ConnectionSlave connectionSlave { get; private set; }
        public NavigationService NavigationService { get; private set; }
        public AlarmHistoryMasterViewModel alarmHistoryMasterViewModel { get; private set; }
        public MainPageMasterViewModel mainPageMasterViewModel { get; private set; }
        public SelfRegMasterViewModel selfRegMasterViewModel { get; private set; }
        public StationDBMasterViewModel stationDBMasterViewModel { get; private set; }
        public SubstationDB1MasterViewModel substationDB1MasterViewModel { get; private set; }
        public SubstationDB2MasterViewModel substationDB2MasterViewModel { get; private set; }
        public SubstationDB3MasterViewModel substationDB3MasterViewModel { get; private set; }
        public SubstationDB4MasterViewModel substationDB4MasterViewModel { get; private set; }
        public SubstationMasterViewModel substationMasterViewModel { get; private set; }
        public SystemResetSettingMasterViewModel systemResetSettingMasterViewModel { get; private set; }
        public SystemSettingMasterViewModel systemSettingMasterViewModel { get; private set; }
        public AlarmHistorySlaveViewModel alarmHistorySlaveViewModel { get; private set; }
        public BaseStationRegSlaveViewModel baseStationRegSlaveViewModel { get; private set; }
        public MainPageSlaveViewModel mainPageSlaveViewModel { get; private set; }
        public SelfRegSlaveViewModel selfRegSlaveViewModel { get; private set; }
        public StationDBSlaveViewModel stationDBSlaveViewModel { get; private set; }
        public SystemResetSettingSlaveViewModel systemResetSettingSlaveViewModel { get; private set; }
        public SystemSettingSlaveViewModel systemSettingSlaveViewModel { get; private set; }
        public App()
        {
            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                if (args.Exception is FileNotFoundException fileEx)
                {
                    string message = $"File not found: {fileEx.FileName}\n" +
                                   $"Message: {fileEx.Message}\n" +
                                   $"Stack trace: {fileEx.StackTrace}";
                    System.Diagnostics.Debug.WriteLine(message);

                    // Log to file
                    try
                    {
                        File.AppendAllText("error_log.txt", message + "\n\n");
                    }
                    catch { }
                }
            };

            this.UnhandledException += (sender, e) =>
            {
                var ex = e.Exception;
                string message = $"Exception: {ex.GetType()}\n" +
                               $"Message: {ex.Message}\n" +
                               $"Stack trace: {ex.StackTrace}";
                System.Diagnostics.Debug.WriteLine(message);

                // Log to file
                try
                {
                    File.AppendAllText("error_log.txt", message + "\n\n");
                }
                catch { }

                e.Handled = true;
            };
            this.InitializeComponent();

            // Add unhandled exception handling
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                if (ex is FileNotFoundException fileEx)
                {
                    Debug.WriteLine($"File not found: {fileEx.FileName}");
                }
                else if (ex is UnauthorizedAccessException)
                {
                    Debug.WriteLine("Unauthorized access to file");
                }
                Debug.WriteLine($"Exception: {ex?.Message}");
                Debug.WriteLine($"Stack trace: {ex?.StackTrace}");
            };

            NavigationService                 = new NavigationService();
            connectionMaster                  = new ConnectionMaster();
            connectionSlave                   = new ConnectionSlave();
            alarmHistoryMasterViewModel       = new AlarmHistoryMasterViewModel(NavigationService);
            stationDBMasterViewModel          = new StationDBMasterViewModel(NavigationService);
            substationDB1MasterViewModel      = new SubstationDB1MasterViewModel(NavigationService, connectionMaster); 
            substationDB2MasterViewModel      = new SubstationDB2MasterViewModel(NavigationService, connectionMaster);
            substationDB3MasterViewModel      = new SubstationDB3MasterViewModel(NavigationService, connectionMaster);
            substationDB4MasterViewModel      = new SubstationDB4MasterViewModel(NavigationService, connectionMaster);
            substationMasterViewModel         = new SubstationMasterViewModel(NavigationService, connectionMaster);
            systemResetSettingMasterViewModel = new SystemResetSettingMasterViewModel(NavigationService, connectionMaster);
            systemSettingMasterViewModel      = new SystemSettingMasterViewModel(NavigationService, connectionMaster);
            mainPageMasterViewModel           = new MainPageMasterViewModel
                                                    (
                                                        NavigationService, 
                                                        connectionMaster, 
                                                        systemSettingMasterViewModel,
                                                        stationDBMasterViewModel,
                                                        substationMasterViewModel,
                                                        substationDB1MasterViewModel,
                                                        substationDB2MasterViewModel,
                                                        substationDB3MasterViewModel,
                                                        substationDB4MasterViewModel,
                                                        alarmHistoryMasterViewModel,
                                                        systemResetSettingMasterViewModel
                                                    );
            selfRegMasterViewModel            = new SelfRegMasterViewModel(NavigationService, connectionMaster, mainPageMasterViewModel);
            alarmHistorySlaveViewModel        = new AlarmHistorySlaveViewModel(NavigationService);
            baseStationRegSlaveViewModel      = new BaseStationRegSlaveViewModel(NavigationService, connectionSlave);
            mainPageSlaveViewModel            = new MainPageSlaveViewModel(NavigationService, connectionSlave);
            selfRegSlaveViewModel             = new SelfRegSlaveViewModel(NavigationService, connectionSlave, mainPageSlaveViewModel);
            stationDBSlaveViewModel           = new StationDBSlaveViewModel(NavigationService);
            systemResetSettingSlaveViewModel  = new SystemResetSettingSlaveViewModel(NavigationService, connectionSlave);
            systemSettingSlaveViewModel       = new SystemSettingSlaveViewModel(NavigationService, connectionSlave);
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            try
            {
                // Check if RESOURCES directory exists
                var resourcePath = Path.Combine(AppContext.BaseDirectory, "RESOURCES");
                if (!Directory.Exists(resourcePath))
                {
                    Debug.WriteLine($"Resources directory not found at: {resourcePath}");
                    Directory.CreateDirectory(resourcePath);
                }

                MainWindow = new MainWindow();
                MainWindow.Activate();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Launch error: {ex.Message}");
            }
        }
    }
}
