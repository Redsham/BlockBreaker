using Voxels;

namespace ExternalResources.Remote.Downloads
{
	public class ModelDownloading : Downloading<ModelAsset>
	{
		public ModelDownloading(string url) : base(url) { }
		protected override void OnSuccessfulDownload() => Data = ModelAsset.Deserialize(RawData);
	}
}