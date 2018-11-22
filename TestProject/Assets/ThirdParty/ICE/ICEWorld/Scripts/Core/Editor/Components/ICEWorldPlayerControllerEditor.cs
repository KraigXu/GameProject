// ##############################################################################
//
// ICEWorldPlayerControllerEditor.cs
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
using UnityEditor;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldPlayerController))]
	public class ICEWorldPlayerControllerEditor : ICEWorldBehaviourEditor {

		public override void OnInspectorGUI()
		{
			ICEWorldPlayerController _target = DrawDefaultHeader<ICEWorldPlayerController>();

			DrawWorldPlayerContect( _target );
			DrawDefaultFooter( _target );

		}

		public void DrawWorldPlayerContect( ICEWorldPlayerController _player )
		{

			/*
			if( Application.isPlaying == false )
			{
				_controller.transform.position = new Vector3( 
					_controller.transform.position.x,
					PositionTools.GetGroundLevel( _controller.transform.position, Groudc , GroundLayerMask );
					ICECreatureRegister.Instance.GetGroundLevel(_controller.transform.position), 
					_controller.transform.position.z );
			}*/

			EditorGUILayout.Separator();
			ICEEditorLayout.Label( "Movement", true );
			EditorGUI.indentLevel++;
				_player.WalkSpeed = ICEEditorLayout.DefaultSlider( "Walk Speed", "", _player.WalkSpeed, 0.025f, 0, 25, 3, "" );
				_player.RunSpeed = ICEEditorLayout.DefaultSlider( "Run Speed", "", _player.RunSpeed, 0.025f, 0, 25, 7, "" );
				EditorGUI.indentLevel++;
					_player.RunstepLenghten = ICEEditorLayout.DefaultSlider( "Runstep Lenghten", "", _player.RunstepLenghten, 0.025f, 0, 25, 7, "" );
				EditorGUI.indentLevel--;	
				_player.JumpSpeed = ICEEditorLayout.DefaultSlider( "Jump Speed", "", _player.JumpSpeed, 0.025f, 0, 100, 25, "" );
				EditorGUI.indentLevel++;
					_player.StickToGroundForce = ICEEditorLayout.DefaultSlider( "Stick To Ground Force", "", _player.StickToGroundForce, 0.025f, 0, 25, 7, "" );
					_player.GravityMultiplier = ICEEditorLayout.DefaultSlider( "Gravity", "", _player.GravityMultiplier, 0.025f, 0, 100, 9.825f, "" );
				EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			ICEEditorLayout.Label( "Sounds", true );
			EditorGUI.indentLevel++;
				ICEEditorLayout.Label( "Footsteps", false );
				EditorGUI.indentLevel++;

					Keyframe[] _keys = new Keyframe[3];
					_keys[0] = new Keyframe( 0, 0f );
					_keys[1] = new Keyframe(_player.WalkSpeed, 0.6f);
					_keys[2] = new Keyframe(_player.RunSpeed, 0.3f);
			
					_player.Interval = ICEEditorLayout.DefaultCurve( "Footstep Interval", "", _player.Interval, new AnimationCurve(_keys) );
					for( int _i = 0 ; _i < _player.FootstepSounds.Count ; _i++ )
					{
						AudioClip _clip = _player.FootstepSounds[_i];
						_clip = (AudioClip)EditorGUILayout.ObjectField("Footstep Clip #" + _i, _clip, typeof(AudioClip), false );
						if( _clip == null )
						{
							_player.FootstepSounds.RemoveAt(_i);
							return;
						}
					}
					AudioClip _new_clip = (AudioClip)EditorGUILayout.ObjectField("Add Footstep Clip", null, typeof(AudioClip), false );
					if( _new_clip != null )
						_player.FootstepSounds.Add( _new_clip );
				EditorGUI.indentLevel--;


				
				EditorGUILayout.Separator();
				_player.JumpSound = (AudioClip)EditorGUILayout.ObjectField("Jump", _player.JumpSound, typeof(AudioClip), false );
				_player.LandSound = (AudioClip)EditorGUILayout.ObjectField("Land", _player.LandSound, typeof(AudioClip), false );           // the sound played when character touches back on ground.
			EditorGUI.indentLevel--;

			WorldObjectEditor.DrawMouseLookObject( _player.MouseLook, EditorHeaderType.LABEL_BOLD );

			EditorGUILayout.Separator();
			ICEEditorLayout.Label( "Debug Mode", true );
			EditorGUI.indentLevel++;
				_player.UseDebugMode = ICEEditorLayout.Toggle( "Enable Debug Mode", "", _player.UseDebugMode , "" );
				_player.KeyFlightMode = (KeyCode) ICEEditorLayout.EnumPopup( "Toogle Flight Mode","",  _player.KeyFlightMode );
				EditorGUI.indentLevel++;
					_player.KeyUp = (KeyCode) ICEEditorLayout.EnumPopup( "Up","",  _player.KeyUp );
					_player.KeyDown = (KeyCode) ICEEditorLayout.EnumPopup( "Down","",  _player.KeyDown );
					_player.FlightSpeedMultiplier = (int)ICEEditorLayout.DefaultSlider( "Flight Speed Multiplier", "", _player.FlightSpeedMultiplier, 1, 1, 100, 10, "" );
				EditorGUI.indentLevel--;
			EditorGUI.indentLevel--;
		}
	}
}
