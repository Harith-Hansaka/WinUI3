using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.MASTER;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.MASTER;

public class AlarmHistoryMasterViewModel : ViewModelBase
{
    public ICommand SystemSettingMasterNavigateCommand { get; }
    public ICommand MainPageMasterNavigateCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand AlarmHistoryExportCommand { get; }

    AlarmHistoryMasterModel newAlarmHistoryMasterModel;
    int alarmHistoryMasterModelCount = 0;
    int alarmHistoryCount = 25;

    // Specify the file path for the CSV output
    string baseDirectory = AppContext.BaseDirectory;
    string filePath;

    private readonly NavigationService _navigationService;
    public AlarmHistoryMasterViewModel(NavigationService navigationService)
    {
        MainPageMasterNavigateCommand = new NavigateCommand(navigationService, "MainPageMasterView");
        SystemSettingMasterNavigateCommand = new NavigateCommand(navigationService, "SystemSettingMasterView");

        AlarmHistoryMasterModel = new ObservableCollection<AlarmHistoryMasterModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        AlarmHistoryExportCommand = new RelayCommand(() => ExportAlarmHistoryModelMasterToCsv(alarmHistoryMasterModel));

        for (int i = 1; i < alarmHistoryCount; i++)
        {
            AddAlarmHistory
            (
                "", "", ""
            );
        }
        filePath = baseDirectory + "AlarmHistoryMasterExport.csv";
        ReadAlarmHistoryFromFile(filePath);
    }

    private ObservableCollection<AlarmHistoryMasterModel> alarmHistoryMasterModel;
    public ObservableCollection<AlarmHistoryMasterModel> AlarmHistoryMasterModel
    {
        get { return alarmHistoryMasterModel; }
        set
        {
            alarmHistoryMasterModel = value;
            OnPropertyChanged(nameof(AlarmHistoryMasterModel));
        }
    }

    public void AddAlarmHistory
    (
        String Date,
        String Time,
        string AlarmName
    )
    {
        if (alarmHistoryMasterModelCount < alarmHistoryCount || alarmHistoryMasterModel.Count < alarmHistoryCount)
        {
            if (Date == "" & Time == "" & AlarmName == "")
            {
                newAlarmHistoryMasterModel = new AlarmHistoryMasterModel
                {
                    ID = null,
                    Date = Date,
                    Time = Time,
                    AlarmName = AlarmName
                };

                alarmHistoryMasterModelCount++;
                AlarmHistoryMasterModel.Add(newAlarmHistoryMasterModel);
            }
            else
            {
                newAlarmHistoryMasterModel = new AlarmHistoryMasterModel
                {
                    ID = alarmHistoryMasterModelCount + 1,
                    Date = Date,
                    Time = Time,
                    AlarmName = AlarmName
                };

                alarmHistoryMasterModelCount++;
                AlarmHistoryMasterModel.Add(newAlarmHistoryMasterModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = alarmHistoryMasterModelCount % alarmHistoryCount;
            AlarmHistoryMasterModel[indexToUpdate] = new AlarmHistoryMasterModel
            {
                ID = indexToUpdate + 1,  // Update ID to match the new entry position
                Date = Date,
                Time = Time,
                AlarmName = AlarmName
            };

            alarmHistoryMasterModelCount++;
        }
    }


    // Method to read the text file
    public void ReadAlarmHistoryFromFile(string _filePath)
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using (StreamReader reader = new StreamReader(_filePath))
                {
                    // Skip the header line
                    string headerLine = reader.ReadLine();
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        // Split the line into fields
                        string[] fields = line.Split(',');
                        if (fields[0] != "")
                        {
                            AddAlarmHistory(fields[1], fields[2], fields[3]);
                        }
                    }
                }
            }
            else
            {
                AddAlarmHistory("", "", "");
            }
        }
        catch (Exception)
        {
            AddAlarmHistory("", "", "");
        }
    }

    private AlarmHistoryMasterModel _selectedAlarmHistoryMasterModel;
    public AlarmHistoryMasterModel SelectedAlarmHistoryMasterModel
    {
        get { return _selectedAlarmHistoryMasterModel; }
        set
        {
            _selectedAlarmHistoryMasterModel = value;
            OnPropertyChanged(nameof(SelectedAlarmHistoryMasterModel));
        }
    }

    public void DeleteSelectedItem()
    {
        if (SelectedAlarmHistoryMasterModel != null)
        {
            AlarmHistoryMasterModel.Remove(SelectedAlarmHistoryMasterModel);
            AddAlarmHistory
            (
                "", "", ""
            );
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedAlarmHistoryMasterModel != null;
    }

    public void ExportAlarmHistoryModelMasterToCsv(ObservableCollection<AlarmHistoryMasterModel> alarmHistoryMasterModel)
    {
        var sb = new StringBuilder();

        // Define the column headers based on the properties of AlarmHistoryMasterModel
        sb.AppendLine
            (
                "ID," +
                "Date," +
                "Time," +
                "Alarm Name"
            );

        // Iterate over the collection to get the row data
        foreach (var station in alarmHistoryMasterModel)
        {
            sb.AppendLine
                (
                    $"{station.ID}," +
                    $"{station.Date}," +
                    $"{station.Time}," +
                    $"{station.AlarmName}"
                );
        }

        // Write the CSV content to a file
        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }
}
