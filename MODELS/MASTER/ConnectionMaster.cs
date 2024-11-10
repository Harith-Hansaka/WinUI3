using Microsoft.UI.Xaml;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.MODELS.MASTER
{
    public class ConnectionMaster
    {
        // CLIENT SOCKET
        public TcpClient? client;

        // SERVER INFO

        // Get the directory where the application is installed
        string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        // Define the file name
        public string fileName = "ServerIPAddress.txt";
        private string _filePath;
        public string currentServerIP;
        public string defaultServerIP = "169.254.1.110";
        public string? newServerIP;
        public IPAddress serverIP;
        const int serverPort = 5625;
        public IPEndPoint iPEndPoint;
        bool connected;
        int _identifier;
        DateTime pressedTime;
        bool isBtnPointerPressed;
        bool isBtnPointerReleased = true;
        bool isLongPressed = false;
        bool stillConnected;
        private MainPageMasterViewModel _mainPageMasterViewModel;
        ReceiveDataMaster? _receiveDataMaster;
        App app;
        int[] RandomNumArray = new int[3];
        string? tempRandomNumArray;
        int index = 0;
        bool connectionCheck2 = false;
        bool finished = false;
        public bool loadingImg = false;

        public ConnectionMaster()
        {
            app = (App)Application.Current;
            CreateAppClassAfterDelay();
            // Combine the directory and file name to get the full path
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            currentServerIP = ReadIPAddressFromFile(_filePath);
            serverIP = IPAddress.Parse(currentServerIP);
            iPEndPoint = new IPEndPoint(serverIP, serverPort);
        }

        private void CreateAppClassAfterDelay()
        {
            Thread.Sleep(1);
            if (_mainPageMasterViewModel == null)
            {
                // Access properties directly
                _mainPageMasterViewModel = app.mainPageMasterViewModel;
            }
        }

        // Method to read the text file
        public string ReadIPAddressFromFile(string _filePath)
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    // Read the contents of the file
                    return File.ReadLines(_filePath).First().Split(',')[1];
                }
                else
                {
                    return defaultServerIP;
                }
            }
            catch (Exception)
            {
                return defaultServerIP;
            }
        }

        public async Task ConnectClick(MainPageMasterViewModel mainPageMasterViewModel, int Identifier)
        {
            _mainPageMasterViewModel = mainPageMasterViewModel;
            int[] RandomNumArray = { 1, 2, 3};
            // Combine the directory and file name to get the full path
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            currentServerIP = ReadIPAddressFromFile(_filePath);
            serverIP = IPAddress.Parse(currentServerIP);
            iPEndPoint = new IPEndPoint(serverIP, serverPort);
            app.systemSettingMasterViewModel.UndaiIPAddress = currentServerIP;
            _identifier = Identifier;
            connectionCheck2 = false;
            loadingImg = true;

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            int timeout = 3000; // Timeout in milliseconds (3 seconds)
            cancellationTokenSource.CancelAfter(timeout);

            if (_mainPageMasterViewModel.Connected == "CONNECT" && isBtnPointerReleased)
            {
                if(client == null)
                {
                    client = new TcpClient();
                    try
                    {
                        await client.ConnectAsync(iPEndPoint, cancellationTokenSource.Token);
                        connected = true;
                        _mainPageMasterViewModel.Connected = "CONNECTED";
                        // Start receiving data from server
                        _receiveDataMaster = new ReceiveDataMaster(connected, client, _mainPageMasterViewModel, this);
                        stillConnected = true;
                        _ = ConnectionCheck();
                    }
                    catch (Exception ex)
                    {
                        client = null;
                        _mainPageMasterViewModel.Connected = "CONNECT";
                        _mainPageMasterViewModel.ReceivedData = "";
                        _mainPageMasterViewModel.MasterAlarmData = ex.Message;
                    }
                }
            }
            else
            {
                if (_identifier == 1)
                {
                    pressedTime = DateTime.Now;
                    isBtnPointerPressed = true;
                    isBtnPointerReleased = false;
                    await LongPressCheck(pressedTime);
                }
                else
                {
                    isBtnPointerPressed = false;
                    isBtnPointerReleased = true;
                }
                if (isLongPressed)
                {
                    CloseConnection();
                }
            }
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

        public async Task ConnectionCheck()
        {
            int messageReceivedCountChangeCheck = 0;
            while (stillConnected)
            {
                if (_mainPageMasterViewModel._mainPageMasterModel.messageReceivedCount > 0)
                {
                    int tempRandomNum = _mainPageMasterViewModel._mainPageMasterModel.randomNumber;
                    RandomNumArray[index] = tempRandomNum;
                    index++; 
                    if (index >= RandomNumArray.Length)
                    {
                        index = 0;
                    }
                    _ = ConnectionCheck2();
                    if (connectionCheck2)
                    {
                        break;
                    }
                    messageReceivedCountChangeCheck = 0;
                    await Task.Delay(1000);
                }
                else
                {
                    messageReceivedCountChangeCheck++;
                    if (messageReceivedCountChangeCheck > 5)
                    {
                        CloseConnection();
                    }
                }
                await Task.Delay(1000);
            }
            
        }

        public async Task ConnectionCheck2()
        {
            if (!finished)
            {
                finished = true;
                tempRandomNumArray = string.Join(", ", RandomNumArray);
                await Task.Delay(2000);
                if (tempRandomNumArray == string.Join(", ", RandomNumArray))
                {
                    tempRandomNumArray = string.Join(", ", RandomNumArray);
                    await Task.Delay(1000);
                    if (tempRandomNumArray == string.Join(", ", RandomNumArray))
                    {
                        connectionCheck2 = true;
                        CloseConnection();
                    }
                }
                finished = false;
            }
        }

        public void CloseConnection()
        {
            try
            {
                // Ensure client is not null and close the connection
                if (client != null)
                {
                    client.Close();
                    client = null; // Optionally set client to null after closing
                }
                // Stop receiving data if applicable
                if (_receiveDataMaster != null)
                {
                    _receiveDataMaster.StopReceiving(); // Ensure StopReceiving is awaited if it's async
                    _receiveDataMaster = null;
                }
                connected = false;
                isLongPressed = false;
                stillConnected = false;
                _mainPageMasterViewModel.Connected = "CONNECT";
                _mainPageMasterViewModel.MasterAlarmData = "Connection closed.";
            }
            catch (Exception ex)
            {
                _mainPageMasterViewModel.MasterAlarmData = $"Error closing connection: {ex.Message}";
            }
        }
    }
}