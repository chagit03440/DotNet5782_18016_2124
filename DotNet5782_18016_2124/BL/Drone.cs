﻿using System;
using System.Collections.Generic;
using System.Text;

namespace IBL.BO
{
    public class Drone
    {
        public int Id { get; set; }
        public String Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public DroneStatuses Status { get; set; }
        public double Battery { get; set; }
        public Location Location { get; set; }
        public PackageInTransfer Package { get; set; }
        public override String ToString()
        {
            String result = "";
            result += $"Id is {Id},\n";
            result += $"Model is {Model},\n";
            result += $"MaxWeight is {MaxWeight},\n";
            result += $"Status is {Status},\n";
            result += $"Battery is {Battery},\n";
            result += $"Location is {Location},\n";
            result += $"Package In Transfer is {Package},\n";

            return result;
        }

    }
}