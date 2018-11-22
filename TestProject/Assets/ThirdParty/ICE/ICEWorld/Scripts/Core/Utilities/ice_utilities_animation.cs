// ##############################################################################
//
// ice_utilities_animation.cs | ICE.World.Utilities.AnimationTools
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

namespace ICE.World.Utilities
{
	public static class AnimationTools 
	{
		public static Animator TryGetAnimatorComponent( GameObject _object )
		{
			if( _object == null )
				return null;
			
			return _object.GetComponentInChildren<Animator>();
		}

		public static Animation TryGetAnimationComponent( GameObject _object )
		{
			if( _object == null )
				return null;
			
			return _object.GetComponentInChildren<Animation>();
		}

		/// <summary>
		/// Get AnimationState by GameObject and AnimationName
		/// </summary>
		/// <returns>The state by index.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		public static AnimationState GetAnimationStateByName( GameObject _object, string _name )
		{
			return GetAnimationStateByName( _object.GetComponentInChildren<Animation>(), _name );
		}

		/// <summary>
		/// Gets AnimationState by AnimationName
		/// </summary>
		/// <returns>The AnimationState.</returns>
		/// <param name="_animation">Animation.</param>
		/// <param name="_name">Name.</param>
		public static AnimationState GetAnimationStateByName( Animation _animation, string _name )
		{
			if( _animation == null )
				return null;

			foreach( AnimationState _state in _animation )
				if( _state.name == _name )
					return _state;

			return null;
		}

		public static AnimationState[] GetAnimationStates( Animation _animation )
		{
			if( _animation == null )
				return null;

			int i = 0;
			AnimationState[] _states = new AnimationState[ GetAnimationClipCount( _animation ) ];
			foreach( AnimationState _state in _animation )
				_states[i++] = _state;

			return _states;
		}

		public static AnimationClip GetAnimationClipByName( Animation _animation, string _name )
		{
			if( _animation == null )
				return null;
			
			foreach( AnimationState _state in _animation )
				if( _state.clip.name == _name )
					return _state.clip;

			return null;
		}

		public static AnimationClip[] GetAnimationClips( Animation _animation )
		{
			if( _animation == null )
				return null;
			int i = 0;
			int _count = GetAnimationClipCount( _animation );
			AnimationClip[] _clips = new AnimationClip[ _count ];
			foreach( AnimationState _state in _animation )
				_clips[i++] = _state.clip;

			return _clips;
		}

		public static int GetAnimationClipCount( Animation _animation )
		{
			if( _animation == null )
				return 0;

			return _animation.GetClipCount();
		}

		public static AnimationClip GetAnimationClipByAnimatorAndName( Animator _animator, string _name )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null )
				return null;

			return GetAnimationClipByName( GetAnimationClips( _animator ), _name );
		}

		public static int GetAnimationClipCount( Animator _animator )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null )
				return 0;

			return _animator.runtimeAnimatorController.animationClips.Length;
		}

		public static AnimationClip GetAnimationClipByIndex( Animator _animator, int _index )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null || _index < 0 || _index >= _animator.runtimeAnimatorController.animationClips.Length )
				return null;

			return _animator.runtimeAnimatorController.animationClips[ _index ];
		}

		public static AnimationClip[] GetAnimationClips( Animator _animator )
		{
			if( _animator == null || _animator.runtimeAnimatorController == null )
				return null;

			return _animator.runtimeAnimatorController.animationClips;
		}

		public static AnimationClip GetAnimationClipByName( AnimationClip[] _clips, string _name )
		{
			if( _clips == null || _clips.Length == 0 )
				return null;
			
			foreach( AnimationClip _clip in _clips )
				if( _clip.name == _name )
					return _clip;

			return null;
		}

		/// <summary>
		/// Compares two animation curves.
		/// </summary>
		/// <returns><c>true</c>, if animation curve was compared, <c>false</c> otherwise.</returns>
		/// <param name="_curve_1">Curve 1.</param>
		/// <param name="_curve_2">Curve 2.</param>
		public static bool CompareAnimationCurve( AnimationCurve _curve_1, AnimationCurve _curve_2 )
		{
			if( _curve_1.length != _curve_2.length )
				return false;
			else
			{
				for( int i = 0; i < _curve_1.length ; i++ )
				{
					if( _curve_1[i].time != _curve_2[i].time ||
						_curve_1[i].value != _curve_2[i].value ||
						_curve_1[i].inTangent != _curve_2[i].inTangent  ||
						_curve_1[i].outTangent != _curve_2[i].outTangent   ||
						_curve_1[i].tangentMode != _curve_2[i].tangentMode )
						return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Determines if the specified _object has valid animations components.
		/// </summary>
		/// <returns><c>true</c> if the specified _object has animations; otherwise, <c>false</c>.</returns>
		/// <param name="_object">Object.</param>
		public static bool HasAnimations( GameObject _object )
		{
			if( _object != null && (
				GetAnimationClipCount( _object.GetComponentInChildren<Animation>() ) > 0 ||
				GetAnimationClipCount( _object.GetComponentInChildren<Animator>() ) > 0 ) )
				return true;
			else
				return false;
		}
			
		/// <summary>
		/// Get AnimationName by Index 
		/// </summary>
		/// <returns>The index to name.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		private static string GetAnimationNameByIndex( GameObject _object, int _index)
		{
			AnimationState state = GetAnimationStateByIndex( _object, _index );
			if (state == null)
				return "";

			return state.name;
		}

		/// <summary>
		/// Get AnimationState by Index
		/// </summary>
		/// <returns>The state by index.</returns>
		/// <param name="_control">_control.</param>
		/// <param name="index">Index.</param>
		public static AnimationState GetAnimationStateByIndex( GameObject _object, int index)
		{
			Animation _anim = _object.GetComponentInChildren<Animation>();

			if( _anim == null )
				return null;

			int i = 0;
			foreach (AnimationState state in _anim )
			{
				if (i == index)
					return state;
				i++;
			}
			return null;
		}

		/*
		public static UnityEditor.Animations.ChildAnimatorState GetChildAnimatorStates( Animator _animator, int _layer_index )
		{
			UnityEditor.Animations.AnimatorController _controller = _animator.runtimeAnimatorController as  UnityEditor.Animations.AnimatorController;

			UnityEditor.Animations.AnimatorControllerLayer _layer = _controller.layers[ _layer_index ];

			UnityEditor.Animations.AnimatorStateMachine _state_machine = _layer.stateMachine;

			return  _state_machine.states;
		}*/

	}




}