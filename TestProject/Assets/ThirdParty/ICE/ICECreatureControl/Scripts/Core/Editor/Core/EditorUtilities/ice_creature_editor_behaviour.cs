// ##############################################################################
//
// ice_creature_editor_behaviour.cs | BehaviourEditor
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

namespace ICE.Creatures.EditorUtilities
{
	public static class BehaviourEditor
	{	
		private static string m_BehaviourKey = "";



		/// <summary>
		/// Handles the behaviour settings.
		/// </summary>
		public static void Print( ICECreatureControl _control ){
			
			if( ! _control.Display.ShowBehaviour )
				return;
			
			ICEEditorStyle.SplitterByIndent( 0 );

			ICEEditorLayout.BeginHorizontal();
				_control.Display.FoldoutBehaviours = ICEEditorLayout.Foldout(_control.Display.FoldoutBehaviours, "Behaviours" );
				if( ICEEditorLayout.SaveButton( "Saves behaviours to file" ) )
					CreatureEditorIO.SaveBehaviourToFile( _control.Creature.Behaviour, _control.gameObject.name );				
				if( ICEEditorLayout.LoadButton( "Loads behavious form file" ) )
					_control.Creature.Behaviour = CreatureEditorIO.LoadBehaviourFromFile( _control.Creature.Behaviour );				
				if( ICEEditorLayout.ResetButton( "Removes all behaviours" ) )
					_control.Creature.Behaviour.Reset();
				ICEEditorLayout.ListFoldoutButtons( _control.Creature.Behaviour.BehaviourModes );
			ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR );
			
			if( _control.Display.FoldoutBehaviours == false ) 
				return;

			EditorGUI.indentLevel++;				
				for( int i = 0 ; i < _control.Creature.Behaviour.BehaviourModes.Count; i++ )
					if( DrawBehaviourMode( _control, _control.Creature.Behaviour.BehaviourModes[i] ) )
						return;
			EditorGUI.indentLevel--;

			m_BehaviourKey = ( ! SystemTools.IsValid( m_BehaviourKey ) ? "NEW" : m_BehaviourKey );
			if( ICEEditorLayout.DrawListAddLine( "Add behaviour mode by key", ref m_BehaviourKey ) )
			{
				_control.Creature.Behaviour.AddBehaviourModeNumbered( m_BehaviourKey );							
				m_BehaviourKey = "";
			}
		}

