using Voxels;

namespace RemoteResources.Downloadings
{
	public class ModelDownloading : Downloading
	{
		public ModelAsset Model { get; private set; }
		public ModelDownloading(string url) : base(url) { }
		
		protected override void OnSuccessfulDownloaded() => Model = ModelAsset.Deserialize(Data);
	}
}