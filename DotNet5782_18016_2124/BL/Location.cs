using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }


        public override String ToString()
        {
            String result = "";

            result += $"Longitude is {Longitude},\n";
            result += $"Lattitude is {Lattitude},\n";
            return result;
        }
    }

}
