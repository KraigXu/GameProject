// ##############################################################################
//
// ice_CameraUnderwater.cs | UnderwaterCameraEffect
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
	public class UnderwaterCameraEffect : ICEOwnerObject
	{
		public bool UseWaterZone = false;
		public float WaterLevel = 7;
		public float WaterLevelMaximum = 100;

		public bool FogEnabled = true;
		public Color FogColor = new Color(0, 0.4f, 0.7f, 0.6f);
		public float FogDensity = 0.04f;
		public Color UnderwaterBackgroundColor = new Color(0, 0.4f, 0.7f, 1);
		public Material UnderwaterSkybox = null;

		private bool m_DefaultFogEnabled;
		private Color m_DefaultFogColor;
		private float m_DefaultFogDensity;
		//TODO: private Color m_DefaultBackgroundColor;
		private Material m_DefaultSkybox;

		private Camera m_Camera = null;
		private Camera MainCamera{
			get{ return m_Camera = ( m_Camera == null ? ( OwnerComponent != null ? OwnerComponent.GetComponent<Camera>() : Camera.main ): m_Camera ); }
		}

		public UnderwaterCameraEffect(){}
		public UnderwaterCameraEffect( ICEWorldBehaviour _component ) : base( _component ){
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			if( Owner == null || OwnerComponent == null )
				return;

			m_DefaultFogEnabled = RenderSettings.fog;
			m_DefaultFogColor = RenderSettings.fogColor;
			m_DefaultFogDensity = RenderSettings.fogDensity;
			m_DefaultSkybox = RenderSettings.skybox;


			m_Camera = OwnerComponent.GetComponent<Camera>();
			if( m_Camera == null )
				m_Camera = Camera.main;

			if( m_Camera != null )
			{
				//TODO: m_DefaultBackgroundColor = m_Camera.backgroundColor;
				m_Camera.backgroundColor = UnderwaterBackgroundColor;
			}


			if( ! UseWaterZone )
				OwnerComponent.OnUpdate += Update;
		}

		public void Update() 
		{
			if( Owner == null || OwnerComponent == null || UseWaterZone )
				return;
			
			m_Camera = MainCamera;

			Vector3 _pos = Owner.transform.TransformPoint( m_Camera.gameObject.transform.position );
			if( _pos.y < WaterLevel )
				SetUnderwater( true );
			else
				SetUnderwater( false );
		}

		private bool m_IsUnderwater = false;
		public bool IsUnderwater{
			get{ return m_IsUnderwater; }
		}

		/// <summary>
		/// Sets the underwater.
		/// </summary>
		/// <param name="_is_underwater">If set to <c>true</c> is underwater.</param>
		public void SetUnderwater( bool _is_underwater )
		{
			if( _is_underwater == m_IsUnderwater )
				return;			
			else if( _is_underwater )
			{
				RenderSettings.fog = FogEnabled;
				RenderSettings.fogColor = FogColor;
				RenderSettings.fogDensity = FogDensity;
				RenderSettings.skybox = UnderwaterSkybox;
				m_IsUnderwater = true;
			}
			else
			{
				RenderSettings.fog = m_DefaultFogEnabled;
				RenderSettings.fogColor = m_DefaultFogColor;
				RenderSettings.fogDensity = m_DefaultFogDensity;
				RenderSettings.skybox = m_DefaultSkybox;
				m_IsUnderwater = false;
			}
		}

		/// <summary>
		/// Checks the collider enter or stay.
		/// </summary>
		/// <param name="_other">Other.</param>
		public void CheckColliderEnterOrStay( Collider _other )
		{
			if( ! UseWaterZone || _other == null || _other.gameObject == null || _other.gameObject.layer != 4 )
				return;

			SetUnderwater( true );
		}

		/// <summary>
		/// Checks the collider exit.
		/// </summary>
		/// <param name="_other">Other.</param>
		public void CheckColliderExit( Collider _other )
		{
			if( ! UseWaterZone || _other == null || _other.gameObject == null || _other.gameObject.layer != 4 )
				return;

			SetUnderwater( false );
		}
	}
}


