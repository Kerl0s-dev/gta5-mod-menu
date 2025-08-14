namespace Kerl0s_ModMenu.Utils.UI
{
    internal class Math
    {
        public static float Clamp(float value, float min, float max)
        {
            if (value < min) { value = min; min = value; }
            if (value > max) { value = max; max = value; }

            return value;
        }
    }
}
