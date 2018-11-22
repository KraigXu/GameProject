// ##############################################################################
//
// ice_creature_editor_objects.cs | CreatureObjectEditor : WorldObjectEditor
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
using UnityEngine.Audio;
//using UnityEngine.Serialization;

using UnityEditor;
//using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;
//using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorInfos;




namespace ICE.Creatures.EditorUtilities
{
	/// <summary>
	/// ICE custom editor layout.
	/// </summary>
	public class CreatureEditorLayout : ICEEditorLayout
	{	
		public static void DrawSelectionTimer( string _title, string _hint, SelectionTimerObject _timer, string _help = "" )
		{
			if( _timer == null )
				return;
			
			ICEEditorLayout.BeginHorizontal();
				float _timer_value = ICEEditorLayout.RoundDisplay( _timer.Timer, 0.025f );
				float _span_value = ICEEditorLayout.RoundDisplay( _timer.Span, 0.025f );
				if( _timer.UseRandomSpan )
					ICEEditorLayout.MinMaxDefaultSlider( _title + " (" + _timer_value + "/" + _span_value + " secs.)", "", ref _timer.SpanMin, ref _timer.SpanMax, 0, ref _timer.SpanMaximum, 0, 0, Init.DECIMAL_PRECISION_TIMER, 40, "" );
				else
					_timer.Span = ICEEditorLayout.MaxDefaultSlider( "Delay Timer (" + _timer_value + " secs.)", "Defines the time-span for the time-out", _timer.Span, Init.DECIMAL_PRECISION_TIMER, 0, ref _timer.SpanMaximum, 0, "" );
				_timer.UseRandomSpan = ICEEditorLayout.CheckButtonSmall( "RND", "", _timer.UseRandomSpan );
			ICEEditorLayout.EndHorizontal( _help );
		}

		public static MoveDataObject DrawMove( MoveDataObject _move, string _help = "" )
		{
			if( _move == null )
				return _move;
			
			if( _help == "" )
				Info.Help ( Info.MOVE );
			else
				Info.Help ( Info.MOVE + "\n\n" + _help );

			_move.SegmentLength = ICEEditorLayout.MaxDefaultSlider( "Segment Length", "Subdivides the main path in segments of unitary length", _move.SegmentLength, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _move.SegmentLengthMaximum , 0, Info.MOVE_SEGMENT_LENGTH );
			if( _move.SegmentLength > 0 )
			{
				EditorGUI.indentLevel++;
					_move.SegmentVariance  = ICEEditorLayout.DefaultSlider( "Segment Variance Multiplier", "Generates randomized deviations along the path.", _move.SegmentVariance, Init.DECIMAL_PRECISION_DISTANCES, 0, 1, 0, Info.MOVE_SEGMENT_VARIANCE );
				EditorGUI.indentLevel--;

				_move.DeviationLength = ICEEditorLayout.MaxDefaultSlider( "Deviation Length", "Maximum value for the lateral deviation", _move.DeviationLength, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _move.DeviationLengthMaximum , 0, Info.MOVE_SEGMENT_LENGTH );
				EditorGUI.indentLevel++;
				_move.DeviationVariance = ICEEditorLayout.DefaultSlider( "Lateral Variance Multiplier", "Generates randomized deviations along the path.", _move.DeviationVariance , Init.DECIMAL_PRECISION_DISTANCES, 0, 1, 0,  Info.MOVE_LATERAL_VARIANCE );
				EditorGUI.indentLevel--;

				ICEEditorLayout.BeginHorizontal();
					_move.StoppingDistance = ICEEditorLayout.MaxDefaultSlider( "Stopping Distance (" + (_move.IgnoreLevelDifference?"circular":"spherical") + ")", "Stop within this distance from the target move position.", _move.StoppingDistance, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _move.StoppingDistanceMaximum, 2 );
					_move.IgnoreLevelDifference = ! ICEEditorLayout.CheckButtonSmall( "3D", "Provides linear distances without consideration of level differences.", ! _move.IgnoreLevelDifference );
				ICEEditorLayout.EndHorizontal( Info.MOVE_STOPPING_DISTANCE );
			}

			return _move;
		}

		public static float DrawStoppingDistance( float _distance, ref float _maximum, ref bool _ignore_level_difference, ref bool _zone_restricted, ref bool _more )
		{
			ICEEditorLayout.BeginHorizontal();
			_distance = ICEEditorLayout.BasicMaxSlider( "Stopping Distance (" + (_ignore_level_difference?"circular":"spherical") + ")","Stop within this distance from the target move position.", _distance, Init.DECIMAL_PRECISION_DISTANCES, Init.STOPPING_DISTANCE_MINIMUM, ref _maximum );
			_ignore_level_difference = ! ICEEditorLayout.CheckButtonSmall( "3D", "Ignore Level Differences - provides linear distances without consideration of level differences.", ! _ignore_level_difference );
			_zone_restricted = ICEEditorLayout.CheckButtonSmall( "BAN", "Use Stopping Distance as restricted zone", _zone_restricted );
			_distance = ICEEditorLayout.ButtonDefault( _distance, Init.STOPPING_DISTANCE_DEFAULT );
			_more = ICEEditorLayout.CheckButtonSmall( "MORE", "Use Stopping Distance as restricted zone", _more );
			ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_STOP_DISTANCE );

			return _distance;
		}
			
		/// <summary>
		/// Draws the stopping distance.
		/// </summary>
		/// <returns>The stopping distance.</returns>
		/// <param name="_distance">Distance.</param>
		/// <param name="_maximum">Maximum.</param>
		/// <param name="_ignore_level_difference">Ignore level difference.</param>
		/// <param name="_zone_restricted">Zone restricted.</param>
		public static float DrawStoppingDistance( float _distance, ref float _maximum, ref bool _ignore_level_difference, ref bool _zone_restricted )
		{
			ICEEditorLayout.BeginHorizontal();
				_distance = ICEEditorLayout.BasicMaxSlider( "Stopping Distance (" + (_ignore_level_difference?"circular":"spherical") + ")","Stop within this distance from the target move position.", _distance, Init.DECIMAL_PRECISION_DISTANCES, Init.STOPPING_DISTANCE_MINIMUM, ref _maximum );
				_ignore_level_difference = ! ICEEditorLayout.CheckButtonSmall( "3D", "Ignore Level Differences - provides linear distances without consideration of level differences.", ! _ignore_level_difference );
				_zone_restricted = ICEEditorLayout.CheckButtonSmall( "BAN", "Use Stopping Distance as restricted zone", _zone_restricted );
				_distance = ICEEditorLayout.ButtonDefault( _distance, Init.STOPPING_DISTANCE_DEFAULT );
			ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_STOP_DISTANCE );

