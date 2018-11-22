// ##############################################################################
//
// ice_CreatureBodyPartMotion.cs
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
using System.Text.RegularExpressions;

using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class BodyPartMotionObject : ICEOwnerObject 
	{
		public BodyPartMotionObject(){}
		public BodyPartMotionObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public BodyPartMotionObject( BodyPartMotionObject _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

			if( Owner != null )
				m_InitialRotation = Owner.transform.localRotation;
		}

		public override void Reset()
		{

		}

		public void Copy( BodyPartMotionObject _object )
		{
			base.Copy( _object );

			RotationSpeed = _object.RotationSpeed;
			RotationSpeedMaximum = _object.RotationSpeedMaximum;

			UseRotationLimits = _object.UseRotationLimits;
			MinRotationLimits = _object.MinRotationLimits;
			MaxRotationLimits = _object.MaxRotationLimits;
			RotationLimitsMaximum = _object.RotationLimitsMaximum;
		}

		public float RotationSpeed = 2;
		public float RotationSpeedMaximum = 10;

		public bool UseRotationLimits = true;
		public Vector3 MinRotationLimits = Vector3.zero;
		public Vector3 MaxRotationLimits = Vector3.zero;
		public Vector3 RotationLimitsMaximum = new Vector3( 90, 180, 45 );

		protected Quaternion m_InitialRotation = Quaternion.identity;

		public void Update() 
		{
			if( ! Enabled || OwnerComponent == null )
				return;

			ICECreatureEntity _entity = OwnerComponent as ICECreatureEntity;
			if( _entity == null )
				return;

			ICECreatureControl _creature = _entity.RootEntity as ICECreatureControl;
			if( _creature != null && _creature.Creature.ActiveTarget != null && _creature.Creature.ActiveTarget.IsValidAndReady )
			{
				//get the target transform position 
				Vector3 _position = _creature.Creature.ActiveTarget.TargetMovePosition;

				//find the vector pointing from our position to the target
				Vector3 _direction = ( _position - Owner.transform.position ).normalized;

				//create the rotation we need to be in to look at the target
				Quaternion _look_rotation = Quaternion.LookRotation( _direction );

				Owner.transform.rotation = Quaternion.Slerp( Owner.transform.rotation, _look_rotation, Time.deltaTime * RotationSpeed );

				if( UseRotationLimits )
				{
					Vector3 _clamped_local_euler = MathTools.ClampEuler( Owner.transform.localEulerAngles - m_InitialRotation.eulerAngles, MinRotationLimits, MaxRotationLimits ) + m_InitialRotation.eulerAngles;

					Owner.transform.localEulerAngles = _clamped_local_euler;

					//Debug.Log( Owner.transform.localEulerAngles.magnitude + " - " + (_clamped_local_euler - m_InitialRotation.eulerAngles).magnitude );
				}
			}
			else
			{
				
				Quaternion _look_rotation = Quaternion.identity; //Quaternion.LookRotation( Vect );
				_look_rotation.eulerAngles = new Vector3( 90 + ( 50 * _creature.Velocity.z ), m_InitialRotation.eulerAngles.y, 0 );
				Owner.transform.localRotation = Quaternion.Slerp( Owner.transform.localRotation, _look_rotation, Time.deltaTime * RotationSpeed );


			}


				
		}

	}
}