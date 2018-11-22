// ##############################################################################
//
// ice_creature_editor_wizard.cs | WizardEditor
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

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

using ICE.Shared;

namespace ICE.Creatures.EditorUtilities
{


	public static class WizardEditor
	{	
		public static void WizardGenerate( ICECreatureControl _control )
		{
			string _tasks = "BEGIN GENERATE CREATURE SETTINGS \n";

				_tasks += "- Reset Creature Object \n";
				//_control.Creature.Reset();

				_tasks += "- Create new Home Location and define the Random Positioning Range \n";
				WizardRandomTarget( _control, _control.Creature.Essentials.Target );

				if( ICECreatureRegister.Instance != null )
				{
					_tasks += "- Set GroundCheck to " + ICECreatureRegister.Instance.Options.GroundCheck.ToString() + " and copy ground layer \n";
					_control.Creature.Move.GroundCheck = ICECreatureRegister.Instance.Options.GroundCheck;
					_control.Creature.Move.GroundLayer.SetLayers( ICECreatureRegister.Instance.Options.GroundLayer.Layers );

					_tasks += "- Set ObstacleCheck to " + ICECreatureRegister.Instance.Options.WaterCheck.ToString() + " and copy obstacle layer \n";
					_control.Creature.Move.WaterCheck = ICECreatureRegister.Instance.Options.WaterCheck;
					_control.Creature.Move.WaterLayer.SetLayers( ICECreatureRegister.Instance.Options.ObstacleLayer.Layers );

					_tasks += "- Set ObstacleCheck to " + ICECreatureRegister.Instance.Options.ObstacleCheck.ToString() + " and copy obstacle layer \n";
					_control.Creature.Move.ObstacleCheck = ICECreatureRegister.Instance.Options.ObstacleCheck;
					_control.Creature.Move.ObstacleLayer.SetLayers( ICECreatureRegister.Instance.Options.ObstacleLayer.Layers );
				}

				_tasks += "- Set GroundOrientation to " + _control.Creature.Essentials.GroundOrientation.ToString() + " \n";
				_control.Creature.Move.DefaultBody.Type = _control.Creature.Essentials.GroundOrientation;

				_tasks += "- Set TrophicLevel to " + _control.Creature.Essentials.TrophicLevel.ToString() + " \n";
				_control.Creature.Status.TrophicLevel = _control.Creature.Essentials.TrophicLevel;
				_control.Creature.Status.IsCannibal = _control.Creature.Essentials.IsCannibal;

				_tasks += "- Create behaviours \n";
				WizardBehaviour( _control );

				if( _control.Creature.Essentials.UseAutoDetectInteractors )
				{
					_tasks += "- Detect and prepare potential interactors \n";
					_control.Creature.AutoDetectInteractors();
				}

				_tasks += "END GENERATE CREATURE SETTINGS";

			Debug.Log( _tasks );

		}


		public static void WizardRandomTarget( ICECreatureControl _control, TargetObject _target )
		{
			string _new_target_name = ( _target.Type.ToString() + "_" + _control.transform.name ).ToUpper();
			
			GameObject _new_target_object = GameObject.Find( _new_target_name );
			
			if( _new_target_object == null )
			{
				_new_target_object = new GameObject();
				_new_target_object.transform.position = _control.transform.position; 
				_new_target_object.name = _new_target_name;
				_new_target_object.AddComponent<ICECreatureLocation>();

				if( ICECreatureRegister.Instance != null )
					_new_target_object.transform.parent = ICECreatureRegister.Instance.HierarchyManagement.GetHierarchyGroupTransform( EntityClassType.Location );
			}

			_target.OverrideTargetGameObject(  _new_target_object );

			_target.Move.Enabled = true;
			_target.Move.Foldout = true;
			_target.Move.RandomRange = 100;//Random.Range( Init.WIZARD_RANDOM_RANGE_MIN, Init.WIZARD_RANDOM_RANGE_MAX );
			_target.Move.UseUpdateOffsetOnActivateTarget = true;
			_target.Move.UseUpdateOffsetOnMovePositionReached = true;
			_target.Move.StoppingDistance = 2;
			_target.Move.IgnoreLevelDifference = true;

			if( ICECreatureRegister.Instance != null )
				ICECreatureRegister.Instance.AddReference( _target.TargetGameObject );
		}

