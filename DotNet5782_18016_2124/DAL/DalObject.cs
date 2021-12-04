//we did the bonus function
using System;
using System.Collections.Generic;
using System.Text;
using IDAL;
using DAL.DalObject;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using IDAL.DO;


namespace DalObject
{
    public class DalObject
    {
        public DalObject()
        {
            DataSource.Initialize();

        }
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add </param>
        public void AddCustomer(Customer customer)
        {
            DataSource.customers.Add(customer);
        }

        /// <summary>
        ///  A function that recieve a station and add it to the lists of the stations
        /// </summary>
        /// <param name="station">the station we need to add</param>
        public void AddStation(Station station)
        {
            DataSource.stations.Add(station);
        }
        /// <summary>
        ///  A function that recieve a drone and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        public void AddDrone(Drone drone)
        {
            DataSource.drones.Add(drone);

        }
        /// <summary>
        ///  A function that recieve a parcel and add it to the lists of the parcels
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        public void AddParcel(Parcel newParcel)
        {
            DataSource.parcels.Add(newParcel);
        }
        /// <summary>
        /// A function that recieve a droneCharge id and return from the list of the droneCharges the droneCharge with this id 
        /// </summary>
        /// <param name="id">the id of the droneCharge</param>
        /// <returns>return the droneCharge with this id </returns>
        public DroneCharge GetDroneCharge(int id)
        {
            return DataSource.incharge.FirstOrDefault(x => x.DroneId == id);
        }
        /// <summary>
        /// A function that recieve a drones id and return from the list of the drones the drone with this id 
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns>return the drone with this id</returns>
        public Drone GetDrone(int id)
        {
            return DataSource.drones.FirstOrDefault(x => x.ID == id);
        }
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcels the parcel with this id
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        /// <returns>return the drone with this id</returns>
        public Parcel GetParcel(int id)
        {
            return DataSource.parcels.FirstOrDefault(x => x.ID == id);
        }
        /// <summary>
        ///  A function that recieve a stations id and return from the list of the stations the station with this id
        /// </summary>
        /// <param name="id">the id of the station</param>
        /// <returns>return the station with this id</returns>
        public Station GetStation(int id)
        {
            return DataSource.stations.FirstOrDefault(x => x.ID == id);
        }
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="id">the id of the customer</param>
        /// <returns>return the customer with this id</returns>
        public Customer GetCustomer(int id)
        {
            return DataSource.customers.FirstOrDefault(x => x.ID == id);
        }
        /// <summary>
        /// A function that return from the list of the drones
        /// </summary>
        /// <returns>return the list of drones </returns>
        public List<Drone> GetDrones()
        {
            return DataSource.drones;
        }
        /// <summary>
        /// A function that return from the list of the customers
        /// </summary>
        /// <returns>return the list of customers</returns>
        public List<Customer> Getcustomers()
        {
            return DataSource.customers;
        }
        /// <summary>
        /// A function that return from the list of the parcels
        /// </summary>
        /// <returns>return the list of parcels</returns>
        public List<Parcel> GetParcels()
        {
            return DataSource.parcels;
        }
        /// <summary>
        /// A function that return from the list of the statoins
        /// </summary>
        /// <returns>return the list of stations</returns>
        public List<Station> GetStations()
        {
            return DataSource.stations;
        }
        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id in the parcels list
        /// </summary>
        /// <param name="parcel">the parcel with new data to update</param>
        public void UpdateParcel(Parcel parcel)
        {

            Parcel Ptmp = new Parcel();
            int result = DataSource.parcels.FindIndex(x => x.ID == parcel.ID);
            Ptmp.ID = DataSource.parcels[result].ID;
            Ptmp.SenderId = DataSource.parcels[result].ID;
            Ptmp.Longitude = DataSource.parcels[result].Longitude;
            Ptmp.Priority = DataSource.parcels[result].Priority;
            Ptmp.Requested = DataSource.parcels[result].Requested;
            Ptmp.Scheduled = DataSource.parcels[result].Scheduled;
            Ptmp.PickedUp = DataSource.parcels[result].PickedUp;
            Ptmp.TargetId = DataSource.parcels[result].TargetId;
            DataSource.parcels.RemoveAt(result);
            DataSource.parcels.Add(Ptmp);
        }

