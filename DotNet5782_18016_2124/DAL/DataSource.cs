using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DO;
namespace DAL
{
    namespace DalObject
    {
        internal class DataSource
        {
            internal class Config
            {

                internal static double available = 0;
                internal static double lightWeight = 10;
                internal static double mediumWeight = 50;
                internal static double heavyWeight = 150;
                internal static double chargingRate = 10;
                internal static int newDroneId = 1;
                internal static int newBaseStationId = 1;
                internal static int newCustomerId = 1;
                internal static int newParcelId = 1000;


            }
            internal static List<Drone> drones = new List<Drone>();
            internal static List<Station> stations = new List<Station>();
            internal static List<Customer> customers = new List<Customer>();
            internal static List<Parcel> parcels = new List<Parcel>();
            internal static List<DroneCharge> incharge = new List<DroneCharge>();
            internal static List<User> ListUser = new List<User>();

            static Random rand = new Random();
            /// <summary>
            /// a function 
            /// </summary>
            internal static void Initialize()
            {
                creatDrone(10);
                creatStation(10);
                creatCustomer(10);
                creatParcel(10);
                createUsers();
               
            }
            /// <summary>
            /// A function that initialize customers with random data
            /// </summary>
            /// <param name="n">a number of customers to intilize</param>
            private static void creatCustomer(int n)
            {
                for (int i =0; i <n; i++)
                {
                    Customer newCustomer = new Customer();

                    newCustomer.ID = i+1000;
                    newCustomer.Name = $"Customer {i}";
                    newCustomer.Phone = $"0{rand.Next(50, 58)}-{rand.Next(1000000, 10000000)}";
                    newCustomer.Lattitude = getRandomCoordinate(31.7);
                    newCustomer.Longitude = getRandomCoordinate(35.1);
                    DataSource.customers.Add(newCustomer);


                }
            }


            private static void createUsers()
            {
                ListUser = new List<User>
            {
                 new User
                 {
                      UserName="Chagit",
                     Password="3440",
                     Worker=true,
                     IsActive = true,
                 },

                 new User
                 {
                     UserName="Sarah",
                     Password="bbb",
                     Worker=true,
                     IsActive = true,
                 },

                 new User
                 {
                     UserName="Yonhatan",
                     Password="ccc",
                     Worker=false,
                     IsActive = true,
                 },

                 new User
                 {
                     UserName="Noa",
                     Password="ddd",
                     Worker=false,
                     IsActive = true,
                 },

                 new User
                 {
                     UserName="Daniel",
                     Password="eee",
                     Worker=false,
                     IsActive = true,
                 },
                  new User
                 {
                     UserName="Avital",
                     Password="aaa",
                     Worker=true,
                     IsActive = true,
                 },

            };
                for (int i = 0; i < 10; i++)
                {
                    User newCustomer = new User();

                    newCustomer.Password = "a"+i;
                    newCustomer.UserName = $"Customer {i}";
                    newCustomer.Worker = false;
                    ListUser.Add(newCustomer);


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
                    newParcel.ID = Config.newParcelId;
                    Config.newParcelId++;
                    newParcel.SenderId = rand.Next(i+1000,i+1000+n);
                    newParcel.TargetId = rand.Next(i + 1000, i + 1000 + n);
                    newParcel.Longitude = (WeightCategories)rand.Next(3);
                    newParcel.DroneId = 0;
                    newParcel.Priority = (Priorities)rand.Next(1, 3);
                    newParcel.Requested = null;
                    newParcel.Scheduled = null;
                    newParcel.PickedUp = null;
                    newParcel.Delivered = null;
                    parcels.Add(newParcel);

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
                    newStation.ID = i+1000;
                    newStation.Name = $"Station{i}";
                    newStation.ChargeSlots = rand.Next(1, 1000); ;
                    newStation.Lattitude = 31.785664 + i;
                    newStation.Longitude = 35.189938 + i;
                    stations.Add(newStation);

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
                WeightCategories[] values = WeightCategories.GetValues<WeightCategories>();

                for (int i = 0; i < n; i++)
                {
                    Drone newDrone = new Drone();
                    newDrone.ID = i+1000;
                    //newDrone.Battery = 1;
                    newDrone.MaxWeight = values[rand.Next(values.Length)];
                    newDrone.Model = "iFly" + i;
                  //  newDrone.Status = DroneStatuses.maintenance;
                    drones.Add(newDrone);

                }
            }
        }
    }
}
