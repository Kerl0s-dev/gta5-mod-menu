using GTA;
using GTA.Math;
using Kerl0s_ModMenu.Data;
using System;
using System.Drawing;
using System.Linq;

namespace Kerl0s_ModMenu.Managers
{
    public static class MenuInitializer
    {
        static Menu mainMenu;
        static Menu playerMenu;
        static Menu vehicleMenu;
        static Menu createVehicleMenu; // Nouveau menu pour la création de véhicule
        static Menu teleportMenu;
        static Menu weaponMenu;
        static Menu worldMenu;
        static Menu timeMenu;
        static Menu weatherMenu;
        static Menu extrasMenu;

        public static void Initialize()
        {
            //VehicleDatabase.LoadVehicles();

            CreateMenus();
            RegisterMenus();

            VehicleDatabase.LoadVehicles();
            UpdateCreateVehicleMenu();
        }

        private static void RegisterMenus()
        {
            MenuManager.Menus.Add("Menu Principal", mainMenu);
            MenuManager.Menus.Add("Joueur", playerMenu);
            MenuManager.Menus.Add("Véhicule", vehicleMenu);
            MenuManager.Menus.Add("Créer un véhicule", createVehicleMenu); // Ajout du sous-menu de création de véhicule
            MenuManager.Menus.Add("Téléportation", teleportMenu);
            MenuManager.Menus.Add("Armes", weaponMenu);
            MenuManager.Menus.Add("Monde", worldMenu);
            MenuManager.Menus.Add("Extras", extrasMenu);
        }

