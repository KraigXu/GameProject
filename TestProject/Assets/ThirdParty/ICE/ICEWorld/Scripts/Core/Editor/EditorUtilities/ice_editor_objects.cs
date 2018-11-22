// ##############################################################################
//
// ice_editor_objects.cs | ObjectEditor
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

namespace ICE.World.EditorUtilities
{
	public class ObjectEditor {

		/// <summary>
		/// Begins the object content or return.
		/// </summary>
		/// <returns><c>true</c>, if object content or return was begun, <c>false</c> otherwise.</returns>
		/// <param name="_type">Type.</param>
		/// <param name="_object">Object.</param>
		public static bool BeginObjectContentOrReturn( EditorHeaderType _type, bool _enabled, bool _foldout )
		{
			if( ( IsFoldoutType( _type ) && _foldout == false ) || ( ! IsFoldoutType( _type ) && _foldout == false ) ) 
				return true;

			EditorGUI.BeginDisabledGroup( _enabled == false );
			EditorGUI.indentLevel++;
			return false;
		}

		/// <summary>
		/// Begins the object content or return.
		/// </summary>
		/// <returns><c>true</c>, if object content or return was begun, <c>false</c> otherwise.</returns>
		/// <param name="_type">Type.</param>
		/// <param name="_object">Object.</param>
		public static bool BeginObjectContentOrReturn( EditorHeaderType _type, ICEDataObject _object )
		{
			if( _object == null )
				return true;

			return BeginObjectContentOrReturn( _type, _object.Enabled, _object.Foldout );
		}

		/// <summary>
		/// Ends the content of the object.
		/// </summary>
		public static void EndObjectContent()
		{
			EditorGUI.indentLevel--;
			EditorGUI.EndDisabledGroup();
			EditorGUILayout.Separator();
		}

