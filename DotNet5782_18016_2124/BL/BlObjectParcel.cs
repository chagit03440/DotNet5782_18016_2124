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
        /// A function that recieve a parcels id and return from the list of the parcel the parcel with this id
        /// </summary>
        /// <param name="requestedId">id parcel</param>
        /// <returns>return the parcel with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int requestedId)
        {
            lock (myDal)
            {
                DroneInParcel d = null;
                DO.Parcel parcelDO;
                try
                {
                    parcelDO = myDal.GetParcel(requestedId);

                }
                catch (DO.InVaildIdException ex)
                {
                    throw new BLInVaildIdException("id didn't exist", ex);
                }
                if (parcelDO.Scheduled != null)
                {
                    d = new DroneInParcel() { Battery = GetDrone(parcelDO.DroneId).Battery, Id = parcelDO.DroneId, Location = GetDrone(parcelDO.DroneId).Location };
                }

                Parcel parcelBO = new Parcel()
                {
                    Id = parcelDO.ID,
                    Longitude = (WeightCategories)parcelDO.Longitude,
                    Priority = (Priorities)parcelDO.Priority,
                    Sender = new CustomerInParcel() { Id = parcelDO.SenderId, Name = GetCustomer(parcelDO.SenderId).Name },
                    Target = new CustomerInParcel() { Id = parcelDO.TargetId, Name = GetCustomer(parcelDO.TargetId).Name },
                    AssociationTime = null,
                    CollectionTime = null,
                    CreationTime = DateTime.MinValue,
                    DroneP = d,
                    SupplyTime = null

                };



                return parcelBO;
            }
        }
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcel the parcel with this id
        /// </summary>
        /// <param name="requestedId">id parcel</param>
        /// <returns>return the parcelForList with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public ParcelForList GetParcelForList(int requestedId)
        {
            lock (myDal)
            {
                DO.Parcel parcelDO;
                try
                {
                    parcelDO = myDal.GetParcel(requestedId);

                }
                catch (DO.InVaildIdException ex)
                {
                    throw new BLInVaildIdException("id didn't exist", ex);
                }
                ParcelForList parcelBO = new ParcelForList()
                {
                    Id = parcelDO.ID,
                    Longitude = (WeightCategories)parcelDO.Longitude,
                    Priority = (Priorities)parcelDO.Priority,
                    SenderId = parcelDO.SenderId,
                    Status = (ParcelStatuses)getParcelStatus(parcelDO),
                    TargetId = parcelDO.TargetId

                };
                return parcelBO;
            }
        }
            /// <summary>
            /// A function that recieve a parcels id and returns the status of the parcel
            /// </summary>
            /// <param name="p">thd  requierd parcel  </param>
            /// <returns></returns>
            private int getParcelStatus(DO.Parcel p)
        {
            if (p.Requested != null)
                return 0;
            if (p.Scheduled != null)
                return 1;
            if (p.PickedUp != null)
                return 2;
            if (p.Delivered != null)
                return 3;

            return -1;
        }
        /// <summary>
        /// A function that recieve a parcel and add the parcel to the list
        /// </summary>
        /// <param name="newParcel">the parcel we need to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddParcel(Parcel newParcel)
        {
            lock (myDal)
            {
                newParcel.AssociationTime = null;
                newParcel.CollectionTime = null;
                newParcel.SupplyTime = null;
                newParcel.CreationTime = DateTime.Now;
                newParcel.DroneP = null;
                DO.Parcel parcelDO = new DO.Parcel();
                parcelDO.SenderId = newParcel.Sender.Id;
                parcelDO.TargetId = newParcel.Target.Id;
                parcelDO.ID = newParcel.Id;
                parcelDO.SenderId = newParcel.Sender.Id;
                parcelDO.TargetId = newParcel.Target.Id;

                try
                {
                    myDal.AddParcel(parcelDO);
                }
                catch (DO.InVaildIdException exp)
                {

                    throw new BLInVaildIdException("", exp);
                }
            }
        }
        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id
        /// </summary>
        /// <param name="parcel">parcel to update</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcel(Parcel parcel)
        {
            lock (myDal)
            {
                DO.Parcel p = myDal.GetParcels().FirstOrDefault(x => x.ID == parcel.Id);
                p.DroneId = parcel.DroneP.Id;
                p.Scheduled = DateTime.Today;
                myDal.UpdateParcel(p);
            }
        }
        /// <summary>
        /// A function that recieve a parcel and delete it from the lists of the parcels
        /// </summary>
        /// <param name="p">the parcel to delete</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteParcel(Parcel p)
        {
            lock (myDal)
            {
                DO.Parcel dp = new DO.Parcel() { ID = p.Id };
                myDal.DeleteParcel(dp);
            }
        }
    }
}