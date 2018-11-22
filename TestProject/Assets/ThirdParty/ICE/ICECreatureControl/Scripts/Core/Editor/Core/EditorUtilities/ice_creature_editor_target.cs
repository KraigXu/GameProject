// ##############################################################################
//
// ice_creature_editor_target.cs | TargetEditor
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
//using UnityEngine.Serialization;

using UnityEditor;
//using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;
//using System.Text.RegularExpressions;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.EditorUtilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

using ICE.World.Utilities;

namespace ICE.Creatures.EditorUtilities
{
	/// <summary>
	/// Target editor.
	/// </summary>
	public static class TargetEditor
	{	
		/// <summary>
		/// Draws the target.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_target">Target.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_help">Help.</param>
		public static void DrawMissionTarget( ICECreatureControl _control, TargetObject _target, string _title, string _help = ""  )
		{
			_target.Foldout = ICEEditorLayout.Foldout( _target.Foldout, _title, ref _target.ShowInfoText, ref _target.InfoText, _help, true );
			if( _target.Foldout )
			{
				DrawTargetObject( _control, _target, "", "" );
				DrawTargetContent( _control, _target, false );
				EditorGUILayout.Separator();	
			}
		}

		/// <summary>
		/// Draws the content of the target.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_target">Target.</param>
		public static void DrawTargetContent( ICECreatureControl _control, TargetObject _target, bool _behaviour = true, string _default_key = "" )
		{
			//EditorGUI.BeginDisabledGroup( _target.TargetGameObject == null );
			EditorGUI.indentLevel++;
				CreatureObjectEditor.DrawTargetSelectorsObject( _control, _target, _target.Selectors, _target.Type, Init.SELECTION_RANGE_MIN, Init.SELECTION_RANGE_MAX );
				DrawTargetMoveSpecification( _control.gameObject, _target );	
				DrawTargetEvents( _control, _target );
				DrawTargetInfluenceSettings( _control, _target );	
				DrawTargetGroupMessage( _control, _target );

				if( _behaviour )
					DrawTargetBehaviour( _control, _target, _default_key );
			EditorGUI.indentLevel--;
			//EditorGUI.EndDisabledGroup();
		}

		/// <summary>
		/// Draws the target object blind.
		/// </summary>
		/// <param name="_target">Target.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_help">Help.</param>
		public static void DrawTargetObjectBlind( TargetObject _target, string _title = "", string _help = "" )
		{
			ICEEditorLayout.BeginHorizontal();

			_target.SetIsPrefab( EditorTools.IsPrefab( _target.TargetGameObject ) );

			string _target_title = "Target Object " + (_target.IsValid?(_target.IsPrefab?"(prefab)":"(scene)"):"(null)");

			_target_title += ( _target.Selectors.Preselection.UseAllAvailableObjects == true && _target.Selectors.AlternateObjectsCount > 1 ? " [" +  _target.Selectors.AlternateObjectsCount + "]" : "" );

			EditorGUILayout.PrefixLabel( _target_title );

			if( _target.Active )
				GUI.backgroundColor = Color.green;
			else if( _target.IsValid == false )
				GUI.backgroundColor = Color.red;

			string _button_title = _target.TargetName + " (" + _target.TargetID + ") selected by " + _target.AccessType.ToString() + " " +
					( _target.AccessType == TargetAccessType.TAG ? ": '" + _target.TargetTag + "'" : 
					( _target.AccessType == TargetAccessType.TYPE ? ": '" +_target.TargetEntityType.ToString() + "'" : "" ) );

			ICEEditorLayout.ButtonDisplayObject(  _target.TargetGameObject, _button_title, GUI.backgroundColor, ICEEditorStyle.ButtonFlex ); 

			//GUILayout.Button( new GUIContent( _target.TargetName ,"" ) );
			//if( ICEEditorLayout.Button( _target.TargetName ,"", ICEEditorStyle.ButtonFlex ) )


			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( _target.TargetGameObject != null )
			{
				//ICEEditorLayout.ButtonShowObject(  _target.TargetGameObject.transform.position); 
				ICEEditorLayout.ButtonSelectObject( _target.TargetGameObject );
			}

			ICEEditorLayout.EndHorizontal( Info.INTERACTION_INTERACTOR_RULE_TARGET );
		}

