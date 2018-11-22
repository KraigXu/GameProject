// ##############################################################################
//
// ice_editor_animation.cs | AnimationEditor
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
using ICE.World.EnumTypes;

using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World.EditorUtilities
{
	public class AnimationEditor : WorldObjectEditor
	{	

		/// <summary>
		/// Gets the name of the animation state.
		/// </summary>
		/// <returns>The animation state name.</returns>
		/// <param name="_animator">Animator.</param>
		/// <param name="_name">Name.</param>
		public static string GetAnimatorStateName( Animator _animator, string _name )
		{
			if( string.IsNullOrEmpty( _name ) || _animator == null || _animator.runtimeAnimatorController == null )
				return "";
			#if UNITY_EDITOR
			UnityEditor.Animations.AnimatorController _controller = _animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;

			foreach( UnityEditor.Animations.AnimatorControllerLayer _layer in _controller.layers )
			{
				UnityEditor.Animations.AnimatorStateMachine _state_machine = _layer.stateMachine;

				foreach( UnityEditor.Animations.ChildAnimatorState _state in _state_machine.states )
				{
					if( _state.state != null && _state.state.motion != null && _state.state.motion.name == _name )
						return _state.state.name;
				}
			}
			#endif
			return "";
		}

		public static string AnimationPopup( Animation _animation, string _name, string _title = "", string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();
				_name = AnimationPopupBase( _animation, _name, _title );

				if( ICEEditorLayout.SystemButtonSmall( "SEL", "" ) )
				{
					AnimationState _state = AnimationTools.GetAnimationStateByName( _animation, _name );	

					if( _state != null && _state.clip != null )
						Selection.activeObject = _state.clip;
				}	
			ICEEditorLayout.EndHorizontal( _help );

			return _name;
		}

		public static string AnimationPopupBase( Animation _animation, string _name, string _title = "" )
		{
			if( _animation == null )
				return "";
			
			int _count = AnimationTools.GetAnimationClipCount( _animation );
			string[] _animation_names = new string[ _count ];
			int[] _animation_index = new int[ _count];

			int _i = 0;
			int _selected = 0;
			if( _count > 0 )
			{
				foreach( AnimationState _state in _animation )
				{
					if( _state == null || _i >= _count )
						continue;

					_animation_index[_i] = _i;
					_animation_names[_i] = _state.name;

					if( _name == _animation_names[_i] )
						_selected = _i;

					_i++;
				}
			}

			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _count - 1 )
				_selected = _count - 1;

			return _animation_names[_selected];
		}

		private static int AnimatorIntPopup( Animator _animator, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
				_selected = AnimatorIntPopupBase( _animator, _selected, _title );

				if( ICEEditorLayout.SystemButtonSmall( "SEL", "" ) )
				{
					AnimationClip _animation_clip = AnimationTools.GetAnimationClipByIndex( _animator, _selected );

					if( _animation_clip != null )
						Selection.activeObject = _animation_clip;
				}		

			ICEEditorLayout.EndHorizontal( Info.ANIMATION_ANIMATOR_POPUP );

			return _selected;
		}

		public static int AnimatorIntPopupBase( Animator _animator, int _selected, string _title = "" )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null ) 
				return -1;


			AnimationClip[] _clips = AnimationTools.GetAnimationClips( _animator );

			string[] _animation_names = new string[ _clips.Length ];//_animator.runtimeAnimatorController.animationClips.Length ];
			int[] _animation_index = new int[  _clips.Length ];//_animator.runtimeAnimatorController.animationClips.Length ];

			int i = 0;							
			foreach( AnimationClip _clip in _clips )// _animator.runtimeAnimatorController.animationClips )
			{
				_animation_index[i] = i;
				_animation_names[i] = _clip.name;

				i++;
			}

			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= _clips.Length - 1 )
				_selected = _clips.Length - 1;

			return _selected;
		}

		public static int AnimationIntPopup( Animation _animation, int _selected, string _title = "" )
		{
			ICEEditorLayout.BeginHorizontal();
			_selected = AnimationIntPopupBase( _animation, _selected, _title );
			ICEEditorLayout.EndHorizontal();

			return _selected;
		}

		public static int AnimationIntPopupBase( Animation _animation, int _selected, string _title = "" )
		{
			if( _animation == null )
				return 0;



			string[] _animation_names = new string[ AnimationTools.GetAnimationClipCount( _animation ) ];
			int[] _animation_index = new int[ AnimationTools.GetAnimationClipCount( _animation ) ];

			int i = 0;	
			foreach (AnimationState _animation_state in _animation )
			{
				_animation_index[i] = i;
				_animation_names[i] = _animation_state.name;

				i++;
			}
			if( _title == "" )
				_title = "Animation";

			_selected = (int)EditorGUILayout.IntPopup( _title , _selected, _animation_names,_animation_index);
			//new GUIContent( _title , "Animation name and length in seconds" )

			_selected = (int)ICEEditorLayout.PlusMinusGroup( _selected, 1, ICEEditorStyle.CMDButtonDouble );

			if( _selected < 0 )
				_selected = 0;
			else if( _selected >= AnimationTools.GetAnimationClipCount( _animation ) - 1 )
				_selected = AnimationTools.GetAnimationClipCount( _animation )  - 1;

			return _selected;
		}


		public static void DrawAnimationDataObject( ICEWorldBehaviour _component, AnimationDataObject _anim, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "", List<AnimationDataObject> _list = null )
		{
			if( _anim == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Animation";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ANIMATION;

			//--------------------------------------------------
			// ANIMATION
			//--------------------------------------------------

			bool _enabled = _anim.Enabled;
			//_anim.Enabled = (_anim.InterfaceType != AnimationInterfaceType.NONE?true:false);

			ICEEditorLayout.BeginHorizontal();

				if( WorldObjectEditor.IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.BeginDisabledGroup( _anim.Enabled == false );
				}		

				WorldObjectEditor.DrawObjectHeaderLine( _anim, WorldObjectEditor.GetSimpleFoldout( _type ), _title , _hint );		
				GUILayout.FlexibleSpace();
				//EditorGUI.BeginDisabledGroup( _anim.Enabled == false );
					_anim.AllowInterfaceSelector = ! ICEEditorLayout.CheckButtonMiddle( "AUTO", "Automatic Interface Selection", ! _anim.AllowInterfaceSelector );
				//EditorGUI.EndDisabledGroup();

				if( _list != null && _list.Count > 1 )
				{
					if( ICEEditorLayout.Button( "SHARE", "Use this move settings for all associated rules" ) )
					{
						foreach( AnimationDataObject _data in _list )
							_data.Copy( _anim );
					}
				}

				if( WorldObjectEditor.IsEnabledFoldoutType( _type ) )
				{
					EditorGUI.EndDisabledGroup();
					_anim.Enabled = ICEEditorLayout.EnableButton( _anim.Enabled );
				}

			ICEEditorLayout.EndHorizontal( _help );

			if( _enabled != _anim.Enabled && _anim.Enabled == true )
				_anim.Foldout = true;



			// CONTENT BEGIN
			if( WorldObjectEditor.BeginObjectContentOrReturn( _type, _anim ) )
				return;

			if( ( _component.GetComponentInChildren<Animator>() != null && _component.GetComponentInChildren<Animation>() != null ) || _anim.AllowInterfaceSelector )
			{
				if( _anim.InterfaceType == AnimationInterfaceType.NONE && _component.GetComponentInChildren<Animator>() != null && _component.GetComponentInChildren<Animator>().runtimeAnimatorController != null )
					_anim.InterfaceType = AnimationInterfaceType.MECANIM;
				else if( _anim.InterfaceType == AnimationInterfaceType.NONE )
					_anim.InterfaceType = AnimationInterfaceType.LEGACY;

				_help = Info.ANIMATION_NONE;
				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM )
					_help = Info.ANIMATION_ANIMATOR;
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					_help = Info.ANIMATION_ANIMATION;
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					_help = Info.ANIMATION_CLIP;
				else if( _anim.InterfaceType == AnimationInterfaceType.CUSTOM )
					_help = Info.ANIMATION_CUSTOM;

				_anim.InterfaceType = (AnimationInterfaceType)ICEEditorLayout.EnumPopup( "Interface","", _anim.InterfaceType , _help );
			}
			else if( _component.GetComponentInChildren<Animator>() )
				_anim.InterfaceType = AnimationInterfaceType.MECANIM;
			else 
				_anim.InterfaceType = AnimationInterfaceType.LEGACY;

			if( _anim.InterfaceType != AnimationInterfaceType.NONE )
			{
				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM )
					_anim.Animator = DrawBehaviourAnimationAnimatorData( _component, _anim.Animator );
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					_anim.Animation = DrawBehaviourAnimationAnimationData( _component,_anim.Animation );
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					_anim.Clip = DrawBehaviourAnimationAnimationClipData( _component, _anim.Clip );
				else if( _anim.InterfaceType == AnimationInterfaceType.CUSTOM )
					Info.Help ( _help );

				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM && _anim.Animator.Type == AnimatorControlType.DIRECT )
					DrawAnimationEventData( _component, _anim.Events, AnimationTools.GetAnimationClipByAnimatorAndName( AnimationTools.TryGetAnimatorComponent( _component.gameObject ), _anim.GetAnimationName() ), EditorHeaderType.FOLDOUT_ENABLED );
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					DrawAnimationEventData( _component, _anim.Events, AnimationTools.GetAnimationClipByName( AnimationTools.TryGetAnimationComponent( _component.gameObject ), _anim.GetAnimationName() ), EditorHeaderType.FOLDOUT_ENABLED );
				else if( _anim.InterfaceType == AnimationInterfaceType.CLIP )
					DrawAnimationEventData( _component, _anim.Events, _anim.Clip.Clip, EditorHeaderType.FOLDOUT_ENABLED );				
			}
			else
				Info.Help ( Info.ANIMATION_NONE );

			WorldObjectEditor.EndObjectContent();
			// CONTENT END
		}


		private static AnimationClipInterface DrawBehaviourAnimationAnimationClipData( ICEWorldBehaviour _control, AnimationClipInterface _clip )
		{
			Animation m_animation = _control.GetComponentInChildren<Animation>();

			if( m_animation != null && m_animation.enabled == true )
			{
				Info.Help ( Info.ANIMATION_CLIP );

				_clip.Clip = (AnimationClip)EditorGUILayout.ObjectField( "Animation Clip", _clip.Clip, typeof(AnimationClip), false );

				if( _clip.Clip != null )
				{
					ICEEditorLayout.Label( "Length", "Animation length in seconds. ", _clip.Clip.length.ToString() + " secs." );
					ICEEditorLayout.Label( "Frame Rate", "This is the frame rate that was used in the animation program you used to create the animation or model.", _clip.Clip.frameRate.ToString() + " secs." );

					_clip.Clip.legacy = ICEEditorLayout.Toggle( "Legacy", "Set to true to use it here with the Legacy Animation component",_clip.Clip.legacy );
					_clip.Clip.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve." , _clip.Clip.wrapMode );
			
					_clip.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _clip.TransitionDuration, 0.01f, 0, 10, ref _clip.AutoTransitionDuration, 0.05f  );

					if( _clip.AutoTransitionDuration )
						_clip.TransitionDuration = _clip.Clip.length / 3;

				}
			}
			else
			{
				EditorGUILayout.HelpBox( "Check your Animation Component", MessageType.Warning ); 
			}

			return _clip;
		}

		public static void DrawAnimationEventData( ICEWorldBehaviour _component, AnimationEventsObject _events, AnimationClip _clip, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _events == null || _clip == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Animation Events";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.ANIMATION_EVENTS;

			ICEEditorLayout.BeginHorizontal();

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.BeginDisabledGroup( _events.Enabled == false );
			}			

			DrawObjectHeaderLine( _events, GetSimpleFoldout( _type ), _title, _hint );

			EditorGUI.BeginDisabledGroup( _events.Enabled == false );
				if( ICEEditorLayout.AddButtonSmall( "" ) )
					_events.Events.Add( new AnimationEventObject() );

				EditorGUI.BeginDisabledGroup( _events.Events.Count == 0 );
					if( ICEEditorLayout.ClearButtonSmall<AnimationEventObject>( _events, _events.Events, "" ) )
						AnimationUtility.SetAnimationEvents( _clip, _events.GetAnimationEvents() );											
				EditorGUI.EndDisabledGroup();
			EditorGUI.EndDisabledGroup();				

			if( IsEnabledFoldoutType( _type ) )
			{
				EditorGUI.EndDisabledGroup();
				bool _enabled = ICEEditorLayout.EnableButton( _events.Enabled );

				if( _enabled != _events.Enabled )
				{
					_events.Enabled = _enabled;
					if( _events.Enabled == false )
						AnimationUtility.SetAnimationEvents( _clip, new AnimationEvent[0] );						
				}
			}
			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _events ) )
				return;

			AnimationEvent[] _clip_events = AnimationUtility.GetAnimationEvents( _clip );

			_events.UpdateAnimationEvents( _clip_events );

			for( int i = 0 ; i < _events.Events.Count ; i++ )
			{
				AnimationEventObject _animation_event = _events.Events[i];
				ICEEditorLayout.BeginHorizontal();


					_animation_event = WorldPopups.AnimationEventPopupLine( _component, _animation_event, _component.BehaviourEvents, ref _animation_event.UseCustomFunction, "", "Event #" + i, "" );

					bool _active = ICEEditorLayout.CheckButtonMiddle( "ACTIVE", "", _animation_event.IsActive );

					if( _active != _animation_event.IsActive )
					{
						_animation_event.IsActive = _active;
						AnimationUtility.SetAnimationEvents( _clip, _events.GetAnimationEvents() );						
					}

					if( ICEEditorLayout.ListDeleteButtonMini<AnimationEventObject>( _events.Events, _animation_event, "Removes this animation event." ) )
					{
						AnimationUtility.SetAnimationEvents( _clip, _events.GetAnimationEvents() );
						return;
					}

				ICEEditorLayout.EndHorizontal( Info.ANIMATION_EVENTS_METHOD_POPUP );

				EditorGUI.indentLevel++;

					if( _animation_event.ParameterType == AnimationEventParameterType.Integer )
						_animation_event.ParameterInteger = ICEEditorLayout.Integer( "Parameter Integer", "", _animation_event.ParameterInteger, Info.EVENT_PARAMETER_INTEGER );
					else if( _animation_event.ParameterType == AnimationEventParameterType.Float )
						_animation_event.ParameterFloat = ICEEditorLayout.Float( "Parameter Float", "", _animation_event.ParameterFloat, Info.EVENT_PARAMETER_FLOAT );
					else if( _animation_event.ParameterType == AnimationEventParameterType.String )
						_animation_event.ParameterString = ICEEditorLayout.Text( "Parameter String", "", _animation_event.ParameterString, Info.EVENT_PARAMETER_STRING );

					_animation_event.Time = ICEEditorLayout.Slider( "Time", "The time at which the event will be fired off.", _animation_event.Time, 0.0001f, 0, _clip.length, Info.ANIMATION_EVENTS_METHOD_TIME );

				EditorGUI.indentLevel--;
			}

			if( _events.UpdateRequired( _clip_events ) )
				AnimationUtility.SetAnimationEvents( _clip, _events.GetAnimationEvents() );

			EndObjectContent();
			// CONTENT END
		}

		private static AnimationInterface DrawBehaviourAnimationAnimationData( ICEWorldBehaviour _control, AnimationInterface _animation_data )
		{
			Animation _animation = _control.GetComponentInChildren<Animation>();

			if( _animation != null && _animation.enabled == true )
			{
				Info.Help ( Info.ANIMATION_ANIMATION );

				if( EditorApplication.isPlaying )
				{
					EditorGUILayout.LabelField("Name", _animation_data.Name );
				}
				else
				{
					string _animation_name = AnimationPopup( _animation, _animation_data.Name, "Animation (" + _animation_data.Length.ToString() + " secs.)", Info.ANIMATION_NAME );
					if( _animation_name != _animation_data.Name )
					{
						AnimationState _state = AnimationTools.GetAnimationStateByName( _control.gameObject, _animation_name );					
						if( _state != null )
						{				
							if( _state.clip != null )
								_state.clip.legacy = true;

							_animation_data.TransitionDuration = 0.25f;
							_animation_data.wrapMode = _state.wrapMode;
							_animation_data.DefaultWrapMode = _state.wrapMode;
							_animation_data.Speed =_state.speed;
							_animation_data.DefaultSpeed = _state.speed;
							_animation_data.Name = _state.name;
							_animation_data.Length = _state.length;
						}
					}
				}

				EditorGUI.indentLevel++;
					_animation_data.wrapMode = DrawBehaviourAnimationWrapMode( _animation_data.wrapMode  );
					DrawBehaviourAnimationData( ref _animation_data.Speed, ref _animation_data.AutoSpeed, ref _animation_data.TransitionDuration, ref _animation_data.AutoTransitionDuration, _animation_data.DefaultSpeed );

					if( _animation_data.AutoTransitionDuration )
						_animation_data.TransitionDuration = _animation_data.Length / 3;

				EditorGUI.indentLevel--;

			}
			else
			{
				EditorGUILayout.HelpBox( "Check your Animation Component", MessageType.Warning ); 
			}

			return _animation_data;
		}

		private static WrapMode DrawBehaviourAnimationWrapMode( WrapMode _mode )
		{
			string[] _modes = new string[]{ "Default", "Once", "Loop", "PingPong", "ClampForever" };
			int[] _modes_int = new int[]{ 0, 1, 2, 4, 8 };
			_mode = (WrapMode)ICEEditorLayout.IntPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", (int)_mode, _modes, _modes_int );

			//_mode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", _mode );
		
			return _mode;
		}

		private static void DrawBehaviourAnimationData( ref float _speed, ref bool _auto_speed, ref float _transition_duration, ref bool _auto_transition_duration, float _default_speed = 1 )
		{
			if( _speed == 0 )
				_speed = 1;
			
			_speed = ICEEditorLayout.AutoSlider( "Animation Speed (" + _default_speed + ")", "The playback speed of the animation. 1 is normal playback speed. A negative playback speed will play the animation backwards. Adapt this value to your movement settings.", _speed, 0.01f, -5, 5, ref _auto_speed, _default_speed );
			_transition_duration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _transition_duration, 0.01f, 0, 10, ref _auto_transition_duration, 0.25f  );
		}

		private static void DrawBehaviourAnimationAnimatorParameterData( ICEWorldBehaviour _control, List<AnimatorParameterObject> _parameter_list )
		{
			Animator _animator = _control.GetComponentInChildren<Animator>();

			if( _animator == null || ! _animator.isInitialized )
				return;
				
			if( _animator.parameterCount == 0 )
				_parameter_list.Clear();

			for( int _i = 0 ; _i < _parameter_list.Count; _i++ )
			{
				AnimatorParameterObject _parameter = _parameter_list[_i];
				var indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;


				// PARAMETER LINE BEGIN
				ICEEditorLayout.BeginHorizontal();

				EditorGUILayout.LabelField("",GUILayout.Width( 15 * indent ) );

				EditorGUI.BeginDisabledGroup( _parameter.Enabled == false );
				AnimatorControllerParameter _data = ICEEditorLayout.AnimatorParametersPopupData( _animator, _parameter.Name, GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 120 ) );

				if( _data != null )
				{
					_parameter.Name = _data.name;
					_parameter.Type = _data.type;

					if( _parameter.Type == AnimatorControllerParameterType.Bool )
					{
						if( _parameter.UseDynamicValue )
							_parameter.BooleanValueType = (DynamicBooleanValueType)EditorGUILayout.EnumPopup( _parameter.BooleanValueType, GUILayout.MinWidth( 65 ) );
						else
							_parameter.BooleanValue = ICEEditorLayout.CheckButtonFlex( (_parameter.BooleanValue?"TRUE":"FALSE" ),"", _parameter.BooleanValue, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Int )
					{
						if( _parameter.UseDynamicValue )
							_parameter.IntegerValueType = (DynamicIntegerValueType)EditorGUILayout.EnumPopup( _parameter.IntegerValueType );
						else
							_parameter.IntegerValue = EditorGUILayout.IntField( _parameter.IntegerValue, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Float )
					{
						if( _parameter.UseDynamicValue )
							_parameter.FloatValueType = (DynamicFloatValueType)EditorGUILayout.EnumPopup( _parameter.FloatValueType );
						else
							_parameter.FloatValue = EditorGUILayout.FloatField( _parameter.FloatValue, GUILayout.MinWidth( 65 ) );
					}
					else if( _parameter.Type == AnimatorControllerParameterType.Trigger )
					{
						EditorGUILayout.MinMaxSlider( ref _parameter.TriggerIntervalMin, ref _parameter.TriggerIntervalMax, 0, _parameter.TriggerIntervalMaximum );
						_parameter.TriggerIntervalMaximum = EditorGUILayout.FloatField( _parameter.TriggerIntervalMaximum, GUILayout.Width( 30 ) );
					}

					EditorGUI.BeginDisabledGroup( _parameter.Type == AnimatorControllerParameterType.Trigger );
						_parameter.UseDynamicValue = ICEEditorLayout.CheckButtonSmall( "DYN", "Use dynamic value", _parameter.UseDynamicValue );
					EditorGUI.EndDisabledGroup();
					_parameter.UseEnd = ICEEditorLayout.CheckButtonSmall( "END", "Sets this value at the end of the behaviour", _parameter.UseEnd );
				}
				EditorGUI.EndDisabledGroup();

				_parameter.Enabled = ICEEditorLayout.EnableButton( "Enables/disables the dynamic value", _parameter.Enabled );

				if( ICEEditorLayout.ListDeleteButtonMini<AnimatorParameterObject>( _parameter_list, _parameter ) )
					return;

				ICEEditorLayout.EndHorizontal(); // Info.GetTargetSelectionExpressionTypeHint( _condition.ExpressionType )
				// PARAMETER LINE END


				EditorGUI.indentLevel = indent;



			}

			// ADD CONDITION LINE BEGIN
			ICEEditorLayout.BeginHorizontal();
			GUILayout.FlexibleSpace();

			EditorGUILayout.LabelField( new GUIContent( "Add Parameter ", "" ), EditorStyles.wordWrappedMiniLabel );
			//ICEEditorLayout.MiniLabel( "Add Parameter" );

			//GUILayout.FlexibleSpace();
			if( ICEEditorLayout.AddButton( "Add parameter" ) )
				_parameter_list.Add( new AnimatorParameterObject() );

			if( ICEEditorLayout.ListClearButton<AnimatorParameterObject>( _parameter_list ) )
				return;

			ICEEditorLayout.EndHorizontal();
		}

		private static AnimatorInterface DrawBehaviourAnimationAnimatorData( ICEWorldBehaviour _control, AnimatorInterface _animator_data )
		{
			Animator m_animator = _control.GetComponentInChildren<Animator>();

			if( m_animator != null && m_animator.enabled == true && m_animator.runtimeAnimatorController != null && m_animator.avatar != null )
			{
				if( ! EditorApplication.isPlaying )
				{
					string _help_control_type = Info.ANIMATION_ANIMATOR_CONTROL_TYPE_DIRECT;

					if( _animator_data.Type == AnimatorControlType.ADVANCED )
						_help_control_type = Info.ANIMATION_ANIMATOR_CONTROL_TYPE_ADVANCED;

					_animator_data.Type = (AnimatorControlType)ICEEditorLayout.EnumPopup( "Control Type", "", _animator_data.Type, _help_control_type );

					if( _animator_data.Type == AnimatorControlType.DIRECT )
					{
						_animator_data.Index = AnimatorIntPopup( m_animator, _animator_data.Index );

						if( AnimationTools.GetAnimationClipCount( m_animator ) == 0 )
						{
							Info.Warning( Info.ANIMATION_ANIMATOR_ERROR_NO_CLIPS );
						}
						else
						{
							AnimationClip _animation_clip = AnimationTools.GetAnimationClipByIndex( m_animator,_animator_data.Index );

							if( _animation_clip != null )
							{				
								

								if( _animator_data.Name != _animation_clip.name )
									_animator_data.Init();

								_animation_clip.legacy = false;
								//_animation_clip.wrapMode = (WrapMode)ICEEditorLayout.EnumPopup( "WarpMode", "Determines how time is treated outside of the keyframed range of an AnimationClip or AnimationCurve.", _animation_clip.wrapMode );

								_animator_data.StateName = AnimationEditor.GetAnimatorStateName( m_animator, _animation_clip.name );
								_animator_data.Name = _animation_clip.name;
								_animator_data.Length = _animation_clip.length;
											
								//_animation_clip.wrapMode = DrawBehaviourAnimationWrapMode( _animation_clip.wrapMode  );
								DrawBehaviourAnimationData( ref _animator_data.Speed, ref _animator_data.AutoSpeed, ref _animator_data.TransitionDuration, ref _animator_data.AutoTransitionDuration );
		
								if( _animator_data.AutoTransitionDuration )
									_animator_data.TransitionDuration = _animator_data.Length * 0.25f;	


								_animator_data.ApplyRootMotion = ICEEditorLayout.Toggle( "Apply Root Motion", "", _animator_data.ApplyRootMotion );

								if( m_animator.hasRootMotion )
									Info.Warning( "The current rig has Root Motions" );



								_animator_data.ShowDetails = ICEEditorLayout.Foldout(  _animator_data.ShowDetails, "Animation Details", false );
								if( _animator_data.ShowDetails )
								{
									AnimationClipSettings _animation_clip_settings = AnimationUtility.GetAnimationClipSettings( _animation_clip );

									EditorGUI.indentLevel++;

									ICEEditorLayout.MiniLabelLeft( "Length \t\t\t\t" + ( _animation_clip_settings.stopTime - _animation_clip_settings.startTime ).ToString() + " secs." );

									ICEEditorLayout.MiniLabelLeft( "WrapMode \t\t\t" + _animation_clip.wrapMode.ToString() );

									EditorGUILayout.Separator();
									ICEEditorLayout.MiniLabelLeft( "Loop Time \t\t\t" +  _animation_clip_settings.loopTime.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tLoop Pose \t\t" + _animation_clip_settings.loopBlend.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tCycle Offset \t\t" +  _animation_clip_settings.cycleOffset.ToString() );

									EditorGUILayout.Separator();
									ICEEditorLayout.MiniLabelLeft( "Root Transform Rotation" );
									ICEEditorLayout.MiniLabelLeft( "\tBake Into Pose \t\t" + _animation_clip_settings.loopBlendOrientation.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tOriginal Orientation \t" +  _animation_clip_settings.keepOriginalOrientation.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tOffset \t\t\t" + _animation_clip_settings.orientationOffsetY.ToString() );

									EditorGUILayout.Separator();
									ICEEditorLayout.MiniLabelLeft( "Root Transform Position (Y)" );
									ICEEditorLayout.MiniLabelLeft( "\tBake Into Pose \t\t" + _animation_clip_settings.loopBlendPositionY.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tOriginal Position (Y) \t" +  _animation_clip_settings.keepOriginalPositionY.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tOffset \t\t\t" + _animation_clip_settings.level.ToString() );

									EditorGUILayout.Separator();
									ICEEditorLayout.MiniLabelLeft( "Root Transform Position (XZ)" );
									ICEEditorLayout.MiniLabelLeft( "\tBake Into Pose \t\t" + _animation_clip_settings.loopBlendPositionXZ.ToString() );
									ICEEditorLayout.MiniLabelLeft( "\tOriginal Position (XZ) \t" +  _animation_clip_settings.keepOriginalPositionXZ.ToString() );

									ICEEditorLayout.MiniLabelLeft( "Mirror \t\t\t\t" + _animation_clip_settings.mirror.ToString() );


									/*
									
										ICEEditorLayout.MiniLabelLeft( "Height From Feet \t\t\t" +  _animation_clip_settings.heightFromFeet );

										ICEEditorLayout.MiniLabelLeft( "Start Time \t\t\t" + _animation_clip_settings.startTime.ToString() );
										ICEEditorLayout.MiniLabelLeft( "Stop Time \t\t\t" + _animation_clip_settings.stopTime.ToString() );

										//ICEEditorLayout.MiniLabelLeft( "Additive Reference Pose Clip \t\t" + _animation_clip_settings.additiveReferencePoseClip.ToString() );
										ICEEditorLayout.MiniLabelLeft( "Additive Reference Pose Time \t" + _animation_clip_settings.additiveReferencePoseTime.ToString() );
										ICEEditorLayout.MiniLabelLeft( "Has Additive Reference Pose \t" + _animation_clip_settings.hasAdditiveReferencePose.ToString() );

										//AnimationUtility.SetAnimationClipSettings( _animation_clip, _animation_clip_settings );

										*/

									EditorGUILayout.HelpBox( "Please note: To adapt the above-listed values please open the animation clip settings or the import settings " +
										"of the selected object. In order to avoid problems with incorrect adapted animations, it is recommended to check and adjust the motion sequences and " +
										"settings at an early stage.", MessageType.None );

									EditorGUI.indentLevel--;
								}
	
							}
						}
					}
					else if( _animator_data.Type == AnimatorControlType.ADVANCED )
					{
						//EditorGUI.BeginDisabledGroup( _control.gameObject.GetComponent<Rigidbody>() == null && _control.gameObject.GetComponent<CharacterController>() == null );
							_animator_data.ApplyRootMotion = ICEEditorLayout.Toggle( "Apply Root Motion", "", _animator_data.ApplyRootMotion );
						//EditorGUI.EndDisabledGroup();

						//if( _control.gameObject.GetComponent<Rigidbody>() == null && _control.gameObject.GetComponent<CharacterController>() == null )
							//_animator_data.ApplyRootMotion = false;

						DrawBehaviourAnimationAnimatorParameterData( _control, _animator_data.Parameters );

					}

				}
				else
				{
					if( _animator_data.Type == AnimatorControlType.DIRECT )
					{
						ICEEditorLayout.Label( "Name", "Animation name.", _animator_data.Name );
						ICEEditorLayout.Label( "Length", "Animation length in seconds.", _animator_data.Length.ToString() + " secs." );
						ICEEditorLayout.Label( "Speed", "Determines how time is treated outside of the keyframed range of an AnimationClip.", _animator_data.Speed.ToString() );
						ICEEditorLayout.Label( "Transition Duration", "Determines how time is treated outside of the keyframed range of an AnimationClip.", _animator_data.TransitionDuration.ToString() );
						ICEEditorLayout.Label( "Root Motion", "Determines how potential Root Motions will be handled for this Behaviour Rule.", _animator_data.ApplyRootMotion.ToString() );

						//_animator_data.Speed = ICEEditorLayout.AutoSlider("Speed", "", _animator_data.Speed, 0.01f, -5, 5, ref _animator_data.AutoSpeed, 1 );
						//_animator_data.TransitionDuration = ICEEditorLayout.AutoSlider( "Transition Duration", "", _animator_data.TransitionDuration, 0.01f, 0, 10, ref _animator_data.AutoTransitionDuration, 0.25f  );

						//DrawBehaviourAnimationData( ref _animator_data.Speed, ref _animator_data.AutoSpeed, ref _animator_data.TransitionDuration, ref _animator_data.AutoTransitionDuration );					
					}
					else if( _animator_data.Type == AnimatorControlType.ADVANCED )
					{
						foreach( AnimatorParameterObject _parameter in _animator_data.Parameters )
						{
							switch( _parameter.Type )
							{
							case AnimatorControllerParameterType.Bool:
								EditorGUILayout.LabelField( _parameter.Name, "(BOOLEAN) " +  _parameter.BooleanValue.ToString() );
								break;
							case AnimatorControllerParameterType.Int:
								EditorGUILayout.LabelField( _parameter.Name, "(INTEGER) " +  _parameter.IntegerValue.ToString() );
								break;
							case AnimatorControllerParameterType.Float:
								EditorGUILayout.LabelField( _parameter.Name, "(FLOAT) " + ( _parameter.UseDynamicValue?_control.GetDynamicFloatValue( _parameter.FloatValueType ):_parameter.FloatValue ) );
								break;
							case AnimatorControllerParameterType.Trigger:
								EditorGUILayout.LabelField( _parameter.Name, "(TRIGGER)" );
								break;
							}
						}
					}
				}
			}
			else 
			{
				if( m_animator != null )
				{
					if( m_animator.enabled == false )
					{
						EditorGUILayout.HelpBox( "Sorry, your Animator Component is disabled!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						m_animator.enabled = ICEEditorLayout.EnableButton( "Enables/disables the Animator Component", m_animator.enabled );
						ICEEditorLayout.EndHorizontal();
					}
					else if( m_animator.runtimeAnimatorController == null )
					{
						EditorGUILayout.HelpBox( "Sorry, there is no Runtime Animator Controller!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						m_animator.enabled = ICEEditorLayout.EnableButton( "Enables/disables the Animator Component", m_animator.enabled );
						ICEEditorLayout.EndHorizontal();
					}
					else if( m_animator.avatar == null )
					{
						EditorGUILayout.HelpBox( "Sorry, there is no Avatar asigned to your Animator Component!", MessageType.Warning ); 

						ICEEditorLayout.BeginHorizontal();
						EditorGUILayout.LabelField( "Enable Animator Component", EditorStyles.boldLabel);
						m_animator.enabled = ICEEditorLayout.EnableButton( "Enables/disables the Animator Component", m_animator.enabled );
						ICEEditorLayout.EndHorizontal();
					}

				}
				else
				{

					EditorGUILayout.HelpBox( "Sorry, there is no Animator Component!", MessageType.Warning ); 

					ICEEditorLayout.BeginHorizontal();
					EditorGUILayout.LabelField( "Add Animator Component", EditorStyles.miniLabel );
					if( ICEEditorLayout.AddButton( "Add Animator Component" ) )
						m_animator = _control.gameObject.AddComponent<Animator>();
					ICEEditorLayout.EndHorizontal();
				}

			}
			return _animator_data;
		}

	}
}