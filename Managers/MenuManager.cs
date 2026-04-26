using System;
using System.Collections.Generic;

namespace Kerl0s_ModMenu.Managers
{
    public static class MenuManager
    {
        // Menu storage (exposed as a read-only property)
        public static Dictionary<string, Menu> Menus { get; } = new Dictionary<string, Menu>();

        // Currently active menu
        public static Menu CurrentMenu { get; set; }

        // Menu open state
        public static bool IsOpen { get; set; } = false;

        #region Player
        public static bool IsGodMode = false;
        public static bool IsSuperSpeed = false;
        public static bool IsSuperSwim = false;
        public static bool IsInfiniteAbility = false;
        #endregion
        #region Vehicle
        public static bool IsSpeedBoost = false;
        public static bool IsSpeedometer = false;
        #endregion
        #region Weapon
        public static bool IsInfiniteAmmo = false;
        public static bool IsNoReload = false;
        public static bool IsExplosiveBullets = false;
        public static bool IsExplosiveMelee = false;
        public static bool IsIncendiaryBullets = false;
        #endregion
        #region World
        public static bool IsNoPolice = false;
        public static bool IsNoTraffic = false;
        public static bool IsNoPeds = false;
        #endregion
        #region Extra
        public static bool IsHudActive = true;
        public static bool IsFreeCamera = false;
        public static bool IsNightVision = false;
        public static bool IsThermalVision = false;
        #endregion

        public static void SetMenu(Menu menu)
        {
            if (menu == null) return;

            CurrentMenu = menu;
            if (CurrentMenu != null)
            {
                CurrentMenu.SelectedIndex = 0;
            }
        }

        public static void ToggleMenu()
        {
            IsOpen = !IsOpen;
            SetMenu(Menus.ContainsKey("Menu Principal") ? Menus["Menu Principal"] : null);
            if (CurrentMenu != null) CurrentMenu.SelectedIndex = 0;
        }

        public static void Draw()
        {
            if (IsOpen && CurrentMenu != null)
            {
                CurrentMenu.Draw();
            }
        }

        // Backwards-compatible method: toggles a field passed by ref and updates the menu label.
        public static void ToggleOption(ref bool flag, Menu menu, int optionIndex, string baseLabel)
        {
            if (menu == null)
            {
                GTA.UI.Screen.ShowSubtitle("Menu non trouvé !");
                return;
            }

            if (menu?.Items == null || optionIndex < 0 || optionIndex >= menu.Items.Count)
            {
                GTA.UI.Screen.ShowSubtitle($"OptionIndex {optionIndex} invalide !");
                return;
            }

            flag = !flag;
            UpdateMenuItemState(menu, optionIndex, baseLabel, flag);
        }

        // Preferred overload: use getter/setter delegates instead of ref for safer encapsulation.
        public static void ToggleOption(Menu menu, int optionIndex, string baseLabel, Func<bool> getter, Action<bool> setter)
        {
            if (getter == null || setter == null)
            {
                GTA.UI.Screen.ShowSubtitle("ToggleOption: getter/setter cannot be null");
                return;
            }

            if (menu == null)
            {
                GTA.UI.Screen.ShowSubtitle("Menu non trouvé !");
                return;
            }

            if (menu?.Items == null || optionIndex < 0 || optionIndex >= menu.Items.Count)
            {
                GTA.UI.Screen.ShowSubtitle($"OptionIndex {optionIndex} invalide !");
                return;
            }

            var newState = !getter();
            setter(newState);
            UpdateMenuItemState(menu, optionIndex, baseLabel, newState);
        }

        private static void UpdateMenuItemState(Menu menu, int optionIndex, string baseLabel, bool state)
        {
            string stateLabel = state ? "~g~ON" : "~r~OFF";
            menu.Items[optionIndex] = $"{baseLabel} {stateLabel}";
        }
    }
}