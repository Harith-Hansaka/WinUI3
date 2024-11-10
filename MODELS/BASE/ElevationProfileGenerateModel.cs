using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Path = System.IO.Path;

namespace UNDAI.MODELS.BASE
{
    public class ElevationProfileGenerateModel
    {
        private StreamReader reader;
        private StreamReader memReader;
        private string? currentLine;
        private string? currentMemLine;
        List<float> floatMapData = new List<float>(new float[5]);
        List<string> stringMEMData;
        List<float> MEMDataArray;
        DateTime dateTime;
        string baseDirectory;
        string path;
        string pathToMAPDATA;
        string pathToMEM;
        string MEMPath;
        int checkLatitude200;
        int checkLongitude200;
        float unknownLatitude1 = 34.081704f;
        float unknownLongitude1 = 134.542952f;
        float unknownLatitude2 = 34.081704f;
        float unknownLongitude2 = 134.542952f;
        float unknownLatitude1DMS;
        float unknownLongitude1DMS;
        float unknownLatitude2DMS;
        float unknownLongitude2DMS;
        float slopOfLine;
        float interceptOfYLine;
        public LineSeries _lineSeries;
        PlotModel plotModel;
        public LinearAxis xAxis;
        public LinearAxis yAxis;
        PlotView _plotView;
        string appBaseDir;
        string fileName;
        string filePath;
        string[] meshCOdeDetails;
        string firstPointMeshCode;
        string secondPointMeshCode;
        int firstPointLatitude200;
        int firstPointLongitude200;
        int secondPointLatitude200;
        int secondPointLongitude200;
        int equFormular = 0;
        float incrementValue;
        int numberOfDataPointHorizontal;
        int numberOfDataPointVertical;
        public float distanceBetweenTwoPoints;
        public bool plotGenerated = false;
        public List<float> elevation;

        public ElevationProfileGenerateModel(PlotView plotView)
        {
            baseDirectory = AppContext.BaseDirectory;
            path = baseDirectory + "RESOURCES\\MAPDATA.csv";

            // Get the application base directory
            appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
            // Set the file name
            fileName = "Elevation.txt";
            filePath = Path.Combine(appBaseDir, fileName);

            // Create a new plot model and series
            plotModel = new PlotModel
            {
                Title = "標高プロファイル",
                TitleFontSize = 40,
            };
            _lineSeries = new AreaSeries
            {
                //Title = "Line Series",
                //InterpolationAlgorithm = InterpolationAlgorithms.CanonicalSpline,  // Smooth curve
                Color = OxyColor.Parse("#004b23"),   // Color of the line
                Fill = OxyColor.Parse("#d5f89c"), // Fill color for the area under the graph
            };

            // Add a linear axis for X
            xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Title = "DISTANCE (km)",
                Minimum = 1,  // Set the minimum value for the X axis
                Maximum = 100,  // Set the maximum value for the X axis
                IsZoomEnabled = false, // Disable zooming on the X-axis
                IsPanEnabled = false, // Disable panning on the X-axis
            };
            plotModel.Axes.Add(xAxis);

            // Add a linear axis for Y
            yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "ELEVATION (m)",
                Minimum = 0,    // Set the minimum value for the Y axis
                Maximum = 100,  // Set the maximum value for the Y axis
                IsZoomEnabled = false, // Disable zooming on the Y-axis
                IsPanEnabled = false, // Disable panning on the Y-axis
            };
            plotModel.Axes.Add(yAxis);

