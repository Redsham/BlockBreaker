using RemoteResources.Data;
using UnityEngine;

namespace RemoteResources.Downloadings
{
	public class MetaDownloading : Downloading
	{
		public ModelMeta ModelMeta { get; private set; }
		public MetaDownloading(string url) : base(url) { }

		protected override void OnSuccessfulDownloaded() => ModelMeta = JsonUtility.FromJson<ModelMeta>(WebRequest.downloadHandler.text);
	}
}