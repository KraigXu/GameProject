// ##############################################################################
//
// ICECreatureWaypointEditor.cs
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
	[CustomEditor(typeof(ICECreatureWaypoint))]
	public class ICECreatureWaypointEditor : ICECreatureMarkerEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureWaypoint _target = DrawEntityHeader<ICECreatureWaypoint>();
			DrawWaypointContent( _target );
			DrawFooter( _target );

		}

		public virtual void DrawWaypointContent( ICECreatureWaypoint _target )
		{
			if( _target == null )
				return;

			CreatureObjectEditor.DrawWaypointLinksObject( _target, _target.Links, m_HeaderType );
		}
	}
}

