using GTA;
using GTA.UI;
using Kerl0s_ModMenu.Data;
using System;
using System.Collections.Generic;

namespace Kerl0s_ModMenu.Managers
{
    public static class MenuInitializer
    {
        private static bool initialized = false;

        public static void Initialize()
        {
            if (initialized) return;
            initialized = true;

            VehicleDatabase.LoadVehicles();

            InitializeMain();
            InitializePlayer();
            InitializeVehicle();
            InitializeWorld();
            InitializeMiscellaneous();
        }

        private static void InitializeMain()
        {
            MenuManager.menus["Main"] = new List<string> { "Player", "Vehicle", "Teleport To Marker", "World", "Miscellaneous" };
            MenuManager.menuActions["Main"] = new List<Action>
            {
                () => MenuManager.SetMenu("Player"),
                () => MenuManager.SetMenu("Vehicle"),
                () => TeleportManager.TeleportToMarker(),
                () => MenuManager.SetMenu("World"),
                () => MenuManager.SetMenu("Miscellaneous")
            };
        }

        private static void InitializePlayer()
        {
            MenuManager.menus["Player"] = AddReturn("God Mod ~r~OFF", "Super Speed ~r~OFF", "Super Swim ~r~OFF");
            MenuManager.menuActions["Player"] = AddReturnAction("Main",
                () => ToggleOption(ref MenuManager.IsGodMod, "Player", 0, "God Mod"),
                () => ToggleOption(ref MenuManager.IsSuperSpeed, "Player", 1, "Super Speed"),
                () => ToggleOption(ref MenuManager.IsSuperSwim, "Player", 2, "Super Swim")
            );
        }

        private static void InitializeVehicle()
        {
            MenuManager.menus["Vehicle"] = AddReturn(
                "Vehicle Spawner",
                "Fix Vehicle",
                "Delete All Vehicles",
                "Speed Boost ~r~OFF",
                "Speed O Meter ~r~OFF",
                "Seatbelt ~r~OFF",
                "Invincible ~r~OFF",
                "Max Upgrade",
                "Rainbow Paint ~r~OFF",
                "Autodrive to Waypoint"
            );

            MenuManager.menuActions["Vehicle"] = AddReturnAction("Main",
                () => MenuManager.SetMenu("Vehicle Spawner"),
                () => { if (Game.Player.Character.CurrentVehicle != null) Game.Player.Character.CurrentVehicle.Repair(); else Notification.PostTicker("~r~You aren't in a vehicle", true); },
                () => { Vehicle[] vehicles = World.GetAllVehicles(); foreach (var vehicle in vehicles) { if (vehicle != Game.Player.Character.CurrentVehicle) vehicle.Delete(); } },
                () => ToggleOption(ref MenuManager.IsSpeedBoost, "Vehicle", 3, "Speed Boost"),
                () => ToggleOption(ref MenuManager.IsSpeedOMeter, "Vehicle", 4, "Speed O Meter"),
                () => { MenuManager.IsSeatbeltOn = !MenuManager.IsSeatbeltOn; VehicleManager.EnableSeatbelt(MenuManager.IsSeatbeltOn); MenuManager.menus["Vehicle"][5] = $"Seatbelt {(MenuManager.IsSeatbeltOn ? "~g~ON" : "~r~OFF")}"; },
                () => { ToggleOption(ref MenuManager.IsInvincible, "Vehicle", 6, "Invincible"); Game.Player.Character.CurrentVehicle.IsInvincible = MenuManager.IsInvincible; },
                () => { if (Game.Player.Character.CurrentVehicle != null) VehicleManager.MaxUpgradeVehicle(Game.Player.Character.CurrentVehicle); else Notification.PostTicker("~r~You aren't in a vehicle", true); },
                () => { ToggleOption(ref MenuManager.IsRainbowPaint, "Vehicle", 8, "Rainbow Paint"); },
                () => VehicleManager.DriveToWaypoint()
            );

            MenuManager.menus["Vehicle Spawner"] = new List<string>();
            MenuManager.menuActions["Vehicle Spawner"] = new List<Action>();

            UpdateVehicleSpawnerMenu();
        }

