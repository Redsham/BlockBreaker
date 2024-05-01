namespace ExternalResources
{
	public enum ModelComponent
	{
		Meta = 1,
		Thumbnail = 2,
		Asset = 4,
		
		Full = Meta | Thumbnail | Asset
	}
}