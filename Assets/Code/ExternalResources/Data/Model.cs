using UnityEngine;

namespace ExternalResources.Data
{
	[System.Serializable]
	public class Model
	{
		public string Id;
		public byte   Version;

		internal ModelMeta Meta;
	}
}