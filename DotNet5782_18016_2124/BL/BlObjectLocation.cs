using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DalObject;
using IBL;
namespace BL
{
    public partial class BLObject : BlApi
    {
        /// <summary>
        /// A function that receives a start and a destination and returns the distance between the two locations
        /// </summary>
        /// <param name="from">the start location</param>
        /// <param name="to">the destination location</param>
        /// <returns>the distance between the locations</returns>
        private double calcDistance(Location from, Location to)
        {
            int R = 6371 * 1000; // metres
            double phi1 = from.Lattitude * Math.PI / 180; // φ, λ in radians
            double phi2 = to.Lattitude * Math.PI / 180;
            double deltaPhi = (to.Lattitude - from.Lattitude) * Math.PI / 180;
            double deltaLambda = (to.Longitude - from.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;
        }
        /// <summary>
        /// A function that receives a location and returns the nearest base station
        /// </summary>
        /// <param name="fromLocatable">the start location </param>
        /// <returns>the closest base station location</returns>
        private Location findClosetBaseStationLocation(Location fromLocatable)
        {
            List<Station> locations = new List<Station>();
            foreach (var baseStation in myDal.GetStations())
            {
                locations.Add(new Station
                {
                    Location = new Location
                    {
                        Lattitude = baseStation.Lattitude,
                        Longitude = baseStation.Longitude
                    }
                });
            }
            Location location = locations[0].Location;
            double distance = calcDistance(fromLocatable, locations[0].Location);
            for (int i = 1; i < locations.Count; i++)
            {
                if (calcDistance(fromLocatable, locations[i].Location) < distance)
                {
                    location = locations[i].Location;
                    distance = calcDistance(fromLocatable, locations[i].Location);
                }
            }
            return location;
        }
        /// <summary>
        /// A function that receives a skimmer and returns its position
        /// </summary>
        /// <param name="droneBO">the drone we want his location</param>
        /// <returns>the location of the drone</returns>
        private Location findDroneLocation(Drone droneBO)
        {
            return droneBO.Location;
        }
        /// <summary>
        /// A function that accepts two locations and calculates the euclidean Distance between them
        /// </summary>
        /// <param name="from">the start </param>
        /// <param name="to">the destination</param>
        /// <returns>the euclidean Distance between them</returns>
        private double euclideanDistance(Location from, Location to)
        {
            return Math.Sqrt(Math.Pow(to.Longitude - from.Longitude, 2) + Math.Pow(to.Lattitude - from.Lattitude, 2));
        }
        /// <summary>
        /// A function that receives a location and returns a list of base stations in order from the closest to the farthest
        /// </summary>
        /// <param name="fromLocatable">the location of the start</param>
        /// <returns> a list of base stations in order from the closest to the farthest</returns>
        private List<Station> ClosetBaseStationsLocation(Location fromLocatable)
        {
            List<Station> locations = new List<Station>();
            foreach (var baseStation in myDal.GetStations())
            {
                locations.Add(new Station
                {
                    Location = new Location
                    {
                        Lattitude = baseStation.Lattitude,
                        Longitude = baseStation.Longitude
                    }
                });
            }
            Location location = locations[0].Location;
            double distance = calcDistance(fromLocatable, locations[0].Location);
            int i, j;
            for (i = 0; i < locations.Count - 1; i++)

                // Last i elements are already in place
                for (j = 0; j < locations.Count - i - 1; j++)
                    if (calcDistance(fromLocatable, locations[j].Location) > calcDistance(fromLocatable, locations[j + 1].Location))
                    {
                        Station temp = locations[j];
                        locations[j] = locations[j + 1];
                        locations[j + 1] = locations[j];
                    }

            return locations;
        }
    }
}