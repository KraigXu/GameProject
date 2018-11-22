// ##############################################################################
//
// ICECreatureBodyPartEditor.cs
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
	[CustomEditor(typeof(ICECreatureBodyPart)), CanEditMultipleObjects]
	public class ICECreatureBodyPartEditor : ICECreatureObjectEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureBodyPart _target = DrawEntityHeader<ICECreatureBodyPart>();

			DrawBodyPartContent( _target );
			DrawFooter( _target );
		}

		protected override void DrawEntityContent( ICECreatureEntity _entity )
		{
			if( _entity == null )
				return;

			if( _entity.IsRootEntity || _entity.Status.IsDestructible || _entity.Status.UseLifespan )
			{
				string _text = "Please note, basically a body part should be a child within the transform hierarchy of an higher entity " +
					"and you should enable the damage transfer multiplier to forward incomming damages to the parent according to the" +
					"specified value, so you could disable and ignore Lifespan and Durability. But in some cases it could be also useful " +
					"to use Lifespan and Durability for body parts to force specific effects but please consider that destroying a body " +
					"part within a given transform hierarchy could result uncomely and unwanted effects, so please be careful what you do!";
				EditorGUILayout.HelpBox( _text , MessageType.None );
			}

			CreatureObjectEditor.DrawEntityStatusObject( _entity, _entity.Status, m_HeaderType );
		}

		public virtual void DrawBodyPartContent( ICECreatureBodyPart _target )
		{
			if( _target == null )
				return;
			
			CreatureObjectEditor.DrawImpactObject( _target, _target.Impact, m_HeaderType );
			CreatureObjectEditor.DrawBodyPartMotionObject( _target, _target.Motion, m_HeaderType );
		}
	}
}
