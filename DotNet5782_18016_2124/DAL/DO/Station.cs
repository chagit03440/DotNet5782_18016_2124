using System;
using System.Collections.Generic;
using System.Text;



    namespace DO
    {
        public struct Station
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public int ChargeSlots { get; set; }
            public override String ToString()
            {
                String result = "";
                result += $"Id is {ID},\n";
                result += $"Name is {Name},\n";
                result += $"Longitude is {Longitude},\n";
                result += $"Lattitude is {Lattitude},\n";
                result += $"ChargeSlots is {ChargeSlots},\n";
                return result;
            }

        }
    }

