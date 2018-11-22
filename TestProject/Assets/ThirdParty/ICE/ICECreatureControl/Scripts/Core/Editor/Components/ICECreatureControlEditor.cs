// ##############################################################################
//
// ice_CreatureControlEditor.cs
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

using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures
{

	[CustomEditor(typeof(ICECreatureControl))]
	public class ICECreatureControlEditor : ICECreatureOrganismEditor
	{
		/*
		Color _color_default_background = GUI.backgroundColor;
		Color _color_add = new Color(0.0F, 0.75F, 1.0F, 1);
		Color _color_delete = new Color(1.0F, 0.5F, 0.5F, 1);
		Color _color_global = new Color(0.0F, 0.75F, 1.0F, 1);*/


		//********************************************************************************
		// INITIAL DECLARATION (EDITOR)
		//********************************************************************************
		private static ICECreatureControl m_creature_control;
		//private static ICECreatureRegister m_creature_register;
		private static ICECreatureControlDebug m_creature_debug;

		//********************************************************************************
		// OnEnable
		//********************************************************************************
		public virtual void OnEnable()
		{
			m_creature_control = (ICECreatureControl)target;
			//m_creature_register = FindObjectOfType<ICECreatureRegister>();

			if( m_creature_control != null )
				m_creature_debug = m_creature_control.gameObject.GetComponent<ICECreatureControlDebug>();

		}

		//********************************************************************************
		// OnInspectorGUI
		//********************************************************************************
		public override void OnInspectorGUI()
		{
			Info.Reset( m_creature_control );
		
			if( m_creature_debug != null )
				m_creature_control.Display.ShowDebug = m_creature_debug.enabled;
			else
				m_creature_control.Display.ShowDebug = false;

			GUI.changed = false;

			EditorGUILayout.Separator();

			// COCKPIT
			DisplayEditor.Print( m_creature_control );
			InfoEditor.Print( m_creature_control );

			// WIZARD
			//EditorWizard.Print( m_creature_control );

			// ESSENTIALS
			EssentialsEditor.Print( m_creature_control );

			// STATUS
			StatusEditor.Print( m_creature_control );

			// MISSIONS
			MissionsEditor.Print( m_creature_control );

			// INTERACTION
			InteractionEditor.Print( m_creature_control );

			// ENVIRONMENT
			EnvironmentEditor.Print( m_creature_control );

			//BEHAVIOURS
			BehaviourEditor.Print( m_creature_control );

			EditorGUILayout.LabelField( " - ICECreatureControl v" + Info.Version + " - ", EditorStyles.centeredGreyMiniLabel );

			if( m_creature_control.Display.ShowDebug )
			{
				if( m_creature_debug == null )
					m_creature_debug = m_creature_control.gameObject.AddComponent<ICECreatureControlDebug>();
				else if( m_creature_debug.enabled == false )
					m_creature_debug.enabled = true;

			}
			else if( m_creature_debug != null )
			{
				m_creature_debug.enabled = false;
				/*
				DestroyImmediate( m_creature_control.GetComponent<ICECreatureControlDebug>() );
				EditorGUIUtility.ExitGUI();*/

			}

			MarkSceneDirty( m_creature_control );
		}
	}
}
