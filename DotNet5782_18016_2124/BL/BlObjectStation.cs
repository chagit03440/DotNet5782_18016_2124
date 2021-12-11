using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
namespace BL
{
    public partial class BLObject : IBL.IBL
    {
        /// <summary>
        /// A function that recieve a stations id and return the location of this station.
        /// </summary>
        /// <param name="stationId">station id</param>
        /// <returns>return the stations location</returns>
        private Location getBaseStationLocation(int stationId)
        {
            IDAL.DO.Station baseStation;
            try
            {

                baseStation = myDal.GetStation(stationId);
            }
            catch (Exception ex)
            {

                throw new BLStationException("The station not exsit", ex);
            }
            return new Location { Lattitude = baseStation.Lattitude, Longitude = baseStation.Longitude };

        }
        /// <summary>
        ///  A function that recieve a station an number of charging stations and and update the station list
        /// </summary>
        /// <param name="st">station we need to updat</param>
        /// <param name="cs">num of charging stations</param>
        public void UpdateStation(Station st, int cs)
        {
            Station s;
            try
            {
                s = GetStation(st.Id);
            }
            catch (Exception ex)
            {

                throw new BLStationException("The station not exsit", ex);
            }
            StationForList sf;
            if (st.Name != null)
                s.Name = st.Name;
            if (cs != -1)
            {
                int notAvailible = drones.Count(x => x.Location == s.Location);
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

            IDAL.DO.Station stationDo = new IDAL.DO.Station() { ID = s.Id, Name = s.Name, ChargeSlots = s.ChargeSlots, Longitude = s.Location.Longitude, Lattitude = s.Location.Lattitude };
            myDal.UpdateStations(stationDo);


        }
        /// <summary>
        /// add Base excestationption to dal
        /// </summary>
        /// <param name="AddbaseStation">receive from BL</param>
        public void AddStation(Station AddbaseStation)
        {
            IDAL.DO.Station baseStationDO =
                new IDAL.DO.Station()
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
            catch (Exception ex)
            {
                //sending inner exception for the exception returning from the DAL
                throw new BLStationException(ex.Message, ex);
            }
        }
        /// <summary>
        /// A function that recieve a stations id and return from the list of the station the station with this id
        /// </summary>
        /// <param name="requestedId">station id</param>
        /// <returns>return the station with this id</returns>
        public Station GetStation(int requestedId)
        {
            IDAL.DO.Station baseStationDO;
            try
            {
                baseStationDO = myDal.GetStation(requestedId);

            }
            catch (BLCustomerException ex)
            {
                throw new BLCustomerException("id didn't exist", ex);
            }
            IBL.BO.Station baseStationBO = new Station();
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
