using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Name is {Name},\n";
            return result;
        }
    }

}