		/// <summary>
		/// Draws the target object.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_target">Target.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_help">Help.</param>
		public static void DrawTargetObject( ICECreatureControl _control, TargetObject _target, string _title, string _help = "" )
		{
			if( _control == null || _target == null )
				return;

			_target.SetIsPrefab( EditorTools.IsPrefab( _target.TargetGameObject ) );

			// PREPARATION END

			if( ! Application.isPlaying && ! EditorTools.IsPrefab( _control.gameObject ) )
			{
				// TARGET OBJECT LINE BEGIN
				ICEEditorLayout.BeginHorizontal();

				if( _target.TargetGameObject == null )
					GUI.backgroundColor = Color.red;
				else if( _target.Active )
					GUI.backgroundColor = Color.green;

				string _target_title = "Target Object " + (_target.IsValid?(_target.IsPrefab?"(prefab)":"(scene)"):"(null)");

				if( _target.AccessType == TargetAccessType.NAME )
					_target.SetTargetByName( Popups.TargetPopup( _target_title, "", _target.TargetName, "" ) );
				else if( _target.AccessType == TargetAccessType.TAG )
					_target.SetTargetByTag( EditorGUILayout.TagField( new GUIContent( _target_title, "" ), _target.TargetTag ) );
				else if( _target.AccessType == TargetAccessType.TYPE )
					_target.SetTargetByType( (EntityClassType)EditorGUILayout.EnumPopup( new GUIContent( _target_title, "" ), _target.TargetEntityType ) );
				else
					_target.SetTargetByGameObject( (GameObject)EditorGUILayout.ObjectField( _target_title , _target.TargetGameObject, typeof(GameObject), true) );


				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				// Type Enum Popup
				int _indent = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_target.AccessType = (TargetAccessType)EditorGUILayout.EnumPopup( _target.AccessType, ICEEditorStyle.Popup, GUILayout.Width( 50 ) ); 
				EditorGUI.indentLevel = _indent;


				if( _target.TargetGameObject != null )
				{
					ICEEditorLayout.ButtonDisplayObject( _target.TargetGameObject.transform.position );
					ICEEditorLayout.ButtonSelectObject( _target.TargetGameObject, ICEEditorStyle.CMDButtonDouble );

					//_target.UseChildObjects = ICEEditorLayout.CheckButtonSmall( "CHD", "Allows the selection of own child objects", _target.UseChildObjects, ICEEditorLayout.SelectionOptionGroup3Color, ICEEditorLayout.SelectionOptionGroup3SelectedColor );

					//if( _target.TargetGameObject.GetComponent<ICECreatureTargetAttribute>() != null )
					{
						EditorGUI.BeginDisabledGroup( _target.ReadTargetAttributeData() == null );
							GUI.backgroundColor = Color.green;
							if(  ICEEditorLayout.ButtonSmall( "TAV", "Use target attribute values" ) )
								_target.SetTargetDefaultValues( _target.ReadTargetAttributeData() );
							GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
						EditorGUI.EndDisabledGroup();
					}
				}
				else 
				{
					if(  ICEEditorLayout.AutoButton( "Creates an empty GameObject as target with default settings" ) )
						WizardEditor.WizardRandomTarget( _control, _target );
				}

				ICEEditorLayout.EndHorizontal( Info.TARGET_OBJECT );
				// TARGET OBJECT LINE END

			}
			else
			{
				DrawTargetObjectBlind( _target );
			}
		}

