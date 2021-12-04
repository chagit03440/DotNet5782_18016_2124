using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{


    public class ParcelForList
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Longitude { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatuses Status { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"SenderId is {SenderId},\n";
            result += $"TargetId is {TargetId},\n";
            result += $"Longitude is {Longitude},\n";
            result += $"Priority is {Priority},\n";
            result += $"Status is {Status},\n";

            return result;
        }
    }

}
