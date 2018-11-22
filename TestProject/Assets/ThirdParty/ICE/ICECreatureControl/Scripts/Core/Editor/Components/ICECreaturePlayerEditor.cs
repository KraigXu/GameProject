// ##############################################################################
//
// ICECreaturePlayerEditor.cs
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
	[CustomEditor(typeof(ICECreaturePlayer)), CanEditMultipleObjects]
	public class ICECreaturePlayerEditor : ICECreatureOrganismEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreaturePlayer _target = DrawEntityHeader<ICECreaturePlayer>();
			DrawPlayerContent( _target );
			DrawFooter( _target );
		}

		public virtual void DrawPlayerContent( ICECreaturePlayer _target )
		{
			if( _target == null )
				return;

			CreatureObjectEditor.DrawPlayerObject( _target.gameObject, _target.Player, m_HeaderType );
			CreatureObjectEditor.DrawPlayerInventoryObject( _target.gameObject, _target.Inventory, m_HeaderType );
			CreatureObjectEditor.DrawPlayerInputEventsObject( _target, _target.Events, m_HeaderType );
		}
	}
}
