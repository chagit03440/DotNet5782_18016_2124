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
     
   internal partial class DalObject : DalApi
 { 
        /// <summary>
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add </param>
        public void AddCustomer(Customer customer)
        {
            if (DataSource.customers.Exists(x => x.ID == customer.ID))
            {
                throw new InVaildIdException($"id{customer.ID} allready exist!!");
            };
            DataSource.customers.Add(customer);
        }
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="id">the id of the customer</param>
        /// <returns>return the customer with this id</returns>
        public Customer GetCustomer(int id)
        {
            if (!DataSource.customers.Exists(x => x.ID == id))
            {
                throw new InVaildIdException($"id{id} doesn't exist!!");
            };
            return DataSource.customers.Find(x => x.ID == id);
        }
        /// <summary>
        /// A function that recieve a customer and update the customer whith the same id in the customer list
        /// </summary>
        /// <param name="customer">customer that need update</param>
        public void UpdateCustomers(Customer customer)
        {
            Customer cus = DataSource.customers.Find(x => x.ID == customer.ID);

            int index = DataSource.customers.FindIndex(x => x.ID == customer.ID);

            DataSource.customers.RemoveAt(index);
            DataSource.customers.Insert(index, cus);
        }
        public int ParcelsCustomerGot(int customerId)
        {
            return DataSource.parcels.Count(h => h.TargetId == customerId);
        }
        public int ParcelsCustomerSendAndDelivered(int customerId)
        {
            return DataSource.parcels.Count(h => h.SenderId == customerId && h.Delivered != null);

        }
        public int ParcelsCustomerSendAndNotDelivered(int customerId)
        {
            return DataSource.parcels.Count(h => h.SenderId == customerId && h.Delivered == null);

        }
        public int ParcelsInTheWayToCustomer(int customerId)
        {
            return DataSource.parcels.Count(h => h.TargetId == customerId && h.PickedUp != null);

        }
    }
}

