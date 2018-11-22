// ##############################################################################
//
// ICECreatureRegisterEditor.cs
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
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorUtilities;
using ICE.Creatures.EditorInfos;

namespace ICE.Creatures
{

	[CustomEditor(typeof(ICECreatureRegister))]
	public class ICECreatureRegisterEditor : ICEWorldBehaviourEditor {

		private ICECreatureRegister m_creature_register;

		public bool m_foldout_register = true;
		public bool m_foldout_options = true;
		public bool m_foldout_environment = true;
	
		public virtual void OnEnable()
		{
			//m_creature_register = (ICECreatureRegister)target;
		}


		public override void OnInspectorGUI()
		{
			m_creature_register = (ICECreatureRegister)target;

			GUI.changed = false;
			Info.HelpButtonIndex = 0;

			EditorGUI.indentLevel++;
				RegisterOptionsEditor.Print( m_creature_register );					
				RegisterGroupsEditor.Print( m_creature_register );
			EditorGUI.indentLevel--;

			EditorGUILayout.Separator();
			MarkSceneDirty( m_creature_register );
		}
	}

}
