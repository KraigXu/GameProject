// ##############################################################################
//
// ice_objects_mouselook.cs | ICEMouseLook
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class ICEMouseLook : ICEDataObject
	{
		public float SensitivityX = 2f;
		public float SensitivityY = 2f;
		public bool ClampVerticalRotation = true;
		public float MinimumX = -90F;
		public float MaximumX = 90F;
		public bool UseSmoothRotation = true;
		public float SmoothRotationSpeed = 5f;
		public float SmoothRotationSpeedMaximum = 25f;
		public bool LockCursor = true;

		private Quaternion m_CharacterTargetRotation;
		private Quaternion m_CameraTargetRotation;
		private bool m_CursorIsLocked = true;

		/// <summary>
		/// Init the mouse look by the specified _character and _camera.
		/// </summary>
		/// <param name="_character">Character.</param>
		/// <param name="_camera">Camera.</param>
		public void Init( Transform _character, Transform _camera )
		{
			if( _character == null || _camera == null )
				return;
			
			m_CharacterTargetRotation = _character.localRotation;
			m_CameraTargetRotation = _camera.localRotation;
		}
			
		/// <summary>
		/// Looks the rotation.
		/// </summary>
		/// <param name="_character">Character.</param>
		/// <param name="_camera">Camera.</param>
		public void LookRotation( Transform _character, Transform _camera )
		{
			if( _character == null || _camera == null )
				return;
			
			float _y_rot = Input.GetAxis("Mouse X") * SensitivityX;
			float _x_rot = Input.GetAxis("Mouse Y") * SensitivityY;

			m_CharacterTargetRotation *= Quaternion.Euler (0f, _y_rot, 0f);
			m_CameraTargetRotation *= Quaternion.Euler (-_x_rot, 0f, 0f);

			if( ClampVerticalRotation )
				m_CameraTargetRotation = ClampRotationAroundXAxis( m_CameraTargetRotation );

			if( UseSmoothRotation )
			{
				_character.localRotation = Quaternion.Slerp( _character.localRotation, m_CharacterTargetRotation, SmoothRotationSpeed * Time.deltaTime );
				_camera.localRotation = Quaternion.Slerp( _camera.localRotation, m_CameraTargetRotation, SmoothRotationSpeed * Time.deltaTime );
			}
			else
			{
				_character.localRotation = m_CharacterTargetRotation;
				_camera.localRotation = m_CameraTargetRotation;
			}

			UpdateCursorLock();
		}

		/// <summary>
		/// Sets the cursor lock.
		/// </summary>
		/// <param name="value">If set to <c>true</c> value.</param>
		public void SetCursorLock( bool _value )
		{
			LockCursor = _value;
			if( ! LockCursor )
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		/// <summary>
		/// Updates the cursor lock.
		/// </summary>
		public void UpdateCursorLock()
		{
			if ( LockCursor )
				InternalLockUpdate();
		}

		/// <summary>
		/// Internals the lock update.
		/// </summary>
		private void InternalLockUpdate()
		{
			if( Input.GetKeyUp( KeyCode.Escape ) )
			{
				m_CursorIsLocked = false;
			}
			else if( Input.GetMouseButtonUp(0) )
			{
				m_CursorIsLocked = true;
			}

			if( m_CursorIsLocked )
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			else if( ! m_CursorIsLocked )
			{
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		}

		/// <summary>
		/// Clamps the rotation around X axis.
		/// </summary>
		/// <returns>The rotation around X axis.</returns>
		/// <param name="q">Q.</param>
		Quaternion ClampRotationAroundXAxis( Quaternion _quaternation )
		{
			_quaternation .x /= _quaternation .w;
			_quaternation .y /= _quaternation .w;
			_quaternation .z /= _quaternation .w;
			_quaternation .w = 1.0f;

			float _x_angle = 2.0f * Mathf.Rad2Deg * Mathf.Atan (_quaternation .x);

			_x_angle = Mathf.Clamp( _x_angle, MinimumX, MaximumX );

			_quaternation.x = Mathf.Tan( 0.5f * Mathf.Deg2Rad * _x_angle );

			return _quaternation ;
		}

	}

}
