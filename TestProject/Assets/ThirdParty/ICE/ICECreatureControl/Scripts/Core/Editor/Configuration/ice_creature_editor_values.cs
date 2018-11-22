// ##############################################################################
//
// ice_CreatureEditorInit.cs
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

namespace ICE.Creatures.EditorInfos
{
	
	public class Init : ICE.World.EditorInfos.Init
	{
		public static readonly float TARGET_OFFSET_DISTANCE_DEFAULT = 0;
		public static readonly float TARGET_OFFSET_DISTANCE_STEP = 0.01f;
		public static readonly float TARGET_OFFSET_DISTANCE_MIN = 0;
		public static readonly float TARGET_OFFSET_DISTANCE_MAX = 100;

		public static readonly float STOPPING_DISTANCE_MINIMUM = 0.25f;
		public static readonly float STOPPING_DISTANCE_DEFAULT = 2;

		public static readonly float TARGET_RANDOM_RANGE_DEFAULT = 0;

		public static readonly Color GIZMOS_COLOR_PATH_PREVIOUS = new Vector4(0, 0.5f, 0.5f, 0.5f);
		public static readonly Color GIZMOS_COLOR_PATH_CURRENT = new Vector4(0, 0.9f, 0.9f, 1f);
		public static readonly Color GIZMOS_COLOR_PATH_PROJECTED = new Vector4(1, 0, 0, 1f);

