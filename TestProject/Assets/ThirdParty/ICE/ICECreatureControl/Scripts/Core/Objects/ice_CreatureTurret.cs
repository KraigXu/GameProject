// ##############################################################################
//
// ice_CreatureTurret.cs | TurretObject
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

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class TurretObject : ICEOwnerObject 
	{
		public TurretObject(){}
		public TurretObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public TurretObject( TurretObject _object ){ Copy( _object ); }

		public override void Init( ICEWorldBehaviour _component ){

			base.Init( _component );

			MovingSound.Init( _component );
		}

		public void Copy( TurretObject _object )
		{
			base.Copy( _object );

			ScanRange = _object.ScanRange;
			ScanRangeMaximum = _object.ScanRangeMaximum;

			Layers.Copy( _object.Layers );

		}

		public float ScanRange = 15.0f;
		public float ScanRangeMaximum = 100.0f;

		[SerializeField]
		private LayerObject m_Layers = null;
		public LayerObject Layers{
			get{ return m_Layers = ( m_Layers == null ? new LayerObject() : m_Layers ); }
			set{ Layers.Copy( value ); }
		}

		public MountingPivotType PivotType = MountingPivotType.PivotalPoint;

		public bool UseParkPosition = true;

		public Quaternion DefaultPivotRotation = Quaternion.identity; 
		public float DefaultPivotYawRotation = 0; 
		public float DefaultPivotPitchRotation = 0; 

		public Transform PivotPoint = null; 
		public Transform PivotYawAxis = null; 
		public Transform PivotPitchAxis = null; 
		public float RotationSpeed = 2; 
		public float RotationSpeedMaximum = 10; 

		public float MaxAngularDeviation = 2;
		public float VerticalTargetAdjustment = 0;

		private float m_ScanIntervalTimer = 0; 
		public float ScanInterval = 0.5f; 
		public float ScanIntervalMaximum = 10; 

		protected Transform m_ActiveTarget = null;
		public Transform ActiveTarget{
			get{ return m_ActiveTarget; }
		}

		public void ForceActiveTarget( GameObject _target )
		{
			if( _target == null )
				return;

			m_ActiveTarget = _target.transform;
			m_IsForced = true;
		}

		public void ResetActiveTarget()
		{
			m_ActiveTarget = null;
			m_IsForced = false;
		}

		[SerializeField]
		private DirectAudioPlayerObject m_MovingSound = null;
		public DirectAudioPlayerObject MovingSound{
			get{ return m_MovingSound = ( m_MovingSound == null ? new DirectAudioPlayerObject( OwnerComponent ) : m_MovingSound ); }
			set{ MovingSound.Copy( value ); }
		}

		private bool m_IsForced = false;
		public bool IsForced{
			get{ return m_IsForced; }
		}

		private bool m_IsMoving = false;
		public bool IsMoving{
			get{ return m_IsMoving; }
		}

		private bool m_IsFocused = false;
		public bool IsFocused{
			get{ return m_IsFocused; }
		}

		/// <summary>
		/// Update the turret incl. movements and scan for targets
		/// </summary>
		public bool Update()
		{
			if( Owner == null )
				return false;

			Scan();

			m_IsFocused = false;
			m_IsMoving = false;

			// runs the default behaviour if there is no target 
			if( m_ActiveTarget == null )
			{
				if( UseParkPosition )
				{
					if( PivotType == MountingPivotType.PivotalPoint )
					{
						PivotPoint.rotation = Quaternion.Slerp( PivotPoint.rotation, DefaultPivotRotation, RotationSpeed * Time.deltaTime );  

						if( Quaternion.Angle( PivotPoint.rotation, DefaultPivotRotation ) > MaxAngularDeviation )
							m_IsMoving = true;
					}
					else if( PivotType == MountingPivotType.SeperateAxes )
					{
						if( PivotYawAxis != null )
						{
							if( PositionTools.Distance( PivotYawAxis.localEulerAngles, new Vector3( 0, DefaultPivotYawRotation, 0) ) > MaxAngularDeviation )
							{
								PivotYawAxis.localEulerAngles = new Vector3( 0, Mathf.LerpAngle( PivotYawAxis.localEulerAngles.y, DefaultPivotYawRotation, RotationSpeed * Time.deltaTime ), 0);
								m_IsMoving = true;
							}
							else
								PivotYawAxis.localEulerAngles = new Vector3( 0, DefaultPivotYawRotation, 0);
						}

						if( PivotPitchAxis != null )
						{
							if( PositionTools.Distance( PivotPitchAxis.localEulerAngles, new Vector3( DefaultPivotPitchRotation, 0, 0) ) > MaxAngularDeviation )
							{
								PivotPitchAxis.localEulerAngles = new Vector3( Mathf.LerpAngle( PivotPitchAxis.localEulerAngles.x, DefaultPivotPitchRotation, RotationSpeed * Time.deltaTime ), 0, 0);
								m_IsMoving = true;
							}
							else
								PivotPitchAxis.localEulerAngles = new Vector3( DefaultPivotPitchRotation, 0, 0);

						}
					}
				}
			}

			// focus an existing target
			else
			{
				float _height = 1;
				Collider _collider = m_ActiveTarget.GetComponent<Collider>();
				if( _collider != null )
					_height = _collider.bounds.size.magnitude / 3;

				_height += VerticalTargetAdjustment;

				Vector3 _target_pos = m_ActiveTarget.position + ( Vector3.up * _height );

				if( PivotType == MountingPivotType.PivotalPoint )
				{
					Quaternion _rotation = Quaternion.LookRotation( _target_pos - PivotPoint.position, Vector3.up );
					PivotPoint.rotation = Quaternion.Slerp( PivotPoint.rotation, _rotation, RotationSpeed * Time.deltaTime );  

					if( Quaternion.Angle( PivotPoint.rotation, _rotation ) < MaxAngularDeviation )
					{
						m_IsFocused = true;
						m_IsMoving = false;
					}
					else
					{
						m_IsFocused = false;
						m_IsMoving = true;
					}
				}
				else if( PivotType == MountingPivotType.SeperateAxes )
				{
					if( PivotYawAxis != null )
					{
						Vector3 _yaw_pos = new Vector3( _target_pos.x, PivotYawAxis.position.y, _target_pos.z );
						Quaternion _yaw_rot = Quaternion.LookRotation( _yaw_pos - PivotYawAxis.position, Vector3.up );
						PivotYawAxis.rotation = Quaternion.Slerp( PivotYawAxis.rotation, _yaw_rot, RotationSpeed * Time.deltaTime );   

						if( Quaternion.Angle( PivotYawAxis.rotation, _yaw_rot ) <= MaxAngularDeviation )
						{
							m_IsFocused = true;
							m_IsMoving = false;
						}
						else
						{
							m_IsFocused = false;
							m_IsMoving = true;
						}
					}

					if( PivotPitchAxis != null )
					{
						Vector3 _pitch_dir = PivotPitchAxis.position - _target_pos;
						Vector3 _pitch_hor = new Vector3( _pitch_dir.x, 0, _pitch_dir.z );
						float _angle = MathTools.NormalizeAngle( 360 + Vector3.Angle( _pitch_dir, _pitch_hor ) * Mathf.Sign(Vector3.Dot(_pitch_dir, Vector3.up)));

						PivotPitchAxis.localEulerAngles = new Vector3( Mathf.LerpAngle( PivotPitchAxis.localEulerAngles.x, _angle, RotationSpeed * Time.deltaTime ), 0, 0);

						if( m_IsFocused && PositionTools.Distance( PivotPitchAxis.localEulerAngles, new Vector3( _angle, 0, 0) ) <= MaxAngularDeviation )
						{
							m_IsFocused = true;
							m_IsMoving = false;
						}
						else
						{
							m_IsFocused = false;
							m_IsMoving = true;
						}
					}
				}
			}

			if( m_IsMoving )
				MovingSound.Play();
			else
				MovingSound.Stop();

			return m_IsFocused;
		}

		/// <summary>
		/// Scan for targets within the scanning range
		/// </summary>
		public void Scan()
		{
			if( m_IsForced && m_ActiveTarget != null )
				return;

			m_ScanIntervalTimer += Time.deltaTime;
			if( Owner == null && m_ScanIntervalTimer > ScanInterval )
				return;

			m_ScanIntervalTimer = 0;
			m_ActiveTarget = null;

			Collider[] _colliders = Physics.OverlapSphere( Owner.transform.position, ScanRange, Layers.Mask );
			foreach( Collider _collider in _colliders)
			{
				if( _collider.transform.IsChildOf( Owner.transform ) || Owner.transform.IsChildOf( _collider.transform ) )
					continue;

				DebugLine( Owner.transform.position, _collider.transform.position + Vector3.up, Color.yellow );

				RaycastHit _obstacle;
				SystemTools.EnableColliders( Owner.transform, false );
				Physics.Linecast( Owner.transform.position + Vector3.up, _collider.transform.position + Vector3.up, out _obstacle );
				SystemTools.EnableColliders( Owner.transform, true );
				if( _obstacle.collider != null && _obstacle.collider != _collider && ! _obstacle.transform.IsChildOf( _collider.transform.root ) )
					continue;

				//TODO : improve the selection
				m_ActiveTarget = _collider.transform;
				return;
			}

			return;

		}
	}

}
