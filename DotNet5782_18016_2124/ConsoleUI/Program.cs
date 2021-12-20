////we did the bonus function


//using System;
//using System.Collections.Generic;
//using System.Text;
//using IDAL;
//using Dal;
//using DAL.Dal;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections;
//using IDAL.DO;




//namespace ConsoleUI
//{
//    class Program
//    {


//        enum MenueOptions { Exit, Add, Update, Show_One, Show_List }
//        enum EntityOptions { Exit, Station, Drone, Cusomer, Parcel }
//        enum UpdateOptions { Assignment = 1, Pickedup, Delivery, Recharge, Releas, Exit }
//        enum ListOptions { Exit, Station, Drones, Cusomers, Parcels, UnAssignmentParcel, AvailableChargingStations }

//        static Dal.Dal data = new Dal.Dal();

//        public static void Menue()
//        {

//            EntityOptions entityOption;
//            MenueOptions menueOption;
//            do
//            {
//                Console.WriteLine("Welcome!");
//                Console.WriteLine("option:\n 1-Add,\n 2-Update,\n 3-Show item,\n 4-Show list,\n 0-Exit");
//                menueOption = (MenueOptions)int.Parse(Console.ReadLine());
//                switch (menueOption)
//                {

//                    case MenueOptions.Add:
//                        Console.WriteLine("Adding option:\n 1-Base Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel");
//                        entityOption = (EntityOptions)int.Parse(Console.ReadLine());
//                        switch (entityOption)
//                        {

//                            case EntityOptions.Station:
//                                Station newStation = new Station();
//                                addStation(ref newStation);
//                                data.AddStation(newStation);
//                                break;
//                            case EntityOptions.Drone:
//                                Drone newDrone = new Drone();
//                                addDrone(ref newDrone);
//                                data.AddDrone(newDrone);
//                                break;
//                            case EntityOptions.Cusomer:
//                                Customer newCustomer = new Customer();
//                                addCustomer(ref newCustomer);
//                                data.AddCustomer(newCustomer);
//                                break;
//                            case EntityOptions.Parcel:
//                                Parcel newParcel = new Parcel();
//                                addParcel(ref newParcel);
//                                data.AddParcel(newParcel);
//                                break;
//                            case EntityOptions.Exit:
//                                break;
//                            default:
//                                break;
//                        }
//                        break;
//                    case MenueOptions.Update:
//                        Console.WriteLine("Update option:\n 1-Assignment,\n 2-Pickedup,\n 3-Delivery,\n 4-Recharge,\n 5-ReleasDrone \n 6-Exit");
//                        UpdateOptions updateOptions;
//                        updateOptions = (UpdateOptions)int.Parse(Console.ReadLine());
//                        switch (updateOptions)
//                        {
//                            case UpdateOptions.Exit:
//                                return;
//                            case UpdateOptions.Assignment:
//                                assignment();
//                                break;
//                            case UpdateOptions.Pickedup:
//                                pickedUp();
//                                break;
//                            case UpdateOptions.Delivery:
//                                delivery();
//                                break;
//                            case UpdateOptions.Recharge:
//                                recharge();
//                                break;
//                            case UpdateOptions.Releas:
//                                releas();
//                                break;
//                            default:
//                                break;
//                        }



//                        break;
//                    case MenueOptions.Show_One:
//                        Console.WriteLine("print option:\n 1-Base Station,\n 2-Drone,\n 3-Customer,\n 4-Parcel");
//                        entityOption = (EntityOptions)int.Parse(Console.ReadLine());
//                        int n;
//                        switch (entityOption)
//                        {
//                            case EntityOptions.Exit:
//                                return;

//                            case EntityOptions.Station:
//                                Console.WriteLine("enter id of station\n");
//                                n = int.Parse(Console.ReadLine());
//                                Station resultS = data.GetStation(n);
//                                if (resultS.ID != 0)
//                                {
//                                    Console.WriteLine(resultS);
//                                }

//                                break;
//                            case EntityOptions.Drone:
//                                Console.WriteLine("enter id of drone\n");
//                                n = int.Parse(Console.ReadLine());
//                                Drone result = data.GetDrone(n);
//                                if (result.ID != 0)
//                                {
//                                    Console.WriteLine(result);
//                                }
//                                break;

