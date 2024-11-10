using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UNDAI.MODELS.BASE
{
    public class GPSMapIntoCircle
    {
        // Earth's radius in meters
        private const float EarthRadius = 6371000;
        int circleRadius = 3000;

        string firstLatitude = "";
        string firstLongitude = "";
        string secondLatitude = "";
        string secondLongitude = "";

        float distance;
        public float angle;
        public float XCoordinate, YCoordinate;

        public GPSMapIntoCircle(string lat1, string long1, string lat2, string long2, int circleRad)
        {
            firstLatitude = lat1;
            firstLongitude = long1;
            secondLatitude = lat2;
            secondLongitude = long2;
            circleRadius = circleRad;

            distance = CalculateDistance(firstLatitude, firstLongitude, secondLatitude, secondLongitude);
            angle = CalculateBearing(firstLatitude, firstLongitude, secondLatitude, secondLongitude);
            (XCoordinate, YCoordinate) = MapPointOnCircle(firstLatitude, firstLongitude, distance, angle);
        }

        public float CalculateDistance(string lat1, string long1, string lat2, string long2)
        {
            // Convert latitudes and longitudes from degrees to radians
            float lat1Rad = ToRadians(float.Parse(lat1));
            float lat2Rad = ToRadians(float.Parse(lat2));
            float deltaLat = ToRadians(float.Parse(lat2) - float.Parse(lat1));
            float deltaLon = ToRadians(float.Parse(long2) - float.Parse(long1));

            // Haversine formula
            float a = (float)(Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2));
            float c = (float)(2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a)));
            return EarthRadius * c; // distance in meters
        }

        public float CalculateBearing(string lat1, string long1, string lat2, string long2)
        {
            float lat1Rad = ToRadians(float.Parse(lat1));
            float lat2Rad = ToRadians(float.Parse(lat2));
            float deltaLon = ToRadians(float.Parse(long2) - float.Parse(long1));

            float y = (float)(Math.Sin(deltaLon) * Math.Cos(lat2Rad));
            float x = (float)(Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                       Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLon));

            return (float)((ToDegrees((float)Math.Atan2(y, x)) + 360) % 360); // Bearing in degrees
        }

        public (float X, float Y) MapPointOnCircle(string lat1, string long1, float distance, float angle)
        {
            // Normalize distance to fit within the circle's radius
            float mappedDistance = Math.Min(distance, circleRadius);

            // Convert polar coordinates to Cartesian coordinates
            float x = (float)(mappedDistance * Math.Cos(ToRadians(90-angle)));
            float y = (float)(mappedDistance * Math.Sin(ToRadians(90-angle)));

            return (x, y);
        }

        private float ToRadians(float degrees)
        {
            return (float)(degrees * (Math.PI / 180));
        }

        private float ToDegrees(float radians) 
        {
            return (float)(radians * (180 / Math.PI));
        }
    }
}
