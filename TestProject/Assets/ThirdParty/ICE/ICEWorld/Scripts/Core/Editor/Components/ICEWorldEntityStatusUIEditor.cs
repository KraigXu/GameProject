// ##############################################################################
//
// ICEWorldEntityStatusUIEditor.cs
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

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EditorUtilities;
using ICE.World.EditorInfos;

namespace ICE.World
{
	[CustomEditor(typeof(ICEWorldEntityStatusUI))]
	public class ICEWorldEntityStatusUIEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICEWorldEntityStatusUI _target = DrawEntityStatusUIHeader<ICEWorldEntityStatusUI>();

			DrawFooter( _target );
		}

		/// <summary>
		/// Draws the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawEntityStatusUIHeader<T>() where T : ICEWorldEntityStatusUI
		{
			m_HeaderType = EditorHeaderType.FOLDOUT_ENABLED_BOLD;
			ICEEditorLayout.SetDefaults();
			T _target = (T)target;

			GUI.changed = false;
			Info.Reset( _target );

			// TARGET DEFAULTS
			DrawEntityStatusUISettings( _target );

			EditorGUILayout.Separator();

			// indentLevel will be decreased in the footer, so we don't need to do it here
			EditorGUI.indentLevel++;

			DrawEntityStatusUIContent( _target );
			return _target;
		}

		/// <summary>
		/// Draws the footer.
		/// </summary>
		/// <param name="_target">Target.</param>
		public virtual void DrawFooter( ICEWorldEntityStatusUI _ui )
		{
			if( _ui == null )
				return;

			EditorGUILayout.Separator();

			// indentLevel was increased in the header, so we have to decrease the level here
			EditorGUI.indentLevel--;

			// Version Info
			EditorGUILayout.LabelField( " - " + _ui.GetType().ToString() + " v" + Info.Version + " - ", EditorStyles.centeredGreyMiniLabel );

			MarkSceneDirty( _ui );
		}


		/// <summary>
		/// Draws the entity settings.
		/// </summary>
		/// <param name="_entity">Entity.</param>
		public static void DrawEntityStatusUISettings( ICEWorldEntityStatusUI _ui )
		{
			if( _ui == null )
				return;

			EditorGUILayout.Separator();

			ICEEditorLayout.BeginHorizontal();

			ICEEditorLayout.PrefixLabel( "Debug Options" );

			GUILayout.FlexibleSpace();
			/*
			_ui.UseDebugLogs = ICEEditorLayout.DebugButtonSmall( "LOG", "Print debug information for this GameObject", _ui.UseDebugLogs );
			EditorGUI.BeginDisabledGroup( _ui.UseDebugLogs == false );
			if( _ui.UseDebugLogs )
				_ui.UseDebugLogsSelectedOnly = ICEEditorLayout.DebugButtonMini( "S", "Shows debug information for selected GameObjects only.", _ui.UseDebugLogsSelectedOnly );
			else
				ICEEditorLayout.DebugButtonMini( "S", "Shows debug information for selected GameObjects only.", false );
			EditorGUI.EndDisabledGroup();
			GUILayout.Space( 3 );

			_ui.UseDebugRays = ICEEditorLayout.DebugButtonSmall( "RAY", "Shows debug rays for this GameObject", _ui.UseDebugRays  );
			EditorGUI.BeginDisabledGroup( _ui.UseDebugRays == false );
			if( _ui.UseDebugRays )
				_ui.UseDebugRaysSelectedOnly = ICEEditorLayout.DebugButtonMini( "S", "Shows debug rays for selected GameObjects only.", _ui.UseDebugRaysSelectedOnly );
			else
				ICEEditorLayout.DebugButtonMini( "S", "Shows debug rays for selected GameObjects only.", false );
			EditorGUI.EndDisabledGroup();
			GUILayout.Space( 3 );

			_ui.UseDebug = ICEEditorLayout.DebugButton( "DEBUG", "Enables enhanced debug options", _ui.UseDebug );		
			GUILayout.Space( 3 );*/

			EditorGUI.BeginDisabledGroup( _ui as ICEWorldBehaviour == null );
			_ui.ShowInfo = ICEEditorLayout.DebugButton( "INFO", "Displays runtime information.", _ui.ShowInfo );
			EditorGUI.EndDisabledGroup();
			GUILayout.Space( 3 );

			_ui.ShowHelp = ICEEditorLayout.DebugButton( "HELP", "Displays all help informations", _ui.ShowHelp );
			_ui.ShowNotes = ICEEditorLayout.DebugButton( "NOTES", "Displays all note fields", _ui.ShowNotes );
			ICEEditorLayout.EndHorizontal();

		}

		protected virtual void DrawEntityStatusUIContent( ICEWorldEntityStatusUI _ui )
		{
			if( _ui == null )
				return;

		
			WorldObjectEditor.DrawEntityStatusDisplayObject( _ui, _ui.StatusUI, m_HeaderType );
		}
	}
}

