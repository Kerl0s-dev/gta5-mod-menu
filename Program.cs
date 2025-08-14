using GTA;
using GTA.Native;
using Kerl0s_ModMenu.Data;
using Kerl0s_ModMenu.Managers;
using System;
using System.Windows.Forms;

public class Program : Script
{
    public static Ped player;
    public static Vehicle car;

    public Program()
    {
        MenuInitializer.Initialize();
        MenuManager.SetMenu("Menu Principal");

        Tick += OnTick;
        KeyDown += OnKeyDown;

        GTA.UI.Screen.ShowHelpText("Menu chargé, amuse-toi bien !");

        foreach (var veh in World.GetAllVehicles())
        {
            veh.Delete();
        }
    }

    private void OnTick(object sender, EventArgs e)
    {
        player = Game.Player.Character;
        car = Game.Player.Character.CurrentVehicle;

        CashManager.OnTick(10000);

        if (MenuManager.IsOpen)
        {
            MenuManager.Draw();
        }

        Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, player, MenuManager.isSuperSpeed ? 10.0f : 1.0f); // Modifie la vitesse de course du joueur
        Function.Call(Hash.SET_PED_MOVE_RATE_IN_WATER_OVERRIDE, player, MenuManager.isSuperSwim ? 3.0f : 1.0f); // Modifie la vitesse de nage du joueur

        player.IsInvincible = MenuManager.isGodMode;

        if (car != null)
        {
            car.EngineTorqueMultiplier = MenuManager.isSpeedBoost ? 1000000.0f : 1.0f;
            car.EnginePowerMultiplier = MenuManager.isSpeedBoost ? 1000000.0f : 1.0f;

            if (MenuManager.isRainbowPaint) VehicleManager.ApplyRainbowPaint(car);
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F4) { MenuManager.ToggleMenu(); Audio.PlaySoundFrontendAndForget("NO", "HUD_FRONTEND_DEFAULT_SOUNDSET"); };

        if (e.KeyCode == Keys.K) GiveAllWeapons();

        if (!MenuManager.IsOpen) return; // Ne fait rien si le menu n'ai pas ouvert

        if (e.KeyCode == Keys.NumPad2) { MenuManager.CurrentMenu.SelectNext(); Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET"); }
        else if (e.KeyCode == Keys.NumPad8) { MenuManager.CurrentMenu.SelectPrevious(); Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET"); }
        else if (e.KeyCode == Keys.NumPad5) { MenuManager.CurrentMenu.ActivateSelected(); Audio.PlaySoundFrontendAndForget("OK", "HUD_FRONTEND_DEFAULT_SOUNDSET"); }

        if (MenuManager.CurrentMenu == MenuManager.Menus["Créer Véhicule"])
        {
            if (e.KeyCode == Keys.NumPad6)
            {
                Pagination.NextPage(VehicleDatabase.vehicles.Count);
                MenuManager.CurrentMenu.SelectedIndex = 0;
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            else if (e.KeyCode == Keys.NumPad4)
            {
                Pagination.PrevPage();
                MenuManager.CurrentMenu.SelectedIndex = 0;
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }

            MenuInitializer.UpdateVehicleSpawnerMenu();
        }
    }

    public static void GiveAllWeapons()
    {
        Ped player = Game.Player.Character;

        // Donne toutes les armes disponibles
        foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
        {
            Function.Call(Hash.GIVE_WEAPON_TO_PED, player, (uint)weapon, 9999, false, true);
        }
    }

}