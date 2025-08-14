using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace Kerl0s_ModMenu.Managers
{
    internal class VehicleManager
    {
        public static void SpawnVehicle(string model)
        {
            if (string.IsNullOrWhiteSpace(model))
                return;

            Model vehicleModel = new Model(model);

            if (!vehicleModel.IsValid || !vehicleModel.IsVehicle)
                return;

            vehicleModel.Request(1000); // attend max 1 sec que le modèle se charge

            if (!vehicleModel.IsLoaded)
                return;

            // Position devant le joueur
            Vector3 spawnPos = Game.Player.Character.Position + Game.Player.Character.ForwardVector * 5;

            Vehicle veh = World.CreateVehicle(vehicleModel, spawnPos);
            
            if (veh != null)
            {
                veh.PlaceOnGround();
                veh.Mods.PrimaryColor = VehicleColor.MatteBlack;
                veh.Mods.SecondaryColor = VehicleColor.MatteBlack;
                Game.Player.Character.SetIntoVehicle(veh, VehicleSeat.Driver);
            }

            vehicleModel.MarkAsNoLongerNeeded();
        }

        public static void MaxUpgradeVehicle(Vehicle veh)
        {
            if (veh == null || !veh.Exists()) return;

            Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);

            // Upgrades classiques
            for (int i = 0; i < 50; i++)
            {
                int modCount = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, veh, i);
                if (modCount > 0)
                {
                    Function.Call(Hash.SET_VEHICLE_MOD, veh, i, modCount - 1, false);
                }
            }

            // Turbo et xenon
            Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 18, true); // Turbo
            Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 22, true); // Xenon Lights

            // Néons (lights sous la caisse)
            Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, veh, 255, 0, 255); // violet flashy (R,G,B)
            for (int i = 0; i < 4; i++)
            {
                Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, veh, i, true);
            }
        }

        private static int rainbowTick = 0;

        public static void ApplyRainbowPaint(Vehicle vehicle)
        {
            int r = (int)(Math.Sin(rainbowTick * 0.1) * 127 + 128);
            int g = (int)(Math.Sin(rainbowTick * 0.1 + 2) * 127 + 128);
            int b = (int)(Math.Sin(rainbowTick * 0.1 + 4) * 127 + 128);

            vehicle.Mods.CustomPrimaryColor = System.Drawing.Color.FromArgb(r, g, b);
            vehicle.Mods.CustomSecondaryColor = System.Drawing.Color.FromArgb(b, g, r);

            Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, vehicle, g, b, r);

            rainbowTick++;
        }
    }
}
