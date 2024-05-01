using UnityEngine;

namespace ExternalResources.Remote.Downloads
{
	public class TextureDownloading : Downloading<Texture2D>
	{
		public TextureDownloading(string url) : base(url) { }
		protected override void OnSuccessfulDownload()
		{
			Data = new Texture2D(0, 0);
			Data.LoadImage(RawData);
		}
	}
}