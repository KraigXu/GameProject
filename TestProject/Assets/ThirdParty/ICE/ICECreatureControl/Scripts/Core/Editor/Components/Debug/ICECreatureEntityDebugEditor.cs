// ##############################################################################
//
// ICECreatureEntityDebugEditor.cs
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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureEntityDebug))]
	public class ICECreatureEntityDebugEditor : ICEWorldEntityDebugEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureEntityDebug _target = DrawDefaultHeader<ICECreatureEntityDebug>();

			EditorGUILayout.HelpBox( "Use the debug options of the related ICECreatureEntity component to adapt the settings and please note, that this component is part of " +
				"the ICECreatureEntity and will be used automatically by it, so please don't assign it manual to other GameObjects. ",MessageType.None );

			DrawEntityDebugContent( _target );
			DrawDefaultFooter( _target );
		}
	}
}