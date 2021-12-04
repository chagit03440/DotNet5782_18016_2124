using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using IDAL.DO;
namespace DAL
{
    namespace DalObject
    {
        internal class DataSource
        {
            internal class Config
            {
                internal static int newDroneID = 0;
                internal static int newStationID = 0;
                internal static int newCustomerID = 0;
                internal static int newParcelID = 0;
            }
            internal static List<Drone> drones = new List<Drone>();
            internal static List<Station> stations = new List<Station>();
            internal static List<Customer> customers = new List<Customer>();
            internal static List<Parcel> parcels = new List<Parcel>();
            internal static List<DroneCharge> incharge = new List<DroneCharge>();

            static Random rand = new Random();

            internal static void Initialize()
            {
                creatDrone(2);
                creatStation(5);
                creatCustomer(10);
                creatParcel(10);
            }
            /// <summary>
            /// A function that initialize customers with random data
            /// </summary>
            /// <param name="n">a number of customers to intilize</param>
            private static void creatCustomer(int n)
            {
                for (int i = 0; i < n; i++)
                {
                    Customer newCustomer = new Customer();

                    newCustomer.ID = rand.Next(100000000, 999999999);
                    newCustomer.Name = $"Customer {Config.newCustomerID}";
                    newCustomer.Phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}";
                    newCustomer.Lattitude = getRandomCoordinate(31.7);
                    newCustomer.Longitude = getRandomCoordinate(35.1);
                    DataSource.customers.Add(newCustomer);
                    Config.newCustomerID++;

                }
            }
            /// <summary>
            /// A function that initialize parcels with random data
            /// </summary>
            /// <param name="n">a number of parcels to intilize</param>
            private static void creatParcel(int n)
            {
                for (int i = 0; i < n; i++)
                {
                    Parcel newParcel = new Parcel();
                    newParcel.ID = Config.newParcelID;
                    newParcel.SenderId = rand.Next(Config.newParcelID);
                    newParcel.TargetId = rand.Next(Config.newParcelID);
                    newParcel.Longitude = (WeightCategories)rand.Next(3);
                    newParcel.DroneId = rand.Next(100000000, 999999999);
                    newParcel.Priority = (Priorities)rand.Next(1, 3);
                    newParcel.Requested = DateTime.Now;
                    newParcel.Scheduled = DateTime.Now;
                    newParcel.PickedUp = DateTime.Now;
                    newParcel.Delivered = DateTime.Now;
                    parcels.Add(newParcel);
                    Config.newParcelID++;
                }
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
                    newStation.ID = rand.Next(100000000, 999999999);
                    newStation.Name = $"Station{Config.newStationID}";
                    newStation.ChargeSlots = 10 + i;
                    newStation.Lattitude = 31.785664 + i;
                    newStation.Longitude = 35.189938 + i;
                    stations.Add(newStation);
                    Config.newStationID++;
                }
            }
            /// <summary>
            /// computes randomcoordinate
            /// </summary>
            /// <param name="n">the number of the beginig</param>
            /// <returns> return a randomcoordinate</returns>
            private static double getRandomCoordinate(double n)
            {
                return n + rand.NextDouble() / 10;
            }
            /// <summary>
            /// A function that initialize drones with random data
            /// </summary>
            /// <param name="n">a number of drones to intilize</param>
            private static void creatDrone(int n)
            {
                for (int i = 0; i < n; i++)
                {
                    Drone newDrone = new Drone();
                    newDrone.ID = Config.newDroneID;
                    newDrone.Battery = 1;
                    newDrone.MaxWeight = (WeightCategories)rand.Next(3);
                    newDrone.Model = "iFly" + i;
                    newDrone.Status = DroneStatuses.maintenance;
                    drones.Add(newDrone);
                    Config.newDroneID++;
                }
            }
        }
    }
}
