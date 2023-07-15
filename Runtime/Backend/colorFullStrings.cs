using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lucky44.LuckyConsole
{
    public static class colorFullStrings
    {
        public static string color(this string original, Color toSet)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(toSet)}>{original}</color>";
        }

        public static string color(this string original, string HexColor)
        {
            return $"<color={HexColor}>{original}</color>";
        }
    }
}