		public static void WizardBehaviour( ICECreatureControl _control )
		{
			string _key = "";
	
			// ESSENTIAL ANIMATION BASED BEHAVIOURS 
			_key = _control.Creature.Behaviour.AddBehaviourMode( "RUN" );
			CreateBehaviourRuleRun( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeRun = _key;
			_control.Creature.Essentials.BehaviourModeTravel = _key;

			_key = _control.Creature.Behaviour.AddBehaviourMode( "WALK" );
			CreateBehaviourRuleWalk( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeWalk = _key;
			_control.Creature.Essentials.BehaviourModeLeisure = _key;

			_key = _control.Creature.Behaviour.AddBehaviourMode( "IDLE" );
			CreateBehaviourRuleIdle( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeIdle = _key;
			_control.Creature.Essentials.BehaviourModeRendezvous = _key;


			_key = _control.Creature.Behaviour.AddBehaviourMode( "SPAWN" );
			CreateBehaviourRuleIdle( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeSpawn = _key;

			_key = _control.Creature.Behaviour.AddBehaviourMode( "DEAD" );
			CreateBehaviourRuleDie( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeDead = _key;



			_key = _control.Creature.Behaviour.AddBehaviourMode( "JUMP" );
			CreateBehaviourRuleJump( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeJump = _key;

			_key = _control.Creature.Behaviour.AddBehaviourMode( "FALL" );
			CreateBehaviourRuleFall( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
			_control.Creature.Essentials.BehaviourModeFall = _key;

			// ADDITIONAL ANIMATION BASED BEHAVIOURS 

			_key = _control.Creature.Behaviour.AddBehaviourMode( "ATTACK" );
			CreateBehaviourRuleAttack( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );

			_key = _control.Creature.Behaviour.AddBehaviourMode( "DEFEND" );
			CreateBehaviourRuleImpact( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
	
			// ADDITIONAL BEHAVIOURS

			_key = _control.Creature.Behaviour.AddBehaviourMode( "SENSE" );
			CreateBehaviourRuleIdle( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );

			_key = _control.Creature.Behaviour.AddBehaviourMode( "HUNT" );
			CreateBehaviourRuleRun( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );

			_key = _control.Creature.Behaviour.AddBehaviourMode( "ESCAPE" );
			CreateBehaviourRuleRun( _control, _control.Creature.Behaviour.GetBehaviourModeByKey( _key ) );
	
		}

		/// <summary>
		/// Creates automatically a run behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleRun( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{	
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
				if( ! _control.Creature.Essentials.IgnoreAnimationRun )
				{
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultRunningSpeed;
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationRun );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
				else if( ! _control.Creature.Essentials.IgnoreAnimationWalk )
				{
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultWalkingSpeed;
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationWalk );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
				else
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultRunningSpeed;	
			}
		}

		/// <summary>
		/// Creates automatically an walk behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleWalk( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
				if( ! _control.Creature.Essentials.IgnoreAnimationWalk )
				{
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultWalkingSpeed;
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationWalk );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
				else if( ! _control.Creature.Essentials.IgnoreAnimationRun )
				{
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultRunningSpeed;
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationRun );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
				else
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultWalkingSpeed;
			}
		}

		/// <summary>
		/// Creates automatically an attack behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleAttack( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.Velocity = Vector3.zero;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
				_behaviour.Rule.Move.ViewingDirection = ViewingDirectionType.CENTER;
				if( ! _control.Creature.Essentials.IgnoreAnimationAttack )
				{
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationAttack );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		/// <summary>
		/// Creates automatically an impact behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleImpact( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.Velocity = Vector3.zero;
				_behaviour.Rule.Move.Motion.AngularVelocity = Vector3.zero;
				if( ! _control.Creature.Essentials.IgnoreAnimationImpact )
				{
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationImpact );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		/// <summary>
		/// Creates automatically an idle behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleIdle( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;
			
			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.Velocity = Vector3.zero;
				_behaviour.Rule.Move.Motion.AngularVelocity = Vector3.zero;
				if( ! _control.Creature.Essentials.IgnoreAnimationIdle )
				{
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationIdle );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		/// <summary>
		/// Creates automatically a jump behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleJump( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultRunningSpeed;
				_behaviour.Rule.Move.Motion.Velocity.y = _control.Creature.Essentials.DefaultRunningSpeed;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
				if( ! _control.Creature.Essentials.IgnoreAnimationJump )
				{	
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationIdle );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		public static void CreateBehaviourRuleFall( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Foldout = true;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
				if( ! _control.Creature.Essentials.IgnoreAnimationFall )
				{
					_behaviour.Rule.Move.Motion.Velocity.z = _control.Creature.Essentials.DefaultRunningSpeed;
					_behaviour.Rule.Move.Motion.Velocity.y = _control.Creature.Essentials.DefaultRunningSpeed;
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationFall );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		/// <summary>
		/// Creates automatically a die behaviour rule.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_key">Key.</param>
		public static void CreateBehaviourRuleDie( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null )
				return;

			_behaviour.NextRule();
			if( _behaviour.Rule != null )
			{			
				_behaviour.Rule.Move.Enabled = false;
				_behaviour.Rule.Move.Foldout = false;
				_behaviour.Rule.Move.Motion.Velocity = Vector3.zero;
				_behaviour.Rule.Move.Motion.AngularVelocity = Vector3.zero;
				if( ! _control.Creature.Essentials.IgnoreAnimationDead )
				{
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationDead );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
				else if( ! _control.Creature.Essentials.IgnoreAnimationIdle )
				{
					_behaviour.Rule.Animation.Copy( _control.Creature.Essentials.AnimationIdle );
					_behaviour.Rule.Animation.Enabled = true;
					_behaviour.Rule.Animation.Foldout = true;
				}
			}
		}

		public static string WizardBehaviour( ICECreatureControl _control, string _key )
		{
			_key = _control.Creature.Behaviour.AddBehaviourMode( _key );

			BehaviourModeObject _behaviour = _control.Creature.Behaviour.GetBehaviourModeByKey( _key );


			if( ! WizardAnimation( _control, _behaviour ) )
				return _key;



			return _key;
		}

		public static bool WizardAnimation( ICECreatureControl _control, BehaviourModeObject _behaviour )
		{
			if( _behaviour == null ) 
				return false;

			_behaviour.NextRule();
			 if( _behaviour.Rule == null )
			   return false;

			if( _behaviour.Key == "RUN" || _behaviour.Key == "TRAVEL" || _behaviour.Key == "JOURNEY" || _behaviour.Key == "HUNT" || _behaviour.Key == "ESCAPE" || _behaviour.Key == "FLEE" )
			{
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Motion.Velocity.z  = _control.Creature.Essentials.DefaultRunningSpeed;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
			}
			else if( _behaviour.Key == "WALK" || _behaviour.Key == "LEISURE" || _behaviour.Key == "AVOID" )
			{
				_behaviour.Rule.Move.Enabled = true;
				_behaviour.Rule.Move.Motion.Velocity.z  = _control.Creature.Essentials.DefaultWalkingSpeed;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = _control.Creature.Essentials.DefaultTurningSpeed;
			}
			else 
			{
				_behaviour.Rule.Move.Enabled = false;
				_behaviour.Rule.Move.Motion.Velocity.z  = 0;
				_behaviour.Rule.Move.Motion.AngularVelocity.y = 0;
			}

			if( _control.GetComponent<Animator>() != null && _control.GetComponent<Animator>().runtimeAnimatorController != null )
			{
				AnimationClip[] _clips = AnimationTools.GetAnimationClips( _control.GetComponent<Animator>() );
				int _index = 0;
				foreach( AnimationClip _clip in _clips )
				{
					if( AnimationIsSuitable( _behaviour.Key, _clip.name ) )
					{
						_behaviour.Rule.Animation.InterfaceType = AnimationInterfaceType.MECANIM;
						_behaviour.Rule.Animation.Animator.Type = AnimatorControlType.DIRECT;
						_behaviour.Rule.Animation.Animator.Name = _clip.name;
						_behaviour.Rule.Animation.Animator.Index = _index;
						break;
					}

					_index++;
				}
			}
			else if( _control.GetComponentInChildren<Animation>() != null )
			{
				Animation _animation = _control.GetComponentInChildren<Animation>();
				int _index = 0;
				foreach (AnimationState _state in _animation )
				{
					if( AnimationIsSuitable( _behaviour.Key, _state.name ) )
					{
						_behaviour.Rule.Animation.InterfaceType = AnimationInterfaceType.LEGACY;
						_behaviour.Rule.Animation.Animation.Name = _state.name;
						_behaviour.Rule.Animation.Animation.Index = _index;
					}
					
					_index++;
				}
			}

			return true;
		}

		public static AnimationDataObject WizardAnimationPopup( string _title, ICECreatureControl _control, AnimationDataObject _anim )
		{
			if( _control.GetComponentInChildren<Animator>() )
				_anim.InterfaceType = AnimationInterfaceType.MECANIM;
			else if( _control.GetComponentInChildren<Animation>() )
				_anim.InterfaceType = AnimationInterfaceType.LEGACY;
			else
				_anim.InterfaceType = AnimationInterfaceType.NONE; 
			
			
			if( _anim.InterfaceType != AnimationInterfaceType.NONE )
			{
				if( _anim.InterfaceType == AnimationInterfaceType.MECANIM )
					_anim.Animator = WizardAnimationPopupMecanim( _title, _control, _anim.Animator );
				else if( _anim.InterfaceType == AnimationInterfaceType.LEGACY )
					_anim.Animation = WizardAnimationPopupLegacy( _title, _control,_anim.Animation );
			}
			return _anim;
		}

		private static AnimationInterface WizardAnimationPopupLegacy( string _title, ICECreatureControl _control, AnimationInterface _animation_data )
		{
			Animation m_animation = _control.GetComponentInChildren<Animation>();
			
			if( m_animation != null && m_animation.enabled == true )
			{
				if( EditorApplication.isPlaying )
				{
					EditorGUILayout.LabelField("Name", _animation_data.Name );
				}
				else
				{
	
					_animation_data.Name = AnimationEditor.AnimationPopupBase( m_animation, _animation_data.Name, _title );
					AnimationState _state = AnimationTools.GetAnimationStateByName( _control.gameObject, _animation_data.Name );					
					if( _state != null )
					{				
						if( _state.clip != null )
							_state.clip.legacy = true;
						
						if( _animation_data.Name != _state.name )
						{
							_animation_data.Name = _state.name;
							_animation_data.Length = _state.length;
							_animation_data.Speed =_state.speed;
							_animation_data.TransitionDuration = 0.25f;
							_animation_data.wrapMode = _state.wrapMode;
							
							_animation_data.Length = _state.length;
							_animation_data.DefaultSpeed = _state.speed;
							_animation_data.DefaultWrapMode = _state.wrapMode;
						}
					}
				}
			}
			else
			{
				EditorGUILayout.HelpBox( "Check your Animation Component", MessageType.Warning ); 
			}
			
			return _animation_data;
		}

		private static AnimatorInterface WizardAnimationPopupMecanim( string _title, ICECreatureControl _control, AnimatorInterface _animator_data )
		{
			Animator m_animator = _control.GetComponent<Animator>();			
			if( m_animator != null && m_animator.enabled == true && m_animator.runtimeAnimatorController != null && m_animator.avatar != null )
			{
				if( ! EditorApplication.isPlaying )
				{
					_animator_data.Type = AnimatorControlType.DIRECT;
					
					ICEEditorLayout.BeginHorizontal();
					_animator_data.Index = AnimationEditor.AnimatorIntPopupBase( m_animator, _animator_data.Index, _title );
					ICEEditorLayout.EndHorizontal();
					
					if( AnimationTools.GetAnimationClipCount( m_animator ) == 0 )
						Info.Warning( Info.BEHAVIOUR_ANIMATION_ANIMATOR_ERROR_NO_CLIPS );
					else
					{
						AnimationClip _animation_clip = AnimationTools.GetAnimationClipByIndex( m_animator, _animator_data.Index );						
						if( _animation_clip != null )
						{				
							if( _animator_data.Name != _animation_clip.name )
								_animator_data.Init();

											
							_animation_clip.wrapMode = WrapMode.Loop;
							_animation_clip.legacy = false;

							_animator_data.StateName = AnimationEditor.GetAnimatorStateName( m_animator, _animation_clip.name );
							_animator_data.Name = _animation_clip.name;
							_animator_data.Length = _animation_clip.length;						
							_animator_data.Speed = 1;
							_animator_data.TransitionDuration = 0.05f;
						}
					}
				}
				else
				{
					ICEEditorLayout.Label( "Name", "Animation name.", _animator_data.Name );
				}
			}
			else 
			{
			
				
			}
			return _animator_data;
		}

		private static bool AnimationIsSuitable( string _key, string _animation )
		{
			_key = _key.ToUpper();
			_animation = _animation.ToUpper();

			if( _key == "RUN" && ( _animation.IndexOf( "RUN" ) > -1 || _animation.IndexOf( "WALK" ) > -1 ) )
				return true;
			else if( _key == "WALK" && _animation.IndexOf( "WALK" ) > -1 )
				return true;
			else if( _key == "IDLE" && _animation.IndexOf( "IDLE" ) > -1 )
				return true;
			else if( _key == "JUMP" && _animation.IndexOf( "JUMP" ) > -1 )
				return true;
			else if( _key == "DEAD" && ( _animation.IndexOf( "DEAD" ) > -1 || _animation.IndexOf( "DEATH" ) > -1 || _animation.IndexOf( "DIE" ) > -1 )  )
				return true;
			else
				return false;
		}



	}
}