			return _distance;
		}

		/// <summary>
		/// Draws the influence slider.
		/// </summary>
		/// <returns>The influence slider.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_tooltip">Tooltip.</param>
		/// <param name="_value">Value.</param>
		/// <param name="_use_percent">Use percent.</param>
		/// <param name="_maximum">Maximum.</param>
		/// <param name="_help">Help.</param>
		public static float DrawInfluenceSlider( string _title, string _tooltip, float _value, ref bool _use_percent, ref bool _use_range, float _maximum = 0, string _help = "" )
		{
			float _precision = 0.001f;
			float _min = -_maximum;
			float _max = _maximum;
			if( _use_percent )
			{
				_min = -100;
				_max = 100;
			}

			ICEEditorLayout.BeginHorizontal();
			_value = ICEEditorLayout.BasicSlider( _title, _tooltip, _value, _precision, _min, _max );
			_value = ICEEditorLayout.ButtonDefault( _value, 0 );
			_use_percent = ICEEditorLayout.CheckButtonSmall( "%", "Use Percent", _use_percent );
			_use_range = ICEEditorLayout.CheckButtonSmall( "R", "Use Range", _use_range );
			ICEEditorLayout.EndHorizontal( _help );

			return ICEEditorLayout.Round( _value, _precision );
		}

		public static void DrawInfluenceMinMaxSlider( string _title, string _tooltip, ref float _min, ref float _max, ref bool _use_percent, ref bool _use_range, float _maximum = 0, string _help = "" )
		{
			float _total_min = -_maximum;
			float _total_max = _maximum;
			if( _use_percent )
			{
				_total_min = -100;
				_total_max = 100;
			}

			ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.MinMaxDefaultSlider( _title,  _tooltip, ref _min, ref _max, _total_min, _total_max, 0, 0, Init.DECIMAL_PRECISION, 40);
				_use_percent = ICEEditorLayout.CheckButtonSmall( "%", "Use Percent", _use_percent );
				_use_range = ICEEditorLayout.CheckButtonSmall( "R", "Use Range", _use_range );
			ICEEditorLayout.EndHorizontal( _help );
		}

	}

	/// <summary>
	/// Container editor.
	/// </summary>
	public static class ContainerEditor
	{	
		/// <summary>
		/// Draws the odour container.
		/// </summary>
		/// <returns>The odour container.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_odour">Odour.</param>
		/// <param name="_help">Help.</param>
		public static OdourContainer DrawOdourContainer( string _title, string _hint, OdourContainer _odour, string _help = "" )
		{
			//ODOUR BEGIN
			ICEEditorLayout.BeginHorizontal();
			_odour.Type = (OdourType)ICEEditorLayout.EnumPopup("Odour","", _odour.Type );	
			_odour.Enabled = ICEEditorLayout.EnableButton( _odour.Enabled );
			ICEEditorLayout.EndHorizontal( Info.ODOUR );
			if( _odour.Type != OdourType.NONE )
			{					
				EditorGUI.indentLevel++;
				_odour.Intensity = ICEEditorLayout.MaxDefaultSlider( "Intensity", "", _odour.Intensity , 1, 0, ref _odour.IntensityMax, 0, Info.ODOUR_INTENSITY );
				_odour.Range = ICEEditorLayout.MaxDefaultSlider( "Range", "", _odour.Range , 1, 0, ref _odour.RangeMax, 0, Info.ODOUR_RANGE );

				_odour.UseMarker = ICEEditorLayout.Toggle( "Use Odour Marker", "" , _odour.UseMarker , Info.ODOUR_MARKER );
				/*if( _odour.UseMarker )
				{
					EditorGUI.indentLevel++;
					ICEEditorLayout.RandomMinMaxGroupExt( "Interval", "", ref _odour.MarkerMinInterval, ref _odour.MarkerMaxInterval, 0, ref _odour.MarkerIntervalMax, 2, 5, 30, 0.25f, Info.STATUS_ODOUR_MARKER_INTERVAL );
					_odour.MarkerPrefab = (ICECreatureMarker)EditorGUILayout.ObjectField( "Marker Prefab", _odour.MarkerPrefab, typeof(ICECreatureMarker), false );
					EditorGUI.indentLevel--;
				}*/

				_odour.UseEffect = ICEEditorLayout.Toggle( "Use Odour Effect", "" , _odour.UseEffect , Info.ODOUR_EFFECT );
				/*if( _odour.UseEffect )
				{
					EditorGUI.indentLevel++;
					_odour.EffectPrefab = (GameObject)EditorGUILayout.ObjectField( "Effect Prefab", _odour.EffectPrefab, typeof(GameObject), false );
					EditorGUI.indentLevel--;
				}*/
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
			// ODOUR END

			return _odour;
		}



	}

	public class CreatureBehaviourEditor : WorldObjectEditor
	{	
		public static void DrawBehaviourAnimation( ICEWorldBehaviour _component, BehaviourModeObject _mode, BehaviourModeRuleObject _rule, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			List<AnimationDataObject> _animation_list = null;
			if( _mode != null && _mode.Rules.Count > 1 )
			{
				_animation_list = new List<AnimationDataObject>( _mode.Rules.Count );
				foreach( BehaviourModeRuleObject _tmp_rule in _mode.Rules )
					_animation_list.Add( _tmp_rule.Animation );						
			}

			AnimationEditor.DrawAnimationDataObject( _component, _rule.Animation, _type, _help, _title, _hint, _animation_list );
		}
		/// <summary>
		/// Draws the move data object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_move">Move.</param>
		public static void DrawBehaviourMove( ICECreatureControl _control, BehaviourModeObject _mode, BehaviourModeRuleObject _rule, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _rule == null )
				return;

			MoveDataObject _move = _rule.Move;
			BodyDataObject _body = _rule.Body;

			if( _move == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Movement";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_MOVEMENTS; 

			if( IsHeaderRequired( _type ) )
			{
				ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _move.Enabled == false );
				DrawObjectHeaderLine( _move, GetSimpleFoldout( _type ), _title, _hint );
				EditorGUI.EndDisabledGroup();

				if( _mode != null && _mode.Rules.Count > 1 )
				{
					if( ICEEditorLayout.Button( "SHARE", "Use this move settings for all associated rules" ) )
					{
						foreach( BehaviourModeRuleObject _tmp_rule in _mode.Rules )
							_tmp_rule.Move.Copy( _move );
					}
				}

				_move.Enabled = ICEEditorLayout.EnableButton( _move.Enabled );
				ICEEditorLayout.EndHorizontal( _help );
			}

			// CONTENT BEGIN
			if( CreatureObjectEditor.BeginObjectContentOrReturn( _type, _move ) )
				return;

			// VELOCITY BEGIN
			//_move.Motion.Type = (VelocityType)ICEEditorLayout.EnumPopup( "Velocity","", _move.Motion.Type, Info.BEHAVIOUR_MOVE_VELOCITY );
			ICEEditorLayout.BeginHorizontal();

			ICEEditorLayout.Label( "Velocity", false );

			bool _altitude_enabled = ICEEditorLayout.CheckButtonSmall( "VER", "Allows vertical movement settings", _move.Altitude.Enabled );

			if( _altitude_enabled != _move.Altitude.Enabled )
			{
				if( _altitude_enabled  )
					_body.Type = BodyOrientationType.GLIDER;
				else
					_body.Type = BodyOrientationType.DEFAULT;
			}

			_move.Altitude.Enabled = _altitude_enabled;

			_move.Motion.UseNegativeVelocity = ICEEditorLayout.CheckButtonSmall( "NEG", "Allows negative velocity settings", _move.Motion.UseNegativeVelocity );


			ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MOVE_VELOCITY );



			EditorGUI.indentLevel++;


			ICEEditorLayout.BeginHorizontal();

			if( _move.Motion.VelocityMaximum.z == 0 )
				_move.Motion.VelocityMaximum.z = Init.MOVE_VELOCITY_FORWARDS_MAX;

			EditorGUI.BeginDisabledGroup( _rule.UseRootMotion == true );

			if( _rule.UseRootMotion )
				ICEEditorLayout.MaxDefaultSlider( "Forwards (z)", "z-Velocity", 0, Init.DECIMAL_PRECISION_VELOCITY, 
					(_move.Motion.UseNegativeVelocity?-_move.Motion.VelocityMaximum.z:0), ref _move.Motion.VelocityMaximum.z, Init.MOVE_VELOCITY_FORWARDS_DEFAULT );
			else
				_move.Motion.Velocity.z = ICEEditorLayout.MaxDefaultSlider( "Forwards (z)", "z-Velocity", _move.Motion.Velocity.z, Init.DECIMAL_PRECISION_VELOCITY, 
					(_move.Motion.UseNegativeVelocity?-_move.Motion.VelocityMaximum.z:0), ref _move.Motion.VelocityMaximum.z, Init.MOVE_VELOCITY_FORWARDS_DEFAULT );

			_move.Motion.UseTargetVelocity = ICEEditorLayout.AutoButtonSmall( "Adapts the velocity automatically to target", _move.Motion.UseTargetVelocity );		
			_move.Motion.UseAdvancedVelocity = ICEEditorLayout.CheckButtonSmall( "ADV", "Use advanced velocity settings", _move.Motion.UseAdvancedVelocity );

			EditorGUI.EndDisabledGroup();

			ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MOVE_VELOCITY_FORWARDS );

			if( _move.Motion.UseAdvancedVelocity )
			{
				EditorGUI.indentLevel++;
				ICEEditorLayout.MinMaxSlider( "Variance (neg/pos)", "", ref _move.Motion.VelocityMinVariance, ref _move.Motion.VelocityMaxVariance, -1, 1, Init.DECIMAL_PRECISION_VELOCITY, 40, Info.BEHAVIOUR_MOVE_VELOCITY_VARIANCE );
				_move.Motion.Inertia = ICEEditorLayout.DefaultSlider( "Mass Inertia", "", _move.Motion.Inertia, Init.DECIMAL_PRECISION, 0, 1, 0, Info.BEHAVIOUR_MOVE_VELOCITY_INERTIA ); 
				EditorGUI.indentLevel--;
				//	_move.Motion.Velocity.x = ICEEditorLayout.DefaultSlider( "Sidewards \t(x)", "x-Velocity", _move.Motion.Velocity.x, 0.1f, -25, 25, 0, Info.BEHAVIOUR_MOVE_VELOCITY_SIDEWARDS);
				/*if( _move.Motion.UseAutoDrift )
						{
							EditorGUI.indentLevel++;
								_move.Motion.DriftMultiplier = ICEEditorLayout.DefaultSlider( "Drift Multiplier" , "",_move.Motion.DriftMultiplier, 0.01f, 0, 1, 0, Info.BEHAVIOUR_MOVE_VELOCITY_DRIFT );
							EditorGUI.indentLevel--;
						}*/

				//_move.Motion.Velocity.y = ICEEditorLayout.DefaultSlider( "Vertical \t(y)", "y-Velocity", _move.Motion.Velocity.y, 0.5f, -25, 25, 0,Info.BEHAVIOUR_MOVE_VELOCITY_VERTICAL );
				EditorGUILayout.Separator();
			}

			if( _move.Altitude.Enabled )
			{
				if( _move.Motion.VelocityMaximum.y == 0 )
					_move.Motion.VelocityMaximum.y = Init.MOVE_VELOCITY_FORWARDS_MAX;

				ICEEditorLayout.BeginHorizontal();
					if( _move.Altitude.UseVerticalSpeedCurve )
					{
						Keyframe[] _keys = new Keyframe[3]{
							new Keyframe( 0, 1f ),
							new Keyframe( 10, 5f ),
							new Keyframe( 100, 1f ) };

						_move.Altitude.VerticalSpeedCurve = ICEEditorLayout.DefaultCurve( "Vertical (y)", "y-Velocity by distance to target", _move.Altitude.VerticalSpeedCurve, new AnimationCurve(_keys ) );
					}					
					else
						_move.Motion.Velocity.y = ICEEditorLayout.MaxDefaultSlider( "Vertical (y)", "y-Velocity", _move.Motion.Velocity.y, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _move.Motion.VelocityMaximum.y, Init.MOVE_VELOCITY_FORWARDS_DEFAULT );

					_move.Altitude.UseVerticalSpeedCurve = ICEEditorLayout.CheckButtonSmall( "CRV", "", _move.Altitude.UseVerticalSpeedCurve );
				ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MOVE_VELOCITY_VERTICAL  );

				EditorGUI.indentLevel++;
				ICEEditorLayout.BeginHorizontal();

				if( _move.Altitude.Maximum == 0 )
					_move.Altitude.Maximum = 1000;

				string _suffix = ( _move.Altitude.UseTargetLevel ? "TRG" : ( _move.Altitude.UseAltitudeAboveGround ? "GND" : "ZERO" ) );
				string _desc = "Desired Altitude (min/max) " + ( _move.Altitude.UseAltitudeAboveGround ? "above zero level" : "above ground level" ) + ".";
				float _default_value = ( _move.Altitude.UseTargetLevel ? 1 : 0 );

				if( _move.Altitude.UseTargetLevel )
					_desc = "Desired vertical variance related to the given target position.";

				ICEEditorLayout.MinMaxDefaultSlider( "Operating Level (" + _suffix + ")", _desc , ref _move.Altitude.Min, ref _move.Altitude.Max, -_move.Altitude.Maximum, ref _move.Altitude.Maximum, _default_value, _default_value, Init.DECIMAL_PRECISION_VELOCITY ); 

				EditorGUI.BeginDisabledGroup( _move.Altitude.UseTargetLevel == true );
				if( _move.Altitude.UseTargetLevel )
					ICEEditorLayout.CheckButtonSmall( "GND", "Evaluates the level over the given ground", false );
				else
					_move.Altitude.UseAltitudeAboveGround = ICEEditorLayout.CheckButtonSmall( "GND", "Evaluates the level over the given ground", _move.Altitude.UseAltitudeAboveGround );
				EditorGUI.EndDisabledGroup();
				_move.Altitude.UseTargetLevel = ICEEditorLayout.CheckButtonSmall( "TRG", "", _move.Altitude.UseTargetLevel );

				ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MOVE_ALTITUDE );

				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}

			ICEEditorLayout.BeginHorizontal();

			if( _move.Motion.AngularVelocityMaximum.y == 0 )
				_move.Motion.AngularVelocityMaximum.y = Init.MOVE_VELOCITY_ANGULAR_MAX;

			_move.Motion.AngularVelocity.y = ICEEditorLayout.MaxDefaultSliderDis( "Angular (y)", "", _move.Motion.AngularVelocity.y, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _move.Motion.AngularVelocityMaximum.y, _move.Motion.UseAutomaticAngularVelocity, (_move.Motion.Velocity.z / 2) );  

			_move.Motion.UseAutomaticAngularVelocity = ICEEditorLayout.AutoButtonSmall( "Adapts the angular velocity automatically to forward velocity", _move.Motion.UseAutomaticAngularVelocity );

			_move.Motion.UseAdvancedAngularVelocity = ICEEditorLayout.CheckButtonSmall( "ADV", "", _move.Motion.UseAdvancedAngularVelocity );

			if( _move.Motion.UseAutomaticAngularVelocity && ! Application.isPlaying )
				_move.Motion.AngularVelocity.y = MathTools.CalculateAngularVelocity( _move.Motion.Velocity, _move.Motion.VelocityMaximum, _move.Motion.AngularVelocityMaximum );

			ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MOVE_ANGULAR_VELOCITY );

			if( _move.Motion.UseAdvancedAngularVelocity )
			{
				EditorGUI.indentLevel++;
				_move.Motion.AngularVelocity.z = ICEEditorLayout.MaxDefaultSlider( "Roll (z)", "", _move.Motion.AngularVelocity.z, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _move.Motion.AngularVelocityMaximum.z, (_move.Motion.Velocity.z / 2) );  
				_move.Motion.AngularVelocity.x = ICEEditorLayout.MaxDefaultSlider( "Pitch (x)", "", _move.Motion.AngularVelocity.x, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _move.Motion.AngularVelocityMaximum.x, (_move.Motion.Velocity.x / 2) );  
				EditorGUI.indentLevel--;
			}



			EditorGUI.indentLevel--;
			// VELOCITY END

			// VIEWING DIRECTION BEGIN
			EditorGUILayout.Separator();					
			_move.ViewingDirection = (ViewingDirectionType)ICEEditorLayout.EnumPopup( "Viewing Direction", "", _move.ViewingDirection, Info.BEHAVIOUR_MOVE_VIEWING_DIRECTION );

			if( _move.ViewingDirection == ViewingDirectionType.POSITION )
				_move = DrawBehaviourModeRuleMoveViewingDirection( _control, _move );
			// VIEWING DIRECTION END

			// BEHAVIOUR BODY BEGIN
			CreatureObjectEditor.DrawBodyDataObject( _control, _body, ! _move.Altitude.Enabled, Info.BODY_ORIENTATION_TYPE_OVERRIDE );
			// BEHAVIOUR BODY END

			// BEHAVIOUR MOVE BEGIN
			string _move_help = Info.BEHAVIOUR_MOVE_DEFAULT;
			if( _move.Type == MoveType.RANDOM )
				_move_help = Info.BEHAVIOUR_MOVE_RANDOM;
			else if( _move.Type == MoveType.CUSTOM )
				_move_help = Info.BEHAVIOUR_MOVE_CUSTOM;
			else if( _move.Type == MoveType.ESCAPE )
				_move_help = Info.BEHAVIOUR_MOVE_ESCAPE;
			else if( _move.Type == MoveType.AVOID )
				_move_help = Info.BEHAVIOUR_MOVE_AVOID;
			else if( _move.Type == MoveType.ORBIT )
				_move_help = Info.BEHAVIOUR_MOVE_ORBIT;
			else if( _move.Type == MoveType.DETOUR )
				_move_help = Info.BEHAVIOUR_MOVE_DETOUR;
			//else if( _move.Type == MoveType.COVER )
			//	_move_help = Info.BEHAVIOUR_MOVE_DETOUR;

			MoveType _move_type = (MoveType)ICEEditorLayout.EnumPopup( "Move", "", _move.Type, _move_help );					
			if( _move_type != _move.Type )
			{
				_move.StoppingDistance = _control.Creature.Move.DefaultMove.StoppingDistance;
				_move.SegmentLength = _control.Creature.Move.DefaultMove.SegmentLength;
				_move.SegmentVariance = _control.Creature.Move.DefaultMove.SegmentVariance;					
				_move.DeviationVariance = _control.Creature.Move.DefaultMove.DeviationVariance;
				_move.IgnoreLevelDifference = _control.Creature.Move.DefaultMove.IgnoreLevelDifference;
			}

			_move.Type = _move_type;				

			EditorGUI.indentLevel++;					
			if( _move.Type == MoveType.RANDOM )
				_move = DrawBehaviourModeRuleMoveRandom( _control, _move );
			else if( _move.Type == MoveType.CUSTOM )
				_move = DrawBehaviourModeRuleMoveCustom( _control, _move );
			else if( _move.Type == MoveType.ESCAPE )
				_move = DrawBehaviourModeRuleMoveEscape( _control, _move );
			else if( _move.Type == MoveType.AVOID )
				_move = DrawBehaviourModeRuleMoveAvoid( _control, _move );
			else if( _move.Type == MoveType.ORBIT )
				_move = DrawBehaviourModeRuleMoveOrbit( _control, _move );
			else if( _move.Type == MoveType.DETOUR )
				_move = DrawBehaviourModeRuleMoveDetour( _control, _move );		
			//else if( _move.Type == MoveType.COVER )
			//	_move = DrawBehaviourModeRuleMoveCover( _control, _move );	
			EditorGUI.indentLevel--;
			// BEHAVIOUR MOVE END

			CreatureObjectEditor.EndObjectContent();
			// CONTENT END
		}


		private static MoveDataObject DrawBehaviourModeRuleMoveCustom( ICECreatureControl _control, MoveDataObject _move )
		{
			EditorGUILayout.Separator();
			return CreatureEditorLayout.DrawMove( _move );
		}

		private static MoveDataObject DrawBehaviourModeRuleMoveAvoid( ICECreatureControl _control, MoveDataObject _move )
		{
			EditorGUILayout.Separator();

			_move.Avoid.AvoidDistance = ICEEditorLayout.DefaultSlider( "Max. Avoid Distance", "", _move.Avoid.AvoidDistance, Init.MOVE_AVOID_DISTANCE_STEP, Init.MOVE_AVOID_DISTANCE_MIN, Init.MOVE_AVOID_DISTANCE_MAX, Init.MOVE_AVOID_DISTANCE_DEFAULT, Info.BEHAVIOUR_MOVE_AVOID_DISTANCE );  

			EditorGUILayout.Separator();
			return CreatureEditorLayout.DrawMove( _move );
		}


		private static MoveDataObject DrawBehaviourModeRuleMoveEscape( ICECreatureControl _control, MoveDataObject _move )
		{
			_move.Escape.EscapeDistance = ICEEditorLayout.DefaultSlider( "Escape Distance", "", _move.Escape.EscapeDistance, Init.MOVE_ESCAPE_DISTANCE_STEP, Init.MOVE_ESCAPE_DISTANCE_MIN, Init.MOVE_ESCAPE_DISTANCE_MAX, Init.MOVE_ESCAPE_DISTANCE_DEFAULT, Info.BEHAVIOUR_MOVE_ESCAPE_DISTANCE );  
			_move.Escape.RandomEscapeAngle = ICEEditorLayout.DefaultSlider( "Random Escape Angle", "", _move.Escape.RandomEscapeAngle, Init.MOVE_ESCAPE_RANDOM_ANGLE_STEP, Init.MOVE_ESCAPE_RANDOM_ANGLE_MIN, Init.MOVE_ESCAPE_RANDOM_ANGLE_MAX, Init.MOVE_ESCAPE_RANDOM_ANGLE_DEFAULT, Info.BEHAVIOUR_MOVE_ESCAPE_ANGLE );  
			EditorGUILayout.Separator();
			return CreatureEditorLayout.DrawMove( _move );
		}

		private static MoveDataObject DrawBehaviourModeRuleMoveOrbit( ICECreatureControl _control, MoveDataObject _move )
		{
			EditorGUILayout.Separator();

			_move.Orbit.Radius = ICEEditorLayout.DefaultSlider( "Orbit Radius", "", _move.Orbit.Radius, Init.MOVE_ORBIT_RADIUS_STEP, _move.StoppingDistance, Init.MOVE_ORBIT_RADIUS_MAX, Init.MOVE_ORBIT_RADIUS_DEFAULT );  

			EditorGUI.indentLevel++;

			_move.Orbit.RadiusShift = ICEEditorLayout.DefaultSlider( "Radius Shift", "", _move.Orbit.RadiusShift, Init.MOVE_ORBIT_SHIFT_STEP, - _move.Orbit.Radius * 2, _move.Orbit.Radius * 2 , Init.MOVE_ORBIT_SHIFT_DEFAULT );  

			if( _move.Orbit.RadiusShift < 0 )
				_move.Orbit.MinDistance = ICEEditorLayout.DefaultSlider( "Min Distance", "", _move.Orbit.MinDistance, Init.MOVE_ORBIT_SHIFT_STEP, _move.StoppingDistance, _move.Orbit.Radius, _move.StoppingDistance );  
			else if( _move.Orbit.RadiusShift > 0 )
				_move.Orbit.MaxDistance = ICEEditorLayout.DefaultSlider( "Max Distance", "", _move.Orbit.MaxDistance, Init.MOVE_ORBIT_SHIFT_STEP, _move.Orbit.Radius, Init.MOVE_ORBIT_RADIUS_MAX, _move.Orbit.Radius + _move.StoppingDistance );  

			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			return CreatureEditorLayout.DrawMove( _move );
		}


		private static MoveDataObject DrawBehaviourModeRuleMoveRandom( ICECreatureControl _control, MoveDataObject _move )
		{
			EditorGUILayout.Separator();
			return CreatureEditorLayout.DrawMove( _move );
		}

		private static bool _add_viewing_position = false;
		private static MoveDataObject DrawBehaviourModeRuleMoveViewingDirection( ICECreatureControl _control, MoveDataObject _move )
		{
			EditorGUI.indentLevel++;
			ICEEditorLayout.BeginHorizontal();

			if( _add_viewing_position )
			{
				Transform _tmp_transform = (Transform)EditorGUILayout.ObjectField("Position", null, typeof(Transform), true);

				if( _tmp_transform != null )
				{
					_move.ViewingDirectionPosition = _tmp_transform.position;
					_add_viewing_position = false;
				}

			}
			else
				_move.ViewingDirectionPosition = EditorGUILayout.Vector3Field( "Position", _move.ViewingDirectionPosition );

			ICEEditorLayout.ButtonDisplayObject(  _move.ViewingDirectionPosition ); 


			if( _add_viewing_position )
				GUI.backgroundColor = Color.yellow;

			if( ICEEditorLayout.AddButton( "Defines a new position by the specified object." ) )
				_add_viewing_position = ! _add_viewing_position;

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;			

			ICEEditorLayout.EndHorizontal();
			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();


			return _move;
		}

		private static bool _add_position = false;
		private static MoveDataObject DrawBehaviourModeRuleMoveDetour( ICECreatureControl _control, MoveDataObject _move )
		{
			ICEEditorLayout.BeginHorizontal();

			if( _add_position )
			{
				Transform _tmp_transform = (Transform)EditorGUILayout.ObjectField("Detour Position", null, typeof(Transform), true);

				if( _tmp_transform != null )
				{
					_move.Detour.Position = _tmp_transform.position;
					_add_position = false;
				}

			}
			else
				_move.Detour.Position = EditorGUILayout.Vector3Field( "Detour Position", _move.Detour.Position );

			ICEEditorLayout.ButtonDisplayObject( _move.Detour.Position );

			if( _add_position )
				GUI.backgroundColor = Color.yellow;

			if( ICEEditorLayout.AddButton( "Defines a new position by the specified object." ) )
				_add_position = ! _add_position;

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;


			ICEEditorLayout.EndHorizontal();

			_move = CreatureEditorLayout.DrawMove( _move );

			EditorGUILayout.Separator();


			return _move;
		}

		private static MoveDataObject DrawBehaviourModeRuleMoveCover( ICECreatureControl _control, MoveDataObject _move )
		{
			_move.Cover.MaxDistance = ICEEditorLayout.MaxDefaultSlider( "Max. Distance", "", _move.Cover.MaxDistance, Init.DECIMAL_PRECISION_DISTANCES, 0f, ref _move.Cover.MaxDistanceMaximum, 10, "" );
			_move.Cover.StepAngle = ICEEditorLayout.DefaultSlider( "Scan Step Angle", "", _move.Cover.StepAngle, Init.DECIMAL_PRECISION_ANGLE, 1f, 36, 3.6f, "" );
			_move.Cover.HorizontalOffset = ICEEditorLayout.MaxDefaultSlider( "Scan Horizontal Offset", "", _move.Cover.HorizontalOffset, Init.DECIMAL_PRECISION_DISTANCES, 0f, ref _move.Cover.HorizontalOffsetMaximum, 0.5f, "" );
			_move.Cover.VerticalOffset = ICEEditorLayout.MaxDefaultSlider( "Scan Vertical Offset", "", _move.Cover.VerticalOffset, Init.DECIMAL_PRECISION_DISTANCES, - _move.Cover.VerticalOffsetMaximum, ref _move.Cover.VerticalOffsetMaximum, 0f, "" );

			EditorGUILayout.Separator();

			_move = CreatureEditorLayout.DrawMove( _move );

			EditorGUILayout.Separator();


			return _move;
		}


		/// <summary>
		/// Draws the behaviour mode rule link object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_list">List.</param>
		/// <param name="_link">Link.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		public static void DrawBehaviourModeRuleLinkObject( ICECreatureControl _control, List<BehaviourModeRuleObject> _list, BehaviourModeRuleLinkObject _link, EditorHeaderType _type, string _default_key = "", string _help = "", string _title = "", string _hint = "" )
		{
			if( _link == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Link";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_LINK;

			DrawObjectHeader( _link, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _link ) )
				return;

			if( _list.Count > 1 )
				_link.Type = (LinkType)ICEEditorLayout.EnumPopup( "Link Type","", _link.Type, Info.BEHAVIOUR_LINK_SELECT );
			else
				_link.Type = LinkType.MODE;

			if( _link.Type == LinkType.MODE )
			{
				EditorGUI.indentLevel++;
				_link.BehaviourModeKey =  BehaviourEditor.BehaviourSelect( _control, "Next Behaviour Mode", "Next desired Behaviour Mode", _link.BehaviourModeKey, ( ! string.IsNullOrEmpty( _default_key ) ? _default_key.ToUpper() + "_LINK" : "LINK" ), Info.BEHAVIOUR_LINK_MODE );
				EditorGUI.indentLevel--;

			}
			else 
			{
				EditorGUI.indentLevel++;
				_link.RuleIndex = (int)ICEEditorLayout.Slider( "Next Rule", "Next desired Behaviour Rule" , _link.RuleIndex + 1, 1, 1 , _list.Count, Info.BEHAVIOUR_LINK_RULE )-1;
				EditorGUI.indentLevel--;
			}

			EndObjectContent();
			// CONTENT END
		}
	}

	/// <summary>
	/// Object editor.
	/// </summary>
	public class CreatureObjectEditor : WorldObjectEditor
	{	


		public static void DrawWaypointLinksObject( ICECreatureWaypoint _component, WaypointLinksObject _links, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _links == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Links";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;

			_links.Init( _component );


			DrawObjectHeader( _links, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _links ) )
				return;

			EditorGUILayout.Separator();

			for( int i = 0 ; i < _links.Links.Count ; i++ )
				DrawWaypointLinkObject( _links, i, EditorHeaderType.FOLDOUT_ENABLED );

			ICEEditorLayout.BeginHorizontal();
			_links.AddWaypointByObject( (GameObject)EditorGUILayout.ObjectField("New Waypoint Link", null, typeof(GameObject), true ) );
			if( ICEEditorLayout.AddButton( "Adds new Waypoint Link" ) )
			{
				GameObject _new_waypoint = new GameObject( "Waypoint" );
				_new_waypoint.transform.position = _component.gameObject.transform.position;
				_links.AddWaypointByObject( _new_waypoint );

				Selection.activeGameObject = _new_waypoint;
			}

			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );



			EndObjectContent();
			// CONTENT END

		}

		public static void DrawWaypointLinkObject( WaypointLinksObject _links, int _index,  EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			WaypointLinkObject _link = _links.Links[_index];
			if( _link == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Link";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.AUDIO;


			ICEEditorLayout.BeginHorizontal();
			EditorGUI.BeginDisabledGroup( _link.Enabled == false );
			_link.Foldout = ICEEditorLayout.Foldout( _link.Foldout, "Link to #" + (int)(_index+1), false );
			EditorGUI.EndDisabledGroup();
			_link.Waypoint = (ICECreatureWaypoint)EditorGUILayout.ObjectField( _link.Waypoint , typeof(ICECreatureWaypoint), true );

				int _indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;
				_link.Weight = EditorGUILayout.FloatField( _link.Weight, GUILayout.Width( 40 ) ); 
			EditorGUI.indentLevel = _indent;
				bool _is_two_way = _link.IsTwoWay;
				bool _use_two_way = ICEEditorLayout.CheckButtonSmall( "TW", "",  _is_two_way );

				if( _use_two_way != _is_two_way ) 
				{
					if( _use_two_way )
						_link.Waypoint.Links.AddWaypointByObject( _link.Owner );
					else
						_link.Waypoint.Links.RemoveWaypointByObject( _link.Owner );
				}

				if( ICEEditorLayout.ListUpDownButtonsMini<WaypointLinkObject>( _links.Links, _index ) )
					return;
			
				_link.Enabled = ICEEditorLayout.EnableButtonMini( _link.Enabled );


				if( ICEEditorLayout.AddButtonMini( "Adds new Waypoint Link" ) )
				{


					ICECreatureWaypoint _waypoint = _links.SubdivideLink( _link );

					Selection.activeGameObject = _waypoint.gameObject;
				}

				if( ICEEditorLayout.ListDeleteButtonMini<WaypointLinkObject>( _links.Links, _link, "Removes this audio clip." ) )
					return;


			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );

			//DrawObjectHeader( _link, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _link ) )
				return;


			ICEEditorLayout.BeginHorizontal();




			ICEEditorLayout.EndHorizontal( Info.AUDIO_CLIP );

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawBodyPartMotionObject( ICEWorldBehaviour _control, BodyPartMotionObject _motion, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _motion == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Motion";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BODYPARTMOTION;


			DrawObjectHeader( _motion, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _motion ) )
				return;



			_motion.RotationSpeed = ICEEditorLayout.MaxDefaultSlider( "Rotation Speed", "", _motion.RotationSpeed, Init.DECIMAL_PRECISION, 0, ref _motion.RotationSpeedMaximum, 2, Info.BODYPARTMOTION_ROTATIONSPEED );

			ICEEditorLayout.BeginHorizontal();
			ICEEditorLayout.Label( "Rotation Limits (degrees)" );
			_motion.UseRotationLimits = ICEEditorLayout.EnableButton( _motion.UseRotationLimits );
			ICEEditorLayout.EndHorizontal();
			if( _motion.UseRotationLimits )
			{
				// *** Note: min and max x value was switched for display reasons, because by default negative values results an upwards move
				_motion.MinRotationLimits.x *= (-1);
				_motion.MaxRotationLimits.x *= (-1);

				EditorGUI.indentLevel++;
					ICEEditorLayout.MinMaxDefaultSlider( "x-Axis", "", ref _motion.MaxRotationLimits.x, ref _motion.MinRotationLimits.x, - _motion.RotationLimitsMaximum.x, ref _motion.RotationLimitsMaximum.x, -15,25, Init.DECIMAL_PRECISION_ANGLE, 50, "" );
					ICEEditorLayout.MinMaxDefaultSlider( "y-Axis", "", ref _motion.MinRotationLimits.y, ref _motion.MaxRotationLimits.y, - _motion.RotationLimitsMaximum.y, ref _motion.RotationLimitsMaximum.y, -30,30, Init.DECIMAL_PRECISION_ANGLE, 50, "" );
					ICEEditorLayout.MinMaxDefaultSlider( "z-Axis", "", ref _motion.MinRotationLimits.z, ref _motion.MaxRotationLimits.z, - _motion.RotationLimitsMaximum.z, ref _motion.RotationLimitsMaximum.z, 0,0, Init.DECIMAL_PRECISION_ANGLE, 50, "" );
				EditorGUI.indentLevel--;

				_motion.MinRotationLimits.x *= (-1);
				_motion.MaxRotationLimits.x *= (-1);
			}

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawRegisterDefaultSettings( ICECreatureRegister _register, RegisterOptionsObject _options, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _register == null || _options == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "World Settings";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.REGISTER_OPTIONS_DEBUG;

			DrawObjectHeader( _options, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _options ) )
				return;

				ICEEditorLayout.DrawGroundCheck( ref _options.GroundCheck, _options.GroundLayer.Layers, false, Info.REGISTER_OPTIONS_POOL_MANAGEMENT_GROUND_CHECK );
				ICEEditorLayout.DrawWaterCheck( ref _options.WaterCheck, _options.WaterLayer.Layers, false, Info.REGISTER_OPTIONS_POOL_MANAGEMENT_WATER_CHECK);
				ICEEditorLayout.DrawObstacleCheck( ref _options.ObstacleCheck, _options.ObstacleLayer.Layers, false, Info.REGISTER_OPTIONS_POOL_MANAGEMENT_OBSTACLE_CHECK );

				EditorGUILayout.Separator();
				_options.TriggerInteraction = (QueryTriggerInteraction)ICEEditorLayout.EnumPopup( "Ground Trigger Interaction", "", _options.TriggerInteraction, Info.REGISTER_OPTIONS_POOL_MANAGEMENT_OBSTACLE_CHECK ); 
			EndObjectContent();
			// CONTENT END

		}

		public static void DrawRegisterDebugObject( ICECreatureRegister _register, CreatureRegisterDebugObject _object, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _object == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Debug";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.REGISTER_OPTIONS_DEBUG;

			DrawObjectHeader( _object, _type, _title, _hint, _help );

			_register.UseDebug = _object.Enabled;

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _object ) )
				return;

				Time.timeScale = ICEEditorLayout.DefaultSlider( "Time Scale", "", Time.timeScale, Init.DECIMAL_PRECISION_TIMER, 0, 5, 1, "" );
			
				if( _register.gameObject.GetComponent<ICECreatureRegisterDebug>() == null )
					_register.gameObject.AddComponent<ICECreatureRegisterDebug>();
			
				{
					_object.UseDrawSelected = ICEEditorLayout.Toggle( "Draw Selected Only", "", _object.UseDrawSelected, Info.REGISTER_OPTIONS_DEBUG_GIZMOS_MODE );

					// BEGIN GIZMOS 
					ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _object.ShowReferenceGizmos == false );
					_object.ColorReferences = EditorGUILayout.ColorField( new GUIContent( "References", ""),_object.ColorReferences);
					_object.ShowReferenceGizmosText = ICEEditorLayout.CheckButtonMiddle( "TEXT", "Enables/Disables text labels",_object.ShowReferenceGizmosText ); 
					EditorGUI.EndDisabledGroup();
					_object.ShowReferenceGizmos = ICEEditorLayout.EnableButton( "Enables/Disables Reference Gizmos",_object.ShowReferenceGizmos ); 
					ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_DEBUG_REFERENCES );
					ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _object.ShowCloneGizmos == false );
					_object.ColorClones = EditorGUILayout.ColorField( new GUIContent( "Clones", ""),_object.ColorClones);
					_object.ShowCloneGizmosText = ICEEditorLayout.CheckButtonMiddle( "TEXT", "Enables/Disables text labels",_object.ShowCloneGizmosText ); 
					EditorGUI.EndDisabledGroup();
					_object.ShowCloneGizmos = ICEEditorLayout.EnableButton( "Enables/Disables Clones Gizmos",_object.ShowCloneGizmos ); 
					ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_DEBUG_CLONES );
					ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _object.ShowSpawnPointGizmos == false );
					_object.ColorSpawnPoints = EditorGUILayout.ColorField( new GUIContent( "SpawnPoints", ""),_object.ColorSpawnPoints);
					_object.ShowSpawnPointGizmosText = ICEEditorLayout.CheckButtonMiddle( "TEXT", "Enables/Disables text labels",_object.ShowSpawnPointGizmosText ); 
					EditorGUI.EndDisabledGroup();
					_object.ShowSpawnPointGizmos = ICEEditorLayout.EnableButton( "Enables/Disables SpawnPoint Gizmos",_object.ShowSpawnPointGizmos ); 
					ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_DEBUG_SPAWNPOINTS );


					// END GIZMOS 
				}
		
	

			EndObjectContent();
			// CONTENT END

		}

		public static void DrawHierarchyManagementObject( ICECreatureRegister _register, HierarchyManagementObject _object, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _object == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Entity Hierarchy Management";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.REGISTER_OPTIONS_GROUPS;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _object.Enabled == false );
					DrawObjectHeaderLine( _object, GetSimpleFoldout( _type ), _title, _hint );
				EditorGUI.EndDisabledGroup();

				bool _hierarchy_management_enabled = ICEEditorLayout.EnableButton( _object.Enabled );

				if( _hierarchy_management_enabled != _object.Enabled )
				{
					_object.Enabled = _hierarchy_management_enabled;
					_object.ReorganizeSceneObjects();
				}

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _object ) )
				return;
			
				_object.Init( _register );

					ICEEditorLayout.BeginHorizontal();
					_object.UpdateSuffix( ICEEditorLayout.Text( "Suffix", "", _object.GroupSuffix ) );

					if( ICEEditorLayout.ButtonMiddle( "UPDATE", "Sorts all creatures, items and locations according to the given groups." ) )
					{
						_object.UpdateHierarchyGroups( false, true );
						_object.ReorganizeSceneObjects();
					}

					if( ICEEditorLayout.ButtonMiddle( "RESET", "Resets the group list" ) )
						_object.ResetHierarchyGroups();

					ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_GROUPS_ROOT );
		
					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _object.HierarchyRootGroup.Enabled == false );
							Transform _root_transform = (Transform)EditorGUILayout.ObjectField( new GUIContent( "Root", ""), _object.HierarchyRootGroup.GroupTransform, typeof(Transform), true);
										
							if( _root_transform != _object.HierarchyRootGroup.GroupTransform )
							{
								_object.HierarchyRootGroup.GroupTransform = _root_transform;
								_object.ReorganizeSceneObjects();
							}

						EditorGUI.EndDisabledGroup();			
						bool _root_enabled = ICEEditorLayout.EnableButton( _object.HierarchyRootGroup.Enabled );
						if( _root_enabled != _object.HierarchyRootGroup.Enabled )
						{
							_object.HierarchyRootGroup.Enabled = _root_enabled;
							_object.ReorganizeSceneObjects();
						}

					ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_GROUPS_ROOT );
					EditorGUI.indentLevel++;
						foreach( HierarchyGroupObject _group in _object.HierarchyGroups )
						{
							if( _group != null )
							{
								ICEEditorLayout.BeginHorizontal();
									EditorGUI.BeginDisabledGroup( _group.Enabled == false );

										Transform _transform = (Transform)EditorGUILayout.ObjectField( new GUIContent( _group.EntityType.ToString(), ""), _group.GroupTransform, typeof(Transform), true, GUILayout.MinWidth( 80 ) );

										if( _transform != _group.GroupTransform )
										{
											_group.GroupTransform = _transform;
											_object.ReorganizeSceneObjects();
										}

								/*
										int _indent = EditorGUI.indentLevel;
										EditorGUI.indentLevel = 0;
										_group.GroupColor = EditorGUILayout.ColorField( _group.GroupColor, GUILayout.Width( 40 ) );
										EditorGUI.indentLevel = _indent;


										if( ICEEditorLayout.AutoButton( "" ) )
										{
											_object.UpdateHierarchyGroup( _group );
											_object.ReorganizeSceneObjects();
										}*/
					
									EditorGUI.EndDisabledGroup();		
		
									bool _enabled = ICEEditorLayout.EnableButton( _group.Enabled );

									if( _enabled != _group.Enabled )
									{
										_group.Enabled = _enabled;
										_object.ReorganizeSceneObjects();
									}

								ICEEditorLayout.EndHorizontal( Info.REGISTER_OPTIONS_GROUPS_PLAYER );
							}
						}
					EditorGUI.indentLevel--;
				EditorGUILayout.Separator();

			EndObjectContent();
			// CONTENT END

		}


		public static void DrawBehaviourModeFavouredObject( ICECreatureControl _control, BehaviourModeFavouredObject _object, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _object == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Favoured";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = "";

			DrawObjectHeader( _object, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _object ) )
				return;

				_object.FavouredPriority = ICEEditorLayout.DefaultSlider( "Priority", "Blocks lower prioritized behaviours and breaks for higher ones.", _object.FavouredPriority, 1, 0, 100, 0,  Info.BEHAVIOUR_MODE_FAVOURED_PRIORITY );

				ICEEditorLayout.MinMaxRandomDefaultSlider( "Desired Length", "", ref _object.MinRuntime, ref _object.MaxRuntime, 0, ref _object.MaxRuntimeMaximum,0,0, Init.DECIMAL_PRECISION_TIMER, 30, Info.BEHAVIOUR_MODE_FAVOURED_PRIORITY );

				_object.FavouredUntilNextMovePositionReached = ICEEditorLayout.Toggle("Next Move Position", "Blocks other targets and behaviours until the next move position was reached.", _object.FavouredUntilNextMovePositionReached, Info.BEHAVIOUR_MODE_FAVOURED_MOVE_POSITION_REACHED );
				_object.FavouredUntilTargetMovePositionReached = ICEEditorLayout.Toggle("Target Move Position", "Blocks other targets and behaviours until the target move position was reached.", _object.FavouredUntilTargetMovePositionReached, Info.BEHAVIOUR_MODE_FAVOURED_TARGET_MOVE_POSITION_REACHED );
				_object.FavouredTarget = ICEEditorLayout.Toggle( "Specific Target", "Blocks other targets and behaviours while waiting for a specific target", _object.FavouredTarget, Info.BEHAVIOUR_MODE_FAVOURED_TARGET );

				if( _object.FavouredTarget )
				{
					EditorGUI.indentLevel++;
					_object.FavouredTargetName =  Popups.TargetPopup( "Wait for Target", "", _object.FavouredTargetName, Info.BEHAVIOUR_MODE_FAVOURED_TARGET_POPUP );
					EditorGUI.BeginDisabledGroup( _object.FavouredTargetName.Trim() == "" );
					_object.FavouredTargetRange = ICEEditorLayout.DefaultSlider( "Range", "Blocks other targets and behaviours until the specified target is inside the range", _object.FavouredTargetRange, 0.25f, 0, 100, 10, Info.BEHAVIOUR_MODE_FAVOURED_TARGET_RANGE );
					EditorGUI.EndDisabledGroup();
					EditorGUI.indentLevel--;
				}

				//if( _mode.HasDetourRules )
				//	_object.FavouredUntilDetourPositionReached = ICEEditorLayout.Toggle("Detour Position", "Blocks other targets and behaviours until a detour position was reached.", _object.FavouredUntilDetourPositionReached, Info.BEHAVIOUR_MODE_FAVOURED_DETOUR );

			EndObjectContent();
			// CONTENT END

		}

		/// <summary>
		/// Draws the body data object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_body">Body.</param>
		public static void DrawBodyDataObject( ICECreatureControl _control, BodyDataObject _body, bool _allow_selection = true, string _help = "" )
		{
			if( _body == null || _control == null )
				return;

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BODY_ORIENTATION_TYPE;
			
			if( _body.Type == BodyOrientationType.DEFAULT  || _body.Type == BodyOrientationType.BIPED )
				_body.UseAdvanced = false;

			if( _control.Creature.Move.MotionControl == MotionControlType.CUSTOM )
			{
				_body.Type = BodyOrientationType.DEFAULT;
				_body.UseAdvanced = false;
			}

			EditorGUI.BeginDisabledGroup( _control.Creature.Move.MotionControl == MotionControlType.CUSTOM ); 

				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _allow_selection == false );
						_body.Type = (BodyOrientationType)ICEEditorLayout.EnumPopup("Body Orientation", "Vertical direction of the body relative to the ground", _body.Type );
					EditorGUI.EndDisabledGroup();
					EditorGUI.BeginDisabledGroup( _body.Type == BodyOrientationType.DEFAULT || _body.Type == BodyOrientationType.BIPED );
						_body.UseAdvanced = ICEEditorLayout.CheckButtonSmall( "ADV", "", _body.UseAdvanced );
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal( _help );

				if( _body.Type != BodyOrientationType.DEFAULT )
				{
					if( _body.UseAdvanced )
					{
						//if( _body.DefaultWidth == 0 || _body.DefaultLength == 0 || _body.DefaultHeight == 0 || _body.DefaultWidth != _body.Width || _body.DefaultLength != _body.Length || _body.DefaultHeight != _body.Height )
						_body.GetDefaultSize( _control.gameObject );

						EditorGUI.indentLevel++;
							_body.Width = ICEEditorLayout.DefaultSlider( "Width", "", _body.Width, 0.01f, 0, 45, _body.DefaultWidth );
							EditorGUI.indentLevel++;
								_body.WidthOffset = ICEEditorLayout.DefaultSlider( "x-Offset", "", _body.WidthOffset, 0.01f, -10, 10, 0 );
							EditorGUI.indentLevel--;
							_body.Length = ICEEditorLayout.DefaultSlider( "Depth", "", _body.Length, 0.01f, 0, 45, _body.DefaultLength );
							EditorGUI.indentLevel++;
								_body.LengthOffset = ICEEditorLayout.DefaultSlider( "z-Offset", "", _body.LengthOffset, 0.01f, -10, 10, 0 );
							EditorGUI.indentLevel--;			
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();
					}

					EditorGUI.indentLevel++;
						_body.UseRollAngle = ICEEditorLayout.Toggle("Use Roll Angle", "Allows to lean into a turn", _body.UseRollAngle ,  Info.BODY_ROLL_ANGLE );
						EditorGUI.indentLevel++;
							if( _body.UseRollAngle )
							{
								//Info.Warning( Info.ESSENTIALS_SYSTEM_LEAN_ANGLE_WARNING );
								_body.RollAngleMultiplier = ICEEditorLayout.DefaultSlider( "Roll Angle Multiplier", "Roll angle multiplier", _body.RollAngleMultiplier, 0.05f, 0, 1, 0.5f );
								_body.MaxRollAngle = ICEEditorLayout.DefaultSlider( "Max. Roll Angle", "Maximum roll angle", _body.MaxRollAngle, 0.25f, 0, 90, 35 );
								EditorGUILayout.Separator();
							}
						EditorGUI.indentLevel--;


						if( _body.Type == BodyOrientationType.GLIDER )
						{
							_body.UsePitch = ICEEditorLayout.Toggle("Use Pitch Angle", "Allows to adapt the pitch acording to the given level difference", _body.UsePitch ,  Info.BODY_PITCH_ANGLE );
							if( _body.UsePitch )
							{
								EditorGUI.indentLevel++;
									_body.PitchAngleMultiplier = ICEEditorLayout.DefaultSlider( "Pitch Angle Multiplier", "Pitch angle multiplier", _body.PitchAngleMultiplier, 0.05f, 0, 1, 0.5f );
									_body.MaxPitchAngle = ICEEditorLayout.DefaultSlider( "Max. Pitch Angle", "Maximum pitch angle", _body.MaxPitchAngle, 0.25f, 0, 90, 35 );
									EditorGUILayout.Separator();
								EditorGUI.indentLevel--;
							}

						}

					EditorGUI.indentLevel--;

					EditorGUILayout.Separator();
				}

			EditorGUI.EndDisabledGroup();

			if( _body.Type != BodyOrientationType.DEFAULT && Terrain.activeTerrain == null &&  _control.Creature.Move.GroundCheck != GroundCheckType.RAYCAST )
			{
				string _text = "While using your scene without an active Terrain you should set the Ground Check Type of the Essential Settings to RAYCAST, " +
					"otherwise the Body Orientation settings will be ignored during the runtime.";
				EditorGUILayout.HelpBox( _text, MessageType.Warning );
			}
		}


		/// <summary>
		/// Draws the look data object.
		/// </summary>
		/// <returns>The look data object.</returns>
		/// <param name="_look">Look.</param>
		/// <param name="_help">Help.</param>
		public static void DrawLookDataObject( LookDataObject _look, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _look == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Look";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = "";

			DrawObjectHeader( _look, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _look ) )
				return;

			EditorGUI.indentLevel++;
				_look.InvisibilityType = (LookInvisibleType)ICEEditorLayout.EnumPopup( "Invisibility Type", "", _look.InvisibilityType, Info.LOOK_INVISIBILITY_TYPE );

				if( _look.InvisibilityType != LookInvisibleType.None )
					_look.UseInvisibility = ICEEditorLayout.Toggle("Use Invisibility", "Deactivates/Activates all renderer", _look.UseInvisibility, Info.LOOK_INVISIBILITY );
			EditorGUI.indentLevel--;

			EndObjectContent();
			// CONTENT END
		}

		public static bool DrawSpawnPointObject( SpawnPointObject _point, List<SpawnPointObject> _list, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _point == null )
				return false;

			if( string.IsNullOrEmpty( _title ) )
				_title = "SpawnPoint " + ( _point.SpawnPointName != "" ? "'" + _point.SpawnPointName + "'": "(INVALID)" );
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _point.Enabled == false );
					DrawObjectHeaderLine( _point, ObjectEditor.GetSimpleFoldout( _type ), _title, _hint );
					_point.UseRandomRect = ICEEditorLayout.CheckButtonMiddle( "RECT", "", _point.UseRandomRect );
					GUILayout.Space( 5 );
				EditorGUI.EndDisabledGroup();

				//EditorGUI.BeginDisabledGroup( (_obj.CreatureController.Creature.Essentials.Target.TargetGameObject == _point.SpawnPointGameObject?true:false) );
				if( ICEEditorLayout.ListDeleteButton<SpawnPointObject>( _list, _point ) )
					return true;
				//EditorGUI.EndDisabledGroup();

				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<SpawnPointObject>( _list, _list.IndexOf( _point ) ) )
					return true;


				_point.Enabled = ICEEditorLayout.EnableButton( "Enables/disables this spawn point.", _point.Enabled );

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _point ) )
				return false;
			
				ICEEditorLayout.BeginHorizontal();
					CreatureObjectEditor.DrawSpawnPointObjectLine( _point, "SpawnPoint" );
				ICEEditorLayout.EndHorizontal( Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT_REFERENCE  );

				if( _point.UseRandomRect )
					_point.RandomRect = ICEEditorLayout.Vector3Field( "Random Rect", "", _point.RandomRect, Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT_RECT );
				else
					ICEEditorLayout.MinMaxRandomDefaultSlider( "Random Range", "Random positioning range around the spawn point", ref _point.SpawningRangeMin, ref _point.SpawningRangeMax, 0, ref _point.SpawningRangeMaximum,0,5, Init.DECIMAL_PRECISION_DISTANCES, 40, Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT_RANGE );

				if( CreatureRegister.GroundCheck == GroundCheckType.RAYCAST )
					_point.LevelDifference = ICEEditorLayout.MaxDefaultSlider( "Level Difference","Potential level differences related to the reference object", _point.LevelDifference, 0.025f, 0, ref _point.LevelDifferenceMaximum, 0.5f, Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT_LEVEL );

				_point.LevelOffset = ICEEditorLayout.MaxDefaultSlider( "Level Offset","Additional level offset which will be used during the spawning process.", _point.LevelOffset, 0.025f, -_point.LevelOffsetMaximum, ref _point.LevelOffsetMaximum, Init.DECIMAL_PRECISION, Info.REGISTER_REFERENCE_OBJECT_SPAWN_POINT_LEVEL_OFFSET );

			EndObjectContent();
			// CONTENT END

			return false;
		}

		/// <summary>
		/// Draws the spawn point object.
		/// </summary>
		/// <param name="_point">Point.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_help">Help.</param>
		public static void DrawSpawnPointObjectLine( SpawnPointObject _point, string _title = ""  )
		{
			if( _point == null )
				return;

				_point.IsPrefab = EditorTools.IsPrefab( _point.SpawnPointGameObject );

				if( _point.SpawnPointGameObject == null )
					GUI.backgroundColor = Color.red;

				string _target_title = "SpawnPoint " + EditorTools.ObjectTitleSuffix( _point.SpawnPointGameObject  );// (_point.IsValid?(_point.IsPrefab?"(prefab)":"(scene)"):"(null)");

				if( _point.AccessType == TargetAccessType.OBJECT )
					_point.SpawnPointGameObject = (GameObject)EditorGUILayout.ObjectField( _target_title , _point.SpawnPointGameObject, typeof(GameObject), true);
				else if( _point.AccessType == TargetAccessType.TAG )
					_point.SetSpawnPointGameObjectByTag( EditorGUILayout.TagField( new GUIContent( _target_title, "" ), _point.SpawnPointTag ) );
				else
					_point.SetSpawnPointGameObjectByName( Popups.TargetPopup( _target_title, "", _point.SpawnPointName, "" ) );

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				// Type Enum Popup
				int _indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_point.AccessType = (TargetAccessType)EditorGUILayout.EnumPopup( _point.AccessType, ICEEditorStyle.Popup, GUILayout.Width( 50 ) ); 
				EditorGUI.indentLevel = _indent;


				if( _point.SpawnPointGameObject != null )
				{
					ICEEditorLayout.ButtonDisplayObject( _point.SpawnPointGameObject.transform.position );
					ICEEditorLayout.ButtonSelectObject( _point.SpawnPointGameObject );
				}
		}

		/// <summary>
		/// Draws the target selectors object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_selectors">Selectors.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_min_distance">Minimum distance.</param>
		/// <param name="_max_distance">Max distance.</param>
		public static void DrawTargetSelectorsObject( ICECreatureControl _control, TargetObject _target, SelectionCriteriaObject _criteria, TargetType _type, float _min_distance , float _max_distance  )
		{
			if( _criteria == null ) //|| _control == null )
				return;

			// TARGET SELECTION CRITERIAS

			string _help = Info.TARGET_SELECTION_CRITERIA ;
			if( _type == TargetType.HOME )
				_help = Info.TARGET_SELECTION_CRITERIA + "\n\n" + Info.TARGET_SELECTION_CRITERIA_HOME;

			Color _color_true = Color.green;
			Color _color_false = (Application.isPlaying?Color.red:ICEEditorLayout.DefaultBackgroundColor);
			Color _color_unchecked = (Application.isPlaying?Color.yellow:ICEEditorLayout.DefaultBackgroundColor);

			ICEEditorLayout.BeginHorizontal();


			EditorGUI.BeginDisabledGroup( _type == TargetType.HOME && _criteria.Enabled == false );
				_criteria.Foldout = ICEEditorLayout.Foldout( _criteria.Foldout, "Target Selection Criteria (" + _target.EntityType.ToString() + " Object)" , "", false );


				//_criteria.UseTargetUpdateInterval = ICEEditorLayout.CheckButtonSmall( "TUI", "Target Update Interval", _criteria.UseTargetUpdateInterval, ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor );


			EditorGUI.EndDisabledGroup();



			if( _type == TargetType.HOME )
			{
				_criteria.Enabled = ICEEditorLayout.EnableButton( "The HOME target should always have the lowest priority, but if you want you could adapt these settings also.", _criteria.Enabled );

				if( _criteria.Enabled == false )
				{
					_criteria.Priority = 0;
					_criteria.UseSelectionRange = false;
					_criteria.UseSelectionAngle = false;
					_criteria.SelectionRange = 0;
					_criteria.SelectionAngle = 0;
					_criteria.UseAdvanced = false;
				}
			}
			else
				_criteria.Enabled = true;

			ICEEditorLayout.EndHorizontal( _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( ( _type == TargetType.HOME ? EditorHeaderType.FOLDOUT_ENABLED_BOLD:EditorHeaderType.FOLDOUT_BOLD ), _criteria ) )
				return;

				// PRIORITY BEGIN
				ICEEditorLayout.BeginHorizontal();				
				if( _criteria.CanUseDefaultPriority )
					_criteria.Priority = (int)ICEEditorLayout.AutoSlider( "Priority","", _criteria.Priority, 1,0, 100, ref _criteria.UseDefaultPriority, _criteria.DefaultPriority );
				else
					_criteria.Priority = (int)ICEEditorLayout.DefaultSlider( "Priority", "Priority to select this target!", _criteria.Priority, 1, 0, 100, _criteria.GetDefaultPriorityByType( _type ) );


				ICEEditorLayout.EndHorizontal( Info.TARGET_SELECTION_CRITERIA_PRIORITY );
				// PRIORITY END



				// OPTIONS BEGIN
				ICEEditorLayout.BeginHorizontal();	

					GUILayout.FlexibleSpace();

					GUILayout.Space( 5 );
					_criteria.UseSelectionRange = ICEEditorLayout.CheckButtonSmall( "SR", "Selection Range", _criteria.UseSelectionRange, ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor );
					_criteria.UseSelectionAngle = ICEEditorLayout.CheckButtonSmall( "SA", "Selection Angle", _criteria.UseSelectionAngle , ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor  );
					//_criteria.UseSelectionCount = ICEEditorLayout.CheckButtonSmall( "SC", "Selection Count", _criteria.UseSelectionCount , ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor  );
					
					GUILayout.Space( 5 );
					_criteria.DelayTimer.Enabled = ICEEditorLayout.CheckButtonSmall( "DT", "Delay Timer", _criteria.DelayTimer.Enabled , ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor  );
					_criteria.RetainingTimer.Enabled = ICEEditorLayout.CheckButtonSmall( "RT", "Retaining Timer", _criteria.RetainingTimer.Enabled , ICEEditorLayout.SelectionOptionGroup1Color, ICEEditorLayout.SelectionOptionGroup1SelectedColor  );

					GUILayout.Space( 5 );
					EditorGUI.BeginDisabledGroup( _control == null || _control.Creature.Status.Sensoria.Enabled == false );

						if( _control != null && _control.Creature.Status.Sensoria.Enabled )
						{
							_criteria.UseFieldOfView = ICEEditorLayout.CheckButtonSmall( "FOV", "Field Of View - the target must be in the visual field of the creature", _criteria.UseFieldOfView , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
							_criteria.UseVisibilityCheck = ICEEditorLayout.CheckButtonSmall( "VC", "Visibility Check - the target must be visible for the creature", _criteria.UseVisibilityCheck , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
							_criteria.UseAudibleCheck = ICEEditorLayout.CheckButtonSmall( "AC", "Audible Check - the target must be hearable for the creature", _criteria.UseAudibleCheck , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
							_criteria.UseOdourCheck = ICEEditorLayout.CheckButtonSmall( "OC", "Odour Check - the target must be smellable for the creature", _criteria.UseOdourCheck , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
							_criteria.UseTactileCheck = ICEEditorLayout.CheckButtonSmall( "TC", "Tactile Check - the target must be palpable for the creature", _criteria.UseTactileCheck , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
							_criteria.UseFlavourCheck = ICEEditorLayout.CheckButtonSmall( "FC", "Flavour Check - the target must be tasty for the creature", _criteria.UseFlavourCheck , ICEEditorLayout.SelectionOptionGroup2Color, ICEEditorLayout.SelectionOptionGroup2SelectedColor );
						}
						else
						{
							ICEEditorLayout.CheckButtonSmall( "FOV", "Field Of View - the target must be in the field of view", false , ICEEditorLayout.SelectionOptionGroup2Color );
							ICEEditorLayout.CheckButtonSmall( "VC", "Visibility Check - the target must be visible for the creature", false , ICEEditorLayout.SelectionOptionGroup2Color );
							ICEEditorLayout.CheckButtonSmall( "AC", "Audible Check - the target must be hearable for the creature", false , ICEEditorLayout.SelectionOptionGroup2Color );
							ICEEditorLayout.CheckButtonSmall( "OC", "Odour Check - the target must be smellable for the creature", false , ICEEditorLayout.SelectionOptionGroup2Color );
							ICEEditorLayout.CheckButtonSmall( "TC", "Tactile Check - the target must be palpable for the creature", false , ICEEditorLayout.SelectionOptionGroup2Color );
							ICEEditorLayout.CheckButtonSmall( "FC", "Flavour Check - the target must be tasty for the creature", false , ICEEditorLayout.SelectionOptionGroup2Color );

						}
					EditorGUI.EndDisabledGroup();
					GUILayout.Space( 5 );
							
				if( _target as InteractorRuleObject != null )
				{
					_criteria.UsePreselection = ICEEditorLayout.CheckButtonSmall( ( _criteria.UsePreselection ? "OVR" : "PRE" ), "Optional pre-selection criteria for faster selection of the TargetGameObjects before running the regular and advanced selection criteria", _criteria.UsePreselection, ICEEditorLayout.SelectionOptionGroup3Color, Color.yellow );
				}
				else
				{				
					_criteria.UsePreselection = ICEEditorLayout.CheckButtonSmall( "PRE", "Optional pre-selection criteria for faster selection of the TargetGameObjects before running the regular and advanced selection criteria", _criteria.UsePreselection, ICEEditorLayout.SelectionOptionGroup3Color, ICEEditorLayout.SelectionOptionGroup3SelectedColor );
				}
				_criteria.UseAdvanced = ICEEditorLayout.CheckButtonSmall( "ADV", "Advanced selection criteria for a more detailed fine-tuning of the target selection criteria.", _criteria.UseAdvanced, ICEEditorLayout.SelectionOptionGroup3Color, ICEEditorLayout.SelectionOptionGroup3SelectedColor );

				ICEEditorLayout.EndHorizontal( Info.TARGET_SELECTION_CRITERIA_OPTIONS );
				// OPTIONS END

			// PRESELECTION CRITERIA BEGIN
			if( _criteria.Preselection.Enabled )
			{
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );	

				if( _target as InteractorRuleObject != null )
				{
					string _text = "By default additional interactor rules are using the same TargetGameObject as the primary interactor rule but if the " +
						"Preselection is enabled the rule will select an own target object of the same type according to the specified pre-selection criteria." +
						"In this way, the creature can react simultaneously to multiple targets of the same type, but please note that the performance cost " +
						"increases with each use of this feature.";
					ICEEditorLayout.MiniLabelLeft( _text );

					EditorGUILayout.Separator();
				}

				// TARGET UPDATE INTERVAL BEGIN
				ICEEditorLayout.BeginHorizontal();	
					EditorGUI.BeginDisabledGroup( _criteria.Preselection.UseTargetRefreshInterval == false || _criteria.Preselection.RefreshInvalidTargetsOnly == true );
						string _interval_title = "";
						if( _criteria.Preselection.RefreshInvalidTargetsOnly )
							_interval_title = "Refresh Invalid Targets Only (INV)";
						else if( ! _criteria.Preselection.UseTargetRefreshInterval )
							_interval_title = "Refresh Interval (OFF)";
						else
							_interval_title += "Refresh Interval (" + ICEEditorLayout.RoundDisplay( _criteria.Preselection.TargetUpdateTime, 0.01f ).ToString() + "/" + ICEEditorLayout.RoundDisplay( _criteria.Preselection.TargetUpdateTimer, 0.01f ).ToString()  + " secs.)";
						ICEEditorLayout.MinMaxDefaultSlider( _interval_title , "Refresh interval in which the creature looks for matching GameObjects", ref _criteria.Preselection.TargetUpdateTimeMin, ref _criteria.Preselection.TargetUpdateTimeMax, 0, ref _criteria.Preselection.TargetUpdateTimeMaximum, 1, 3, Init.DECIMAL_PRECISION_TIMER, 40 );
					EditorGUI.EndDisabledGroup();
					EditorGUI.BeginDisabledGroup( _criteria.Preselection.UseTargetRefreshInterval == false );
						_criteria.Preselection.RefreshInvalidTargetsOnly = ICEEditorLayout.CheckButtonSmall( "INV", "Refreshes invalid TargetGameObjects only", _criteria.Preselection.RefreshInvalidTargetsOnly );
						_criteria.Preselection.UseTargetRecheck = ICEEditorLayout.CheckButtonSmall( "CHK", "Rechecks a preselected goal to ensure that the conditions are still true", _criteria.Preselection.UseTargetRecheck );
					EditorGUI.EndDisabledGroup();
					_criteria.Preselection.UseTargetRefreshInterval = ICEEditorLayout.EnableButton( "Target Refresh Interval", _criteria.Preselection.UseTargetRefreshInterval );
				ICEEditorLayout.EndHorizontal( Info.TARGET_PRESELECTION_REFRESH_INTERVAL  );
				// TARGET UPDATE INTERVAL BEGIN

				// TARGET ACTIVE COUNTERPARTS LIMIT BEGIN
				string _range_title = "Active Counterparts Limit";
				if( _criteria.Preselection.ActiveCounterpartsLimit < 0 )
					_range_title += " (infinite)";
				else
					_range_title += " (limited)";

				ICEEditorLayout.BeginHorizontal();					
					EditorGUI.BeginDisabledGroup( _criteria.Preselection.UseActiveCounterpartsLimit == false );
						_criteria.Preselection.ActiveCounterpartsLimit = ICEEditorLayout.DefaultSlider( _range_title , "Takes into account how often the specified target is currently selected by other objects and prevents a further selection whenever the limit was reached", _criteria.Preselection.ActiveCounterpartsLimit, 0, 100, 0, "" );
					EditorGUI.EndDisabledGroup();
					_criteria.Preselection.UseActiveCounterpartsLimit = ICEEditorLayout.EnableButton( "Use Active Counterparts Limit", _criteria.Preselection.UseActiveCounterpartsLimit );
				ICEEditorLayout.EndHorizontal( Info.TARGET_PRESELECTION_ACTIVE_COUNTERPARTS_LIMIT );
				// TARGET ACTIVE COUNTERPARTS LIMIT END

				_criteria.Preselection.PreferActiveCounterparts = ICEEditorLayout.LabelEnableButton( "Prefer Active Counterparts", "Prefers objects that have been selected our creature as their own active target.", _criteria.Preselection.PreferActiveCounterparts, Info.TARGET_PREFER_ACTIVE_COUNTERPARTS );

				_criteria.Preselection.UseChildObjects = ICEEditorLayout.LabelEnableButton( "Use Child Objects", "Allows the selection of own child objects.", _criteria.Preselection.UseChildObjects, Info.TARGET_PRESELECTION_USE_CHILD_OBJECTS );
				//_target.UseTargetAttributes = ICEEditorLayout.LabelEnableButton( "Allow Target Attributes", "Allows the selection of own child objects", _target.UseTargetAttributes, Info.TARGET_CHILD_SELECTION );
			
				_criteria.Preselection.UseAllAvailableObjects = ICEEditorLayout.LabelEnableButton( "Use All Available Objects", "Considers all available objects of the given type during the final target selection process", _criteria.Preselection.UseAllAvailableObjects, Info.TARGET_PRESELECTION_USE_ALL_AVAILABLE_OBJECTS );


				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );	
			}
			// PRESELECTION CRITERIA END

				// SELECTION RANGE BEGIN
				if( _criteria.UseSelectionRange )
				{
					string _range_title = "Selection Range";
					if( _criteria.SelectionRange == 0 )
						_range_title += " (infinite)";
					else
						_range_title += " (limited)";

					ICEEditorLayout.BeginHorizontal();					
					GUI.backgroundColor = (_criteria.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_criteria.IsValid?_color_true:_color_false) );
					_criteria.SelectionRange = ICEEditorLayout.DefaultSlider( _range_title , "If the selection range greater than 0 this target will only select if the creature is within the specified range", _criteria.SelectionRange, Init.SELECTION_RANGE_STEP, _min_distance, _max_distance, _criteria.GetDefaultRangeByType( _type ) );
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
					ICEEditorLayout.EndHorizontal( Info.TARGET_SELECTION_CRITERIA_RANGE );
				}
				// SELECTION RANGE END

				/*
					if( _selectors.UseVisibilityCheck )
					{
						EditorGUI.indentLevel++;
							_selectors.VisibilityCheckVerticalOffset = ICEEditorLayout.DefaultSlider( "Vertical Offset" , "", _selectors.VisibilityCheckVerticalOffset, 0.01f, -2, 2, 0, "" );
						EditorGUI.indentLevel--;
					}*/

				// SELECTION ANGLE BEGIN
				if( _criteria.UseSelectionAngle )
				{
					string _angle_title = "Selection Angle";
					if( _criteria.SelectionAngle == 0 || _criteria.SelectionAngle == 180 )
						_angle_title += " (full-circle)";
					else if( _criteria.SelectionAngle == 90 )
						_angle_title += " (semi-circle)";
					else if( _criteria.SelectionAngle == 45 )
						_angle_title += " (quadrant)";
					else
						_angle_title += " (sector)";


					ICEEditorLayout.BeginHorizontal();	
						GUI.backgroundColor = (_criteria.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_criteria.IsValid?_color_true:_color_false) );
							_criteria.SelectionAngle = ICEEditorLayout.BasicSlider( _angle_title , "", _criteria.SelectionAngle * 2, Init.SELECTION_ANGLE_STEP, Init.SELECTION_ANGLE_MIN, Init.SELECTION_ANGLE_MAX ) / 2;
						GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

						_criteria.SelectionAngle = ICEEditorLayout.ButtonOption( "90", _criteria.SelectionAngle, 45 );
						_criteria.SelectionAngle = ICEEditorLayout.ButtonOption( "180", _criteria.SelectionAngle, 90 );
						_criteria.SelectionAngle = ICEEditorLayout.ButtonOption( "270", _criteria.SelectionAngle, 135 );
						_criteria.SelectionAngle = ICEEditorLayout.ButtonOption( "360", _criteria.SelectionAngle, 180 );
						_criteria.SelectionAngle = ICEEditorLayout.ButtonDefault( _criteria.SelectionAngle, 0 );

					ICEEditorLayout.EndHorizontal( Info.TARGET_SELECTION_CRITERIA_ANGLE );
				}
				// SELECTION ANGLE END
			/*
				// SELECTION COUNT BEGIN
				if( _criteria.UseSelectionCount )
				{
					string _range_title = "Selection Count";
					if( _criteria.SelectionCount < 0 )
						_range_title += " (infinite)";
					else
						_range_title += " (limited)";

					ICEEditorLayout.BeginHorizontal();					
					GUI.backgroundColor = (_criteria.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_criteria.IsValid?_color_true:_color_false) );
					_criteria.SelectionCount = ICEEditorLayout.DefaultSlider( _range_title , "", _criteria.SelectionCount, -1, 100, -1 );
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
					ICEEditorLayout.EndHorizontal( Info.TARGET_SELECTION_CRITERIA_RANGE );
				}
				// SELECTION COUNT END
			*/

				// DELAY TIMER BEGIN
				if( _criteria.DelayTimer.Enabled )
				{
					GUI.backgroundColor = (_criteria.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_criteria.IsValid?_color_true:_color_false) );
					CreatureEditorLayout.DrawSelectionTimer( "Delay Timer", "", _criteria.DelayTimer, Info.TARGET_SELECTION_CRITERIA_DELAY_TIMER );
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
				}
				// DELAY TIMER END

				// RETAINING TIMER BEGIN
				if( _criteria.RetainingTimer.Enabled )
				{
					GUI.backgroundColor = (_criteria.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_criteria.IsValid?_color_true:_color_false) );
					CreatureEditorLayout.DrawSelectionTimer( "Retaining Timer", "", _criteria.RetainingTimer, Info.TARGET_SELECTION_CRITERIA_RETAINING_TIMER );
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
				}
				// RETAINING TIMER END


				if( _criteria.UseAdvanced )
				{
					if( _criteria.ConditionGroups.Count == 0 ) 
						_criteria.ConditionGroups.Add( new SelectionConditionGroupObject( ConditionalOperatorType.AND ) );

					// SELECTOR GROUPS BEGIN
					foreach( SelectionConditionGroupObject _group in _criteria.ConditionGroups )
					{
						var indent = EditorGUI.indentLevel;
						EditorGUI.indentLevel = 0;

						int _offset = 45;
						int _condition_size = 65;

						if( _group.Conditions.Count == 0 )
							_group.Conditions.Add( new SelectionConditionObject( ConditionalOperatorType.AND, SelectionExpressionType.OwnFitness ) );
						
						if( _group.Conditions.Count > 1 )
						{
							
							// ADD CONDITION LINE BEGIN
							ICEEditorLayout.BeginHorizontal();

								GUILayout.Space( _offset );

								Color _color = (_group.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_group.IsValid?_color_true:_color_false) );

								GUI.backgroundColor = _color;

								_group.InitialOperatorType = (ConditionalOperatorType)EditorGUILayout.EnumPopup( _group.InitialOperatorType , GUILayout.Width( 65 ) );

								string _group_title = "Condition Group (" + _group.Conditions.Count + ") ";
								string _group_hint = _group_title;
								string _expressions = "";
								foreach( SelectionConditionObject _c in _group.Conditions )
								{
									_expressions += ( _expressions != "" ? "|" : "" ) + _c.ExpressionType.ToString();
									_group_hint += "\n" + _c.ConditionToString() + " " + _c.ExpressionType.ToString()  + " " + _c.OperatorToString(); // TODO get the value + " " + _c.;
								}

								_group_title += _expressions;								

								int _max_length = 80;
								if( _group_title.Length > _max_length )
									_group_title = _group_title.Substring( 0, _max_length ) + " ...";						
								
								_group.Foldout = ICEEditorLayout.CheckButton( _group_title, _group_hint, _group.Foldout, _color, _color, ICEEditorStyle.ButtonFlex );	
								_group.UseUpdateLastPosition = ICEEditorLayout.CheckButtonMini( "U", "Updates the last known position is the condition is valid", _group.UseUpdateLastPosition );
								_group.Enabled = ICEEditorLayout.CheckButtonMini( "E", "Enables/disables this condition group", _group.Enabled );
										
								if( ICEEditorLayout.ListUpDownButtonsMini<SelectionConditionGroupObject>( _criteria.ConditionGroups, _criteria.ConditionGroups.IndexOf( _group ) ) )
									return;
								
								if( ICEEditorLayout.ListDeleteButton<SelectionConditionGroupObject>( _criteria.ConditionGroups, _group ) )
									return;

							ICEEditorLayout.EndHorizontal( " " );
							// ADD CONDITION LINE END
						}

						if( _group.Foldout || _group.Conditions.Count == 1 )
						{
							if( _group.Conditions.Count < 2 )
								_group.Enabled = true;
							
							EditorGUI.BeginDisabledGroup( _group.Enabled == false );
							for( int i = 0 ; i < _group.Conditions.Count ; i++ )
							{
								SelectionConditionObject _condition = _group.Conditions[i];

								EditorGUI.BeginDisabledGroup( _condition.Enabled == false );

					


								// CONDITION LINE BEGIN
								ICEEditorLayout.BeginHorizontal();


								GUI.backgroundColor = (_condition.Status == SelectionStatus.UNCHECKED?_color_unchecked:(_condition.IsValid?_color_true:_color_false) );

								if( i > 0 ) 
								{
									_offset = 60;
									_condition_size = 50;

									GUILayout.Space( _offset );
									_condition.ConditionType = (ConditionalOperatorType)EditorGUILayout.EnumPopup(  _condition.ConditionType , GUILayout.Width( _condition_size ) );
								}
								else if( _group.Conditions.Count == 1 )
								{
									GUILayout.Space( _offset );
									_condition.ConditionType = (ConditionalOperatorType)EditorGUILayout.EnumPopup(  _condition.ConditionType , GUILayout.Width( _condition_size ) );

								}
								else
									GUILayout.Space( _offset + _condition_size + 4 );						

								

								//EditorGUILayout.LabelField("",GUILayout.Width( _offset ) );

								if( _condition.ShowOwner == true || 
									_condition.ShowTarget == true || 
									_condition.ShowActiveTarget == true || 
									_condition.ShowLastTarget == true ||
									_condition.ShowEnvironment == true || 
									_condition.ShowSystem == true )
								{
									_condition.ShowOwner = ( SelectionTools.ExpressionContains( "Own", _condition.ExpressionType ) ? true : _condition.ShowOwner );
									_condition.ShowTarget = ( SelectionTools.ExpressionContains( "Target", _condition.ExpressionType ) ? true : _condition.ShowTarget );
									_condition.ShowActiveTarget = ( SelectionTools.ExpressionContains( "ActiveTarget", _condition.ExpressionType ) ? true : _condition.ShowActiveTarget );
									_condition.ShowLastTarget = ( SelectionTools.ExpressionContains( "LastTarget", _condition.ExpressionType ) ? true : _condition.ShowLastTarget );
									_condition.ShowEnvironment = ( SelectionTools.ExpressionContains( "Environment", _condition.ExpressionType ) ? true : _condition.ShowEnvironment );
									_condition.ShowSystem = ( SelectionTools.ExpressionContains( "System", _condition.ExpressionType ) ? true : _condition.ShowSystem );
								}

								_condition.ShowOwner = ICEEditorLayout.CheckButtonMini( "O", "Shows the available conditions for this creature", _condition.ShowOwner );
								_condition.ShowTarget = ICEEditorLayout.CheckButtonMini( "T", "Shows the potentially available conditions for the defined '" + _target.TargetName + "' target", _condition.ShowTarget );
								_condition.ShowActiveTarget = ICEEditorLayout.CheckButtonMini( "A", "Shows the potentially available conditions for the active runtime target", _condition.ShowActiveTarget );	
								_condition.ShowLastTarget = ICEEditorLayout.CheckButtonMini( "L", "Shows the potentially available conditions for the last runtime target", _condition.ShowLastTarget );
								_condition.ShowEnvironment = ICEEditorLayout.CheckButtonMini( "E", "Shows the environmental conditions", _condition.ShowEnvironment );
								_condition.ShowSystem = ICEEditorLayout.CheckButtonMini( "S", "Shows the system conditions", _condition.ShowSystem );

								if( Application.isPlaying )
									_condition.ExpressionType = (SelectionExpressionType)EditorGUILayout.EnumPopup( "", _condition.ExpressionType, GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 220 ) );
								else
									_condition.ExpressionType = SelectionTools.StrToType( ICEEditorLayout.DrawListPopupExt( "", _condition.ExpressionTypeKey, _condition.GetSelectionExpressions( _target, _condition.ShowOwner, _condition.ShowTarget, _condition.ShowActiveTarget , _condition.ShowLastTarget, _condition.ShowEnvironment, _condition.ShowSystem ), GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 220 ) ) );
								
								if( SelectionTools.NeedLogicalOperator( _condition.ExpressionType ) )	
									_condition.Operator = Popups.LogicalOperatorPopup(  _condition.Operator, GUILayout.Width( 45 ) );
								else
									_condition.Operator = Popups.OperatorPopup(  _condition.Operator, GUILayout.Width( 45 ) );



								// CREATURE BEHAVIOUR
								if( _condition.ExpressionType == SelectionExpressionType.OwnBehaviour )								
									_condition.StringValue = Popups.BehaviourPopup( _control, _condition.StringValue );

								// CREATURE BEHAVIOUR
								else if( _condition.ExpressionType == SelectionExpressionType.ActiveTargetName ||
									_condition.ExpressionType == SelectionExpressionType.LastTargetName ||
									_condition.ExpressionType == SelectionExpressionType.TargetName ||
									_condition.ExpressionType == SelectionExpressionType.ActiveTargetParentName ||
									_condition.ExpressionType == SelectionExpressionType.TargetParentName )								
									_condition.StringValue = Popups.TargetPopup( _condition.StringValue );

								// ZONES
								else if( _condition.ExpressionType == SelectionExpressionType.OwnZoneName ||
									_condition.ExpressionType == SelectionExpressionType.TargetZoneName )								
									_condition.StringValue = Popups.ZonePopup( _condition.StringValue );

								// CREATURE POSITION
								else if( _condition.ExpressionType == SelectionExpressionType.OwnerPosition )								
									_condition.PositionType = (TargetSelectorPositionType)EditorGUILayout.EnumPopup( _condition.PositionType );

								// STRING VALUES
								else if( SelectionTools.IsStringValue( _condition.ExpressionType ) )
								{
									_condition.StringValue = EditorGUILayout.TextField( _condition.StringValue );
								}

								// ENUM VALUES
								else if( SelectionTools.IsEnumValue( _condition.ExpressionType ) )
								{
									if( _condition.ExpressionType == SelectionExpressionType.TargetEntityType ||
									_condition.ExpressionType == SelectionExpressionType.ActiveTargetEntityType ||
									_condition.ExpressionType == SelectionExpressionType.LastTargetEntityType  )
										_condition.IntegerValue = (int)(EntityClassType)EditorGUILayout.EnumPopup( (EntityClassType)_condition.IntegerValue );
									else if( _condition.ExpressionType == SelectionExpressionType.EnvironmentWeather )
										_condition.IntegerValue = (int)(WeatherType)EditorGUILayout.EnumPopup( (WeatherType)_condition.IntegerValue );
								

									else if( _condition.ExpressionType == SelectionExpressionType.OwnGenderType )
										_condition.IntegerValue = (int)(CreatureGenderType)EditorGUILayout.EnumPopup( (CreatureGenderType)_condition.IntegerValue );
									else if( _condition.ExpressionType == SelectionExpressionType.OwnTrophicLevel )
										_condition.IntegerValue = (int)(TrophicLevelType)EditorGUILayout.EnumPopup( (TrophicLevelType)_condition.IntegerValue );


			
									else if( _condition.ExpressionType == SelectionExpressionType.CreatureGenderType )
										_condition.IntegerValue = (int)(CreatureGenderType)EditorGUILayout.EnumPopup( (CreatureGenderType)_condition.IntegerValue );
									else if( _condition.ExpressionType == SelectionExpressionType.CreatureTrophicLevel )
										_condition.IntegerValue = (int)(TrophicLevelType)EditorGUILayout.EnumPopup( (TrophicLevelType)_condition.IntegerValue );

									else if( _condition.ExpressionType == SelectionExpressionType.OwnOdour ||
										_condition.ExpressionType == SelectionExpressionType.ActiveTargetOdour ||
										_condition.ExpressionType == SelectionExpressionType.TargetOdour )
										_condition.IntegerValue = (int)(OdourType)EditorGUILayout.EnumPopup( (OdourType)_condition.IntegerValue );
							}

								// ENUM VALUES
								else if( SelectionTools.IsBooleanValue( _condition.ExpressionType ) )
								{
									BooleanValueType _boolean_value = (BooleanValueType)EditorGUILayout.EnumPopup( (_condition.BooleanValue?BooleanValueType.TRUE:BooleanValueType.FALSE) );

									_condition.BooleanValue = (_boolean_value == BooleanValueType.TRUE?true:false);
								}

								// KEYCODE VALUES
								else if( SelectionTools.IsKeyCodeValue( _condition.ExpressionType ) )
								{
									_condition.KeyCodeValue = (KeyCode)EditorGUILayout.EnumPopup( _condition.KeyCodeValue );
								}

								// AXIS VALUES
								else if( SelectionTools.IsAxisValue( _condition.ExpressionType ) )
								{
									_condition.AxisValue = WorldPopups.AxisPopup( _condition.AxisValue );
								}

								// UI TOGGLE VALUES
								else if( SelectionTools.IsUIToggleValue( _condition.ExpressionType ) )
								{
									UnityEngine.UI.Toggle _toggle = _condition.GetUIToggle();

									 _toggle = (UnityEngine.UI.Toggle)EditorGUILayout.ObjectField( _toggle, typeof(UnityEngine.UI.Toggle), true );

									_condition.ToggleValue = ( _toggle != null ? _toggle.name : "" );
								}

								// UI BUTTON VALUES
								else if( SelectionTools.IsUIButtonValue( _condition.ExpressionType ) )
								{
									UnityEngine.UI.Button _button = _condition.GetUIButton();

									_button = (UnityEngine.UI.Button)EditorGUILayout.ObjectField( _button, typeof(UnityEngine.UI.Button), true );

									_condition.ButtonValue = ( _button != null ? _button.name : "" );
								}


								// LOGICAL VALUES
								else if( SelectionTools.NeedLogicalOperator( _condition.ExpressionType ) )	
								{
									if( SelectionTools.IsDynamicValue( _condition.ExpressionType ) )
									{
										if( _condition.UseDynamicValue )
										{									
											if( Application.isPlaying )
												_condition.ExpressionValue = (SelectionExpressionType)EditorGUILayout.EnumPopup( "", _condition.ExpressionValue, GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 220 ) );
											else if( _condition.ShowAll )
												_condition.ExpressionValue = SelectionTools.StrToType( ICEEditorLayout.DrawListPopupExt( "", _condition.ExpressionValueKey, _condition.GetValueSelectionExpressionsByType( _condition.ExpressionType ), GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 220 ) ) );
											else
												_condition.ExpressionValue = SelectionTools.StrToType( ICEEditorLayout.DrawListPopupExt( "", _condition.ExpressionValueKey, _condition.GetValueSelectionExpressions( _condition.ExpressionType ), GUILayout.MinWidth( 120 ), GUILayout.MaxWidth( 220 ) ) );
										}
										else
											_condition.FloatValue = EditorGUILayout.FloatField( _condition.FloatValue );

										GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;	
										_condition.UseDynamicValue = ICEEditorLayout.CheckButtonSmall( "DYN", "Use dynamic value", _condition.UseDynamicValue );

										EditorGUI.BeginDisabledGroup( _condition.UseDynamicValue == false );
											_condition.ShowAll = ICEEditorLayout.CheckButtonMini( "A", "Shows all available values.", _condition.ShowAll );
										EditorGUI.EndDisabledGroup();

									}
									else
									{
										_condition.UseDynamicValue = false;
										_condition.FloatValue = EditorGUILayout.FloatField( _condition.FloatValue );
									}
								}

								// OBJECT VALUES
								else if( SelectionTools.IsObjectValue( _condition.ExpressionType ) )
								{
									//TODO : Object selection
									_condition.ExpressionValue = (SelectionExpressionType)EditorGUILayout.EnumPopup( _condition.ExpressionValue  );
								}

								

								GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

								_condition.UseUpdateLastPosition = ICEEditorLayout.CheckButtonMini( "U", "Updates the last known position is the condition is valid", _condition.UseUpdateLastPosition );

								EditorGUI.EndDisabledGroup();

								_condition.Enabled = ICEEditorLayout.CheckButtonMini( "E", "Enables/disables this condition", _condition.Enabled );
	
								if( _group.Conditions.Count > 1 )
								{
									if( ICEEditorLayout.ListUpDownButtonsMini<SelectionConditionObject>( _group.Conditions, i ) )
										return;
								}
								else
								{
									if( ICEEditorLayout.ListUpDownButtonsMini<SelectionConditionGroupObject>( _criteria.ConditionGroups, _criteria.ConditionGroups.IndexOf( _group ) ) )
										return;
								}
								
								if( ICEEditorLayout.ListDeleteButton<SelectionConditionObject>( _group.Conditions, _condition ) )
								{
									if( _group.Conditions.Count == 0 )
										_criteria.ConditionGroups.Remove( _group );
									return;
								}

								ICEEditorLayout.EndHorizontal( Info.GetTargetSelectionExpressionTypeHint( _condition.ExpressionType ) );
								// CONDITION LINE END



								

							}
							EditorGUI.EndDisabledGroup();

							EditorGUI.indentLevel = indent;
							EditorGUI.indentLevel++;
								ICEEditorLayout.DrawListAddLine<SelectionConditionObject>( _group.Conditions, new SelectionConditionObject( ConditionalOperatorType.AND ), false, "Add Condition" );
							EditorGUI.indentLevel--;
						}
						else
							EditorGUI.indentLevel = indent;
					}

					ICEEditorLayout.DrawListAddClearLine<SelectionConditionGroupObject>( _criteria.ConditionGroups, new SelectionConditionGroupObject( ConditionalOperatorType.AND ), true, "Add Condition Group" );

					if( _criteria.ConditionGroups.Count == 0 ) 
						_criteria.UseAdvanced = false; 
				}

			EndObjectContent();
			// CONTENT END

		}

		public static void DrawRangedWeaponMuzzleFlashObject( RangedWeaponMuzzleFlashObject _flash, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _flash == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Muzzle Flash";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RANGED_WEAPON;


			DrawObjectHeader( _flash, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _flash ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				_flash.MuzzleFlash = (GameObject)EditorGUILayout.ObjectField("Muzzle Flash", _flash.MuzzleFlash, typeof(GameObject), true );

			EditorGUI.BeginDisabledGroup( _flash.MuzzleFlash == null );

			if( _flash.MuzzleFlash == null )
				ICEEditorLayout.CheckButtonMiddle( "SHOW", "", false );
			else
				_flash.MuzzleFlash.SetActive( ICEEditorLayout.CheckButtonMiddle( "SHOW", "", _flash.MuzzleFlash.activeInHierarchy ) );


			EditorGUI.EndDisabledGroup();


			ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_MUZZLE_FLASH );
			_flash.MuzzleFlashScale = ICEEditorLayout.MaxDefaultSlider( "Muzzle Flash Scale", "", _flash.MuzzleFlashScale, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _flash.MuzzleFlashScaleMaximum, 10, Info.RANGED_WEAPON_MUZZLE_FLASH_SCALE );


			EditorGUILayout.Separator();
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawFireObject( ICEWorldBehaviour _control, FireObject _fire, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _fire == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Fire";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.FIRE;


			DrawObjectHeader( _fire, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _fire ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				_fire.FireLight = (Light)EditorGUILayout.ObjectField( "Light", _fire.FireLight, typeof(Light), true );
				if( ICEEditorLayout.AutoButton( "Assigns light in children." ) )
				{
					_fire.FireLight = _control.gameObject.GetComponentInChildren<Light>();
				}
			ICEEditorLayout.EndHorizontal( Info.FIRE_LIGHT );

			ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.BasicMinMaxSlider( "Intensity", "Random min/max intensity range.", ref _fire.IntensityMin, ref _fire.IntensityMax, 0, 8, Init.DECIMAL_PRECISION, 40 );

				if( ICEEditorLayout.ButtonDefault( _fire.IntensityMin + _fire.IntensityMax , 1.5f + 3f ) == 4.5f )
				{
					_fire.IntensityMin = 1.5f;
					_fire.IntensityMax = 3f;
				}

			ICEEditorLayout.EndHorizontal( Info.FIRE_INTENSITY );

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawRangedWeaponObject( ICEWorldBehaviour _control, RangedWeaponObject _weapon, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _weapon == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Weapon";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RANGED_WEAPON;


			DrawObjectHeader( _weapon, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _weapon ) )
				return;

				DrawRangedWeaponAmmunitionObject( _weapon.Ammunition, EditorHeaderType.FOLDOUT_ENABLED  );
				DrawTimerObject( _weapon.Automatic, EditorHeaderType.FOLDOUT_ENABLED, Info.BEHAVIOUR_AUDIO, "Automatic" );
				DrawDirectAudioPlayerObject( _weapon.LaunchSound, EditorHeaderType.FOLDOUT_ENABLED, Info.BEHAVIOUR_AUDIO, "Sound" );
				DrawRangedWeaponMuzzleFlashObject( _weapon.MuzzleFlash, EditorHeaderType.FOLDOUT_ENABLED, Info.BEHAVIOUR_AUDIO );
				DrawRangedWeaponShellObject( _weapon.Shell, EditorHeaderType.FOLDOUT_ENABLED  );
				DrawDirectEffectObject( _control, _weapon.Effect, EditorHeaderType.FOLDOUT_ENABLED );
				//DrawRangedWeaponRecoilObject( _weapon.Recoil, EditorHeaderType.FOLDOUT_ENABLED  );

				EditorGUILayout.Separator();
	

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawRangedWeaponRecoilObject( RangedWeaponRecoilObject _recoil, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _recoil == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Recoil";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RANGED_WEAPON_AMMO;


			DrawObjectHeader( _recoil, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _recoil ) )
				return;



			EndObjectContent();
			// CONTENT END
		}


		public static void DrawRangedWeaponShellObject( RangedWeaponShellObject _shell, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _shell == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Shell";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RANGED_WEAPON_SHELL;


			DrawObjectHeader( _shell, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _shell ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				_shell.ReferenceShell = (GameObject)EditorGUILayout.ObjectField( "Reference Shell", _shell.ReferenceShell, typeof(GameObject), true );
			ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_SHELL_REFERENCE );

			ICEEditorLayout.BeginHorizontal();
				_shell.SpawnPoint = (GameObject)EditorGUILayout.ObjectField( "Spawn Point", _shell.SpawnPoint, typeof(GameObject), true );
			ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_SHELL_SPAWNPOINT );

			_shell.EjectionSpeed = ICEEditorLayout.MaxDefaultSlider( "Ejection Speed", "", _shell.EjectionSpeed, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _shell.EjectionSpeedMaximum, 10, Info.RANGED_WEAPON_AMMO_DAMAGE_VALUE );



			EndObjectContent();
			// CONTENT END
		}


		public static void DrawRangedWeaponAmmunitionObject( RangedWeaponAmmunitionObject _ammo, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _ammo == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Ammunition";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.RANGED_WEAPON_AMMO;


			DrawObjectHeader( _ammo, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _ammo ) )
				return;

				_ammo.Type  = (RangedWeaponAmmunitionType)ICEEditorLayout.EnumPopup( "Type", "", _ammo.Type, Info.RANGED_WEAPON_AMMO_TYPE );

				EditorGUI.indentLevel++;
					if( _ammo.Type == RangedWeaponAmmunitionType.Simulated )
					{
						_ammo.ImpactType = (DamageTransferType)ICEEditorLayout.EnumPopup( "Damage Type", "", _ammo.ImpactType, Info.IMPACT_TYPE );

						if( _ammo.ImpactType != DamageTransferType.Direct )
						{
							ICEEditorLayout.BeginHorizontal();
								_ammo.MethodName = ICEEditorLayout.Text( "Damage Method", "", _ammo.MethodName );
								_ammo.MethodName = ICEEditorLayout.ButtonDefault( _ammo.MethodName, "ApplyDamage" );
							ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_AMMO_DAMAGE_METHOD );
						}
			
						_ammo.MethodDamage = ICEEditorLayout.MaxDefaultSlider( "Damage Value", "", _ammo.MethodDamage, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _ammo.MethodDamageMaximum, 10, Info.RANGED_WEAPON_AMMO_DAMAGE_VALUE );
						
						_ammo.ForceType = (DamageForceType)ICEEditorLayout.EnumPopup( "Force Type", "", _ammo.ForceType, Info.IMPACT_FORCE_TYPE );
						if( _ammo.ForceType != DamageForceType.None )
						{
							ICEEditorLayout.MinMaxRandomDefaultSlider( "Energie (min/max)", "", ref _ammo.ForceMin, ref _ammo.ForceMax, 0, ref _ammo.ForceMaximum, 100, 100, Init.DECIMAL_PRECISION_VELOCITY, 40, Info.IMPACT_FORCE_ENERGY );

							if( _ammo.ForceType == DamageForceType.Explosion )
							{
								_ammo.ExplosionRadius = ICEEditorLayout.MaxDefaultSlider( "Explosion Radius", "", _ammo.ExplosionRadius, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _ammo.ExplosionRadiusMaximum, 5,  Info.IMPACT_EXPLOSION_RADIUS ); 
							}
						}
					}
					else if( _ammo.Type == RangedWeaponAmmunitionType.Projectile || _ammo.Type == RangedWeaponAmmunitionType.BallisticProjectile )
					{
						ICEEditorLayout.BeginHorizontal();
							_ammo.Projectile = (GameObject)EditorGUILayout.ObjectField( "Projectile", _ammo.Projectile, typeof(GameObject), true );
						ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_AMMO_PROJECTILE );

						_ammo.ProjectileScale = ICEEditorLayout.DefaultSlider( "Scale", "", _ammo.ProjectileScale, Init.DECIMAL_PRECISION_DISTANCES, 0, 100, 1, Info.RANGED_WEAPON_AMMO_PROJECTILE_SCALE );

						ICEEditorLayout.BeginHorizontal();
						_ammo.ProjectileSpawnPoint = (GameObject)EditorGUILayout.ObjectField( "Muzzle (Spawn Point)", _ammo.ProjectileSpawnPoint, typeof(GameObject), true );
						ICEEditorLayout.EndHorizontal( Info.RANGED_WEAPON_AMMO_PROJECTILE_SPAWN_POINT );

					
						if( _ammo.Type == RangedWeaponAmmunitionType.Projectile )
						{
						}
						else if( _ammo.Type == RangedWeaponAmmunitionType.BallisticProjectile )
						{
							_ammo.ProjectileMuzzleVelocity = ICEEditorLayout.MaxDefaultSlider( "Muzzle Velocity", "", _ammo.ProjectileMuzzleVelocity, Init.DECIMAL_PRECISION_VELOCITY, 0, ref _ammo.ProjectileMuzzleVelocityMaximum, 1000, Info.RANGED_WEAPON_AMMO_PROJECTILE_MUZZLE_VELOVITY );
						}
					}
		
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the audio object.
		/// </summary>
		/// <param name="_audio">Audio.</param>
		/// <param name="_help">Help.</param>
		public static void DrawTurretObject( TurretObject _turret, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _turret == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Turret";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.TURRET;

	
			DrawObjectHeader( _turret, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _turret ) )
				return;

				_turret.ScanRange = ICEEditorLayout.MaxDefaultSlider( "Scan Range", "", _turret.ScanRange, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _turret.ScanRangeMaximum, 15, Info.TURRET_SCAN_RANGE );
				_turret.ScanInterval = ICEEditorLayout.MaxDefaultSlider( "Scan Interval", "", _turret.ScanInterval, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _turret.ScanIntervalMaximum, 0.5f, Info.TURRET_SCAN_RANGE );

				WorldObjectEditor.DrawLayerObject( _turret.Layers, EditorHeaderType.FOLDOUT_ENABLED, "", "Target Layer" );

				EditorGUILayout.Separator();

			ICEEditorLayout.BeginHorizontal();
				_turret.PivotType = (MountingPivotType)ICEEditorLayout.EnumPopup( "Mounting Pivot Type", "", _turret.PivotType );
				_turret.UseParkPosition = ICEEditorLayout.CheckButtonMiddle( "PARK", "Returns the turret to its initial stand-by position", _turret.UseParkPosition );
			ICEEditorLayout.EndHorizontal( Info.TURRET_SCAN_RANGE );

				EditorGUI.indentLevel++;
					if( _turret.PivotType == MountingPivotType.PivotalPoint )
					{
						ICEEditorLayout.BeginHorizontal();
							_turret.PivotPoint = (Transform)EditorGUILayout.ObjectField("Pivot Point", _turret.PivotPoint, typeof(Transform), true );
							if( ICEEditorLayout.ButtonMiddle( "ALIGN", "" ) )
								_turret.PivotPoint.rotation = Quaternion.identity;
				
						ICEEditorLayout.EndHorizontal( Info.TURRET_PIVOT_POINT );
					}
					else if( _turret.PivotType == MountingPivotType.SeperateAxes )
					{
						ICEEditorLayout.BeginHorizontal();
							_turret.PivotYawAxis = (Transform)EditorGUILayout.ObjectField("Yaw Axis (y)", _turret.PivotYawAxis, typeof(Transform), true );
							if( ICEEditorLayout.ButtonMiddle( "ALIGN", "" ) )
								_turret.PivotYawAxis.rotation = Quaternion.identity;

						ICEEditorLayout.EndHorizontal( Info.TURRET_PIVOT_AXIS_YAW );

						ICEEditorLayout.BeginHorizontal();
							_turret.PivotPitchAxis = (Transform)EditorGUILayout.ObjectField("Pitch Axis (x)", _turret.PivotPitchAxis, typeof(Transform), true );
							if( ICEEditorLayout.ButtonMiddle( "ALIGN", "" ) )
								_turret.PivotPitchAxis.rotation = Quaternion.identity;
				
						ICEEditorLayout.EndHorizontal( Info.TURRET_PIVOT_AXIS_PITCH );
					}

					_turret.RotationSpeed = ICEEditorLayout.MaxDefaultSlider( "Rotation Speed", "", _turret.RotationSpeed, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _turret.RotationSpeedMaximum, 2, Info.TURRET_SCAN_RANGE );

					_turret.MaxAngularDeviation = ICEEditorLayout.DefaultSlider( "Angular Deviation (max)", "", _turret.MaxAngularDeviation, Init.DECIMAL_PRECISION_DISTANCES, 0, 36, 2, Info.TURRET_ANGULAR_DEVIATION );

					_turret.VerticalTargetAdjustment = ICEEditorLayout.DefaultSlider( "Vertical Target Adjustment", "", _turret.VerticalTargetAdjustment, Init.DECIMAL_PRECISION_DISTANCES, -5, 5, 0, Info.TURRET_VERTICAL_ADJUSTMENT );



				EditorGUI.indentLevel--;

				EditorGUILayout.Separator();
				DrawDirectAudioPlayerObject( _turret.MovingSound, EditorHeaderType.FOLDOUT_ENABLED, Info.BEHAVIOUR_AUDIO, "Sound" );

			EndObjectContent();
			// CONTENT END
		}


		public static void DrawMemoryObject( ICECreatureControl _control, MemoryObject _memory, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _memory == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Memory";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.STATUS_MEMORY;

			DrawObjectHeader( _memory, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _memory ) )
				return;

			float _max_capacity = _memory.SpatialMemory.CapacityMax;
			_memory.SpatialMemory.Capacity = (int)ICEEditorLayout.MaxDefaultSlider( "Spatial Memory", "", _memory.SpatialMemory.Capacity , 1, 0, ref _max_capacity, 0, Info.STATUS_MEMORY_SPATIAL );
			_memory.SpatialMemory.CapacityMax = (int)_max_capacity;

			_max_capacity = _memory.ShortTermMemory.CapacityMax;
			_memory.ShortTermMemory.Capacity = (int)ICEEditorLayout.MaxDefaultSlider( "Short-Term Memory", "", _memory.ShortTermMemory.Capacity , 1, 0, ref _max_capacity, 0, Info.STATUS_MEMORY_SHORT );
			_memory.ShortTermMemory.CapacityMax = (int)_max_capacity;

			_max_capacity = _memory.LongTermMemory.CapacityMax;
			_memory.LongTermMemory.Capacity = (int)ICEEditorLayout.MaxDefaultSlider( "Long-Term Memory", "", _memory.LongTermMemory.Capacity , 1, 0, ref _max_capacity, 0, Info.STATUS_MEMORY_LONG );
			_memory.LongTermMemory.CapacityMax = (int)_max_capacity;

			EndObjectContent();
			// CONTENT END
		}

		public static void DrawSensoriaObject( ICECreatureControl _control, SensoriaObject _sensoria, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _sensoria == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Sensoria";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.SENSORIA;

			DrawObjectHeader( _sensoria, _type, _title, _hint, _help );

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _sensoria ) )
				return;

				// FOV BEGIN
				_sensoria.FieldOfView = ICEEditorLayout.DefaultSlider( "Field Of View", "Field Of View", _sensoria.FieldOfView * 2, 0.05f, 0, 360, 160, Info.SENSORIA_FOV ) / 2;
				if( _sensoria.FieldOfView > 0 )
				{
					EditorGUI.indentLevel++;

						_sensoria.VisualRange = ICEEditorLayout.MaxDefaultSlider( "Visual Range", "Max. Sighting Distance", _sensoria.VisualRange, 0.25f, 0, ref _sensoria.VisualRangeMaximum, 0, Info.SENSORIA_FOV_VISUAL_RANGE );

						ICEEditorLayout.BeginHorizontal();
						if( _sensoria.UseDynamicVisualSensorPosition )
							_sensoria.VisualSensorName = ICEEditorLayout.TransformPopup( "Visual Sensor Position", "", _sensoria.VisualSensorName, _control.transform, false, "");
						else
							_sensoria.VisualSensorOffset = EditorGUILayout.Vector3Field( new GUIContent( "Visual Sensor Position", "Position of the visual sensor e.g. eyes" ), _sensoria.VisualSensorOffset );

						_sensoria.UseDynamicVisualSensorPosition = ICEEditorLayout.CheckButtonSmall("DYN", "", _sensoria.UseDynamicVisualSensorPosition );

						ICEEditorLayout.EndHorizontal( Info.SENSORIA_FOV_VISUAL_OFFSET  );

						EditorGUI.indentLevel++;		
							_sensoria.VisualSensorHorizontalOffset = ICEEditorLayout.DefaultSlider( "Horizontal Offset", "", _sensoria.VisualSensorHorizontalOffset, 0.01f, 0, 5, 0.15f, Info.SENSORIA_FOV_VISUAL_HORIZONTAL_OFFSET );								
						EditorGUI.indentLevel--;

						ICEEditorLayout.BeginHorizontal();
							EditorGUI.BeginDisabledGroup( _sensoria.UseSphereCast == false );
								_sensoria.SphereCastRadius = ICEEditorLayout.MaxDefaultSlider( "Use Spherical Cast (Radius)", "SphereCast Radius", _sensoria.SphereCastRadius, Init.DECIMAL_PRECISION, 0, ref _sensoria.SphereCastRadiusMaximum, 0.3f );

							EditorGUI.EndDisabledGroup();
							_sensoria.UseSphereCast = ICEEditorLayout.EnableButton( _sensoria.UseSphereCast );
						ICEEditorLayout.EndHorizontal( Info.SENSORIA_USE_SPHERECAST  );

					EditorGUI.indentLevel--;	
					EditorGUILayout.Separator();
				}
				// FOV END

				ICEEditorLayout.Label( "Sensorial Attributes", false );
				//_control.Display.FoldoutStatusSensory = ICEEditorLayout.Foldout( _control.Display.FoldoutStatusSensory, "Sensory Indicators", Info.STATUS_SENSES, false );				
				//if( _control.Display.FoldoutStatusSensory )
				{
					EditorGUI.indentLevel++;
						_sensoria.DefaultSenseVisual  = (int)ICEEditorLayout.DefaultSlider( "Visual (%)", "Optical sense", _sensoria.DefaultSenseVisual,0.025f, 0,100,100, Info.SENSORIA_ATTRIBUTES_VISUAL );
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.UseAging )
								_sensoria.SenseVisualAgeMultiplier = ICEEditorLayout.Slider("Age Multiplier (-)","", _sensoria.SenseVisualAgeMultiplier, 0.025f,0,1);
							_sensoria.SenseVisualFitnessMultiplier = ICEEditorLayout.Slider("Fitness Multiplier (-)","", _sensoria.SenseVisualFitnessMultiplier, 0.025f,0,1);
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

						_sensoria.DefaultSenseAuditory = (int)ICEEditorLayout.DefaultSlider( "Auditory (%)", "Hearing sense", _sensoria.DefaultSenseAuditory,0.025f, 0,100,100, Info.SENSORIA_ATTRIBUTES_AUDITORY );
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.UseAging )
								_sensoria.SenseAuditoryAgeMultiplier = ICEEditorLayout.Slider("Age Multiplier (-)","", _sensoria.SenseAuditoryAgeMultiplier, 0.025f,0,1);
							_sensoria.SenseAuditoryFitnessMultiplier = ICEEditorLayout.Slider("Fitness Multiplier (-)","", _sensoria.SenseAuditoryFitnessMultiplier, 0.025f,0,1);
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

						_sensoria.DefaultSenseOlfactory  = (int)ICEEditorLayout.DefaultSlider( "Olfactory (%)", "Olfactory sense", _sensoria.DefaultSenseOlfactory,0.025f, 0,100,100, Info.SENSORIA_ATTRIBUTES_OLFACTORY );
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.UseAging )
								_sensoria.SenseOlfactoryAgeMultiplier = ICEEditorLayout.Slider("Age Multiplier (-)","", _sensoria.SenseOlfactoryAgeMultiplier, 0.025f,0,1);
							_sensoria.SenseOlfactoryFitnessMultiplier = ICEEditorLayout.Slider("Fitness Multiplier (-)","", _sensoria.SenseOlfactoryFitnessMultiplier, 0.025f,0,1);
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

						_sensoria.DefaultSenseGustatory  = (int)ICEEditorLayout.DefaultSlider( "Gustatory (%)", "Taste sense", _sensoria.DefaultSenseGustatory,0.025f, 0,100,100, Info.SENSORIA_ATTRIBUTES_GUSTATORY );
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.UseAging )
								_sensoria.SenseGustatoryAgeMultiplier = ICEEditorLayout.Slider("Age Multiplier (-)","", _sensoria.SenseGustatoryAgeMultiplier, 0.025f,0,1);
							_sensoria.SenseGustatoryFitnessMultiplier = ICEEditorLayout.Slider("Fitness Multiplier (-)","", _sensoria.SenseGustatoryFitnessMultiplier, 0.025f,0,1);
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

						_sensoria.DefaultSenseTactile = (int)ICEEditorLayout.DefaultSlider( "Tactile (%)", "Touch sense", _sensoria.DefaultSenseTactile,0.025f, 0,100,100, Info.SENSORIA_ATTRIBUTES_TACTILE );
						EditorGUI.indentLevel++;
							if( _control.Creature.Status.UseAging )
								_sensoria.SenseTouchAgeMultiplier = ICEEditorLayout.Slider("Age Multiplier (-)","", _sensoria.SenseTouchAgeMultiplier, 0.025f,0,1);
							_sensoria.SenseTouchFitnessMultiplier = ICEEditorLayout.Slider("Fitness Multiplier (-)","", _sensoria.SenseTouchFitnessMultiplier, 0.025f,0,1);
						EditorGUI.indentLevel--;
						EditorGUILayout.Separator();

					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}

			
			EndObjectContent();
			// CONTENT END
		}


		/// <summary>
		/// Draws the flashlight object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_flashlight">Flashlight.</param>
		/// <param name="_component">Component.</param>
		/// <param name="_help">Help.</param>
		public static void DrawFlashlightObject( ICEWorldBehaviour _component, FlashlightObject _flashlight, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _flashlight == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Flashlight";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.FLASHLIGHT;

			DrawObjectHeader( _flashlight, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _flashlight ) )
				return;



			ICEEditorLayout.BeginHorizontal();


				_flashlight.Init( _component );
	
				_flashlight.Light = (Light)EditorGUILayout.ObjectField( "Light Bulb", _flashlight.Light, typeof(Light), true );

				EditorGUI.BeginDisabledGroup( _flashlight.Light != null );
				if( ICEEditorLayout.AddButton( "Adds a light as bulb" ) )
				{
					GameObject _bulb = new GameObject( "LightBulb" );
					if( _bulb != null )
					{
						_bulb.transform.SetParent( _component.transform, false );
						_bulb.transform.position = _component.transform.position;
						_bulb.transform.rotation = _component.transform.rotation;
						_flashlight.Light = _bulb.AddComponent<Light>();
						if( _flashlight.Light!= null )
						{
							_flashlight.Light.type = LightType.Spot;
							_flashlight.Light.intensity = 1;
							_flashlight.Light.color = Color.yellow;
						}
					}
				}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _flashlight.Light == null );
					_flashlight.IsActive = ICEEditorLayout.CheckButtonMiddle( "ACTIVE", "", _flashlight.IsActive ); //ICEEditorLayout.Toggle( "Flashlight Is Active", "", _flashlight.IsActive, "" );
				EditorGUI.EndDisabledGroup();
			ICEEditorLayout.EndHorizontal();

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _flashlight.UseActivateByLightIntensity == false );
					_flashlight.LightIntensityLimit = ICEEditorLayout.DefaultSlider( "Use At Night (Intensity Limit)", "", _flashlight.LightIntensityLimit, Init.DECIMAL_PRECISION, 0, 8, 1f, "" );
				EditorGUI.EndDisabledGroup();
				_flashlight.UseActivateByLightIntensity = ICEEditorLayout.CheckButtonMiddle( "AUTO", "", _flashlight.UseActivateByLightIntensity );
			ICEEditorLayout.EndHorizontal();

			EndObjectContent();
			// CONTENT END
		}

		/// <summary>
		/// Draws the laser object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_laser">Laser.</param>
		/// <param name="_component">Component.</param>
		/// <param name="_help">Help.</param>
		public static void DrawLaserObject( ICEWorldBehaviour _component, LaserObject _laser, EditorHeaderType _type, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _laser == null || _component == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Laser";

			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";

			if( string.IsNullOrEmpty( _help ) )
				_help = Info.LASER;

			DrawObjectHeader( _laser, _type,_title, _hint, _help );			

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _laser ) )
				return;

				_laser.IsActive = ICEEditorLayout.Toggle( "Laser Is Active", "", _laser.IsActive, "" );
				_laser.Width = ICEEditorLayout.MaxDefaultSlider( "Laser Width", "", _laser.Width, 0.0010f, 0, ref _laser.WidthMaximum, 0.01f, "" );
				_laser.Noise = ICEEditorLayout.MaxDefaultSlider( "Laser Noise", "", _laser.Noise, 0.001f, 0, ref _laser.NoiseMaximum, 0, "" );
				_laser.LengthMax = ICEEditorLayout.MaxDefaultSlider( "Laser Length Max.", "", _laser.LengthMax, 0.001f, 0, ref _laser.LengthMaximum, 50, "" );
				_laser.Offset = ICEEditorLayout.OffsetGroup( "Laser Offset", _laser.Offset, "" );
				_laser.StartColor = ICEEditorLayout.DefaultColor( "Laser Start Color", "", _laser.StartColor , Color.red, "" );
				_laser.EndColor = ICEEditorLayout.DefaultColor( "Laser End Color", "", _laser.EndColor , new Color(1,0,0,0.5f), "" );
				_laser.EndEffect = (ParticleSystem)EditorGUILayout.ObjectField( "Laser End Effect", _laser.EndEffect, typeof(ParticleSystem), false );

				if( _component.transform.GetComponent<LineRenderer>() == null )
				{
					if( ICEEditorLayout.ButtonExtraLarge( "Add LineRenderer", "" ) )
						_component.transform.gameObject.AddComponent<LineRenderer>();
				}
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawPlayerObject( GameObject _owner, PlayerObject _player, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _player == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Player";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.PLAYER_PLAYER;

			//LIFESPAN BEGIN
			DrawObjectHeader( _player, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _player ) )
				return;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _player.UseDeathCamera == false );
					_player.DeathCameraReference = (GameObject)EditorGUILayout.ObjectField( "Death Camera", _player.DeathCameraReference, typeof(GameObject), false );
				EditorGUI.EndDisabledGroup();
				_player.UseDeathCamera = ICEEditorLayout.EnableButton( _player.UseDeathCamera );
			ICEEditorLayout.EndHorizontal( Info.PLAYER_PLAYER_DEATHCAM );

			_player.UseMousePositionToAim = ICEEditorLayout.Toggle( "Use Mouse Position To Aim", "", _player.UseMousePositionToAim, Info.PLAYER_PLAYER_USEMOUSEPOSITIONTOAIM );

			EndObjectContent();
			// CONTENT END
		}


		public static void DrawPlayerInputEventsObject( ICEWorldBehaviour _component, InputEventsObject _events, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _events == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Events";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.PLAYER_EVENTS;

			//LIFESPAN BEGIN
			DrawObjectHeader( _events, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _events ) )
				return;

			foreach( InputEventObject _event in _events.Events )
			{
				
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _event.Enabled == false );
						if( _event.UseKeyInput )
							_event.KeyInput = (KeyCode)ICEEditorLayout.EnumPopup( "Input Key" , "", _event.KeyInput ); 
						else
							_event.AxisInput = Popups.AxisPopup( _event.AxisInput, "Input Axis" , "" ); 

						_event.UseKeyInput = ICEEditorLayout.CheckButtonSmall( "KEY", "", _event.UseKeyInput ); 
						_event.UseTimer = ICEEditorLayout.CheckButtonSmall( "TIMER", "", _event.UseTimer ); 
					EditorGUI.EndDisabledGroup();

					if( ICEEditorLayout.ListUpDownButtonsMini<InputEventObject>( _events.Events, _events.Events.IndexOf(_event) ) )
						return;

					if( ICEEditorLayout.ListDeleteButtonMini<InputEventObject>( _events.Events, _event, "Delete Event" ) )
						return;
				
					_event.Enabled = ICEEditorLayout.EnableButton( _event.Enabled ); 
				ICEEditorLayout.EndHorizontal( Info.PLAYER_EVENT_INPUT );

				EditorGUI.BeginDisabledGroup( _event.Enabled == false );
					DrawBehaviourEvent( _component, _event.Event, "", "" );
				if( _event.UseTimer )
					WorldObjectEditor.DrawTimerObject( _event.Timer );
				EditorGUI.EndDisabledGroup();
			}

			ICEEditorLayout.DrawListAddLine<InputEventObject>( _events.Events , new InputEventObject(), "Add Event" );




			EndObjectContent();
			// CONTENT END
		}



		public static void DrawPlayerInventoryObject( GameObject _owner, PlayerInventoryObject _inventory, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			DrawInventoryObject( _owner, _inventory, _type );
		}

		/// <summary>
		/// Draws the inventory object.
		/// </summary>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_inventory">Inventory.</param>
		/// <param name="_owner">Owner.</param>
		/// <param name="_help">Help.</param>
		public static void DrawInventoryObject( GameObject _owner, InventoryObject _inventory, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _inventory == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Inventory";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.INVENTORY;

			//LIFESPAN BEGIN
			DrawObjectHeader( _inventory, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _inventory ) )
				return;

				DrawInventoryObjectContent( _owner, _inventory );

			EndObjectContent();
			// CONTENT END
		}



		public static bool DrawInventorySlotObject( GameObject _owner, InventoryObject _inventory, int _index )
		{
			if( _inventory == null || _index < 0 || _index >= _inventory.Slots.Count )
				return false;
			
			InventorySlotObject _slot = _inventory.Slots[_index];

			if( ! Application.isPlaying )
				_slot.Init( _owner );

			ICEEditorLayout.BeginHorizontal();

				_slot.MountPointName = ICEEditorLayout.TransformPopup( "Slot #" + (_index) + (_slot.IsNotional?" [notional]":" [mounted]"), "", _slot.MountPointName, _owner.transform, true );
			/*
					EditorGUI.BeginDisabledGroup( _slot.IsNotional == true );

						if( _slot.IsEquipped )
						{
							if( ICEEditorLayout.Button( "FREE", "", ICEEditorStyle.ButtonMiddle ) )
								_slot.Remove( _owner );
						}
						else
						{
							if( ICEEditorLayout.Button( "EQUIP", "", ICEEditorStyle.ButtonMiddle ) )
								_slot.Equip( _owner );
						}

					EditorGUI.EndDisabledGroup();*/

				if( ICEEditorLayout.ListDeleteButton<InventorySlotObject>( _inventory.Slots, _slot ) )
					return true;
			
				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<InventorySlotObject>( _inventory.Slots, _index ) )
					return true;


			ICEEditorLayout.EndHorizontal( Info.INVENTORY_SLOT );

			EditorGUI.indentLevel++;

			ICEEditorLayout.BeginHorizontal();

				if( _slot.ItemObject )
					GUI.backgroundColor = Color.green;
				else if( _slot.ItemReferenceObject )
					GUI.backgroundColor = Color.yellow;
				else
					GUI.backgroundColor = Color.blue;

				_slot.ItemName = Popups.ItemPopup( "Item", "Assigned item for slot #" + ( _index) + "", _slot.ItemName );

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				EditorGUI.BeginDisabledGroup( _slot.ItemObject == null );
				//ICEEditorLayout.ButtonShowTransform( "DTL", "Show details", _item, ICEEditorStyle.CMDButtonDouble );
				ICEEditorLayout.ButtonSelectObject( _slot.ItemObject , ICEEditorStyle.CMDButtonDouble  );
				EditorGUI.EndDisabledGroup();
			/*
				EditorGUI.BeginDisabledGroup( _slot.IsNotional == true || _slot.IsEquipped == false );								
				if( ICEEditorLayout.ButtonMiddle( "DROP", "" ) )
					_slot.DropItem();										
				EditorGUI.EndDisabledGroup();
				*/

				if( ! Application.isPlaying && _slot.Amount == 0 && _slot.ItemName != "" )
					_slot.IsExclusive = true;
			
				EditorGUI.BeginDisabledGroup( _slot.ItemName == "" );	
					_slot.IsExclusive = ICEEditorLayout.CheckButtonSmall( "EXCL", "Reserved slot for the specified item type", _slot.IsExclusive );
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _inventory.UseDetachOnDie == true );
					if( _inventory.UseDetachOnDie )
						ICEEditorLayout.CheckButtonSmall( "DOD", "Detach item on die", true );
					else
						_slot.UseDetachOnDie = ICEEditorLayout.CheckButtonSmall( "DOD", "Detach item on die", _slot.UseDetachOnDie );
				EditorGUI.EndDisabledGroup();

				if( ICEEditorLayout.ResetButtonSmall( "Resets this slot" ) )
					_slot.ItemName = "";

				ICEEditorLayout.EndHorizontal( Info.INVENTORY_SLOT_ITEM );

				if( _slot.UseDetachOnDie || _inventory.UseDetachOnDie )
				{
					EditorGUI.indentLevel++;
					_slot.DropRange = ICEEditorLayout.DefaultSlider( "Drop Range", "Radius around the creature the detached item will be droped", _slot.DropRange, 0.05f, 0, 25, _inventory.DefaultDropRange, Info.INVENTORY_SLOT_ITEM_DROP_RANGE );
					EditorGUI.indentLevel--;
				}

			ICEEditorLayout.BeginHorizontal();

				int _max = _slot.MaxAmount;
				_slot.Amount = ICEEditorLayout.InitialMaxDefaultSlider( "Amount", "Current amount and maximum capacity for slot #" + ( _index), _slot.Amount, 0, ref _max, ref _slot.InitialAmount, 0, "" );
				_slot.MaxAmount = _max;


				if( ! Application.isPlaying )
					_slot.InitialAmount = _slot.Amount;

				EditorGUI.BeginDisabledGroup( _slot.ItemName == "" );	
				_slot.UseRandomAmount = ICEEditorLayout.CheckButtonSmall( "RND", "", _slot.UseRandomAmount );
				EditorGUI.EndDisabledGroup();

			ICEEditorLayout.EndHorizontal( Info.INVENTORY_SLOT_AMOUNT );

			EditorGUI.indentLevel--;

			return false;
		}


		/// <summary>
		/// Draws the content of the inventory object.
		/// </summary>
		/// <param name="_owner">Owner.</param>
		/// <param name="_inventory">Inventory.</param>
		public static void DrawInventoryObjectContent( GameObject _owner, InventoryObject _inventory )
		{
			if( _inventory == null )
				return;					

			// HEAD BEGIN
			ICEEditorLayout.BeginHorizontal();

				float _max_slots = _inventory.MaxSlots;
				int _slots = (int)ICEEditorLayout.BasicMaxSlider( "Slots (" + _inventory.Slots.Count + ")", "", _inventory.AvailableSlots, 1,0, ref _max_slots, "" );
				_inventory.MaxSlots = (int)_max_slots;

				while( _slots < _inventory.AvailableSlots )
					_inventory.Slots.RemoveAt(_inventory.Slots.Count-1);
				while( _slots > _inventory.AvailableSlots )
					_inventory.Slots.Add( new InventorySlotObject() );

				_inventory.UseStaticSlots = ICEEditorLayout.CheckButtonSmall( "LOCK", "Locks the inventory structure during the runtime.", _inventory.UseStaticSlots );
				_inventory.UseDetachOnDie = ICEEditorLayout.CheckButtonSmall( "DOD", "Detach all items on destroy.", _inventory.UseDetachOnDie );

	
				if( ICEEditorLayout.ButtonSmall( "SCAN", "" ) )
				{
					ICECreatureItem[] _items = _owner.GetComponentsInChildren<ICECreatureItem>();
					foreach( ICECreatureItem _item in _items )
					{
						InventorySlotObject _slot = _inventory.GetSlotByItemName( _item.name );
						if( _slot == null || ! _item.transform.IsChildOf( _slot.MountPointTransform ) )
							_inventory.Insert( _item.gameObject );
					}
				}

				if( ICEEditorLayout.ListClearButtonSmall<InventorySlotObject>( _inventory.Slots ) )
					return;

				//_inventory.IgnoreInventoryOwner = ICEEditorLayout.ButtonCheck( "", "", _inventory.IgnoreInventoryOwner, ICEEditorStyle.CMDButtonDouble );

			ICEEditorLayout.EndHorizontal( Info.INVENTORY_SLOTS );
			// HEAD END

			// LIST BEGIN
			EditorGUI.indentLevel++;

			for( int _i = 0;_i < _inventory.Slots.Count ; _i++ )
				if( DrawInventorySlotObject( _owner, _inventory, _i ) )
					return;
			
			EditorGUI.indentLevel--;
			// LIST END

			// OPTIONS BEGIN
			ICEEditorLayout.Label( "Options", false );
			EditorGUI.indentLevel++;
			_inventory.DefaultDropRange = ICEEditorLayout.DefaultSlider( "Default Drop Range", "", _inventory.DefaultDropRange, 0.25f,0, 25, 0, Info.INVENTORY_DROP_RANGE );
			EditorGUI.indentLevel--;
			// OPTIONS END
		}

		/// <summary>
		/// Draws the inventory action object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_inventory">Inventory.</param>
		/// <param name="_help">Help.</param>
		public static void DrawInventoryActionObject( ICECreatureControl _control, InventoryActionObject _inventory, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _inventory == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Inventory";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_INVENTORY;
			
			DrawObjectHeader( _inventory, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _inventory ) )
				return;

				foreach( InventoryActionDataObject _action in _inventory.ActionList )
				{
					string _help_type = Info.BEHAVIOUR_INVENTORY_TYPE;
					if( _action.Type == InventoryActionType.CollectActiveItem )
						_help_type = Info.BEHAVIOUR_INVENTORY_TYPE_COLLECT;
					else if( _action.Type == InventoryActionType.DropItem )
						_help_type = Info.BEHAVIOUR_INVENTORY_TYPE_DISTRIBUTE;
					else if( _action.Type == InventoryActionType.ChangeParent )
						_help_type = Info.BEHAVIOUR_INVENTORY_TYPE_EQUIP;

					ICEEditorLayout.BeginHorizontal();
						EditorGUI.BeginDisabledGroup( _action.Enabled == false );
							_action.Type = (InventoryActionType)ICEEditorLayout.EnumPopup( "Action Type","Collects the given active target item object", _action.Type );
						EditorGUI.EndDisabledGroup();

						_action.Enabled = ICEEditorLayout.EnableButton( _action.Enabled );

						if( ICEEditorLayout.ListDeleteButtonMini<InventoryActionDataObject>( _inventory.ActionList, _action, "Removes this action item." ) )
							return;
					ICEEditorLayout.EndHorizontal( _help_type );
					EditorGUI.indentLevel++;

					EditorGUI.BeginDisabledGroup( _action.Enabled == false );

						DrawTimerObject( _action, EditorHeaderType.NONE );

						if( _action.Type == InventoryActionType.CollectActiveItem )
						{
						}
						else if( _action.Type == InventoryActionType.DropItem )
						{
							//_action.ItemName = Popups.InventoryItemPopup( _control, "Drop Item", "", _action.ItemName, Info.BEHAVIOUR_INVENTORY_ITEM ); 
							_action.ItemName = Popups.ItemPopup( "Item", "Select drop item", _action.ItemName );

							EditorGUI.indentLevel++;
								_action.ParentName = ICEEditorLayout.TransformPopup( "Parent", "", _action.ParentName, _control.transform, true );
								_action.DistributionOffsetType = (RandomOffsetType)ICEEditorLayout.EnumPopup( "Offset Type","", _action.DistributionOffsetType );
								EditorGUI.indentLevel++;
									if( _action.DistributionOffsetType == RandomOffsetType.EXACT )
										_action.DistributionOffset = EditorGUILayout.Vector3Field( "Offset", _action.DistributionOffset );
									else 
										_action.DistributionOffsetRadius = ICEEditorLayout.Slider( "Offset Radius", "", _action.DistributionOffsetRadius, 0.25f, 0, 100 );
								EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;

						}
						else if( _action.Type == InventoryActionType.ChangeParent )
						{
							_action.ItemName = Popups.InventoryItemPopup( _control, "Inventory Item", "", _action.ItemName, Info.BEHAVIOUR_INVENTORY_ITEM ); 
							_action.ParentName = ICEEditorLayout.TransformPopup( "Desired Parent", "", _action.ParentName, _control.transform, false );
						}
					EditorGUI.EndDisabledGroup();
					EditorGUI.indentLevel--;
				}	

				ICEEditorLayout.BeginHorizontal();
				ICEEditorLayout.Label( "Add Inventory Action", false );
				if( ICEEditorLayout.AddButton( "" ) )
					_inventory.ActionList.Add( new InventoryActionDataObject() );
				ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_INVENTORY_TYPE_DISTRIBUTE_INTERVAL );

			EndObjectContent();
			// CONTENT END
		}


		public static void DrawInfluenceObject( InfluenceObject _influences, EditorHeaderType _type, bool _advanced = true, float _max = 100, string _help = "", string _title = "", string _hint = ""  ){
			DrawInfluenceObject( null, _influences, _type, _advanced, _max, _help, _title, _hint );
		}

		/// <summary>
		/// Draws the content of the influences.
		/// </summary>
		/// <returns>The influences content.</returns>
		/// <param name="_influences">Influences.</param>
		/// <param name="_help">Help.</param>
		/// <param name="_advanced">If set to <c>true</c> advanced.</param>
		/// <param name="_max">Max.</param>
		public static void DrawInfluenceObject( BehaviourModeObject _mode, InfluenceObject _influences, EditorHeaderType _type, bool _advanced = true, float _max = 100, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _influences == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Influences";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_INFLUENCES;

			//DrawObjectHeader( _influences, _type, _title, _hint, _help );	

			if( IsHeaderRequired( _type ) )
			{
				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _influences.Enabled == false );
						DrawObjectHeaderLine( _influences, GetSimpleFoldout( _type ), _title, _hint );
					EditorGUI.EndDisabledGroup();

					if( _mode != null && _mode.Rules.Count > 1 )
					{
						if( ICEEditorLayout.Button( "SHARE", "Use this influences settings for all associated rules" ) )
						{
							foreach( BehaviourModeRuleObject _rule in _mode.Rules )
								_rule.Influences.Copy( _influences );
						}
					}

					_influences.Enabled = ICEEditorLayout.EnableButton( _influences.Enabled );
				ICEEditorLayout.EndHorizontal( _help );
			}

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _influences ) )
				return;

				BehaviourInfluenceObject _behaviour_influences = _influences as BehaviourInfluenceObject;
				if( _behaviour_influences != null )
				{
					CreatureEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _behaviour_influences.OverwritePerceptionTime == false );
							ICEEditorLayout.MinMaxDefaultSlider( "Perception Time (secs.)", "", 					
								ref _behaviour_influences.PerceptionTimeMin, 
								ref _behaviour_influences.PerceptionTimeMax,
								0,
								10,
								0.4f,
								0.6f,
								Init.DECIMAL_PRECISION_TIMER,
								40 );
						EditorGUI.EndDisabledGroup();
						_behaviour_influences.OverwritePerceptionTime = CreatureEditorLayout.CheckButtonSmall( "PT", "Overwrite Perception Time", _behaviour_influences.OverwritePerceptionTime );
					CreatureEditorLayout.EndHorizontal( Info.STATUS_PERCEPTION_TIME_OVERWRITE );

					CreatureEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _behaviour_influences.OverwriteReactionTime == false );
							ICEEditorLayout.MinMaxDefaultSlider( "Reaction Time (secs.)", "", 					
								ref _behaviour_influences.ReactionTimeMin, 
								ref _behaviour_influences.ReactionTimeMax,
								0,
								2,
								0.1f,
								0.2f,
								Init.DECIMAL_PRECISION_TIMER,
							40 );
						EditorGUI.EndDisabledGroup();
						_behaviour_influences.OverwriteReactionTime = CreatureEditorLayout.CheckButtonSmall( "RT", "Overwrite Reaction Time", _behaviour_influences.OverwriteReactionTime );
					CreatureEditorLayout.EndHorizontal( Info.STATUS_REACTION_TIME_OVERWRITE );

					EditorGUILayout.Separator();
				}

				string _advanced_help =  "";
				if( ! _advanced )
					_advanced_help += "\n\nPlease note: You can activate the Dynamic Vital Signs of the Status settings to get further options!";

				DrawTimerObject( _influences, EditorHeaderType.NONE );

				EditorGUILayout.Separator();

				if( _influences.UseDamageRange )
					CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Damage " + ( _influences.UseDamageInPercent?"(%)":""), "Damage influence in percent", ref _influences.DamageMin, ref _influences.DamageMax, ref _influences.UseDamageInPercent, ref _influences.UseDamageRange, _max, Info.STATUS_INFLUENCES_DAMAGE + _advanced_help );
				else
					_influences.Damage = CreatureEditorLayout.DrawInfluenceSlider( "Damage " + ( _influences.UseDamageInPercent?"(%)":""), "Damage influence in percent", _influences.Damage, ref _influences.UseDamageInPercent, ref _influences.UseDamageRange, _max, Info.STATUS_INFLUENCES_DAMAGE + _advanced_help );		

				if( _advanced )
				{
					if( _influences.UseStressRange )
						CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Stress " + ( _influences.UseStressInPercent?"(%)":""), "Stress influence in percent", ref _influences.StressMin, ref _influences.StressMax, ref _influences.UseStressInPercent, ref _influences.UseStressRange, _max, Info.STATUS_INFLUENCES_STRESS );
					else
						_influences.Stress = CreatureEditorLayout.DrawInfluenceSlider( "Stress " + ( _influences.UseStressInPercent?"(%)":""), "Stress influence in percent", _influences.Stress, ref _influences.UseStressInPercent, ref _influences.UseStressRange, _max,Info.STATUS_INFLUENCES_STRESS );				
					
					if( _influences.UseDebilityRange )
						CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Debility " + ( _influences.UseDebilityInPercent?"(%)":""), "Debility influence in percent", ref _influences.DebilityMin, ref _influences.DebilityMax, ref _influences.UseDebilityInPercent, ref _influences.UseDebilityRange, _max, Info.STATUS_INFLUENCES_DEBILITY );
					else
						_influences.Debility = CreatureEditorLayout.DrawInfluenceSlider( "Debility " + ( _influences.UseDebilityInPercent?"(%)":""), "Debility influence in percent", _influences.Debility, ref _influences.UseDebilityInPercent, ref _influences.UseDebilityRange, _max,Info.STATUS_INFLUENCES_DEBILITY );
					
					if( _influences.UseHungerRange )
						CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Hunger " + ( _influences.UseHungerInPercent?"(%)":""), "Hunger influence in percent", ref _influences.HungerMin, ref _influences.HungerMax, ref _influences.UseHungerInPercent, ref _influences.UseHungerRange, _max, Info.STATUS_INFLUENCES_HUNGER );
					else
						_influences.Hunger = CreatureEditorLayout.DrawInfluenceSlider( "Hunger " + ( _influences.UseHungerInPercent?"(%)":""), "Hunger influence in percent", _influences.Hunger, ref _influences.UseHungerInPercent, ref _influences.UseHungerRange, _max,Info.STATUS_INFLUENCES_HUNGER );				
					
					if( _influences.UseThirstRange )
						CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Thirst " + ( _influences.UseThirstInPercent?"(%)":""), "Thirst influence in percent", ref _influences.ThirstMin, ref _influences.ThirstMax, ref _influences.UseThirstInPercent, ref _influences.UseThirstRange, _max, Info.STATUS_INFLUENCES_THIRST );
					else
						_influences.Thirst = CreatureEditorLayout.DrawInfluenceSlider( "Thirst " + ( _influences.UseThirstInPercent?"(%)":""), "Thirst influence in percent", _influences.Thirst, ref _influences.UseThirstInPercent, ref _influences.UseThirstRange, _max,Info.STATUS_INFLUENCES_THIRST );				

					EditorGUILayout.Separator();

					_influences.Aggressivity = ICEEditorLayout.DefaultSlider( "Aggressivity (%)", "Aggressivity influence in percent", _influences.Aggressivity, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_AGGRESSIVITY );				
					_influences.Anxiety = ICEEditorLayout.DefaultSlider( "Anxiety (%)", "Anxiety influence in percent", _influences.Anxiety, 0.0025f, -100, 100,0,Info.STATUS_INFLUENCES_ANXIETY );
					_influences.Experience = ICEEditorLayout.DefaultSlider( "Experience (%)", "Experience influence in percent", _influences.Experience, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_EXPERIENCE );				
					_influences.Nosiness = ICEEditorLayout.DefaultSlider( "Nosiness (%)", "Nosiness influence in percent", _influences.Nosiness, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_NOSINESS );				

					EditorGUILayout.Separator();
				}

				
			EndObjectContent();
			// CONTENT END
		}

		public static void DrawInfluenceDataObject( InfluenceDataObject _influences, EditorHeaderType _type, bool _advanced = true, float _max = 100, string _help = "", string _title = "", string _hint = ""  )
		{
			if( _influences == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Influences";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_LINK;

			DrawObjectHeader( _influences, _type, _title, _hint, _help );	

			// CONTENT BEGIN
			if( BeginObjectContentOrReturn( _type, _influences ) )
				return;

			string _advanced_help =  "";
			if( ! _advanced )
				_advanced_help += "\n\nPlease note: You can activate the Advanced Status to get further options!";

			if( _influences.UseDamageRange )
				CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Damage " + ( _influences.UseDamageInPercent?"(%)":""), "Damage influence in percent", ref _influences.DamageMin, ref _influences.DamageMax, ref _influences.UseDamageInPercent, ref _influences.UseDamageRange, _max, Info.STATUS_INFLUENCES_DAMAGE + _advanced_help );
			else
				_influences.Damage = CreatureEditorLayout.DrawInfluenceSlider( "Damage " + ( _influences.UseDamageInPercent?"(%)":""), "Damage influence in percent", _influences.Damage, ref _influences.UseDamageInPercent, ref _influences.UseDamageRange, _max, Info.STATUS_INFLUENCES_DAMAGE + _advanced_help );		
			
			if( _advanced )
			{
				if( _influences.UseStressRange )
					CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Stress " + ( _influences.UseStressInPercent?"(%)":""), "Stress influence in percent", ref _influences.StressMin, ref _influences.StressMax, ref _influences.UseStressInPercent, ref _influences.UseStressRange, _max, Info.STATUS_INFLUENCES_STRESS );
				else
					_influences.Stress = CreatureEditorLayout.DrawInfluenceSlider( "Stress " + ( _influences.UseStressInPercent?"(%)":""), "Stress influence in percent", _influences.Stress, ref _influences.UseStressInPercent, ref _influences.UseStressRange, _max,Info.STATUS_INFLUENCES_STRESS );				

				if( _influences.UseDebilityRange )
					CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Debility " + ( _influences.UseDebilityInPercent?"(%)":""), "Debility influence in percent", ref _influences.DebilityMin, ref _influences.DebilityMax, ref _influences.UseDebilityInPercent, ref _influences.UseDebilityRange, _max, Info.STATUS_INFLUENCES_DEBILITY );
				else
					_influences.Debility = CreatureEditorLayout.DrawInfluenceSlider( "Debility " + ( _influences.UseDebilityInPercent?"(%)":""), "Debility influence in percent", _influences.Debility, ref _influences.UseDebilityInPercent, ref _influences.UseDebilityRange, _max,Info.STATUS_INFLUENCES_DEBILITY );

				if( _influences.UseHungerRange )
					CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Hunger " + ( _influences.UseHungerInPercent?"(%)":""), "Hunger influence in percent", ref _influences.HungerMin, ref _influences.HungerMax, ref _influences.UseHungerInPercent, ref _influences.UseHungerRange, _max, Info.STATUS_INFLUENCES_HUNGER );
				else
					_influences.Hunger = CreatureEditorLayout.DrawInfluenceSlider( "Hunger " + ( _influences.UseHungerInPercent?"(%)":""), "Hunger influence in percent", _influences.Hunger, ref _influences.UseHungerInPercent, ref _influences.UseHungerRange, _max,Info.STATUS_INFLUENCES_HUNGER );				

				if( _influences.UseThirstRange )
					CreatureEditorLayout.DrawInfluenceMinMaxSlider( "Thirst " + ( _influences.UseThirstInPercent?"(%)":""), "Thirst influence in percent", ref _influences.ThirstMin, ref _influences.ThirstMax, ref _influences.UseThirstInPercent, ref _influences.UseThirstRange, _max, Info.STATUS_INFLUENCES_THIRST );
				else
					_influences.Thirst = CreatureEditorLayout.DrawInfluenceSlider( "Thirst " + ( _influences.UseThirstInPercent?"(%)":""), "Thirst influence in percent", _influences.Thirst, ref _influences.UseThirstInPercent, ref _influences.UseThirstRange, _max,Info.STATUS_INFLUENCES_THIRST );				

				EditorGUILayout.Separator();

				_influences.Aggressivity = ICEEditorLayout.DefaultSlider( "Aggressivity (%)", "Aggressivity influence in percent", _influences.Aggressivity, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_AGGRESSIVITY );				
				_influences.Anxiety = ICEEditorLayout.DefaultSlider( "Anxiety (%)", "Anxiety influence in percent", _influences.Anxiety, 0.0025f, -100, 100,0,Info.STATUS_INFLUENCES_ANXIETY );
				_influences.Experience = ICEEditorLayout.DefaultSlider( "Experience (%)", "Experience influence in percent", _influences.Experience, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_EXPERIENCE );				
				_influences.Nosiness = ICEEditorLayout.DefaultSlider( "Nosiness (%)", "Nosiness influence in percent", _influences.Nosiness, 0.0025f, -_max, _max,0, Info.STATUS_INFLUENCES_NOSINESS );				

				EditorGUILayout.Separator();
			}

			EndObjectContent();
			// CONTENT END
		}


	}
}