        public static void UpdateVehicleSpawnerMenu()
        {
            var pageItems = Pagination.GetPage(VehicleDatabase.vehicles);
            MenuManager.menus["Vehicle Spawner"].Clear();
            MenuManager.menuActions["Vehicle Spawner"].Clear();

            foreach (var model in pageItems)
            {
                MenuManager.menus["Vehicle Spawner"].Add(model);
                MenuManager.menuActions["Vehicle Spawner"].Add(() => VehicleManager.SpawnVehicle(model));
            }

            MenuManager.menus["Vehicle Spawner"].Add("Back");
            MenuManager.menuActions["Vehicle Spawner"].Add(() => MenuManager.SetMenu("Main"));
        }

        private static void InitializeWorld()
        {
            
            MenuManager.menus["World"] =AddReturn("Change Time", "Change Weather");
            MenuManager.menuActions["World"] = AddReturnAction("Main",
                () => MenuManager.SetMenu("Time Menu"),
                () => MenuManager.SetMenu("Weather Menu")
            );

            MenuManager.menus["Time Menu"] = AddReturn("Morning", "Noon", "Afternoon", "Night", "Midnight");
            MenuManager.menuActions["Time Menu"] = AddReturnAction("World",
                () => WorldManager.SetTime(8, 0),
                () => WorldManager.SetTime(12, 0),
                () => WorldManager.SetTime(14, 0),
                () => WorldManager.SetTime(20, 0),
                () => WorldManager.SetTime(0, 0)
            );

            MenuManager.menus["Weather Menu"] = AddReturn("Extra Sunny", "Clear", "Clouds", "Smog", "Foggy", "Overcast", "Rain", "Thunder", "Clearing", "Neutral", "Snowing", "Blizzard", "Snowlight", "Xmas", "Halloween");
            MenuManager.menuActions["Weather Menu"] = AddReturnAction("World",
                () => WorldManager.SetWeather(Weather.ExtraSunny),
                () => WorldManager.SetWeather(Weather.Clear),
                () => WorldManager.SetWeather(Weather.Clouds),
                () => WorldManager.SetWeather(Weather.Smog),
                () => WorldManager.SetWeather(Weather.Foggy),
                () => WorldManager.SetWeather(Weather.Overcast),
                () => WorldManager.SetWeather(Weather.Raining),
                () => WorldManager.SetWeather(Weather.ThunderStorm),
                () => WorldManager.SetWeather(Weather.Clearing),
                () => WorldManager.SetWeather(Weather.Neutral),
                () => WorldManager.SetWeather(Weather.Snowing),
                () => WorldManager.SetWeather(Weather.Blizzard),
                () => WorldManager.SetWeather(Weather.Snowlight),
                () => WorldManager.SetWeather(Weather.Christmas),
                () => WorldManager.SetWeather(Weather.Halloween)
            );
        }

        private static void InitializeMiscellaneous()
        {
            MenuManager.menus["Miscellaneous"] = AddReturn(
                "Night Vision ~r~OFF",
                "Heat Vision ~r~OFF",
                "HUD ~g~ON"
            );

            MenuManager.menuActions["Miscellaneous"] = AddReturnAction("Main",
                () => { ToggleOption(ref MenuManager.IsNightVision, "Miscellaneous", 0, "Night Vision"); },
                () => { ToggleOption(ref MenuManager.IsHeatVision, "Miscellaneous", 1, "Heat Vision"); },
                () => { ToggleOption(ref MenuManager.IsHudActive, "Miscellaneous", 2, "HUD"); }
            );
        }

        public static void ToggleOption(ref bool flag, string menuKey, int optionIndex, string baseLabel)
        {
            flag = !flag;
            string state = flag ? "~g~ON" : "~r~OFF";
            MenuManager.menus[menuKey][optionIndex] = $"{baseLabel} {state}";
        }

        private static List<string> AddReturn(params string[] options)
            => new List<string>(options) { "Back" };

        private static List<Action> AddReturnAction(string BackMenu, params Action[] actions)
            => new List<Action>(actions) { () => MenuManager.SetMenu(BackMenu) };
    }
}