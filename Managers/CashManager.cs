using GTA;
using System;

namespace Kerl0s_ModMenu.Managers
{
    public static class CashManager
    {
        private static bool rainingCash = false;
        private static DateTime lastDropTime;

        public static void ToggleCashRain(int amountPerDrop = 2500)
        {
            rainingCash = !rainingCash;
            GTA.UI.Notification.PostTicker(rainingCash ? "~g~Pluie de cash activée" : "~r~Pluie de cash désactivée", true);

            if (rainingCash)
                lastDropTime = DateTime.Now;
        }

        public static void OnTick(int amountPerDrop = 1)
        {
            if (!rainingCash) return;

            if ((DateTime.Now - lastDropTime).TotalMilliseconds > .01) // toutes les 0.5 secondes
            {
                GiveCashToPlayer(amountPerDrop);
                lastDropTime = DateTime.Now;
            }
        }

        private static void GiveCashToPlayer(int amount)
        {
            Ped player = Game.Player.Character;

            // Ajoute l'argent
            if (Game.Player.Money < 2000000000)
                Game.Player.Money += amount;
            else GTA.UI.Notification.PostTicker("Plus d'argent en stock", true);
        }
    }
}