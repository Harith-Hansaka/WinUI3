using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNDAI.VIEWMODELS;
using UNDAI.VIEWMODELS.MASTER;

namespace UNDAI.MODELS.MASTER
{
    public class LongPressHandleMaster
    {
        private MainPageMasterViewModel _mainPageMasterViewModel;
        private MainPageMasterModel _mainPageMasterModel;
        public bool isBtnPointerPressed = false;

        public LongPressHandleMaster(MainPageMasterViewModel mainPageMasterViewModel, MainPageMasterModel mainPageMasterModel)
        {
            _mainPageMasterViewModel = mainPageMasterViewModel;
            _mainPageMasterModel = mainPageMasterModel;
        }

        public async Task LongPressCheck(int identifier, DateTime pressedTime)
        {
            while (isBtnPointerPressed)
            {
                if (identifier == 1)
                {
                    if ((DateTime.Now - pressedTime).TotalMilliseconds > 2000)
                    {
                        _mainPageMasterViewModel.IsSendDataLongPress = true;
                        isBtnPointerPressed = false;
                    }
                }

                else if (identifier == 2)
                {
                    if ((DateTime.Now - pressedTime).TotalMilliseconds > 2000)
                    {
                        _mainPageMasterModel.isBtnUnitNoUpLongPress = true;
                        isBtnPointerPressed = false;
                    }
                }

                else if (identifier == 3)
                {
                    if ((DateTime.Now - pressedTime).TotalMilliseconds > 2000)
                    {
                        _mainPageMasterModel.isBtnUnitNoDownLongPress = true;
                        isBtnPointerPressed = false;
                    }
                }
                await Task.Delay(50);
            }
        }
    }
}