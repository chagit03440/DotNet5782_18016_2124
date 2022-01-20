﻿using System.Runtime.CompilerServices;
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
            foreach (var item in droneCharge)
            {
                UpdatePluseChargeSlots(item.StationId);
            }
            droneCharge.Clear();
            XMLTools.SaveListToXMLSerializer(droneCharge, droneChargePath);
        }
        #endregion


        #region station

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


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null)
        {
            List<Station> listOfAllStations = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            if (predicate == null)
                return listOfAllStations;
            return listOfAllStations.Where(predicate);


        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int stationID)
        {
            var listOfStation = XMLTools.LoadListFromXMLSerializer<Station>(stationPath);
            Station station = listOfStation.Find(x => x.ID == stationID);
            if (!listOfStation.Exists(x => x.ID == station.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return station;

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station station)
        {
            XElement stationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stations = (from stationElem in stationsRoot.Elements()
                            where (stationElem.Element("ID").Value == station.ID.ToString())

                            select stationElem).FirstOrDefault();
            if (stations == null)
                throw new InVaildIdException("the station dosn't exists");
            Station s = GetStation(station.ID);
            stations.Element("Name").Value = s.Name.ToString();
            stations.Element("ChargeSlots").Value = station.ChargeSlots.ToString();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdatePluseChargeSlots(int stationId)
        {

            Station s = GetStation(stationId);
            s.ChargeSlots++;
            UpdateStation(s);

        }
        #endregion

        #region drone
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDrone(Drone droneToAdd)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            if (listOfAllDrones.Exists(x => x.ID == droneToAdd.ID))
                throw new AlreadyExistExeption("The point already axist in the path");
            listOfAllDrones.Add(droneToAdd);
            XMLTools.SaveListToXMLSerializer<Drone>(listOfAllDrones, dronePath);
        }
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int id)
        {
            List<Drone> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            Drone drone = listOfAllDrones.Find(x => x.ID == id);
            if (!listOfAllDrones.Exists(x => x.ID == id))
                throw new InVaildIdException("The drone in path doesn't exist");
            return drone;
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddUser(User userToAdd)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            if (listOfAllUsers.Exists(x => x.UserName == userToAdd.UserName))
                throw new AlreadyExistExeption("This user already exist");
            listOfAllUsers.Add(userToAdd);
            XMLTools.SaveListToXMLSerializer<User>(listOfAllUsers, usersPath);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetPartOfUsers(Predicate<User> UserCondition)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            var list = from user in listOfAllUsers
                       where (UserCondition(user))
                       select user;
            return list;
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<User> GetAllUsers()
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            return listOfAllUsers;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public User GetUser(string userName)
        {
            List<User> listOfAllUsers = XMLTools.LoadListFromXMLSerializer<User>(usersPath);
            User myUser = listOfAllUsers.Find(x => x.UserName == userName);
            if (listOfAllUsers.Exists(x => x.UserName == userName))
                return myUser;
            throw new InVaildIdException("the user doesn't exists in system");
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (listOfCustomer.Exists(x => x.ID == customer.ID))
                throw new AlreadyExistExeption("The bus already exist in the system");
            listOfCustomer.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int customerID)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            Customer customer = listOfCustomer.Find(x => x.ID == customerID);
            if (!listOfCustomer.Exists(x => x.ID == customer.ID))

                throw new InVaildIdException("The customer doesn't exist in system");
            return customer;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> Getcustomers(Func<Customer, bool> predicate = null)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if (predicate == null)
                return listOfCustomer;
            return listOfCustomer.Where(predicate);

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
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

            int index = listOfCustomer.FindIndex(x => x.ID == myCustomer.ID);
            listOfCustomer.RemoveAt(index);
            listOfCustomer.Insert(index, myCustomer);

            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        #endregion

        #region droneCharge


        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int id)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            DroneCharge droneC = listOfAllDrones.Find(x => x.DroneId == id);
            if (!listOfAllDrones.Exists(x => x.DroneId == id))
                throw new InVaildIdException("The droneCharge in path doesn't exist");
            return droneC;

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDroneCharge(DroneCharge droneCharge)
        {
            List<DroneCharge> listOfAllDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (listOfAllDroneCharge.Exists(x => x.DroneId == droneCharge.DroneId))
                throw new AlreadyExistExeption("The DronCharge already axist in the path");
            listOfAllDroneCharge.Add(droneCharge);
            XMLTools.SaveListToXMLSerializer<DroneCharge>(listOfAllDroneCharge, droneChargePath);
        }
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel parcelToAdd)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (!listOfAllParcels.Exists(x => x.ID == parcelToAdd.ID))
                throw new AlreadyExistExeption("The parcel already axist in the path");
            listOfAllParcels.Add(parcelToAdd);
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelPath);
        }

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = listOfAllParcels.Find(x => x.ID == id);
            if (!listOfAllParcels.Exists(x => x.ID == id))
                throw new InVaildIdException("The parcel in path doesn't exist");
            return parcel;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (predicate == null)
                return listOfAllParcels;
            return listOfAllParcels.Where(predicate);

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parceltoUpdate)
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
            station.ChargeSlots--;
            DroneCharge dCharge = new DroneCharge()
            {
                DroneId = drone.ID,
                StationId = station.ID
            };
            AddDroneCharge(dCharge);
            UpdateStation(station);
        }

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
            UpdateParcel(parcel);
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Hav(double radian)
        {
            return Math.Sin(radian / 2) * Math.Sin(radian / 2);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Radians(double degree)
        {
            return degree * Math.PI / 180;
        }
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerGot(int customerId)
        {
            return GetParcels(h => h.TargetId == customerId).Count();
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndDelivered(int customerId)
        {
            return GetParcels(h => h.SenderId == customerId && h.Delivered != null).Count();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcels(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomers(Customer customer)
        {
            throw new NotImplementedException();
        }

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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerRequest()
        {
            double[] arr = new double[5];
            arr[0] = available;
            arr[1] = lightWeight;
            arr[2] = mediumWeight;
            arr[3] = heavyWeight;
            arr[4] = chargingRate;
            return arr;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndNotDelivered(int iD)
        {
            return GetParcels(h => h.SenderId == iD && h.Delivered == null).Count();
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsInTheWayToCustomer(int customerId)
        {
            return GetParcels(h => h.TargetId == customerId && h.PickedUp != null).Count();

        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStations(Station st)
        {
            throw new NotImplementedException();
        }


        #endregion



    }
}
