using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct DroneCharge
        {
            public int DroneId { get; set; }
            public int StationId { get; set; }
            public override String ToString()
            {
                String result = "";
                result += $"DroneId is {DroneId},\n";
                result += $"StationId is {StationId},\n";
                return result;
            }
        }
    }

}