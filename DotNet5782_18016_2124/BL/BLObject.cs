﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
namespace BL
{

    public partial class BLObject : IBL.IBL
    {

        private IDAL.DO.IDal myDal;
        private List<DroneForList> drones;
        private static Random rand = new Random();
        /// <summary>
        /// constructor
        /// </summary>
        public BLObject()
        {
            myDal = DalFactory.GetDal("DalObject");
            drones = new List<DroneForList>();
            try
            {
                initializeDrones();
            }
            catch (BLStationException ex)
            {

                Console.WriteLine(ex);
            }
            catch (BLCustomerException ex)
            {

                Console.WriteLine(ex);
            }
            catch (BLDroneException ex)
            {

                Console.WriteLine(ex);
            }
            catch (BLParcelException ex)
            {

                Console.WriteLine(ex);
            }
            catch (IDAL.DO.CustomerException ex)
            {

                Console.WriteLine(ex);
            }
            catch (IDAL.DO.StationException ex)
            {

                Console.WriteLine(ex);
            }
            catch (IDAL.DO.DroneChargeException ex)
            {

                Console.WriteLine(ex);
            }
            catch (IDAL.DO.DroneException ex)
            {

                Console.WriteLine(ex);
            }
            catch (IDAL.DO.ParcelException ex)
            {

                Console.WriteLine(ex);
            }


        }
        /// <summary>
        /// a function that intialize the list of drones with the information from the datasource
        /// </summary>
        private void initializeDrones()
        {
            foreach (var drone in myDal.GetDrones())
            {
                drones.Add(new DroneForList
                {
                    Id = drone.ID,
                    Model = drone.Model,
                    MaxWeight = (WeightCategories)drone.MaxWeight
                });
            }

            foreach (var drone in drones)
            {
                if (isDroneWhileShipping(drone))
                {
                    drone.Status = DroneStatuses.Shipping;
                    drone.Location = findDroneLocation(drone);
                    // note : drone.DeliveryId getting value inside isDroneWhileShipping
                    int minBattery = calcMinBatteryRequired(drone);
                    drone.Battery = (double)rand.Next(minBattery, 100) / 100;
                }
                else
                {
                    drone.Status = (DroneStatuses)rand.Next(1, 2);
                    if (drone.Status == DroneStatuses.Maintenance)
                    {
                        IDAL.DO.Station station = default;
                        IDAL.DO.Drone drone1 = default;
                        int stationId = rand.Next(1, myDal.GetStations().Count());
                        try
                        {

                            station = myDal.GetStation(stationId);
                        }
                        catch (IDAL.DO.StationException ex)
                        {

                            throw new BLStationException("the station doesn't exist", ex);
                        }
                        try
                        {
                            drone1 = myDal.GetDrone(drone.Id);

                        }
                        catch (IDAL.DO.DroneException ex)
                        {

                            throw new BLDroneException("the drone doesn't exist", ex);
                        }

                        myDal.AnchorDroneStation(station, drone1);
                        drone.Location = getBaseStationLocation(stationId);
                        drone.Battery = (double)rand.Next(0, 20) / 100;
                        drone.ParcelId = 0;
                    }

                    if (drone.Status == DroneStatuses.Free)
                    {
                        drone.Location = findDroneLocation(drone);
                        drone.ParcelId = 0;
                        int minBattery = calcMinBatteryRequired(drone);
                        drone.Battery = (double)rand.Next(minBattery, 100) / 100;
                    }
                }
            }
        }
        //private void initializeDrones()
        //{
        //    bool flag = false;
        //    Random rnd = new Random();
        //    double minBatery = 0;

