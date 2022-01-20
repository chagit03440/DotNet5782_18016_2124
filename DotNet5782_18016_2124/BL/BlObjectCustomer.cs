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
        ///  A function that recieve a customer and update the customer whith the same id in the customer list
        /// </summary>
        /// <param name="cus">customer that need update</param>
        public void UpdateCustomer(Customer cus)
        {
            Customer c;
            try
            {
                c = GetCustomer(cus.Id);
            }
            catch (Exception ex)
            {

                throw new BLInVaildIdException("The customer not exsit", ex);
            }
            if (cus.Name != null)
                c.Name = cus.Name;
            if (cus.Phone != null)
                c.Phone = cus.Phone;
            DO.Customer cuaDo = new DO.Customer()
            {
                ID = c.Id,
                Name = c.Name,
                Lattitude = c.Location.Lattitude,
                Longitude = c.Location.Longitude,
                Phone = c.Phone
            };
            myDal.UpdateCustomers(cuaDo);
        }
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add</param>
        public void AddCustomer(Customer customer)
        {

            DO.Customer customerDO =
                            new DO.Customer()
                            {
                                ID = customer.Id,
                                Name = customer.Name,
                                Phone = customer.Phone,
                                Lattitude = customer.Location.Lattitude,
                                Longitude = customer.Location.Longitude
                            };

            try
            {
                myDal.AddCustomer(customerDO);
            }
            catch (Exception ex)
            {
                //sending inner exception for the exception returning from the DAL
                throw new BLInVaildIdException(ex.Message, ex);
            }
        }
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="requestedId">customer id</param>
        /// <returns>return customer with this id</returns>
        public Customer GetCustomer(int requestedId)
        {
            DO.Customer customerDO;
            try
            {
                customerDO = myDal.GetCustomer(requestedId);

            }
            catch (BLInVaildIdException ex)
            {
                throw new BLInVaildIdException("id didn't exist", ex);
            }
            Customer customerBO = new Customer();
            customerBO.Id = customerDO.ID;
            customerBO.Name = customerDO.Name;
            customerBO.Phone = customerDO.Phone;
            customerBO.Location = new Location() { Lattitude = customerDO.Lattitude, Longitude = customerDO.Longitude };
            customerBO.ParcelsFromTheCustomer =(List<ParcelInCustomer> )getCustomerShippedParcels(requestedId);
            customerBO.ParcelsToTheCustomer = (List<ParcelInCustomer>)getCustomerReceivedParcels(requestedId);
            return customerBO;
        }
        /// <summary>
        /// A function that receives a customer and returns all the packages that the customer received
        /// </summary>
        /// <param name="requestedId">the id of the customer</param>
        /// <returns>the of all the packages that the customer received</returns>
        private IEnumerable<ParcelInCustomer> getCustomerReceivedParcels(int requestedId)
        {
            List<ParcelInCustomer> deliveries = new List<ParcelInCustomer>();
            foreach (DO.Parcel p in myDal.GetParcels())
            {
                if (p.TargetId == requestedId)
                {
                    ParcelInCustomer pi = new ParcelInCustomer
                    {
                        Id = p.ID,
                        Sender = new CustomerInParcel() { Id = p.SenderId },
                        Target = new CustomerInParcel() { Id = p.TargetId },
                        Priority = (Priorities)p.Priority,
                        Longitude = (WeightCategories)p.Longitude

                    };
                    deliveries.Add(pi);
                }
            }

            return deliveries;
        }
        /// <summary>
        ///  A function that receives a customer and returns all the packages that the customer shipped
        /// </summary>
        /// <param name="requestedId">the id of the customer</param>
        /// <returns>the of all the packages that the customer shipped</returns>
        private IEnumerable<ParcelInCustomer> getCustomerShippedParcels(int requestedId)
        {
            List<ParcelInCustomer> deliveries = new List<ParcelInCustomer>();
            foreach (var parcel in myDal.GetParcels())
            {
                if (parcel.SenderId == requestedId)
                {
                    deliveries.Add(new ParcelInCustomer()
                    {
                        Id = parcel.ID,
                        Sender = new CustomerInParcel() { Id = parcel.SenderId },
                        Target = new CustomerInParcel() { Id = parcel.TargetId },
                        Priority = (Priorities)parcel.Priority,
                        Longitude = (WeightCategories)parcel.Longitude
                    }
                    );
                }
            }
            return deliveries;
        }
        ///// <summary>
        ///// A function that recieve a customers id and add it to the list of the customers
        ///// </summary>
        ///// <param name="customer">the customer we need to add</param>
        //public void AddNewCustomer(Customer customer)
        //{
        //    IDAL.DO.Customer customerDO = new IDAL.DO.Customer();
        //    customerDO.ID = customer.Id;
        //    customerDO.Name = customer.Name;
        //    customerDO.Phone = customer.Phone;
        //    customerDO.Lattitude = customer.DroneLocation.Lattitude;
        //    customerDO.Longitude = customer.DroneLocation.Longitude;

        //    try
        //    {
        //        myDal.AddCustomer(customerDO);
        //    }
        //    catch (Exception exp)
        //    {

        //        throw new BLInVaildIdException("", exp);
        //    }
        //}

    }
}