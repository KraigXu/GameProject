// ##############################################################################
//
// ice_CreatureSurface.cs
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

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.World.Utilities;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class TextureDataObject : ICEObject
	{
		public TextureDataObject(){}
		public TextureDataObject( TextureDataObject _object ){ Copy( _object ); }

		public void Copy( TextureDataObject _object )
		{
			if( _object == null )
				return;

			Image = _object.Image;
			FilePath = _object.FilePath;
		}

		[SerializeField]
		private Texture m_Image = null;
		[XmlIgnore]
		public Texture Image{
			get{
#if UNITY_EDITOR
				if( m_Image == null && ! string.IsNullOrEmpty( FilePath ) )
					m_Image = (Texture)UnityEditor.AssetDatabase.LoadAssetAtPath<Texture>( FilePath );
#endif
				return m_Image;
			}
			set{ 

				m_Image = value; 

#if UNITY_EDITOR
				if( m_Image != null )
					FilePath = UnityEditor.AssetDatabase.GetAssetPath( m_Image );
#endif
			
			}
		}

		public string FilePath = "";
	}

	[System.Serializable]
	public class SurfaceDataObject : ICEDataObject
	{
		public SurfaceDataObject(){
			Foldout = true;
			Enabled = true;
		}
		public SurfaceDataObject( SurfaceDataObject _object ) : base( _object ) { Copy( _object ); }

		public void Copy( SurfaceDataObject _object )
		{
			if( _object == null )
				return;
			
			base.Copy( _object );

			Name = _object.Name;
			UseBehaviourModeKey = _object.UseBehaviourModeKey;
			BehaviourModeKey = _object.BehaviourModeKey;

			Audio.Copy( _object.Audio );
			Effect.Copy( _object.Effect );
			Influences.Copy( _object.Influences );

			Textures = _object.Textures;
		}

		public string Name = ""; 

		public bool UseBehaviourModeKey = false;
		public string BehaviourModeKey = "";

		[SerializeField]
		private List<TextureDataObject> m_Textures = null;
		public List<TextureDataObject> Textures{
			get{ return m_Textures = ( m_Textures == null ? new List<TextureDataObject>(): m_Textures ); }
			set{ 		
				Textures.Clear();
				if( value == null ) return;	
				foreach( TextureDataObject _data in value )
					Textures.Add( new TextureDataObject( _data ) );  
			}
		}

		[SerializeField]
		private FootstepAudioObject m_Footsteps = null;
		public FootstepAudioObject Footsteps{
			get{ return m_Footsteps = ( m_Footsteps == null ? new FootstepAudioObject(): m_Footsteps ); }
			set{ Footsteps.Copy( value ); }
		}

		[SerializeField]
		private AudioObject m_Audio = null;
		public AudioObject Audio{
			get{ return m_Audio = ( m_Audio == null ? new AudioObject(): m_Audio ); }
			set{ Audio.Copy( value ); }
		}

		[SerializeField]
		private EffectObject m_Effect = null;
		public EffectObject Effect{
			get{ return m_Effect = ( m_Effect == null ? new EffectObject(): m_Effect ); }
			set{ Effect.Copy( value ); }
		}

		[SerializeField]
		private InfluenceObject m_Influences = null;
		public InfluenceObject Influences{
			get{ return m_Influences = ( m_Influences == null ? new InfluenceObject():m_Influences ); }
			set{ Influences.Copy( value ); }
		}

	}
	
	[System.Serializable]
	public class SurfaceObject : ICEOwnerObject
	{
		public SurfaceObject(){}
		public SurfaceObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component ); }
		public SurfaceObject( SurfaceObject _object ) : base( _object ) { Copy( _object ); }

		private string m_ActiveTextureName = "";
		public string ActiveTextureName{
			get{ return m_ActiveTextureName; }
		}

		private SurfaceDataObject m_ActiveSurface = null;
		public SurfaceDataObject ActiveSurface{
			get{ return m_ActiveSurface; }
		}
		
		public List<SurfaceDataObject> Surfaces = new List<SurfaceDataObject>();

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			AudioPlayer.Init( _component );
			FootstepPlayer.Init( _component );
		}

		public void Copy( SurfaceObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			AudioPlayer.Copy( _object.AudioPlayer );
			FootstepPlayer.Copy( _object.FootstepPlayer );

			GroundScanInterval = _object.GroundScanInterval;
			GroundScanIntervalMaximum = _object.GroundScanIntervalMaximum;

			Surfaces.Clear();
			foreach( SurfaceDataObject _data in _object.Surfaces )
				Surfaces.Add( new SurfaceDataObject( _data ) );
		}

		public override void Reset()
		{
			Surfaces.Clear();	
			m_AudioPlayer = new AudioPlayerObject( OwnerComponent );

			m_GroundScanIntervalTimer = 0;
			GroundScanInterval = 1;
			Enabled = false;
			Foldout = false;
		}

		[SerializeField]
		private FootstepAudioPlayerObject m_FootstepPlayer = null;
		public FootstepAudioPlayerObject FootstepPlayer{
			get{ return m_FootstepPlayer = ( m_FootstepPlayer == null ? new FootstepAudioPlayerObject( OwnerComponent ): m_FootstepPlayer ); }
			set{ FootstepPlayer.Copy( value ); }
		}

		[SerializeField]
		private AudioPlayerObject m_AudioPlayer = null;
		public AudioPlayerObject AudioPlayer{
			get{ return m_AudioPlayer = ( m_AudioPlayer == null ? new AudioPlayerObject( OwnerComponent ):m_AudioPlayer ); }
			set{ AudioPlayer.Copy( value ); }
		}
			
		private float m_GroundScanIntervalTimer = 0;
		public float GroundScanInterval = 1;
		public float GroundScanIntervalMaximum = 30;

		/// <summary>
		/// Update surface handling.
		/// </summary>
		public void Update( CreatureObject _creature )
		{
			if( Enabled == false || _creature == null || _creature.Move.IsGrounded == false )
				return;

			Vector3 _velocity = _creature.Move.DesiredVelocity;
			if( _creature.Behaviour.ActiveBehaviourMode != null && _creature.Behaviour.ActiveBehaviourMode.Rule != null )
				_velocity = _creature.Behaviour.ActiveBehaviourMode.Rule.Move.Motion.Velocity;


			// Update Ground Texture Name - only required while creature is moving or texture name is empty
			if( _velocity.z > 0 || m_ActiveTextureName == "" )
			{
				string _detected_texture_name = m_ActiveTextureName;

				m_GroundScanIntervalTimer += Time.deltaTime;
				if( m_GroundScanIntervalTimer >= GroundScanInterval )
				{
					m_GroundScanIntervalTimer = 0;
					_detected_texture_name = _creature.Move.UpdateGroundTextureName();
				}

				if( _detected_texture_name != m_ActiveTextureName )
				{
					m_ActiveTextureName = _detected_texture_name;

					SurfaceDataObject _best_surface = null;
					foreach( SurfaceDataObject _surface in Surfaces)
					{
						foreach( TextureDataObject _texture in _surface.Textures )
						{
							if( _texture != null && _texture.Image != null && _texture.Image.name == m_ActiveTextureName )
							{
								_best_surface = _surface;
								break;
							}
						}
					}

					if( _best_surface != null )
					{
						if( m_ActiveSurface != _best_surface )
						{
							if( m_ActiveSurface != null )
								m_ActiveSurface.Effect.Stop();
							
							m_ActiveSurface = _best_surface;
							m_ActiveSurface.Effect.Start( OwnerComponent );

							AudioPlayer.Play( m_ActiveSurface.Audio );
							FootstepPlayer.SetData( m_ActiveSurface.Footsteps );
						}
					}
					else
					{
						if( m_ActiveSurface != null )
							m_ActiveSurface.Effect.Stop();

						m_ActiveSurface = null;

						FootstepPlayer.SetData( null );
						AudioPlayer.Stop();
					}
				}
			}

			if( _velocity.z == 0 )
				AudioPlayer.Stop();
			else
			{
				AudioPlayer.Update();
				FootstepPlayer.Update( _velocity.z );
			}
		}
	}
}