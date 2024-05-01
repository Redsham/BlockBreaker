using System;
using System.Collections;
using System.Reflection;

namespace Bootstrapping
{
	internal class BootstrapTarget
	{
		public int Order { get; }
		private IBootstrappble Instance { get; }
		private MethodInfo Method { get; }
			
			
		public BootstrapTarget(int order, IBootstrappble instance, MethodInfo method)
		{
			if (instance == null && method == null)
				throw new Exception("Instance and method cannot be null at the same time.");
				
			Order = order;
			Instance = instance;
			Method = method;
		}
			
			
		public IEnumerator Bootstrap()
		{
			if (Instance != null)
				return Instance.Bootstrap();
			
			
			bool isEnumerator = Method.ReturnType == typeof(IEnumerator);
			if (!isEnumerator && Method.ReturnType != typeof(void))
				throw new Exception($"Bootstrap method ({Method.DeclaringType}.{Method.Name}) must return IEnumerator.");
			
			
			ParameterInfo[] parameters = Method.GetParameters();
			if (parameters.Length != 0)
				throw new Exception($"Bootstrap method ({Method.DeclaringType}.{Method.Name}) must have no parameters.");
			
			
			object result = Method.Invoke(null, null);
			return isEnumerator ? (IEnumerator)result : null;
		}
	}

}