        //    IEnumerable<IDAL.DO.Drone> d = myDal.GetDrones();
        //    IEnumerable<IDAL.DO.Parcel> p = myDal.GetParcels();
        //    foreach (var item in d)
        //    {
        //        IBL.BO.DroneForList drt = new DroneForList();
        //        drt.Id = item.ID;
        //        drt.Model = item.Model;
        //        foreach (var pr in p)
        //        {
        //            if (pr.DroneId == item.ID && pr.Delivered == DateTime.MinValue)
        //            {
        //                IDAL.DO.Customer sender = myDal.GetCustomer(pr.SenderId);
        //                IDAL.DO.Customer target = myDal.GetCustomer(pr.TargetId);
        //                IBL.BO.Location senderLocation = new Location { Lattitude = sender.Lattitude, Longitude = sender.Longitude };
        //                IBL.BO.Location targetLocation = new Location { Lattitude = target.Lattitude, Longitude = target.Longitude };
        //                drt.Status = DroneStatuses.Shipping;
        //                if (pr.PickedUp == null && pr.Scheduled != null)//החבילה שויכה אבל עדיין לא נאספה
        //                {
        //                    drt.Location = new Location { Lattitude = findClosetBaseStationLocation(senderLocation).Lattitude, Longitude = findClosetBaseStationLocation(senderLocation).Longitude };
        //                    minBatery = calcDistance(drt.Location, senderLocation) * myDal.PowerRequest()[0];
        //                    minBatery += calcDistance(senderLocation, targetLocation) * myDal.PowerRequest()[3]/*/*//*indexOfChargeCapacity(pr.Longitude)*//*/*/;
        //                    minBatery += calcDistance(targetLocation, new Location { Lattitude = findClosetBaseStationLocation(targetLocation).Lattitude, Longitude = findClosetBaseStationLocation(targetLocation).Longitude }) * myDal.PowerRequest()[0];
        //                }
        //                if (pr.PickedUp != null && pr.Delivered == null)//החבילה נאספה אבל עדיין לא הגיעה ליעד
        //                {
        //                    drt.Location = new Location();
        //                    drt.Location = senderLocation;
        //                    minBatery = calcDistance(targetLocation, new Location { Lattitude = findClosetBaseStationLocation(targetLocation).Lattitude, Longitude = findClosetBaseStationLocation(targetLocation).Longitude }) * myDal.PowerRequest()[0];
        //                    minBatery += calcDistance(drt.Location, targetLocation) * myDal.PowerRequest()[2];//indexOfChargeCapacity(pr.Longitude);
        //                }
        //                drt.Battery = rnd.Next((int)minBatery, 101);
        //                flag = true;
        //                break;
        //            }
        //        }
        //        if (!flag)
        //        {
        //            int temp = rnd.Next(1, 3);
        //            if (temp == 1)
        //                drt.Status = IBL.BO.DroneStatuses.Free;
        //            else
        //                drt.Status = IBL.BO.DroneStatuses.Maintenance;
        //            if (drt.Status == IBL.BO.DroneStatuses.Maintenance)
        //            {
        //                int l = rnd.Next(0, myDal.GetStations().Count()), i = 0;
        //                IDAL.DO.Station s = new IDAL.DO.Station();
        //                foreach (var ite in myDal.GetStations())
        //                {
        //                    s = ite;
        //                    if (i == l)
        //                        break;
        //                    i++;
        //                }
        //                Station station = new Station()
        //                {
        //                    Id = s.ID,
        //                    ChargeSlots = s.ChargeSlots,
        //                    Name = s.Name,
        //                    Location = new Location() { Lattitude = s.Lattitude, Longitude = s.Longitude }
        //                };
        //                drt.Location = new Location { Lattitude = s.Lattitude, Longitude = s.Longitude };
        //                drt.Battery = rnd.Next(0, 21);

        //                AnchorDroneStation(station, drt);
        //            }
        //            else
        //            {
        //                List<IDAL.DO.Customer> lst = new List<IDAL.DO.Customer>();
        //                foreach (var pr in p)
        //                {
        //                    if (pr.Delivered != null)
        //                        lst.Add(myDal.GetCustomer(pr.TargetId));
        //                }

