using System.Collections.Generic;
using UnityEngine;

namespace ExternalResources.Local
{
	public class StorageHeader
	{
		public List<LocalModel> Models = new();
		
		public static StorageHeader FromJson(string json) => JsonUtility.FromJson<StorageHeader>(json);
		public string ToJson() => JsonUtility.ToJson(this, true);
	}
}