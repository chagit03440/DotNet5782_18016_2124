using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;

namespace BlApi
{
    public interface IBL
    {

        /// <summary>
        /// A function that assigns a package to a skimmer
        /// </summary>
        /// <param name="parcelId">the parcel we want to assign</param>
        /// <param name="droneId">the drone we want to assign</param>
        void AssignmentParcelToDrone(int droneId);
        /// <summary>
        ///   The function collects the package by the drone if the package can be collected and updates the required data,If no appropriate exception is sent
        /// </summary>
        /// <param name="droneId">the number of the drone</param>
        void PickedupParcel(int parcelId);
        /// <summary>
        /// The function checks whether the drone can be sent for charging if it can sends it to a charging station near it with charging stations and updates the data accordingly if it does not send appropriate exceptions
        /// </summary>
        /// <param name="droneId">the number of drone that need to send to recharge</param>
        void SendDroneToRecharge(int droneId);
        /// <summary>
        /// A function that gets a user and return true if the user exists or false if not
        /// </summary>
        /// <param name="curUser">the user it checks</param>
        /// <returns>true if the user exists or false if not</returns>
        bool LogInVerify(User curUser);
        /// <summary>
        /// A function that gets a user and return true if the user is a worker or false if not
        /// </summary>
        /// <param name="curUser">the user it checks</param>
        /// <returns>true if the user is a worker or false if not</returns>
        bool isWorker(User curUser);

        /// <summary>
        ///   The function provides a package by a drone if the package can be delivered updates the required data, if no appropriate exception is sent
        /// </summary>
        /// <param name="droneId">the number of the drone</param>
        void PackageDeliveryByDrone(int droneId);
        /// <summary>
        ///  A function that recieve a drone and time and checks whether the drone can be released from charging if you can, update the data layer with the required updates ,if no, exception is thrown
        /// </summary>
        /// <param name="droneId">the number drone its need to release</param>
        /// <param name="time">Time period on charge </param>
        void ReleaseDroneFromRecharge(int droneId, double time);
        /// <summary>
        /// A function that recieve a stations id and return from the list of the station the station with this id
        /// </summary>
        /// <param name="requestedId">station id</param>
        /// <returns>return the station with this id</returns>
        Station GetStation(int requestedId);
        /// <summary>
        /// A function that recieve a drones id and return from the list of the drone the drone with this id
        /// </summary>
        /// <param name="requestedId">drone id</param>
        /// <returns>return the drone with this id</returns>
        Drone GetDrone(int requestedId);
        /// <summary>
        /// A function that recieve a drones id and return from the list of the drone the drone with this id
        /// </summary>
        /// <param name="requestedId">drone id</param>
        /// <returns>return the drone with this id</returns>
        DroneForList GetDroneForList(int requestedId);
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="requestedId">customer id</param>
        /// <returns>return customer with this id</returns>
        Customer GetCustomer(int requestedId);
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcel the parcel with this id
        /// </summary>
        /// <param name="requestedId">id parcel</param>
        /// <returns>return the parcel with this id</returns>
        Parcel GetParcel(int requestedId);
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcel the parcel with this id
        /// </summary>
        /// <param name="requestedId">id parcel</param>
        /// <returns>return the parcel with this id</returns>
        ParcelForList GetParcelForList(int requestedId);
        /// <summary>
        /// A function that return  the list of the stations
        /// </summary>
        /// <returns>return list of station</returns>
        IEnumerable<BO.StationForList> GetStations(Func<StationForList, bool> predicate = null);
        /// <summary>
        /// A function that return  the list of the drones
        /// </summary>
        /// <returns>return list of drone</returns>
        IEnumerable<DroneForList> GetDrones(Func<DroneForList, bool> predicate = null);
        /// <summary>
        /// A function that return  the list of the customers
        /// </summary>
        /// <returns>return list of the customer</returns>
        IEnumerable<CustomerForList> GetCustomers(Func<CustomerForList, bool> predicate = null);
        /// <summary>
        /// A function that return  the list of the parcels
        /// </summary>
        /// <returns>return list of the parcel</returns>
        IEnumerable<ParcelForList> GetParcels(Func<ParcelForList, bool> predicate = null);
        /// <summary>
        /// A function that return  the list of the unAssignmentParcels
        /// </summary>
        /// <returns>return list of the unAssignmentParcel</returns>
        //IEnumerable<ParcelForList> UnAssignmentParcels();
        ///// <summary>
        ///// A function that return  the list of the availableChargingStations
        ///// </summary>
        ///// <returns>return list of the availableChargingStation</returns>
        //IEnumerable<Station> AvailableChargingStations();

        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="d">the drone we need to update</param>
        void updateDroneForList(DroneForList d);
        /// <summary>
        ///  A function that recieve a customer and update the customer whith the same id in the customer list
        /// </summary>
        /// <param name="cus">customer that need update</param>
        /// <summary>
        void UpdateCustomer(Customer cus);
        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list

