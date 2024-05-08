using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Bootstrapping
{
	[DefaultExecutionOrder(-1000)]
	public class Bootstrapper : MonoBehaviour
	{
		public static Bootstrapper Active { get; private set; }
		
		public UnityEvent OnCompleted = new UnityEvent();
		
		private void Awake() => Active = this;
		
		public void Bootstrap() => StartCoroutine(BootstrapCoroutine());
		
		private IEnumerator BootstrapCoroutine()
		{
			Debug.Log("[Bootstraper] Bootstrapping started.");
			
			IEnumerable<BootstrapTarget> bootstraps = CollectBootstrapsInstances();
			IEnumerable<BootstrapTarget> bootstrapMethods = CollectBootstrapMethods();

			BootstrapTarget[] allBootstraps = bootstraps.Concat(bootstrapMethods).OrderBy(x => -x.Order).ToArray();
			foreach (BootstrapTarget bootstrap in allBootstraps)
			{
				IEnumerator coroutine = bootstrap.Bootstrap();
				if (coroutine != null)
					yield return coroutine;
			}
			
			Debug.Log("[Bootstraper] Bootstrapping completed successfully.");
			
			OnCompleted.Invoke();
		}
		
		private static IEnumerable<BootstrapTarget> CollectBootstrapsInstances()
		{
			List<Type> bootstrapTypes = new List<Type>();
			
			// Поиск всех типов, реализующих интерфейс IBootstrap
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if ((type.IsInterface || (type.IsAbstract && !type.IsSealed)) && !type.IsSubclassOf(typeof(MonoBehaviour)))
						continue;
					
					if (type.GetInterfaces().Contains(typeof(IBootstrappble)))
						bootstrapTypes.Add(type);
				}
			}

			List<BootstrapTarget> bootstrapInstances = new List<BootstrapTarget>();
			foreach (Type type in bootstrapTypes)
			{
				// Поиск объекта в сцене
				object instance = FindObjectOfType(type);
					
				// Если объект не найден, то выводим предупреждение
				if (instance == null)
				{
					Debug.LogWarning($"Bootstrapper: {type.Name} not found in scene.");
					continue;
				}
					
				IBootstrappble bootstrappble = (IBootstrappble)instance;
					
				// Добавляем экземпляр в список
				bootstrapInstances.Add(new BootstrapTarget(bootstrappble.Order, bootstrappble, null));
			}

			return bootstrapInstances;
		}
		private static IEnumerable<BootstrapTarget> CollectBootstrapMethods()
		{
			List<BootstrapTarget> bootstrapMethods = new List<BootstrapTarget>();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsInterface || (type.IsAbstract && !type.IsSealed))
						continue;
					
					foreach (MethodInfo method in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
					{
						object[] attributes = method.GetCustomAttributes(typeof(BootstrapMethodAttribute), false);
					
						if (attributes.Length == 0)
							continue;

						BootstrapMethodAttribute attribute = (BootstrapMethodAttribute)attributes[0];
						attribute.Method = method;
						bootstrapMethods.Add(new BootstrapTarget(attribute.Order, null, method));
					}
				}
			}

			return bootstrapMethods;
		}
	}
}