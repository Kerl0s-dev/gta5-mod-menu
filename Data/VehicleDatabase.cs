using GTA;
using System;
using System.Collections.Generic;

namespace Kerl0s_ModMenu.Data
{
    internal static class VehicleDatabase
    {
        // Keep existing field for compatibility with callers in the codebase
        public static List<string> vehicles = new List<string>();

        // Expose read-only view for new code
        public static IReadOnlyList<string> Vehicles => vehicles;

        public static void LoadVehicles()
        {
            var list = new List<string>();
            foreach (VehicleHash vh in Enum.GetValues(typeof(VehicleHash)))
            {
                list.Add(vh.ToString());
            }
            vehicles = list;
        }
    }
}
