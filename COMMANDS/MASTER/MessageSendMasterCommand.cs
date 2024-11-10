using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNDAI.MODELS.MASTER;
using UNDAI.VIEWMODELS.MASTER;
using UNDAI.COMMANDS.BASE;

namespace UNDAI.COMMANDS.MASTER
{
    public class MessageSendMasterCommand : CommandBase
    {
        private ConnectionMaster _connectionMaster;
        private MainPageMasterViewModel _mainPageMasterViewModel;
        SendDataMaster _sendDataMaster;
        String _message;
        bool _isLongPressed;

        public MessageSendMasterCommand(ConnectionMaster connectionMaster, MainPageMasterViewModel mainPageMasterViewModel, String message) 
        {
            _connectionMaster = connectionMaster;
            _mainPageMasterViewModel = mainPageMasterViewModel;
            _isLongPressed = mainPageMasterViewModel.IsSendDataLongPress;
            _message = message;
        }

        public override void Execute(object? parameter)
        {
            if (_connectionMaster.client != null && _connectionMaster.client.Connected)
            {
                _isLongPressed = _mainPageMasterViewModel.IsSendDataLongPress;
                _sendDataMaster = new SendDataMaster(_connectionMaster.client, _message, _isLongPressed);
            }
        }
    }
}
