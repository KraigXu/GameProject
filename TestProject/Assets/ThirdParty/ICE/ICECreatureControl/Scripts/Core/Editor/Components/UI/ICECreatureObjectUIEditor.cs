// ##############################################################################
//
// ICECreatureObjectUIEditor.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.Attributes;
using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures.UI
{
	[CustomEditor(typeof(ICECreatureObjectUI))]
	public class ICECreatureObjectUIEditor : ICEWorldEntityStatusUIEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICECreatureObjectUI _target = DrawEntityStatusUIHeader<ICECreatureObjectUI>();
			DrawObjectUIContent( _target );
			DrawFooter( _target );
		}


		protected virtual void DrawObjectUIContent( ICECreatureObjectUI _ui )
		{
			if( _ui == null )
				return;

		}
	}
}
