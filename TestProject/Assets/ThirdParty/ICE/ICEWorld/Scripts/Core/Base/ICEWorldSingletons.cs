// ##############################################################################
//
// ICEWorldSingleton.cs | ICEWorldSingleton
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using System.Collections;

namespace ICE.World
{
	/// <summary>
	/// ICE World. 
	/// </summary>
	public abstract class ICEWorldSingletons<T> : ICEWorldBehaviour where T : ICEWorldBehaviour
	{
		private static T m_Instance;

		private static object m_Lock = new object();

		public static T Instance
		{
			get
			{
				if( m_ApplicationIsQuitting ) 
				{
					ICEDebug.LogWarning("[ICEWorldSingletons] Instance '"+ typeof(T) +
						"' already destroyed on application quit." +
						" Won't create again - returning null.");
					return null;
				}

				lock( m_Lock )
				{
					if( m_Instance == null )
					{
						m_Instance = (T) FindObjectOfType(typeof(T));

						if ( FindObjectsOfType(typeof(T)).Length > 1 )
						{
							ICEDebug.LogError("[ICEWorldSingletons] Something went really wrong " +
								" - there should never be more than 1 singleton!" +
								" Reopening the scene might fix it.");
							return m_Instance;
						}

						if (m_Instance == null)
						{
							GameObject _singleton = new GameObject();
							m_Instance = _singleton.AddComponent<T>();
							_singleton.name = "(ICEWorldSingletons) "+ typeof(T).ToString();

							DontDestroyOnLoad(_singleton);

							ICEDebug.Log("[ICEWorldSingletons] An instance of " + typeof(T) + 
								" is needed in the scene, so '" + _singleton +
								"' was created with DontDestroyOnLoad.");
						} else {
							ICEDebug.Log("[ICEWorldSingletons] Using instance already created: " +
								m_Instance.gameObject.name);
						}
					}

					return m_Instance;
				}
			}
		}

		private static bool m_ApplicationIsQuitting = false;
		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		public override void OnDestroy () {
			m_ApplicationIsQuitting = true;
		}

	}
}