using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace Kerl0s_ModMenu.Managers
{
    public static class VehicleManager
    {
        public static void SpawnVehicle(string modelName)
        {
            Model model = new Model(modelName);
            if (!model.IsValid || !model.IsInCdImage) return;

            model.Request(500);
            if (!model.IsLoaded) return;

            Ped player = Game.Player.Character;
            Vector3 pos = player.Position + player.ForwardVector * 5;
            Vehicle veh = World.CreateVehicle(model, pos);
            player.SetIntoVehicle(veh, VehicleSeat.Driver);
            EnableSeatbelt(true); // ceinture attachée
        }

        public static void EnableSeatbelt(bool state)
        {
            Ped player = Game.Player.Character;
            Function.Call(Hash.SET_PED_CAN_BE_KNOCKED_OFF_VEHICLE, player, state ? 1 : 0); // 1 = never
        }

        public static void MaxUpgradeVehicle(Vehicle veh)
        {
            if (veh == null || !veh.Exists()) return;

            Function.Call(Hash.SET_VEHICLE_MOD_KIT, veh, 0);
            for (int i = 0; i < 50; i++)
            {
                int modCount = Function.Call<int>(Hash.GET_NUM_VEHICLE_MODS, veh, i);
                if (modCount > 0)
                {
                    Function.Call(Hash.SET_VEHICLE_MOD, veh, i, modCount - 1, false);
                }
            }

            Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 18, true); // Turbo
            Function.Call(Hash.TOGGLE_VEHICLE_MOD, veh, 22, true); // Xenon Lights
        }

        private static int rainbowTick = 0;

        public static void ApplyRainbowPaint(Vehicle vehicle)
        {
            int r = (int)(Math.Sin(rainbowTick * 0.1) * 127 + 128);
            int g = (int)(Math.Sin(rainbowTick * 0.1 + 2) * 127 + 128);
            int b = (int)(Math.Sin(rainbowTick * 0.1 + 4) * 127 + 128);

            vehicle.Mods.CustomPrimaryColor = System.Drawing.Color.FromArgb(r, g, b);
            vehicle.Mods.CustomSecondaryColor = System.Drawing.Color.FromArgb(r, g, b);

            rainbowTick++;
        }

        public static void DriveToWaypoint()
        {
            Ped player = Game.Player.Character;

            if (!player.IsInVehicle()) return;
            Vehicle veh = player.CurrentVehicle;

            if (World.WaypointBlip == null)
            {
                GTA.UI.Screen.ShowSubtitle("~r~Aucun marqueur n'a été placé !");
                return;
            }

            Vector3 dest = World.WaypointPosition;

            Function.Call(Hash.TASK_VEHICLE_DRIVE_TO_COORD, player, veh, dest.X, dest.Y, dest.Z, 30f, 1f, veh.GetHashCode(), 786603, 1f, true);
            GTA.UI.Screen.ShowSubtitle("~g~Direction le marqueur !");
        }
    }
}
