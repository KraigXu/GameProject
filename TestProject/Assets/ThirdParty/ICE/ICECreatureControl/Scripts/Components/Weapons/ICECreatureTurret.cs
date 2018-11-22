// ##############################################################################
//
// ICECreatureTurret.cs
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


	public class ICECreatureTurret : ICECreatureRangedWeapon 
	{
		/// <summary>
		/// Gets the entity classification type of the object
		/// </summary>
		/// <value>The classification type of the entity.</value>
		/// <description>The EntityClassType will be used to quickly 
		/// identify the correct class type of a derived entity object 
		/// without casts
		/// </description>
		public override EntityClassType EntityType{
			get{ return EntityClassType.Turret; }
		}

		/// <summary>
		/// OnRegisterBehaviourEvents is called whithin the GetBehaviourEvents() method to update the 
		/// m_BehaviourEvents list. Override this event to register your own events by using the 
		/// RegisterBehaviourEvent method, while doing so you can use base.OnRegisterBehaviourEvents(); 
		/// to call the event in the base classes too.
		/// </summary>
		protected override void OnRegisterBehaviourEvents()
		{
			base.OnRegisterBehaviourEvents();
			//e.g.RegisterBehaviourEvent( "ApplyDamage", BehaviourEventParameterType.Float );
			RegisterBehaviourEvent( "TurretForceActiveTarget", BehaviourEventParameterType.Sender );
			RegisterBehaviourEvent( "TurretResetActiveTarget" );
		}

		[SerializeField]
		private TurretObject m_Turret = null;
		public TurretObject Turret{
			get{ return m_Turret = ( m_Turret == null ? new TurretObject(this) : m_Turret ); }
			set{ Turret.Copy( value ); }
		}

		/// <summary>
		/// Gets the active target transform from the turret or null if there is no active target available.
		/// </summary>
		/// <returns>The target transform.</returns>
		public override Transform GetTargetTransform(){
			return Turret.ActiveTarget;
		}


	
		public override void Start () {
		
			base.Start();

			// prepares the turret
			Turret.Init(this);

			Turret.DefaultPivotRotation = ( Turret.PivotPoint != null ? Turret.PivotPoint.rotation : Quaternion.identity ); 
			Turret.DefaultPivotYawRotation = ( Turret.PivotYawAxis != null ? Turret.PivotYawAxis.localEulerAngles.y : 0 );
			Turret.DefaultPivotPitchRotation = ( Turret.PivotPitchAxis != null ? Turret.PivotPitchAxis.localEulerAngles.x : 0 );
		}
		

		public override void Update () {
			
			base.Update();

			// handles the turret include movements and target and returns true if a target is available and focused
			if( Turret.Update() )
				Weapon.Fire( GetTargetTransform(), false );
			else
				Weapon.Stop();
			
		}

		public void TurretForceActiveTarget( GameObject _target )
		{
			Turret.ForceActiveTarget( _target );
		}

		public void TurretResetActiveTarget()
		{
			Turret.ResetActiveTarget();
		}
	}
}
