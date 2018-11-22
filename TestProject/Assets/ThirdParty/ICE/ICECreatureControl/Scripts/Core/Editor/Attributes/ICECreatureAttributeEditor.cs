// ##############################################################################
//
// ICECreatureAttributeEditor.cs
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
using UnityEditor;
using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;



namespace ICE.Creatures.Attributes
{
	[CustomEditor(typeof(ICECreatureAttribute)), CanEditMultipleObjects]
	public class ICECreatureAttributeEditor : ICEWorldBehaviourEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureAttribute _attribute = DrawEntityHeader<ICECreatureAttribute>();

			DrawFooter( _attribute );
		}

		/// <summary>
		/// Draws the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawEntityHeader<T>() where T : ICECreatureAttribute
		{
			T _target = (T)target;

			GUI.changed = false;
			Info.HelpButtonIndex = 0;

			EditorGUILayout.Separator();

			return _target;
		}

		/// <summary>
		/// Draws the footer.
		/// </summary>
		/// <param name="_target">Target.</param>
		public virtual void DrawFooter( ICECreatureAttribute _target )
		{
			if( _target == null )
				return;

			EditorGUILayout.Separator();
			MarkSceneDirty( _target );
		}
	}
}
