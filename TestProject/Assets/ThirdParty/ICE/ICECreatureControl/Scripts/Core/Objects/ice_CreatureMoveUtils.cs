// ##############################################################################
//
// ice_CreatureMoveUtils.cs
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

namespace ICE.Creatures.Container
{
}

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class MoveDetourObject : ICEObject
	{
		public MoveDetourObject(){}
		public MoveDetourObject( MoveDetourObject _object ){ Copy( _object ); }

		public Vector3 Position = Vector3.zero;

		public void Copy( MoveDetourObject _object )
		{
			if( _object == null )
				return;

			Position = _object.Position;
		}
	}

	[System.Serializable]
	public class MoveEscapeObject : ICEObject
	{
		public MoveEscapeObject(){}
		public MoveEscapeObject( MoveEscapeObject _object ){ Copy( _object ); }

		public float EscapeDistance = 0;
		public float RandomEscapeAngle = 0;

		public void Copy( MoveEscapeObject _object )
		{
			if( _object == null )
				return;

			EscapeDistance = _object.EscapeDistance;
			RandomEscapeAngle = _object.RandomEscapeAngle;
		}
	}

	[System.Serializable]
	public class MoveCoverObject : ICEObject
	{
		public MoveCoverObject(){}
		public MoveCoverObject( MoveCoverObject _object ){ Copy( _object ); }

		public float MaxDistance = 15;
		public float MaxDistanceMaximum = 100;
		public float StepAngle = 1;
		public float HorizontalOffset = 0.5f;
		public float HorizontalOffsetMaximum = 0.5f;
		public float VerticalOffset = 0f;
		public float VerticalOffsetMaximum = 5f;

		public void Copy( MoveCoverObject _object )
		{
			if( _object == null )
				return;

			MaxDistance = _object.MaxDistance;
			MaxDistanceMaximum = _object.MaxDistanceMaximum;
			StepAngle = _object.StepAngle;
			HorizontalOffset = _object.HorizontalOffset;
			HorizontalOffsetMaximum = _object.HorizontalOffsetMaximum;
		}
	}

	[System.Serializable]
	public class MoveAvoidObject : ICEObject
	{
		public MoveAvoidObject(){}
		public MoveAvoidObject( MoveAvoidObject _object ){ Copy( _object ); }

		public float AvoidDistance;

		public void Copy( MoveAvoidObject _object )
		{
			if( _object == null )
				return;

			AvoidDistance = _object.AvoidDistance;
		}
	}

	[System.Serializable]
	public class MoveOrbitObject : ICEObject
	{
		public MoveOrbitObject(){}
		public MoveOrbitObject( MoveOrbitObject _object ){ Copy( _object ); }

		public float Radius = 0;
		public float RadiusShift = 0;
		public float MaxDistance = 0;
		public float MinDistance = 0;

		public void Copy( MoveOrbitObject _object )
		{
			if( _object == null )
				return;

			Radius = _object.Radius;
			RadiusShift = _object.RadiusShift;
			MaxDistance = _object.MaxDistance;
			MinDistance = _object.MinDistance;
		}
	}

	/*

	[System.Serializable]
	public struct MoveVelocityContainer
	{
		//TODO [System.Obsolete ("Use Angular instead")]
		public VelocityType Type;

		public Vector3 Velocity;
		public Vector3 VelocityMaximum;

		//TODO [System.Obsolete ("Use Angular instead")]
		public float AngularVelocity;
		//TODO [System.Obsolete ("Use AngularMaximum instead")]
		public float AngularVelocityMaximum;
		public bool AngularVelocityAuto;

		public Vector3 Angular;
		public Vector3 AngularMaximum;

		public bool UseNegativeVelocity;
		public float VelocityMinVariance;
		public float VelocityMaxVariance;
		public bool UseTargetVelocity;

		public bool UseAdvancedVelocity;
		public bool UseAdvancedAngularVelocity;
		public float Inertia;

		public bool UseAutoDrift;
		public float DriftMultiplier;

		public float VelocityMultiplier;
		public float VelocityMultiplierUpdateTimer;
		public float VelocityMultiplierUpdateInterval;
	}
*/

	[System.Serializable]
	public class MoveMotionObject : ICEObject
	{
		public MoveMotionObject(){}
		public MoveMotionObject( MoveMotionObject _object ){ Copy( _object ); }

		public Vector3 Velocity = Vector3.zero;
		public Vector3 VelocityMaximum = new Vector3( 0, 0, 25 );

		public bool UseAutomaticAngularVelocity = false;
		public Vector3 AngularVelocity = new Vector3( 0, 2, 0 );
		public Vector3 AngularVelocityMaximum = new Vector3( 0, 30, 0 );

		public bool UseTargetVelocity = false;
		public bool UseNegativeVelocity = false;

		public float VelocityMinVariance = 0;
		public float VelocityMaxVariance = 0;

		public bool UseAdvancedVelocity = false;
		public bool UseAdvancedAngularVelocity = false;
		public float Inertia = 0;

		public bool UseAutoDrift = false;
		public float DriftMultiplier = 1;


		public float VelocityMultiplierUpdateTimer = 0;
		public float VelocityMultiplierUpdateInterval = 0;

		private float m_VelocityMultiplier = 0;
		public float VelocityMultiplier{
			get{ return m_VelocityMultiplier; }
		}

		public void Copy( MoveMotionObject _object )
		{
			if( _object == null )
				return;
			
			Velocity = _object.Velocity;
			VelocityMaximum = _object.VelocityMaximum;
			UseNegativeVelocity = _object.UseNegativeVelocity;
			VelocityMinVariance = _object.VelocityMinVariance;
			VelocityMaxVariance = _object.VelocityMaxVariance;

			UseTargetVelocity = _object.UseTargetVelocity;
			AngularVelocity = _object.AngularVelocity;
			AngularVelocityMaximum = _object.AngularVelocityMaximum;
			UseAutomaticAngularVelocity = _object.UseAutomaticAngularVelocity;

			UseAdvancedVelocity = _object.UseAdvancedVelocity;
			UseAdvancedAngularVelocity = _object.UseAdvancedAngularVelocity;

			Inertia = _object.Inertia;

			UseAutoDrift = _object.UseAutoDrift;
			DriftMultiplier = _object.DriftMultiplier;

			VelocityMultiplierUpdateTimer = _object.VelocityMultiplierUpdateTimer;
			VelocityMultiplierUpdateInterval = _object.VelocityMultiplierUpdateInterval;

			m_VelocityMultiplier = _object.VelocityMultiplier;
		}

		public float UpdateVelocityMultiplier()
		{
			m_VelocityMultiplier = Random.Range( VelocityMinVariance, VelocityMaxVariance );
			
			return m_VelocityMultiplier;
		}
	}

	[System.Serializable]
	public class BodyDataObject : ICEObject
	{
		public BodyDataObject(){}
		public BodyDataObject( BodyDataObject _body ){ Copy( _body ); } 

		public void CopyDefault( BodyDataObject _body )
		{
			if( _body == null )
				return;

			Type = _body.Type;
			UseAdvanced = _body.UseAdvanced;
			Length = _body.Length;
			Width = _body.Width;
			Height = _body.Height;
			DefaultLength = _body.DefaultLength;
			DefaultWidth = _body.DefaultWidth;
			DefaultHeight = _body.DefaultHeight;
			LengthOffset = _body.LengthOffset;
			WidthOffset = _body.WidthOffset;

			UseRollAngle = _body.UseRollAngle;
			MaxRollAngle = _body.MaxRollAngle;
			RollAngleMultiplier = _body.RollAngleMultiplier;

			UsePitch = _body.UsePitch;
			MaxPitchAngle = _body.MaxPitchAngle;
			PitchAngleMultiplier = _body.PitchAngleMultiplier;
		}

		public void Copy( BodyDataObject _body )
		{
			if( _body == null )
				return;

			Type = _body.Type;
			UseAdvanced = _body.UseAdvanced;

			UseRollAngle = _body.UseRollAngle;
			MaxRollAngle = _body.MaxRollAngle;
			RollAngleMultiplier = _body.RollAngleMultiplier;

			UsePitch = _body.UsePitch;
			MaxPitchAngle = _body.MaxPitchAngle;
			PitchAngleMultiplier = _body.PitchAngleMultiplier;

			Width = _body.Width;
			Length = _body.Length;
			Height = _body.Height;
			WidthOffset = _body.WidthOffset;
			LengthOffset = _body.LengthOffset;
			HeightOffset = _body.HeightOffset;

			DefaultWidth = _body.DefaultWidth;
			DefaultLength = _body.DefaultLength;
			DefaultHeight = _body.DefaultHeight;
		}

		public BodyOrientationType Type = BodyOrientationType.DEFAULT;
		public bool UseAdvanced = false;

		public bool UseRollAngle = false;
		public float MaxRollAngle = 30;
		public float RollAngleMultiplier = 0.5f;

		public bool UsePitch = false;
		public float MaxPitchAngle = 30;
		public float PitchAngleMultiplier = 0.5f;

		public float Width = 0;
		public float Length = 0;
		public float Height = 0;
		public float WidthOffset = 0;
		public float LengthOffset = 0;
		public float HeightOffset = 0;

		public float DefaultWidth = 1;
		public float DefaultLength = 1;
		public float DefaultHeight = 1;


		/// <summary>
		/// Gets the stored default size.
		/// </summary>
		/// <value>The default size.</value>
		public Vector3 DefaultSize{
			get{ return new Vector3( DefaultWidth, DefaultHeight, DefaultLength ); }
		}

		public Vector3 GetSize( GameObject _owner )
		{
			Vector3 _size = SystemTools.GetObjectSize( _owner );
			if( Width == 0 ) Width = _size.x;				
			if( Length == 0 ) Length = _size.z;				
			if( Height == 0 ) Height = _size.y;

			return new Vector3( Width, Length, Height );
		}

		public Vector3 GetDefaultSize( GameObject _owner )
		{
			Vector3 _size = SystemTools.GetObjectSize( _owner );
			DefaultWidth = _size.x;				
			DefaultLength = _size.z;				
			DefaultHeight = _size.y;

			return new Vector3( DefaultWidth, DefaultHeight, DefaultLength );
		}
	}

	[System.Serializable]
	public class MoveAltitudeObject : ICEDataObject
	{
		public MoveAltitudeObject(){}
		public MoveAltitudeObject( MoveAltitudeObject _altitude ){ Copy( _altitude );  }

		public void Copy( MoveAltitudeObject _altitude )
		{
			if( _altitude == null )
				return;

			base.Copy( _altitude );

			UseAltitudeAboveGround = _altitude.UseAltitudeAboveGround;
			UseTargetLevel = _altitude.UseTargetLevel;
			UseVerticalSpeedCurve = _altitude.UseVerticalSpeedCurve;

			m_DesiredAltitude = _altitude.DesiredAltitude;

			Min = _altitude.Min;
			Max = _altitude.Max;
			Maximum = _altitude.Maximum;

			VerticalSpeedCurve = _altitude.VerticalSpeedCurve;
		}

		public bool UseAltitudeAboveGround = false;
		public bool UseTargetLevel = false;
		public bool UseVerticalSpeedCurve = false;

		public float Min = 0;
		public float Max = 0;
		public float Maximum = 1000;

		[SerializeField]
		private AnimationCurve m_VerticalSpeedCurve = null;
		public AnimationCurve VerticalSpeedCurve{
			set{ m_VerticalSpeedCurve = value; }
			get{ return m_VerticalSpeedCurve = ( m_VerticalSpeedCurve == null ? new AnimationCurve() : m_VerticalSpeedCurve ); }
		}
		

		private float m_DesiredAltitude = 0;
		public float DesiredAltitude{
			get{ return m_DesiredAltitude; }
		}

		/// <summary>
		/// Refreshs the Altitude parameter by using AltitudeMin and AltitudeMax
		/// </summary>
		public void Init(){
			m_DesiredAltitude = Random.Range( Min, Max );
		}
	}

	[System.Serializable]
	public class MoveDeadlockObject : ICEDataObject
	{
		public MoveDeadlockObject(){}
		public MoveDeadlockObject( MoveDeadlockObject _object ){ Copy( _object );  }

		public void Copy( MoveDeadlockObject _object )
		{
			if( _object == null )
				return;

			Enabled = _object.Enabled;
			Foldout = _object.Foldout;

			Action = _object.Action;
			Behaviour = _object.Behaviour;

			MinMoveDistance = _object.MinMoveDistance;
			MoveInterval = _object.MoveInterval;
			MoveMaxCriticalPositions = _object.MoveMaxCriticalPositions;

			LoopRange = _object.LoopRange;
			LoopInterval = _object.LoopInterval;
			LoopMaxCriticalPositions = _object.LoopMaxCriticalPositions;
		}

		public DeadlockActionType Action = DeadlockActionType.DIE;
		public string Behaviour = "";

		public float MinMoveDistance = 0.25f;
		public float MoveInterval = 2;
		public int MoveMaxCriticalPositions = 10;

		public float LoopRange = 2f;
		public float LoopInterval = 5;
		public int LoopMaxCriticalPositions = 10;

		private bool m_Deadlocked = false;
		public bool Deadlocked{
			get{ return m_Deadlocked;}
		}

		private List<Vector3> m_DeadlocksCriticalPositions = new List<Vector3>();
		public int DeadlocksCriticalPositions{
			get{ return m_DeadlocksCriticalPositions.Count;}
		}

		private List<Vector3> m_DeadlocksCriticalLoops = new List<Vector3>();
		public int DeadlocksCriticalLoops{
			get{ return m_DeadlocksCriticalLoops.Count;}
		}

		private float m_DeadlockMoveTimer = 0;
		public float DeadlockMoveTimer{
			get{ return m_DeadlockMoveTimer;}
		}

		private float m_DeadlockLoopTimer = 0;
		public float DeadlockLoopTimer{
			get{ return m_DeadlockLoopTimer;}
		}


		private int m_DeadlocksCount = 0;
		public int DeadlocksCount{
			get{ return m_DeadlocksCount;}
		}

		private int m_DeadlockLoopsCount = 0;
		public int DeadlockLoopsCount{
			get{ return m_DeadlockLoopsCount;}
		}

		private float m_DeadlocksDistance= 0;
		public float DeadlocksDistance{
			get{ return m_DeadlocksDistance;}
		}

		private Vector3 m_DeadlockPosition = Vector3.zero;

		public void Reset( Transform _transform )
		{
			if( _transform == null )
				return;
			
			m_DeadlockMoveTimer = 0;
			m_DeadlockLoopTimer = 0;
			m_Deadlocked = false;
			m_DeadlockPosition = _transform.position;
		}

		public bool Check( Transform _transform, Vector3 _velocity )
		{
			if( ! Enabled || _transform == null )
				return false;

			if( _velocity.z == 0 )
			{
				m_DeadlockMoveTimer = 0;
				m_DeadlockLoopTimer = 0;
				return false;
			}

			m_DeadlockMoveTimer += Time.deltaTime;
			m_DeadlockLoopTimer += Time.deltaTime;

			if( m_DeadlockPosition == Vector3.zero )
				m_DeadlockPosition = _transform.position;

			if( m_DeadlockMoveTimer >= MoveInterval )
			{
				m_DeadlocksDistance = PositionTools.Distance( _transform.position, m_DeadlockPosition );

				// CHECK DEADLOCK
				if( m_DeadlocksDistance <= MinMoveDistance )
				{
					if( m_Deadlocked == false )
						m_DeadlocksCount++;

					m_DeadlocksCriticalPositions.Add( _transform.position );

					if( m_DeadlocksCriticalPositions.Count > MoveMaxCriticalPositions )
						m_Deadlocked = true;
				}
				else if( m_DeadlocksCriticalPositions.Count > 0 )
					m_DeadlocksCriticalPositions.RemoveAt(0);
				else 
				{
					m_DeadlockPosition = _transform.position;
					m_DeadlockMoveTimer = 0;
				}


			}

			// CHECK INFINITY LOOP
			if( m_DeadlockLoopTimer >= LoopInterval )
			{
				if( m_DeadlocksDistance <= LoopRange )
				{
					if( m_Deadlocked == false )
						m_DeadlockLoopsCount++;

					m_DeadlocksCriticalLoops.Add( _transform.position );

					if( m_DeadlocksCriticalLoops.Count > LoopMaxCriticalPositions )
						m_Deadlocked = true;
				}
				else if( m_DeadlocksCriticalLoops.Count > 0 )
					m_DeadlocksCriticalLoops.RemoveAt(0);
				else
					m_DeadlockLoopTimer = 0;

			}

			if( m_DeadlockMoveTimer == 0 && m_DeadlocksCriticalPositions.Count == 0 && m_DeadlockLoopTimer == 0 && m_DeadlocksCriticalLoops.Count == 0 )
				m_Deadlocked = false;

			return m_Deadlocked;

		}

	}

	[System.Serializable]
	public class MoveJumpObject : ICEDataObject
	{
		public MoveJumpObject(){}
		public MoveJumpObject( MoveJumpObject _move ){ Copy( _move );  }

		public void Copy( MoveJumpObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );
		}
	}
		
	[System.Serializable]
	public class MoveDataObject : ICEDataObject //, ISerializationCallbackReceiver 
	{
		public MoveDataObject(){}
		public MoveDataObject( MoveDataObject _move ){ Copy( _move );  }

		//TODO: UPDATE OBSOLETE VALUES
		//public void OnBeforeSerialize(){}
		//public void OnAfterDeserialize(){}

		public MoveType Type = MoveType.DEFAULT;

		[SerializeField]
		private MoveAltitudeObject m_Altitude = null;
		public MoveAltitudeObject Altitude{
			get{ return m_Altitude = ( m_Altitude == null ? new MoveAltitudeObject() : m_Altitude ); }
			set{ Altitude.Copy( value ); }
		}

		[SerializeField]
		private MoveInfoObject m_Info = null;
		public MoveInfoObject Info{
			get{ return m_Info = ( m_Info == null ? new MoveInfoObject() : m_Info ); }
			set{ Info.Copy( value ); }
		}

		[SerializeField]
		private MoveDetourObject m_Detour = null;
		public MoveDetourObject Detour{
			get{ return m_Detour = ( m_Detour == null ? new MoveDetourObject() : m_Detour ); }
			set{ Detour.Copy( value ); }
		}

		[SerializeField]
		private MoveOrbitObject m_Orbit = null;
		public MoveOrbitObject Orbit{
			get{ return m_Orbit = ( m_Orbit == null ? new MoveOrbitObject() : m_Orbit ); }
			set{ Orbit.Copy( value ); }
		}
			
		[SerializeField]
		private MoveEscapeObject m_Escape = null;
		public MoveEscapeObject Escape{
			get{ return m_Escape = ( m_Escape == null ? new MoveEscapeObject() : m_Escape ); }
			set{ Escape.Copy( value ); }
		}

		[SerializeField]
		private MoveCoverObject m_Cover = null;
		public MoveCoverObject Cover{
			get{ return m_Cover = ( m_Cover == null ? new MoveCoverObject() : m_Cover ); }
			set{ Cover.Copy( value ); }
		}


		[SerializeField]
		private MoveAvoidObject m_Avoid = null;
		public MoveAvoidObject Avoid{
			get{ return m_Avoid = ( m_Avoid == null ? new MoveAvoidObject() : m_Avoid ); }
			set{ Avoid.Copy( value ); }
		}

		[SerializeField]
		private MoveMotionObject m_Motion = null;
		public MoveMotionObject Motion{
			get{ return m_Motion = ( m_Motion == null ? new MoveMotionObject() : m_Motion ); }
			set{ Motion.Copy( value ); }
		}
			
		public ViewingDirectionType ViewingDirection = ViewingDirectionType.DEFAULT;
		public Vector3 ViewingDirectionPosition = Vector3.zero;

		// DEFAULT
		public float StoppingDistance = 2;
		public float StoppingDistanceMaximum = 10;
		public float SegmentLength = 0;
		public float SegmentLengthMaximum = 100;
		public float SegmentVariance = 0;	
		public float DeviationLength = 0;
		public float DeviationLengthMaximum = 100;
		public float DeviationVariance = 0;		
		public bool IgnoreLevelDifference = true;

		//public MoveCompleteType Link = MoveCompleteType.DEFAULT;
		public string NextBehaviourModeKey = "";

		public void CopyDefault( MoveDataObject _move )
		{
			if( _move == null )
				return;

			StoppingDistance = _move.StoppingDistance;
			StoppingDistanceMaximum = _move.StoppingDistanceMaximum;
			IgnoreLevelDifference = _move.IgnoreLevelDifference;

			SegmentLength = _move.SegmentLength;
			SegmentLengthMaximum = _move.SegmentLengthMaximum;
			SegmentVariance = _move.SegmentVariance;
			DeviationLength = _move.DeviationLength;
			DeviationLengthMaximum = _move.DeviationLengthMaximum;
			DeviationVariance = _move.DeviationVariance;
		}

		public void Copy( MoveDataObject _move )
		{
			if( _move == null )
				return;

			base.Copy( _move );

			SetType( _move.Type );

			Altitude.Copy( _move.Altitude );

			Cover = _move.Cover;
			Detour = _move.Detour;
			Orbit = _move.Orbit;
			Escape = _move.Escape;
			Avoid = _move.Avoid;
			Motion.Copy( _move.Motion );

			ViewingDirection = _move.ViewingDirection;
			ViewingDirectionPosition = _move.ViewingDirectionPosition;

			StoppingDistance = _move.StoppingDistance;
			IgnoreLevelDifference = _move.IgnoreLevelDifference;

			SegmentLength = _move.SegmentLength;
			SegmentLengthMaximum = _move.SegmentLengthMaximum;
			SegmentVariance = _move.SegmentVariance;

			DeviationLength = _move.DeviationLength;
			DeviationLengthMaximum = _move.DeviationLengthMaximum;
			DeviationVariance = _move.DeviationVariance;

			//this.Link = _move.Link;
			NextBehaviourModeKey = _move.NextBehaviourModeKey;
		}

		protected virtual void SetType( MoveType _type ){
			this.Type = _type;
		}

		public float GetMoveDeviationVariance()
		{
			float _length = DeviationLength;
			float _variance = DeviationVariance;
			
			if(  _length > 0 && _variance > 0 )
				_length = _length * Random.Range( - _variance, _variance );
			
			return _length;
		}

		public float GetMoveSegmentLength()
		{
			float _directional_variance = SegmentVariance;
			float _segment_legth = SegmentLength;
			
			if( _segment_legth > 0 && _directional_variance > 0 )
				_segment_legth += _segment_legth * Random.Range( - _directional_variance, _directional_variance );

			return _segment_legth;
		}

		public float MoveSegmentLengthMax{
			get{ 
				float _directional_variance = SegmentVariance;
				float _segment_legth = SegmentLength;

				if( _directional_variance > 0 )
					_segment_legth += _segment_legth * _directional_variance;
				return _segment_legth;
			}
		}

		public float MoveSegmentLengthMin{
			get{ 
				float _directional_variance = SegmentVariance;
				float _segment_legth = SegmentLength;
				
				if( _directional_variance > 0 )
					_segment_legth += _segment_legth *  - _directional_variance;
				return _segment_legth;
			}
		}

		public float GetMaxMoveSegmentLength()
		{
			float _directional_variance = SegmentVariance;
			float _segment_legth = SegmentLength;
			
			if( _segment_legth > 0 && _directional_variance > 0 )
				_segment_legth *= Random.Range( - _directional_variance, _directional_variance );
			
			return _segment_legth;
		}
	}


	public class CurrentMoveDataObject : MoveDataObject
	{
		public CurrentMoveDataObject(){}
		public CurrentMoveDataObject( CurrentMoveDataObject _move ){ base.Copy( _move );  }

		protected override void SetType( MoveType _type ){

			if( _type == MoveType.ORBIT )
				m_HasOrbit = true;
			else
			{
				m_HasOrbit = false;
				m_OrbitComplete = false;
				OrbitRadius = 0;
				OrbitAngle = 0;
				OrbitDegrees = 10;
			}

			this.Type = _type;
		}

		protected bool m_HasOrbit = false;
		public bool HasOrbit{
			get{ return ( Type == MoveType.ORBIT ? true : false ); }
		}

		protected bool m_OrbitComplete = false;
		public bool OrbitComplete{
			get{return m_OrbitComplete;}
		}
		public void SetOrbitComplete( bool _value ){
			m_OrbitComplete = _value;
		}

		// ORBIT INFOS
		public float OrbitRadius = 0;
		public float OrbitAngle = 0;
		public float OrbitDegrees = 10;

		private float _drift_time ;
		private float _drift_timer;
		public Vector3 GetVelocity( ICECreatureControl _control, Vector3 _current_velocity )
		{
			Vector3 _velocity = _current_velocity;

			// FORWARD BEGIN
			if( _control.Creature.Move.MoveHandledByRootMotion )
				_velocity.z = ( _control.Creature.Move.AnimatorComponent.deltaPosition / Time.deltaTime ).magnitude;
			else
			{
				float _target_speed = ( _control.Creature.Move.CurrentTarget != null ? _control.Creature.Move.CurrentTarget.TargetSpeed : 0 );
				float _forwards = 0;

				if( Motion.UseTargetVelocity && _target_speed > 0 ) 
					_forwards = _target_speed + ( _target_speed * Motion.VelocityMultiplier ) + Motion.Velocity.z; 
				else
					_forwards = Motion.Velocity.z + ( Motion.Velocity.z * Motion.VelocityMultiplier ); 

				if( Motion.UseAdvancedVelocity && Motion.Inertia > 0 && _velocity.z < _forwards )
					_velocity.z += ( _forwards - _velocity.z ) * Motion.Inertia;
				else
					_velocity.z = _forwards;

				//_velocity.z *= _control.Creature.Status.SpeedMultiplier;
			}
			// FORWARD END


			/*
		
			// DRIFT BEGIN
			if( UseAutoDrift )
			{
				if( _drift_time == 0 || _drift_timer > _drift_time )
					_drift_time = Random.Range ( 0.5f, 3 );
				else
					_drift_timer += Time.deltaTime;

				if( _drift_time != 0 && _drift_timer > _drift_time )
				{
					_velocity.x += ( 0.1f * Random.Range ( - DriftMultiplier, DriftMultiplier ) );
					_drift_timer = 0;
				}
			}
			// DRIFT END	*/


			if( Altitude.Enabled )
				_velocity.y = ( Altitude.UseVerticalSpeedCurve ? Altitude.VerticalSpeedCurve.Evaluate( Mathf.Abs( _control.Creature.Move.CurrentOperatingLevelDifference ) ) : Motion.Velocity.y );
			else
				_velocity.y = 0;

			return _velocity;
		}
			
		public float GetYawSpeed( ICECreatureControl _control )
		{
			float _speed = 0;

			if( Motion.UseAutomaticAngularVelocity )
				_speed = MathTools.CalculateAngularVelocity( Motion.Velocity, Motion.VelocityMaximum, Motion.AngularVelocityMaximum );
			else
				_speed = Motion.AngularVelocity.y;

			return _speed;
		}
	}
}

