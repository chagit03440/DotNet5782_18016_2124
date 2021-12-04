using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int ChargeSlots { get; set; }
        public List<DroneInCharging> Drones { get; set; }
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Name is {Name},\n";
            result += $"Location is {Location},\n";
            result += $"ChargeSlots is {ChargeSlots},\n";
            result += $"DroneP In charging is {Drones},\n";
            return result;
        }
    }
}
