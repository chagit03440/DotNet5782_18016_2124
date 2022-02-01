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
    internal partial class DalObject : IDal
    {
        /// <summary>
        ///  A function that recieve a station and add it to the lists of the stations
        /// </summary>
        /// <param name="station">the station we need to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddStation(Station station)
        {
            if (DataSource.stations.Exists(x => x.ID == station.ID))
            {
                throw new AlreadyExistExeption($"id{station.ID} allready exist!!");
            };
            DataSource.stations.Add(station);
        }
        /// <summary>
        ///  A function that recieve a stations id and return from the list of the stations the station with this id
        /// </summary>
        /// <param name="id">the id of the station</param>
        /// <returns>return the station with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetStation(int id)
        {
            if (!DataSource.stations.Exists(x => x.ID == id))
            {
                throw new InVaildIdException($"id{id} doesn't exist!!");
            };
            return DataSource.stations.Find(x => x.ID == id);
        }
        /// <summary>
        /// A function that recieve a station and delete the station whith the same id in the stations list
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateStations(Station st)
        {
            DataSource.stations.RemoveAll(item => item.ID == st.ID);
            DataSource.stations.Add(st);
        }
        /// <summary>
        /// a unction that calculate the number of available Charging Ports
        /// </summary>
        /// <param name="baseStationId">the base staion to check on</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int AvailableChargingPorts(int baseStationId)
        {
            try
            {
                return GetStation(baseStationId).ChargeSlots - DataSource.incharge.Count(dc => dc.StationId == baseStationId);

            }
            catch (InVaildIdException ex)
            {
                throw new InVaildIdException("Station didn't exist", ex);
            }
        }
        /// <summary>
        /// A function that recieve a station and delete the station whith the same id in the stations list
        /// </summary>
        /// <param name="s"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteStation(Station s)
        {
            DataSource.parcels.RemoveAll(x => x.ID == s.ID);
        }
    }
}
