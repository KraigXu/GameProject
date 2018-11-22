// ##############################################################################
//
// ICECreatureTool.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
// 
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// ICECreatureTarget
// -> ICECreatureObject
// --> ICECreatureItem
// ---> ICECreatureWeapon
// ---> ICECreatureRangedWeapon
// ---> ICECreatureMeleeWeapon
// -> ICECreatureOrganism
// --> ICECreatureControl
// --> ICECreaturePlayer
// --> ICECreaturePlant
// -> ICECreatureMarker
// --> ICECreatureWaypoint
// --> ICECreatureLocation
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

	public class ICECreatureTool : ICECreatureItem {

		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts</description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Tool; }
		}

		/// <summary>
		/// Register public methods. Override this method to register your own methods by using the RegisterPublicMethod();
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			RegisterBehaviourEvent( "ToolOff" );
			RegisterBehaviourEvent( "ToolStandBy" );
			RegisterBehaviourEvent( "ToolOperate" );
			//e.g. RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
		}

		[SerializeField]
		private ToolObject m_Tool = null;
		public ToolObject Tool{
			get{ return m_Tool = ( m_Tool == null ? new ToolObject(this) : m_Tool); }
			set{ Tool.Copy( value ); }
		}

		public override void Awake()
		{
			base.Awake();

			Tool.Init( this );
		}
			
		public void ToolOff(){
			Tool.Stop();
		}

		public void ToolStandBy(){
			Tool.StandBy();
		}

		public void ToolOperate(){
			Tool.Operate();
		}
	}
}
