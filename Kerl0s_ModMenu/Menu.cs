using GTA;
using GTA.Native;
using GTA.Math;
using Kerl0s_ModMenu.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA.UI;

namespace Kerl0s_ModMenu
{
    internal class Menu : Script
    {
        private bool menuOpen = false;
        private int navIndex = 0;

        private string currentMenu = "Main";  // Current active menu
        private Dictionary<string, List<string>> menus = new Dictionary<string, List<string>>();
        private Dictionary<string, List<Action>> menuActions = new Dictionary<string, List<Action>>();

        #region Actions
        /* PLAYER */
        static bool godMode = false;
        static bool superSpeed = false;
        static bool infiniteAbility = false;

        /* VEHICLE */
        static bool speedBoost = false;
        static bool speedOMeter = false;
        #endregion

        #region Menu Related
        float x = 0.11f, y = 0.04f;
        float spacing = 0.05f; // Add more space between each option
        #endregion

        public Menu()
        {
            // Define Menus and their options
            menus["Main"] = new List<string> { "Player", "Vehicle", "Time", "Free Fly" };
            
            menus["Player"] = new List<string> { "God Mode ~r~OFF", "Super Speed ~r~OFF", "Infinite Ability ~r~OFF", "Teleport", "Give a parachute", "Back to Main Menu" };

            #region Vehicle Menu
            menus["Vehicle"] = new List<string> { "Spawn Vehicle", "Repair", "Speed Boost ~r~OFF", "Speed O Meter ~r~OFF", "Back to Main Menu" };
            menus["Spawn Vehicle"] = new List<string> { "Vehicle Page 1", "Vehicle Page 2", "Vehicle Page 3", "Vehicle Page 4", "Planes", "Back to Vehicle Menu" };
            #region Vehicle Pages
            menus["Vehicle Page 1"] = new List<string> { "Adder", "Zentorno", "T20", "Osiris", "ItaliGTB", "Pariah", "Banshee", "Kuruma2", "Dominator", "Schafter2", "Elegy", "Buffalo", "XA21", "Back" };
            menus["Vehicle Page 2"] = new List<string> { "Deveste", "Neo", "ItaliRSX", "Futo2", "Komoda", "Sugoi", "Remus", "Jester4", "Calico", "Dominator7", "Euros", "SultanRS", "Schlagen", "Back" };
            menus["Vehicle Page 3"] = new List<string> { "Prototipo", "Voltics", "Cerberus", "Revolter", "Paragon", "Imperator", "Ambulance", "Burrito2", "RallyTruck", "RatLoader", "Panto", "Faggio", "Voltics2", "Back" };
            menus["Vehicle Page 4"] = new List<string> { "Asbo", "Buffalo3", "Dominator3", "Penumbra2", "RapidGT", "Schafter4", "Sultan2", "Elegy2", "Baller4", "Virgo", "Cog552", "Euros", "Cheetah2", "Back" };
            menus["Planes"] = new List<string> { "Duster", "Maverick", "Back" };
            #endregion
            #endregion

            menus["Time"] = new List<string> { "Set to Morning", "Set To Noon", "Set to AfterNoon", "Set To Night", "Back" };

            menus["Teleport"] = new List<string>
            {
                "Marker",
                "Michael House",
                "Franklin House",
                "Trevor Trailer",
                "Maze Bank Tower",
                "Los Santos Airport",
                "Mount Chiliad",
                "Sandy Shores Airfield",
                "Los Santos Customs",
                "Back"
            };

            menuActions["Main"] = new List<Action>
            {
                () => SwitchMenu("Player"),
                () => SwitchMenu("Vehicle"),
                () => SwitchMenu("Time"),
                FreeFly
            };

            menuActions["Player"] = new List<Action>
            {
                ToggleGodMode,
                ToggleSuperSpeed,
                ToggleInfiniteAbility,
                () => SwitchMenu("Teleport"),
                () => Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, WeaponHash.Parachute, 1, true, false),
                () => SwitchMenu("Main")
            };

            menuActions["Vehicle"] = new List<Action>
            {
                () => SwitchMenu("Spawn Vehicle"),
                RepairVehicle,
                ToggleSpeedBoost,
                ToggleSpeedOMeter,
                () => SwitchMenu("Main")
            };

            #region Vehicle SubMenu
            menuActions["Spawn Vehicle"] = new List<Action>
            {
                () => SwitchMenu("Vehicle Page 1"),
                () => SwitchMenu("Vehicle Page 2"),
                () => SwitchMenu("Vehicle Page 3"),
                () => SwitchMenu("Vehicle Page 4"),
                () => SwitchMenu("Planes"),
                () => SwitchMenu("Vehicle"),
            };

