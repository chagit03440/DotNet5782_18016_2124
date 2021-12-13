using System;
using System.Collections.Generic;
using System.Text;




namespace IBL.BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; }
        public Location Location { get; set; }
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Battery is {Battery},\n";
            result += $"DroneLocation is {Location},\n";

            return result;
        }

    }
}


