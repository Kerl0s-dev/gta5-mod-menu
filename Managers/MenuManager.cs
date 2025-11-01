using System;
using System.Collections.Generic;
using GTA;

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

        //
        // Toggle flags
        // Keep the original fields for binary/backwards compatibility (ref usage),
        // but expose PascalCase properties that wrap them for clearer, modern usage.
        //
        [Obsolete("Use MenuManager.IsGodMode property instead", false)]
        public static bool isGodMode = false;
        public static bool IsGodMode
        {
            get => isGodMode;
            set => isGodMode = value;
        }

        [Obsolete("Use MenuManager.IsSuperSpeed property instead", false)]
        public static bool isSuperSpeed = false;
        public static bool IsSuperSpeed
        {
            get => isSuperSpeed;
            set => isSuperSpeed = value;
        }

        [Obsolete("Use MenuManager.IsSuperSwim property instead", false)]
        public static bool isSuperSwim = false;
        public static bool IsSuperSwim
        {
            get => isSuperSwim;
            set => isSuperSwim = value;
        }

        [Obsolete("Use MenuManager.IsSpeedBoost property instead", false)]
        public static bool isSpeedBoost = false;
        public static bool IsSpeedBoost
        {
            get => isSpeedBoost;
            set => isSpeedBoost = value;
        }

        [Obsolete("Use MenuManager.IsRainbowPaint property instead", false)]
        public static bool isRainbowPaint = false;
        public static bool IsRainbowPaint
        {
            get => isRainbowPaint;
            set => isRainbowPaint = value;
        }

        [Obsolete("Use MenuManager.HudActive property instead", false)]

        public static bool hudActive = true;
        public static bool HudActive
        {
            get => hudActive;
            set => hudActive = value;
        }

        public static void SetMenu(string name)
        {
            if (string.IsNullOrEmpty(name)) return;

            if (Menus.ContainsKey(name))
            {
                CurrentMenu = Menus[name];
                if (CurrentMenu != null)
                {
                    CurrentMenu.SelectedIndex = 0;
                }
            }
        }

        public static void ToggleMenu()
        {
            IsOpen = !IsOpen;
            SetMenu("Menu Principal");
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
        public static void ToggleOption(ref bool flag, string menuKey, int optionIndex, string baseLabel)
        {
            if (!Menus.ContainsKey(menuKey))
            {
                GTA.UI.Screen.ShowSubtitle($"Menu {menuKey} non trouvé !");
                return;
            }

            var menu = Menus[menuKey];

            if (menu?.Items == null || optionIndex < 0 || optionIndex >= menu.Items.Count)
            {
                GTA.UI.Screen.ShowSubtitle($"OptionIndex {optionIndex} invalide !");
                return;
            }

            flag = !flag;
            UpdateMenuItemState(menu, optionIndex, baseLabel, flag);
        }

        // Preferred overload: use getter/setter delegates instead of ref for safer encapsulation.
        public static void ToggleOption(string menuKey, int optionIndex, string baseLabel, Func<bool> getter, Action<bool> setter)
        {
            if (getter == null || setter == null)
            {
                GTA.UI.Screen.ShowSubtitle("ToggleOption: getter/setter cannot be null");
                return;
            }

            if (!Menus.ContainsKey(menuKey))
            {
                GTA.UI.Screen.ShowSubtitle($"Menu {menuKey} non trouvé !");
                return;
            }

            var menu = Menus[menuKey];

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