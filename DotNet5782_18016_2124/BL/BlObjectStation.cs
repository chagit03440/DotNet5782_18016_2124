using System.Runtime.CompilerServices;
using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dal;
using BlApi;

namespace BL
{
    public partial class BLObject : IBL
    {
        /// <summary>
        /// A function that recieve a stations id and return the location of this station.
        /// </summary>
        /// <param name="stationId">station id</param>
        /// <returns>return the stations location</returns>
        private Location getBaseStationLocation(int stationId)
        {
            DO.Station baseStation;
            try
            {

                baseStation = myDal.GetStation(stationId);
            }
            catch (DO.InVaildIdException ex)
            {

                throw new BLInVaildIdException("The station not exsit", ex);
            }
            return new Location { Lattitude = baseStation.Lattitude, Longitude = baseStation.Longitude };

        }
        /// <summary>
        ///  A function that recieve a station an number of charging stations and and update the station list
        /// </summary>
        /// <param name="st">station we need to updat</param>
        /// <param name="cs">num of charging stations</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStation(Station st, int cs)
        {
            Station s;
            try
            {
                s = GetStation(st.Id);
            }
            catch (DO.InVaildIdException ex)
            {

                throw new BLInVaildIdException("The station not exsit", ex);
            }
            StationForList sf;
            if (st.Name != null)
                s.Name = st.Name;
            if (cs != -1)
            {
                int notAvailible = drones.Count(x => x.DroneLocation == s.Location);
                int availible = cs - notAvailible;


                sf = new StationForList()
                {
                    Id = s.Id,
                    Name = s.Name,
                    NotAvailableChargeSlots = notAvailible,
                    AvailableChargeSlots = availible
                };
                s.ChargeSlots = sf.AvailableChargeSlots;
            }

            DO.Station stationDo = new DO.Station() { ID = s.Id, Name = s.Name, ChargeSlots = s.ChargeSlots, Longitude = s.Location.Longitude, Lattitude = s.Location.Lattitude };
            myDal.UpdateStations(stationDo);


        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station s)
        {
            DO.Station dp = new DO.Station() { ID = s.Id };
            myDal.DeleteStation(dp);
        }
        /// <summary>
        /// add Base excestationption to dal
        /// </summary>
        /// <param name="AddbaseStation">receive from BL</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station AddbaseStation)
        {
            DO.Station baseStationDO =
                new DO.Station()
                {
                    ID = AddbaseStation.Id,
                    Name = AddbaseStation.Name,
                    ChargeSlots = AddbaseStation.ChargeSlots,
                    Lattitude = AddbaseStation.Location.Lattitude,
                    Longitude = AddbaseStation.Location.Longitude
                };

            try
            {
                myDal.AddStation(baseStationDO);
            }
            catch (DO.InVaildIdException ex)
            {
                //sending inner exception for the exception returning from the DAL
                throw new BLInVaildIdException(ex.Message, ex);
            }
        }
        /// <summary>
        /// A function that recieve a stations id and return from the list of the station the station with this id
        /// </summary>
        /// <param name="requestedId">station id</param>
        /// <returns>return the station with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int requestedId)
        {
            DO.Station baseStationDO;
            try
            {
                baseStationDO = myDal.GetStation(requestedId);

            }
            catch (BLInVaildIdException ex)
            {
                throw new BLInVaildIdException("id didn't exist", ex);
            }
            Station baseStationBO = new Station();
            baseStationBO.Id = baseStationDO.ID;
            baseStationBO.Name = baseStationDO.Name;
            baseStationBO.ChargeSlots = myDal.AvailableChargingPorts(baseStationDO.ID);
            baseStationBO.Location = new Location()
            {
                Lattitude = baseStationDO.Lattitude,
                Longitude = baseStationDO.Longitude
            };
            baseStationBO.Drones = dronesInStation(baseStationBO.Id);

            return baseStationBO;
        }
        /// <summary>
        /// a function that recieves a station id and return a list of all the drones in this station
        /// </summary>
        /// <param name="stationId">the id of the station</param>
        /// <returns>a list of all the drones in this station</returns>
        private List<DroneInCharging> dronesInStation(int stationId)
        {
            List<DroneInCharging> l = new List<DroneInCharging>();
            int stationId1 = 0;
            foreach (var dc in GetDrones())
            {
                foreach (var dr in myDal.GetDronesInCharge())
                {
                    if (dr.DroneId == dc.Id)
                        stationId1 = dr.StationId;
                }
                if (stationId1 == stationId)
                {
                    DroneInCharging d = new DroneInCharging()
                    {
                        Id = dc.Id,
                        Battery = getDroneBattery(dc.Id)
                    };

                    l.Add(d);
                }
            }
            return l;

        }
        /// <summary>
        /// a function that recieves a station id and returns the used Charging Ports of the station
        /// </summary>
        /// <param name="baseStationId">the id of the station</param>
        /// <returns>the used Charging Ports of the station</returns>
        private int getUsedChargingPorts(int baseStationId)
        {
            return myDal.GetStation(baseStationId).ChargeSlots - myDal.AvailableChargingPorts(baseStationId);
        }
    }
}
