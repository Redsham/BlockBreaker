using UnityEngine;

namespace UI.Other
{
	public static class Palette
	{
		public static readonly Color Turquoise = FromHex("#1ABC9C");
		public static readonly Color GreenSea = FromHex("#16A085");
		public static readonly Color Sunflower = FromHex("#F1C40F");
		public static readonly Color Orange = FromHex("#F39C12");
		public static readonly Color Emerald = FromHex("#2ECC71");
		public static readonly Color Nephritis = FromHex("#27AE60");
		public static readonly Color Carrot = FromHex("#E67E22");
		public static readonly Color Pumpkin = FromHex("#D35400");
		public static readonly Color PeterRiver = FromHex("#3498DB");
		public static readonly Color BelizeHole = FromHex("#2980B9");
		public static readonly Color Alizarin = FromHex("#E74C3C");
		public static readonly Color Pomegranate = FromHex("#C0392B");
		public static readonly Color Amethyst = FromHex("#9B59B6");
		public static readonly Color Wisteria = FromHex("#8E44AD");
		public static readonly Color Clouds = FromHex("#ECF0F1");
		public static readonly Color Silver = FromHex("#BDC3C7");
		public static readonly Color WetAsphalt = FromHex("#34495E");
		public static readonly Color MidnightBlue = FromHex("#2C3E50");
		public static readonly Color Concrete = FromHex("#95A5A6");
		public static readonly Color Asbestos = FromHex("#7F8C8D");
		
		
		private static Color FromHex(string hex) => ColorUtility.TryParseHtmlString(hex, out Color color) ? color : Color.clear;
	}
}