// ##############################################################################
//
// ICECreatureOdourAttribute.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Attributes
{
	public class ICECreatureOdourAttribute : ICECreatureAttribute {

		public OdourType Odour = OdourType.NONE;
		public float OdourIntensity = 50f;
		public float OdourIntensityMax = 100f;
		public float OdourRange = 25f;
		public float OdourRangeMax = 100f;
	}
}
