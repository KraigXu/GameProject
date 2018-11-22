// ##############################################################################
//
// ICEWorldBehaviourEditor.cs | ICEWorldBehaviourEditor : Editor
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
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;


namespace ICE.World
{
	[CustomEditor(typeof( ICEWorldCamera))]
	public class ICEWorldCameraEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICEWorldCamera _target = DrawDefaultHeader< ICEWorldCamera>();
			DrawCameraContent( _target );
			DrawDefaultFooter( _target );

		}

		public virtual void DrawCameraContent( ICEWorldCamera _target )
		{
			WorldObjectEditor.DrawUnderwaterCameraEffect( _target, _target.Underwater, m_HeaderType );
		}
	}
}

