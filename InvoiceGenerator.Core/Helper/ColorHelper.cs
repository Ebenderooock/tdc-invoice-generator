// Copyright 2024 - Eben de Roock
// Created: 2024${File.CreatedMonth}${File.CreatedDay} @19:12

using System;
using System.Security.Cryptography;
using System.Text;

namespace InvoiceGenerator.Core.Helper
{
    public static class ColorHelper
    {

        public static string StringToColor(string str)
        {
            byte[] byteValue = Encoding.UTF8.GetBytes(str);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(byteValue);

                byte r = hashBytes[0];
                byte g = hashBytes[1];
                byte b = hashBytes[2];

                double brightnessFactor = 4;
                r = Clamp((byte)(r * brightnessFactor));
                g = Clamp((byte)(g * brightnessFactor));
                b = Clamp((byte)(b * brightnessFactor));

                string colorCode = $"#{r:X2}{g:X2}{b:X2}";

                return colorCode;
            }
        }

        public static string GetContrastingText(string color)
        {
            // Assuming color is in the format "#RRGGBB"
            int r = Convert.ToInt32(color.Substring(1, 2), 16);
            int g = Convert.ToInt32(color.Substring(3, 2), 16);
            int b = Convert.ToInt32(color.Substring(5, 2), 16);

            // Calculate luminance (perceived brightness) using the formula Y = 0.299*R + 0.587*G + 0.114*B
            double luminance = 0.299 * r + 0.587 * g + 0.114 * b;

            // Switch between black and white text based on luminance
            string textColor = (luminance > 128) ? "#000000" : "#FFFFFF";

            return textColor;
        }

        
        public static byte Clamp(byte value)
        {
            return Math.Max((byte)0, Math.Min((byte)255, value));
        }
    }
}