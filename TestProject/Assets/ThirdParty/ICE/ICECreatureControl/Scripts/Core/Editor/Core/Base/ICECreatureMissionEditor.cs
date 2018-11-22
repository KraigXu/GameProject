// ##############################################################################
//
// ICECreatureMissionEditor.cs
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

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures
{
	[CustomEditor(typeof(ICECreatureMission))]
	public class ICECreatureMissionEditor : ICEWorldBehaviourEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICECreatureMission _target = DrawMissionHeader<ICECreatureMission>();

			TargetEditor.DrawTargetObject( _target.CreatureControl, _target.Target, "", "" );
			TargetEditor.DrawTargetContent( _target.CreatureControl, _target.Target );

			DrawMissionFooter( _target );
		}

		/// <summary>
		/// Draws the header.
		/// </summary>
		/// <returns>The header.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public virtual T DrawMissionHeader<T>() where T : ICECreatureMission
		{
			m_HeaderType = EditorHeaderType.FOLDOUT_ENABLED_BOLD;
			ICEEditorLayout.SetDefaults();
			T _target = (T)target;

			GUI.changed = false;
			Info.Reset( _target );

			EditorGUILayout.Separator();

			// indentLevel will be decreased in the footer, so we don't need to do it here
			EditorGUI.indentLevel++;

			return _target;
		}

		/// <summary>
		/// Draws the footer.
		/// </summary>
		/// <param name="_target">Target.</param>
		public virtual void DrawMissionFooter( ICECreatureMission _entity )
		{
			if( _entity == null )
				return;

			EditorGUILayout.Separator();


			// indentLevel was increased in the header, so we have to decrease the level here
			EditorGUI.indentLevel--;

			// Version Info
			EditorGUILayout.LabelField( " - " + _entity.GetType().ToString() + " v" + Info.Version + " - ", EditorStyles.centeredGreyMiniLabel );

			MarkSceneDirty( _entity );

		}
	}
}
	