            // Add series to the model
            plotModel.Series.Add(_lineSeries);
            _plotView = plotView;
            // Assign the model to the PlotView
            _plotView.Model = plotModel;
        }

        public async Task ElevationProfileGenerator(string Lat1, string Long1, string Lat2, string Long2, CancellationToken cancellationToken)
        {
            stringMEMData = new List<string>();
            bool isComplete = false; // Flag to track if the generation is complete

            try
            {
                for (int k = 0; k < 50; k++)
                {
                    // Check periodically for cancellation
                    cancellationToken.ThrowIfCancellationRequested();

                    await Task.Delay(10, cancellationToken); // Simulating work for 100 milliseconds

                    if (!isComplete)
                    {
                        if
                        (!(
                            string.IsNullOrEmpty(Lat1) ||
                            string.IsNullOrEmpty(Lat2) ||
                            string.IsNullOrEmpty(Long1) ||
                            string.IsNullOrEmpty(Long2) ||
                            !float.TryParse(Lat1, out _) ||
                            !float.TryParse(Lat2, out _) ||
                            !float.TryParse(Long1, out _) ||
                            !float.TryParse(Long2, out _)
                        ))
                        {
                            // Handle invalid input or missing data here

                            if (float.TryParse(Lat1, out unknownLatitude1))
                            {
                                // Round the parsed value
                                unknownLatitude1 = (float)Math.Round(unknownLatitude1, 4);
                            }

                            if (float.TryParse(Long1, out unknownLongitude1))
                            {
                                // Round the parsed value
                                unknownLongitude1 = (float)Math.Round(unknownLongitude1, 4);
                            }

                            if (float.TryParse(Lat2, out unknownLatitude2))
                            {
                                // Round the parsed value
                                unknownLatitude2 = (float)Math.Round(unknownLatitude2, 4);
                            }

                            if (float.TryParse(Long2, out unknownLongitude2))
                            {
                                // Round the parsed value
                                unknownLongitude2 = (float)Math.Round(unknownLongitude2, 4);
                            }

                            if ((unknownLongitude2 != unknownLongitude1) && (unknownLatitude2 != unknownLatitude1))
                            {
                                // General case where the slope is defined
                                slopOfLine = (unknownLatitude2 - unknownLatitude1) / (unknownLongitude2 - unknownLongitude1);
                                interceptOfYLine = unknownLatitude1 - (slopOfLine * unknownLongitude1);
                                equFormular = 1;
                            }

                            else if ((unknownLongitude2 != unknownLongitude1) && (unknownLatitude2 == unknownLatitude1))
                            {
                                // Horizontl line
                                slopOfLine = 0;
                                interceptOfYLine = unknownLatitude1;
                                equFormular = 2;
                            }

                            else if (unknownLongitude1 == unknownLongitude2)
                            {
                                // Vertical line
                                equFormular = 3;
                            }

                            List<float> longitudesOfLine = new List<float>();
                            List<float> latitudesOfLine = new List<float>();
                            elevation = new List<float>();
                            MEMDataArray = new List<float>();

                            if (ReadMapDataLine(unknownLatitude1, unknownLongitude1))
                            {
                                firstPointMeshCode = meshCOdeDetails[0];
                                firstPointLongitude200 = checkLongitude200;
                                firstPointLatitude200 = checkLatitude200;
                            }

                            if (ReadMapDataLine(unknownLatitude2, unknownLongitude2))
                            {
                                secondPointMeshCode = meshCOdeDetails[0];
                                secondPointLongitude200 = checkLongitude200;
                                secondPointLatitude200 = checkLatitude200;
                            }

                            int countNumberOfVerticalMeshCode = CountNumberOfVerticalMeshCode(firstPointMeshCode, secondPointMeshCode) - 1;
                            int countNumberOfHorizontalMeshCode = CountNumberOfHorizontalMeshCode(firstPointMeshCode, secondPointMeshCode) - 1;

                            if (countNumberOfVerticalMeshCode <= 0)
                            {
                                numberOfDataPointVertical = Math.Abs(firstPointLatitude200 - secondPointLatitude200);
                            }

                            else
                            {
                                numberOfDataPointVertical = 200 * countNumberOfVerticalMeshCode + 200 - firstPointLatitude200 + secondPointLatitude200;
                            }

                            if (countNumberOfHorizontalMeshCode <= 0)
                            {
                                numberOfDataPointHorizontal = Math.Abs(firstPointLongitude200 - secondPointLongitude200);
                            }

                            else
                            {
                                numberOfDataPointHorizontal = 200 * countNumberOfHorizontalMeshCode + 200 - firstPointLongitude200 + secondPointLongitude200;
                            }

                            incrementValue = Math.Abs(unknownLongitude1 - unknownLongitude2) / (float)Math.Sqrt((float)(Math.Pow(numberOfDataPointVertical, 2)) + (float)(Math.Pow(numberOfDataPointHorizontal, 2)));

                            if (incrementValue == 0)
                            {
                                incrementValue = Math.Abs(unknownLatitude1 - unknownLatitude2) / (float)Math.Sqrt((float)(Math.Pow(numberOfDataPointVertical, 2)) + (float)(Math.Pow(numberOfDataPointHorizontal, 2)));
                            }

                            distanceBetweenTwoPoints = (float)HaversineDistance(unknownLatitude1, unknownLongitude1, unknownLatitude2, unknownLongitude2);

                            if (equFormular == 1) // General case
                            {
                                if (slopOfLine > 0)
                                {
                                    if (unknownLongitude2 > unknownLongitude1)
                                    {
                                        for (int i = 0; (unknownLongitude1 + incrementValue * i) <= unknownLongitude2; i++)
                                        {
                                            longitudesOfLine.Add((float)Math.Round((unknownLongitude1 + incrementValue * i), 4));
                                            latitudesOfLine.Add((float)Math.Round(slopOfLine * (unknownLongitude1 + incrementValue * i) + interceptOfYLine, 4));
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; (unknownLongitude1 - incrementValue * i) >= unknownLongitude2; i++)
                                        {
                                            longitudesOfLine.Add((float)Math.Round((unknownLongitude1 - incrementValue * i), 4));
                                            latitudesOfLine.Add((float)Math.Round(slopOfLine * (unknownLongitude1 - incrementValue * i) + interceptOfYLine, 4));
                                        }
                                    }
                                }
                                else // unknownLatitude2 < unknownLatitude1
                                {
                                    if (unknownLongitude2 > unknownLongitude1)
                                    {
                                        for (int i = 0; (unknownLongitude1 + incrementValue * i) <= unknownLongitude2; i++)
                                        {
                                            longitudesOfLine.Add((float)Math.Round((unknownLongitude1 + incrementValue * i), 4));
                                            latitudesOfLine.Add((float)Math.Round(slopOfLine * (unknownLongitude1 + incrementValue * i) + interceptOfYLine, 4));
                                        }
                                    }
                                    else
                                    {
                                        for (int i = 0; (unknownLongitude1 - incrementValue * i) >= unknownLongitude2; i++)
                                        {
                                            longitudesOfLine.Add((float)Math.Round((unknownLongitude1 - incrementValue * i), 4));
                                            latitudesOfLine.Add((float)Math.Round(slopOfLine * (unknownLongitude1 - incrementValue * i) + interceptOfYLine, 4));
                                        }
                                    }
                                }
                            }

                            else if (equFormular == 2) // Horizontal line
                            {
                                if (unknownLongitude1 < unknownLongitude2)
                                {
                                    for (int i = 0; (unknownLongitude1 + incrementValue * i) <= unknownLongitude2; i++)
                                    {
                                        longitudesOfLine.Add((float)Math.Round((unknownLongitude1 + incrementValue * i), 4));
                                        latitudesOfLine.Add((float)Math.Round(unknownLatitude1, 4)); // For a horizontal line, latitudes are constant
                                    }
                                }
                                else // unknownLatitude2 < unknownLatitude1
                                {
                                    for (int i = 0; unknownLongitude2 <= (unknownLongitude1 - incrementValue * i); i++)
                                    {
                                        longitudesOfLine.Add((float)Math.Round((unknownLongitude1 - incrementValue * i), 4)); // For a horizontal line, latitudes are constant
                                        latitudesOfLine.Add((float)Math.Round(unknownLatitude1, 4));
                                    }
                                }
                            }

                            else if (equFormular == 3)
                            {
                                if (unknownLatitude1 < unknownLatitude2)
                                {
                                    for (int i = 0; (unknownLatitude1 + incrementValue * i) <= unknownLatitude2; i++)
                                    {
                                        latitudesOfLine.Add((float)Math.Round((unknownLatitude1 + incrementValue * i), 4));
                                        longitudesOfLine.Add((float)Math.Round(unknownLongitude1, 4)); // For a vertical line, longitude are constant
                                    }
                                }
                                else // unknownLatitude2 < unknownLatitude1
                                {
                                    for (int i = 0; unknownLatitude2 <= (unknownLatitude1 - incrementValue * i); i++)
                                    {
                                        latitudesOfLine.Add((float)Math.Round((unknownLatitude1 - incrementValue * i), 4));
                                        longitudesOfLine.Add((float)Math.Round(unknownLongitude1, 4)); // For a vertical line, longitude are constant
                                    }
                                }
                            }

                            int batchSize = 5000;
                            int batchCount = 0;
                            if (longitudesOfLine.Count > 0)
                            {
                                for (int i = 0; i < latitudesOfLine.Count; i++)
                                {
                                    if (ReadMapDataLine(latitudesOfLine[i], longitudesOfLine[i]))
                                    {
                                        pathToMEM = baseDirectory + "RESOURCES\\MAP DATA\\" + (floatMapData[0].ToString()).Substring(0, 4);
                                        MEMPath = pathToMEM + "\\" + (floatMapData[0].ToString()) + ".mem";
                                        memReader = new StreamReader(MEMPath);
                                        currentMemLine = memReader.ReadLine();
                                        currentMemLine = memReader.ReadLine();

                                        MEMDataArray.Add(floatMapData[0]);
                                        stringMEMData = new List<string>();
                                        while (currentMemLine != null)
                                        {
                                            stringMEMData.Add((currentMemLine.ToString()).Substring(9));
                                            currentMemLine = memReader.ReadLine();

                                            batchCount++; 
                                            //if (batchCount >= batchSize)
                                            //{
                                            //    await Task.Delay(10);  // Delay less frequently
                                            //    batchCount = 0;
                                            //}
                                        }
                                        elevation.Add((float.Parse((stringMEMData[199 - checkLatitude200]).Substring((checkLongitude200) * 5, 5))) / 10);
                                    }
                                }
                            }

                            ClearGraph();
                            // Use StreamWriter to append text to the file
                            using (StreamWriter writer = new StreamWriter(filePath, append: false))
                            {
                                writer.WriteLine(string.Empty);
                            }
                            // Method to dynamically add points
                            for (int i = 0; i < elevation.Count; i++)
                            {
                                if (elevation[i] == -999.9f)
                                {
                                    AddNewPoint(Math.Round(distanceBetweenTwoPoints * (i) / (elevation.Count), 2), 0);
                                }
                                else
                                {
                                    AddNewPoint(Math.Round(distanceBetweenTwoPoints * (i) / (elevation.Count), 2), elevation[i]);
                                }

                                // Use StreamWriter to append text to the file
                                using (StreamWriter writer = new StreamWriter(filePath, append: true))
                                {
                                    writer.WriteLine(i + 1 + "," + Math.Round(distanceBetweenTwoPoints * (i) / (elevation.Count), 2) + "," + elevation[i] + "," + longitudesOfLine[i] + "," + latitudesOfLine[i] + "," + MEMDataArray[i]);
                                }
                            }

                            _plotView.InvalidatePlot(true); // Refresh the plot view to show new data

                        }
                        isComplete = true;
                        plotGenerated = true;
                    }
                }
            }
            catch
            {
                isComplete = true;
                plotGenerated = false;
            }
        }

        private int CountNumberOfVerticalMeshCode(string firstPointMeshCode, string secondPointMeshCode)
        {
            // 1 - 493244
            // 2 - 513312
            int firstPointMeshCodeSubstring = int.Parse(firstPointMeshCode.Substring(0, 2) + firstPointMeshCode.Substring(4, 1)); // 494
            int secondPointMeshCodeSubstring = int.Parse(secondPointMeshCode.Substring(0, 2) + secondPointMeshCode.Substring(4, 1)); // 511
            int returnValue = 0;

            if (firstPointMeshCodeSubstring > secondPointMeshCodeSubstring)
            {
                int min = secondPointMeshCodeSubstring;
                int max = firstPointMeshCodeSubstring;
                firstPointMeshCodeSubstring = min;
                secondPointMeshCodeSubstring = max;
            }
            int tempFirstPointMeshCodeSubstring = firstPointMeshCodeSubstring;
            int tempSecondPointMeshCodeSubstring = secondPointMeshCodeSubstring;

            for (int i = 0; tempFirstPointMeshCodeSubstring < tempSecondPointMeshCodeSubstring; i++)
            {
                returnValue += 1;
                tempFirstPointMeshCodeSubstring += 1;
                if (tempFirstPointMeshCodeSubstring.ToString().EndsWith("8"))
                {
                    tempFirstPointMeshCodeSubstring += 2;
                }
            }

            return returnValue;
        }

        private int CountNumberOfHorizontalMeshCode(string firstPointMeshCode, string secondPointMeshCode)
        {
            // 1 - 493244
            // 2 - 513312
            int firstPointMeshCodeSubstring = int.Parse(firstPointMeshCode.Substring(2, 2) + firstPointMeshCode.Substring(5)); // 324
            int secondPointMeshCodeSubstring = int.Parse(secondPointMeshCode.Substring(2, 2) + secondPointMeshCode.Substring(5)); // 332
            int returnValue = 0;

            if (firstPointMeshCodeSubstring > secondPointMeshCodeSubstring)
            {
                int min = secondPointMeshCodeSubstring;
                int max = firstPointMeshCodeSubstring;
                firstPointMeshCodeSubstring = min;
                secondPointMeshCodeSubstring = max;
            }
            int tempFirstPointMeshCodeSubstring = firstPointMeshCodeSubstring;
            int tempSecondPointMeshCodeSubstring = secondPointMeshCodeSubstring;

            for (int i = 0; tempFirstPointMeshCodeSubstring < tempSecondPointMeshCodeSubstring; i++)
            {
                returnValue += 1;
                tempFirstPointMeshCodeSubstring += 1;
                if (tempFirstPointMeshCodeSubstring.ToString().EndsWith("8"))
                {
                    tempFirstPointMeshCodeSubstring += 2;
                }
            }

            return returnValue;
        }

        private bool ReadMapDataLine(float unknownLatitude, float unknownLongitude)
        {
            reader = new StreamReader(path);
            currentLine = reader.ReadLine();

            while (currentLine != null)
            {
                meshCOdeDetails = currentLine.Split(',');
                for (int i = 0; i < meshCOdeDetails.Length; i++)
                {
                    floatMapData[i] = (float.Parse(meshCOdeDetails[i]));
                }

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

        // Method to dynamically add points
        public void AddNewPoint(double x, double y)
        {
            _lineSeries.Points.Add(new DataPoint(x, y));

            // Optionally adjust the Y-axis limits dynamically
            double minY = _lineSeries.Points.Min(point => point.Y);
            double maxY = _lineSeries.Points.Max(point => point.Y);
            yAxis.Minimum = 0 ; // Adjust the minimum value of Y-axis
            yAxis.Maximum = maxY + 10; // Add some margin above the max value

            // Optionally adjust the X-axis limits dynamically
            double minX = _lineSeries.Points.Min(point => point.X);
            double maxX = _lineSeries.Points.Max(point => point.X);
            xAxis.Minimum = minX; // Adjust the minimum value of Y-axis
            xAxis.Maximum = maxX; // Add some margin above the max value
        }

        public void ClearGraph()
        {
            _lineSeries.Points.Clear(); // Clear all points from the series
            _plotView.InvalidatePlot(true); // Refresh the plot view to reflect the changes
        }

        public double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Radius of the Earth in kilometers
            double lat1Rad = ToRadians(lat1);
            double lat2Rad = ToRadians(lat2);
            double deltaLat = ToRadians(lat2 - lat1);
            double deltaLon = ToRadians(lon2 - lon1);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // Distance in kilometers
        }

        private double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

    }
}
