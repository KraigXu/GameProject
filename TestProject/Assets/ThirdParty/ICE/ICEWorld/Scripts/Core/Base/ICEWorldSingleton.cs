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
	public abstract class ICEWorldSingleton : ICEWorldBehaviour {

		protected static ICEWorldSingleton m_Instance = null;
		public static ICEWorldSingleton Instance{
			get { return m_Instance = ( m_Instance == null ? GameObject.FindObjectOfType<ICEWorldSingleton>():m_Instance ); }
		}
	}
}