using GTA;
using GTA.Native;
using Kerl0s_ModMenu.Data;
using System;
using System.Drawing;

namespace Kerl0s_ModMenu.Managers
{
    public static class MenuInitializer
    {
        public static void Initialize()
        {
            VehicleDatabase.LoadVehicles();

            CreateMenus();
            UpdateVehicleSpawnerMenu();
            UpdateTimeMenu();
            UpdateWeatherMenu();
        }

        private static void CreateMenus()
        {
            var mainMenu = new Menu("Menu Principal", Color.FromArgb(10, 0, 50),
                new[] { "Joueur", "Véhicule", "Monde", "HUD", "Téléporter Au Marqueur", "Donner de l'argent", "Enlever la police" },
                new Action[] {
                    () => MenuManager.SetMenu("Joueur"),
                    () => MenuManager.SetMenu("Véhicule"),
                    () => MenuManager.SetMenu("Monde"),
                    () => MenuManager.SetMenu("HUD"),
                    () => TeleportManager.TeleportToMarker(),
                    () => CashManager.ToggleCashRain(2500),
                    () => Game.Player.WantedLevel = 0
                }
            );

            var playerMenu = new Menu("Joueur", Color.FromArgb(255, 100, 0),
                new[] { "Mode Divin ~r~OFF", "Super Vitesse ~r~OFF", "Super Nage ~r~OFF", "Changer de Personnage", "~y~Retour" },
                new Action[] {
                    () => { MenuManager.ToggleOption(ref MenuManager.isGodMode, "Joueur", 0, "Mode Divin"); },
                    () => { MenuManager.ToggleOption(ref MenuManager.isSuperSpeed, "Joueur", 1, "Super Vitesse"); },
                    () => { MenuManager.ToggleOption(ref MenuManager.isSuperSwim, "Joueur", 2, "Super Nage"); },
                    () => MenuManager.SetMenu("Personnage"),
                    () => MenuManager.SetMenu("Menu Principal")
                }
            );

            var characterMenu = new Menu("Personnage", Color.Gold,
                new[] {"Franklin", "Michael", "Trevor", "Singe", "Cochon", "~y~Retour" },
                new Action[] {
                    () => Game.Player.ChangeModel(PedHash.Franklin),
                    () => Game.Player.ChangeModel(PedHash.Michael),
                    () => Game.Player.ChangeModel(PedHash.Trevor),
                    () => Game.Player.ChangeModel(PedHash.Chimp),
                    () => Game.Player.ChangeModel(PedHash.Pig),
                    () => MenuManager.SetMenu("Joueur"),
                }
            );

            var vehicleMenu = new Menu("Véhicule", Color.FromArgb(0, 100, 255),
                new[] { "Boost de Vitesse ~r~OFF", "Créer Véhicule", "Réparer le véhicule", "Améliorer au Maximum", "Mode Arc en ciel ~r~OFF", "~y~Retour" },
                new Action[]
                {
                    () => { MenuManager.ToggleOption(ref MenuManager.isSpeedBoost, "Véhicule", 0, "Boost de Vitesse"); },
                    () => MenuManager.SetMenu("Créer Véhicule"),
                    () => { if (Game.Player.Character.CurrentVehicle != null) Game.Player.Character.CurrentVehicle.Repair(); },
                    () => VehicleManager.MaxUpgradeVehicle(Game.Player.Character.CurrentVehicle),
                    () => MenuManager.ToggleOption(ref MenuManager.isRainbowPaint, "Véhicule", 4, "Mode Arc en ciel"),
                    () => MenuManager.SetMenu("Menu Principal")
                }
            );

            var spawnerMenu = new Menu("Créer Véhicule", Color.FromArgb(0, 150, 255),
                new[] { "" },
                new Action[] { () => { } }
            );

            var worldMenu = new Menu("Monde", Color.FromArgb(0, 50, 50),
                new[] { "Changer l'heure", "Changer la météo", "~y~Retour" },
                new Action[] {
                    () => MenuManager.SetMenu("Heure"),
                    () => MenuManager.SetMenu("Météo"),
                    () => MenuManager.SetMenu("Menu Principal")
                }
            );

            var timeMenu = new Menu("Heure", Color.DarkBlue,
                new[] { "" },
                new Action[] { () => { } }
            );

            var weatherMenu = new Menu("Météo", Color.DeepSkyBlue,
                new[] { "" },
                new Action[] { () => { } }
            );

            var hudMenu = new Menu("HUD", Color.FromArgb(100, 255, 0),
                new[] { "Afficher HUD ~g~ON", "~y~Retour" },
                new Action[] {
                    () => {
                        MenuManager.ToggleOption(ref MenuManager.hudActive, "HUD", 0, "Afficher HUD");
                        Function.Call(Hash.DISPLAY_HUD, MenuManager.hudActive);
                        Function.Call(Hash.DISPLAY_RADAR, MenuManager.hudActive);
                    },
                    () => MenuManager.SetMenu("Menu Principal")
                }
            );

            // Register menus
            MenuManager.Menus.Add("Menu Principal", mainMenu);
            MenuManager.Menus.Add("Joueur", playerMenu);
            MenuManager.Menus.Add("Personnage", characterMenu);
            MenuManager.Menus.Add("Véhicule", vehicleMenu);
            MenuManager.Menus.Add("Créer Véhicule", spawnerMenu);
            MenuManager.Menus.Add("Monde", worldMenu);
            MenuManager.Menus.Add("Heure", timeMenu);
            MenuManager.Menus.Add("Météo", weatherMenu);
            MenuManager.Menus.Add("HUD", hudMenu);
        }