		/// <summary>
		/// Draws the target group message.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_target">Target.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_help">Help.</param>
		public static void DrawTargetGroupMessage( ICECreatureControl _control, TargetObject _target, string _title = "", string _hint = "", string _help = "" )
		{
			if( _control == null || _target == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Creature Messages";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.TARGET_INFLUENCES;
			
			ICEEditorLayout.BeginHorizontal();

				WorldObjectEditor.DrawObjectHeaderLine( _target.GroupMessage, EditorHeaderType.FOLDOUT_ENABLED, _title, _hint );

			ICEEditorLayout.EndHorizontal( _help );




			// CONTENT BEGIN
			if( WorldObjectEditor.BeginObjectContentOrReturn( EditorHeaderType.FOLDOUT_ENABLED, _target.GroupMessage ) )
				return;

				_target.GroupMessage.Type = (BroadcastMessageType)ICEEditorLayout.EnumPopup( "Group Message", "", _target.GroupMessage.Type, Info.TARGET_GROUP_MESSAGE );


				if( _target.GroupMessage.Type == BroadcastMessageType.COMMAND )
				{
					EditorGUI.indentLevel++;
					_target.GroupMessage.Command = ICEEditorLayout.Text( "Command", "Command", _target.GroupMessage.Command, Info.TARGET_GROUP_MESSAGE_COMMAND ).ToUpper();

					EditorGUI.indentLevel--;
				}

			WorldObjectEditor.EndObjectContent();
			// CONTENT END
		}
			
		public static void DrawTargetBehaviour( ICECreatureControl _control, TargetObject _target, string _default_key = "" , params string[] _params )
		{
			if( _target == null )
				return;

			string _title = "Creature Behaviour " + _target.Behaviour.BehaviourTitleSuffix();
			string _hint = "";
			string _help = Info.TARGET_BEHAVIOUR;
			
			ICEEditorLayout.BeginHorizontal();

				_target.Behaviour.Foldout = ICEEditorLayout.Foldout( _target.Behaviour.Foldout, _title, _hint, false );

				EditorGUI.BeginDisabledGroup( _target.Behaviour.UseSelectiveBehaviour == true );
					_target.Behaviour.UseAdvancedBehaviour = ICEEditorLayout.CheckButtonSmall( "ADV", "Allows advanced behaviour settings", _target.Behaviour.UseAdvancedBehaviour );
				EditorGUI.EndDisabledGroup();

				_target.Behaviour.UseSelectiveBehaviour = ICEEditorLayout.CheckButtonSmall( "SEL", "Allows selective behaviour settings", _target.Behaviour.UseSelectiveBehaviour );
		
			ICEEditorLayout.EndHorizontal( _help );

			if( ! _target.Behaviour.Foldout )
				return;
			
			EditorGUI.indentLevel++;

				if( _target.Behaviour.UseSelectiveBehaviour )
				{
					EditorGUILayout.HelpBox( "Sorry, this feature is unfortunately not available in the current version!", MessageType.Info );
				}
				else if( _target.Behaviour.UseAdvancedBehaviour )
				{
					string _title_1 = ( _params.Length > 1 ?_params[1] : "Standard" );
					string _title_2 = ( _params.Length > 2 ?_params[2] : "Rendezvous" );

					string _default_key_1 = _default_key + "_TS_" + ( _params.Length > 1 ?_params[1] : "Standard" ) + "_" + _target.TargetName;
					string _default_key_2 = _default_key + "_TR_" + ( _params.Length > 2 ?_params[2] : "Rendezvous" ) + "_" + _target.TargetName;

					_target.Behaviour.BehaviourModeKey = BehaviourEditor.BehaviourSelect( _control, _title_1, "Move behaviour while approching or avoiding the target", _target.Behaviour.BehaviourModeKey, _default_key_1.ToUpper(), Info.TARGET_BEHAVIOUR_STANDARD ); 
					_target.Behaviour.BehaviourModeKeyReached = BehaviourEditor.BehaviourSelect( _control, _title_2, "Rendezvous behaviour while the final move position of the target was reached", _target.Behaviour.BehaviourModeKeyReached, _default_key_2.ToUpper(), Info.TARGET_BEHAVIOUR_RENDEZVOUS ); 
				}
				else
				{
					string _title_0 = ( _params.Length > 0 ?_params[0]: "Standard" );

					string _default_key_1 = _default_key + "_TS_" + ( _params.Length > 1 ?_params[1] : "Standard" ) + "_" + _target.TargetName;

					_target.Behaviour.BehaviourModeKey = BehaviourEditor.BehaviourSelect( _control, _title_0, "Behaviour while the target is active.", _target.Behaviour.BehaviourModeKey, _default_key_1.ToUpper(), Info.TARGET_BEHAVIOUR_STANDARD ); 
				}

			EditorGUI.indentLevel--;
		}

		public static void DrawTargetEvents( ICECreatureControl _control, TargetObject _target )
		{
			if( _target == null )
				return;
			
			ICEWorldBehaviour _behaviour = null;

			if( _target.TargetGameObject != null )
				_behaviour = _target.TargetGameObject.GetComponent<ICEWorldBehaviour>();

			CreatureObjectEditor.DrawEventsObject( _behaviour, _target.Events, EditorHeaderType.FOLDOUT_ENABLED, EditorHeaderType.FOLDOUT_ENABLED, Info.TARGET_EVENT, "Target Events" );
		}

		/// <summary>
		/// Draws the target influence settings.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_target">Target.</param>
		public static void DrawTargetInfluenceSettings( ICECreatureControl _control, TargetObject _target )
		{
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.Influences.Enabled == false );
					_target.Influences.Foldout = ICEEditorLayout.Foldout( _target.Influences.Foldout, "Creature Influences", "", false );
				EditorGUI.EndDisabledGroup();

				GUILayout.FlexibleSpace();

				InteractorRuleObject _rule = _target as InteractorRuleObject;
				if( _rule != null )
				{
					_rule.OverrideInfluences = ICEEditorLayout.CheckButtonMiddle( "OVERRIDE", "Overrides initial target influences", _rule.OverrideInfluences );

					_rule.Influences.Enabled = _rule.OverrideInfluences;

					if( ! _rule.OverrideInfluences )
						_rule.Influences.Foldout = false;
				}
				else
					_target.Influences.Enabled = ICEEditorLayout.EnableButton( "Enables/disables the influences", _target.Influences.Enabled );

			ICEEditorLayout.EndHorizontal( Info.TARGET_INFLUENCES );

			EditorGUI.BeginDisabledGroup( _target.Influences.Enabled == false );
				CreatureObjectEditor.DrawInfluenceObject( _target.Influences, EditorHeaderType.FOLDOUT_CUSTOM, _control.Creature.Status.UseAdvanced, _control.Creature.Status.InitialDurabilityMax, Info.ENVIROMENT_COLLISION_INFLUENCES );
			EditorGUI.EndDisabledGroup();
		}