        //                int l = rnd.Next(0, lst.Count());
        //                drt.Location = new Location { Lattitude = lst[l].Lattitude, Longitude = lst[l].Longitude };
        //                minBatery += calcDistance(drt.Location, new Location { Longitude = findClosetBaseStationLocation(drt.Location).Longitude, Lattitude = findClosetBaseStationLocation(drt.Location).Lattitude }) * myDal.PowerRequest()[0];
        //                drt.Battery = rnd.Next((int)minBatery, 101);
        //            }
        //        }
        //        drones.Add(drt);
        //    }
        //}




        /// <summary>
        /// A function that return  the list of the drones
        /// </summary>
        /// <returns>return list of drone</returns>
        public IEnumerable<DroneForList> GetDrones(Func<DroneForList, bool> predicate = null)
        {
            if (predicate == null)
                return drones;
            return drones.Where(predicate);

        }
        /// <summary>
        /// A function that return  the list of the stations
        /// </summary>
        /// <returns>return list of station</returns>
        public IEnumerable<StationForList> GetStations(Func<StationForList, bool> predicate = null)
        {
            IEnumerable<StationForList> l;


            l = myDal.GetStations()
                .Select(baseStation =>
                    new StationForList
                    {
                        Id = baseStation.ID,
                        Name = baseStation.Name,
                        NotAvailableChargeSlots = getUsedChargingPorts(baseStation.ID),
                        AvailableChargeSlots = myDal.AvailableChargingPorts(baseStation.ID)
                    }
                );
            if (predicate == null)
                return l;
            return l.Where(predicate);
        }




        /// <summary>
        /// A function that return  the list of the customers
        /// </summary>
        /// <returns>return list of the customer</returns>
        public IEnumerable<CustomerForList> GetCustomers(Func<CustomerForList, bool> predicate = null)
        {
            IEnumerable<CustomerForList> l;

            l = myDal.Getcustomers()
                .Select(customer =>
                    new CustomerForList
                    {
                        Id = customer.ID,
                        Name = customer.Name,
                        Phone = customer.Phone,
                        ParcelsHeGot = myDal.ParcelsCustomerGot(customer.ID),
                        ParcelsHeSendAndDelivered = myDal.ParcelsCustomerSendAndDelivered(customer.ID),
                        ParcelsHeSendAndNotDelivered = myDal.ParcelsCustomerSendAndNotDelivered(customer.ID),
                        ParcelsInTheWayToCustomer = myDal.ParcelsInTheWayToCustomer(customer.ID),

                    }
                );
            if (predicate == null)
                return l;
            return l.Where(predicate);
        }
        /// <summary>
        /// A function that return  the list of the parcels
        /// </summary>
        /// <returns>return list of the parcel</returns>
        public IEnumerable<ParcelForList> GetParcels(Func<ParcelForList, bool> predicate = null)
        {
            IEnumerable<ParcelForList> l;

            l = myDal.GetParcels()
                    .Select(parcel =>
                        new ParcelForList
                        {
                            Id = parcel.ID,
                            SenderId = parcel.SenderId,
                            TargetId = parcel.TargetId,
                            Longitude = (WeightCategories)parcel.Longitude,
                            Priority = (Priorities)parcel.Priority,
                            Status = (ParcelStatuses)myDal.GetStatusOfParcel(parcel.ID)

                        }
                    );
            if (predicate == null)
                return l;
            return l.Where(predicate);
        }
        /// <summary>
        /// A function that return  the list of the unAssignmentParcels
        /// </summary>
        /// <returns>return list of the unAssignmentParcel</returns>
        public IEnumerable<ParcelForList> UnAssignmentParcels()
        {
            List<ParcelForList> p = new List<ParcelForList>();
            ParcelForList parcelBo;
            foreach (var parcel in myDal.GetParcels())
            {
                if (parcel.DroneId == 0)
                {
                    parcelBo = new ParcelForList()
                    {
                        Id = parcel.ID,
                        Longitude = (WeightCategories)parcel.Longitude,
                        Priority = (Priorities)parcel.Priority,
                        SenderId = parcel.SenderId,
                        TargetId = parcel.TargetId,
                        Status = ParcelStatuses.Requested


                    };
                    p.Add(parcelBo);
                }
            }
            return p;
        }
        /// <summary>
        /// A function that return  the list of the availableChargingStations
        /// </summary>
        /// <returns>return list of the availableChargingStation</returns>
        public IEnumerable<Station> AvailableChargingStations()
        {
            List<Station> s = new List<Station>();
            Station stationBo;
            foreach (var station in myDal.GetStations())
            {
                if (station.ChargeSlots > 0)
                {
                    List<DroneInCharging> d = new List<DroneInCharging>();
                    foreach (var drone in drones)
                    {
                        if (drone.Location.Longitude == station.Longitude && drone.Location.Lattitude == station.Lattitude)
                        {
                            DroneInCharging dIc = new DroneInCharging() { Battery = drone.Battery, Id = drone.Id };
                            d.Add(dIc);
                        }
                    }
                    stationBo = new Station()
                    {
                        Id = station.ID,
                        Name = station.Name,
                        ChargeSlots = station.ChargeSlots,
                        Drones = d,
                        Location = new Location()
                        {
                            Lattitude = station.Lattitude,
                            Longitude = station.Longitude
                        }



                    };
                    s.Add(stationBo);
                }
            }
            return s;
        }






