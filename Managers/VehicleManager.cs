using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Drawing;

namespace Kerl0s_ModMenu.Managers
{
    public static class VehicleManager
    {
        private const int ModelRequestTimeoutMs = 1000;
        private const float SpawnForwardOffset = 5.0f;
        private const int NeonR = 255;
        private const int NeonG = 0;
        private const int NeonB = 255;

        private static int _rainbowTick = 0;

        public static void SpawnVehicle(string model)
        {
            if (string.IsNullOrWhiteSpace(model)) return;

            var vehicleModel = new Model(model);
            if (!vehicleModel.IsValid || !vehicleModel.IsVehicle) return;

            vehicleModel.Request(ModelRequestTimeoutMs);
            if (!vehicleModel.IsLoaded) return;

            try
            {
                var player = Game.Player.Character;
                Vector3 spawnPos = player.Position + player.ForwardVector * SpawnForwardOffset;

                var veh = World.CreateVehicle(vehicleModel, spawnPos);
                if (veh != null)
                {
                    veh.PlaceOnGround();
                    veh.Mods.PrimaryColor = VehicleColor.MatteBlack;
                    veh.Mods.SecondaryColor = VehicleColor.MatteBlack;
                    player.SetIntoVehicle(veh, VehicleSeat.Driver);
                }
            }
            catch
            {
                // Ignore spawn failures
            }
            finally
            {
                vehicleModel.MarkAsNoLongerNeeded();
            }
        }

        public static void MaxUpgradeVehicle(Vehicle veh)
        {
            if (veh == null || !veh.Exists()) return;

            try
            {
                Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);

                // Apply best available mod for each mod type
                for (int modType = 0; modType < 50; modType++)
                {
                    int modCount = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, veh, modType);
                    if (modCount > 0)
                    {
                        Function.Call(Hash.SET_VEHICLE_MOD, veh, modType, modCount - 1, false);
                    }
                }

                // Turbo and xenon
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 18, true); // Turbo
                Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 22, true); // Xenon Lights

                // Neon lights (underbody)
                Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, veh, NeonR, NeonG, NeonB);
                for (int i = 0; i < 4; i++)
                {
                    Function.Call(Hash.SET_VEHICLE_NEON_ENABLED, veh, i, true);
                }
            }
            catch
            {
                // Best-effort: ignore upgrade failures
            }
        }

        public static void ApplyRainbowPaint(Vehicle vehicle)
        {
            if (vehicle == null || !vehicle.Exists()) return;

            try
            {
                int r = (int)(Math.Sin(_rainbowTick * 0.1) * 127 + 128);
                int g = (int)(Math.Sin(_rainbowTick * 0.1 + 2) * 127 + 128);
                int b = (int)(Math.Sin(_rainbowTick * 0.1 + 4) * 127 + 128);

                vehicle.Mods.CustomPrimaryColor = Color.FromArgb(r, g, b);
                vehicle.Mods.CustomSecondaryColor = Color.FromArgb(b, g, r);

                Function.Call(Hash.SET_VEHICLE_NEON_COLOUR, vehicle, g, b, r);
            }
            catch
            {
                // Ignore paint failures
            }
            finally
            {
                _rainbowTick++;
            }
        }
    }
}