		/// <summary>
		/// Draws the target move specifications.
		/// </summary>
		/// <returns>The target move specifications.</returns>
		/// <param name="_game_object">Game object.</param>
		/// <param name="_target">Target.</param>
		public static TargetObject DrawTargetMoveSpecification( GameObject _owner, TargetObject _target )
		{
			/*
			if( ! _target.Move.Enabled )
			{
				_target.Move.StoppingDistance = CreatureEditorLayout.DrawStoppingDistance( _target.Move.StoppingDistance, ref _target.Move.StoppingDistanceMaximum,  ref  _target.Move.IgnoreLevelDifference,  ref  _target.Move.StoppingDistanceZoneRestricted, ref _target.Move.Enabled );
				return _target;
			}
			*/

			InteractorRuleObject _rule = _target as InteractorRuleObject;

			string _title_suffix = ( _rule != null && ! _rule.OverrideTargetMovePosition ? " (same as in Act One)" : ( ! _target.Move.Enabled ? " (Transform Position)" : " (" + _target.Move.MovePositionType.ToString() + ")" ) );
			// BEGIN TARGET MOVE SPECIFICTAION HEADER
			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _target.Move.Enabled == false );
					_target.Move.Foldout = ICEEditorLayout.Foldout( _target.Move.Foldout, "Target Move Specification" + _title_suffix, "", false );
				EditorGUI.EndDisabledGroup();

