// ##############################################################################
//
// ICEWorldSingletonEditor.cs | ICEWorldBehaviourEditor : Editor
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
using ICE.World.EditorInfos;
using ICE.World.Utilities;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldSingleton))]
	public class ICEWorldSingletonEditor : ICEWorldBehaviourEditor 
	{
		public override void OnInspectorGUI()
		{
			ICEWorldBehaviour _target = DrawDefaultHeader<ICEWorldBehaviour>();

			DrawDefaultFooter( _target );

		}
	}
}

