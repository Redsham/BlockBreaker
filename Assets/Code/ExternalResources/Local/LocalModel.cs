using ExternalResources.Data;

namespace ExternalResources.Local
{
	[System.Serializable]
	public class LocalModel : Model
	{
		public bool MetaDownloaded;
		public bool ThumbnailDownloaded;
		public bool AssetDownloaded;
	}
}