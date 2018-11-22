//
// ICECreatureLinkEditor.cs
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
	[CustomEditor(typeof(ICECreatureLink))]
	public class ICECreatureLinkEditor : ICECreatureLocationEditor {
		
		public override void OnInspectorGUI()
		{
			ICECreatureLink _target = DrawEntityHeader<ICECreatureLink>();
			DrawLinkContent( _target );
			DrawFooter( _target );
		}

		/// <summary>
		/// Draws the content of the zone entity.
		/// </summary>
		/// <param name="_target">Zone Entity.</param>
		public virtual void DrawLinkContent( ICECreatureLink _target )
		{
			if( _target == null )
				return;

			CreatureObjectEditor.DrawInfluenceDataObject( _target.Influences, m_HeaderType );
		}
	}
}
