// ##############################################################################
//
// ice_creature_editor_popups.cs | Popups
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

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

using ICE.World.Utilities;

namespace ICE.Creatures.EditorUtilities
{
	/// <summary>
	/// Popup editor.
	/// </summary>
	public class Popups : WorldPopups
	{
		public static int QuickSelectIndex = 0;
		private static ReferenceGroupObject QuickSelectGroup = null; 

		public static void QuickSelectionPopup( string _title, ICECreatureEntity _entity )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;

			if( _register == null )
			{
			}
			else
			{
				ICEEditorLayout.BeginHorizontal();

				int _new_index = Popups.QuickSelectionPopup( _title, QuickSelectIndex );

				if( _register.ReferenceGroupObjects.Count > 0 )
				{
					int _own_index = CreatureRegister.GetReferenceIndexByName( _entity.name );
					if( _new_index != _own_index )
					{
						QuickSelectIndex = _new_index;
						QuickSelectGroup = _register.ReferenceGroupObjects[QuickSelectIndex];
					}

					if( ICEEditorLayout.DebugButton( "SELECT", "Activates the displayed GameObject. You can use this funtion to switch between your objects." ) && _new_index < _register.ReferenceGroupObjects.Count )
					{
						if( _entity != null )
							QuickSelectIndex = CreatureRegister.GetReferenceIndexByName( _entity.name );

						Selection.activeGameObject = _register.ReferenceGroupObjects[_new_index].ReferenceGameObject;
					}
				}
				else
				{
					if( ICEEditorLayout.DebugButton( "UPDATE", "Updates the register by scanning the scene for relevant objects" ) )
						_register.UpdateReferences();
				}

				if( ICEEditorLayout.DebugButton( "REGISTER", "Switches the focus to the Creature Register" ) )
					Selection.activeGameObject = _register.gameObject;

				if( QuickSelectGroup == null || QuickSelectGroup.EntityType != _entity.EntityType )
				{
					EditorGUI.BeginDisabledGroup( true );
					ICEEditorLayout.DebugButton( "COPY", "Copys the settings from the selected entity to this one. Both entities must be from the same type!" );
					EditorGUI.EndDisabledGroup();
				}
				else
				{
					if( ICEEditorLayout.DebugButton( "COPY", "Copies the settings from the selected entity to this one. Both entities must be from the same type!" ) )
					{
						string _warning = "Please note, this function copies the settings from the selected entity (" + QuickSelectGroup.Name + ") to this one (" + _entity.name + "). This overwrites all settings " +
							"of this entity. Are you sure you want to do that? \n\n" +
							"Press COPY to continue or CANCEL to abort.";

						if( EditorUtility.DisplayDialog( "Copy Message",  _warning, "COPY", "CANCEL" ) )
						{
							UnityEditorInternal.ComponentUtility.CopyComponent( QuickSelectGroup.EntityComponent );
							UnityEditorInternal.ComponentUtility.PasteComponentValues( _entity );
						}
					}
				}

				ICEEditorLayout.EndHorizontal( Info.ENTITY_QUICK_SELECTION );
			}
		}

		/// <summary>
		/// Draws the register popup.
		/// </summary>
		/// <returns>The register popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_index">Index.</param>
		public static int QuickSelectionPopup( string _title, int _index = 0 )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;

