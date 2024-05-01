using UnityEngine;

namespace ExternalResources.Data
{
	public struct ModelMeta
	{
		public string Color;
		public uint   Cost;
		public string Author;


		public static ModelMeta FromJson(string json) => JsonUtility.FromJson<ModelMeta>(json);
	}
}