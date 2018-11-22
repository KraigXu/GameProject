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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;
using ICE.World.EditorUtilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

using ICE.Creatures.EditorInfos;
using ICE.Creatures.EditorUtilities;

namespace ICE.Creatures.Attributes
{
	[CustomEditor(typeof(ICECreatureOdourAttribute))]
	public class ICECreatureOdourAttributeEditor : ICECreatureAttributeEditor {

		public override void OnInspectorGUI()
		{
			ICECreatureOdourAttribute _attribute = DrawEntityHeader<ICECreatureOdourAttribute>();
			DrawOdourContent( _attribute );
			DrawFooter( _attribute );
		}

		/// <summary>
		/// Draws the content of the odour attribute.
		/// </summary>
		/// <param name="_attribute">Attribute.</param>
		public void DrawOdourContent( ICECreatureOdourAttribute _attribute )
		{
			_attribute.Odour = (OdourType)ICEEditorLayout.EnumPopup("Odour","", _attribute.Odour);			
			if( _attribute.Odour != OdourType.NONE )
			{					
				EditorGUI.indentLevel++;
				_attribute.OdourIntensity = ICEEditorLayout.MaxDefaultSlider( "Intensity", "", _attribute.OdourIntensity , 1, 0, ref _attribute.OdourIntensityMax, 0, Info.ODOUR_INTENSITY );
				_attribute.OdourRange = ICEEditorLayout.MaxDefaultSlider( "Range", "", _attribute.OdourRange , 1, 0, ref _attribute.OdourRangeMax, 0, Info.ODOUR_RANGE );
				EditorGUI.indentLevel--;
				EditorGUILayout.Separator();
			}
		}
	}
}
