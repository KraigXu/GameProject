// ##############################################################################
//
// ICE.World.ICEWorldCamera.cs
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

using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	using UnityEngine;
	using System.Collections;

	/// <summary>
	/// ICE world camera.
	/// </summary>
	public class ICEWorldCamera : ICEWorldBehaviour {

		[SerializeField]
		private UnderwaterCameraEffect m_UnderwaterEffect = null;
		public UnderwaterCameraEffect Underwater{
			get{ return m_UnderwaterEffect = ( m_UnderwaterEffect == null ? new UnderwaterCameraEffect( this ) : m_UnderwaterEffect ); }
			set{ Underwater.Copy( value ); }
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		public override void Start () {
			Underwater.Init( this );
		}
			
		/// <summary>
		/// Raises the trigger enter event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerEnter( Collider _other ){
			Underwater.CheckColliderEnterOrStay( _other );
		}

		/// <summary>
		/// Raises the trigger stay event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerStay( Collider _other ){
			Underwater.CheckColliderEnterOrStay( _other );
		}

		/// <summary>
		/// Raises the trigger exit event.
		/// </summary>
		/// <param name="_other">Other.</param>
		public virtual void OnTriggerExit( Collider _other ){
			Underwater.CheckColliderExit( _other );
		}
	}
}
