using GTA;
using GTA.Native;

namespace Kerl0s_ModMenu.Managers
{
    public static class WorldManager
    {
        public static void SetTime(int hour, int minutes)
        {
            Function.Call(Hash.SET_CLOCK_TIME, hour, minutes);
        }

        public static void SetWeather(Weather type)
        {
            string weatherName = type.ToString().ToUpper(); // "Clear" -> "CLEAR"
            Function.Call(Hash.SET_WEATHER_TYPE_NOW_PERSIST, weatherName);
            Function.Call(Hash.SET_WEATHER_TYPE_NOW, weatherName);
            Function.Call(Hash.SET_WEATHER_TYPE_PERSIST, weatherName);
        }
    }
}
