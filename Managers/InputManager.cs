using GTA;
using Kerl0s_ModMenu.Data;
using System.Windows.Forms;

namespace Kerl0s_ModMenu.Managers
{
    public static class InputManager
    {
        public static void HandleInput(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) { FreecamManager.ToggleFreecam(); }

            if (e.KeyCode == Keys.F4) { MenuManager.ToggleMenu(); }

            if (!MenuManager.MenuOpen) return;

            if (e.KeyCode == Keys.NumPad8)
            {
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                MenuManager.NavigateUp();
            }

            else if (e.KeyCode == Keys.NumPad2)
            {
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                MenuManager.NavigateDown();
            }
            else if (e.KeyCode == Keys.NumPad5)
            {
                Audio.PlaySoundFrontendAndForget("OK", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                MenuManager.Select();
            }

            if (MenuManager.currentMenu == "Vehicle Spawner")
            {
                if (e.KeyCode == Keys.NumPad4) // Page précédente
                {
                    Pagination.PrevPage();
                    MenuInitializer.UpdateVehicleSpawnerMenu();
                    Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }
                else if (e.KeyCode == Keys.NumPad6) // Page suivante
                {
                    Pagination.NextPage(VehicleDatabase.vehicles.Count);
                    MenuInitializer.UpdateVehicleSpawnerMenu();
                    Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                }
            }
        }
    }
}
