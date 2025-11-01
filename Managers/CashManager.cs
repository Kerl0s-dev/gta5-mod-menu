using GTA;
using System;

namespace Kerl0s_ModMenu.Managers
{
    public static class CashManager
    {
        private static bool _isRaining = false;
        private static DateTime _lastDropTime = DateTime.MinValue;

        // Tunable values
        private static readonly TimeSpan DropInterval = TimeSpan.FromMilliseconds(500); // every 500ms
        private const int MaxStoredMoney = 2_000_000_000;

        public static void ToggleCashRain(int amountPerDrop = 2500)
        {
            _isRaining = !_isRaining;
            GTA.UI.Notification.PostTicker(_isRaining ? "~g~Pluie de cash activée" : "~r~Pluie de cash désactivée", true);

            if (_isRaining)
            {
                _lastDropTime = DateTime.UtcNow;
            }
        }

        public static void OnTick(int amountPerDrop = 1)
        {
            if (!_isRaining) return;

            if ((DateTime.UtcNow - _lastDropTime) >= DropInterval)
            {
                GiveCashToPlayer(amountPerDrop);
                _lastDropTime = DateTime.UtcNow;
            }
        }

        private static void GiveCashToPlayer(int amount)
        {
            try
            {
                if (Game.Player.Money < MaxStoredMoney)
                {
                    Game.Player.Money += amount;
                }
                else
                {
                    GTA.UI.Notification.PostTicker("Plus d'argent en stock", true);
                }
            }
            catch
            {
                // Best-effort; ignore unexpected failures at runtime
                GTA.UI.Notification.PostTicker("Erreur lors de l'ajout d'argent", true);
            }
        }
    }
}