using System;
using System.Collections.Generic;
using System.Text;
using IDAL;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using IDAL.DO;

namespace DAL
{
    public partial class DalObject : IDal
    {
        /// <summary>
        /// A function that recieve a droneCharge id and return from the list of the droneCharges the droneCharge with this id 
        /// </summary>
        /// <param name="id">the id of the droneCharge</param>
        /// <returns>return the droneCharge with this id </returns>
        public DroneCharge GetDroneCharge(int id)
        {
            if (!DataSource.incharge.Exists(x => x.DroneId == id))
            {
                throw new DroneChargeException($"id{id} doesn't exist!!");
            };
            return DataSource.incharge.Find(x => x.DroneId == id);
        }
    }
}
