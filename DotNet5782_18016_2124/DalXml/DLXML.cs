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
        string customerPath = @"CustomersXml.xml";//XElement
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


        public void DeleteStation(int firstCodeStation, int secondCodeStation)
        {
            XElement twoStationsRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var twoStationElem = (from twoStations in twoStationsRoot.Elements()
                                  where (twoStations.Element("FirstStationCode").Value == firstCodeStation.ToString()
                                  && twoStations.Element("SecondStationCode").Value == secondCodeStation.ToString())
                                  select twoStations).FirstOrDefault();
            if (twoStationElem == null)
                throw new InfoTwoStationsMissException(firstCodeStation, secondCodeStation, "miss information beteen to stations");

            twoStationElem.Remove();
            XMLTools.SaveListToXMLElement(twoStationsRoot, twoStationsPath);
        }

        public IEnumerable<TwoFollowingStations> GetAllFollowingStations()
        {
            XElement twoStationsRoot = XMLTools.LoadListFromXMLElement(twoStationsPath);
            var allFollowingStations = from twoStations in twoStationsRoot.Elements()
                                       select new TwoFollowingStations
                                       {
                                           AverageTimeBetweenStations = TimeSpan.Parse(twoStations.Element("AverageTimeBetweenStations").Value),
                                           FirstStationCode = Convert.ToInt32(twoStations.Element("FirstStationCode").Value),
                                           SecondStationCode = Convert.ToInt32(twoStations.Element("SecondStationCode").Value),
                                           DistanceBetweenStations = Convert.ToDouble(twoStations.Element("DistanceBetweenStations").Value)
                                       };
            return allFollowingStations;
        }

        public IEnumerable<TwoFollowingStations> GetPartOfTwoFollowingStations(Predicate<TwoFollowingStations> TwoFollowingStationsCondition)
        {
            XElement twoStationsRoot = XMLTools.LoadListFromXMLElement(twoStationsPath);
            var partOfFollowingStations = from twoStations in twoStationsRoot.Elements()
                                          let stations = new TwoFollowingStations
                                          {
                                              AverageTimeBetweenStations = TimeSpan.Parse(twoStations.Element("AverageTimeBetweenStations").Value),
                                              FirstStationCode = Convert.ToInt32(twoStations.Element("FirstStationCode").Value),
                                              SecondStationCode = Convert.ToInt32(twoStations.Element("SecondStationCode").Value),
                                              DistanceBetweenStations = Convert.ToDouble(twoStations.Element("DistanceBetweenStations").Value)
                                          }
                                          where TwoFollowingStationsCondition(stations)
                                          select stations;

            return partOfFollowingStations;
        }

        public TwoFollowingStations GetTwoStations(int firstCodeStation, int secondCodeStation)
        {
            XElement twoStationsRoot = XMLTools.LoadListFromXMLElement(twoStationsPath);
            var twoStations = (from twoStationsElem in twoStationsRoot.Elements()
                               where (twoStationsElem.Element("FirstStationCode").Value == firstCodeStation.ToString()
                               && twoStationsElem.Element("SecondStationCode").Value == secondCodeStation.ToString())
                               select twoStationsElem).FirstOrDefault();
            if (twoStations == null)
                throw new InfoTwoStationsMissException(firstCodeStation, secondCodeStation, "miss information beteen to stations");
            return new TwoFollowingStations
            {
                AverageTimeBetweenStations = TimeSpan.Parse(twoStations.Element("AverageTimeBetweenStations").Value),
                FirstStationCode = Convert.ToInt32(twoStations.Element("FirstStationCode").Value),
                SecondStationCode = Convert.ToInt32(twoStations.Element("SecondStationCode").Value),
                DistanceBetweenStations = Convert.ToDouble(twoStations.Element("DistanceBetweenStations").Value)
            };
        }

        public void UpdateTwoFollowingStations(TwoFollowingStations twoStationToUpdate)
        {
            XElement twoStationsRoot = XMLTools.LoadListFromXMLElement(twoStationsPath);
            var twoStations = (from twoStationsElem in twoStationsRoot.Elements()
                               where (twoStationsElem.Element("FirstStationCode").Value == twoStationToUpdate.FirstStationCode.ToString()
                               && twoStationsElem.Element("SecondStationCode").Value == twoStationToUpdate.SecondStationCode.ToString())
                               select twoStationsElem).FirstOrDefault();
            if (twoStations == null)
                throw new InfoTwoStationsMissException(twoStationToUpdate.FirstStationCode, twoStationToUpdate.SecondStationCode,
     "miss information beteen to stations");
            twoStations.Element("AverageTimeBetweenStations").Value = twoStationToUpdate.AverageTimeBetweenStations.ToString();
            twoStations.Element("DistanceBetweenStations").Value = twoStationToUpdate.DistanceBetweenStations.ToString();
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
            if (listOfAllDrones.Find(x => x.ID == droneToAdd.ID) != null)
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
            var listOfCustomer= XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            if ((listOfCustomer.Find(x => x.ID == customer.ID))!= null)
                throw new AlreadyExistExeption("The customer already exist in the system");
            listOfCustomer.Add(customer);
            XMLTools.SaveListToXMLSerializer<Customer>(listOfCustomer, customerPath);
        }

        public void DeleteBus(string licenseNumberToDelete)
        {
            var listOfBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            Bus bus = listOfBuses.Find(x => x.LicenseNumber == licenseNumberToDelete);
            if (bus == null || bus.Deleted)
                throw new DoesntExistException("The bus doesn't exist in system");
            listOfBuses.Remove(bus);
            XMLTools.SaveListToXMLSerializer<Bus>(listOfBuses, busesPath);

        }

        public Bus GetABus(string licenseNumber)
        {
            var listOfBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            Bus bus = listOfBuses.Find(x => x.LicenseNumber == licenseNumber);
            if (bus != null)
                return bus;
            throw new DoesntExistException("The bus doesn't exist in system");
        }

        public IEnumerable<Bus> GetAllBuses()
        {
            return XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
        }

        public IEnumerable<Bus> GetPartOfBuses(Predicate<Bus> BusCondition)
        {
            var listOfBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            var list = from bus in listOfBuses
                       where (BusCondition(bus))
                       select bus;
            return list;
        }

        public void UpdateBus(Bus busToUpdate)
        {
            var listOfBuses = XMLTools.LoadListFromXMLSerializer<Bus>(busesPath);
            Bus myBus = listOfBuses.Find(x => x.LicenseNumber == busToUpdate.LicenseNumber);
            if (myBus == null)
                throw new DoesntExistException("This bus doesn't exist in the system");
            myBus.Mileage = busToUpdate.Mileage;
            myBus.InCity = busToUpdate.InCity;
            myBus.SelfPayment = busToUpdate.SelfPayment;
            myBus.Status = busToUpdate.Status;
            XMLTools.SaveListToXMLSerializer<Bus>(listOfBuses, busesPath);
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
            if (predicate==null)
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
            if (!listOfAllParcels.Exists(x => x.ID ==id))
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

        #region stations
        public void AddStation(Station stationToAdd)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var stationElement = (from p in stationRoot.Elements()
                                  where Convert.ToInt32(p.Element("StationCode").Value) == stationToAdd.StationCode
                                  select p).FirstOrDefault();
            if (stationElement != null)
                throw new AlreadyExistExeption("The station already exist in the system");

            XElement stationElem = new XElement("Station", new XElement("StationCode", stationToAdd.StationCode),
                                   new XElement("LocationLatitude", stationToAdd.LocationLatitude),
                                   new XElement("LocationLongitude", stationToAdd.LocationLongitude),
                                   new XElement("StationName", stationToAdd.StationName),
                                   new XElement("StationAddress", stationToAdd.StationAddress),
                                   new XElement("PlaceToSit", stationToAdd.PlaceToSit),
                                   new XElement("BoardBusTiming", stationToAdd.BoardBusTiming));
            stationRoot.Add(stationElem);
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);

        }

        public void DeleteStation(int stationCodeToDelete)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == stationCodeToDelete
                                       select p).FirstOrDefault();
            if (stationElement == null)
                throw new DoesntExistException("This station doesn't exist in system");
            stationElement.Remove();
            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }

        public Station GetAStation(int stationCode)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            var station = (from p in stationRoot.Elements()
                           where p.Element("StationCode").Value == stationCode.ToString()
                           select p).FirstOrDefault();

            if (station != null)
                return new Station()
                {
                    StationCode = Convert.ToInt32(station.Element("StationCode").Value),
                    StationName = station.Element("StationName").Value,
                    LocationLatitude = Convert.ToDouble(station.Element("LocationLatitude").Value),
                    LocationLongitude = Convert.ToDouble(station.Element("LocationLongitude").Value),
                    StationAddress = station.Element("StationAddress").Value,
                    PlaceToSit = Convert.ToBoolean(station.Element("PlaceToSit").Value),
                    BoardBusTiming = Convert.ToBoolean(station.Element("BoardBusTiming").Value)
                };
            throw new DoesntExistException("The station doesn't exist in system");

        }

        public void UpdateStation(Station stationToUpdate)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            XElement stationElement = (from p in stationRoot.Elements()
                                       where Convert.ToInt32(p.Element("StationCode").Value) == stationToUpdate.StationCode
                                       select p).FirstOrDefault();
            stationElement.Element("StationName").Value = stationToUpdate.StationName;
            stationElement.Element("LocationLatitude").Value = stationToUpdate.LocationLatitude.ToString();
            stationElement.Element("LocationLongitude").Value = stationToUpdate.LocationLongitude.ToString();
            stationElement.Element("StationAddress").Value = stationToUpdate.StationAddress;
            stationElement.Element("PlaceToSit").Value = stationToUpdate.PlaceToSit.ToString();
            stationElement.Element("BoardBusTiming").Value = stationToUpdate.BoardBusTiming.ToString();

            XMLTools.SaveListToXMLElement(stationRoot, stationPath);
        }
        public IEnumerable<Station> GetPartOfStations(Predicate<Station> StationCondition)
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            return from p in stationRoot.Elements()
                   let pAsStation = new Station()
                   {
                       StationCode = Convert.ToInt32(p.Element("StationCode").Value),
                       StationName = p.Element("StationName").Value,
                       LocationLatitude = Convert.ToDouble(p.Element("LocationLatitude").Value),
                       LocationLongitude = Convert.ToDouble(p.Element("LocationLongitude").Value),
                       StationAddress = p.Element("StationAddress").Value.ToString(),
                       PlaceToSit = Convert.ToBoolean(p.Element("PlaceToSit").Value),
                       BoardBusTiming = Convert.ToBoolean(p.Element("BoardBusTiming").Value)
                   }
                   where StationCondition(pAsStation)
                   select pAsStation;
        }

        public IEnumerable<Station> GetAllStations()
        {
            XElement stationRoot = XMLTools.LoadListFromXMLElement(stationPath);
            return from p in stationRoot.Elements()
                   select new Station()
                   {
                       StationCode = Convert.ToInt32(p.Element("StationCode").Value),
                       StationName = p.Element("StationName").Value,
                       LocationLatitude = Convert.ToDouble(p.Element("LocationLatitude").Value),
                       LocationLongitude = Convert.ToDouble(p.Element("LocationLongitude").Value),
                       StationAddress = p.Element("StationAddress").Value.ToString(),
                       PlaceToSit = Convert.ToBoolean(p.Element("PlaceToSit").Value),
                       BoardBusTiming = Convert.ToBoolean(p.Element("BoardBusTiming").Value)
                   };

        }
        #endregion

        
 
    }
}
 
