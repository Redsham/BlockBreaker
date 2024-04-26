using UnityEngine;
using UnityEngine.Networking;

namespace RemoteResources
{
	public class TextureDownloading : Downloading
	{
		public Texture2D Texture { get; private set; }
		public TextureDownloading(string url) : base(url) { }

		protected override void OnSuccessfulDownloaded()
		{
			Texture = new Texture2D(0, 0);
			Texture.LoadImage(Data);
		}
	}
}