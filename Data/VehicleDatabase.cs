using GTA;
using System;
using System.Collections.Generic;

namespace Kerl0s_ModMenu.Data
{
    internal class VehicleDatabase
    {
        public static List<string> vehicles;

        public static void LoadVehicles()
        {
            vehicles = new List<string>();
            foreach (VehicleHash vehicle in Enum.GetValues(typeof(VehicleHash)))
            {
                vehicles.Add(vehicle.ToString());
            }
        }
    }
}
