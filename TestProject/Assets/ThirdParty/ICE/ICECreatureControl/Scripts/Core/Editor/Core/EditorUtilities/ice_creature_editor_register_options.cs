// ##############################################################################
//
// ice_creature_editor_register_options.cs | RegisterOptionsEditor
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
	public static class RegisterOptionsEditor
	{
		private static bool m_foldout = true;
		public static void Print( ICECreatureRegister _register )
		{
			EditorGUILayout.Separator();

			m_foldout =  ICEEditorLayout.Foldout( m_foldout, "Options", Info.REGISTER_OPTIONS );
			if( ! m_foldout )
				return;

			EditorGUILayout.Separator();
			EditorGUI.indentLevel++;

				_register.Options.Enabled = true;
				CreatureObjectEditor.DrawRegisterDefaultSettings( _register, _register.Options, EditorHeaderType.FOLDOUT_BOLD );
				CreatureObjectEditor.DrawHierarchyManagementObject( _register, _register.HierarchyManagement, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
				CreatureObjectEditor.DrawRegisterDebugObject( _register, _register.RegisterDebug, EditorHeaderType.FOLDOUT_ENABLED_BOLD );

				EditorGUILayout.Separator();
				_register.UseDontDestroyOnLoad = ICEEditorLayout.Toggle( "Don't Destroy On Load", "", _register.UseDontDestroyOnLoad, Info.REGISTER_OPTIONS_DONTDESTROYONLOAD );
				_register.UsePoolManagementCoroutine = ICEEditorLayout.Toggle( "Use Pool Management Coroutine", "Use coroutine for pool management", _register.UsePoolManagementCoroutine, Info.REGISTER_OPTIONS_POOL_MANAGEMENT_COROUTINE );
				_register.UseGarbageCollection = ICEEditorLayout.Toggle( "Custom Garbage Collection", "", _register.UseGarbageCollection, Info.REGISTER_OPTIONS_CUSTOM_GARBAGE_COLLECTION );
				if( _register.UseGarbageCollection )
				{
					EditorGUI.indentLevel++;
						_register.GarbageCollectionInterval = ICEEditorLayout.DefaultSlider( "Interval", "", _register.GarbageCollectionInterval, Init.DECIMAL_PRECISION_TIMER, 0, 10, 3, "" );
					EditorGUI.indentLevel--;
				}

				_register.RandomSeed = (RandomSeedType)ICEEditorLayout.EnumPopup( "Random Seed","Sets the seed for the random number generator.", _register.RandomSeed, Info.REGISTER_OPTIONS_RANDOMSEED );
				if( _register.RandomSeed == RandomSeedType.CUSTOM )
				{
					EditorGUI.indentLevel++;
					_register.CustomRandomSeed = ICEEditorLayout.IntField( "Seed Value","Custom RandomSeed Integer Value", _register.CustomRandomSeed, Info.REGISTER_OPTIONS_RANDOMSEED_CUSTOM  );
					EditorGUI.indentLevel--;
					EditorGUILayout.Separator();
				}

			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
			ICEEditorStyle.SplitterByIndent(0);
		}
	}
}
