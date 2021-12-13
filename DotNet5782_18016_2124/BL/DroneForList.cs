using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class DroneForList
    {
        public int Id { get; set; }
        public String Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatuses Status { get; set; }
        public double Battery { get; set; }
        public Location DroneLocation { get; set; }
        public int ParcelId { get; set; }
        //public int DeliveryId { get; internal set; }
        
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Model is {Model},\n";
            result += $"MaxWeight is {MaxWeight},\n";
            result += $"Status is {Status},\n";
            result += $"Battery is {Battery},\n";
            result += $"DroneLocation is {DroneLocation.ToString()},\n";
            result += $"Parcel Id is {ParcelId},\n";

            return result;
        }

    }
}
