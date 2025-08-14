using System.Collections.Generic;

namespace Kerl0s_ModMenu.Managers
{
    public static class MenuManager
    {
        public static Dictionary<string, Menu> Menus = new Dictionary<string, Menu>();
        public static Menu CurrentMenu;
        public static bool IsOpen = false;

        // Player Menu
        public static bool isGodMode = false;
        public static bool isSuperSpeed = false;
        public static bool isSuperSwim = false;

        // Vehicle Menu
        public static bool isSpeedBoost = false;
        public static bool isRainbowPaint = false;

        // HUD Menu
        public static bool hudActive = true;

        public static void SetMenu(string name)
        {
            if (Menus.ContainsKey(name))
            {
                CurrentMenu = Menus[name];
                CurrentMenu.SelectedIndex = 0;
            }
        }

        public static void ToggleMenu()
        {
            IsOpen = !IsOpen;
            SetMenu("Menu Principal");
            CurrentMenu.SelectedIndex = 0;
        }

        public static void Draw()
        {
            if (IsOpen && CurrentMenu != null)
            {
                CurrentMenu.Draw();
            }
        }

        // Méthode pour toggle une option et mettre à jour l'affichage
        public static void ToggleOption(ref bool flag, string menuKey, int optionIndex, string baseLabel)
        {
            if (!Menus.ContainsKey(menuKey))
            {
                GTA.UI.Screen.ShowSubtitle($"Menu {menuKey} non trouvé !");
                return;
            }

            var menu = Menus[menuKey];

            if (optionIndex < 0 || optionIndex >= menu.Items.Count)
            {
                GTA.UI.Screen.ShowSubtitle($"OptionIndex {optionIndex} invalide !");
                return;
            }

            flag = !flag;
            string state = flag ? "~g~ON" : "~r~OFF";
            menu.Items[optionIndex] = $"{baseLabel} {state}";
        }
    }
}