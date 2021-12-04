using System;
using System.Collections.Generic;
using System.Text;

namespace IDAL
{
    namespace DO
    {
        public struct Customer
        {
            public int ID { get; set; }
            public String Name { get; set; }
            public String Phone { get; set; }
            public double Longitude { get; set; }
            public double Lattitude { get; set; }
            public override String ToString()
            {
                String result = "";
                result += $"Id is {ID},\n";
                result += $"Name is {Name},\n";
                result += $"Telephone is {Phone},\n";
                result += $"Longitude is {Longitude},\n";
                result += $"Lattitude is {Lattitude},\n";
                return result;
            }
        }
    }

}
