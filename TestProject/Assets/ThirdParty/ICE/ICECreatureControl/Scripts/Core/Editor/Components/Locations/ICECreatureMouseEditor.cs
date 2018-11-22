//
// ICECreatureMouseEditor.cs
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
	[CustomEditor(typeof(ICECreatureMouse))]
	public class ICECreatureMouseEditor : ICECreatureLocationEditor {

		public override void OnInspectorGUI()
		{
			ICECreatureMouse _target = DrawEntityHeader<ICECreatureMouse>();
			DrawMouseContent( _target );
			DrawFooter( _target );

		}

		public virtual void DrawMouseContent( ICECreatureMouse _target )
		{
			if( _target == null )
				return;

			_target.UseMoveOnButtonDown = ICEEditorLayout.Toggle( "Move On Button Down", "", _target.UseMoveOnButtonDown , "" );
			_target.SurfaceOffset = ICEEditorLayout.DefaultSlider( "Surface Offset", "", _target.SurfaceOffset, 0.01f,0, 10,0, "" ); 
		}
	}
}
