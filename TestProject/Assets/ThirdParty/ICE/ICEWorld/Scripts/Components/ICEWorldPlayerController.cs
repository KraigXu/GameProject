// ##############################################################################
//
// ICEWorldPlayerController.cs
// Version 1.4.0
// 
// Copyrights © Pit Vetterick, ICE Technologies Consulting LTD. All Rights Reserved.
// http://www.icecreaturecontrol.com
// mailto:support@icecreaturecontrol.com
//
// Unity Asset Store End User License Agreement (EULA)
// http://unity3d.com/legal/as_terms
//
// This controller based on the Unity Standard Assets First Person Controller 
// and will used in ICE demo scenes instead of the original Unity Standard Assets
// scripts to avoid conflicts with given namespaces. 
//
// ##############################################################################

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{

    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
	public class ICEWorldPlayerController : ICEWorldBehaviour
    {
        public bool IsRunning;
		public float WalkSpeed = 3;
		public float RunSpeed = 7;
		public float RunstepLenghten = 7;
		public float JumpSpeed = 20;
		public float StickToGroundForce = 7;
		public float GravityMultiplier = 9.82f;
		public ICEMouseLook MouseLook = new ICEMouseLook();
		public float StepInterval;
		public AnimationCurve Interval = new AnimationCurve();
		public List<AudioClip> FootstepSounds = new List<AudioClip>();    // an array of footstep sounds that will be randomly selected from.
		public AudioClip JumpSound = null;          // the sound played when character leaves the ground.
		public AudioClip LandSound = null;            // the sound played when character touches back on ground.

		public bool UseDebugMode = false;
		public KeyCode KeyFlightMode = KeyCode.Q;
		public KeyCode KeyUp = KeyCode.UpArrow;
		public KeyCode KeyDown = KeyCode.DownArrow;
		public int FlightSpeedMultiplier = 10;


        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        //private float m_StepCycle;
        //private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;
		private bool m_FlightMode = false;

		public bool FlightMode{
			get{ return m_FlightMode; }
		}

	
        // Use this for initialization
		public override void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
           // m_FovKick.Setup(m_Camera);
            //m_HeadBob.Setup(m_Camera, m_StepInterval);
            //m_StepCycle = 0f;
            //m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			MouseLook.Init(transform , m_Camera.transform);
        }


        // Update is called once per frame
		public override void Update()
        {
			if( UseDebugMode )
			{
				if( Input.GetKeyUp( KeyFlightMode ) )
					m_FlightMode = ! m_FlightMode;
			}
				
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if( ! m_Jump )
         		m_Jump = Input.GetKeyDown( KeyCode.Space );
    
            if( ! m_PreviouslyGrounded && m_CharacterController.isGrounded )
            {
               // StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }

            if( ! m_CharacterController.isGrounded && ! m_Jumping && m_PreviouslyGrounded )
         		m_MoveDir.y = 0f;
     
            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = LandSound;
            m_AudioSource.Play();
            //m_NextStep = m_StepCycle + .5f;
        }


		public override void FixedUpdate()
        {
            float _speed;
			GetInput( out _speed );
            // always move along the camera forward as it is the direction that it being aimed at
			Vector3 _desired_move = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast( transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
					m_CharacterController.height/2f, ~0, QueryTriggerInteraction.Ignore );
			
            _desired_move = Vector3.ProjectOnPlane( _desired_move, hitInfo.normal ).normalized;

            m_MoveDir.x = _desired_move.x * _speed;
            m_MoveDir.z = _desired_move.z * _speed;

			if( UseDebugMode && m_FlightMode )
			{
				if( Input.GetKey( KeyUp ) )
					m_MoveDir.y += 1;
				else if( Input.GetKey( KeyDown ) )
					m_MoveDir.y -= 1;
				else
					m_MoveDir.y = 0;
			}
			else if( m_CharacterController.isGrounded )
            {
				m_MoveDir.y = -StickToGroundForce;

                if( m_Jump )
                {
                    m_MoveDir.y = JumpSpeed;
                    PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
			else
            {
				m_MoveDir += Physics.gravity * GravityMultiplier * Time.fixedDeltaTime;
            }
	

            m_CollisionFlags = m_CharacterController.Move( m_MoveDir * Time.fixedDeltaTime );

            ProgressStepCycle(_speed);
            //UpdateCameraPosition(_speed);

            MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = JumpSound;
            m_AudioSource.Play();
        }

		private float m_IntervalTimer = 0;
		private void ProgressStepCycle( float _speed )
		{
			if( m_CharacterController.velocity.sqrMagnitude == 0 && (m_Input.x == 0 || m_Input.y == 0))
				_speed = 0;
				
			m_IntervalTimer += Time.fixedDeltaTime;
			StepInterval = Interval.Evaluate( _speed );

			if( StepInterval > 0 && m_IntervalTimer > StepInterval )
			{
				PlayFootStepAudio();
				m_IntervalTimer = 0;
			}
			else if( StepInterval == 0 )
			{
				m_AudioSource.Stop();
			}


		}


        private void PlayFootStepAudio()
        {
            if( ! m_CharacterController.isGrounded )
           		return;
         
			if( FootstepSounds.Count > 0 )
			{
				m_AudioSource.clip = FootstepSounds[ UnityEngine.Random.Range( 0, FootstepSounds.Count ) ];
	            m_AudioSource.PlayOneShot( m_AudioSource.clip );
			}
        }


		private void UpdateCameraPosition( float _speed )
        {
			Vector3 _new_camera_position;

            if( m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded )
            {
                _new_camera_position = m_Camera.transform.localPosition;
				_new_camera_position.y = m_Camera.transform.localPosition.y;// - m_JumpBob.Offset();
            }
            else
            {
                _new_camera_position = m_Camera.transform.localPosition;
				_new_camera_position.y = m_OriginalCameraPosition.y;// - m_JumpBob.Offset();
            }

            m_Camera.transform.localPosition = _new_camera_position;
        }


		private void GetInput( out float _speed )
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            IsRunning = Input.GetKey( KeyCode.LeftShift );
#endif
            // set the desired speed to be walking or running
			_speed  = ( IsRunning ? RunSpeed : WalkSpeed );
			m_Input = new Vector2( horizontal, vertical );

			if( UseDebugMode && m_FlightMode )
				_speed  = ( IsRunning ? RunSpeed : WalkSpeed ) * FlightSpeedMultiplier;

            // normalize input if it exceeds 1 in combined length:
            if( m_Input.sqrMagnitude > 1 )
           		 m_Input.Normalize();
        }


        private void RotateView()
        {
            MouseLook.LookRotation (transform, m_Camera.transform);
        }


		private void OnControllerColliderHit( ControllerColliderHit _hit )
        {
			Rigidbody _body = _hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if( m_CollisionFlags == CollisionFlags.Below )
        		 return;
    
            if( _body == null || _body.isKinematic )
          		return;
      
            _body.AddForceAtPosition( m_CharacterController.velocity * 0.1f, _hit.point, ForceMode.Impulse );
        }
    }
}
