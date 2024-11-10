using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.MODELS.MASTER
{
    public class ReceiveDataMaster
    {
        bool _connected;
        TcpClient? _client;
        public MainPageMasterViewModel _mainPageMasterViewModel;
        public SelfRegMasterViewModel selfRegPageMasterViewModel;
        public MainPageMasterModel _mainPageMasterModel;
        public SelfRegMasterModel _selfRegPageMasterModel;
        public SystemSettingMasterViewModel _systemSettingMasterViewModel;
        ConnectionMaster _connectionMaster;
        int ReceivingDataCount=0;

        bool _isSendDataLongPressed;
        public string response;
        public string result;
        private readonly CancellationTokenSource _cancellationTokenSource;
        string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"MasterResponse.txt");

        public ReceiveDataMaster(bool connected, TcpClient client, MainPageMasterViewModel mainPageMasterViewModel, ConnectionMaster connectionMaster)
        {
            _connected = connected;
            _connectionMaster = connectionMaster;
            _client = _connectionMaster.client;
            _mainPageMasterViewModel = mainPageMasterViewModel;
            _cancellationTokenSource = new CancellationTokenSource();
            response = string.Empty; // Initialize the response field
            _mainPageMasterModel = _mainPageMasterViewModel._mainPageMasterModel;
            selfRegPageMasterViewModel = _mainPageMasterViewModel._selfRegMasterViewModel;
            _systemSettingMasterViewModel = _mainPageMasterViewModel._systemSettingMasterViewModel;
            StartReceiving(_cancellationTokenSource.Token);
        }

        public async Task StartReceiving(CancellationToken cancellationToken)
        {
            try 
            {
                while (_connected && !cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        NetworkStream stream = _client.GetStream();
                        var buffer = new byte[200];
                        int received = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                        if (received > 0 && !buffer.All(x => x== 0))
                        {
                            response = Encoding.UTF8.GetString(buffer, 0, received);
                            using (StreamWriter writer = new StreamWriter(_filePath, true, Encoding.UTF8))
                            {
                                writer.WriteLine(response); // This writes the response on a new line
                            }
                            _mainPageMasterViewModel.ReceivingDataCount = ReceivingDataCount;
                            // Update the UI with the received response
                            if (response.Contains('\n') && response.IndexOf("\n")>3)
                            {
                                result = response.Substring(0, response.IndexOf("\n")-1);
                                ReceivingDataCount++;
                                _systemSettingMasterViewModel.ReceivedData = result;
                                _systemSettingMasterViewModel.ReceivedData2 = result;
                                _mainPageMasterModel.ReceivedMessageAsync(result);
                            }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        _mainPageMasterViewModel.ReceivedData = "";
                        _mainPageMasterViewModel.MasterAlarmData = "Receiving operation canceled.";
                        break;
                    }
                    catch (SocketException ex)
                    {
                        _mainPageMasterViewModel.ReceivedData = "";
                        _mainPageMasterViewModel.MasterAlarmData = $"Socket error: {ex.Message}";
                        break;
                    }
                    catch (Exception ex)
                    {
                        _mainPageMasterViewModel.ReceivedData = "";
                        _mainPageMasterViewModel.MasterAlarmData = $"Unexpected error: {ex.Message}";
                        break;
                    }
                    await Task.Delay(10);
                }
            }
            finally
            {
                _connected = false; // or other state management
                if(_client != null)
                {
                    _connectionMaster.CloseConnection();
                    _client = null;
                }
            }
        }

        public void StopReceiving()
        {
            _cancellationTokenSource.Cancel();
            _mainPageMasterViewModel.ReceivedData = "";
        }

    }
}