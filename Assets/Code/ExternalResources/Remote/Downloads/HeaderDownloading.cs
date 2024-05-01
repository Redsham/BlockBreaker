using System.Text;
using UnityEngine;

namespace ExternalResources.Remote.Downloads
{
	public class HeaderDownloading : Downloading<StorageHeader>
	{
		public HeaderDownloading(string url) : base(url) { }
		protected override void OnSuccessfulDownload() => Data = StorageHeader.FromJson(Encoding.UTF8.GetString(RawData));
	}
}