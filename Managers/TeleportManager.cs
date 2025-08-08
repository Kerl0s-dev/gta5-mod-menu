using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;

namespace Kerl0s_ModMenu.Managers
{
    public static class TeleportManager
    {
        public static void TeleportToMarker()
        {
            Ped playerPed = Game.Player.Character;
            Vehicle playerVehicle = Game.Player.Character.CurrentVehicle;
            Vector3 pos = World.WaypointPosition;

            if (World.WaypointBlip == null)
            {
                Notification.PostTicker("~r~MARKER NOT SET~n~~w~Please set a waypoint on the map", true);
                return;
            }

            Function.Call(Hash.DO_SCREEN_FADE_OUT, 250);
            Script.Wait(250);

            float groundZ;
            if (TryFindGroundZ(pos.X, pos.Y, out groundZ))
            {
                if (playerPed.IsInVehicle())
                    playerVehicle.Position = new Vector3(pos.X, pos.Y, groundZ);
                else
                    playerPed.Position = new Vector3(pos.X, pos.Y, groundZ);
            }
            else
            {
                // Téléportation à 1000 si le sol n’est pas trouvé
                if (playerPed.IsInVehicle())
                    playerVehicle.Position = new Vector3(pos.X, pos.Y, 1000f);
                else
                    playerPed.Position = new Vector3(pos.X, pos.Y, 1000f);
            }

            Script.Wait(250);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 250);
        }

        private static bool TryFindGroundZ(float x, float y, out float zResult)
        {
            unsafe
            {
                float z = 0f;
                for (float height = 0f; height <= 1500f; height += 50f)
                {
                    if (Game.Player.Character.IsInVehicle())
                        Game.Player.Character.CurrentVehicle.Position = new Vector3(x, y, height);
                    else
                        Game.Player.Character.Position = new Vector3(x, y, height);

                    Script.Wait(100);
                    if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, x, y, height, &z))
                    {
                        zResult = z;
                        return true;
                    }
                }
                zResult = 0f;
                return false;
            }
        }
    }
}