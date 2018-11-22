// ##############################################################################
//
// ICE.World.ICEWorldEntityDebug.cs
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

using UnityEditor;
using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

namespace ICE.World
{
	[RequireComponent (typeof (ICEWorldEntityDebug))]
	public class ICEWorldEntityDebugEditor : ICEWorldBehaviourDebugEditor
	{
		public override void OnInspectorGUI()
		{
			ICEWorldEntityDebug _target = DrawDefaultHeader<ICEWorldEntityDebug>();
			DrawEntityDebugContent( _target );
			DrawDefaultFooter( _target );
		}

		public virtual void DrawEntityDebugContent( ICEWorldEntityDebug _target )
		{
			if( _target == null )
				return;


			_target.BaseOffset = ICEEditorLayout.DrawBaseOffsetGround( _target.transform, "Base Offset", "", _target.BaseOffset, ref _target.BaseOffsetMaximum, ref _target.GroundedInEditorMode, "" );

			_target.DrawSelectedOnly = ICEEditorLayout.Toggle( "Draw Selected Only", "" , _target.DrawSelectedOnly , "" );

			EditorGUILayout.Separator();
			_target.GizmoSize = ICEEditorLayout.DefaultSlider( "Gizmo Size", "", _target.GizmoSize, 0.01f, 0, 10, 0.25f, "");
		
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.UseCustomGizmoColor == false );
					_target.CustomGizmoColor = ICEEditorLayout.DefaultColor( "Custom Gizmo Color", "", _target.CustomGizmoColor, Color.red, "");
				EditorGUI.EndDisabledGroup();
				_target.UseCustomGizmoColor = ICEEditorLayout.EnableButton( _target.UseCustomGizmoColor );
			ICEEditorLayout.EndHorizontal( "" );
		}
	}
}
