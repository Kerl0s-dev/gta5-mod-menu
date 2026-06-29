using GTA;
using GTA.Native;
using Kerl0s_ModMenu.Data;
using Kerl0s_ModMenu.Managers;
using Kerl0s_ModMenu.Utils.UI;
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
    private const float SpeedBoostMultiplier = 1000.0f;

    public Program()
    {
        MenuInitializer.Initialize();
        MenuManager.SetMenu(MenuManager.Menus["Menu Principal"]);

        Tick += OnTick;
        KeyDown += OnKeyDown;

        GTA.UI.Screen.ShowHelpText("~g~Menu chargé~w~");
    }

    private void OnTick(object sender, EventArgs e)
    {
        UpdatePlayerAndVehicleReferences();

        if (MenuManager.IsOpen)
        {
            MenuManager.Draw();
        }

        ApplyPlayerModifiers();
        ApplyVehicleModifiers();
        ApplyWeaponModifiers();
        ApplyWorldModifiers();
        ApplyExtraModifiers();
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F4)
        {
            MenuManager.ToggleMenu();
            PlayUISound("NO");
            return;
        }

        if (!MenuManager.IsOpen) return; // ignore navigation when menu closed

        if (e.KeyCode == Keys.NumPad2)
        {
            PlayUISound("NAV_UP_DOWN");
            MenuManager.CurrentMenu?.SelectNext();
        }
        else if (e.KeyCode == Keys.NumPad8)
        {
            PlayUISound("NAV_UP_DOWN");
            MenuManager.CurrentMenu?.SelectPrevious();
        }
        else if (e.KeyCode == Keys.NumPad5)
        {
            PlayUISound("OK");
            MenuManager.CurrentMenu?.ActivateSelected();
        }

        if (MenuManager.CurrentMenu == MenuManager.Menus["Créer un véhicule"])
        {
            int totalItems = VehicleDatabase.Vehicles.Count;

            if (e.KeyCode == Keys.NumPad4)
            {
                Pagination.PrevPage();
                MenuInitializer.UpdateCreateVehicleMenu();
            }

            if (e.KeyCode == Keys.NumPad6)
            {
                Pagination.NextPage(totalItems);
                MenuInitializer.UpdateCreateVehicleMenu();
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
        Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Player, MenuManager.IsSuperSpeed ? SuperRunMultiplier : DefaultMoveRate);
        Function.Call(Hash.SET_PED_MOVE_RATE_IN_WATER_OVERRIDE, Player, MenuManager.IsSuperSwim ? SuperSwimMultiplier : DefaultMoveRate);
    }

    private void ApplyPlayerModifiers()
    {
        if (Player == null) return;
        
        Player.IsInvincible = MenuManager.IsGodMode;
        Player.CanRagdoll = !MenuManager.IsGodMode;

        if (MenuManager.IsInfiniteAbility) Game.Player.RefillSpecialAbility();

        ApplyMovementOverrides();
    }

    private void ApplyVehicleModifiers()
    {
        if (Car == null) return;

        var multiplier = MenuManager.IsSpeedBoost ? SpeedBoostMultiplier : DefaultMoveRate;

        Car.EngineTorqueMultiplier = multiplier;
        Car.EnginePowerMultiplier = multiplier;

        Car.Turbo = multiplier;
        Car.ThrottlePower = multiplier;

        if (MenuManager.IsSpeedBoost)
        {
            Car.MaxSpeed = multiplier;
        }
        else { Car.MaxSpeed = 250; }

        if (MenuManager.IsInvisible) { Car.IsVisible = false; Player.IsVisible = true; }
        else { Car.IsVisible = true; }
    }

    private void ApplyWeaponModifiers()
    {
        if (MenuManager.IsInfiniteAmmo)
        {
            Game.Player.Character.Weapons.Current.InfiniteAmmo = true;
        }

        if (MenuManager.IsNoReload)
        {
            Game.Player.Character.Weapons.Current.InfiniteAmmoClip = true;
        }

        if (MenuManager.IsExplosiveBullets)
        {
            Function.Call(Hash.SET_EXPLOSIVE_AMMO_THIS_FRAME, Game.Player);
        }

        if (MenuManager.IsExplosiveMelee)
        {
            Function.Call(Hash.SET_EXPLOSIVE_MELEE_THIS_FRAME, Game.Player);
        }

        if (MenuManager.IsIncendiaryBullets)
        {
            Function.Call(Hash.SET_FIRE_AMMO_THIS_FRAME, Game.Player);
        }
    }

    private void ApplyWorldModifiers()
    {
        if (MenuManager.IsNoPeds)
        {
            foreach (var ped in World.GetAllPeds())
            {
                if (ped != Player)
                {
                    ped.Delete();
                }
            }
        }

        if (MenuManager.IsNoTraffic)
        {
            foreach (var vehicle in World.GetAllVehicles())
            {
                if (vehicle != Car)
                {
                    vehicle.Delete();
                }
            }
        }

        Function.Call(Hash.SET_POLICE_IGNORE_PLAYER, Game.Player, MenuManager.IsNoPolice);
        Function.Call(Hash.SET_DISPATCH_COPS_FOR_PLAYER, Game.Player, !MenuManager.IsNoPolice);
        if (MenuManager.IsNoPolice) { Game.Player.WantedLevel = 0; Function.Call(Hash.SET_MAX_WANTED_LEVEL, 0); }
        else { Function.Call(Hash.SET_MAX_WANTED_LEVEL, 5); }
    }

    private void ApplyExtraModifiers()
    {
        if (!MenuManager.IsHudActive) { Function.Call(Hash.HIDE_HUD_AND_RADAR_THIS_FRAME); }
        Function.Call(Hash.SET_NIGHTVISION, MenuManager.IsNightVision);
        Function.Call(Hash.SET_SEETHROUGH, MenuManager.IsThermalVision);

        if (MenuManager.IsSpeedometer)
        {
            if (Car != null)
            {
                UIDrawer.DrawSpeedometer(Car.Speed);
            }
            else
            {
                UIDrawer.DrawSpeedometer(Player.Speed);
            }
        }
    }

    private void PlayUISound(string soundName, string soundset = "HUD_FRONTEND_DEFAULT_SOUNDSET")
    {
        Audio.PlaySoundFrontend(soundName, soundset);
    }

    #endregion
}