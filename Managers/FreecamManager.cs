using GTA;
using GTA.Math;
using System.Windows.Forms;

namespace Kerl0s_ModMenu.Managers
{
    public static class FreecamManager
    {
        private static Camera cam;
        public static bool isActive = false;
        public static float moveSpeed = 0.5f;

        public static void ToggleFreecam()
        {
            if (isActive)
            {
                ScriptCameraDirector.StopRendering();
                Game.Player.SetControlState(true, SetPlayerControlFlags.LeaveCameraControlOn);
                isActive = false;
                cam.Delete(); // Nettoyage de la caméra
                GTA.UI.Screen.ShowSubtitle("Freecam désactivée", 2000);
                return;
            }

            Ped player = Game.Player.Character;

            cam = Camera.Create(ScriptedCameraNameHash.DefaultScriptedCamera, true);
            cam.Position = player.Position + new Vector3(0, 0, 1); // Légèrement devant le joueur
            cam.Rotation = new Vector3(0, 0, 0);

            ScriptCameraDirector.StartRendering();
            Game.Player.SetControlState(false, SetPlayerControlFlags.LeaveCameraControlOn);
            isActive = true;

            GTA.UI.Screen.ShowSubtitle("Freecam activée", 2000);
        }

        public static void Update()
        {
            if (!isActive || cam == null) return;

            Vector3 pos = cam.Position;
            Vector3 rot = cam.Rotation;

            // Mouvement
            if (Game.IsKeyPressed(Keys.NumPad8)) pos += cam.Direction * moveSpeed;
            if (Game.IsKeyPressed(Keys.NumPad2)) pos -= cam.Direction * moveSpeed;
            if (Game.IsKeyPressed(Keys.NumPad4)) pos -= cam.RightVector * moveSpeed;
            if (Game.IsKeyPressed(Keys.NumPad6)) pos += cam.RightVector * moveSpeed;
            if (Game.IsKeyPressed(Keys.Add)) pos.Z += moveSpeed;
            if (Game.IsKeyPressed(Keys.NumPad0)) pos.Z -= moveSpeed;

            // Rotation
            if (Game.IsKeyPressed(Keys.NumPad7)) rot.Z -= 1f;
            if (Game.IsKeyPressed(Keys.NumPad9)) rot.Z += 1f;
            if (Game.IsKeyPressed(Keys.NumPad1)) rot.X -= 1f;
            if (Game.IsKeyPressed(Keys.NumPad3)) rot.X += 1f;

            cam.Position = pos;
            cam.Rotation = rot;
        }
    }
}
