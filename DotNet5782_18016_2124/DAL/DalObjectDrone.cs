using System;
using System.Collections.Generic;
using System.Text;
using DAL.DalObject;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using DO;

namespace DalObject
{
    public partial class DalObject : DalApi
    {
        /// <summary>
        ///  A function that recieve a drone and add it to the lists of the drones
        /// </summary>
        /// <param name="drone">the drone we need to add</param>
        public void AddDrone(Drone drone)
        {
            if (DataSource.drones.Exists(x => x.ID == drone.ID))
            {
                throw new DroneException($"id{drone.ID} allready exist!!");
            };
            DataSource.drones.Add(drone);

        }
        /// <summary>
        /// A function that recieve a drones id and return from the list of the drones the drone with this id 
        /// </summary>
        /// <param name="id">the id of the drone</param>
        /// <returns>return the drone with this id</returns>
        public Drone GetDrone(int id)
        {
            if (!DataSource.drones.Exists(x => x.ID == id))
            {
                throw new DroneException($"id{id} doesn't exist!!");
            };
            return DataSource.drones.Find(x => x.ID == id);
        }

        /// <summary>
        ///  A function that recieve a drone and update the drone whith the same id in the drones list
        /// </summary>
        /// <param name="dr">the drone with new data to update</param>
        public void UpdateDrones(Drone dr)
        {
            Drone drone = DataSource.drones.Find(x => x.ID == dr.ID);

            int index = DataSource.drones.FindIndex(x => x.ID == dr.ID);

            DataSource.drones.RemoveAt(index);
            DataSource.drones.Insert(index, drone);
        }

    }
}
