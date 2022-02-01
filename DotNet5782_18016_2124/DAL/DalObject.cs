//we did the bonus function
using System.Runtime.CompilerServices;
using System;
using System.Collections.Generic;
using System.Text;
using DAL.DalObject;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using DO;
using DalApi;


namespace Dal
{
    internal sealed partial class DalObject : IDal
    {
        #region singelton
        public static readonly Lazy<DalObject> instance = new Lazy<DalObject>(() => new DalObject()); 
        static DalObject() { }// static ctor to ensure instance init is done just before first usage
        
        public static IDal Instance { get => instance.Value; }// The public Instance property to use
        #endregion
        private DalObject() => DataSource.Initialize();


        /// <summary>
        /// A function that return from the list of the drones
        /// </summary>
        /// <returns>return the list of drones </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Drone> GetDrones(Func<Drone, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.drones;
            return DataSource.drones.Where(predicate);
        }
        /// <summary>
        /// A function that return from the list of the customers
        /// </summary>
        /// <returns>return the list of customers</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Customer> Getcustomers(Func<Customer, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.customers;
            return DataSource.customers.Where(predicate);
        }
        /// <summary>
        /// A function that return from the list of the parcels
        /// </summary>
        /// <returns>return the list of parcels</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Parcel> GetParcels(Func<Parcel, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.parcels;
            return DataSource.parcels.Where(predicate);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneCharge> GetDronesInCharge(Func<DroneCharge, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.incharge;
            return DataSource.incharge.Where(predicate);
        }
        /// <summary>
        /// A function that return from the list of the statoins
        /// </summary>
        /// <returns>return the list of stations</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Station> GetStations(Func<Station, bool> predicate = null)
        {
            if (predicate == null)
                return DataSource.stations;
            return DataSource.stations.Where(predicate);
        }
        //public IEnumerable<Station> AvailableChargingStations()
        //{
        //    Station[] baseStations = new Station[DataSource.Config.newBaseStationId];
        //    for (int i = 0; i < DataSource.Config.newBaseStationId; i++)
        //    {
        //        baseStations[i] = DataSource.stations[i];
        //        baseStations[i].ChargeSlots -= DataSource.incharge.Count(dc => dc.StationId == i);
        //    }
        //    return baseStations;
        //}


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
                throw new InVaildIdException($"cannot anchor station{station.ID}to drone");
               
            }
            try
            {
                GetDrone(drone.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot anchor drone{drone.ID}to station");
            }
            if(station.ChargeSlots>0)
                station.ChargeSlots--;
            DroneCharge dCharge = new DroneCharge()
            {
                DroneId = drone.ID,
                StationId = station.ID
            };
            DataSource.incharge.Add(dCharge);
            DataSource.stations.RemoveAll(s => s.ID == station.ID);
            DataSource.stations.Add(station);

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


            DataSource.incharge.RemoveAll(dc => dc.StationId == st.ID && dc.DroneId == drone.ID);
            st.ChargeSlots++;
            UpdateStations(st);

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
            DataSource.customers.RemoveAll(x => x.ID == customer.ID);
            UpdateParcels(parcel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="latOrLot"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public string DecimalToSexagesimal(double coord, char latOrLot)// funciton receives char to decide wheter it is t=latitude and n=lonitude.
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

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double Distance(int ID, double lonP, double latP)
        {
            if (ID > 9999)//if its a customer
                foreach (Customer cus in DataSource.customers) { if (cus.ID == ID) return Haversine(lonP, latP, cus.Longitude, cus.Lattitude); }

            // DataSource.customerList.ForEach(c => { if (int.Parse(c.ID) == ID) { return Haversine(lonP, latP, c.longitude, c.latitude); });//returns in a string the distnace between the customer and given point                   
            else//its a station
                //DataSource.stationsList.ForEach(s => { if (s.ID == ID) { return Haversine(lonP, latP, s.longitude, s.latitude); });//returns in a string the distnace between the station and given point                   
                foreach (Station Kingsx in DataSource.stations) { if (Kingsx.ID == ID) return Haversine(lonP, latP, Kingsx.Longitude, Kingsx.Lattitude); }
            return 0.0;// default return
        }



        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] PowerRequest()
        {
            double[] arr = new double[5];
            arr[0] = DataSource.Config.available;
            arr[1] = DataSource.Config.lightWeight;
            arr[2] = DataSource.Config.mediumWeight;
            arr[3] = DataSource.Config.heavyWeight;
            arr[4] = DataSource.Config.chargingRate;
            return arr;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool LogInVerify(User user)
        {
            DO.User us = DataSource.ListUser.Find(u => u.UserName == user.UserName);
            if (DataSource.ListUser.Exists(x => x.UserName == us.UserName))
            {
                if (us.Password == user.Password)
                {

                    return true;
                }
                else
                    throw new DO.InVaildIdException( $"wrong password:{user.UserName}");
            }
            else
                throw new DO.InVaildIdException( $"bad user id: {user.UserName}");
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool isWorker(User user)
        {
            bool worker;
            DO.User us = DataSource.ListUser.Find(u => u.UserName == user.UserName);
            worker = us.Worker;
            return worker;
        }

    }
}