            menuActions["Vehicle Page 1"] = new List<Action>
            {
                () => SpawnVehicle(VehicleHash.Adder),
                () => SpawnVehicle(VehicleHash.Zentorno),
                () => SpawnVehicle(VehicleHash.T20),
                () => SpawnVehicle(VehicleHash.Osiris),
                () => SpawnVehicle(VehicleHash.ItaliGTB),
                () => SpawnVehicle(VehicleHash.Pariah),
                () => SpawnVehicle(VehicleHash.Banshee),
                () => SpawnVehicle(VehicleHash.Kuruma2),
                () => SpawnVehicle(VehicleHash.Dominator),
                () => SpawnVehicle(VehicleHash.Schafter2),
                () => SpawnVehicle(VehicleHash.Elegy),
                () => SpawnVehicle(VehicleHash.Buffalo),
                () => SpawnVehicle(VehicleHash.XA21),
                () => SwitchMenu("Spawn Vehicle")
            };
            menuActions["Vehicle Page 2"] = new List<Action>
            {
                () => SpawnVehicle(VehicleHash.Deveste),
                () => SpawnVehicle(VehicleHash.Neo),
                () => SpawnVehicle(VehicleHash.ItaliRSX),
                () => SpawnVehicle(VehicleHash.Futo2),
                () => SpawnVehicle(VehicleHash.Komoda),
                () => SpawnVehicle(VehicleHash.Sugoi),
                () => SpawnVehicle(VehicleHash.Remus),
                () => SpawnVehicle(VehicleHash.Jester4),
                () => SpawnVehicle(VehicleHash.Calico),
                () => SpawnVehicle(VehicleHash.Dominator7),
                () => SpawnVehicle(VehicleHash.Euros),
                () => SpawnVehicle(VehicleHash.SultanRS),
                () => SpawnVehicle(VehicleHash.Schlagen),
                () => SwitchMenu("Spawn Vehicle")
            };
            menuActions["Vehicle Page 3"] = new List<Action>
            {
                () => SpawnVehicle(VehicleHash.Prototipo),
                () => SpawnVehicle(VehicleHash.Voltic),
                () => SpawnVehicle(VehicleHash.Cerberus),
                () => SpawnVehicle(VehicleHash.Revolter),
                () => SpawnVehicle(VehicleHash.Paragon),
                () => SpawnVehicle(VehicleHash.Imperator),
                () => SpawnVehicle(VehicleHash.Ambulance),
                () => SpawnVehicle(VehicleHash.Burrito2),
                () => SpawnVehicle(VehicleHash.RallyTruck),
                () => SpawnVehicle(VehicleHash.RatLoader),
                () => SpawnVehicle(VehicleHash.Panto),
                () => SpawnVehicle(VehicleHash.Faggio),
                () => SpawnVehicle(VehicleHash.Voltic2),
                () => SwitchMenu("Spawn Vehicle")
            };
            menuActions["Vehicle Page 4"] = new List<Action>
            {
                () => SpawnVehicle(VehicleHash.Asbo),
                () => SpawnVehicle(VehicleHash.Buffalo3),
                () => SpawnVehicle(VehicleHash.Dominator3),
                () => SpawnVehicle(VehicleHash.Penumbra2),
                () => SpawnVehicle(VehicleHash.RapidGT),
                () => SpawnVehicle(VehicleHash.Schafter4),
                () => SpawnVehicle(VehicleHash.Sultan2),
                () => SpawnVehicle(VehicleHash.Elegy2),
                () => SpawnVehicle(VehicleHash.Baller4),
                () => SpawnVehicle(VehicleHash.Virgo),
                () => SpawnVehicle(VehicleHash.Cog552),
                () => SpawnVehicle(VehicleHash.Euros),
                () => SpawnVehicle(VehicleHash.Cheetah2),
                () => SwitchMenu("Spawn Vehicle")
            };

            menuActions["Planes"] = new List<Action>
            {
                () => SpawnVehicle(VehicleHash.Duster),
                () => SpawnVehicle(VehicleHash.Maverick),
                () => SwitchMenu("Spawn Vehicle")
            };
            #endregion

            menuActions["Time"] = new List<Action>
            {
                () => SetTime(8,0,0),
                () => SetTime(12,0,0),
                () => SetTime(18,0,0),
                () => SetTime(22,0,0),
                () => SwitchMenu("Main")
            };

