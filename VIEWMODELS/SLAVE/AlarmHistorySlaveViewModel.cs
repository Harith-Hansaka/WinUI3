using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UNDAI.COMMANDS.BASE;
using UNDAI.MODELS.SLAVE;
using UNDAI.SERVICES;

namespace UNDAI.VIEWMODELS.SLAVE;

public class AlarmHistorySlaveViewModel : ViewModelBase
{
    private readonly NavigationService _navigationService;
    public ICommand SystemSettingSlaveNavigateCommand { get; }
    public ICommand MainPageSlaveNavigateCommand { get; }
    public ICommand DeleteSelectedItemCommand { get; }
    public ICommand AlarmHistoryExportCommand { get; }

    AlarmHistorySlaveModel newAlarmHistorySlaveModel;
    int alarmHistorySlaveModelCount = 0;
    int alarmHistoryCount = 25;

    // Specify the file path for the CSV output
    string baseDirectory = AppContext.BaseDirectory;
    string filePath;

    public AlarmHistorySlaveViewModel(NavigationService navigationService)
    {
        _navigationService = navigationService;
        AlarmHistorySlaveModel = new ObservableCollection<AlarmHistorySlaveModel>();
        DeleteSelectedItemCommand = new RelayCommand(DeleteSelectedItem);
        AlarmHistoryExportCommand = new RelayCommand(() => ExportAlarmHistoryModelSlaveToCsv(alarmHistorySlaveModel));

        for (int i = 1; i < alarmHistoryCount; i++)
        {
            AddAlarmHistory
            (
                "", "", ""
            );
        }
        filePath = baseDirectory + "AlarmHistorySlaveExport.csv";
        ReadAlarmHistoryFromFile(filePath);
    }

    private ObservableCollection<AlarmHistorySlaveModel> alarmHistorySlaveModel;
    public ObservableCollection<AlarmHistorySlaveModel> AlarmHistorySlaveModel
    {
        get { return alarmHistorySlaveModel; }
        set
        {
            alarmHistorySlaveModel = value;
            OnPropertyChanged(nameof(AlarmHistorySlaveModel));
        }
    }

    public void AddAlarmHistory
    (
        String Date,
        String Time,
        string AlarmName
    )
    {
        if (alarmHistorySlaveModelCount < alarmHistoryCount || alarmHistorySlaveModel.Count < alarmHistoryCount)
        {
            if (Date == "" & Time == "" & AlarmName == "")
            {
                newAlarmHistorySlaveModel = new AlarmHistorySlaveModel
                {
                    ID = null,
                    Date = Date,
                    Time = Time,
                    AlarmName = AlarmName
                };

                alarmHistorySlaveModelCount++;
                AlarmHistorySlaveModel.Add(newAlarmHistorySlaveModel);
            }
            else
            {
                newAlarmHistorySlaveModel = new AlarmHistorySlaveModel
                {
                    ID = alarmHistorySlaveModelCount + 1,
                    Date = Date,
                    Time = Time,
                    AlarmName = AlarmName
                };

                alarmHistorySlaveModelCount++;
                AlarmHistorySlaveModel.Add(newAlarmHistorySlaveModel);
            }

        }
        else
        {
            // Update the existing item at the current index
            int indexToUpdate = alarmHistorySlaveModelCount % alarmHistoryCount;
            AlarmHistorySlaveModel[indexToUpdate] = new AlarmHistorySlaveModel
            {
                ID = indexToUpdate + 1,  // Update ID to match the new entry position
                Date = Date,
                Time = Time,
                AlarmName = AlarmName
            };

            alarmHistorySlaveModelCount++;
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

    private AlarmHistorySlaveModel _selectedAlarmHistorySlaveModel;
    public AlarmHistorySlaveModel SelectedAlarmHistorySlaveModel
    {
        get { return _selectedAlarmHistorySlaveModel; }
        set
        {
            _selectedAlarmHistorySlaveModel = value;
            OnPropertyChanged(nameof(SelectedAlarmHistorySlaveModel));
        }
    }

    public void DeleteSelectedItem()
    {
        if (SelectedAlarmHistorySlaveModel != null)
        {
            AlarmHistorySlaveModel.Remove(SelectedAlarmHistorySlaveModel);
            AddAlarmHistory
            (
                "", "", ""
            );
        }
    }

    public bool CanDeleteSelectedItem()
    {
        return SelectedAlarmHistorySlaveModel != null;
    }

    public void ExportAlarmHistoryModelSlaveToCsv(ObservableCollection<AlarmHistorySlaveModel> alarmHistorySlaveModel)
    {
        var sb = new StringBuilder();

        // Define the column headers based on the properties of AlarmHistorySlaveModel
        sb.AppendLine
            (
                "ID," +
                "Date," +
                "Time," +
                "Alarm Name"
            );

        // Iterate over the collection to get the row data
        foreach (var station in alarmHistorySlaveModel)
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
