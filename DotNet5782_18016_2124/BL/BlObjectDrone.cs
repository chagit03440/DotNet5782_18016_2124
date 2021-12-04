﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DalObject;
namespace BL
{
    public partial class BLObject : IBL.IBL
    {
        /// <summary>
        /// A function that receives a skimmer and returns the minimum battery the skimmer needs to reach the destination
        /// </summary>
        /// <param name="drone">the drone to calculate</param>
        /// <returns>the minimum battery required</returns>
        private int calcMinBatteryRequired(DroneForList drone)
        {
            if (drone.Status == DroneStatuses.Free)
            {
                Location location = findClosetBaseStationLocation(drone.Location);
                return (int)(myDal.PowerRequest()[(int)BatteryUsage.Available] * calcDistance(drone.Location, location));
            }

            if (drone.Status == DroneStatuses.Shipping)
            {
                IDAL.DO.Parcel parcel = myDal.GetParcel(drone.ParcelId);
                if (parcel.PickedUp is null)
                {
                    int minValue;
                    Customer sender = GetCustomer(parcel.SenderId);
                    Customer target = GetCustomer(parcel.TargetId);
                    double droneToSender = calcDistance(drone.Location, sender.Location);
                    minValue = (int)(myDal.PowerRequest()[(int)BatteryUsage.Available] * droneToSender);
                    double senderToTarget = calcDistance(sender.Location, target.Location);
                    BatteryUsage batteryUsage =
                        (BatteryUsage)Enum.Parse(typeof(BatteryUsage), parcel.Longitude.ToString());
                    minValue += (int)(myDal.PowerRequest()[(int)batteryUsage] * senderToTarget);
                    Location baseStationLocation = findClosetBaseStationLocation(target.Location);
                    double targetToCharge = calcDistance(target.Location, baseStationLocation);
                    minValue += (int)(myDal.PowerRequest()[(int)DroneStatuses.Free] * targetToCharge);
                    return minValue;
                }

                if (parcel.Delivered is null)
                {
                    int minValue;
                    Customer sender = GetCustomer(parcel.SenderId);
                    Customer target = GetCustomer(parcel.TargetId);
                    double senderToTarget = calcDistance(sender.Location, target.Location);
                    BatteryUsage batteryUsage = (BatteryUsage)Enum.Parse(typeof(BatteryUsage), parcel.Longitude.ToString());
                    minValue = (int)(myDal.PowerRequest()[(int)batteryUsage] * senderToTarget);
                    Location baseStationLocation = findClosetBaseStationLocation(target.Location);
                    double targetToCharge = calcDistance(target.Location, baseStationLocation);
                    minValue += (int)(myDal.PowerRequest()[(int)BatteryUsage.Available] * targetToCharge);
                    return minValue;
                }
            }
            return 90;
        }
        /// <summary>
        /// note: There is a side effect, if True drone.DeliveryId getting value
        /// </summary>
        /// <param name="drone"></param>
        /// <returns>True if drone while shipping </returns>
        private bool isDroneWhileShipping(DroneForList drone)
        {
            var parcels = myDal.GetParcels();
            for (int i = 0; i < parcels.Count(); i++)
            {
                if (parcels.ElementAt(i).DroneId == drone.Id)
                {
                    if (parcels.ElementAt(i).Delivered == null)
                    {
                        drone.ParcelId = parcels.ElementAt(i).ID;
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        ///   The function that receives a drone and finds its location
        /// </summary>
        /// <param name="drone">the drone we need</param>
        /// <returns>return location of this drone</returns>
        private Location findDroneLocation(DroneForList drone)
        {
            if (drone.Status == DroneStatuses.Shipping)
            {
                IDAL.DO.Parcel parcel = myDal.GetParcel(drone.ParcelId);
                if (parcel.PickedUp == null)
                {
                    Customer customer = GetCustomer(parcel.SenderId);
                    return findClosetBaseStationLocation(customer.Location);
                }
                if (parcel.Delivered == null)
                {
                    return GetCustomer(parcel.SenderId).Location;
                }
            }
            if (drone.Status == DroneStatuses.Free)
            {
                var targetsIds = myDal.GetParcels()
                    .Where(parcel => parcel.DroneId == drone.Id)
                    .Select(parcel => parcel.TargetId).ToList();

                if (targetsIds.Count == 0)
                {

                    IDAL.DO.Station station = new IDAL.DO.Station();
                    station.ID = rand.Next(1, myDal.GetStations().Count());
                    IDAL.DO.Drone tdrone = myDal.GetDrone(drone.Id);
                    myDal.AnchorDroneStation(station, tdrone);
                    return getBaseStationLocation(station.ID);
                }
                //TODO: get last customer location
                return GetCustomer(targetsIds[rand.Next(targetsIds.Count)]).Location;
            }
            return new Location();
        }
        /// <summary>
        /// A function that recieve a drones id and return him buttery
        /// </summary>
        /// <param name="droneId">dron id</param>
        /// <returns>return the buttery drone</returns>
        private double getDroneBattery(int droneId)
        {
            return drones.Find(drone => drone.Id == droneId).Battery;
        }
        /// <summary>
        /// A function that recieve a drones id and return from the list of the drone the drone with this id
        /// </summary>
        /// <param name="requestedId">drone id</param>
        /// <returns>return the drone with this id</returns>
        public DroneForList GetDrone(int requestedId)
        {

            DroneForList df = drones.FirstOrDefault(x => x.Id == requestedId);
            if (df.Id != 0)
                return df;
            DroneForList droneBO = new DroneForList();
            IDAL.DO.Drone droneDO = myDal.GetDrone(requestedId);
            DroneForList drone = drones.Find(d => d.Id == requestedId);
            droneBO.Id = droneDO.ID;
            droneBO.Model = droneDO.Model;
            droneBO.MaxWeight = (WeightCategories)droneDO.MaxWeight;
            droneBO.Location = drone.Location;
            droneBO.Battery = drone.Battery;
            droneBO.Status = drone.Status;
            droneBO.ParcelId = drone.ParcelId;
            return droneBO;
        }
        /// <summary>
        ///  A function that recieve a drone and number of station and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        /// <param name="stationId">station  id</param>
        public void AddDrone(DroneForList drone, int stationId)
        {
            drone.Battery = (double)rand.Next(20, 40);

            //IDAL.DO.Drone tdrone = myDal.GetDrones(drone.Id);
            IDAL.DO.Drone tdrone = new IDAL.DO.Drone() { ID = drone.Id, MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight, Model = drone.Model };
            IDAL.DO.Station tstation = myDal.GetStation(stationId);
            drone.Status = DroneStatuses.Maintenance;
            myDal.AnchorDroneStation(tstation, tdrone);
            drone.Location = getBaseStationLocation(stationId);
            drone.ParcelId = 0;
            drones.Add(drone);

            IDAL.DO.Drone droneDO = new IDAL.DO.Drone();
            droneDO.ID = drone.Id;
            try
            {
                myDal.AddDrone(droneDO);
            }
            catch (Exception exp)
            {

                throw new BLDroneException("", exp);
            }
        }
        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="d">the drone we need to update</param>
        public void updateDroneForList(DroneForList d)
        {
            DroneForList drone = new DroneForList
            {
                Id = d.Id,
                Model = d.Model,
                MaxWeight = d.MaxWeight,
                Battery = d.Battery,
                Location = d.Location,
                Status = d.Status,
                ParcelId = d.ParcelId


            };

            int index = drones.FindIndex(x => x.Id == d.Id);

            drones.RemoveAt(index);
            drones.Insert(index, drone);
        }
        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list

        /// </summary>
        /// <param name="drone">the drone we need to update</param>
        public void UpdateDrone(Drone drone)
        {
            try
            {
                myDal.GetDrone(drone.Id);
            }
            catch (Exception ex)
            {

                throw new BLDroneException("The drone not exsit", ex);
            }
            DroneForList dr = drones.Find(x => x.Id == drone.Id);

            int index = drones.FindIndex(x => x.Id == dr.Id);
            dr.Model = drone.Model;
            drones.RemoveAt(index);
            drones.Insert(index, dr);
        }
    }
}