using System;
using System.Collections.Generic;
using System.Text;
//using DAL.DalObject;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using DO;
namespace DalApi
{
    public interface IDal
    {
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add </param>
        void AddCustomer(Customer customer);


        /// <summary>
        ///  A function that recieve a station and add it to the lists of the stations
        /// </summary>
        /// <param name="station">the station we need to add</param>
        void AddStation(Station station);

        /// <summary>
        ///  A function that recieve a drone and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        void AddDrone(Drone drone);

        /// <summary>
        ///  A function that recieve a parcel and add it to the lists of the parcels
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        void AddParcel(Parcel newParcel);

        /// <summary>
        /// A function that recieve a droneCharge id and return from the list of the droneCharges the droneCharge with this id
        /// </summary>
        /// <param name="id">the id of the droneCharge</param>
        /// <returns>return the droneCharge with this id </returns>
        DroneCharge GetDroneCharge(int id);

        /// <summary>
        /// A function that recieve a drones id and return from the list of the drones the drone with this id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns>return the drone with this id</returns>
        Drone GetDrone(int id);

        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcels the parcel with this id
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        /// <returns>return the drone with this id</returns>
        Parcel GetParcel(int id);

        /// <summary>
        ///  A function that recieve a stations id and return from the list of the stations the station with this id
        /// </summary>
        /// <param name="id">the id of the station</param>
        /// <returns>return the station with this id</returns>
        Station GetStation(int id);

        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="id">the id of the customer</param>
        /// <returns>return the customer with this id</returns>
        Customer GetCustomer(int id);

        /// <summary>
        /// A function that return from the list of the drones
        /// </summary>
        /// <returns>return the list of drones </returns>
        IEnumerable<Drone> GetDrones(Func<Drone, bool> predicate = null);

        /// <summary>
        /// A function that return from the list of the customers
        /// </summary>
        /// <returns>return the list of customers</returns>
        IEnumerable<Customer> Getcustomers(Func<Customer, bool> predicate = null);

        /// <summary>
        /// A function that return from the list of the parcels
        /// </summary>
        /// <returns>return the list of parcels</returns>
        IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null);

        /// <summary>
        /// A function that return from the list of the statoins
        /// </summary>
        /// <returns>return the list of stations</returns>
        IEnumerable<Station> GetStations(Func<Station, bool> predicate = null);

        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id in the parcels list
        /// </summary>
        /// <param name="parcel">the parcel with new data to update</param>
        void UpdateParcel(Parcel parcel);
        void DeleteParcel(Parcel p);


        /// <summary>
        /// A function that recieve a station and a drone and create a new dronecharge with both id`s and update the station accordingly
        /// </summary>
        /// <param name="station">the station that the drone charge there</param>
        /// <param name="drone">the drone thet need to charge</param>
        void AnchorDroneStation(Station station, Drone drone);


        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="dr">the drone with new data to update</param>
        void UpdateDrones(Drone dr);

        /// <summary>
        /// A function that recieve a parcel and a drone and and update the parcel to be connected to the drone, update the list of the parcels and drones accordingly
        /// </summary>
        /// <param name="parcel">the parcel thet need to updat</param>
        /// <param name="drone">the drone thet need to updat</param>
        void BelongingParcel(Parcel parcel, Drone drone);



        /// <summary>
        /// A function that recieve a station and a drone and releas the ststion that charged him, and update the lists of the drones and station accordingly
        /// </summary>
        /// <param name="drone">the drone its need to release</param>
        /// <param name="st">the station that charged this drone</param>
        void ReleasDrone(Drone drone, Station st);

        /// <summary>
        /// function that recieve a parcel and a customer,update the parcel supply to the customer and update  the lists of the customers and parcels accordingly
        /// </summary>
        /// <param name="parcel">the parcel its need to supply</param>
        /// <param name="customer">the customer that recieve the parcel</param>
        void SupplyParcel(Parcel parcel, Customer customer);

        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="latOrLot"></param>
        /// <returns></returns>
        string DecimalToSexagesimal(double coord, char latOrLot);// funciton receives char to decide wheter it is t=latitude and n=lonitude.



        /// <summary>
        /// computes half a versine of the angle
        /// </summary>
        /// <param name="radian">the angle`s computes </param>
        /// <returns>return half a versine of the angle</returns>
        double Hav(double radian);


        /// <summary>
        /// computes an angle in radians
        /// </summary>
        /// <param name="degree">a number to transfore to radian</param>
        /// <returns>returns an angle in radians</returns>
        double Radians(double degree);



        /// <summary>
        ///  receiving 2 points the haversine formula returns the distance (in km) between the 2
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        double Haversine(double lon1, double lat1, double lon2, double lat2);
        int AvailableChargingPorts(int baseStationId);
        IEnumerable<DroneCharge> GetDronesInCharge(Func<DroneCharge, bool> predicate = null);
        // IEnumerable<Station> AvailableChargingStations();
        int ParcelsCustomerGot(int customerId);
        int ParcelsCustomerSendAndDelivered(int customerId);

        void UpdateStations(Station st);
        void UpdateParcels(Parcel parcel);
        void UpdateCustomers(Customer customer);
        double Distance(int ID, double lonP, double latP);

        double[] PowerRequest();
        int ParcelsCustomerSendAndNotDelivered(int iD);
        int ParcelsInTheWayToCustomer(int iD);
        int GetStatusOfParcel(int iD);
        bool LogInVerify(User userDO);
        bool isWorker(User userDO);
    }
}


