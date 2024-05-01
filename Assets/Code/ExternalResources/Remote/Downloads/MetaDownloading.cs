using System.Text;
using ExternalResources.Data;

namespace ExternalResources.Remote.Downloads
{
	public class MetaDownloading : Downloading<ModelMeta>
	{
		public MetaDownloading(string url) : base(url) { }
		protected override void OnSuccessfulDownload() => Data = ModelMeta.FromJson(Encoding.UTF8.GetString(RawData));
	}
}