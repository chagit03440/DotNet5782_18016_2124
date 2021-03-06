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
        /// A function that recieve a droneCharge id and return from the list of the droneCharges the droneCharge with this id 
        /// </summary>
        /// <param name="id">the id of the droneCharge</param>
        /// <returns>return the droneCharge with this id </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public DroneCharge GetDroneCharge(int id)
        {
            if (!DataSource.incharge.Exists(x => x.DroneId == id))
            {
                throw new InVaildIdException($"id{id} doesn't exist!!");
            };
            return DataSource.incharge.Find(x => x.DroneId == id);
        }
    }
}