        public static void UpdateVehicleSpawnerMenu()
        {
            var pageItems = Pagination.GetPage(VehicleDatabase.vehicles);
            if (!MenuManager.Menus.ContainsKey("Créer Véhicule")) return;

            var menu = MenuManager.Menus["Créer Véhicule"];

            menu.Items.Clear();
            menu.Actions.Clear();

            foreach (var model in pageItems)
            {
                string vehicleName = model;
                menu.Items.Add(vehicleName);
                menu.Actions.Add(() => VehicleManager.SpawnVehicle(vehicleName));
            }

            menu.Items.Add("~y~Retour");
            menu.Actions.Add(() => MenuManager.SetMenu("Véhicule"));
        }

        public static void UpdateTimeMenu()
        {
            if (!MenuManager.Menus.ContainsKey("Heure")) return;

            var menu = MenuManager.Menus["Heure"];

            menu.Items.Clear();
            menu.Actions.Clear();

            int[] hours = { 0, 6, 12, 18, 21 };

            foreach (var hour in hours)
            {
                int localHour = hour;
                menu.Items.Add(localHour.ToString("00") + ":00");
                menu.Actions.Add(() =>
                {
                    Function.Call(Hash.SET_CLOCK_TIME, localHour, 0, 0);
                });
            }

            menu.Items.Add("~y~Retour");
            menu.Actions.Add(() => MenuManager.SetMenu("Monde"));
        }

        public static void UpdateWeatherMenu()
        {
            if (!MenuManager.Menus.ContainsKey("Météo")) return;

            var menu = MenuManager.Menus["Météo"];

            menu.Items.Clear();
            menu.Actions.Clear();

            foreach (Weather weather in Enum.GetValues(typeof(Weather)))
            {
                var localWeather = weather;
                menu.Items.Add(localWeather.ToString());
                menu.Actions.Add(() =>
                {
                    string weatherName = localWeather.ToString();
                    Function.Call(Hash.SET_WEATHER_TYPE_PERSIST, weatherName);
                    Function.Call(Hash.SET_WEATHER_TYPE_OVERTIME_PERSIST, weatherName);
                    Function.Call(Hash.SET_WEATHER_TYPE_NOW_PERSIST, weatherName);
                });
            }

            menu.Items.Add("~y~Retour");
            menu.Actions.Add(() => MenuManager.SetMenu("Monde"));
        }
    }
}