using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Bootstrapping
{
	[DefaultExecutionOrder(-1000)]
	public class Bootstraper : MonoBehaviour
	{
		public static Bootstraper Active { get; private set; }
		
		private void Awake() => Active = this;
		private IEnumerator Start()
		{
			yield return Bootstrap();
			yield return SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
			Debug.Log("Bootstraper: Bootstrapping completed.");
		}


		public IEnumerator Bootstrap()
		{
			IEnumerable<BootstrapTarget> bootstraps = CollectBootstrapsInstances();
			IEnumerable<BootstrapTarget> bootstrapMethods = CollectBootstrapMethods();

			BootstrapTarget[] allBootstraps = bootstraps.Concat(bootstrapMethods).OrderBy(x => -x.Order).ToArray();
			foreach (BootstrapTarget bootstrap in allBootstraps)
			{
				IEnumerator coroutine = bootstrap.Bootstrap();
				if (coroutine != null)
					yield return coroutine;
			}
		}
		
		private static IEnumerable<BootstrapTarget> CollectBootstrapsInstances()
		{
			List<Type> bootstrapTypes = new();
			
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

			List<BootstrapTarget> bootstrapInstances = new();
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
			List<BootstrapTarget> bootstrapMethods = new();
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (Type type in assembly.GetTypes())
				{
					if (type.IsInterface || (type.IsAbstract && !type.IsSealed))
						continue;
					
					foreach (MethodInfo method in type.GetMethods())
					{
						if (!method.IsStatic)
							continue;
					
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