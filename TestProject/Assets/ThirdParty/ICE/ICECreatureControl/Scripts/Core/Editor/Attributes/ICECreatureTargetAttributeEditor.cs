// ##############################################################################
//
// ICECreatureTargetAttributeEditor.cs
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

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;



namespace ICE.Creatures.Attributes
{
	[CustomEditor(typeof(ICECreatureTargetAttribute))]
	public class ICECreatureTargetAttributeEditor : ICECreatureAttributeEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureTargetAttribute _attribute = DrawEntityHeader<ICECreatureTargetAttribute>();
			DrawTargetContent( _attribute );
			DrawFooter( _attribute );
		}

		/// <summary>
		/// Draws the content of the target.
		/// </summary>
		/// <param name="_attribute">Attribute.</param>
		public void DrawTargetContent( ICECreatureTargetAttribute _attribute )
		{
			TargetType _type = TargetType.UNDEFINED;
			if( _attribute.GetComponentInChildren<ICECreaturePlayer>() != null )
				_type = TargetType.PLAYER;
			else if( _attribute.GetComponentInChildren<ICECreatureItem>() != null )
				_type = TargetType.ITEM;
			else if( _attribute.GetComponentInChildren<ICECreatureLocation>() != null )
				_type = TargetType.WAYPOINT;
			else if( _attribute.GetComponentInChildren<ICECreatureWaypoint>() != null )
				_type = TargetType.WAYPOINT;
			else if( _attribute.GetComponentInChildren<ICECreatureMarker>() != null )
				_type = TargetType.WAYPOINT;
			else if( _attribute.GetComponentInChildren<ICECreatureControl>() != null )
				_type = TargetType.CREATURE;

			ICEEditorLayout.Label( "Default Target Settings", true );
			EditorGUI.indentLevel++;
			CreatureObjectEditor.DrawTargetSelectorsObject( null, _attribute.Target, _attribute.Target.Selectors, _type, 0, 250 );
			TargetEditor.DrawTargetMoveSpecification( null, _attribute.Target );
			EditorGUI.indentLevel--;
		}

	}
}