			if( _register == null )
			{
				EditorGUILayout.LabelField( _title );
				return 0;
			}
			else if( _register.ReferenceGroupObjects.Count == 0 )
			{
				EditorGUILayout.LabelField( _title );
				return 0;
			}
			else
			{
				List<ReferenceGroupObject> _groups = _register.ReferenceGroupObjects;

				string[] _names = new string[_groups.Count];

				if( _index > _groups.Count )
					_index = 0;

				for(int i=0;i < _groups.Count ;i++)
				{
					_names[i] = _groups[i].Name;
				}

				GUI.backgroundColor = ICEEditorLayout.DebugButtonColor;
				_index = EditorGUILayout.Popup( _title, _index, _names );
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
				return _index;

			}			
		}





		/// <summary>
		/// Behaviours the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_creature_control">Creature control.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_key">Key.</param>
		public static string BehaviourPopup( ICECreatureControl _creature_control, string _title, string _hint, string _key, ref bool _key_exists )
		{
			string _new_key = "";
			if( _creature_control.Creature.Behaviour.BehaviourModes.Count == 0 )
			{
				EditorGUILayout.LabelField( _title );
				return _new_key;
			}
			else
			{
				GUIContent[] _options = new GUIContent[ _creature_control.Creature.Behaviour.BehaviourModes.Count + 1];
				int _options_index = 0;

				_options[0] = new GUIContent( " ");
				for( int i = 0 ; i < _creature_control.Creature.Behaviour.BehaviourModes.Count ; i++ )
				{
					BehaviourModeObject _mode = _creature_control.Creature.Behaviour.BehaviourModes[i];

					int _index = i + 1;

					_options[ _index ] = new GUIContent( _mode.Key );

					if( _mode.Key == _key )
					{
						_options_index = _index;
					}
				}

				if( _options_index == 0 && ! string.IsNullOrEmpty( _key ) && _key.Trim() != "" )
				{
					GUIContent[] _new_options = new GUIContent[ _options.Length + 1];
					int _new_options_index = _options.Length;

					for( int i = 0 ; i < _options.Length ; i++ )
						_new_options[i] = new GUIContent( _options[i].text );

					_new_options[_new_options_index] = new GUIContent( _key );

					GUI.backgroundColor = Color.yellow;
					_new_options_index = EditorGUILayout.Popup( new GUIContent( _title, _hint), _new_options_index , _new_options  );
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

					_new_key = _new_options[_new_options_index].text;
					_key_exists = false;
				}
				else
				{
					_options_index = EditorGUILayout.Popup( new GUIContent( _title, _hint), _options_index , _options  );
					_new_key = _options[ _options_index ].text;
					_key_exists = true;
				}
			}

			return _new_key;
		}


		/// <summary>
		/// Behaviours the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_creature_control">Creature control.</param>
		/// <param name="_key">Key.</param>
		public static string BehaviourPopup( ICECreatureControl _creature_control, string _key )
		{
			string _new_key = "";
			if( _creature_control.Creature.Behaviour.BehaviourModes.Count == 0 )
			{
				EditorGUILayout.LabelField( "" );
				return _new_key;
			}
			else
			{
				GUIContent[] _options = new GUIContent[ _creature_control.Creature.Behaviour.BehaviourModes.Count + 1];
				int _options_index = 0;

				_options[0] = new GUIContent( " ");
				for( int i = 0 ; i < _creature_control.Creature.Behaviour.BehaviourModes.Count ; i++ )
				{
					BehaviourModeObject _mode = _creature_control.Creature.Behaviour.BehaviourModes[i];

					int _index = i + 1;

					_options[ _index ] = new GUIContent( _mode.Key );

					if( _mode.Key == _key )
					{
						_options_index = _index;
					}
				}


				_options_index = EditorGUILayout.Popup( _options_index , _options );

				_new_key = _options[ _options_index ].text;
			}

			return _new_key;
		}

		/// <summary>
		/// Targets the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_group">Group.</param>
		public static string TargetPopup( string _key ){
			return TargetPopup( "", "", _key, "" );
		}

		/// <summary>
		/// Targets the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_group">Group.</param>
		/// <param name="_help">Help.</param>
		public static string TargetPopup( string _title, string _hint, string _key, string _help = "" )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			List<string> _list = null;
			if( _register != null )
				_list = _register.ReferenceGameObjectNames;

			return ReferencePopup( _title, _hint, _key, _list, _help );
		}

		/// <summary>
		/// Items the popup.
		/// </summary>
		/// <returns>The popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_group">Group.</param>
		/// <param name="_help">Help.</param>
		public static string ItemPopup( string _title, string _hint, string _key, string _help = "" )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			List<string> _list = null;
			if( _register != null )
				_list = _register.ReferenceItemNames;

			return ReferencePopup( _title, _hint, _key, _list, _help );	
		}

		/// <summary>
		/// Players popup.
		/// </summary>
		/// <returns>Players popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_group">Group.</param>
		/// <param name="_help">Help.</param>
		public static string PlayerPopup( string _title, string _hint, string _key, string _help = "" )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			List<string> _list = null;
			if( _register != null )
				_list = _register.ReferencePlayerNames;

			return ReferencePopup( _title, _hint, _key, _list, _help );	
		}

		public static string ZonePopup( string _key ){
			return ZonePopup("", "", _key, "" );
		}
		/// <summary>
		/// Zones popup.
		/// </summary>
		/// <returns>Zones popup.</returns>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_key">Key.</param>
		/// <param name="_help">Help.</param>
		public static string ZonePopup( string _title, string _hint, string _key, string _help = "" )
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			List<string> _list = null;
			if( _register != null )
				_list = _register.ReferenceZoneNames;

			return ReferencePopup( _title, _hint, _key, _list, _help );	
		}

		public static string ReferencePopup( string _title, string _hint, string _key, List<string> _list, string _help = "" )
		{
			if( _list == null || _list.Count == 0 )
			{
				EditorGUILayout.LabelField( _title + " - " + _key + " (EMPTY LIST)"  );
				return _key;
			}
			else
			{
				_list.Insert(0, " " );

				string[] _names = new string[_list.Count];

				int _index = 0;
				for(int i=0;i < _list.Count ;i++)
				{
					if( _list[i] == " " && i == 0 )
						_names[i] = " ";
					else if( _list[i] == " " )
						_names[i] = "";
					else
						_names[i] = _list[i];

					if( _key == _names[i] )
						_index = i;
				}

				Color _org_color = GUI.backgroundColor;
				if( ! string.IsNullOrEmpty( _key ) && _index == 0 )
				{
					_names = SystemTools.AddArrayValue( _names, _key, ref _index );
					GUI.backgroundColor = Color.red;
				}
					
				if( _title.Trim() != "" )
					_index = ICEEditorLayout.Popup( _title, _hint, _index, _names, _help );
				else
					_index = EditorGUILayout.Popup( _index, _names );

				GUI.backgroundColor = _org_color;
				
				return _names[_index].Trim();
			}
	
		}



		/// <summary>
		/// Inventories the item popup.
		/// </summary>
		/// <returns>The item popup.</returns>
		/// <param name="_control">Control.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_name">Name.</param>
		/// <param name="_help">Help.</param>
		public static string InventoryItemPopup( ICECreatureControl _control, string _title, string _hint, string _name, string _help = "" )
		{
			if( _control == null )
				return "";

			InventoryDataObject _inventory = _control.Creature.Status.Inventory;

			if( _inventory == null || _inventory.Slots.Count == 0  )
			{
				EditorGUILayout.LabelField( _title + " (- no reference item - check register!)"  );
				return "";
			}
			else
			{
				List<string> _keys = _inventory.AvailableItems;

				_keys.Insert(0, " " );

				string[] _names = new string[_keys.Count];

				int _index = 0;
				for(int i=0;i < _keys.Count ;i++)
				{
					if( _keys[i] == " " && i == 0 )
						_names[i] = " ";
					else if( _keys[i] == " " )
						_names[i] = "";
					else
						_names[i] = _keys[i];

					if( _name == _names[i] )
						_index = i;

				}

				if( ICE.World.Utilities.SystemTools.FindChildByName( _name, _control.gameObject.transform ) != null )
					GUI.backgroundColor = Color.green;

				if( _title.Trim() != "" )
					_index = ICEEditorLayout.Popup( _title, _hint, _index, _names, _help );
				else
					_index = EditorGUILayout.Popup( _index, _names );

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				return _names[_index].Trim();

			}	
		}

	}
}
