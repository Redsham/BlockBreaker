using UnityEngine;

namespace RemoteResources.Downloadings
{
	public class MetaDownloading : Downloading
	{
		public MetaResource Meta { get; private set; }
		public MetaDownloading(string url) : base(url) { }

		protected override void OnSuccessfulDownloaded() => Meta = JsonUtility.FromJson<MetaResource>(WebRequest.downloadHandler.text);
	}
}