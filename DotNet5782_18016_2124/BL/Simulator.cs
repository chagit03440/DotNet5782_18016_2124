using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using static System.Math;
using static BL.BLObject;
using static System.Math;
using DalApi;

namespace BL
{
    internal class Simulator
    {
        enum Maintenance { Starting, Going, Charging }
        private const double VELOCITY = 1.0;
        private const int DELAY = 500;
        private const double TIME_STEP = DELAY / 1000.0;
        private const double STEP = VELOCITY / TIME_STEP;
        private DalApi.IDal myDal;

        public Simulator(BLObject blo, int droneId, Action updateDrone, Func<bool> checkStop)
        {
             
            myDal = DalFactory.GetDal();
            var bl = blo;
            var drone = bl.GetDroneForList(droneId);
            int? parcelId = null;
            int? baseStationId = null;
            Station bs = null;
            double distance = 0.0;
            int batteryUsage = 0;
            DO.Parcel? parcel = null;
            bool pickedUp = false;
            Customer customer = null;
            Maintenance maintenance = Maintenance.Starting;

            void initDelivery(int id)
            {
                parcel = myDal.GetParcel(id);
                batteryUsage = (int)Enum.Parse(typeof(BatteryUsage), parcel?.Longitude.ToString());
                pickedUp = parcel?.PickedUp is not null;
                customer = bl.GetCustomer((int)(pickedUp ? parcel?.TargetId : parcel?.SenderId));
            }

            do
            {
               

                switch (drone)
                {
                    case DroneForList { Status: DroneStatuses.Free }:
                        if (!sleepDelayTime()) break;

                        lock (bl) lock (myDal)
                            {
                                parcelId = bl.GetParcels(p => p?.Status != ParcelStatuses.Scheduled
                                                                  && (WeightCategories)(p?.Longitude) <= drone.MaxWeight
                                                                  && bl.calcMinBatteryRequired(drone) < drone.Battery)
                                                 .OrderByDescending(p => p?.Priority)
                                                 .ThenByDescending(p => p?.Longitude)
                                                 .FirstOrDefault()?.Id;
                                switch (parcelId, drone.Battery)
                                {
                                    case (null, 1.0):
                                        break;

                                    case (null, _):
                                        baseStationId = bl.findClosetBaseStation(drone)?.Id;
                                        if (baseStationId != null)
                                        {
                                            drone.Status = DroneStatuses.Maintenance;
                                            maintenance = Maintenance.Starting;
                                            myDal.BaseStationDroneIn((int)baseStationId);
                                            myDal.AddDroneCharge(droneId, (int)baseStationId);
                                        }
                                        break;
                                    case (_, _):
                                        try
                                        {
                                            myDal.ParcelSchedule(parcelId, droneId);
                                            drone.ParcelId = (int)parcelId;
                                            initDelivery((int)parcelId);
                                            drone.Status = DroneStatuses.Shipping;
                                        }
                                        catch (DO.AlreadyExistExeption ex) { throw new BLAlreadyExistExeption("Internal error getting parcel", ex); }
                                        break;
                                }
                            }
                        break;

                    case DroneForList { Status: DroneStatuses.Maintenance }:
                        switch (maintenance)
                        {
                            case Maintenance.Starting:
                                lock (bl) lock (myDal)
                                    {
                                        try { bs = bl.GetStation(myDal.GetDroneCharge(drone.Id).StationId); }
                                        catch (DO.InVaildIdException ex) { throw new BLInVaildIdException("Internal error base station", ex); }
                                        distance = bl.calcDistance(drone.DroneLocation,bs.Location);
                                        maintenance = Maintenance.Going;
                                    }
                                break;

                            case Maintenance.Going:
                                if (distance < 0.01)
                                    lock (bl)
                                    {
                                        drone.DroneLocation = bs.Location;
                                        maintenance = Maintenance.Charging;
                                    }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl)
                                    {
                                        double delta = distance < STEP ? distance : STEP;
                                        distance -= delta;
                                        drone.Battery = Max(0.0, drone.Battery - bl.BatteryUsages(delta, 0));
                                    }
                                }
                                break;

                            case Maintenance.Charging:
                                if (drone.Battery == 1.0)
                                    lock (bl) lock (myDal)
                                        {
                                            drone.Status = DroneStatuses.Free;
                                            myDal.DeleteDroneCharge(droneId);
                                            myDal.BaseStationDroneOut(bs.Id);
                                        }
                                else
                                {
                                    if (!sleepDelayTime()) break;
                                    lock (bl) drone.Battery = Min(1.0, drone.Battery + bl.BatteryUsages(TIME_STEP, 4));
                                }
                                break;
                            default:
                                throw new BLInVaildIdException("Internal error: wrong maintenance substate");
                        }
                        break;

                    case DroneForList { Status: DroneStatuses.Shipping }:
                        lock (bl) lock (myDal)
                            {
                                try { if (parcelId == null) initDelivery((int)drone.ParcelId); }
                                catch (DO.AlreadyExistExeption ex) { throw new BLInVaildIdException("Internal error getting parcel", ex); }
                                distance = bl.calcDistance(drone.DroneLocation, customer.Location);
                            }

                        if (distance < 0.01 || drone.Battery == 0.0)
                            lock (bl) lock (myDal)
                                {
                                    drone.DroneLocation = customer.Location;
                                    if (pickedUp)
                                    {
                                        bl.PackageDeliveryByDrone ((int)parcel?.ID);
                                        drone.Status = DroneStatuses.Free;

                                    }
                                    else
                                    {
                                        myDal.ParcelPickup((int)parcel?.ID);
                                        customer = bl.GetCustomer((int)parcel?.TargetId);
                                        pickedUp = true;
                                    }
                                }
                        else
                        {
                            if (!sleepDelayTime()) break;
                            lock (bl)
                            {
                                double delta = distance < STEP ? distance : STEP;
                                double proportion = delta / distance;
                                drone.Battery = Max(0.0, drone.Battery - bl.BatteryUsages(delta, 0));
                                double lat = drone.DroneLocation.Lattitude + (customer.Location.Lattitude - drone.DroneLocation.Lattitude) * proportion;
                                double lon = drone.DroneLocation.Longitude + (customer.Location.Longitude - drone.DroneLocation.Longitude) * proportion;
                                drone.DroneLocation = new() { Lattitude = lat, Longitude = lon };
                            }
                        }
                        break;

                    default:
                        throw new BLInVaildIdException("Internal error: not available after Delivery...");

                }
                updateDrone();
            } while (!checkStop());
        }

        private static bool sleepDelayTime()
        {
            try { Thread.Sleep(DELAY); } catch (ThreadInterruptedException) { return false; }
            return true;
        }
    }
}
