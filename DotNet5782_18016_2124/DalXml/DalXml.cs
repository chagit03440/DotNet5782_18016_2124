using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.IO;
using DalApi;
using DO;
using System.Security.Cryptography;

namespace Dal
{
    class DalXml : IDal
    {
        #region singelton

        static string parcelPath = "ParcelsXml.xml";//XMLSerializer
        static string stationPath = "StationsXml.xml";  //XElement
        static string customerPath = "CustomersXml.xml";//XMLSerializer
        static string dronePath = "DronesXml.xml";//XMLSerializer
        static string usersPath = "UsersXml.xml";//XMLSerializer
        static string droneChargePath = "DroneChargeXml.xml";
        static string configPath = "ConfigXml.xml";


        internal static double available = 0;
        internal static double lightWeight = 10;
        internal static double mediumWeight = 50;
        internal static double heavyWeight = 150;
        internal static double chargingRate = 10.26;
        static readonly DalXml instance = new DalXml();
        static DalXml() { }

        public static DalXml Instance { get => instance; }
        // DalXml() {  }
        private DalXml() //private  
        {

            List<DroneCharge> droneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            //foreach (var item in droneCharge)
            //{
            //    UpdatePluseChargeSlots(item.StationId);
            //}
            droneCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneCharge, droneChargePath);
        }
        #endregion


