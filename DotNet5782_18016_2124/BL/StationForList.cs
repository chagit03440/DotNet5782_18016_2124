using System;
using System.Collections.Generic;
using System.Text;


namespace IBL.BO
{
    public class StationForList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableChargeSlots { get; set; }
        public int NotAvailableChargeSlots { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Name is {Name},\n";
            result += $"Available chargeSlots is {AvailableChargeSlots},\n";
            result += $"Not available chargeSlots is {NotAvailableChargeSlots},\n";
            return result;
        }
    }
}