		/// <summary>
		/// Determines if is header required the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is header required the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsHeaderRequired( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.NONE:
			case EditorHeaderType.FOLDOUT_CUSTOM:
			case EditorHeaderType.TOGGLE_CUSTOM:
			case EditorHeaderType.LABEL_CUSTOM:
				return false;
			default:
				return true;
			}
		}

		/// <summary>
		/// Determines if is foldout type the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is foldout type the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsFoldoutType( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT:
			case EditorHeaderType.FOLDOUT_BOLD:
			case EditorHeaderType.FOLDOUT_ENABLED:
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
			case EditorHeaderType.FOLDOUT_CUSTOM:
				return true;
			default:
				return false;
			}
		}

		/// <summary>
		/// Determines if is enabled foldout type the specified _type.
		/// </summary>
		/// <returns><c>true</c> if is enabled foldout type the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsEnabledFoldoutType( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT_ENABLED:
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
				return true;
			default:
				return false;
			}
		}

		/// <summary>
		/// Determines if the specified type requires a enabled button.
		/// </summary>
		/// <returns><c>true</c> if is enabled type the specified _type; otherwise, <c>false</c>.</returns>
		/// <param name="_type">Type.</param>
		public static bool IsEnabledType( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.TOGGLE:
			case EditorHeaderType.TOGGLE_CUSTOM:
			case EditorHeaderType.TOGGLE_LEFT:
			case EditorHeaderType.TOGGLE_LEFT_BOLD:
			case EditorHeaderType.FOLDOUT_ENABLED:
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
			case EditorHeaderType.LABEL_ENABLED:
			case EditorHeaderType.LABEL_ENABLED_BOLD:
				return true;
			default:
				return false;
			}
		}

		/// <summary>
		/// Gets the simple foldout.
		/// </summary>
		/// <returns>The simple foldout.</returns>
		/// <param name="_type">Type.</param>
		public static EditorHeaderType GetSimpleFoldout( EditorHeaderType _type )
		{
			switch( _type )
			{
			case EditorHeaderType.FOLDOUT_ENABLED_BOLD:
				return EditorHeaderType.FOLDOUT_BOLD;
			case EditorHeaderType.FOLDOUT_ENABLED:
				return EditorHeaderType.FOLDOUT;
			default:
				return _type;
			}
		}


		/// <summary>
		/// Draws the object header.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawObjectHeader( ICEDataObject _object, EditorHeaderType _type, string _title, string _hint = "", string _help = "" )
		{
			if( _object == null || IsHeaderRequired( _type ) == false )
				return;

			if( ! IsEnabledType( _type ) )
				_object.Enabled = true;				

			ICEEditorLayout.BeginHorizontal();
			DrawObjectHeaderLine( _object, _type, _title, _hint );
			ICEEditorLayout.EndHorizontal( _help );
		}

		/// <summary>
		/// Draws the object header line.
		/// </summary>
		/// <param name="_object">Object.</param>
		/// <param name="_type">Type.</param>
		/// <param name="_bold">If set to <c>true</c> bold.</param>
		/// <param name="_title">Title.</param>
		/// <param name="_hint">Hint.</param>
		/// <param name="_help">Help.</param>
		public static void DrawObjectHeaderLine( ICEDataObject _object, EditorHeaderType _type, string _title, string _hint, string _help = "" )
		{
			if( _object == null )
				return;

			DrawObjectHeaderLine( ref _object.Enabled, ref _object.Foldout, _type, _title, _hint, _help );
		}

		public static bool DrawAddButton<T>( ICEDataObject _object, EditorHeaderType _type, List<T> _list ) where T : new()
		{
			if( _object == null || _list == null )
				return false;

			if( ICEEditorLayout.AddButtonSmall( "" ) )
			{
				_list.Add( ICEObjectFactory.Create<T>() );

				_object.Enabled = true;
				_object.Foldout = true;
				return true;
			}
			return false;
		}

		public static bool DrawClearButton<T>( ICEDataObject _object, List<T> _list ) where T : ICEObject
		{
			if( _object == null || _list == null )
				return false;

			if( ICEEditorLayout.ClearButtonSmall( "" ) )
			{
				_list.Clear();
				_object.Enabled = false;
				_object.Foldout = false;
				return true;
			}

			return false;
		}

		public static bool DrawClearButton( ICEDataObject _object, List<ICEObject> _list )
		{
			if( _object == null || _list == null )
				return false;

			if( ICEEditorLayout.ClearButtonSmall( "" ) )
			{
				_list.Clear();
				_object.Enabled = false;
				_object.Foldout = false;
				return true;
			}
			return false;
		}

		public static void DrawEnabledButton<T>( ICEDataObject _object, EditorHeaderType _type, List<T> _list ) where T : new()
		{
			if( _object == null || _list == null )
				return;
			
			if( IsEnabledType( _type ) )
			{
				bool _enabled = ICEEditorLayout.EnableButton( _object.Enabled );

				// if enabled was canged to true we will prepare some thinks to optimize the handling for the user
				if( _enabled && _enabled != _object.Enabled )
				{
					// if the list is empty we will insert a first item 
					if( _list.Count == 0 )
						_list.Add( ICEObjectFactory.Create<T>() );
							
					// we foldout the parent object 
					_object.Foldout = true;
				}

				// update the enabled status of the parent object 
				_object.Enabled = _enabled;
			}
		}

		public static void DrawEnabledButton( ICEDataObject _object, EditorHeaderType _type )
		{
			if( IsEnabledType( _type ) )
			{
				bool _enabled = ICEEditorLayout.EnableButton( _object.Enabled );

				if( _enabled && _enabled != _object.Enabled )
					_object.Foldout = true;

				_object.Enabled = _enabled;
			}
		}

		public static void DrawObjectHeaderLine( ref bool _enabled, ref bool _foldout, EditorHeaderType _type, string _title, string _hint, string _help = "" )
		{
			if( IsHeaderRequired( _type ) == false )
				return;

			// TOOGLE
			if( _type == EditorHeaderType.TOGGLE )
			{
				_enabled = ICEEditorLayout.Toggle( _title, _hint, _enabled, _help );
				_foldout = _enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT )
			{
				_enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _enabled, false, _help );	
				_foldout = _enabled;
			}
			else if( _type == EditorHeaderType.TOGGLE_LEFT_BOLD )
			{
				_enabled = ICEEditorLayout.ToggleLeft( _title, _hint, _enabled, true, _help );
				_foldout = _enabled;
			}

			// LABEL
			else if( _type == EditorHeaderType.LABEL )
			{
				ICEEditorLayout.Label( _title, false, _help );
				_foldout = true;
				_enabled = true;
			}
			else if( _type == EditorHeaderType.LABEL_BOLD )
			{
				ICEEditorLayout.Label( _title, true, _help );
				_foldout = true;
				_enabled = true;
			}
			else if( _type == EditorHeaderType.LABEL_ENABLED || _type == EditorHeaderType.LABEL_ENABLED_BOLD )
			{
				EditorGUI.BeginDisabledGroup( _enabled == false );
				if( _type == EditorHeaderType.LABEL_ENABLED_BOLD )
					ICEEditorLayout.Label( _title, true, _help );
				else
					ICEEditorLayout.Label( _title, false, _help );
				_foldout = true;
				EditorGUI.EndDisabledGroup();

				_enabled = ICEEditorLayout.EnableButton( "Enables/disables this feature", _enabled );
			}

			// FOLDOUT
			else if( _type == EditorHeaderType.FOLDOUT )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, false );
				//EditorGUI.EndDisabledGroup();

			}
			else if( _type == EditorHeaderType.FOLDOUT_BOLD )
			{
				//EditorGUI.BeginDisabledGroup( _object.Enabled == false );
				_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, true );
				//EditorGUI.EndDisabledGroup();
			}
			else 
			{
				bool _enabled_in = _enabled;

				EditorGUI.BeginDisabledGroup( _enabled == false );
				if( _type == EditorHeaderType.FOLDOUT_ENABLED_BOLD )
					_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, true );
				else
					_foldout = ICEEditorLayout.Foldout( _foldout, _title, _help, false );
				EditorGUI.EndDisabledGroup();

				_enabled = ICEEditorLayout.EnableButton( _enabled );

				// Auto foldout if the feature was enabled by the user
				if( _enabled_in != _enabled && _enabled == true )
					_foldout = true;
			}
		}
	}
}
