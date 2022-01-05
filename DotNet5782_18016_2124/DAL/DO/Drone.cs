using System;
using System.Collections.Generic;
using System.Text;



    namespace DO
    {
        public struct Drone
        {
            public int ID { get; set; }
            public String Model { get; set; }
            public WeightCategories MaxWeight { get; set; }
            //public DroneStatuses Status { get; set; }
            //public double Battery { get; set; }
            public override String ToString()
            {
                String result = "";
                result += $"Id is {ID},\n";
                result += $"Model is {Model},\n";
                result += $"MaxWeight is {MaxWeight},\n";
                //result += $"Status is {Status},\n";
                //result += $"Battery is {Battery},\n";
                return result;
            }


        }
    }


