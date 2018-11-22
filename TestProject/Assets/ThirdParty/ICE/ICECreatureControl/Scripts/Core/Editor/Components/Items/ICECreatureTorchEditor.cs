// ##############################################################################
//
// ICECreatureMeleeWeaponEditor.cs
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
	[CustomEditor(typeof(ICECreatureTorch))]
	public class ICECreatureTorchEditor : ICECreatureItemEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureTorch _target = DrawEntityHeader<ICECreatureTorch>();
			DrawTorchContent( _target );
			DrawFooter( _target );
		}

		public virtual void DrawTorchContent( ICECreatureTorch _target )
		{
			if( _target == null )
				return;

			CreatureObjectEditor.DrawFireObject( _target, _target.Fire, EditorHeaderType.FOLDOUT_ENABLED_BOLD );
			DrawItemContent( _target );
		}
	}
}
