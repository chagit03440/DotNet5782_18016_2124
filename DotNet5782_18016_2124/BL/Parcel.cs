using System;
using System.Collections.Generic;
using System.Text;


namespace IBL.BO
{


    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Longitude { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel DroneP { get; set; }
        public DateTime? CreationTime { get; set; }
        public int AssociationTime { get; set; }
        public int CollectionTime { get; set; }
        public int SupplyTime { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Sender is {Sender},\n";
            result += $"Target is {Target},\n";
            result += $"Longitude is {Longitude},\n";
            result += $"Priority is {Priority},\n";
            result += $"Drone P in parcel is {DroneP},\n";
            result += $"Creation time is {CreationTime},\n";
            result += $"AssociationTime {AssociationTime},\n";
            result += $"CollectionTime {CollectionTime},\n";
            result += $"SupplyTime {SupplyTime},\n";

            return result;
        }
    }

}