        /// <summary>
        ///  Calculates the required power consumption of drone 
        /// </summary>
        /// <param name="distance"> The distance of the drone.</param>
        /// <param name="index"> Location of the drone in the list</param>
        /// <returns>return the power consumption of the drone   </returns>

        private double BatteryUsages(double distance, int index)
        {
            return myDal.PowerRequest()[index] * distance;
        }
        /// <summary>
        /// A function that assigns a package to a skimmer
        /// </summary>
        /// <param name="parcelId">the parcel we want to assign</param>
        /// <param name="droneId">the drone we want to assign</param>
        public void AssignmentParcelToDrone( int droneId)
        {
            try
            {

                DroneForList drone = GetDroneForList(droneId);
                if (drone.Id == 0)
                    throw new BLDroneException("DroneP not found");
                int maxW = 0, maxPri = 0;
                double minDis = 0;
                int pId = 0;
                int dId = 0;
                IDAL.DO.Parcel? parcelDo = null;
                if (drone.Status == DroneStatuses.Free)
                {
                    foreach (IDAL.DO.Parcel parcel in myDal.GetParcels())
                    {
                        Customer sc = GetCustomer(parcel.SenderId);
                        Location senderCustomerL = sc.Location;
                        Customer tc = GetCustomer(parcel.TargetId);
                        Location targetCustomerL = tc.Location;
                        double toCus = calcDistance(drone.Location, senderCustomerL);
                        double toStation = calcDistance(senderCustomerL, findClosetBaseStationLocation(senderCustomerL));
                        double betweenCustomers = calcDistance(senderCustomerL, targetCustomerL);
                        maxPri = (int)parcel.Priority;
                        maxW = (int)parcel.Longitude;
                        minDis = calcDistance(drone.Location, senderCustomerL);
                        if (BatteryUsages(toCus, 0) + BatteryUsages(toStation, 0) + BatteryUsages(betweenCustomers, (int)parcel.Longitude + 1) < drone.Battery)
                        {
                            parcelDo = parcel;

                            pId = parcel.ID;
                            dId = parcel.DroneId;
                        }

                    }
                    if (parcelDo != null)
                    {
                        int index = drones.FindIndex(x => x.Id == drone.Id);
                        drones[index].Status = DroneStatuses.Shipping;
                        DroneInParcel dp = new DroneInParcel() { Id = dId };
                        Parcel p = new Parcel()
                        {
                            Id = pId,
                            DroneP = dp
                        };
                        UpdateParcel(p);
                    }
                }
            }
            catch (Exception ex)
            {

                throw new BLDroneException("The drone can't be recharge", ex);
            }

        }

