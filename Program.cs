using GTA;
using GTA.Native;
using Kerl0s_ModMenu.Data;
using Kerl0s_ModMenu.Managers;
using System;
using System.Windows.Forms;

public class Program : Script
{
    // Exposed as properties for clearer intent
    public static Ped Player { get; private set; }
    public static Vehicle Car { get; private set; }

    // Tunable constants
    private const float DefaultMoveRate = 1.0f;
    private const float SuperRunMultiplier = 10.0f;
    private const float SuperSwimMultiplier = 3.0f;
    private const float SpeedBoostMultiplier = 250.0f;
    private const int CashTickIntervalMs = 10000;

    public Program()
    {
        MenuInitializer.Initialize();
        MenuManager.SetMenu("Menu Principal");

        Tick += OnTick;
        KeyDown += OnKeyDown;

        GTA.UI.Screen.ShowHelpText("Menu chargé, amuse-toi bien !");

        ClearWorldVehicles();
    }

    private void OnTick(object sender, EventArgs e)
    {
        UpdatePlayerAndVehicleReferences();

        CashManager.OnTick(CashTickIntervalMs);

        if (MenuManager.IsOpen)
        {
            MenuManager.Draw();
        }

        ApplyMovementOverrides();
        ApplyPlayerInvincibility();
        ApplyVehicleModifiers();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F4)
        {
            MenuManager.ToggleMenu();
            PlayUiSound("NO");
            return;
        }

        if (e.KeyCode == Keys.K)
        {
            GiveAllWeapons();
            return;
        }

        if (!MenuManager.IsOpen) return; // ignore navigation when menu closed

        if (e.KeyCode == Keys.NumPad2)
        {
            MenuManager.CurrentMenu?.SelectNext();
            PlayUiSound("NAV_UP_DOWN");
        }
        else if (e.KeyCode == Keys.NumPad8)
        {
            MenuManager.CurrentMenu?.SelectPrevious();
            PlayUiSound("NAV_UP_DOWN");
        }
        else if (e.KeyCode == Keys.NumPad5)
        {
            MenuManager.CurrentMenu?.ActivateSelected();
            PlayUiSound("OK");
        }

        if (MenuManager.CurrentMenu == MenuManager.Menus["Créer Véhicule"])
        {
            if (e.KeyCode == Keys.NumPad6)
            {
                Pagination.NextPage(VehicleDatabase.vehicles.Count);
                MenuManager.CurrentMenu.SelectedIndex = 0;
                PlayUiSound("NAV_UP_DOWN");
            }
            else if (e.KeyCode == Keys.NumPad4)
            {
                Pagination.PrevPage();
                MenuManager.CurrentMenu.SelectedIndex = 0;
                PlayUiSound("NAV_UP_DOWN");
            }

            MenuInitializer.UpdateVehicleSpawnerMenu();
        }
    }

    /// <summary>
    /// Gives every weapon enum value to the player. Exceptions are ignored to avoid crashes from invalid enum values.
    /// </summary>
    public static void GiveAllWeapons()
    {
        var playerPed = Game.Player.Character;

        foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
        {
            try
            {
                Function.Call(Hash.GIVE_WEAPON_TO_PED, playerPed, (uint)weapon, 9999, false, true);
            }
            catch
            {
                // ignore invalid/unsupported enum values
            }
        }
    }

    #region Helpers

    private void UpdatePlayerAndVehicleReferences()
    {
        Player = Game.Player?.Character;
        Car = Player?.CurrentVehicle;
    }

    private void ApplyMovementOverrides()
    {
        if (Player == null) return;

        Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Player, MenuManager.IsSuperSpeed ? SuperRunMultiplier : DefaultMoveRate);
        Function.Call(Hash.SET_PED_MOVE_RATE_IN_WATER_OVERRIDE, Player, MenuManager.IsSuperSwim ? SuperSwimMultiplier : DefaultMoveRate);
    }

    private void ApplyPlayerInvincibility()
    {
        if (Player == null) return;
        Player.IsInvincible = MenuManager.IsGodMode;
    }

    private void ApplyVehicleModifiers()
    {
        if (Car == null) return;

        var multiplier = MenuManager.IsSpeedBoost ? SpeedBoostMultiplier : DefaultMoveRate;
        Car.EngineTorqueMultiplier = multiplier;
        Car.EnginePowerMultiplier = multiplier;

        if (MenuManager.IsRainbowPaint) VehicleManager.ApplyRainbowPaint(Car);
    }

    private void ClearWorldVehicles()
    {
        foreach (var veh in World.GetAllVehicles())
        {
            try
            {
                veh.Delete();
            }
            catch
            {
                // best-effort deletion
                GTA.UI.Screen.ShowSubtitle("Erreur lors de la suppression d'un véhicule");
            }
        }
    }

    private void PlayUiSound(string soundName, string soundset = "HUD_FRONTEND_DEFAULT_SOUNDSET")
    {
        Audio.PlaySoundFrontendAndForget(soundName, soundset);
    }

    #endregion
}