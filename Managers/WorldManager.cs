namespace Kerl0s_ModMenu.Managers
{
    public class WorldManager
    {
        public static void ChangeWeather(string weatherType)
        {
            GTA.Native.Function.Call(GTA.Native.Hash.SET_WEATHER_TYPE_NOW_PERSIST, weatherType);
        }

        public static void ChangeTime(int hour, int minute)
        {
            GTA.Native.Function.Call(GTA.Native.Hash.SET_CLOCK_TIME, hour, minute, 0);
        }

        public static void SetTimeScale(float scale)
        {
            GTA.Native.Function.Call(GTA.Native.Hash.SET_TIME_SCALE, scale);
        }
    }
}