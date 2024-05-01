using ExternalResources.Data;
using UnityEngine;

namespace ExternalResources.Remote
{
	public class StorageHeader
	{
		public string  Version;
		public Model[] Models;
		
		
		public static StorageHeader FromJson(string json) => JsonUtility.FromJson<StorageHeader>(json);
	}
}