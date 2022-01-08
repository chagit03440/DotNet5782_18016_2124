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
    class DLXML
    {
        #region singelton

        static readonly DLXML instance = new DLXML();
        static DLXML() { }
        DLXML() { }
        public static DLXML Instance => instance;
        #endregion
        string parcelPath = @"ParcelsXml.xml";//XMLSerializer
        string stationPath = @"StationsXml.xml";  //XElement
        string customerPath = @"CustomersXml.xml";//XMLSerializer
        string dronePath = @"DronesXml.xml";//XMLSerializer
        string usersPath = @"UsersXml.xml";//XMLSerializer
        string droneChargePath = @"DroneChargeXml.xml";




        #region station
        public void AddStations(Station station)
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
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }


        public void DeleteStation(int stationID)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElem = (from stations in stationsRoot.Elements()
                               where (stations.Element("ID").Value == stationID.ToString())

                               select stations).FirstOrDefault();
            if (stationElem == null)
                throw new InVaildIdException("the station dosn't exists");

            stationElem.Remove();
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }



        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            if (predicate == null)
                return listOfAllStations;
            return listOfAllStations.Where(predicate);


        }


        public Station GetStation(int stationID)
        {
            var listOfStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station station = listOfStation.Find(x => x.ID == stationID);
            if (!listOfStation.Exists(x => x.ID == station.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return station;

        }

        public void UpdateStations(Station station)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stations = (from stationElem in stationsRoot.Elements()
                            where (stationElem.Element("ID").Value == station.ID.ToString())

                            select stationElem).FirstOrDefault();
            if (stations == null)
                throw new InVaildIdException("the station dosn't exists");

            stations.Element("Name").Value = station.Name.ToString();
            stations.Element("ChargeSlots").Value = station.ChargeSlots.ToString();
        }

        #endregion

        #region drone
        public void UpdateDrone(Drone droneToUpdate)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            Drone drone = listOfAllDrones.Find(x => x.ID == droneToUpdate.ID);

            if (!listOfAllDrones.Exists(x => x.ID == droneToUpdate.ID))
                throw new InVaildIdException("This drone doesn't exist in the system");
            drone.Model = droneToUpdate.Model;
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }
        public void AddDrone(Drone droneToAdd)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (listOfAllDrones.Exists(x => x.ID == droneToAdd.ID))
                throw new AlreadyExistExeption("The point already axist in the path");
            listOfAllDrones.Add(droneToAdd);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }

        public void DeleteDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);

            Drone drone = listOfAllDrones.Find(x => x.ID == id);
            if (!listOfAllDrones.Exists(x => x.ID == id))
                throw new InVaildIdException("The drone doesn't exist in system");
            listOfAllDrones.Remove(drone);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }

        //public IEnumerable<Drone> GetDrones()
        //{
        //    List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
        //    return listOfAllDrones;
        //}

        public Drone GetDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            Drone drone = listOfAllDrones.Find(x => x.ID == id);
            if (!listOfAllDrones.Exists(x => x.ID == id))
                throw new InVaildIdException("The drone in path doesn't exist");
            return drone;
        }

        public IEnumerable<Drone> GetDrones(Func<Drone, bool> predicate = null)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (predicate == null)
                return listOfAllDrones;
            return listOfAllDrones.Where(predicate);

        }

        #endregion

        #region users
        public void AddUser(User userToAdd)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            if (listOfAllUsers.Find(x => x.UserName == userToAdd.UserName) != null)
                throw new AlreadyExistExeption("This user already exist");
            listOfAllUsers.Add(userToAdd);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        public IEnumerable<User> GetPartOfUsers(Predicate<User> UserCondition)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            var list = from user in listOfAllUsers
                       where (UserCondition(user))
                       select user;
            return list;
        }

        public void DeleteUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User user = listOfAllUsers.Find(x => x.UserName == userName);
            if (user == null)
                throw new InVaildIdException("the user dosn't exist in system");
            listOfAllUsers.Remove(user);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        public IEnumerable<User> GetAllUsers()
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            return listOfAllUsers;
        }
        public User GetAUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == userName);
            if (myUser != null)
                return myUser;
            throw new InVaildIdException("the user doesn't exists in system");
        }

        public void UpdateUser(User usertoUpdate)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == usertoUpdate.UserName);
            if (myUser == null)
                throw new InVaildIdException("This user doesn't exist in the system");
            myUser.Password = usertoUpdate.Password;
            myUser.Worker = usertoUpdate.Worker;
            myUser.IsActive = usertoUpdate.IsActive;
            myUser.UserName = usertoUpdate.UserName;
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }

        #endregion

        #region customer
        public void AddCustomer(Customer customer)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new AlreadyExistExeption("The bus already exist in the system");
            listOfCustomer.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        public void DeleteCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new InVaildIdException("The customer doesn't exist in system");
            listOfCustomer.Remove(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);

        }

        public Customer GetCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return customer;
        }

        public IEnumerable<Customer> GetAllCustomer()
        {
            return XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
        }

        public IEnumerable<Customer> GetCustomers(Func<Customer, bool> func = null)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            var list = from customer in listOfCustomer
                       where (func(customer))
                       select customer;
            return list;
        }

        public void UpdateCustomer(Customer customer)
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
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        #endregion

        #region droneCharge 



        public DroneCharge GetDroneCharge(int id)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge droneC = listOfAllDrones.Find(x => x.DroneId == id);
            if (!listOfAllDrones.Exists(x => x.DroneId == id))
                throw new InVaildIdException("The droneCharge in path doesn't exist");
            return droneC;

        }

        //public IEnumerable<DroneCharge> GetDronesInCharge()
        //{
        //    XElement DroneChargeRoot = XMLTools.LoadListFromXMLElement(droneChargePath);
        //    var listOfAllDrones = from drone in DroneChargeRoot.Elements()
        //                         select new DroneCharge
        //                         {
        //                             DroneId = Convert.ToInt32(drone.Element("DroneId").Value),
        //                             StationId = Convert.ToInt32(drone.Element("StationId").Value),
        //                         };
        //    return listOfAllDrones;

        //}

        public IEnumerable<DroneCharge> GetDronesInCharge(Func<DroneCharge, bool> predicate = null)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (predicate == null)
                return listOfAllDrones;
            return listOfAllDrones.Where(predicate);

        }


        #endregion

        #region parcel
        public void AddParcel(Parcel parcelToAdd)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (!listOfAllParcels.Exists(x => x.ID == parcelToAdd.ID))
                throw new AlreadyExistExeption("The parcel already axist in the path");
            listOfAllParcels.Add(parcelToAdd);
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelPath);
        }


        public void DeleteParcel(int id)
        {
            XElement parcelRoot = XMLTools.LoadListFromXMLElement(parcelPath);

            XElement parcel = (from p in parcelRoot.Elements()
                               where (p.Element("ID").Value == id.ToString())
                               select p).FirstOrDefault();
            if (parcel == null)
                throw new InVaildIdException("the data about parcel doesn't exist in system");
            parcel.Remove();
            XMLTools.SaveListToXMLElement(parcelRoot, parcelPath);
        }

        public Parcel GetParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = listOfAllParcels.Find(x => x.ID == id);
            if (!listOfAllParcels.Exists(x => x.ID == id))
                throw new InVaildIdException("The parcel in path doesn't exist");
            return parcel;
        }
        //public IEnumerable<Parcel> GetParcels()
        //{
        //    List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
        //    return listOfAllParcels;
        //}

        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (predicate == null)
                return listOfAllParcels;
            return listOfAllParcels.Where(predicate);

        }


        public void UpdateParcel(Parcel parceltoUpdate, TimeSpan OldFirstExit)
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
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelPath);
        }
        #endregion

       

    }
}