				//GUILayout.FlexibleSpace();
				//_target.Move.MovePositionType = (TargetMovePositionType)EditorGUILayout.EnumPopup( _target.Move.MovePositionType, GUILayout.MaxWidth( 200 ) );

				//EditorGUI.BeginDisabledGroup( _target.Move.UseDynamicOffsetDistance == false );		
				//_target.UseLastKnownPosition = ICEEditorLayout.ButtonCheck( "LKP", "Use last known position", _target.UseLastKnownPosition, ICEEditorStyle.CMDButtonDouble );
				//EditorGUI.EndDisabledGroup();

				
				if( _rule != null )
				{
					_rule.OverrideTargetMovePosition = ICEEditorLayout.CheckButtonMiddle( "OVERRIDE", "Overrides initial target move specifications", _rule.OverrideTargetMovePosition );

					if( ! _rule.OverrideTargetMovePosition )
						_target.Move.Foldout = false;

					EditorGUI.BeginDisabledGroup( _rule.OverrideTargetMovePosition == false );
						_target.Move.Enabled = ICEEditorLayout.EnableButton( "Enables/Disables the move specification settings", _target.Move.Enabled );
					EditorGUI.EndDisabledGroup();
				}
				else
					_target.Move.Enabled = ICEEditorLayout.EnableButton( "Enables/Disables the move specification settings", _target.Move.Enabled );

			ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS );
			// END TARGET MOVE SPECIFICTAION HEADER

			// BEGIN TARGET MOVE SPECIFICTAION CONTENT
			if( ! _target.Move.Foldout )
				return _target;

			EditorGUI.BeginDisabledGroup( _target.Move.Enabled == false );
			EditorGUI.indentLevel++;

				_target.Move.StoppingDistance = CreatureEditorLayout.DrawStoppingDistance( _target.Move.StoppingDistance, ref _target.Move.StoppingDistanceMaximum,  ref  _target.Move.IgnoreLevelDifference,  ref  _target.Move.StoppingDistanceZoneRestricted );
				
				ICEEditorLayout.BeginHorizontal();
					_target.Move.MovePositionType = (TargetMovePositionType)ICEEditorLayout.EnumPopup( "Preferred Move Position", "", _target.Move.MovePositionType );				
					
