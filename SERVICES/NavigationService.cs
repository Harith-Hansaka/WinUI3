using UNDAI.SERVICES;
using UNDAI.VIEWS;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using UNDAI.VIEWS.MASTER;
using UNDAI.VIEWS.SLAVE;

public class NavigationService : INavigationService
{
    private Frame _frame;
    private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();

    public NavigationService()
    {
        // Register pages in constructor
        _pages.Add("AlarmHistoryMasterView"         , typeof(AlarmHistoryMasterView));
        _pages.Add("MainPageMasterView"             , typeof(MainPageMasterView));
        _pages.Add("SelfRegMasterView"              , typeof(SelfRegMasterView));
        _pages.Add("StationDBMasterView"            , typeof(StationDBMasterView));
        _pages.Add("Substation1ElevationProfileView", typeof(Substation1ElevationProfileView));
        _pages.Add("SubstationDB1MasterView"        , typeof(SubstationDB1MasterView));
        _pages.Add("SubstationDB2MasterView"        , typeof(SubstationDB2MasterView));
        _pages.Add("SubstationDB3MasterView"        , typeof(SubstationDB3MasterView));
        _pages.Add("SubstationDB4MasterView"        , typeof(SubstationDB4MasterView));
        _pages.Add("SubstationMasterView"           , typeof(SubstationMasterView));
        _pages.Add("SystemResetSettingMasterView"   , typeof(SystemResetSettingMasterView));
        _pages.Add("SystemSettingMasterView"        , typeof(SystemSettingMasterView));
        _pages.Add("AlarmHistorySlaveView"          , typeof(AlarmHistorySlaveView));
        _pages.Add("BaseStationRegSlaveView"        , typeof(BaseStationRegSlaveView));
        _pages.Add("MainPageSlaveView"              , typeof(MainPageSlaveView));
        _pages.Add("SelfRegSlaveView"               , typeof(SelfRegSlaveView));
        _pages.Add("StationDBSlaveView"             , typeof(StationDBSlaveView));
        _pages.Add("SystemResetSettingSlaveView"    , typeof(SystemResetSettingSlaveView));
        _pages.Add("SystemSettingSlaveView"         , typeof(SystemSettingSlaveView));
    }

    public void Initialize(Frame frame)
    {
        _frame = frame;
    }

    public bool NavigateTo(string pageKey)
    {
        if (_pages.ContainsKey(pageKey))
        {
            return _frame.Navigate(_pages[pageKey]);
        }
        return false;
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            _frame.GoBack();
            return true;
        }
        return false;
    }

    public bool CanGoBack => _frame?.CanGoBack ?? false;
}