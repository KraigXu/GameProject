// ##############################################################################
//
// ICEEnvironmentControllerEditor.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.ice-technologies.com
// mailto:support@ice-technologies.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ##############################################################################

using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.AnimatedValues;

using ICE;
using ICE.World;
using ICE.World.EditorUtilities;

using ICE.Environment;
using ICE.Environment.Objects;
using ICE.Environment.EditorUtilities;
using ICE.Environment.EditorInfos;


namespace ICE.Environment
{
	[CustomEditor(typeof(ICEEnvironment))]
	public class ICEEnvironmentEditor : ICEWorldSingletonEditor {

		public override void OnInspectorGUI()
		{
			ICEEnvironment _target = DrawDefaultHeader<ICEEnvironment>();
			DrawEnvironmentContent( _target );
			DrawDefaultFooter( _target );
		}

		public void DrawEnvironmentContent( ICEEnvironment _environment )
		{
			EnvironmentObjectEditor.DrawDisplayObjectSettings( _environment, _environment.Display, m_HeaderType );
			EnvironmentObjectEditor.DrawAstronomyObjectSettings( _environment, _environment.Astronomy, m_HeaderType );
			EnvironmentObjectEditor.DrawWeatherObjectSettings( _environment, _environment.Weather, m_HeaderType );
		}
	}
}