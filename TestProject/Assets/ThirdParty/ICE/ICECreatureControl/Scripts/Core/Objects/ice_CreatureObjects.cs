// ##############################################################################
//
// ice_CreatureTarget.cs
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
using System.Xml;
using System.Xml.Serialization;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class EntityDataObject : ICEEntityObject
	{
		public EntityDataObject(){}
		public EntityDataObject( EntityDataObject _data ) : base( _data ) {
			Copy( _data );
		}

		public void Copy( EntityDataObject _data )
		{
			if( _data == null )
				return;

			base.Copy( _data );
		}
			
		/// <summary>
		/// Returns the Entity as ICECreatureControl instance or null.
		/// </summary>
		public new ICECreatureEntity EntityComponent{
			get{ return base.EntityComponent as ICECreatureEntity; }
		}

		/// <summary>
		/// Returns the Entity as ICECreatureControl instance or null.
		/// </summary>
		public ICECreatureControl EntityCreature{
			get{ return EntityComponent as ICECreatureControl; }
		}

		/// <summary>
		/// Returns the Entity as ICECreaturePlayer instance or null.
		/// </summary>
		public ICECreaturePlayer EntityPlayer{
			get{ return EntityComponent as ICECreaturePlayer; }
		}

		/// <summary>
		/// Returns the Entity as ICECreaturePlant instance or null.
		/// </summary>
		public ICECreaturePlant EntityPlant{
			get{ return EntityComponent as ICECreaturePlant; }
		}

		/// <summary>
		/// Returns the Entity as ICECreatureItem instance or null.
		/// </summary>
		public ICECreatureItem EntityItem{
			get{ return EntityComponent as ICECreatureItem; }
		}

		/// <summary>
		/// Returns the Entity as ICECreatureLocation instance or null.
		/// </summary>
		public ICECreatureLocation EntityLocation{
			get{ return EntityComponent as ICECreatureLocation; }
		}

		/// <summary>
		/// Returns the Entity as ICECreatureWaypoint instance or null.
		/// </summary>
		public ICECreatureWaypoint EntityWaypoint{
			get{ return EntityComponent as ICECreatureWaypoint; }
		}

		/// <summary>
		/// Returns the Entity as ICECreatureMarker instance or null.
		/// </summary>
		public ICECreatureMarker EntityMarker{
			get{ return EntityComponent as ICECreatureMarker; }
		}
	}
}