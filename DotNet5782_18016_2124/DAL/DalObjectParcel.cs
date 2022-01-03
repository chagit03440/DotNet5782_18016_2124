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
        ///  A function that recieve a parcel and add it to the lists of the parcels
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        public void AddParcel(Parcel newParcel)
        {
            if (DataSource.parcels.Exists(x => x.ID == newParcel.ID))
            {
                throw new AlreadyExistExeption($"id{newParcel.ID} allready exist!!");
            };
            DataSource.parcels.Add(newParcel);
        }
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcels the parcel with this id
        /// </summary>
        /// <param name="id">the id of the parcel</param>
        /// <returns>return the drone with this id</returns>
        public Parcel GetParcel(int id)
        {
            if (!DataSource.parcels.Exists(x => x.ID == id))
            {
                throw new InVaildIdException($"id{id} doesn't exist!!");
            };
            return DataSource.parcels.Find(x => x.ID == id);
        }
        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id in the parcels list
        /// </summary>
        /// <param name="parcel">the parcel with new data to update</param>
        public void UpdateParcel(Parcel parcel)
        {
            try
            {
                GetParcel(parcel.ID);
            }
            catch (InVaildIdException p)
            {
                throw new InVaildIdException($"cannot update parcel{parcel.ID}", p);
            }


            Parcel Ptmp = new Parcel();
            int result = DataSource.parcels.FindIndex(x => x.ID == parcel.ID);
            Ptmp.ID = DataSource.parcels[result].ID;
            Ptmp.SenderId = DataSource.parcels[result].ID;
            Ptmp.Longitude = DataSource.parcels[result].Longitude;
            Ptmp.Priority = DataSource.parcels[result].Priority;
            Ptmp.Requested = DataSource.parcels[result].Requested;
            Ptmp.Scheduled = DataSource.parcels[result].Scheduled;
            Ptmp.PickedUp = DataSource.parcels[result].PickedUp;
            Ptmp.TargetId = DataSource.parcels[result].TargetId;
            DataSource.parcels.RemoveAt(result);
            DataSource.parcels.Add(Ptmp);
        }
        public void UpdateParcels(Parcel parcel)
        {
            DataSource.parcels.RemoveAll(x => x.ID == parcel.ID);
            DataSource.parcels.Add(parcel);
        }
        public int GetStatusOfParcel(int parceliD)
        {
            Parcel parcel = GetParcel(parceliD);
            if (parcel.Requested != null)
                return 0;
            if (parcel.Scheduled != null)
                return 1;
            if (parcel.PickedUp != null)
                return 2;
            if (parcel.Delivered != null)
                return 3;
            return -1;

        }
        public void DeleteParcel(Parcel p)
        {
            DataSource.parcels.RemoveAll(x => x.ID == p.ID);
        }
    }
}
