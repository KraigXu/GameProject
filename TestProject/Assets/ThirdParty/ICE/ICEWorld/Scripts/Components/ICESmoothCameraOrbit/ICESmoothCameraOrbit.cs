// ##############################################################################
//
// ICESmoothCameraOrbit.cs
// Version 1.4.0
//
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// Note: parts of this script based on the maxCamera.cs script of Share Alike
// The original content is available under: http://wiki.unity3d.com/index.php?title=MouseOrbitZoom
//
// ##############################################################################

using UnityEngine;
using System.Collections;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	public class ICESmoothCameraOrbit : ICEWorldBehaviour
	{
	    public Transform CameraTarget;
	    public Vector3 CameraTargetOffset;

	    public float Distance = 5.0f;
		public float DistanceMaximum = 100;
	    public float MaxDistance = 20;
	    public float MinDistance = .6f;
		public float FollowSpeed = 0;
		public float FollowSpeedMaximum = 25.0f;
	    public float HorizontalSpeed = 200.0f;
		public float HorizontalSpeedMaximum = 500.0f;
	    public float VerticalSpeed = 200.0f;
		public float VerticalSpeedMaximum = 500.0f;
	    public int VerticalMinLimit = -80;
	    public int VerticalMaxLimit = 80;
		public int VerticalLimitsMaximum = 200;
	    public int ZoomRate = 40;
		public int ZoomRateMaximum = 100;
	   // public float PanSpeed = 0.3f;
	    public float ZoomDampening = 5.0f;
		public float ZoomDampeningMaximum = 10.0f;
		public float AutoRotate = 1f;
		public float AutoRotateMaximum = 10f;
		public float AutoRotateSpeed = 0.1f;
		public float AutoRotateSpeedMaximum = 1f;
		
	    private float m_xDeg = 0.0f;
		private float m_yDeg = 0.0f;
	    private float m_current_distance;
	    private float m_desired_distance;
	    private Quaternion m_current_rotation;
	    private Quaternion m_desired_rotation;
	    private Quaternion m_rotation;
	    private Vector3 m_position;
		private Vector3 m_temp_position;
		private float m_idle_timer = 0.0f;
		private float m_idle_smooth = 0.0f;
		
		public override void Start(){ 
			Init(); 
		}

		public override void OnEnable(){ 
			Init(); 
		}

	    public void Init()
	    {
			if( ! TargetReady() )
				return;

	        //distance = Vector3.Distance(transform.position, target.position);
	        m_current_distance = Distance;
	        m_desired_distance = Distance;
	               
	        //be sure to grab the current rotations as starting points.
	        m_position = transform.position;
	        m_rotation = transform.rotation;
	        m_current_rotation = transform.rotation;
	        m_desired_rotation = transform.rotation;
	       
	        m_xDeg = Vector3.Angle(Vector3.right, transform.right );
	        m_yDeg = Vector3.Angle(Vector3.up, transform.up );


			m_position = CameraTarget.position - (m_rotation * Vector3.forward * m_current_distance + CameraTargetOffset);
	    }

		public bool TargetReady()
		{
			if( CameraTarget != null && CameraTarget.gameObject.activeInHierarchy )
				return true;
			
			ICEWorldEntity[] _entities = GameObject.FindObjectsOfType<ICEWorldEntity>();
			if( _entities != null && _entities.Length > 0 )
			{
				ICEWorldEntity _entity = _entities[ UnityEngine.Random.Range( 0, _entities.Length ) ];
				if( _entity != null )
					CameraTarget = _entity.ObjectTransform;
			}

			if( CameraTarget != null && CameraTarget.gameObject.activeInHierarchy )
				return true;
			else
				return false;
		}

	    /*
	     * Camera logic on LateUpdate to only update after all character movement logic has been handled.
	     */
		public override void LateUpdate()
	    {
			if( ! TargetReady() )
				return;
				
			float _f = 0.02f;

	        // If Control and Alt and Middle button? ZOOM!
	        if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
	        {
				m_desired_distance -= Input.GetAxis( "Mouse Y" ) * _f  * ZoomRate * 0.125f * Mathf.Abs( m_desired_distance );
	        }

	        // If middle mouse and left alt are selected? ORBIT
	        else if (Input.GetMouseButton(0) )
	        {
				m_xDeg += Input.GetAxis("Mouse X") * HorizontalSpeed * _f;
				m_yDeg -= Input.GetAxis("Mouse Y") * VerticalSpeed * _f;
				            //Clamp the vertical axis for the orbit
	            m_yDeg = MathTools.ClampAngle360(m_yDeg, VerticalMinLimit, VerticalMaxLimit);
	            // set camera rotation
	            m_desired_rotation = Quaternion.Euler(m_yDeg, m_xDeg, 0);
	            m_current_rotation = transform.rotation;
				m_rotation = Quaternion.Lerp(m_current_rotation, m_desired_rotation, _f * ZoomDampening);
	        	transform.rotation = m_rotation;
				///////// Reset idle timers
				m_idle_timer=0;
	            m_idle_smooth=0;
	       
			}
			else
			{
			    // SMOOTH IDLE ROTATION BEGIN
				m_idle_timer += _f;
				if( AutoRotate > 0 && m_idle_timer > AutoRotate )
				{
					m_idle_smooth += ( _f + m_idle_smooth ) * 0.005f;
					m_idle_smooth = Mathf.Clamp( m_idle_smooth, 0, 1 );
					m_xDeg += HorizontalSpeed * m_idle_smooth * AutoRotateSpeed * Time.deltaTime;
				}
				// SMOOTH IDLE ROTATION END
				
				// SMOOTH EXIT BEGIN
	            //Clamp the vertical axis for the orbit
	            m_yDeg = MathTools.ClampAngle360( m_yDeg, VerticalMinLimit, VerticalMaxLimit );
	            m_desired_rotation = Quaternion.Euler( m_yDeg, m_xDeg, 0 );
	            m_current_rotation = transform.rotation;
				m_rotation = Quaternion.Lerp( m_current_rotation, m_desired_rotation, _f * ZoomDampening * 2 );
	        	transform.rotation = m_rotation;
				// SMOOTH EXIT END
			}

	        //OrbitRadius Position
	        // affect the desired Zoom distance if we roll the scrollwheel
			m_desired_distance -= Input.GetAxis( "Mouse ScrollWheel" ) * _f  * ZoomRate * Mathf.Abs( m_desired_distance );
	        //clamp the zoom min/max
	        m_desired_distance = Mathf.Clamp( m_desired_distance, MinDistance, MaxDistance );
	        // For smoothing of the zoom, lerp distance
			m_current_distance = Mathf.Lerp( m_current_distance, m_desired_distance, _f * ZoomDampening );

			if( FollowSpeed > 0 )
				m_temp_position = Vector3.Lerp( m_temp_position, CameraTarget.position, FollowSpeed * Time.deltaTime );
			else
				m_temp_position = CameraTarget.position;

	        // calculate position based on the new currentDistance
			m_position = m_temp_position - ( m_rotation * Vector3.forward * m_current_distance + CameraTargetOffset );
	        transform.position = m_position;
	    }
	}
}