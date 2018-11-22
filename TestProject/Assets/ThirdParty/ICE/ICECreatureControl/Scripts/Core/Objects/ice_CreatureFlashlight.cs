// ##############################################################################
//
// ice_CreatureFlashlightObject.cs
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
using ICE.World;
using ICE.World.Objects;

using ICE.World.Utilities;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class FlashlightObject : ICEOwnerObject
	{
		[SerializeField]
		private Light m_Light = null;
		public Light Light{
			get{ return m_Light = ( m_Light == null && Owner != null ? Owner.GetComponentInChildren<Light>() : m_Light );}
			set{ m_Light = value; }
		}

		public FlashlightObject(){}
		public FlashlightObject( ICEWorldBehaviour _component ) : base( _component ) {
			Init( _component );
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init (_component);

			if( Light != null && Application.isPlaying )
				Light.enabled = false;
		}

		public bool UseActivateByLightIntensity = true;
		public float LightIntensityLimit = 1f;

		public bool IsActive{
			set{ if( Light != null )Light.enabled = ( Enabled ? value : false ); }
			get{ return ( Light != null ?Light.enabled:false ); }
		}

		public void Update()
		{
			if( ! Enabled )
				IsActive = false;
			else 
			{
				if( UseActivateByLightIntensity && ICEWorldEnvironment.Instance != null )
				{
					float _intensity = ICEWorldEnvironment.Instance.LightIntensity;
					IsActive = ( _intensity <= LightIntensityLimit ? true : false );
				}
			}
		}

	}
}