            menuActions["Teleport"] = new List<Action>
            {
                TeleportToMarker,
                () => TeleportToPosition(new Vector3(-850.5f, 179.9f, 69.0f)), // Michael's House
                () => TeleportToPosition(new Vector3(7.9f, 548.1f, 174.0f)), // Franklin's House (Vinewood Hills)
                () => TeleportToPosition(new Vector3(1985.7f, 3820.2f, 31.5f)), // Trevor's Trailer (Sandy Shores)
                () => TeleportToPosition(new Vector3(-75.0f, -818.9f, 325.0f)), // Maze Bank Tower (Roof)
                () => TeleportToPosition(new Vector3(-1336.0f, -3044.0f, 12.8f)), // Los Santos Airport (Runway)
                () => TeleportToPosition(new Vector3(501.0f, 5593.0f, 794.0f)), // Mount Chiliad (Panorama Viewpoint)
                () => TeleportToPosition(new Vector3(1747.0f, 3273.7f, 40.0f)), // Sandy Shores Airfield
                () => TeleportToPosition(new Vector3(-365.4f, -131.4f, 37.5f)), // Los Santos Customs (City)
                () => SwitchMenu("Player")
            };

            Tick += Update;
            KeyDown += HandleInput;
        }

        public void Update(object sender, EventArgs e)
        {
            if (menuOpen) { DrawMenu(); }

            #region Player
            Game.Player.Character.IsInvincible = godMode;
            Function.Call(Hash.SET_PED_MOVE_RATE_OVERRIDE, Game.Player.Character, (superSpeed ? 10.0f : 1.0f));
            if (infiniteAbility) { Game.Player.ChargeSpecialAbility(100); }
            #endregion

            if (Game.Player.Character.IsInVehicle())
            {
                Game.Player.Character.CurrentVehicle.EnginePowerMultiplier = (speedBoost ? 150 : 1);
                Game.Player.Character.CurrentVehicle.EngineTorqueMultiplier = (speedBoost ? 150 : 1);
            }

            if (speedOMeter)
            {
                float Speed = Game.Player.Character.Speed;

                float speedKmh = Speed * 3.6f; // Convert m/s to km/h
                UI.DrawSpeedometer(speedKmh);
            }
        }

