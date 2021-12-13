using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class CustomerForList
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public int ParcelsHeSendAndDelivered { get; set; }
        public int ParcelsHeSendAndNotDelivered { get; set; }
        public int ParcelsHeGot { get; set; }
        public int ParcelsInTheWayToCustomer { get; set; }
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Name is {Name},\n";
            result += $"Telephone is {Phone},\n";
            result += $"Num of parcels he send and delivered is {ParcelsHeSendAndDelivered},\n";
            result += $"Num of parcels he send and not delivered is {ParcelsHeSendAndNotDelivered},\n";
            result += $"Num of parcels he got is {ParcelsHeGot},\n";
            result += $"Num of parcels int the way to a customer is {ParcelsInTheWayToCustomer},\n";
            return result;
        }
    }
}
 