using System;
using System.Collections.Generic;
using System.IO;

namespace UNDAI.MODELS.MASTER
{
    public class ElevationCalculationModel
    {
        public StreamReader? reader;
        public StreamReader? memReader;
        public string? currentLine;
        public string? currentMemLine;
        List<string>? stringMEMData;
        public string baseDirectory;
        public string path;
        public string? pathToMAPDATA;
        public string? pathToMEM;
        public string? MEMPath;
        float latitude;
        float longitude;
        int checkLatitude200;
        int checkLongitude200;
        string[]? meshCOdeDetails;

        public ElevationCalculationModel()
        {
            baseDirectory = AppContext.BaseDirectory;
            path = baseDirectory + "RESOURCES\\MAPDATA.csv";
        }

        public float ElevationCalculator(float lat, float lon)
        {
            latitude = lat;
            longitude = lon;

            if (ReadMapDataLine(latitude, longitude) && meshCOdeDetails != null) 
            {
                pathToMEM = string.Concat(baseDirectory, "RESOURCES\\MAP DATA\\", (meshCOdeDetails[0]).AsSpan(0, 4));
                MEMPath = string.Concat(pathToMEM, "\\", meshCOdeDetails[0], ".mem");
                memReader = new StreamReader(MEMPath);
                currentMemLine = memReader.ReadLine();
                currentMemLine = memReader.ReadLine();

                stringMEMData = new List<string>();
                while (currentMemLine != null)
                {
                    stringMEMData.Add((currentMemLine.ToString()).Substring(9));
                    currentMemLine = memReader.ReadLine();
                }
                return (float.Parse((stringMEMData[199 - checkLatitude200]).Substring((checkLongitude200) * 5, 5))) / 10;
            }
            return -999.9f;
        }

        public bool ReadMapDataLine(float unknownLatitude, float unknownLongitude)
        {
            reader = new StreamReader(path);
            currentLine = reader.ReadLine();

            while (currentLine != null)
            {
                meshCOdeDetails = currentLine.Split(',');
                if (checkMapLatitude(ConvertDMS2DD(meshCOdeDetails[1]), ConvertDMS2DD(meshCOdeDetails[3]), unknownLatitude) && checkMapLongitude(ConvertDMS2DD(meshCOdeDetails[2]), ConvertDMS2DD(meshCOdeDetails[4]), unknownLongitude))
                {
                    reader.Close();
                    return true;
                }
                currentLine = reader.ReadLine();
            }
            reader.Close();
            return false;
        }

        private bool checkMapLatitude(float LowLatitude, float HighLatitude, float toCheckLatitude)
        {
            if (LowLatitude <= toCheckLatitude && toCheckLatitude < HighLatitude)
            {
                checkLatitude200 = (int)Math.Round((toCheckLatitude - LowLatitude) * 200 / (HighLatitude - LowLatitude));
                if (checkLatitude200 == 200) { checkLatitude200 = 199; }
                return true;
            }
            else { return false; }
        }

        private bool checkMapLongitude(float LowLongitude, float HighLongitude, float toCheckLongitude)
        {
            if (LowLongitude <= toCheckLongitude && toCheckLongitude < HighLongitude)
            {
                checkLongitude200 = (int)Math.Round((toCheckLongitude - LowLongitude) * 200 / (HighLongitude - LowLongitude));
                if (checkLongitude200 == 200) { checkLongitude200 = 199; }
                return true;
            }
            else { return false; }
        }

        private float ConvertDMS2DD(string DMS)
        {
            // Split the DMS into degrees and the rest
            int Degrees = int.Parse(DMS.Split('.')[0]); // Get the degrees part

            // Check for the existence of the minutes and seconds part
            string[] parts = DMS.Split('.');
            if (parts[1].Length < 4)
            {
                parts[1] = parts[1] + new string('0', 4 - parts[1].Length);
            }
            int Minutes = int.Parse(parts[1].Substring(0, 2)); // Get the minutes
            int Seconds = int.Parse(parts[1].Substring(2, 2)); // Get the seconds

            // Convert DMS to Decimal Degrees
            float dd = Degrees + (Minutes / 60f) + (Seconds / 3600f);
            return dd;
        }
    }
}
