// ##############################################################################
//
// ice_editor_info.cs | ICEEditorInfo
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

namespace ICE.World.EditorUtilities
{
	/// <summary>
	/// ICE editor info.
	/// </summary>
	public class ICEEditorInfo
	{
		public static bool HelpEnabled = false;
		public static bool NotesEnabled = false;

		public static void Reset( ICEWorldBehaviour _behaviour )
		{
			HelpButtonIndex = 0;

			HelpEnabled = _behaviour.ShowHelp;
			NotesEnabled = _behaviour.ShowNotes;

			ICEEditorLayout.SetColors( true );
		
		}

		public static void Desc( string _text )
		{
			if( _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Help( string _text )
		{
			if( HelpEnabled && _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Note( string _text )
		{
			EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static void Warning( string _text )
		{
			EditorGUILayout.HelpBox( _text , MessageType.Warning); 
		}

		public static int HelpButtonIndex = 0;
		public static bool[] HelpFlag = new bool[1000];
		public static void HelpButton()
		{
			EditorGUI.BeginDisabledGroup( HelpEnabled == true );
				HelpButtonIndex++;

				if( HelpFlag.Length < HelpButtonIndex && HelpFlag[HelpButtonIndex] == true )
					GUI.backgroundColor = Color.yellow;
				else
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				if( ICEEditorLayout.Button( "?", "", ICEEditorStyle.InfoButton ))
					HelpFlag[HelpButtonIndex] = ! HelpFlag[HelpButtonIndex];

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();
		}

		public static void HelpButton( Rect _rect )
		{
			if( HelpEnabled == true )
				return;

			HelpButtonIndex++;

			if( HelpFlag.Length > HelpButtonIndex && HelpFlag[HelpButtonIndex] == true )
				GUI.backgroundColor = Color.yellow;
			else
				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

			if( GUI.Button( _rect, new GUIContent( "?", "Show the help text for this feature."), ICEEditorStyle.InfoButton ) )
				HelpFlag[HelpButtonIndex] = ! HelpFlag[HelpButtonIndex];

			GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
		}

		public static void ShowHelp( string _text )
		{
			if( ( HelpEnabled || ( HelpFlag.Length > HelpButtonIndex && HelpFlag[HelpButtonIndex] ) ) && _text != "" )
				EditorGUILayout.HelpBox( _text , MessageType.None); 
		}

		public static bool InfoButton( bool _value, string _info )
		{
			EditorGUI.BeginDisabledGroup( NotesEnabled == true );
					
				if( _value && NotesEnabled == false )
					GUI.backgroundColor = Color.yellow;
				else if( _info.Trim() != "" )
					GUI.backgroundColor = Color.magenta;
				else
					GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;

				if( ICEEditorLayout.Button( "i", "Show the note editor for this feature.", ICEEditorStyle.InfoButton ))
					_value = ! _value; 

				GUI.backgroundColor = ICEEditorLayout.DefaultBackgroundColor;
			EditorGUI.EndDisabledGroup();

			return _value;
		}

		public static string InfoText( bool _show, string _info )
		{
			if( NotesEnabled || _show )
			{
				EditorStyles.textField.wordWrap = true;
				_info = EditorGUILayout.TextArea( _info );
			}

			return _info;
		}
	}
}