        private static void CreateMenus()
        {
            mainMenu = new Menu("Menu Principal", null, Color.FromArgb(0, 0, 100),
                new System.Collections.Generic.Dictionary<string, Action>                {
                    { "Joueur", () => MenuManager.SetMenu(MenuManager.Menus["Joueur"]) },
                    { "Véhicule", () => MenuManager.SetMenu(MenuManager.Menus["Véhicule"]) },
                    { "Téléportation", () => MenuManager.SetMenu(MenuManager.Menus["Téléportation"]) },
                    { "Armes", () => MenuManager.SetMenu(MenuManager.Menus["Armes"]) },
                    { "Monde", () => MenuManager.SetMenu(MenuManager.Menus["Monde"]) },
                    { "Extras", () => MenuManager.SetMenu(MenuManager.Menus["Extras"]) }
                }
            );

            playerMenu = new Menu("Joueur", mainMenu, Color.FromArgb(255, 100, 0),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Mode Divin ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsGodMode, playerMenu, 0, "Mode Divin"); } },
                    { "Super Vitesse ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsSuperSpeed, playerMenu, 1, "Super Vitesse"); } },
                    { "Super Nage ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsSuperSwim, playerMenu, 2, "Super Nage"); } },
                    { "Abilité Infinie ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsInfiniteAbility, playerMenu, 3, "Abilité Infinie"); } },
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );

            vehicleMenu = new Menu("Véhicule", mainMenu, Color.FromArgb(0, 100, 255),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Boost de Vitesse ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsSpeedBoost, vehicleMenu, 0, "Boost de Vitesse"); } },
                    { "Réparer le véhicule", () => { if (Game.Player.Character.CurrentVehicle != null) Game.Player.Character.CurrentVehicle.Repair(); } },
                    { "Créer un véhicule", () => { Pagination.Reset(); UpdateCreateVehicleMenu(); MenuManager.SetMenu(createVehicleMenu); } },
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );

            createVehicleMenu = new Menu("Créer un véhicule", vehicleMenu, Color.FromArgb(0, 255, 255),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "", () => { } }
                }
            );

            teleportMenu = new Menu("Téléportation", mainMenu, Color.FromArgb(0, 255, 100),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Téléporter au waypoint", () => { TeleportManager.TeleportToMarker(); } },
                    { "Téléporter à la maison de Franklin", () => { TeleportManager.TeleportToCoords(8.75f, 540f, 175.0f); } }, // Coordonnées de la maison de Franklin
                    { "Téléporter à la maison de Michael", () => { TeleportManager.TeleportToCoords(-813.0f, 180.0f, 75.0f); } }, // Coordonnées de la maison de Michael
                    { "Téléporter à la maison de Trevor", () => { TeleportManager.TeleportToCoords(1985.0f, 3822.0f, 31.0f); } }, // Coordonnées de la maison de Trevor
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );

            weaponMenu = new Menu("Armes", mainMenu, Color.FromArgb(255, 0, 100),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Donner toutes les armes", () => { WeaponManager.GiveAllWeapons(); } },
                    { "Supprimer toutes les armes", () => { WeaponManager.RemoveAllWeapons(); } },
                    { "Balles Explosives ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsExplosiveBullets, weaponMenu, 2, "Balles Explosives"); } },
                    { "Mêlée Explosive ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsExplosiveMelee, weaponMenu, 3, "Mêlée Explosive"); } },
                    { "Balles Incendiaires ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsIncendiaryBullets, weaponMenu, 4, "Balles Incendiaires"); } },
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );

            worldMenu = new Menu("Monde", mainMenu, Color.FromArgb(100, 0, 255),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Changer le temps", () => MenuManager.SetMenu(timeMenu) },
                    { "Changer la météo", () => MenuManager.SetMenu(weatherMenu) },
                    { "Rétablir le temps normal", () => { WorldManager.SetTimeScale(1.0f); } },
                    { "Ralentir le temps", () => { WorldManager.SetTimeScale(Game.TimeScale - 0.1f); } },
                    { "Accélérer le temps", () => { WorldManager.SetTimeScale(Game.TimeScale + 0.1f); } },
                    { "Pas de police ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsNoPolice, worldMenu, 5, "Pas de police"); } },
                    { "Pas de trafic ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsNoTraffic, worldMenu, 6, "Pas de trafic"); } },
                    { "Pas de piétons ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsNoPeds, worldMenu, 7, "Pas de piétons"); } },
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );

            timeMenu = new Menu("Changer le temps", worldMenu, Color.FromArgb(255, 255, 0),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Matin", () => { WorldManager.ChangeTime(8, 0); } },
                    { "Midi", () => { WorldManager.ChangeTime(12, 0); } },
                    { "Soir", () => { WorldManager.ChangeTime(18, 0); } },
                    { "Nuit", () => { WorldManager.ChangeTime(0, 0); } },
                    { "~y~Retour", () => MenuManager.SetMenu(worldMenu) }
                }
            );

            weatherMenu = new Menu("Changer la météo", worldMenu, Color.FromArgb(255, 255, 0),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "Ensoleillé", () => { WorldManager.ChangeWeather("CLEAR"); } },
                    { "Pluvieux", () => { WorldManager.ChangeWeather("RAIN"); } },
                    { "Orageux", () => { WorldManager.ChangeWeather("THUNDER"); } },
                    { "Brouillard", () => { WorldManager.ChangeWeather("FOGGY"); } },
                    { "~y~Retour", () => MenuManager.SetMenu(worldMenu) }
                }
            );

            extrasMenu = new Menu("Extras", mainMenu, Color.FromArgb(255, 255, 0),
                new System.Collections.Generic.Dictionary<string, Action>
                {
                    { "HUD ~g~ON", () => { MenuManager.ToggleOption(ref MenuManager.IsHudActive, extrasMenu, 0, "HUD"); } },
                    { "Vision Nocturne ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsNightVision, extrasMenu, 1, "Vision Nocturne"); } },
                    { "Vision Thermique ~r~OFF", () => { MenuManager.ToggleOption(ref MenuManager.IsThermalVision, extrasMenu, 2, "Vision Thermique"); } },
                    { "Mesure Vitesse ~r~OFF", () => MenuManager.ToggleOption(ref MenuManager.IsSpeedometer, extrasMenu, 3, "Mesure Vitesse") },
                    { "~y~Retour", () => MenuManager.SetMenu(mainMenu) }
                }
            );
        }

        public static void UpdateCreateVehicleMenu()
        {
            createVehicleMenu.Actions.Clear();
            createVehicleMenu.Items.Clear();

            var allVehicles = VehicleDatabase.Vehicles.ToList();
            int totalPages = Pagination.TotalPages(allVehicles.Count);

            // Met à jour le titre dynamiquement
            createVehicleMenu.Name = $"Créer un véhicule ({Pagination.PageIndex + 1}/{totalPages})";

            var page = Pagination.GetPage(allVehicles);

            foreach (var vehicle in page)
            {
                var v = vehicle;

                createVehicleMenu.Items.Add(v);
                createVehicleMenu.Actions.Add(() =>
                {
                    VehicleManager.SpawnVehicle(v);
                });
            }

            // Retour
            createVehicleMenu.Items.Add("~y~Retour");
            createVehicleMenu.Actions.Add(() =>
            {
                Pagination.Reset();
                MenuManager.SetMenu(vehicleMenu);
            });
        }
    }
}