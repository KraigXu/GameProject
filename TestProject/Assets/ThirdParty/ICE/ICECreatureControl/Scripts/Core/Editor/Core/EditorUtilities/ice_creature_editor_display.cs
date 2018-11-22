// ##############################################################################
//
// ice_creature_editor_display.cs | DisplayEditor
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
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures.EditorUtilities
{
	public static class DisplayEditor
	{
		public static void Print( ICECreatureControl _control )
		{
			string _global_info = "";

			//ICEEditorLayout.SetColors( _control.Display.ShowOptions );

			if( _control.Display.UseGlobalAll )
				_global_info = " - all global";
			else if( _control.Display.UseGlobal )
				_global_info = " - global";
			
			ICEEditorLayout.BeginHorizontal();
			EditorGUILayout.LabelField( _control.gameObject.name + " (" + _control.gameObject.GetInstanceID() + ")" + _global_info, ICEEditorStyle.LabelBold );

				if( ICEEditorLayout.SaveButton( "" ) )
					CreatureEditorIO.SaveCreatureToFile( _control.Creature, _control.gameObject.name );				
				if( ICEEditorLayout.LoadButton( "" ) )
					_control.Creature = CreatureEditorIO.LoadCreatureFromFile( _control.Creature );				
				if( ICEEditorLayout.ResetButton( "" ) )
					_control.Creature = new CreatureObject();

				GUILayout.Space( 5 );
				_control.Display.UseGlobalAll = ICEEditorLayout.DebugButton( "GLOBAL", "Use the current display options for all your creatures" , _control.Display.UseGlobalAll );
				//_control.Display.ShowOptions = ICEEditorLayout.DebugButton( "OPTIONS", "Use colored button to visualize the related functionality." , _control.Display.ShowOptions );

			ICEEditorLayout.EndHorizontal( Info.CREATURE_PRESETS );
			EditorGUILayout.Separator();

			if( _control.Display.ShowOptions )
			{
				
			}

			_control.UseDebug = _control.Display.ShowDebug; 

			// TARGET DEFAULTS
			ICECreatureEntityEditor.DrawEntitySettings( _control );
			EditorGUILayout.Separator();

			_control.Display.ShowDebug = _control.UseDebug; 

			ICEEditorLayout.BeginHorizontal();

				string[] _tabs = new string[] {"ESSENTIALS", "STATUS", "MISSIONS", "INTERACTION", "ENVIRONMENT", "BEHAVIOURS" };
				int _tab_index = 0;
				if( _control.Display.ShowEssentials ) _tab_index = 0;
				else if( _control.Display.ShowStatus ) _tab_index = 1;
				else if( _control.Display.ShowMissions ) _tab_index = 2;
				else if( _control.Display.ShowInteraction ) _tab_index = 3;
				else if( _control.Display.ShowEnvironment ) _tab_index = 4;
				else if( _control.Display.ShowBehaviour ) _tab_index = 5;

				_tab_index = GUILayout.SelectionGrid ( _tab_index, _tabs, 3);

				_control.Display.ShowEssentials = ( _tab_index == 0 ?true:false );
				_control.Display.ShowStatus = ( _tab_index == 1 ?true:false );
				_control.Display.ShowMissions = ( _tab_index == 2 ?true:false );
				_control.Display.ShowInteraction = ( _tab_index == 3 ?true:false );
				_control.Display.ShowEnvironment = ( _tab_index == 4 ?true:false );
				_control.Display.ShowBehaviour = ( _tab_index == 5 ?true:false );

			ICEEditorLayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
}
