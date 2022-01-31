using System;
using BO;
using System.Threading;
using static BL.BLObject;
using System.Linq;
using static System.Math;
using System.Collections.Generic;

namespace BL
{
    internal class Simulator
    {
        BLObject bl;

        public Simulator(BLObject _bl, int droneID, Action ReportProgressInSimultor, Func<bool> IsTimeRun)
        {
            bl = _bl;
            double distance;
            int battery;
            DroneForList drone = bl.GetDrones().First(x => x.Id == droneID);
            while (!IsTimeRun())
            {
                switch (drone.Status)
                {
                    case DroneStatuses.Free:
                        try
                        {
                            bl.AssignmentParcelToDrone(droneID);
                            ReportProgressInSimultor();
                        }
                        catch
                        {
                            if (drone.Battery < 100)
                            {
                                battery = (int)drone.Battery;
                                BO.Station baseStation = bl.findClosetBaseStation(drone);
                                distance = bl.calcDistance(drone.DroneLocation, baseStation.Location);
                                while (distance > 0)
                                {
                                    drone.Battery -= (int)bl.Power()[0];//the drone is available
                                    ReportProgressInSimultor();
                                    distance -= 1;
                                    Thread.Sleep(1000);
                                }
                                drone.Battery = battery;//restarting the battery
                                bl.SendDroneToRecharge(droneID);//here it will change it to the correct battery
                                ReportProgressInSimultor();
                            }
                        }
                        break;
                    case DroneStatuses.Maintenance:
                        while (drone.Battery < 100)//meens the battery isnt full
                        {
                            if (drone.Battery + (int)bl.Power()[4] > 100)//check if there aill be to much battery
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery = 100;
                            }
                            else
                            {
                                bl.GetDrones().First(x => x.Id == droneID).Battery += (int)bl.Power()[4];
                            }
                            ReportProgressInSimultor();
                            Thread.Sleep(1000);
                        }
                        bl.FreeDroneFromeCharger(droneID);
                        ReportProgressInSimultor();
                        break;
                    case DroneStatuses.Shipping:
                        Drone _drone = bl.GetDrone(droneID);
                        if (bl.GetParcel(_drone.Package.Id).CollectionTime == null)//meens the drone needs to pick up the parcel
                        {
                            battery = (int)drone.Battery;
                            Location location = new Location { Longitude = drone.DroneLocation.Longitude, Lattitude = drone.DroneLocation.Lattitude };
                            //the distance between the drone and the sender of the parcel
                            distance = bl.calcDistance
                                (_drone.Location, _drone.Package.Collection);
                            double latitude = Abs((bl.GetCustomer(_drone.Package.Sender.Id).Location.Lattitude - drone.DroneLocation.Lattitude) / distance);
                            double longitude = Abs((bl.GetCustomer(_drone.Package.Sender.Id).Location.Longitude - drone.DroneLocation.Longitude) / distance);
                            //meens the drone didnt get to sender yet
                            while (distance > 1)
                            {
                                drone.Battery -= (int)bl.Power()[0];//the drone is available
                                distance -= 1;
                                UpdateLocationDrone(_drone.Location, bl.GetCustomer(_drone.Package.Sender.Id).Location, _drone, longitude, latitude);
                                drone.DroneLocation = _drone.Location;
                                bl.GetDrones().First(item => item.Id == drone.Id).DroneLocation = drone.DroneLocation;
                                ReportProgressInSimultor();
                                Thread.Sleep(1000);
                            }
                            //meens the drone arraived to the sender
                            drone.DroneLocation = location;
                            drone.Battery = battery;
                            bl.PickedupParcel(_drone.Id);
                            ReportProgressInSimultor();
                        }
                        else //PickedUp != null
                        {
                            battery =(int) drone.Battery;
                            distance = _drone.Package.TransportDistance;//the distance betwwen the sender and the resever
                            Location d = new Location { Longitude = drone.DroneLocation.Longitude, Lattitude = drone.DroneLocation.Lattitude };
                            double latitude = Abs((bl.GetCustomer(_drone.Package.Target.Id).Location.Lattitude - drone.DroneLocation.Lattitude) / distance);
                            double longitude = Abs((bl.GetCustomer(_drone.Package.Target.Id).Location.Longitude - drone.DroneLocation.Longitude) / distance);
                            //meens the drone didnt get to recever yet
                            while (distance > 1)
                            {
                                switch (_drone.Package.Longitude)//according the parcels weight
                                {
                                    case WeightCategories.Light:
                                        drone.Battery -= (int)bl.Power()[1];//light
                                        break;
                                    case WeightCategories. Medium:
                                        drone.Battery -= (int)bl.Power()[2];//medium
                                        break;
                                    case WeightCategories.Heavy:
                                        drone.Battery -= (int)bl.Power()[3];//heavy
                                        break;
                                    default:
                                        break;
                                }
                                UpdateLocationDrone(_drone.Location, bl.GetCustomer(_drone.Package.Target.Id).Location, _drone, longitude, latitude);
                                drone.DroneLocation = _drone.Location;
                                ReportProgressInSimultor();
                                distance -= 1;
                                Thread.Sleep(500);
                            }
                            drone.DroneLocation = d;
                            drone.Battery = battery;
                            bl.PackageDeliveryByDrone(_drone.Id);
                            ReportProgressInSimultor();
                        }
                        break;
                    default:
                        break;
                }
                Thread.Sleep(1000);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locationOfDrone">the drones location</param>
        /// <param name="locationOfNextStep">the target location</param>
        /// <param name="myDrone">the drone that is in delivery</param>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        private void UpdateLocationDrone(Location locationOfDrone, Location locationOfNextStep, Drone myDrone, double lon, double lat)
        {
            double droneLatitude = locationOfDrone.Lattitude;
            double droneLongitude = locationOfDrone.Longitude;

            double targetLatitude = locationOfNextStep.Lattitude;
            double targetLongitude = locationOfNextStep.Longitude;

            //Calculate the latitude of the new location.
            if (droneLatitude < targetLatitude)
                myDrone.Location.Lattitude += lat;
            else
                myDrone.Location.Lattitude -= lat;
            //Calculate the Longitude of the new location.
            if (droneLongitude < targetLongitude)
                myDrone.Location.Longitude += lon;
            else
                myDrone.Location.Longitude -= lon;
        }
    }
}
