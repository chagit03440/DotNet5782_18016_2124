﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DalObject;
namespace BL
{
    public partial class BLObject : IBL.IBL
    {
        /// <summary>
        /// A function that recieve a parcels id and return from the list of the parcel the parcel with this id
        /// </summary>
        /// <param name="requestedId">id parcel</param>
        /// <returns>return the parcel with this id</returns>
        public ParcelForList GetParcel(int requestedId)
        {
            IDAL.DO.Parcel parcelDO;
            try
            {
                parcelDO = myDal.GetParcel(requestedId);

            }
            catch (BLParcelException ex)
            {
                throw new BLParcelException("id didn't exist", ex);
            }
            IBL.BO.ParcelForList parcelBO = new ParcelForList()
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
        /// <summary>
        /// A function that recieve a parcels id and returns the status of the parcel
        /// </summary>
        /// <param name="p">thd  requierd parcel  </param>
        /// <returns></returns>
        private int getParcelStatus(IDAL.DO.Parcel p)
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
        public void AddParcel(Parcel newParcel)
        {
            newParcel.AssociationTime = 0;
            newParcel.CollectionTime = 0;
            newParcel.SupplyTime = 0;
            newParcel.CreationTime = DateTime.Now;
            newParcel.DroneP = null;
            IDAL.DO.Parcel parcelDO = new IDAL.DO.Parcel();
            parcelDO.ID = newParcel.Id;
            try
            {
                myDal.AddParcel(parcelDO);
            }
            catch (Exception exp)
            {

                throw new BLParcelException("", exp);
            }
        }
        /// <summary>
        /// A function that recieve a parcel and update the parcel whith the same id
        /// </summary>
        /// <param name="parcel">parcel to update</param>
        public void UpdateParcel(Parcel parcel)
        {
            IDAL.DO.Parcel p = myDal.GetParcels().FirstOrDefault(x => x.ID == parcel.Id);
            p.DroneId = parcel.DroneP.Id;
            p.Scheduled = DateTime.Today;
            myDal.UpdateParcel(p);

        }
    }
}