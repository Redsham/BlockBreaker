using System.Collections;

namespace Bootstrapping
{
	public interface IBootstrappble
	{
		int Order { get; }
		IEnumerator Bootstrap();
	}
}