using GTA;
using System;

namespace Kerl0s_ModMenu.Managers
{
    public class WeaponManager
    {
        public static void GiveAllWeapons()
        {
            foreach (WeaponHash weapon in Enum.GetValues(typeof(WeaponHash)))
            {
                Game.Player.Character.Weapons.Give(weapon, 9999, false, true);
            }
        }

        public static void RemoveAllWeapons()
        {
            Game.Player.Character.Weapons.RemoveAll();
        }
    }
}