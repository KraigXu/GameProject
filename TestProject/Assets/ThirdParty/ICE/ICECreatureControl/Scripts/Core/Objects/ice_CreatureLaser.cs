// ##############################################################################
//
// ice_CreatureLaser.cs
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
	public class LaserObject : ICEOwnerObject
	{
		public LaserObject(){}
		public LaserObject( ICEWorldBehaviour _component ) : base( _component ) 
		{
			
		}

		public override void Init (ICEWorldBehaviour _component)
		{
			base.Init (_component);

			if( _component != null && Enabled && _component.GetComponent<LineRenderer>() != null )
			{
				m_Transform = _component.transform;
				m_LineRenderer = _component.GetComponent<LineRenderer>();

				if( m_LineRenderer != null )
				{
					#if UNITY_5_5 || UNITY_5_5_OR_NEWER
					m_LineRenderer.startWidth = Width;
					m_LineRenderer.endWidth = Width;
					#else
					m_LineRenderer.SetWidth( Width, Width );
					#endif
					m_LineRenderer.material = new Material(Shader.Find("Particles/Additive"));
				}
			
				m_LaserOffset = new Vector3(0,0,0);
				EndEffect = _component.GetComponentInChildren<ParticleSystem>();

				if( EndEffect != null )
					m_LaserEndEffectTransform = EndEffect.transform;
			}
		}
			
		private Transform m_Transform = null;
		public bool IsActive = false;
		public float Width = 0.1f;
		public float Noise = 0f;
		public float LengthMax = 50.0f;
		public float WidthMaximum = 1.0f;
		public float NoiseMaximum = 1.0f;
		public float LengthMaximum = 50.0f;
		public Vector3 Offset = Vector3.zero;
		public Color StartColor = Color.red;
		public Color EndColor = Color.red;
		public ParticleSystem EndEffect;

		private LineRenderer m_LineRenderer;
		private int m_LaserLength;
		private Vector3[] m_LaserPosition;
		private Vector3 m_LaserEndPosition;

		private Transform m_LaserEndEffectTransform;
		private Vector3 m_LaserOffset;

		/// <summary>
		/// Renders the laser.
		/// </summary>
		public void RenderLaser()
		{
			if( Enabled == false || IsActive == false || m_LineRenderer == null )
			{
				if( m_LineRenderer != null )
					m_LineRenderer.enabled = false;
				return;
			}
			else
			{
				m_LineRenderer.enabled = true;
				#if UNITY_5_5 || UNITY_5_5_OR_NEWER
					m_LineRenderer.startWidth = Width;
					m_LineRenderer.endWidth = Width;
				#else
					m_LineRenderer.SetWidth( Width, Width );
				#endif
			}
				
			RaycastHit[] _hit = Physics.RaycastAll( m_Transform.position, m_Transform.forward, LengthMax );

			for( int _i = 0 ; _i < _hit.Length ; _i++ )
			{
				if( ! _hit[_i].collider.isTrigger)
				{
					m_LaserLength = (int)Mathf.Round( _hit[_i].distance ) + 2;
					m_LaserPosition = new Vector3[ m_LaserLength ];
					m_LaserEndPosition = _hit[_i].point;

					if( EndEffect )
					{
						m_LaserEndEffectTransform.position = m_LaserEndPosition;
						if( ! EndEffect.isPlaying )
							EndEffect.Play();
					}

          #if UNITY_5_6_OR_NEWER
            m_LineRenderer.positionCount = m_LaserLength;
					#elif UNITY_5_5 || UNITY_5_5_OR_NEWER
						m_LineRenderer.numPositions = m_LaserLength;
					#else
						m_LineRenderer.SetVertexCount( m_LaserLength );
					#endif

				
					return;
				}
			}

			if( EndEffect )
			{
				if( EndEffect.isPlaying )
					EndEffect.Stop();
			}

			m_LaserLength = (int)LengthMax;
			m_LaserPosition = new Vector3[ m_LaserLength ];

      #if UNITY_5_6_OR_NEWER
        m_LineRenderer.positionCount = m_LaserLength;

				m_LineRenderer.startColor = StartColor;
				m_LineRenderer.endColor = EndColor;
			#elif UNITY_5_5 || UNITY_5_5_OR_NEWER
				m_LineRenderer.numPositions = m_LaserLength;

				m_LineRenderer.startColor = StartColor;
				m_LineRenderer.endColor = EndColor;
			#else
				m_LineRenderer.SetVertexCount( m_LaserLength );
				m_LineRenderer.SetColors( StartColor, EndColor );
			#endif

			m_LaserEndPosition = m_Transform.position + ( m_Transform.forward  * m_LaserLength );

			for( int i = 0; i<m_LaserLength; i++ )
			{
				Vector3 _pos = m_Transform.TransformPoint( Offset );
				//Set the position here to the current location and project it in the forward direction of the object it is attached to
				m_LaserOffset.x = _pos.x + i * m_Transform.forward.x + Random.Range( -Noise,Noise );
				m_LaserOffset.z = _pos.z + i * m_Transform.forward.z + Random.Range( -Noise,Noise );
				m_LaserOffset.y = Mathf.Lerp( _pos.y, m_LaserEndPosition.y, MathTools.Normalize( i, 0, m_LaserLength ) ) + i * m_Transform.forward.y + Random.Range( -Noise,Noise );
				m_LaserPosition[i] = m_LaserOffset;
				m_LaserPosition[0] = _pos;

				m_LineRenderer.SetPosition(i, m_LaserPosition[i]);

			}
		}
	}
}
