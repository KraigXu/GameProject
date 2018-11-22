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
	[CustomEditor(typeof( ICESmoothCameraOrbit))]
	public class ICESmoothCameraOrbitEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICESmoothCameraOrbit _target = DrawDefaultHeader< ICESmoothCameraOrbit>();
			DrawSmoothCameraOrbitContent( _target );
			DrawDefaultFooter( _target );

		}

		public virtual void DrawSmoothCameraOrbitContent( ICESmoothCameraOrbit _target )
		{
			_target.CameraTarget = (Transform)EditorGUILayout.ObjectField( "Camera Target", _target.CameraTarget, typeof(Transform), true );
			EditorGUI.indentLevel++;
			_target.CameraTargetOffset = ICEEditorLayout.Vector3Field( "Offset", "", _target.CameraTargetOffset, "" );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();


			ICEEditorLayout.MinMaxDefaultSlider( "Distances (min/max)", "", ref _target.MinDistance, ref _target.MaxDistance, 0, ref _target.DistanceMaximum, 0.6f, 20, Init.DECIMAL_PRECISION_DISTANCES, 40, "" ); 
			EditorGUI.indentLevel++;
				_target.Distance = ICEEditorLayout.MaxDefaultSlider( "Initial Distance", "", _target.Distance, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _target.DistanceMaximum, 5, "" );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();

			ICEEditorLayout.MinMaxDefaultSlider( "Vertical Limits (min/max °)", "", ref _target.VerticalMinLimit, ref _target.VerticalMaxLimit, - _target.VerticalLimitsMaximum, ref _target.VerticalLimitsMaximum, -80, 80, 40, "" ); 
			EditorGUILayout.Separator();

			_target.FollowSpeed = ICEEditorLayout.MaxDefaultSlider( "Follow Speed", "", _target.FollowSpeed, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.FollowSpeedMaximum, 0, "" );

			_target.HorizontalSpeed = ICEEditorLayout.MaxDefaultSlider( "Horizontal Speed", "", _target.HorizontalSpeed, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.HorizontalSpeedMaximum, 200, "" );
			_target.VerticalSpeed = ICEEditorLayout.MaxDefaultSlider( "Vertical Speed", "", _target.VerticalSpeed, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.VerticalSpeedMaximum, 200, "" );
			EditorGUILayout.Separator();

			_target.ZoomRate = ICEEditorLayout.MaxDefaultSlider( "Zoom Rate", "", _target.ZoomRate, 0, ref _target.ZoomRateMaximum, 40, "" );
			EditorGUI.indentLevel++;
			_target.ZoomDampening = ICEEditorLayout.MaxDefaultSlider( "Zoom Dampening", "", _target.ZoomDampening, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.ZoomDampeningMaximum, 5f, "" );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
			//_target.PanSpeed = ICEEditorLayout.MaxDefaultSlider( "Pan Speed", "", _target.PanSpeed, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.PanSpeedMaximum, 0.3f, "" );
			_target.AutoRotate = ICEEditorLayout.MaxDefaultSlider( "Auto Rotate", "", _target.AutoRotate, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.AutoRotateMaximum, 1f, "" );
			EditorGUI.indentLevel++;
			_target.AutoRotateSpeed = ICEEditorLayout.MaxDefaultSlider( "Auto Rotate Speed", "", _target.AutoRotateSpeed, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _target.AutoRotateSpeedMaximum, 0.1f, "" );
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
		}
	}
}