        /// <summary>
        /// A function that recieve a station and a drone and create a new dronecharge with both id`s and update the station accordingly
        /// </summary>
        /// <param name="station">the station that the drone charge there</param>
        /// <param name="drone">the drone thet need to charge</param>
        public void AnchorDroneStation(Station station, Drone drone)
        {
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
        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="dr">the drone with new data to update</param>
        public void UpdateDrone(Drone dr)
        {

            Drone drone = DataSource.drones.FirstOrDefault(x => x.ID == dr.ID);

            int index = DataSource.drones.FindIndex(x => x.ID == dr.ID);

            DataSource.drones.RemoveAt(index);
            DataSource.drones.Insert(index, drone);
        }
        /// <summary>
        /// A function that recieve a parcel and a drone and and update the parcel to be connected to the drone, update the list of the parcels and drones accordingly
        /// </summary>
        /// <param name="parcel">the parcel thet need to updat</param>
        /// <param name="drone">the drone thet need to updat</param>
        public void BelongingParcel(Parcel parcel, Drone drone)

        {
            parcel.DroneId = drone.ID;
            parcel.Scheduled = DateTime.Today;
            drone.Status = DroneStatuses.shipping;

            //updating drones
            DataSource.drones.RemoveAll(x => x.ID == drone.ID);
            DataSource.drones.Add(drone);

            //updating parcels
            DataSource.parcels.RemoveAll(x => x.ID == parcel.ID);
            DataSource.parcels.Add(parcel);



        }


        /// <summary>
        /// A function that recieve a station and a drone and releas the ststion that charged him, and update the lists of the drones and station accordingly
        /// </summary>
        /// <param name="drone">the drone its need to release</param>
        /// <param name="st">the station that charged this drone</param>
        public void ReleasDrone(Drone drone, Station st)
        {

            DataSource.incharge.RemoveAll(dc => dc.StationId == st.ID && dc.DroneId == drone.ID);
            st.ChargeSlots++;
            DataSource.stations.RemoveAll(item => item.ID == st.ID);
            DataSource.stations.Add(st);

        }
        /// <summary>
        /// function that recieve a parcel and a customer,update the parcel supply to the customer and update  the lists of the customers and parcels accordingly
        /// </summary>
        /// <param name="parcel">the parcel its need to supply</param>
        /// <param name="customer">the customer that recieve the parcel</param>
        public void SupplyParcel(Parcel parcel, Customer customer)
        {

            parcel.Scheduled = DateTime.Today;
            DataSource.customers.RemoveAll(x => x.ID == customer.ID);
            DataSource.parcels.RemoveAll(x => x.ID == parcel.ID);
            DataSource.parcels.Add(parcel);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="coord"></param>
        /// <param name="latOrLot"></param>
        /// <returns></returns>
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
        public double Hav(double radian)
        {
            return Math.Sin(radian / 2) * Math.Sin(radian / 2);
        }

        /// <summary>
        /// computes an angle in radians
        /// </summary>
        /// <param name="degree">a number to transfore to radian</param>
        /// <returns>returns an angle in radians</returns>
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
        public double Haversine(double lon1, double lat1, double lon2, double lat2)
        {
            const int RADIUS = 6371;//earths radius in KM

            double radLon = Radians(lon2 - lon1);//converts differance btween the points to radians
            double radLat = Radians(lat2 - lat1);
            double havd = Hav(radLat) + (Math.Cos(Radians(lat2)) * Math.Cos(Radians(lat1)) * Hav(radLon));//haversine formula determines the spherical distance between the two points using given versine
            double distance = 2 * RADIUS * Math.Asin(havd);
            return distance;
        }
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


    }
}