        /// <summary>
        /// The function checks whether the drone can be sent for charging if it can sends it to a charging station near it with charging stations and updates the data accordingly if it does not send appropriate exceptions
        /// </summary>
        /// <param name="droneId">the number of drone that need to send to recharge</param>
        public void SendDroneToRecharge(int droneId)
        {
            try
            {
                DroneForList drone = GetDroneForList(droneId);
                if (drone.Status == DroneStatuses.Free)
                {
                    List<Station> l = ClosetBaseStationsLocation(drone.Location);
                    foreach (Station item in l)
                    {
                        if (item.ChargeSlots > 0 && drone.Battery >= calcDistance(item.Location, drone.Location) * myDal.PowerRequest()[4])
                        {
                            //update drone
                            drone.Location = item.Location;
                            drone.Battery -= calcDistance(item.Location, drone.Location) * myDal.PowerRequest()[4];
                            drone.Status = DroneStatuses.Maintenance;
                            DroneForList d = new DroneForList()
                            {
                                Id = drone.Id,
                                Battery = drone.Battery,
                                Location = drone.Location,
                                Status = drone.Status
                            };
                            updateDroneForList(d);
                            //anchore station and drone
                            AnchorDroneStation(item, drone);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new BLDroneException("The drone can't be recharge", ex);
            }


        }
        /// <summary>
        /// A function that recieve a station and a drone and create a new dronecharge with both id`s and update the station accordingly
        /// </summary>
        /// <param name="station">the station that the drone charge there</param>
        /// <param name="d">the drone thet need to charge</param>

        public void AnchorDroneStation(Station station, DroneForList d)
        {
            IDAL.DO.Station s = new IDAL.DO.Station
            {
                ID = station.Id,
                Name = station.Name,
                Longitude = station.Location.Longitude,
                Lattitude = station.Location.Lattitude,
                ChargeSlots = station.ChargeSlots

            };
            IDAL.DO.Drone drone = new IDAL.DO.Drone
            {
                ID = d.Id,
                Model = d.Model,
                MaxWeight = (IDAL.DO.WeightCategories)d.MaxWeight

            };
            myDal.AnchorDroneStation(s, drone);
        }




        /// <summary>
        ///  A function that recieve a drone and time and checks whether the drone can be released from charging if you can, update the data layer with the required updates ,if no, exception is thrown
        /// </summary>
        /// <param name="droneId">the number drone its need to release</param>
        /// <param name="time">Time period on charge </param>
        public void ReleaseDroneFromRecharge(int droneId, double time)
        {

            try
            {
                DroneForList drone = drones.FirstOrDefault(x => x.Id == droneId);
                if (drone.Status == DroneStatuses.Maintenance)
                {
                    //update drone

                    drone.Battery += time * myDal.PowerRequest()[4];
                    drone.Status = DroneStatuses.Free;
                    updateDroneForList(drone);
                    //update station and drone in BL
                    IDAL.DO.Drone d = new IDAL.DO.Drone()
                    {
                        ID = drone.Id,
                        Model = drone.Model,
                        MaxWeight = (IDAL.DO.WeightCategories)drone.MaxWeight,

                    };
                    myDal.AddDrone(d);
                    int staionid = myDal.GetDronesInCharge().FirstOrDefault(x => x.DroneId == drone.Id).StationId;
                    IDAL.DO.Station s = myDal.GetStation(staionid);
                    myDal.ReleasDrone(d, s);

                }
            }
            catch (Exception ex)
            {

                throw new BLDroneException("The drone can't be release", ex);
            }
        }
        /// <summary>
        ///   The function provides a package by a drone if the package can be delivered updates the required data, if no appropriate exception is sent
        /// </summary>
        /// <param name="droneId">the number of the drone</param>
        public void PackageDeliveryByDrone(int droneId)
        {


            try
            {
                DroneForList droneL = drones.Find(x => x.Id == droneId);
                IDAL.DO.Parcel parcel = myDal.GetParcels().First(item => item.ID == droneL.ParcelId);
                IDAL.DO.Customer customer = myDal.Getcustomers().First(item => item.ID == parcel.ID);
                if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                {

                    Location l2 = new Location() { Lattitude = customer.Lattitude, Longitude = customer.Longitude };
                    double distance = calcDistance(droneL.Location, l2);
                    droneL.Battery -= (int)(distance * myDal.PowerRequest()[4]);
                    droneL.Location.Lattitude = customer.Lattitude;
                    droneL.Location.Longitude = customer.Longitude;
                    int index = drones.FindIndex(x => x.Id == droneId);
                    drones[index] = droneL;
                    myDal.UpdateParcels(parcel);
                }
                else throw new BLDroneException("The drone meet the condition that it is associated but has not been collect");


            }

            catch (ArgumentNullException ex)
            {
                throw new BLDroneException($"Drone {droneId} not exist", ex);
            }
            catch (BLDroneException ex)
            {
                throw new BLDroneException("not sucsseed", ex);
            }


        }

        /// <summary>
        ///   The function collects the package by the drone if the package can be collected and updates the required data,If no appropriate exception is sent
        /// </summary>
        /// <param name="droneId">the number of the drone</param>
        public void PickedupParcel(int droneId)
        {


            {
                try
                {
                    DroneForList droneL = drones.Find(x => x.Id == droneId);
                    IDAL.DO.Parcel parcel = myDal.GetParcels().First(item => item.ID == droneL.ParcelId);
                    DroneForList df = GetDroneForList(droneId);
                    ParcelForList pf = GetParcelForList(df.ParcelId);
                    CustomerInParcel s = new CustomerInParcel()
                    { Id = GetCustomer(pf.SenderId).Id, Name = GetCustomer(pf.SenderId).Name };
                    CustomerInParcel t = new CustomerInParcel()
                    { Id = GetCustomer(pf.TargetId).Id, Name = GetCustomer(pf.TargetId).Name };
                    PackageInTransfer pi = new PackageInTransfer()
                    {
                        Id = pf.Id,
                        Longitude = pf.Longitude,
                        Collection = GetCustomer(pf.SenderId).Location,
                        DeliveryDestination = GetCustomer(pf.TargetId).Location,
                        Priority = pf.Priority,
                        Sender = s,
                        Target = t,
                        Status = pf.Status,
                        TransportDistance = calcDistance(GetCustomer(pf.SenderId).Location, GetCustomer(pf.TargetId).Location)


                    };
                    Drone drone = new Drone()
                    { Id = df.Id, Battery = df.Battery, Location = df.Location, MaxWeight = df.MaxWeight, Model = df.Model, Status = df.Status, Package = pi };
                    if (parcel.Scheduled != DateTime.MinValue && parcel.PickedUp == DateTime.MinValue)
                    {

                        Location l1 = new Location() { Lattitude = drone.Package.Collection.Lattitude, Longitude = drone.Package.Collection.Longitude };
                        Location l2 = new Location() { Lattitude = drone.Package.DeliveryDestination.Lattitude, Longitude = drone.Package.DeliveryDestination.Longitude };
                        double distance = calcDistance(l1, l2);
                        droneL.Battery -= (int)(distance * myDal.PowerRequest()[4]);
                        droneL.Location = drone.Package.DeliveryDestination;
                        droneL.Status = DroneStatuses.Maintenance;
                        int index = drones.FindIndex(x => x.Id == droneId);
                        drones[index] = droneL;
                        myDal.UpdateParcels(parcel);
                    }
                    else
                        throw new BLDroneException("drone could not deliver parcel");


                }

                catch (ArgumentNullException ex)
                {
                    throw new BLDroneException($"drone {droneId} not exist", ex);
                }
                catch (BLDroneException ex)
                {
                    throw new BLDroneException($"drone {droneId}could not deliver parcel", ex);
                }
            }
        }

    }
}
