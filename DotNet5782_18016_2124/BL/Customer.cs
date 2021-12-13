using System;
using System.Collections.Generic;
using System.Text;




namespace IBL.BO
{
    public class Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelInCustomer> ParcelsFromTheCustomer { get; set; }
        public List<ParcelInCustomer> ParcelsToTheCustomer { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Name is {Name},\n";
            result += $"Telephone is {Phone},\n";
            result += Location.ToString();
            result += $"Parcels from the customer is i{ParcelsFromTheCustomer},\n";
            result += $"Parcels to the customer is {ParcelsToTheCustomer},\n";


            return result;
        }
    }
}

