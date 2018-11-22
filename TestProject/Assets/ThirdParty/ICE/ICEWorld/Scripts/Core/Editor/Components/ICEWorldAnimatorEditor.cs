// ##############################################################################
//
// ICEWorldAnimatorEditor.cs | ICEWorldBehaviourEditor : Editor
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

	[CustomEditor(typeof(ICEWorldAnimator))]
	public class ICEWorldAnimatorEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICEWorldAnimator _target = DrawDefaultHeader<ICEWorldAnimator>();
			DrawAnimatorContent( _target );
			DrawDefaultFooter( _target );

		}

		public virtual void DrawAnimatorContent( ICEWorldAnimator _target )
		{
			_target.AxisInput = WorldPopups.InputPopup( _target, _target.AxisInput, "", "Axis" );

			_target.Parameter = ICEEditorLayout.AnimatorParametersPopup( _target.AnimatorComponent, "Parameter", "", _target.Parameter );
			_target.HandleColliders = ICEEditorLayout.Toggle( "Handle Colliders", "Enables and disables child colliders", _target.HandleColliders );
		}
	}
}

