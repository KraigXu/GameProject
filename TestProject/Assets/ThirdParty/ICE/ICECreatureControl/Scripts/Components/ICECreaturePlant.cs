// ##############################################################################
//
// ICECreaturePlant.cs
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

	public class ICECreaturePlant : ICECreatureOrganism {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Plant; }
		}

		/// <summary>
		/// OnRegisterPublicMethods is called whithin the GetPublicMethods() method to update the m_PublicMethods list.
		/// Override this event to register your own methods by 
		/// using the RegisterPublicMethod(); while doing so you can use base.OnRegisterPublicMethods(); to call the event in
		/// the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		[SerializeField]
		private InventoryObject m_Inventory = new InventoryObject();
		public InventoryObject Inventory{
			set{ m_Inventory = value; }
			get{ return m_Inventory; }
		}
	}
}
