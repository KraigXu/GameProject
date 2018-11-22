﻿// ##############################################################################
//
// ICECreatureItem.cs
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

namespace ICE.Creatures{

	public class ICECreatureLocation : ICECreatureEntity {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Location; }
		}

		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		public override void Update () {

			if( Status.IsDestroyed )
			{
				Status.Remove();
				return;
			}

			base.Update();
		}

	}
}
