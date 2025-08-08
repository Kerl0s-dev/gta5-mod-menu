using GTA.Native;
using System.Drawing;

namespace Kerl0s_ModMenu.UI
{
    public static class UIDrawer
    {
        public static void DrawRect(float x, float y, float width, float height, int r, int g, int b, int a)
        {
            Function.Call(Hash.DRAW_RECT, x, y, width, height, r, g, b, a);
        }
        public static void DrawHeader(string text, int fontIndex, float textSize, Color color, int a, float x, float y, float width = 0.2f, float height = 0.05f)
        {
            // Use Function.Call to draw the rectangle
            DrawRect(x, y, width, height, color.R, color.G, color.B, a);

            DrawText(text, textSize, fontIndex, x - 0.085f, y - 0.005);
        }
        public static void DrawText(string text, float textSize, int fontIndex, float x, double y)
        {
            // Use Function.Call to display the text
            Function.Call(Hash.SET_TEXT_SCALE, textSize, textSize);
            Function.Call(Hash.SET_TEXT_FONT, fontIndex);
            Function.Call(Hash.SET_TEXT_OUTLINE);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, text);
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, x, y - 0.01, 0);
        }

        public static void DrawSpeedometer(float speedKmh)
        {
            float maxSpeed = 300f; // Maximum speed for scaling
            float normalizedSpeed = Clamp(speedKmh / maxSpeed, 0f, 1f);


            // Lerp colors (Green → Yellow → Red)
            int r, g, b;

            if (normalizedSpeed < 0.5f) // Green to Yellow (0 - 150 km/h)
            {
                float t = normalizedSpeed * 2f; // Scale for 0.5 range
                r = (int)(0 + (255 - 0) * t);  // Green to Yellow (increasing Red)
                g = 255;
                b = 0;
            }
            else // Yellow to Red (150 - 300 km/h)
            {
                float t = (normalizedSpeed - 0.5f) * 2f; // Scale for 0.5 range
                r = 255;
                g = (int)(255 - (255 * t)); // Decreasing Green
                b = 0;
            }

            float baseX = 0.5f;
            float baseY = 0.9f;
            float barWidth = 0.2f;
            float barHeight = 0.035f;

            // Background Bar (Gray)
            Function.Call(Hash.DRAW_RECT, baseX, baseY, barWidth, barHeight, 50, 50, 50, 200);

            // Speed Bar (Lerp Color)
            Function.Call(Hash.DRAW_RECT, baseX - (barWidth / 2) + (normalizedSpeed * barWidth / 2), baseY, normalizedSpeed * barWidth, barHeight, r, g, b, 255);

            // Display Speed Text
            Function.Call(Hash.SET_TEXT_FONT, 4);
            Function.Call(Hash.SET_TEXT_SCALE, 0.5f, 0.5f);
            Function.Call(Hash.SET_TEXT_OUTLINE);
            Function.Call(Hash.SET_TEXT_CENTRE, true);
            Function.Call(Hash.BEGIN_TEXT_COMMAND_DISPLAY_TEXT, "STRING");
            Function.Call(Hash.ADD_TEXT_COMPONENT_SUBSTRING_PLAYER_NAME, $"{(int)speedKmh} km/h");
            Function.Call(Hash.END_TEXT_COMMAND_DISPLAY_TEXT, baseX, baseY - 0.015, 0);
        }

        private static float Clamp(float value, float min, float max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }
    }
}