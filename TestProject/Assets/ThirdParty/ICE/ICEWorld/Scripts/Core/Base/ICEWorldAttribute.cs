// ##############################################################################
//
// ICE.World.ICEWorldAttribute.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	/// <summary>
	/// ICEWorldAttribute is the base class for all ICEWorld based attributes. Attributes represents specific data classes 
	/// to enhance ICEWorldEntity based objects.
	/// </summary>
	public class ICEWorldAttribute : ICEWorldBehaviour {

		/// <summary>
		/// Gets the attributes.
		/// </summary>
		/// <returns>The attributes.</returns>
		public ICEWorldAttribute[] GetAttributes()
		{
			return transform.GetComponents<ICEWorldAttribute>();
		}

		/// <summary>
		/// Gets the attributes in children.
		/// </summary>
		/// <returns>The attributes in children.</returns>
		public ICEWorldAttribute[] GetAttributesInChildren()
		{
			return transform.GetComponentsInChildren<ICEWorldAttribute>();
		}

		/// <summary>
		/// Gets the attributes in parent.
		/// </summary>
		/// <returns>The attributes in parent.</returns>
		public ICEWorldAttribute[] GetAttributesInParent()
		{
			return transform.GetComponentsInParent<ICEWorldAttribute>();
		}
	}
}