		private static string m_BehaviourRenameKey = "";
		private static BehaviourModeObject m_BehaviourRenameMode = null;
		public static void RenameBehaviourMode( ICECreatureControl _control, BehaviourModeObject _mode )
		{
			if( m_BehaviourRenameMode == null || m_BehaviourRenameMode != _mode )
				return;

			ICEEditorLayout.Label( "Rename Behaviour Mode", true );
			EditorGUI.indentLevel++;
			ICEEditorLayout.BeginHorizontal();
				m_BehaviourRenameKey = EditorGUILayout.TextField( "Mode Key",_control.Creature.Behaviour.GetFixedBehaviourModeKey(  m_BehaviourRenameKey ) );

				if( ICEEditorLayout.Button( "RENAME" ))
				{
					if( _control.Creature.Behaviour.BehaviourModeExists( m_BehaviourRenameKey ) == false )
					{
						m_BehaviourRenameMode.Key = m_BehaviourRenameKey;
						m_BehaviourRenameKey = "";
						m_BehaviourRenameMode = null;
					}
				}

				if( ICEEditorLayout.Button( "CANCEL" ) )
				{
					m_BehaviourRenameKey = "";
					m_BehaviourRenameMode = null;
				}

			ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MODE_RENAME );
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
		}


		public static void DrawBehaviourEditor( ICECreatureControl _control, ref string _key )
		{
			if( string.IsNullOrEmpty( _key ) || _key.Trim() == "" )
				return;

			BehaviourModeObject _mode = _control.Creature.Behaviour.GetBehaviourModeByKey ( _key );

			if( _mode == null )
				return;

			EditorGUILayout.BeginVertical( "box" );
				DrawBehaviourMode( _control, _mode, true );
			EditorGUILayout.EndVertical();

			_key = _mode.Key;
		}

		/// <summary>
		/// Draws a behaviour mode.
		/// </summary>
		/// <param name="_mode">_mode.</param>
		/// <param name="_index">_index.</param>
		public static bool DrawBehaviourMode( ICECreatureControl _control, BehaviourModeObject _mode, bool _editor = false )
		{
			if ( _mode == null ) 
				return false;

			int _indent = EditorGUI.indentLevel;

			if( _editor )
			{
				EditorGUILayout.Separator();
				_mode.Foldout = true;
				EditorGUI.indentLevel = 1;
			}
			else
			{
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );
			}

			string _title = _mode.Key + " (" + _mode.Rules.Count + (_mode.Rules.Count == 1?" Rule)":" Rules)" );
				
			ICEEditorLayout.BeginHorizontal();
				if( _editor )
				EditorGUILayout.PrefixLabel( _title, "Button", EditorStyles.boldLabel );
				else
					_mode.Foldout = ICEEditorLayout.Foldout( _mode.Foldout, _title );
			
				GUILayout.FlexibleSpace();

				
				EditorGUI.BeginDisabledGroup( m_BehaviourRenameMode == _mode );
					if( ICEEditorLayout.Button( "RENAME" ) )
					{
						m_BehaviourRenameKey = _mode.Key;
						m_BehaviourRenameMode = _mode;
					}
				EditorGUI.EndDisabledGroup();
				GUILayout.Space( 5 );

				if( ICEEditorLayout.SaveButtonSmall( "Saves selected behaviour mode to file." ) )
					CreatureEditorIO.SaveBehaviourModeToFile( _mode, _control.name + "_" + _mode.Key );

				if( ICEEditorLayout.LoadButtonSmall( "Loads a behaviour mode from file." ) )
					_mode.Copy( CreatureEditorIO.LoadBehaviourModeFromFile( new BehaviourModeObject() ) );

				if( ICEEditorLayout.CopyButtonSmall( "Creates a copy of the selected behaviour mode." ) )
					_control.Creature.Behaviour.CopyBehaviourMode( _mode );

				if( ICEEditorLayout.ResetButtonSmall( "Resets the selected behaviour mode." ) )
					_mode.Copy( new BehaviourModeObject( _mode.Key ));

				//EditorGUI.EndDisabledGroup();

				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListDeleteButton<BehaviourModeObject>( _control.Creature.Behaviour.BehaviourModes, _mode, "Removes selected behaviour mode." ) )
					return true;
			
				GUILayout.Space( 5 );
				if( ICEEditorLayout.ListUpDownButtons<BehaviourModeObject>( _control.Creature.Behaviour.BehaviourModes, _control.Creature.Behaviour.BehaviourModes.IndexOf( _mode ) ) )
					return true;


				EditorGUI.BeginDisabledGroup( _mode.Rules.Count < 2 );
					ICEEditorLayout.ListFoldoutButtonsMini( _mode.Rules );
				EditorGUI.EndDisabledGroup();
				GUILayout.Space( 3 );

			ICEEditorLayout.EndHorizontal(  ref _mode.ShowInfoText, ref _mode.InfoText, Info.BEHAVIOUR_MODE );
			RenameBehaviourMode( _control, _mode );
				//EditorGUILayout.Separator();
		


			if( _mode.Foldout ) 
				DrawBehaviourModeContent( _control, _mode );

			if( _editor )
			{
				EditorGUI.indentLevel = _indent;
			}

			return false;
		}

		/// <summary>
		/// Draws the content of the behaviour mode.
		/// </summary>
		/// <param name="_control">Control.</param>
		/// <param name="_mode">Mode.</param>
		public static void DrawBehaviourModeContent( ICECreatureControl _control, BehaviourModeObject _mode )
		{
			if( _mode == null )
				return;

			EditorHeaderType _header = EditorHeaderType.FOLDOUT_ENABLED_BOLD;

			EditorGUI.indentLevel++;
				CreatureObjectEditor.DrawBehaviourModeFavouredObject( _control, _mode.Favoured, _header, Info.BEHAVIOUR_MODE_FAVOURED );
			EditorGUI.indentLevel--;

			if( _mode.Rules.Count > 1 )
			{
				ICEEditorLayout.Label("Rules", true, Info.BEHAVIOUR_MODE_RULE );
				EditorGUI.indentLevel++;
					ICEEditorLayout.BeginHorizontal();
						_mode.RulesOrderType = (SequenceOrderType)ICEEditorLayout.EnumPopup("Order Type","", _mode.RulesOrderType );
						EditorGUI.BeginDisabledGroup( _mode.RulesOrderType != SequenceOrderType.CYCLE );
							_mode.RulesOrderInverse = ICEEditorLayout.CheckButtonMiddle( "INVERSE", "", _mode.RulesOrderInverse ); 
						EditorGUI.EndDisabledGroup();
					ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MODE_RULES_ORDER );
				EditorGUI.indentLevel--;
			}
			
			for( int i = 0; i < _mode.Rules.Count; i++ )
				DrawBehaviourModeRule( _control, _mode, i, _mode.Rules, _mode.Key  );

			if( _mode.Rules.Count == 1 )
			{
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel + 1 );
				ICEEditorLayout.BeginHorizontal();
					EditorGUILayout.LabelField( "Add or Copy Behaviour Rule" , EditorStyles.miniLabel );	
					if( ICEEditorLayout.AddButton( "Add Behaviour Rule" ) )
						_mode.Rules.Add( new BehaviourModeRuleObject() );
					if( ICEEditorLayout.CopyButtonMiddle( "Copy Behaviour Rule" ) )
						_mode.Rules.Add( new BehaviourModeRuleObject( _mode.Rules[0] ) );
				ICEEditorLayout.EndHorizontal( "Add or Copy Behaviour Rule" );
			}
			else
				ICEEditorLayout.DrawListAddLine<BehaviourModeRuleObject>( _mode.Rules, new BehaviourModeRuleObject(), true, "Add Behaviour Rule", Info.BEHAVIOUR_MODE_RULE_ADD );
			
			//EditorGUILayout.Separator();
		}
		
		
		/// <summary>
		/// Draws the behaviour mode rule.
		/// </summary>
		/// <param name="_index">_index.</param>
		/// <param name="_list">_list.</param>
		public static void DrawBehaviourModeRule( ICECreatureControl _control, BehaviourModeObject _mode, int _index, List<BehaviourModeRuleObject> _list, string _key )
		{
			BehaviourModeRuleObject _rule = _list[_index];
			
			if( _rule == null )
				return;

			if( _list.Count > 1 )
			{
				EditorGUI.indentLevel++;
				
				ICEEditorStyle.SplitterByIndent( EditorGUI.indentLevel );

				ICEEditorLayout.BeginHorizontal();
					EditorGUI.BeginDisabledGroup( _rule.Enabled == false );
						_rule.Foldout = EditorGUILayout.Foldout( _rule.Foldout, _key + " Rule #"+(_index+1), ICEEditorStyle.Foldout );
					EditorGUI.EndDisabledGroup();

					GUILayout.FlexibleSpace();

					ICEEditorLayout.StatusButton( "ACTIVE", _rule.Active == false, Color.green );

					if( ICEEditorLayout.CopyButtonMiddle() )
						_list.Add ( new BehaviourModeRuleObject( _rule ) );
							
					GUILayout.Space( 5 );
					if( ICEEditorLayout.ListDeleteButton<BehaviourModeRuleObject>( _list, _rule, "Removes this rule." ) )
						return;
				
					if( ICEEditorLayout.ListUpDownButtons<BehaviourModeRuleObject>( _list, _list.IndexOf( _rule ) ) )
						return;
					

					
					_rule.Enabled = ICEEditorLayout.EnableButton( "Enables/disables this rule", _rule.Enabled );

				ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_MODE_RULE );
			}
			else
			{
				_rule.Foldout = true;
				_rule.Enabled = true;
			}
				
			if( _rule.Foldout )
			{
				
				EditorHeaderType _header = EditorHeaderType.FOLDOUT_ENABLED_BOLD;
				EditorHeaderType _sub_header = EditorHeaderType.FOLDOUT_ENABLED_BOLD;

				EditorGUI.BeginDisabledGroup( _rule.Enabled == false );

				// WEIGHTEDRANDOM BEGIN
				if( _list.Count > 1 && _mode.RulesOrderType == SequenceOrderType.WEIGHTEDRANDOM )
					_rule.Weight = ICEEditorLayout.DefaultSlider( "Weight", "Weight Value for WEIGHTEDRANDOM Sequence Type, relative to other rules weight value", _rule.Weight, Init.DECIMAL_PRECISION, 0, 100, 1, Info.BEHAVIOUR_MODE_RULE_WEIGHT );
				// WEIGHTEDRANDOM END

				EditorGUI.indentLevel++;

					// CUSTOM LENGTH BEGIN
					if( _list.Count > 1 || _rule.Link.Enabled )
						DrawBehaviourCustomLength( _control, _rule, _header  );	
					// CUSTOM LENGTH END

					CreatureBehaviourEditor.DrawBehaviourAnimation( _control,  _mode, _rule, _header );
					CreatureBehaviourEditor.DrawBehaviourMove( _control, _mode, _rule, _header );			
					CreatureObjectEditor.DrawInfluenceObject( _mode, _rule.Influences, _header, _control.Creature.Status.UseAdvanced, _control.Creature.Status.InitialDurabilityMax, Info.BEHAVIOUR_INFLUENCES );
					CreatureObjectEditor.DrawInventoryActionObject( _control, _rule.Inventory, _header, Info.BEHAVIOUR_INVENTORY );
					CreatureObjectEditor.DrawAudioObject( _rule.Audio, _header, Info.BEHAVIOUR_AUDIO );
					CreatureObjectEditor.DrawEventsObject( _control, _rule.Events, _header, _sub_header, Info.BEHAVIOUR_EVENTS );
					CreatureObjectEditor.DrawLookDataObject( _rule.Look, _header, Info.BEHAVIOUR_LOOK );
					CreatureObjectEditor.DrawEffectObject( _control, _rule.Effect, _header, Info.BEHAVIOUR_EFFECT );				
					CreatureBehaviourEditor.DrawBehaviourModeRuleLinkObject( _control, _list, _rule.Link, _header, Info.BEHAVIOUR_LINK, "", "", _key + "_" + _index );					

					EditorGUILayout.Separator();
					EditorGUI.EndDisabledGroup();
				EditorGUI.indentLevel--;
			}
			
			if( _list.Count > 1 )
				EditorGUI.indentLevel--;
			
		}

		/// <summary>
		/// Draws the length of the behaviour custom.
		/// </summary>
		/// <returns>The behaviour custom length.</returns>
		/// <param name="_rule">Rule.</param>
		private static void DrawBehaviourCustomLength( ICECreatureControl _control, BehaviourModeRuleObject _rule, EditorHeaderType _type, string _help = "", string _title = "", string _hint = "" )
		{
			if( _rule == null )
				return;

			if( string.IsNullOrEmpty( _title ) )
				_title = "Rule Length";
			if( string.IsNullOrEmpty( _hint ) )
				_hint = "";
			if( string.IsNullOrEmpty( _help ) )
				_help = Info.BEHAVIOUR_LENGTH;

			ICEEditorLayout.BeginHorizontal();
				CreatureObjectEditor.DrawObjectHeaderLine( ref _rule.UseCustomLength, ref _rule.FoldoutCustomLength, _type, _title, _hint );
			ICEEditorLayout.EndHorizontal( _help );

			if( _rule.UseCustomLength && _rule.FoldoutCustomLength )
			{
				EditorGUI.indentLevel++;

					ICEEditorLayout.BeginHorizontal();
						ICEEditorLayout.MinMaxSlider( "Min/Max Length (secs.)", "Enter the desired Play-Length or press 'RND' to set randomized values.", ref _rule.LengthMin, ref _rule.LengthMax, 0, ref _rule.LengthMaximum, 0.25f, 35 );
						if( ICEEditorLayout.RandomButton( "" ) )
						{
							_rule.LengthMax = Random.Range( _rule.LengthMin, _rule.LengthMaximum );
							_rule.LengthMin = Random.Range( 0, _rule.LengthMax );
						}

						if( ICEEditorLayout.ButtonSmall( "ANIM", "" ) )
						{
							_rule.LengthMin = _rule.Animation.GetAnimationLength();
							_rule.LengthMax = _rule.LengthMin;
						}

						if( ICEEditorLayout.ResetButtonSmall( "" ) )
						{
							_rule.LengthMin = 0;
							_rule.LengthMax = 0;
						}
					ICEEditorLayout.EndHorizontal( Info.BEHAVIOUR_LENGTH );

				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
		}


		public static void DrawInRangeBehaviour( ICECreatureControl _control, ref string _leisure, ref string rendezvous, ref float _duration, ref bool _transit, float _range, int _index = 0 )
		{
			_duration = ICEEditorLayout.DurationSlider( "Duration Of Stay", "Desired duration of stay", _duration, Init.DURATION_OF_STAY_STEP, Init.DURATION_OF_STAY_MIN, Init.DURATION_OF_STAY_MAX,Init.DURATION_OF_STAY_DEFAULT, ref _transit );

			if( _transit == false )
			{
				EditorGUI.indentLevel++;

					if( _range > 0 )
					{
						_leisure = BehaviourEditor.BehaviourSelect( _control, "Leisure", "Randomized leisure activities after reaching the Random Range of the target. Please note, if the Random Range is adjusted to zero, leisure is not available.", _leisure, "WP_LEISURE" + (_index>0?"_"+_index:""), Info.ESSENTIALS_BEHAVIOURS_LEISURE );
					}

					EditorGUI.BeginDisabledGroup( _duration == 0 );
					rendezvous = BehaviourEditor.BehaviourSelect( _control, "Rendezvous", "Action behaviour after reaching the Stop Distance of the given target move position.", rendezvous, "WP_RENDEZVOUS" + (_index>0?"_"+_index:""), Info.ESSENTIALS_BEHAVIOURS_RENDEZVOUS );
					EditorGUI.EndDisabledGroup();

				EditorGUI.indentLevel--;
			}
		}


		public static string BehaviourSelectEnabled( ICECreatureControl _control, ref bool _enabled, string _title, string _hint, string _key, string _default_key, string _help = ""  )
		{
			bool _editor = false;

			ICEEditorLayout.BeginHorizontal();
				EditorGUI.BeginDisabledGroup( _enabled == false );
					_key = BehaviourPopup( _control, _title, _hint, _key, ref _editor, _default_key, _enabled );
				EditorGUI.EndDisabledGroup();
				_enabled = ICEEditorLayout.EnableButton( _enabled );
			ICEEditorLayout.EndHorizontal( _help );

			if( _editor )
				DrawBehaviourEditor( _control, ref _key );

			return _key;
		}

		public static string BehaviourSelect( ICECreatureControl _control, string _title, string _hint, string _key, string _default_key = "", string _help = ""  )
		{
			bool _editor = false;

			ICEEditorLayout.BeginHorizontal();
				_key = BehaviourPopup( _control, _title, _hint, _key, ref _editor, _default_key, true );
			ICEEditorLayout.EndHorizontal( _help );

			if( _editor )
				DrawBehaviourEditor( _control, ref _key );

			return _key;
		}
			

		private static Hashtable m_BehaviourSelectEditor = new Hashtable();
		private static Hashtable m_BehaviourBackupKeys = new Hashtable();

		public static string BehaviourPopup( ICECreatureControl _control, string _title, string _hint, string _key, ref bool _editor, string _default_key, bool _show_color, string _help = ""  )
		{
			string _hash = _default_key.GetHashCode().ToString();
			if( ! m_BehaviourSelectEditor.ContainsKey( _hash ) )
				m_BehaviourSelectEditor.Add( _hash, false );
			_editor = (bool)m_BehaviourSelectEditor[ _hash ];

			bool _backup_key_exists = m_BehaviourBackupKeys.ContainsKey( _hash );

			if( _control.Creature.Behaviour.BehaviourModes.Count > 0 && ! _backup_key_exists )
			{
				bool _key_exists = false;

				if( ! IsValid( _key ) && _show_color ) GUI.backgroundColor = Color.red;				
				_key = Popups.BehaviourPopup( _control,_title, _hint, _key, ref _key_exists );
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				if( _key_exists )
				{
					if( ICEEditorLayout.NewButton( "Creates a new Behaviour Mode" ) )
					{	
						m_BehaviourBackupKeys.Add( _hash, _key );
						_key = "";
					}
				}
				else
				{
					_editor = false;
					if( ICEEditorLayout.AddButton( "Creates a new Behaviour Mode" ) )
					{	
						_key = _control.Creature.Behaviour.AddBehaviourMode( _key );
						_editor = true;
					}
				}


				if( IsValid( _key ) )
				{
					EditorGUI.BeginDisabledGroup( _key_exists == false );
						_editor = ICEEditorLayout.EditButton( _editor, "Shows/hides the Behaviour Mode Editor" );
					EditorGUI.EndDisabledGroup();
				}
				else
				{
					EditorGUI.BeginDisabledGroup( IsValid( _default_key ) == false );
						if( ICEEditorLayout.AutoButton( "Generates a new behaviour mode automatically." ) )
							_key = WizardEditor.WizardBehaviour( _control, _default_key );
					EditorGUI.EndDisabledGroup();
				}


			}
			else
			{
				if( ! IsValid( _key ) && _show_color ) GUI.backgroundColor = Color.red;	
				string _new_key = EditorGUILayout.TextField( new GUIContent( _title, "Name of a new Behaviour") , _key );
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				if( _key != _new_key )
				{
					if( ! _backup_key_exists )
						m_BehaviourBackupKeys.Add( _hash, _key );

					_key = _new_key;
				}


				EditorGUI.BeginDisabledGroup( IsValid( _key ) == false );
					if( ICEEditorLayout.AddButton( "Adds new behaviour mode" ) )
					{	
						_key = _control.Creature.Behaviour.AddBehaviourMode( _key );

						if( IsValid( _key ) )
						{
							if( _backup_key_exists )
								m_BehaviourBackupKeys.Remove( _hash );
						
							_editor = true;
						}
					}
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( _backup_key_exists == false );	
					if( ICEEditorLayout.BackButton( "Cancels this process" ) )
					{
						_key = m_BehaviourBackupKeys[ _hash ].ToString();
						m_BehaviourBackupKeys.Remove( _hash );
					}				
				EditorGUI.EndDisabledGroup();

				EditorGUI.BeginDisabledGroup( IsValid( _default_key ) == false );
					if( ICEEditorLayout.AutoButton( "Generates a new behaviour mode automatically." ) )
						_key = WizardEditor.WizardBehaviour( _control, _default_key );
				EditorGUI.EndDisabledGroup();
			}

			m_BehaviourSelectEditor[ _hash ] = _editor;

			return _key.Trim();
		}

	
		private static bool IsValid( string _value )
		{
			if( ! string.IsNullOrEmpty( _value ) && _value.Trim() != "" )
				return true;
			else
				return false;
		}

	}
}