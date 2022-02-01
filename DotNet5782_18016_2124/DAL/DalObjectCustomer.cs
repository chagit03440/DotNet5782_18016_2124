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
        /// A function that recieve a customer and add it to the lists of the customers
        /// </summary>
        /// <param name="customer">the customer we need to add </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddCustomer(Customer customer)
        {
            if (DataSource.customers.Exists(x => x.ID == customer.ID))
            {
                throw new InVaildIdException($"id{customer.ID} allready exist!!");
            };
            DataSource.customers.Add(customer);
            User u = new User()
            {
                UserName = customer.Name,
                Password =customer.ID.ToString(),
                Worker = false
            };
            DataSource.ListUser.Add(u);
        }
        /// <summary>
        /// A function that recieve a customers id and return from the list of the customers the customer with this id
        /// </summary>
        /// <param name="id">the id of the customer</param>
        /// <returns>return the customer with this id</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
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
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomers(Customer customer)
        {
            Customer cus = DataSource.customers.Find(x => x.ID == customer.ID);

            int index = DataSource.customers.FindIndex(x => x.ID == customer.ID);

            DataSource.customers.RemoveAt(index);
            DataSource.customers.Insert(index, cus);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerGot(int customerId)
        {
            return DataSource.parcels.Count(h => h.TargetId == customerId);
        }
        /// <summary>
        /// a unction that calculate the number of parcels the customer send and not delievered
        /// </summary>
        /// <param name="iD">the customer to check on</param>
        /// <returns>the number of parcels the customer send and delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndDelivered(int customerId)
        {
            return DataSource.parcels.Count(h => h.SenderId == customerId && h.Delivered != null);

        }
        /// <summary>
        /// a unction that calculate the number of parcels the customer send and not delievered
        /// </summary>
        /// <param name="iD">the customer to check on</param>
        /// <returns>the number of parcels the customer send and not delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsCustomerSendAndNotDelivered(int customerId)
        {
            return DataSource.parcels.Count(h => h.SenderId == customerId && h.Delivered == null);

        }
        /// <summary>
        /// a unction that calculate the number of parcels on the way to the customer 
        /// </summary>
        /// <param name="iD">the customer to check on</param>
        /// <returns>the number of parcels the customer send and not delievered</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public int ParcelsInTheWayToCustomer(int customerId)
        {
            return DataSource.parcels.Count(h => h.TargetId == customerId && h.PickedUp != null);

        }
    }
}

