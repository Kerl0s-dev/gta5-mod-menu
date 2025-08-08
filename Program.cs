using GTA;
using GTA.Native;
using GTA.UI;
using Kerl0s_ModMenu.Data;
using Kerl0s_ModMenu.Managers;
using Kerl0s_ModMenu.UI;
using System;
using System.Collections.Generic;

public class Program : Script
{
    public static Dictionary<string, List<string>> menus = new Dictionary<string, List<string>>();
    public static Dictionary<string, List<Action>> menuActions = new Dictionary<string, List<Action>>();

    public Program()
    {
        Tick += OnTick;
        KeyDown += InputManager.HandleInput;

        MenuManager manager = new MenuManager();

        Screen.ShowHelpText("Kerl0s Mod Menu chargé avec succès");
    }

    bool wasDead = false;

    private void OnTick(object sender, EventArgs e)
    {
        var player = Game.Player.Character;
        var currentVehicle = player.CurrentVehicle;

        if (MenuManager.MenuOpen)
        {
            MenuManager.Draw();
        }

        FreecamManager.Update();

        player.IsInvincible = MenuManager.IsGodMod;
        Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, player, (MenuManager.IsSuperSpeed ? 10.0f : 1.0f));
        Function.Call(Hash.SET_PED_MOVE_RATE_IN_WATER_OVERRIDE, player, (MenuManager.IsSuperSwim ? 3.0f : 1.0f));
        Function.Call(Hash.SET_SWIM_MULTIPLIER_FOR_PLAYER, player, (MenuManager.IsSuperSwim ? 10.0f : 1.0f));

        Function.Call(Hash.SET_NIGHTVISION, MenuManager.IsNightVision);
        Function.Call(Hash.SET_SEETHROUGH, MenuManager.IsHeatVision);

        // Active/Désactive le HUD
        Function.Call(Hash.DISPLAY_HUD, MenuManager.IsHudActive);
        Function.Call(Hash.DISPLAY_RADAR, MenuManager.IsHudActive);

        if (currentVehicle != null)
        {
            currentVehicle.EnginePowerMultiplier = (MenuManager.IsSpeedBoost ? 500 : 1);
            currentVehicle.EngineTorqueMultiplier = (MenuManager.IsSpeedBoost ? 500 : 1);
        }

        if (MenuManager.IsSpeedOMeter && Game.Player.Character.IsInVehicle())
        {
            var veh = Game.Player.Character.CurrentVehicle;
            float speed = veh.Speed * 3.6f; // m/s → km/h
            UIDrawer.DrawSpeedometer(speed);
        }

        // Rainbow paint
        if (MenuManager.IsRainbowPaint)
        {
            var veh = Game.Player.Character.CurrentVehicle;
            if (veh != null && veh.Exists())
                VehicleManager.ApplyRainbowPaint(veh);
        }

        if (MenuManager.currentMenu == "Vehicle Spawner")
            Screen.ShowSubtitle($"Page {Pagination.PageIndex + 1} / {Pagination.TotalPages(VehicleDatabase.vehicles.Count)}", 10);
    }
}