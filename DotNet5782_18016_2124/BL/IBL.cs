using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IBL.BO;

namespace IBL
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

    }
}