//                            case EntityOptions.Cusomer:
//                                Console.WriteLine("enter id of customer\n");
//                                n = int.Parse(Console.ReadLine());
//                                Customer resultC = data.GetCustomer(n);
//                                if (resultC.ID != 0)
//                                {
//                                    Console.WriteLine(resultC);
//                                }
//                                break;
//                            case EntityOptions.Parcel:
//                                Console.WriteLine("enter id of parcel\n");
//                                n = int.Parse(Console.ReadLine());
//                                Parcel resultP = data.GetParcel(n);
//                                if (resultP.ID != 0)
//                                {
//                                    Console.WriteLine(resultP);
//                                }
//                                break;
//                            default:
//                                break;
//                        }
//                        break;
//                    case MenueOptions.Show_List:
//                        Console.WriteLine("print of list option:\n 1-Exit,\n 2-Station,\n 3-Drones,\n 4-Cusomers,\n 5-Parcels,\n 6-UnAssignmentParcel,\n 7-AvailableChargingStations");
//                        ListOptions listOptions;
//                        listOptions = (ListOptions)int.Parse(Console.ReadLine());
//                        switch (listOptions)
//                        {
//                            case ListOptions.Exit:
//                                return;

//                            case ListOptions.Station:
//                                foreach (Station s in data.GetStations())
//                                {
//                                    Console.WriteLine($"{s} ");
//                                }
//                                break;
//                            case ListOptions.Drones:
//                                foreach (Drone d in data.GetDrones())
//                                {
//                                    Console.WriteLine($"{d} ");
//                                }
//                                break;
//                            case ListOptions.Cusomers:
//                                foreach (Customer c in data.Getcustomers())
//                                {
//                                    Console.WriteLine($"{c} ");
//                                }
//                                break;
//                            case ListOptions.Parcels:
//                                foreach (Parcel p in data.GetParcels())
//                                {
//                                    Console.WriteLine($"{p} ");
//                                }
//                                break;
//                            case ListOptions.AvailableChargingStations:
//                                foreach (Station s in data.GetStations())
//                                {
//                                    if (s.ChargeSlots > 0)
//                                        Console.WriteLine($"{s.ID} ");
//                                }
//                                break;
//                            case ListOptions.UnAssignmentParcel:
//                                foreach (Parcel p in data.GetParcels())
//                                {
//                                    if (p.DroneId == 0)
//                                        Console.WriteLine($"{p.ID} ");
//                                }
//                                break;
//                            default:
//                                break;
//                        }
//                        break;
//                    case MenueOptions.Exit:
//                        return;

//                    default:
//                        break;
//                }
//            }
//            while (menueOption != 0);


//        }
//        static void Main(string[] args)
//        {
//            Menue();
//        }
//        /// <summary>
//        /// a function that pick up id of a parcel and a customer and supply the parcel to the customer
//        /// </summary>
//        private static void delivery()
//        {
//            foreach (var item in data.GetParcels())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("enter parcel id\n");
//            int np = int.Parse(Console.ReadLine());
//            foreach (var item in data.Getcustomers())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("enter customer id\n");
//            int nc = int.Parse(Console.ReadLine());
//            Parcel parcel = data.GetParcel(np);
//            Customer customer = data.GetCustomer(nc);
//            data.SupplyParcel(parcel, customer);

//        }
//        /// <summary>
//        /// a function that pick up data of a drone from the user and releas the station that charged the drone
//        /// </summary>
//        private static void releas()
//        {
//            foreach (var item in data.GetDrones())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("enter drone id\n");
//            int nd = int.Parse(Console.ReadLine());
//            Drone drone = data.GetDrone(nd);

//            DroneCharge dg = data.GetDroneCharge(nd);
//            Station station = data.GetStation(dg.StationId);
//            data.ReleasDrone(drone, station);
//        }
//        /// <summary>
//        /// a function that pick up data of a drone and a parcel from the user and belon the drone to this parcel
//        /// </summary>
//        private static void assignment()
//        {
//            foreach (var item in data.GetParcels())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("enter parcel id\n");
//            int np = int.Parse(Console.ReadLine());
//            foreach (var item in data.GetDrones())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("enter drone id\n");
//            int nd = int.Parse(Console.ReadLine());
//            Parcel parcel = data.GetParcel(np);
//            Drone drone = data.GetDrone(nd);
//            data.BelongingParcel(parcel, drone);
//        }
//        /// <summary>
//        /// a function that pick up data of a parcel from the user and update that the customer got the parcel
//        /// </summary>
//        private static void pickedUp()
//        {
//            foreach (var item in data.GetParcels())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("Enter Id of parcel:");