		public static readonly Color GIZMOS_COLOR_MOVE = new Vector4(0, 0.5f, 0.5f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_DETOUR = new Vector4(0,0.75f, 0.75f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_ORBIT = new Vector4(0,0.75f, 0.75f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_ESCAPE = new Vector4(0,0.75f, 0.75f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_CUSTOM = new Vector4(0,0.75f, 0.75f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_RANDOM = new Vector4(0,0.75f, 0.75f, 0.5f);
		public static readonly Color GIZMOS_COLOR_MOVE_AVOID = new Vector4(0,0.75f, 0.75f, 0.5f);

		public static readonly Color GIZMOS_COLOR_POINTER_MOVE = new Vector4(1f, 0.0f, 0.0f, 0.20f);
		public static readonly Color GIZMOS_COLOR_POINTER_MOVE_TARGET = new Vector4(0, 0.0f, 1f, 0.20f);
		public static readonly Color GIZMOS_COLOR_POINTER_MOVE_DESIRED = new Vector4(0, 1f, 0.0f, 0.20f);

		public static readonly Color GIZMOS_COLOR_TARGET = new Vector4(0, 0.5f, 1, 0.5f);
		public static readonly Color GIZMOS_COLOR_TARGET_ACTIVE = new Vector4(0, 0, 1, 1);
		public static readonly Color GIZMOS_COLOR_TARGET_STOPPING_DISTANCE = new Vector4(1, 0, 0, 0.05f);
		public static readonly Color GIZMOS_COLOR_TARGET_SELECTION_RANGE = new Vector4(0, 0, 1, 0.05f);
		public static readonly Color GIZMOS_COLOR_TARGET_RANDOM_RANGE = new Vector4(1, 1, 0, 0.05f);

		public static readonly Color GIZMOS_COLOR_INTERACTION = new Vector4(0.75f, 0.5f, 0.65f, 1);
		public static readonly float GIZMOS_COLOR_INTERACTION_ALPHA = 0.05f;

		public static readonly float DEFAULT_STEP_DISTANCE = 0.25f;
		public static readonly float DEFAULT_MIN_DISTANCE = 0;
		public static readonly float DEFAULT_MAX_DISTANCE = 100;

		public static readonly float STOP_DISTANCE_DEFAULT = 0;
		public static readonly float STOP_DISTANCE_STEP = 0.05f;
		public static readonly float STOP_MIN_DISTANCE = 0;
		public static readonly float STOP_MAX_DISTANCE = 10;

		public static readonly float MOVE_DISTANCE_DEFAULT = 0;
		public static readonly float MOVE_DISTANCE_STEP = 0.05f;
		public static readonly float MOVE_MIN_DISTANCE = 0;
		public static readonly float MOVE_MAX_DISTANCE = 50;

		public static readonly float MOVE_ESCAPE_RANDOM_ANGLE_DEFAULT = 0;
		public static readonly float MOVE_ESCAPE_RANDOM_ANGLE_MIN = 0;
		public static readonly float MOVE_ESCAPE_RANDOM_ANGLE_MAX = 45;
		public static readonly float MOVE_ESCAPE_RANDOM_ANGLE_STEP = 0.25f;

		public static readonly float MOVE_ESCAPE_DISTANCE_DEFAULT = 10;
		public static readonly float MOVE_ESCAPE_DISTANCE_MIN = 0;
		public static readonly float MOVE_ESCAPE_DISTANCE_MAX = 100;
		public static readonly float MOVE_ESCAPE_DISTANCE_STEP = 0.5f;

		public static readonly float MOVE_AVOID_DISTANCE_DEFAULT = 1;
		public static readonly float MOVE_AVOID_DISTANCE_MIN = 0;
		public static readonly float MOVE_AVOID_DISTANCE_MAX = 25;
		public static readonly float MOVE_AVOID_DISTANCE_STEP = 0.1f;

		public static readonly float MOVE_ORBIT_RADIUS_DEFAULT = 10;
		public static readonly float MOVE_ORBIT_RADIUS_MAX = 100;
		public static readonly float MOVE_ORBIT_RADIUS_STEP = 0.25f;

		public static readonly float MOVE_ORBIT_SHIFT_DEFAULT = 0;
		public static readonly float MOVE_ORBIT_SHIFT_STEP = 0.25f;
		public static readonly float MOVE_ORBIT_SHIFT_MIN = -10;
		public static readonly float MOVE_ORBIT_SHIFT_MAX = 10;

		public static readonly float MOVE_VELOCITY_FORWARDS_DEFAULT = 0;
		public static readonly float MOVE_VELOCITY_FORWARDS_MAX = 50;
		public static readonly float MOVE_VELOCITY_ANGULAR_MAX = 30;

		public static readonly float DURATION_OF_STAY_DEFAULT = 0;
		public static readonly float DURATION_OF_STAY_STEP = 0.25f;
		public static readonly float DURATION_OF_STAY_MIN = 0.0f;
		public static readonly float DURATION_OF_STAY_MAX = 360;
	

		public static readonly float MOVE_INTERFERENCE_DEFAULT = 0;
		public static readonly float MOVE_INTERFERENCE_STEP = 0.015f;
		public static readonly float MOVE_INTERFERENCE_MIN = 0;
		public static readonly float MOVE_INTERFERENCE_MAX = 1;

		public static readonly float SELECTION_RANGE_STEP = 0.01f;
		public static readonly float SELECTION_RANGE_MIN = 0;
		public static readonly float SELECTION_RANGE_MAX = 100;

		public static readonly float SELECTION_ANGLE_STEP = 0.25f;
		public static readonly float SELECTION_ANGLE_MIN = 0;
		public static readonly float SELECTION_ANGLE_MAX = 360;

		public static readonly float STATUS_PERCEPTION_TIME_DEFAULT = 0.5f;
		public static readonly float STATUS_PERCEPTION_TIME_STEP = 0.025f;
		public static readonly float STATUS_PERCEPTION_TIME_MIN = 0;
		public static readonly float STATUS_PERCEPTION_TIME_MAX = 30;

		public static readonly float STATUS_REACTION_TIME_DEFAULT = 0.5f;
		public static readonly float STATUS_REACTION_TIME_STEP = 0.025f;
		public static readonly float STATUS_REACTION_TIME_MIN = 0;
		public static readonly float STATUS_REACTION_TIME_MAX = 30;


		// WIZARD RANDOM VALUES

		public static readonly float WIZARD_RANDOM_RANGE_MIN = 2;
		public static readonly float WIZARD_RANDOM_RANGE_MAX = 100;

	}
}
