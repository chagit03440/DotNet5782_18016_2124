using System;
using System.Collections.Generic;
using System.Text;

namespace BO
{
    public class DroneInCharging
    {
        public int Id { get; set; }
        public int Battery { get; set; }

        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Battery is {Battery},\n";

            return result;
        }

    }
}