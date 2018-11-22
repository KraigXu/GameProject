// ##############################################################################
//
// ICECreatureRangedWeaponEditor.cs
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
	[CustomEditor(typeof(ICECreatureRangedWeapon))]
	public class ICECreatureRangedWeaponEditor : ICECreatureWeaponEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureRangedWeapon _target = DrawEntityHeader<ICECreatureRangedWeapon>();
			DrawRangedWeaponContent( _target );
			DrawFooter( _target );
		}

		public virtual void DrawRangedWeaponContent( ICECreatureRangedWeapon _target )
		{
			if( _target == null )
				return;

			CreatureObjectEditor.DrawRangedWeaponObject( _target, _target.Weapon, m_HeaderType, Info.PRIMARY_WEAPON, "Primary Weapon" );
			CreatureObjectEditor.DrawRangedWeaponObject( _target, _target.SecondaryWeapon, m_HeaderType, Info.SECONDARY_WEAPON, "Secondary Weapon" );

			// WEAPON LASER BEAM
			CreatureObjectEditor.DrawLaserObject( _target, _target.Laser, m_HeaderType, Info.LASER );

			// WEAPON FLASHLIGHT
			CreatureObjectEditor.DrawFlashlightObject( _target, _target.Flashlight, m_HeaderType );

			// WEAPON SPECIFIC
			DrawWeaponContent( _target );
		}
	}
}
