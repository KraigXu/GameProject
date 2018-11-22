// ##############################################################################
//
// ICECreaturePlayerUIEditor.cs
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
using UnityEngine.UI;
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

namespace ICE.Creatures.UI
{
	[CustomEditor(typeof(ICECreaturePlayerUI))]
	public class ICECreaturePlayerUIEditor : ICEWorldEntityStatusUIEditor 
	{
		/// <summary>
		/// Raises the inspector GUI event.
		/// </summary>
		public override void OnInspectorGUI()
		{
			ICECreaturePlayerUI _target = DrawEntityStatusUIHeader<ICECreaturePlayerUI>();
			DrawPlayerUIContent( _target );
			DrawFooter( _target );
		}
			

		protected virtual void DrawPlayerUIContent( ICECreaturePlayerUI _player )
		{
			if( _player == null )
				return;

			_player.UseDisplayEntityInfos = ICEEditorLayout.Toggle( "Display World Information", "", _player.UseDisplayEntityInfos );
			_player.HealthBar = (Image)EditorGUILayout.ObjectField( "Health Bar", _player.HealthBar, typeof(Image), true );
			_player.InventorySlotBar = (Image)EditorGUILayout.ObjectField( "Inventory Slot Bar ", _player.InventorySlotBar, typeof(Image), true );

			_player.DamageIndicator = (Image)EditorGUILayout.ObjectField( "Damage Indicator", _player.DamageIndicator, typeof(Image), true );
			//CreatureObjectEditor.DrawEntityStatusObject( _entity, _entity.Status, m_HeaderType );
		}
	}
}
	