					if( _target.Move.OverlapPreventionLayerMask == 0 )
						_target.Move.UseVerifiedDesiredTargetMovePosition = false;
					EditorGUI.BeginDisabledGroup( _target.Move.OverlapPreventionLayerMask == 0 );
						_target.Move.UseVerifiedDesiredTargetMovePosition = ICEEditorLayout.CheckButtonMiddle( "VERIFIED", "Ensures that the determined position is outside of other colliders.", _target.Move.UseVerifiedDesiredTargetMovePosition ); 
					EditorGUI.EndDisabledGroup();
				ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_OFFSET );

				// BEGIN MOVE POSITION TYPE
				{
					// MOVE POSITION TYPE : RANGE
					if( _target.Move.MovePositionType == TargetMovePositionType.Range )
					{
						EditorGUI.indentLevel++;
							ICEEditorLayout.BeginHorizontal();
								_target.Move.FixedRange = ICEEditorLayout.MaxDefaultSlider( "Distance", "",_target.Move.FixedRange, Init.DECIMAL_PRECISION_DISTANCES, _target.Move.StoppingDistance, ref _target.Move.FixedRangeMaximum, 0 );
							ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_CIRCULAR_RANGE );
						EditorGUI.indentLevel--;
					}

					// MOVE POSITION TYPE : COVER
					if( _target.Move.MovePositionType == TargetMovePositionType.Cover )
					{
						EditorGUI.indentLevel++;
							_target.Move.CoverRange = ICEEditorLayout.MaxDefaultSlider( "Range", "", _target.Move.CoverRange, Init.DECIMAL_PRECISION_DISTANCES, 0f, ref _target.Move.CoverRangeMaximum, 10, "" );
							
							EditorGUILayout.Separator();
							_target.Move.CoverStepAngle = ICEEditorLayout.DefaultSlider( "Scan Step Angle", "", _target.Move.CoverStepAngle, Init.DECIMAL_PRECISION_ANGLE, 1f, 36, 3.6f, "" );
							_target.Move.CoverMaxAngle = ICEEditorLayout.DefaultSlider( "Scan Max. Angle", "", _target.Move.CoverMaxAngle, Init.DECIMAL_PRECISION_ANGLE, 45, 360, 180, "" );
						
							_target.Move.CoverHorizontalOffset = ICEEditorLayout.MaxDefaultSlider( "Scan Horizontal Offset", "", _target.Move.CoverHorizontalOffset, Init.DECIMAL_PRECISION_DISTANCES, 0f, ref _target.Move.CoverHorizontalOffsetMaximum, 0.5f, "" );
							//_target.Move.CoverVerticalOffset = ICEEditorLayout.MaxDefaultSlider( "Scan Vertical Offset", "", _move.Cover.VerticalOffset, Init.DECIMAL_PRECISION_DISTANCES, - _move.Cover.VerticalOffsetMaximum, ref _move.Cover.VerticalOffsetMaximum, 0f, "" );

							EditorGUILayout.Separator();
							CreatureEditorLayout.DrawCoverCheck( _target.Move.CoverLayer.Layers, true, "" );
						EditorGUI.indentLevel--;
					}

					// MOVE POSITION TYPE : OFFSET
					else if( _target.Move.MovePositionType == TargetMovePositionType.Offset )
					{						
						if( _target.Move.UpdateOffset( ICEEditorLayout.OffsetGroup( "Offset", _target.Move.Offset, _owner, _target.TargetGameObject, 0.5f, 25, Info.TARGET_MOVE_SPECIFICATIONS_OFFSET ) ) )
							_target.UpdateTargetMovePositionOffset( false );

						// BEGIN OFFSET DISTANCE AND ANGLE
						EditorGUI.indentLevel++;
						{

							// BEGIN MOVE POSITION TYPE : OFFSET DISTANCE
							ICEEditorLayout.BeginHorizontal();
								float _distance = _target.Move.OffsetDistance;
								if( _target.Move.UseDynamicOffsetDistance )
								{
									float _min_distance = _target.Move.MinOffsetDistance;
									float _max_distance = _target.Move.MaxOffsetDistance;
									ICEEditorLayout.MinMaxDefaultSlider( "Distance", "", ref _min_distance, ref _max_distance, 0, ref _target.Move.OffsetDistanceMaximum, 0, 0, Init.DECIMAL_PRECISION_DISTANCES, 40 );

									if( _target.Move.MinOffsetDistance != _min_distance || _target.Move.MaxOffsetDistance != _max_distance )
										_distance = Random.Range( _min_distance, _max_distance );

									_target.Move.MinOffsetDistance = _min_distance;
									_target.Move.MaxOffsetDistance = _max_distance;
								}
								else
								{
									_distance = ICEEditorLayout.MaxDefaultSlider( "Distance", "",_target.Move.OffsetDistance, Init.DECIMAL_PRECISION_DISTANCES, Init.TARGET_OFFSET_DISTANCE_MIN, ref _target.Move.OffsetDistanceMaximum, Init.TARGET_OFFSET_DISTANCE_DEFAULT );
								}

								_target.Move.UseDynamicOffsetDistance = ICEEditorLayout.CheckButtonSmall( "DYN", "Dynamic Offset Distance", _target.Move.UseDynamicOffsetDistance );

								EditorGUI.BeginDisabledGroup( _target.Move.UseDynamicOffsetDistance == false );		
									_target.Move.UseRandomOffsetDistance = ICEEditorLayout.CheckButtonSmall( "RND", "Random Offset Distance", _target.Move.UseRandomOffsetDistance );
								EditorGUI.EndDisabledGroup();
							ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_OFFSET_DISTANCE  );

							if( _target.Move.UseDynamicOffsetDistance )
							{
								EditorGUI.indentLevel++;
									_target.Move.DynamicOffsetDistanceUpdateSpeed = ICEEditorLayout.DefaultSlider( "Speed", "", _target.Move.DynamicOffsetDistanceUpdateSpeed,  0.01f, 0, 36, 0, Info.TARGET_MOVE_SPECIFICATIONS_OFFSET_ANGLE );					
								EditorGUI.indentLevel--;
							}

							// END MOVE POSITION TYPE : OFFSET DISTANCE

	

							// BEGIN MOVE POSITION TYPE : OFFSET ANGLE
								ICEEditorLayout.BeginHorizontal();
									float _angle = _target.Move.OffsetAngle;

									if( _target.Move.UseDynamicOffsetAngle )
									{
										float _min_angle = _target.Move.MinOffsetAngle;
										float _max_angle = _target.Move.MaxOffsetAngle;
										ICEEditorLayout.MinMaxDefaultSlider( "Angle", "", ref _min_angle, ref _max_angle, 0, 360, 0, 0, Init.DECIMAL_PRECISION, 40 );

										if( _target.Move.MinOffsetAngle != _min_angle || _target.Move.MaxOffsetAngle != _max_angle )
											_angle = Random.Range( _min_angle, _max_angle );

										_target.Move.MinOffsetAngle = _min_angle;
										_target.Move.MaxOffsetAngle = _max_angle;
									}
									else
									{
										_angle = ICEEditorLayout.DefaultSlider( "Angle", "", _target.Move.OffsetAngle, 0.01f, 0, 360, 0 );	
									}
														
									_target.Move.UseDynamicOffsetAngle = ICEEditorLayout.CheckButtonSmall( "DYN", "Dynamic Offset Angle", _target.Move.UseDynamicOffsetAngle );

									EditorGUI.BeginDisabledGroup( _target.Move.UseDynamicOffsetAngle == false );		
										_target.Move.UseRandomOffsetAngle = ICEEditorLayout.CheckButtonSmall( "RND", "Random Offset Angle", _target.Move.UseRandomOffsetAngle );
									EditorGUI.EndDisabledGroup();
								ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_OFFSET_ANGLE );

								if( _target.Move.UseRandomOffsetAngle || _target.Move.UseDynamicOffsetAngle )
								{
									EditorGUI.indentLevel++;
										_target.Move.DynamicOffsetAngleUpdateSpeed = ICEEditorLayout.DefaultSlider( "Speed", "", _target.Move.DynamicOffsetAngleUpdateSpeed,  0.01f, 0, 36, 0, Info.TARGET_MOVE_SPECIFICATIONS_OFFSET_ANGLE );					
									EditorGUI.indentLevel--;
								}
								// END MOVE POSITION TYPE : OFFSET ANGLE

								if( _target.Move.UpdateOffset( _angle, _distance ) )
									_target.UpdateTargetMovePositionOffset( false );
						}						
						EditorGUI.indentLevel--;
						// END OFFSET DISTANCE AND ANGLE

						EditorGUILayout.Separator();						

						// BEGIN MOVE POSITION TYPE : OFFSET SMOOTHING
						ICEEditorLayout.BeginHorizontal();
							_target.Move.SmoothingSpeed = ICEEditorLayout.MaxDefaultSlider( "Smoothing Speed", "Smoothing Speed affects step-size and update speed of the TargetMovePosition.", _target.Move.SmoothingSpeed, 0.01f, 0, ref _target.Move.SmoothingSpeedMaximum, 0 );
							_target.Move.UseCreatureSpeed = ICEEditorLayout.AutoButtonSmall( "Adds the defined smoothing speed and the current speed of the creature", _target.Move.UseCreatureSpeed );
						ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_SMOOTHING );
						// END MOVE POSITION TYPE : OFFSET SMOOTHING

						// BEGIN MOVE POSITION TYPE : OFFSET RANDOM POSITIONING
						ICEEditorLayout.BeginHorizontal();

							if( _target.Move.UseRandomRect )
							{
								if( _target.Move.UpdateRandomRange( ICEEditorLayout.Vector3Field( "Random Positioning Range", "", _target.Move.RandomRangeRect ) ) )
									_target.UpdateTargetMovePositionOffset( true );
								if( ICEEditorLayout.RandomButton( "" ) )
									_target.Move.UpdateRandomRange( new Vector3( Random.Range( 10, _target.Move.RandomRangeMaximum ), 0, Random.Range( 10, _target.Move.RandomRangeMaximum ) ) );
								if( ICEEditorLayout.ButtonSmall( "TEST", "" ) )
									_target.UpdateTargetMovePositionOffset( true );
							}
							else
							{
								if( _target.Move.UpdateRandomRange( ICEEditorLayout.MaxDefaultSlider( "Random Positioning Range", "", _target.Move.RandomRange, Init.DECIMAL_PRECISION_DISTANCES, 0, ref _target.Move.RandomRangeMaximum, Init.TARGET_RANDOM_RANGE_DEFAULT ) ) )
									_target.UpdateTargetMovePositionOffset( true );
								if( ICEEditorLayout.RandomButton( "" ) )
									_target.Move.UpdateRandomRange( Random.Range( 10, _target.Move.RandomRangeMaximum ) );
								if( ICEEditorLayout.ButtonSmall( "TEST", "" ) )
									_target.UpdateTargetMovePositionOffset( true );
							}

							bool _org_random_rect = _target.Move.UseRandomRect;
							_target.Move.UseRandomRect = ICEEditorLayout.CheckButtonSmall( "RECT", "", _target.Move.UseRandomRect );
							if( _target.Move.UseRandomRect != _org_random_rect )
								_target.UpdateTargetMovePositionOffset( true );
				


						ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_RANDOM_RANGE );

						if( _target.Move.HasRandomRange )
						{
							EditorGUI.indentLevel++;
								ICEEditorLayout.BeginHorizontal();
									EditorGUILayout.PrefixLabel( new GUIContent( "Update Position on ... ", "" ) );
									_target.Move.UseUpdateOffsetOnTargetChanged = ICEEditorLayout.CheckButtonFlex( "CHANGED", "Update on target changed", _target.Move.UseUpdateOffsetOnTargetChanged );
									_target.Move.UseUpdateOffsetOnActivateTarget = ICEEditorLayout.CheckButtonFlex( "ACTIVATE", "Update on activate target", _target.Move.UseUpdateOffsetOnActivateTarget );
									_target.Move.UseUpdateOffsetOnMovePositionReached = ICEEditorLayout.CheckButtonFlex( "REACHED", "Update on reached MovePosition", _target.Move.UseUpdateOffsetOnMovePositionReached );
									_target.Move.UseUpdateOffsetOnRandomizedTimer = ICEEditorLayout.CheckButtonFlex( "TIMER", "Update on timer interval", _target.Move.UseUpdateOffsetOnRandomizedTimer );
								ICEEditorLayout.EndHorizontal( Info.TARGET_MOVE_SPECIFICATIONS_OFFSET_UPDATE );
								EditorGUI.indentLevel++;
									if( _target.Move.UseUpdateOffsetOnRandomizedTimer == true )
										ICEEditorLayout.MinMaxSlider( "Min/Max Interval", "", ref _target.Move.OffsetUpdateTimeMin, ref _target.Move.OffsetUpdateTimeMax, 0, 360, 0.25f, 40, "" );
								EditorGUI.indentLevel--;
							EditorGUI.indentLevel--;
						}

						// END MOVE POSITION TYPE : OFFSET RANDOM POSITIONING
					}
					
					// MOVE POSITION TYPE : OTHERTARGET
					else if( _target.Move.MovePositionType == TargetMovePositionType.OtherTarget )
					{
						EditorGUI.indentLevel++;
							_target.Move.MoveTargetName = Popups.TargetPopup( "Move Target", "", _target.Move.MoveTargetName, "" );
						EditorGUI.indentLevel--;
					}

				}
				// END MOVE POSITION TYPE
				EditorGUILayout.Separator();
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			// END TARGET MOVE SPECIFICTAION CONTENT
			return _target;
		}



	}

}
