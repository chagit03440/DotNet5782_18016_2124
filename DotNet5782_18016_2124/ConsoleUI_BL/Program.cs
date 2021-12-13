using System;
using IBL.BO;

namespace ConsoleUI_BL
{
    class Program
    {
        enum MenueOptions { Exit, Add, Update, Show_One, Show_List }
        enum EntityOptions { Exit, Station, Drone, Customer, Parcel }
        enum UpdateOptions { Exit, Assignment, Pickedup, Delivery, Recharge, Release, Drone, Station, Customer }
        enum ListOptions { Exit, Stations, Drones, Customers, Parcels, UnAssignmentParcel, AvailableChargingStations }

        static BL.BLObject data;
        static void Main(string[] args)
        {
            data = new BL.BLObject();
            ShowMenu();
        }
        private static void ShowMenu()
        {
            MenueOptions menuOption;
            try
            {
                do
                {
                    Console.WriteLine("Welcome!");
                    Console.WriteLine("option:\n 1-Add,\n 2-Update,\n 3-Show item,\n 4-Show list,\n 0-Exit");
                    menuOption = (MenueOptions)int.Parse(Console.ReadLine());
                    switch (menuOption)
                    {
                        case MenueOptions.Add:
                            MenuAddOptions();
                            break;
                        case MenueOptions.Update:
                            MenuUpdateOptions();
                            break;
                        case MenueOptions.Show_One:
                            MenuShowOneOptions();
                            break;
                        case MenueOptions.Show_List:
                            MenuShowListOptions();
                            break;
                        case MenueOptions.Exit:
                            break;
                    }
                } while (menuOption != MenueOptions.Exit);
            }
           
            catch (DO.InVaildIdException ex)
            {

                Console.WriteLine(ex);
            }
            catch (DO.AlreadyExistExeption ex)
            {

                Console.WriteLine(ex);
            }
            catch (BLInVaildIdException ex)
            {

                Console.WriteLine(ex);
            }
            catch (BLAlreadyExistExeption ex)
            {

                Console.WriteLine(ex);
            }


        }

        private static void MenuShowListOptions()
        {
            Console.WriteLine(
                "List option:\n 1-Base Stations,\n 2-Drones,\n 3-Customers,\n 4-Parcels,\n 5-UnAssignment Parcels,\n 6-Available Charging Stations,\n 0-Exit");
            ListOptions listOptions = (ListOptions)int.Parse(Console.ReadLine());
            switch (listOptions)
            {
                case ListOptions.Stations:
                    foreach (var item in data.GetStations())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Drones:
                    foreach (var item in data.GetDrones())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Customers:
                    foreach (var item in data.GetCustomers())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Parcels:
                    foreach (var item in data.GetParcels())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.UnAssignmentParcel:
                    foreach (var item in data.UnAssignmentParcels())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.AvailableChargingStations:
                    foreach (var item in data.AvailableChargingStations())
                    {
                        Console.WriteLine(item);
                    }

                    break;
                case ListOptions.Exit:
                    break;
            }
        }

        private static void MenuShowOneOptions()
        {
            EntityOptions entityOption;
            Console.WriteLine("View item option:\n 1-Base Station,\n 2-DroneP,\n 3-Customer,\n 4-Parcel\n, 0-Exit");
            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
            Console.WriteLine($"Enter a requested {entityOption} id");
            int requestedId;
            int.TryParse(Console.ReadLine(), out requestedId);
            switch (entityOption)
            {
                case EntityOptions.Station:
                    Station baseStation = data.GetStation(requestedId);
                    Console.WriteLine(baseStation);
                    //ShowList(baseStation.DronesInCharging);
                    break;
                case EntityOptions.Drone:
                    Console.WriteLine(data.GetDrone(requestedId));
                    break;
                case EntityOptions.Customer:
                    Customer customer = data.GetCustomer(requestedId);
                    Console.WriteLine(customer);
                    break;
                case EntityOptions.Parcel:
                    Console.WriteLine(data.GetParcel(requestedId));
                    break;
                case EntityOptions.Exit:
                    break;
            }
        }

        private static void MenuUpdateOptions()
        {
            Console.WriteLine("Update option:\n 1-Assignment,\n 2-Pickedup,\n 3-Delivery,\n 4-Recharge,\n 5-Release,\n6-Drone,\n7-Station,8-Customer,\n 0-Exit");
            UpdateOptions updateOptions;
            updateOptions = (UpdateOptions)int.Parse(Console.ReadLine());
            int parcelId;
            int droneId;
            switch (updateOptions)
            {
                case UpdateOptions.Assignment:
                    assignment(out parcelId, out droneId);
                    break;
                case UpdateOptions.Pickedup:
                    pickedup();
                    break;
                case UpdateOptions.Delivery:
                    delivery();
                    break;
                case UpdateOptions.Recharge:
                    recharge();
                    break;
                case UpdateOptions.Release:
                    release();
                    break;
                case UpdateOptions.Drone:
                    updateDrone();
                    break;
                case UpdateOptions.Station:
                    updateStation();
                    break;
                case UpdateOptions.Customer:
                    updateCustomer();
                    break;
                case UpdateOptions.Exit:
                    break;
            }
        }
        private static void MenuAddOptions()
        {
            EntityOptions entityOption;
            Console.WriteLine("Adding option:\n 1-Base Station,\n 2-DroneP,\n 3-Customer,\n 4-Parcel");
            entityOption = (EntityOptions)int.Parse(Console.ReadLine());
            switch (entityOption)
            {
                case EntityOptions.Station:
                    addStation();
                    break;
                case EntityOptions.Drone:

                    addDrone();
                    break;
                case EntityOptions.Customer:
                    addCustomer();
                    break;
                case EntityOptions.Parcel:
                    addParcel();

                    break;

                case EntityOptions.Exit:
                    break;
            }
            return;
        }
        private static void delivery()
        {
            int droneId;
            Console.WriteLine("Enter ID for drone:");
            droneId = int.Parse(Console.ReadLine());
            data.PackageDeliveryByDrone(droneId);

        }

