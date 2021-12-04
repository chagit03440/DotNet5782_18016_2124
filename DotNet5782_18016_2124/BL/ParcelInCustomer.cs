using System;
using System.Collections.Generic;
using System.Text;


namespace IBL.BO
{


    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Longitude { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Longitude is {Longitude},\n";
            result += $"Priority is {Priority},\n";
            result += $"Status is {Status},\n";
            result += $"Sender is {Sender},\n";
            result += $"Target is {Target},\n";

            return result;
        }
    }

}


