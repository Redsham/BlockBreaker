using System;
using System.Reflection;

namespace Bootstrapping
{
	[AttributeUsage(AttributeTargets.Method)]
	public class BootstrapMethodAttribute : Attribute
	{
		public int Order { get; }
		public MethodInfo Method { get; set; }
			
			
		public BootstrapMethodAttribute(int order = 0) => Order = order;
	}
}