//            int idp = Int32.Parse(Console.ReadLine());
//            Parcel p = data.GetParcel(idp);
//            p.PickedUp = DateTime.Today;
//            data.UpdateParcel(p);
//        }
//        /// <summary>
//        /// a function that pick up data of a drone from the user, update the drone to be charged
//        /// </summary>
//        private static void recharge()
//        {
//            foreach (var item in data.GetDrones())
//            {
//                Console.WriteLine(item);
//            }
//            Console.WriteLine("Enter Id of drone:");

//            int id = Int32.Parse(Console.ReadLine());

//            Drone dr = data.GetDrone(id);
//            dr.Status = DroneStatuses.free;
//            dr.Battery = 100;
//            data.UpdateDrone(dr);
//            //update station
//            Console.WriteLine("Enter Id of station:");
//            id = Int32.Parse(Console.ReadLine());
//            Station station = data.GetStation(id);
//            data.AnchorDroneStation(station, dr);
//        }
//        /// <summary>
//        /// A function that picks up data from the user for a parcel, places the data in the parcel and sends it to a function that adds the parcel to the list of parcels
//        /// </summary>
//        /// <param name="newParcel">the parcel that the function sends to another function to add to the parcels list</param>
//        private static void addParcel(ref Parcel newParcel)
//        {

//            Console.WriteLine("Enter ID number:");
//            newParcel.ID = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter sending cusomer id:");
//            newParcel.SenderId = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter target customer id \n");
//            newParcel.TargetId = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter weight category: 0- low, 1- middle,2- heavy\n");
//            newParcel.Longitude = (WeightCategories)int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter priority: 0- regular, 1- fast,2- emergency\n");
//            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter drone id:\n");
//            newParcel.DroneId = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter delivery time:\n");
//            newParcel.Requested = DateTime.Parse(Console.ReadLine());
//            Console.WriteLine("Enter time to assign the parcel to the skimmer:\n");
//            newParcel.Scheduled = DateTime.Parse(Console.ReadLine());
//            Console.WriteLine("Enter time to collecte the parcel from the sender:\n");
//            newParcel.PickedUp = DateTime.Parse(Console.ReadLine());
//            Console.WriteLine("Enter time to the package to arrive to the recipient:\n");
//            newParcel.Delivered = DateTime.Parse(Console.ReadLine());

//        }
//        /// <summary>
//        /// A function that picks up data from the user for a station, places the data in the station and sends it to a function that adds the station to the list of stations
//        /// </summary>
//        /// <param name="newStation">the station that the function sends to another function to add to the stations list</param>
//        private static void addStation(ref Station newStation)
//        {
//            Console.WriteLine("Enter ID:");
//            newStation.ID = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter station`s name:");
//            newStation.Name = Console.ReadLine();
//            Console.WriteLine("Enter chargeSlots number:");
//            newStation.ChargeSlots = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter longitude:");
//            newStation.Longitude = double.Parse(Console.ReadLine());
//            Console.WriteLine("Enter latitude:");
//            newStation.Lattitude = double.Parse(Console.ReadLine());
//        }
//        /// <summary>
//        /// A function that picks up data from the user for a customer, places the data in the customer and sends it to a function that adds the customer to the list of customers
//        /// </summary>
//        /// <param name="newCustomer">the customer that the function sends to another function to add to the customers list</param>
//        private static void addCustomer(ref Customer newCustomer)
//        {

//            Console.WriteLine("Enter ID:");
//            newCustomer.ID = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter customr`s name:");
//            newCustomer.Name = Console.ReadLine();
//            Console.WriteLine("Enter phone number:");
//            newCustomer.Phone = Console.ReadLine();
//            Console.WriteLine("Enter longitude:");
//            newCustomer.Longitude = double.Parse(Console.ReadLine());
//            Console.WriteLine("Enter latitude:");
//            newCustomer.Lattitude = double.Parse(Console.ReadLine());
//        }
//        /// <summary>
//        /// A function that picks up data from the user for a skimmer, places the data in the skimmer and sends it to a function that adds the skimmer to the list of skimmers
//        /// </summary>
//        /// <param name="newDrone">the skimmer that the function sends to another function to add to the skimmers list</param>
//        private static void addDrone(ref Drone newDrone)
//        {

//            Console.WriteLine("Enter ID number:");
//            newDrone.ID = int.Parse(Console.ReadLine());
//            Console.WriteLine("Enter drone model:");
//            newDrone.Model = Console.ReadLine();
//            Console.WriteLine("Enter weight category: 0- low, 1- middle,2- heavy\n");
//            newDrone.MaxWeight = (WeightCategories)int.Parse(Console.ReadLine());
//            newDrone.Battery = 100;
//            newDrone.Status = DroneStatuses.free;

//        }
//    }

//}

