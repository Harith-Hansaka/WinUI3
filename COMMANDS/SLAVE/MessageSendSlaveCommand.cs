using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNDAI.MODELS.SLAVE;
using UNDAI.VIEWMODELS.SLAVE;
using UNDAI.COMMANDS.BASE;

namespace UNDAI.COMMANDS.SLAVE
{
    public class MessageSendSlaveCommand : CommandBase
    {
        private ConnectionSlave _connectionSlave;
        private MainPageSlaveViewModel _mainPageSlaveViewModel;
        SendDataSlave _sendDataSlave;
        String _message;
        bool _isLongPressed;

        public MessageSendSlaveCommand(ConnectionSlave connectionSlave, MainPageSlaveViewModel mainPageSlaveViewModel, String message) 
        {
            _connectionSlave = connectionSlave;
            _mainPageSlaveViewModel = mainPageSlaveViewModel;
            _isLongPressed = mainPageSlaveViewModel.IsSendDataLongPress;
            _message = message;
        }

        public override void Execute(object? parameter)
        {

            if (_connectionSlave.client != null && _connectionSlave.client.Connected)
            {
                _isLongPressed = _mainPageSlaveViewModel.IsSendDataLongPress;
                _sendDataSlave = new SendDataSlave(_connectionSlave.client, _message, _isLongPressed);
            }
        }
    }
}
