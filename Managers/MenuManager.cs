using Kerl0s_ModMenu.UI;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Kerl0s_ModMenu.Managers
{
    public class MenuManager
    {
        public static Dictionary<string, List<string>> menus = new Dictionary<string, List<string>>();
        public static Dictionary<string , List<Action>> menuActions = new Dictionary<string , List<Action>>();

        public static string currentMenu = "Main";
        public static int navIndex = 0;

        public static bool MenuOpen { get; private set; } = false;

        public static bool IsGodMod = false;
        public static bool IsSuperSpeed = false;
        public static bool IsSuperSwim = false;

        public static bool IsSpeedBoost = false;
        public static bool IsSpeedOMeter = false;
        public static bool IsSeatbeltOn = false;
        public static bool IsInvincible = false;
        public static bool IsRainbowPaint = false;

        public static bool IsNightVision = false;
        public static bool IsHeatVision = false;
        public static bool IsHudActive = true;
        public static bool IsFreeCam = false;

        public MenuManager ()
        {
            currentMenu = "Main";
            navIndex = 0;

            IsGodMod = false;
            IsSuperSpeed = false;
            IsSuperSwim = false;
            IsSpeedBoost = false;
            IsSpeedOMeter = false;
            IsNightVision = false;
            IsHeatVision = false;

            MenuInitializer.Initialize();
        }

        public static void Draw()
        {
            if (!menus.ContainsKey(currentMenu)) return;

            float startX = 0.11f;
            float startY = 0.09f;
            float spacing = 0.05f;

            // Affiche le titre
            UIDrawer.DrawHeader($"KMM V2 - {currentMenu}", 1, 0.6f, Color.OrangeRed, 255, startX, startY - 0.05f);

            // Affiche chaque option
            var options = menus[currentMenu];
            for (int i = 0; i < options.Count; i++)
            {
                var color = (i == navIndex) ? Color.Gray : Color.Black;
                UIDrawer.DrawHeader(options[i], 2, 0.5f, color, 100, startX, startY + (i * spacing));
            }
        }

        public static void ToggleMenu()
        {
            MenuOpen = !MenuOpen;
            currentMenu = "Main";
            navIndex = 0;
        }

        #region Menu Navigation
        public static void NavigateUp()
        {
            if (menus[currentMenu].Count == 0) return;
            navIndex = (navIndex - 1 + menus[currentMenu].Count) % menus[currentMenu].Count;
        }

        public static void NavigateDown()
        {
            if (menus[currentMenu].Count == 0) return;
            navIndex = (navIndex + 1) % menus[currentMenu].Count;
        }

        public static void Select()
        {
            if (menuActions.ContainsKey(currentMenu) && navIndex < menuActions[currentMenu].Count)
            {
                menuActions[currentMenu][navIndex].Invoke();
            }
        }
        #endregion

        public static void SetMenu(string name)
        {
            if (menus.ContainsKey(name))
            {
                currentMenu = name;
                navIndex = 0;
            }
        }
    }
}