using UnityEngine;

namespace ExternalResources.Data
{
	public class ModelMeta
	{
		public string Color;
		public uint   Cost;
		public string Author;

		public Color GetColor() => ColorUtility.TryParseHtmlString(Color, out Color color) ? color : UnityEngine.Color.gray;
		public static ModelMeta FromJson(string json) => JsonUtility.FromJson<ModelMeta>(json);
	}
}