        #region station
        /// <summary>
        ///  A function that recieve a station and add it to the lists of the stations
        /// </summary>
        /// <param name="station">the station we need to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElem = (from stations in stationsRoot.Elements()
                               where (stations.Element("ID").Value == station.ToString())
                               select stations).FirstOrDefault();
            if (stationElem != null)
                throw new InVaildIdException("The two following stations already exist in the system");
            XElement newStation = new XElement("Station"
                , new XElement("ID", station.ID),
                new XElement("Name", station.Name),
                new XElement("Longitude", station.Longitude.ToString()),
                new XElement("Lattitude", station.Lattitude.ToString()),
                new XElement("ChargeSlots", station.ChargeSlots.ToString()));
            stationsRoot.Add(newStation);

        }
        /// <summary>
        /// A function that recieve a station and delete the station whith the same id in the stations list
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station stationID)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElem = (from stations in stationsRoot.Elements()
                               where (stations.Element("ID").Value == stationID.ID.ToString())

                               select stations).FirstOrDefault();
            if (stationElem == null)
                throw new InVaildIdException("the station dosn't exists");

            stationElem.Remove();
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }

        /// <summary>
        /// A function that return from the list of the statoins
        /// </summary>
        /// <returns>return the list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            if (predicate == null)
                return listOfAllStations;
            return listOfAllStations.Where(predicate);


        }
        /// <summary>
        ///  A function that recieve a stations id and return from the list of the stations the station with this id
        /// </summary>
        /// <param name="id">the id of the station</param>
        /// <returns>return the station with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            var listOfStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station station = listOfStation.Find(x => x.ID == stationID);
            if (!listOfStation.Exists(x => x.ID == station.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return station;

        }
        /// <summary>
        /// a function to update a station in the list
        /// </summary>
        /// <param name="st">the station to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station stationT)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);

            Station station = listOfAllStations.Find(x => x.ID == stationT.ID);

            if (!listOfAllStations.Exists(x => x.ID == stationT.ID))
                throw new InVaildIdException("This drone doesn't exist in the system");
            station.ChargeSlots = stationT.ChargeSlots;
            if (stationT.Name != null)
                station.Name = stationT.Name;
            int index = listOfAllStations.FindIndex(x => x.ID == station.ID);
            listOfAllStations.RemoveAt(index);
            listOfAllStations.Insert(index, station);

            XMLTools.SaveListToXMLSerializer<Station>(listOfAllStations, stationPath);
        }
        /// <summary>
        /// a function that update  the charge slots of the station plus 1
        /// </summary>
        /// <param name="stationId">the station to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePluseChargeSlots(int stationId)
        {

            Station s = GetStation(stationId);
            s.ChargeSlots++;
            UpdateStation(s);

        }
        #endregion

        #region drone
        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="dr">the drone with new data to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrones(Drone droneToUpdate)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            Drone drone = listOfAllDrones.Find(x => x.ID == droneToUpdate.ID);

            if (!listOfAllDrones.Exists(x => x.ID == droneToUpdate.ID))
                throw new InVaildIdException("This drone doesn't exist in the system");
            drone.Model = droneToUpdate.Model;

            int index = listOfAllDrones.FindIndex(x => x.ID == drone.ID);
            listOfAllDrones.RemoveAt(index);
            listOfAllDrones.Insert(index, drone);

            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }
        /// <summary>
        ///  A function that recieve a drone and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone droneToAdd)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (listOfAllDrones.Exists(x => x.ID == droneToAdd.ID))
                throw new AlreadyExistExeption("The point already axist in the path");
            listOfAllDrones.Add(droneToAdd);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }
        /// <summary>
        /// a function to delete a drone
        /// </summary>
        /// <param name="id">the id of the drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Drone drone = listOfAllDrones.Find(x => x.ID == id);
            if (!listOfAllDrones.Exists(x => x.ID == id))
                throw new InVaildIdException("The drone doesn't exist in system");
            listOfAllDrones.Remove(drone);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }

        /// <summary>
        /// A function that recieve a drones id and return from the list of the drones the drone with this id
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns>return the drone with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            Drone drone = listOfAllDrones.Find(x => x.ID == id);
            if (!listOfAllDrones.Exists(x => x.ID == id))
                throw new InVaildIdException("The drone in path doesn't exist");
            return drone;
        }
        /// <summary>
        /// A function that return from the list of the drones
        /// </summary>
        /// <returns>return the list of drones </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Func<Drone, bool> predicate = null)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (predicate == null)
                return listOfAllDrones;
            return listOfAllDrones.Where(predicate);

        }

        #endregion

        #region users
        /// <summary>
        /// a function to add a user 
        /// </summary>
        /// <param name="userToAdd">the user to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User userToAdd)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            if (listOfAllUsers.Exists(x => x.UserName == userToAdd.UserName))
                throw new AlreadyExistExeption("This user already exist");
            listOfAllUsers.Add(userToAdd);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        /// <summary>
        /// a function that return part of the users list
        /// </summary>
        /// <param name="UserCondition">the condition to the users</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetPartOfUsers(Predicate<User> UserCondition)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            var list = from user in listOfAllUsers
                       where (UserCondition(user))
                       select user;
            return list;
        }
        /// <summary>
        ///  function that delete a user
        /// </summary>
        /// <param name="userName">the name of the user</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User user = listOfAllUsers.Find(x => x.UserName == userName);
            if (!listOfAllUsers.Exists(x => x.UserName == userName))
                throw new InVaildIdException("the user dosn't exist in system");
            listOfAllUsers.Remove(user);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        /// <summary>
        /// a function that returns the users list
        /// </summary>
        /// <returns>the list of users</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetAllUsers()
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            return listOfAllUsers;
        }
        /// <summary>
        /// a function that returns the user with this name
        /// </summary>
        /// <param name="userName">the name of the user</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == userName);
            if (listOfAllUsers.Exists(x => x.UserName == userName))
                return myUser;
            throw new InVaildIdException("the user doesn't exists in system");
        }
        /// <summary>
        /// a function that update the parcel
        /// </summary>
        /// <param name="usertoUpdate">the parcel to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateUser(User usertoUpdate)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == usertoUpdate.UserName);
            if (!listOfAllUsers.Exists(x => x.UserName == usertoUpdate.UserName))
                throw new InVaildIdException("This user doesn't exist in the system");
            myUser.Password = usertoUpdate.Password;
            myUser.Worker = usertoUpdate.Worker;
            myUser.IsActive = usertoUpdate.IsActive;
            myUser.UserName = usertoUpdate.UserName;

            int index = listOfAllUsers.FindIndex(x => x.UserName == myUser.UserName);
            listOfAllUsers.RemoveAt(index);
            listOfAllUsers.Insert(index, myUser);

            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        /// <summary>
        /// A function that gets a user and return true if the user exists or false if not
        /// </summary>
        /// <param name="userDO">the user it checks</param>
        /// <returns>true if the user exists or false if not</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool LogInVerify(User user)
        {
            List<User> users = XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            DO.User us = users.Find(u => u.UserName == user.UserName);
            if (users.Exists(x => x.UserName == us.UserName))
            {
                if (us.Password == user.Password)
                {

                    return true;
                }
                else
                    throw new DO.InVaildIdException($"wrong password:{user.UserName}");
            }
            else
                throw new DO.InVaildIdException($"bad user id: {user.UserName}");
        }
        /// <summary>
        /// A function that gets a user and return true if the user is a worker or false if not
        /// </summary>
        /// <param name="userDO">the user it checks</param>
        /// <returns>true if the user is a worker or false if not</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool isWorker(User user)
        {
            bool worker;
            DO.User us = GetUser(user.UserName);

            worker = us.Worker;
            return worker;
        }


        #endregion

        #region customer
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new AlreadyExistExeption("The bus already exist in the system");
            listOfCustomer.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
            User u = new User()
            {
                UserName = customer.Name,
                Password = customer.ID.ToString(),
                Worker = false
            };
            AddUser(u);
        }
        /// <summary>
        /// A function that recieve a customer and delete the customer whith the same id in the customers list
        /// </summary>
        /// <param name="customerID">the customer id to delete</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new InVaildIdException("The customer doesn't exist in system");
            listOfCustomer.Remove(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);

        }
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="id">the id of the customer</param>
        /// <returns>return the customer with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return customer;
        }

        /// <summary>
        /// A function that return from the list of the customers
        /// </summary>
        /// <returns>return the list of customers</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> Getcustomers(Func<Customer, bool> predicate = null)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (predicate == null)
                return listOfCustomer;
            return listOfCustomer.Where(predicate);

        }
        /// <summary>
        /// a function to update a customer in the list
        /// </summary>
        /// <param name="customer">the customer to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomers(Customer customer)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer myCustomer = listOfCustomer.Find(x => x.ID == customer.ID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))

                throw new InVaildIdException("This customer doesn't exist in the system");
            myCustomer.ID = customer.ID;
            myCustomer.Name = customer.Name;
            myCustomer.Phone = customer.Phone;
            myCustomer.Lattitude = customer.Lattitude;
            myCustomer.Longitude = customer.Longitude;

            int index = listOfCustomer.FindIndex(x => x.ID == myCustomer.ID);
            listOfCustomer.RemoveAt(index);
            listOfCustomer.Insert(index, myCustomer);

            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        #endregion

        #region droneCharge
        /// <summary>
        /// A function that recieve a droneCharge id and return from the list of the droneCharges the droneCharge with this id
        /// </summary>
        /// <param name="id">the id of the droneCharge</param>
        /// <returns>return the droneCharge with this id </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int id)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge droneC = listOfAllDrones.Find(x => x.DroneId == id);
            if (!listOfAllDrones.Exists(x => x.DroneId == id))
                throw new InVaildIdException("The droneCharge in path doesn't exist");
            return droneC;

        }
        /// <summary>
        /// A function that recieve a dronecharge and add it to the lists of the dronescharge
        /// </summary>
        /// <param name="droneCharge">the drone to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (listOfAllDroneCharge.Exists(x => x.DroneId == droneCharge.DroneId))
                throw new AlreadyExistExeption("The DronCharge already axist in the path");
            listOfAllDroneCharge.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(listOfAllDroneCharge, droneChargePath);
        }
        /// <summary>
        /// A function that recieve a droneCharge and delete the droneCharge whith the same id in the droneCharges list
        /// </summary>
        /// <param name="dCharge">the drone to delete</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void deleteDroneCharge(DroneCharge dCharge)
        {
            XElement droneRoot = XMLTools.LoadListFromXMLElement(droneChargePath);

            XElement droneX = (from p in droneRoot.Elements()
                               where (p.Element("DroneId").Value == dCharge.DroneId.ToString() && p.Element("StationId").Value == dCharge.StationId.ToString())
                               select p).FirstOrDefault();
            if (droneX == null)
                throw new InVaildIdException("the data about parcel doesn't exist in system");
            droneX.Remove();
            XMLTools.SaveListToXMLElement(droneRoot, droneChargePath);
        }
        /// <summary>
        /// a function that returns the list of drones charge
        /// </summary>
        /// <param name="predicate">if there is a requierment</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesInCharge(Func<DroneCharge, bool> predicate = null)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (predicate == null)
                return listOfAllDrones;
            return listOfAllDrones.Where(predicate);

        }


        #endregion

        #region parcel
        /// <summary>
        ///  A function that recieve a parcel and add it to the lists of the parcels
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcelToAdd)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            XElement configXml = XMLTools.LoadListFromXMLElement(configPath);
            parcelToAdd.ID = 1 + int.Parse(configXml.Element("RowNumbers").Element("NewParcelId").Value);
            parcelToAdd.Requested = DateTime.Now;
            parcelToAdd.Scheduled = null;
            parcelToAdd.PickedUp = null;
            parcelToAdd.Delivered = null;
            listOfAllParcels.Add(parcelToAdd);
            XMLTools.SaveListToXMLSerializer(listOfAllParcels, parcelPath);
            configXml.Element("RowNumbers").Element("NewParcelId").Value = parcelToAdd.ID.ToString();
            XMLTools.SaveListToXMLElement(configXml, "ConfigXml.xml");

        }
        /// <summary>
        /// A function that recieve a parcel and delete the station whith the same id in the parcels list
        /// </summary>
        /// <param name="p">the parcel to delete</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel parcel)
        {
            XElement parcelRoot = XMLTools.LoadListFromXMLElement(parcelPath);

            XElement parcelX = (from p in parcelRoot.Elements()
                                where (p.Element("ID").Value == parcel.ID.ToString())
                                select p).FirstOrDefault();
            if (parcelX == null)
                throw new InVaildIdException("the data about parcel doesn't exist in system");
            parcelX.Remove();
            XMLTools.SaveListToXMLElement(parcelRoot, parcelPath);
        }
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcels the parcel with this id
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        /// <returns>return the drone with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = listOfAllParcels.Find(x => x.ID == id);
            if (!listOfAllParcels.Exists(x => x.ID == id))
                throw new InVaildIdException("The parcel in path doesn't exist");
            return parcel;
        }
        /// <summary>
        /// A function that return from the list of the parcels
        /// </summary>
        /// <returns>return the list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (predicate == null)
                return listOfAllParcels;
            return listOfAllParcels.Where(predicate);

        }
        /// <summary>
        /// a function to update a parcel in the list
        /// </summary>
        /// <param name="parcel">the parcel to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcels(Parcel parceltoUpdate)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = listOfAllParcels.Find(x => x.ID == parceltoUpdate.ID);
            if (!listOfAllParcels.Exists(x => x.ID == parceltoUpdate.ID))
                throw new InVaildIdException("This parcel doesn't exist in the system");
            parcel.Delivered = parceltoUpdate.Delivered;
            parcel.DroneId = parceltoUpdate.DroneId;
            parcel.Longitude = parceltoUpdate.Longitude;
            parcel.PickedUp = parceltoUpdate.PickedUp;
            parcel.Priority = parceltoUpdate.Priority;
            parcel.Requested = parceltoUpdate.Requested;
            parcel.Scheduled = parceltoUpdate.Scheduled;
            parcel.SenderId = parceltoUpdate.SenderId;
            parcel.TargetId = parceltoUpdate.TargetId;

            int index = listOfAllParcels.FindIndex(x => x.ID == parcel.ID);
            listOfAllParcels.RemoveAt(index);
            listOfAllParcels.Insert(index, parcel);

            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelPath);
        }
        /// <summary>
        /// a function that returns the status of the parcel
        /// </summary>
        /// <param name="iD">the id of the parcel</param>
        /// <returns>the status of the parcel</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int GetStatusOfParcel(int iD)
        {
            Parcel parcel = GetParcel(iD);
            if (parcel.Requested != null)
                return 0;
            if (parcel.Scheduled != null)
                return 1;
            if (parcel.PickedUp != null)
                return 2;
            if (parcel.Delivered != null)
                return 3;
            return -1;
        }


        /// <summary>
        /// A function that recieve a station and a drone and create a new dronecharge with both id`s and update the station accordingly
        /// </summary>
        /// <param name="station">the station that the drone charge there</param>
        /// <param name="drone">the drone thet need to charge</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AnchorDroneStation(Station station, Drone drone)
        {

            try
            {
                GetStation(station.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot anchor station{station.ID}to drone", p);

            }
            try
            {
                GetDrone(drone.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot anchor drone{drone.ID}to station", p);
            }
            if(station.ChargeSlots>0)
                 station.ChargeSlots--;
            DroneCharge dCharge = new DroneCharge()
            {
                DroneId = drone.ID,
                StationId = station.ID
            };
            AddDroneCharge(dCharge);
            UpdateStation(station);
        }
        /// <summary>
        /// A function that recieve a parcel and a drone and and update the parcel to be connected to the drone, update the list of the parcels and drones accordingly
        /// </summary>
        /// <param name="parcel">the parcel thet need to updat</param>
        /// <param name="drone">the drone thet need to updat</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void BelongingParcel(Parcel parcel, Drone drone)
        {
            try
            {
                GetParcel(parcel.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot belonging parcel{parcel.ID}to station", p);
            }
            try
            {
                GetDrone(drone.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot belonging parcel{drone.ID}to station", p);
            }

            parcel.DroneId = drone.ID;
            parcel.Scheduled = DateTime.Today;
            //drone.Status = DroneStatuses.shipping;

            //updating drones
            UpdateDrones(drone);

            //updating parcels
            UpdateParcels(parcel);
        }
        /// <summary>
        /// A function that recieve a station and a drone and releas the ststion that charged him, and update the lists of the drones and station accordingly
        /// </summary>
        /// <param name="drone">the drone its need to release</param>
        /// <param name="st">the station that charged this drone</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleasDrone(Drone drone, Station st)
        {
            try
            {
                GetStation(st.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot releas drone{ st.ID}from station", p);
            }
            try
            {
                GetDrone(drone.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot releas drone{drone.ID}from station", p);
            }
            DroneCharge dCharge = new DroneCharge()
            {
                DroneId = drone.ID,
                StationId = st.ID
            };
            deleteDroneCharge(dCharge);
            st.ChargeSlots++;
            UpdateStation(st);
        }

        /// <summary>
        /// function that recieve a parcel and a customer,update the parcel supply to the customer and update  the lists of the customers and parcels accordingly
        /// </summary>
        /// <param name="parcel">the parcel its need to supply</param>
        /// <param name="customer">the customer that recieve the parcel</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SupplyParcel(Parcel parcel, Customer customer)
        {
            try
            {
                GetParcel(parcel.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot supllay parcel{ parcel.ID}to customer", p);
            }
            try
            {
                GetCustomer(customer.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot supllay parcel{ parcel.ID}to customer", p);
            }


            parcel.Scheduled = DateTime.Today;
            DeleteCustomer(customer.ID);
            UpdateParcels(parcel);
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="latOrLot"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string DecimalToSexagesimal(double coord, char latOrLot)
        {
            char direction;
            if (latOrLot == 't')// if latitude
                if (coord >= 0)//determines how many minutse norht or south 0 is the equator larger then 0 is north smaller is south
                    direction = 'N';
                else
                {
                    direction = 'S';
                    coord = coord * -1;
                }
            else//if longitude
                if (coord >= 0) //determines how many minutse east or west, 0 is Grinwich larger then 0 is east smaller is west
                direction = 'E';
            else
            {
                direction = 'W';
                coord = coord * -1;
            }
            //determines the various sexagesimal factors
            int deg = (int)(coord / 1);
            int min = (int)((coord % 1) * 60) / 1;
            double sec = (((coord % 1) * 60) % 1) * 60;
            const string quote = "\"";
            string toReturn = deg + "° " + min + $"' " + sec + quote + direction;
            return toReturn;
        }
        /// <summary>
        /// computes half a versine of the angle
        /// </summary>
        /// <param name="radian">the angle`s computes </param>
        /// <returns>return half a versine of the angle</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Hav(double radian)
        {
            return Math.Sin(radian / 2) * Math.Sin(radian / 2);
        }
        /// <summary>
        /// computes an angle in radians
        /// </summary>
        /// <param name="degree">a number to transfore to radian</param>
        /// <returns>returns an angle in radians</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Radians(double degree)
        {
            return degree * Math.PI / 180;
        }
        /// <summary>
        ///  receiving 2 points the haversine formula returns the distance (in km) between the 2
        /// </summary>
        /// <param name="lon1"></param>
        /// <param name="lat1"></param>
        /// <param name="lon2"></param>
        /// <param name="lat2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Haversine(double lon1, double lat1, double lon2, double lat2)
        {
            const int RADIUS = 6371;//earths radius in KM

            double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
            double radLat = Radians(lat2 - lat1);
            double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));//haversine formula determines the spherical distance between the two points using given versine
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }
        /// <summary>
        /// a unction that calculate the number of available Charging Ports
        /// </summary>
        /// <param name="baseStationId">the base staion to check on</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AvailableChargingPorts(int baseStationId)
        {
            try
            {
                return GetStation(baseStationId).ChargeSlots - GetDronesInCharge().Count(dc => dc.StationId == baseStationId);

            }
            catch (InVaildIdException ex)
            {
                throw new InVaildIdException("Station didn't exist", ex);
            }
        }
        /// <summary>
        /// a unction that calculate the number of parcels the customer got
        /// </summary>
        /// <param name="customerId">the customer to check on</param>
        /// <returns>the number of parcels the customer got</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerGot(int customerId)
        {
            return GetParcels(h => h.TargetId == customerId).Count();
        }
        /// <summary>
        /// a unction that calculate the number of parcels the customer send and not delievered
        /// </summary>
        /// <param name="customerId">the customer to check on</param>
        /// <returns>the number of parcels the customer send and delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndDelivered(int customerId)
        {
            return GetParcels(h => h.SenderId == customerId && h.Delivered != null).Count();
        }
        /// <summary>
        /// a function to update a parcel in the list
        /// </summary>
        /// <param name="parcel">the parcel to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// A function that recieve a customer and update the customer whith the same id in the customers list
        /// </summary>
        /// <param name="customer">the customer with new data to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// a function to calc a distance between points
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="lonP"></param>
        /// <param name="latP"></param>
        /// <returns>the distance</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distance(int ID, double lonP, double latP)
        {

            if (ID > 9999)//if its a customer
                foreach (Customer cus in Getcustomers()) { if (cus.ID == ID) return Haversine(lonP, latP, cus.Longitude, cus.Lattitude); }

            // DataSource.customerList.ForEach(c => { if (int.Parse(c.ID) == ID) { return Haversine(lonP, latP, c.longitude, c.latitude); });//returns in a string the distnace between the customer and given point                  
            else//its a station
                //DataSource.stationsList.ForEach(s => { if (s.ID == ID) { return Haversine(lonP, latP, s.longitude, s.latitude); });//returns in a string the distnace between the station and given point                  
                foreach (Station Kingsx in GetStations()) { if (Kingsx.ID == ID) return Haversine(lonP, latP, Kingsx.Longitude, Kingsx.Lattitude); }
            return 0.0;// default return
        }
        /// <summary>
        /// a function that returns an array with the power requested in each status of the drone
        /// </summary>
        /// <returns>an array with the power requested in each status of the drone</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerRequest()
        {

           var l= XMLTools.LoadListFromXMLElement(configPath);
            return  l.Element("BatteryUsages").Elements()
                    .Select(e => Convert.ToDouble(e.Value)).ToArray();
            
        }
        /// <summary>
        /// a unction that calculate the number of parcels the customer send and not delievered
        /// </summary>
        /// <param name="iD">the customer to check on</param>
        /// <returns>the number of parcels the customer send and not delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndNotDelivered(int iD)
        {
            return GetParcels(h => h.SenderId == iD && h.Delivered == null).Count();
        }

        /// <summary>
        /// a unction that calculate the number of parcels on the way to the customer 
        /// </summary>
        /// <param name="iD">the customer to check on</param>
        /// <returns>the number of parcels the customer send and not delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsInTheWayToCustomer(int customerId)
        {
            return GetParcels(h => h.TargetId == customerId && h.PickedUp != null).Count();

        }

        /// <summary>
        ///  A function that recieve a station and update the station whith the same id in the stations list
        /// </summary>
        /// <param name="stationT">the drone with new data to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStations(Station stationT)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);

            Station station = listOfAllStations.Find(x => x.ID == stationT.ID);

            if (!listOfAllStations.Exists(x => x.ID == stationT.ID))
                throw new InVaildIdException("This drone doesn't exist in the system");
            station.ChargeSlots = stationT.ChargeSlots;
            if (stationT.Name != null)
                station.Name = stationT.Name;
            int index = listOfAllStations.FindIndex(x => x.ID == station.ID);
            listOfAllStations.RemoveAt(index);
            listOfAllStations.Insert(index, station);

            XMLTools.SaveListToXMLSerializer<Station>(listOfAllStations, stationPath);
        }


        #endregion



    }
}