        private static void updateCustomer()
        {
            Customer cus = new Customer();
            Console.WriteLine("Enter ID for customer");
            cus.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter name for customer :");
            cus.Name = Console.ReadLine();
            Console.WriteLine("Enter phone customer");
            cus.Phone = Console.ReadLine();
            data.UpdateCustomer(cus);
        }

        private static void updateStation()
        {
            Station st = new Station();
            Console.WriteLine("Enter ID for station");
            st.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter name for station :");
            st.Name = Console.ReadLine();
            Console.WriteLine("Enter number of charging slots:");
            int chargeSlots;
            bool flag = int.TryParse(Console.ReadLine(), out chargeSlots);
            if (!flag)
                chargeSlots = -1;
            data.UpdateStation(st, chargeSlots);
        }

        private static void updateDrone()
        {
            Drone dr = new Drone();
            Console.WriteLine("Enter ID for drone");
            dr.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter model for drone :");
            dr.Model = Console.ReadLine();
            data.UpdateDrone(dr);
        }

        private static void release()
        {
            int droneId;
            Console.WriteLine("Enter ID for drone:");
            droneId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Charging time :");
            double time = double.Parse(Console.ReadLine());
            data.ReleaseDroneFromRecharge(droneId, time);

        }

        private static void recharge()
        {
            int droneId;
            Console.WriteLine("Enter IDs for drone :");
            droneId = int.Parse(Console.ReadLine());
            data.SendDroneToRecharge(droneId);
        }

        private static void pickedup()
        {
            int parcelId;
            Console.WriteLine("Enter parcel Id:");
            parcelId = int.Parse(Console.ReadLine());
            data.PickedupParcel(parcelId);
        }

        private static void assignment(out int parcelId, out int droneId)
        {
            Console.WriteLine("Enter IDs for parcel and drone:");
            parcelId = int.Parse(Console.ReadLine());
            droneId = int.Parse(Console.ReadLine());
            data.AssignmentParcelToDrone(droneId);
        }



        private static void addCustomer()
        {
            Console.WriteLine("Enter Id and Name, phone :");
            int id;
            string name;
            string phone;
            int.TryParse(Console.ReadLine(), out id);
            name = Console.ReadLine();
            phone = Console.ReadLine();
            Console.WriteLine("Enter location:");
            double Lattitude1, Longitude1;
            double.TryParse(Console.ReadLine(), out Lattitude1);
            double.TryParse(Console.ReadLine(), out Longitude1);
            Customer customer = new Customer()
            {
                Id = id,
                Name = name,
                Phone = phone,
                Location = new Location() { Lattitude = Lattitude1, Longitude = Longitude1 }
            };
            try
            {
                data.AddCustomer(customer);
            }
            catch (Exception exp)
            {

                Console.WriteLine(exp.Message);
            }
        }

        private static void addDrone()
        {
            int id;
            string model;
            WeightCategories weight;
            Console.WriteLine("Enter Id and Model :");


            int.TryParse(Console.ReadLine(), out id);
            model = Console.ReadLine();
            Console.WriteLine("Enter weight category: 0- low, 1- middle,2- heavy\n");
            weight = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter station id for first charging");
            int statinId;
            int.TryParse(Console.ReadLine(), out statinId);
            DroneForList drone = new DroneForList { Id = id, Model = model, MaxWeight = weight };
            data.AddDrone(drone, statinId);


        }

        private static void addStation()
        {
            Console.WriteLine("Enter Name and Number of charging positions:");
            string stationName = Console.ReadLine();
            int chargeSlots;
            int id1;
            int.TryParse(Console.ReadLine(), out chargeSlots);
            int.TryParse(Console.ReadLine(), out id1);
            Console.WriteLine("Enter location:");
            double Lattitude1, Longitude1;
            double.TryParse(Console.ReadLine(), out Lattitude1);
            double.TryParse(Console.ReadLine(), out Longitude1);
            Station baseStation = new Station()
            {
                Id = id1,
                Name = stationName,
                Location = new Location() { Lattitude = Lattitude1, Longitude = Longitude1 },
                ChargeSlots = chargeSlots,
                Drones = null

            };
            try
            {
                data.AddStation(baseStation);
            }

            catch (Exception ex)
            {

                Console.WriteLine("adding base station exception: ", ex.Message); ;
            }
        }

        private static void addParcel()
        {
            Parcel newParcel = new Parcel();
            Console.WriteLine("Enter ID number:");
            newParcel.Id = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter sending cusomer id:");
            int Senderid = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter target customer id \n");
            int Targetid = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter weight category: 0- low, 1- middle,2- heavy\n");
            newParcel.Longitude = (WeightCategories)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter priority: 0- regular, 1- fast,2- emergency\n");
            newParcel.Priority = (Priorities)int.Parse(Console.ReadLine());
            Console.WriteLine("Enter drone id:\n");


            try
            {

                data.AddParcel(newParcel);
            }
            catch (Exception exp)
            {

                Console.WriteLine(exp.Message);
            }

        }

    }
}


