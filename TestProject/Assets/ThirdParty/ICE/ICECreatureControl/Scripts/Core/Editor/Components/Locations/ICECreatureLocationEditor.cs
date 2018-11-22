// ##############################################################################
//
// ICECreatureLocationEditor.cs
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
	[CustomEditor(typeof(ICECreatureLocation))]
	public class ICECreatureLocationEditor : ICECreatureEntityEditor 
	{
		public override void OnInspectorGUI()
		{
			ICECreatureLocation _target = DrawEntityHeader<ICECreatureLocation>();
			DrawLocationContent( _target );
			DrawFooter( _target );
					
		}

		public virtual void DrawLocationContent( ICECreatureLocation _target )
		{
			if( _target == null )
				return;

		}
	}
}
