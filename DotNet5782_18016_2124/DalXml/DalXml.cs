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
    class DalXml:IDal
    {
        #region singelton

        static readonly DalXml instance = new DalXml();
        static DalXml() { }
        DalXml() { Initialize(); }
        public static DalXml Instance { get => instance; }
        #endregion
        static string parcelPath = @"data\ParcelsXml.xml";//XMLSerializer
        static string stationPath = @"data\StationsXml.xml";  //XElement
        static string customerPath = @"data\CustomersXml.xml";//XMLSerializer
        static string dronePath = @"data\DronesXml.xml";//XMLSerializer
        static string usersPath = @"data\UsersXml.xml";//XMLSerializer
        static string droneChargePath = @"data\DroneChargeXml.xml";

        static Random rand= new Random();

        internal  void Initialize()
        {
            rand = new Random();
            creatDrone(10);
            //creatStation(10);
            //creatCustomer(10);
            //creatParcel(10);
            //createUsers();
            //XMLTools.SaveListToXMLSerializer(stations, @"StationsXml.xml");
            //XMLTools.SaveListToXMLSerializer(customers, @"CustomersXml.xml");
            //XMLTools.SaveListToXMLSerializer(parcels, @"ParcelsXml.xml");
            //XMLTools.SaveListToXMLSerializer(ListUser, @"UsersXml.xml");
            //XMLTools.SaveListToXMLSerializer(incharge, @"DroneChargeXml.xml");
        }
        /// <summary>
        /// A function that initialize customers with random data
        /// </summary>
        /// <param name="n">a number of customers to intilize</param>
        private  void creatCustomer(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Customer newCustomer = new Customer();

                newCustomer.ID = i + 1000;
                newCustomer.Name = $"Customer {i}";
                newCustomer.Phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}";
                //newCustomer.Lattitude = getRandomCoordinate(31.7);
                //newCustomer.Longitude = getRandomCoordinate(35.1);
                DataSource.customers.Add(newCustomer);


            }
        }


        //private static void createUsers()
        //{
        //    ListUser = new List<User>
        //    {
        //         new User
        //         {
        //              UserName="Chagit",
        //             Password="3440",
        //             Worker=false,
        //             IsActive = true,
        //         },

        //         new User
        //         {
        //             UserName="Sarah",
        //             Password="bbb",
        //             Worker=true,
        //             IsActive = true,
        //         },

        //         new User
        //         {
        //             UserName="Yonhatan",
        //             Password="ccc",
        //             Worker=false,
        //             IsActive = true,
        //         },

        //         new User
        //         {
        //             UserName="Noa",
        //             Password="ddd",
        //             Worker=false,
        //             IsActive = true,
        //         },

        //         new User
        //         {
        //             UserName="Daniel",
        //             Password="eee",
        //             Worker=false,
        //             IsActive = true,
        //         },

        //    };
        //}
        /// <summary>
        /// A function that initialize parcels with random data
        /// </summary>
        /// <param name="n">a number of parcels to intilize</param>
        private  void creatParcel(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Parcel newParcel = new Parcel();
                newParcel.ID = i + 1000;
                newParcel.SenderId = rand.Next(i + 1000, i + 1000 + n);
                newParcel.TargetId = rand.Next(i + 1000, i + 1000 + n);
                newParcel.Longitude = (WeightCategories)rand.Next(3);
                newParcel.DroneId = rand.Next(1000, 9999);
                newParcel.Priority = (Priorities)rand.Next(1, 3);
                newParcel.Requested = null;
                newParcel.Scheduled = null;
                newParcel.PickedUp = null;
                newParcel.Delivered = null;
                //parcels.Add(newParcel);

            }
        }
        private  void creatDrone(int n)
        {
            WeightCategories[] values = WeightCategories.GetValues<WeightCategories>();
            List<Drone> drones = XMLTools.LoadListFromXMLSerializer<Drone>(dronePath);
            for (int i = 0; i < n; i++)
            {
                Drone newDrone = new Drone();
                newDrone.ID = i + 1000;
                //newDrone.Battery = 1;
                newDrone.MaxWeight = values[rand.Next(values.Length)];
                newDrone.Model = "iFly" + i;
                //  newDrone.Status = DroneStatuses.maintenance;
                drones.Add(newDrone);

            }
            XMLTools.SaveListToXMLSerializer(drones, dronePath);

        }
        /// <summary>
        /// A function that initialize stations with random data
        /// </summary>
        /// <param name="n">a number of stations to intilize</param>
        private static void creatStation(int n)
        {
            for (int i = 0; i < n; i++)
            {
                Station newStation = new Station();
                newStation.ID = i + 1000;
                newStation.Name = $"Station{i}";
                newStation.ChargeSlots = 10 + i;
                newStation.Lattitude = 31.785664 + i;
                newStation.Longitude = 35.189938 + i;
                //stations.Add(newStation);

            }
        }



        #region station
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
            XMLTools.SaveListToXMLElement(stationsRoot, stationPath);
        }


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

        public void UpdateStation(Station station)
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
        public void UpdateDrones(Drone droneToUpdate)
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
        public User GetUser(string userName)
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

        //public IEnumerable<Customer> GetAllCustomer()
        //{
        //    return XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
        //}

        public IEnumerable<Customer> Getcustomers(Func<Customer, bool> predicate = null)
        {
            var listOfCustomer = XMLTools.LoadListFromXMLSerializer<Customer>(customerPath);
            var list = from customer in listOfCustomer
                       where (predicate(customer))
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

      

        public IEnumerable<DroneCharge> GetDronesInCharge(Func<DroneCharge, bool> predicate = null)
        {
            List<DroneCharge> listOfAllDrones = XMLTools.LoadListFromXMLSerializer<DroneCharge>(droneChargePath);
            if (predicate == null)
                return listOfAllDrones;
            return listOfAllDrones.Where(predicate);

        }
        private void deleteDroneCharge(DroneCharge dCharge)
        {
            XElement droneRoot = XMLTools.LoadListFromXMLElement(droneChargePath);

            XElement droneX = (from p in droneRoot.Elements()
                                where (p.Element("DroneId").Value == dCharge.DroneId.ToString()&& p.Element("StationId").Value == dCharge.StationId.ToString())
                                select p).FirstOrDefault();
            if (droneX == null)
                throw new InVaildIdException("the data about parcel doesn't exist in system");
            droneX.Remove();
            XMLTools.SaveListToXMLElement(droneRoot, droneChargePath);
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

        public Parcel GetParcel(int id)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            Parcel parcel = listOfAllParcels.Find(x => x.ID == id);
            if (!listOfAllParcels.Exists(x => x.ID == id))
                throw new InVaildIdException("The parcel in path doesn't exist");
            return parcel;
        }
      

        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        {
            List<Parcel> listOfAllParcels = XMLTools.LoadListFromXMLSerializer<Parcel>(parcelPath);
            if (predicate == null)
                return listOfAllParcels;
            return listOfAllParcels.Where(predicate);

        }

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
            XMLTools.SaveListToXMLSerializer<Parcel>(listOfAllParcels, parcelPath);
        }


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

        public bool LogInVerify(User user)
        {
            List<User> users= XMLTools.LoadListFromXMLSerializer<User>(usersPath);

            DO.User us =users.Find(u => u.UserName == user.UserName);
            if (us != null)
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
        public bool isWorker(User user)
        {
            bool worker;
            DO.User us = GetUser(user.UserName);

            worker = us.Worker;
            return worker;
        }
    

   

        public void AnchorDroneStation(Station station, Drone drone)
        {
            throw new NotImplementedException();
        }

        
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

       

        public void SupplyParcel(Parcel parcel, Customer customer)
        {
            throw new NotImplementedException();
        }

        public string DecimalToSexagesimal(double coord, char latOrLot)
        {
            throw new NotImplementedException();
        }

        public double Hav(double radian)
        {
            return Math.Sin(radian / 2) * Math.Sin(radian / 2);
        }

        public double Radians(double degree)
        {
            return degree * Math.PI / 180;
        }

        public double Haversine(double lon1, double lat1, double lon2, double lat2)
        {
            const int RADIUS = 6371;//earths radius in KM

            double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
            double radLat = Radians(lat2 - lat1);
            double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));//haversine formula determines the spherical distance between the two points using given versine
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }

        public int AvailableChargingPorts(int baseStationId)
        {
            throw new NotImplementedException();
        }

        public int ParcelsCustomerGot(int customerId)
        {
            return GetParcels(h => h.TargetId == customerId).Count();
        }

        public int ParcelsCustomerSendAndDelivered(int customerId)
        {
            return GetParcels(h => h.SenderId == customerId && h.Delivered != null).Count();
        }

        public void UpdateParcels(Parcel parcel)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomers(Customer customer)
        {
            throw new NotImplementedException();
        }

        public double Distance(int ID, double lonP, double latP)
        {
            throw new NotImplementedException();
        }

        public double[] PowerRequest()
        {
            throw new NotImplementedException();
        }

        public int ParcelsCustomerSendAndNotDelivered(int iD)
        {
            return GetParcels(h => h.SenderId == iD && h.Delivered == null).Count();
        }

        public int ParcelsInTheWayToCustomer(int iD)
        {
            throw new NotImplementedException();
        }

        public void UpdateStations(Station st)
        {
            throw new NotImplementedException();
        }



        #endregion



    }
}