        /// </summary>
        /// <param name="drone">the drone we need to update</param>

        void UpdateDrone(Drone drone);
        /// <summary>
        ///  A function that recieve a station an number of charging stations and and update the station list
        /// </summary>
        /// <param name="st">station we need to updat</param>
        /// <param name="cs">num of charging stations</param>
        void UpdateStation(Station st, int cs);
        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id
        /// </summary>
        /// <param name="parcel">parcel to update</param>
        void UpdateParcel(Parcel parcel);
        /// <summary>
        /// A function that recieve a parcel and add the parcel to the list
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        void AddParcel(Parcel newParcel);
        /// <summary>
        /// add Base station to dal
        /// </summary>
        /// <param name="AddbaseStation">receive from BL</param>
        void AddStation(Station baseStation);
        /// <summary>
        ///  A function that recieve a drone and number of station and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        /// <param name="stationId">station  id</param>
        void AddDrone(DroneForList drone, int stationId);
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add</param>
        void AddCustomer(Customer customer);
        /// <summary>
        /// A function that recieve a parcel and delete it from the lists of the parcels
        /// </summary>
        /// <param name="p">the parcel to delete</param>
        void DeleteParcel(Parcel p);
        /// <summary>
        /// A function that recieve a station and delete it from the lists of the stations
        /// </summary>
        /// <param name="s">the station to delete</param>
        void DeleteStation(Station s);

        /// <summary>
        /// A function that receives a location and returns the nearest base station
        /// </summary>
        /// <param name="fromLocatable">the start location </param>
        /// <returns>the closest base station location</returns>
        Location findClosetBaseStationLocation(Location fromLocatable);

        /// <summary>
        ///  Calculates the required power consumption of drone 
        /// </summary>
        /// <param name="distance"> The distance of the drone.</param>
        /// <param name="index"> DroneLocation of the drone in the list</param>
        /// <returns>return the power consumption of the drone   </returns>
        double BatteryUsages(double distance, int index);
        /// <summary>
        /// A function that receives a start and a destination and returns the distance between the two locations
        /// </summary>
        /// <param name="from">the start location</param>
        /// <param name="to">the destination location</param>
        /// <returns>the distance between the locations</returns>
        double calcDistance(Location from, Location to);
        /// <summary>
        /// a function that recieves a drone and return the closest base station to the drone
        /// </summary>
        /// <param name="drone">the drone that need to recharge</param>
        /// <returns>the closest base station to the drone</returns>
        Station findClosetBaseStation(DroneForList drone);
        /// <summary>
        /// a function that start  the simulator of the drone
        /// </summary>
        /// <param name="id">the drone that need to be in simulator</param>
        /// <param name="update">a function that update the windows of the drones</param>
        /// <param name="checkStop">a function that tells to the simulator if it needs to stop or not</param>
        void StartDroneSimulator(int id, Action update, Func<bool> checkStop);
        /// <summary>
        /// a function that returns an array with the power requested in each status of the drone
        /// </summary>
        /// <returns>an array with the power requested in each status of the drone</returns>
        public double[] Power();
    }
}


