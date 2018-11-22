// ##############################################################################
//
// ice_world_editor_values.cs | Init
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
using UnityEditor;

namespace ICE.World.EditorInfos
{
	public class Init
	{
		public static readonly float DECIMAL_PRECISION = 0.001f;
		public static readonly float DECIMAL_PRECISION_MASS = 0.1f;
		public static readonly float DECIMAL_PRECISION_TIMER = 0.001f;
		public static readonly float DECIMAL_PRECISION_ANGLE = 0.01f;
		public static readonly float DECIMAL_PRECISION_DISTANCES = 0.001f;
		public static readonly float DECIMAL_PRECISION_VELOCITY = 0.001f;
		public static readonly float DECIMAL_PRECISION_INDICATOR = 0.025f;
		public static readonly float DECIMAL_PRECISION_MAX = 0.0001f;

		public static readonly float SLIDER_MIN = 0;
		public static readonly float SLIDER_MAX_PERCENT = 0;
	}
}