        public void HandleInput(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5) {
                menuOpen = !menuOpen; // Open/Close Menu
                currentMenu = "Main";
                navIndex = 0;
                Audio.PlaySoundFrontendAndForget("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }

            if (Game.Player.Character.IsInParachuteFreeFall)
            {
                if (e.KeyCode == Keys.Add)
                {
                    Game.Player.Character.ApplyForce(Vector3.WorldUp * 100);
                }
                if (e.KeyCode == Keys.Enter)
                {
                    Game.Player.Character.ApplyForce(Vector3.WorldUp * -100);
                }
                if (e.KeyCode == Keys.NumPad8)
                {
                    Game.Player.Character.ApplyForceRelative(new Vector3(0,100,0));
                }
                if (e.KeyCode == Keys.NumPad2)
                {
                    Game.Player.Character.ApplyForceRelative(new Vector3(0, -100, 0));
                }
                if (e.KeyCode == Keys.NumPad6)
                {
                    Game.Player.Character.ApplyForceRelative(new Vector3(100, 0, 0));
                }
                if (e.KeyCode == Keys.NumPad4)
                {
                    Game.Player.Character.ApplyForceRelative(new Vector3(-100, 0, 0));
                }
            }

            if (!menuOpen) return;

            if (e.KeyCode == Keys.NumPad8)
            {
                navIndex = (navIndex - 1 + menus[currentMenu].Count) % menus[currentMenu].Count;
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            if (e.KeyCode == Keys.NumPad2)
            {
                navIndex = (navIndex + 1) % menus[currentMenu].Count;
                Audio.PlaySoundFrontendAndForget("NAV_UP_DOWN", "HUD_FRONTEND_DEFAULT_SOUNDSET");
            }
            if (e.KeyCode == Keys.NumPad5)
            {
                Audio.PlaySoundFrontendAndForget("SELECT", "HUD_FRONTEND_DEFAULT_SOUNDSET");
                ExecuteOption(navIndex);
            }
        }

        private void ExecuteOption(int navIndex)
        {
            if (navIndex >= 0 && navIndex < menuActions[currentMenu].Count)
            {
                menuActions[currentMenu][navIndex].Invoke();
            }
        }

        void DrawMenu()
        {
            UI.DrawHeader($"Mod - {currentMenu}", 1, 0.6f, Color.FromArgb(255, 255, 0, 0), 255, x, y);

            float startY = y + spacing;

            for (int i = 0; i < menus[currentMenu].Count; i++)
            {
                Color optionColor = i == navIndex ? Color.FromArgb(255, 150, 150, 150) : Color.FromArgb(255, 0, 0, 0);

                UI.DrawHeader(menus[currentMenu][i].ToString(), 2, 0.5f, optionColor, 100, x, startY + (i * 0.05f));
            }
        }

        #region Actions
        /// <summary>
        /// Switches to a different menu.
        /// </summary>
        void SwitchMenu(string newMenu)
        {
            if (menus.ContainsKey(newMenu))
            {
                currentMenu = newMenu;
                navIndex = 0;  // Reset selection
            }
        }

        #region Player
        void ToggleGodMode()
        {
            godMode = !godMode;
            menus["Player"][0] = "God Mode " + (godMode ? "~g~ON" : "~r~OFF");
        }

        void ToggleSuperSpeed()
        {
            superSpeed = !superSpeed;
            menus["Player"][1] = "Super Speed " + (superSpeed ? "~g~ON" : "~r~OFF");
        }

        void ToggleInfiniteAbility()
        {
            infiniteAbility = !infiniteAbility;
            menus["Player"][2] = "Infinite Ability " + (infiniteAbility ? "~g~ON" : "~r~OFF");
        }
        #endregion
        #region Vehicle
        /// <summary>
        /// Repairs the player's current vehicle if they are in one.
        /// </summary>
        void RepairVehicle()
        {
            if (Game.Player.Character.IsInVehicle()) Game.Player.Character.CurrentVehicle.Repair();
        }

        /// <summary>
        /// Toggles the speed boost for the player's vehicle.
        /// </summary>
        void ToggleSpeedBoost()
        {
            speedBoost = !speedBoost;
            menus["Vehicle"][2] = "Speed Boost " + (speedBoost ? "~g~ON" : "~r~OFF");
        }

        /// <summary>
        /// Spawns a vehicle of the specified VehicleHash at the player's position. 
        /// Sets the license plate and puts the player in the driver's seat.
        /// </summary>
        void SpawnVehicle(VehicleHash vehicleHash)
        {
            Vehicle car = World.CreateVehicle(vehicleHash, Game.Player.Character.Position, Game.Player.Character.Heading);

            car.Mods.LicensePlate = "KERL0S";

            Game.Player.Character.SetIntoVehicle(car, VehicleSeat.Driver);
        }

        /// <summary>
        /// Switches the speedometer display on or off.
        /// </summary>
        private void ToggleSpeedOMeter()
        {
            speedOMeter = !speedOMeter;
            menus["Vehicle"][3] = "Speed O Meter " + (speedOMeter ? "~g~ON" : "~r~OFF");
        }
        #endregion

        /// <summary>
        /// Sets the in-game clock to a specific time.
        /// <paramref name="hours"/> The hour to set the clock to (0-23).
        /// <paramref name="minutes"/> The minute to set the clock to (0-59).
        /// <paramref name="seconds"/> The second to set the clock to (0-59).
        /// </summary>
        void SetTime(int hours, int minutes, int seconds)
        {
            Function.Call(Hash.SET_CLOCK_TIME, hours, minutes, seconds);
        }

        private void FreeFly()
        {
            menuOpen = false;

            // Check if the player is in a vehicle or on foot
            if (Game.Player.Character.IsInVehicle()) return;

            // Give the parachute by adding it as an item to the player's inventory
            Function.Call(Hash.GIVE_WEAPON_TO_PED, Game.Player.Character, WeaponHash.Parachute, 1, false, false);

            Game.Player.Character.ApplyForce(Vector3.WorldUp * 5000);
            Function.Call(Hash.TASK_PARACHUTE, Game.Player.Character);
        }

        void TeleportToMarker()
        {
            Ped playerPed = Game.Player.Character;
            Vehicle playerVehicle = Game.Player.Character.CurrentVehicle;
            Vector3 pos = World.WaypointPosition;

            float[] groundHeight = new float[301];

            for (int i = 0; i <= 300; i++)
            {
                groundHeight[i] = i * 50.0f;
            }

            if (World.WaypointBlip != null)
            {
                Function.Call(Hash.DO_SCREEN_FADE_OUT, 250);
                Wait(250);
                foreach (float height in groundHeight)
                {
                    unsafe
                    {
                        float z = 0;
                        playerPed.Position = new Vector3(pos.X, pos.Y, height);
                        Wait(100);
                        if (Function.Call<bool>(Hash.GET_GROUND_Z_FOR_3D_COORD, pos.X, pos.Y, height, &z))
                        {
                            playerPed.Position = new Vector3(pos.X, pos.Y, z);
                            break;
                        }
                    }
                }
                Wait(250);
                Function.Call(Hash.DO_SCREEN_FADE_IN, 250);
            }
            else Notification.PostTicker(
                "~r~MARKER NOT SET" +
                "~n~~w~Please set up a marker on the map", true
            );
        }

        void TeleportToPosition(Vector3 targetPosition)
        {
            Function.Call(Hash.DO_SCREEN_FADE_OUT, 250);
            Wait(250);
            Game.Player.Character.Position = targetPosition;
            Wait(250);
            Function.Call(Hash.DO_SCREEN_FADE_IN, 250);
        }

        #endregion
    }
}
