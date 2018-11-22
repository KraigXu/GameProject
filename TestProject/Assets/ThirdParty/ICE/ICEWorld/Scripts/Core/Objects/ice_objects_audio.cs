// ##############################################################################
//
// ice_objects_audio.cs | ICE.World.Objects.AudioDataObject | AudioObject | AudioSourceObject
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
using UnityEngine.Audio;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE.World;
using ICE.World.Objects;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class FootstepAudioPlayerObject : ICEOwnerObject
	{
		public FootstepAudioPlayerObject(){}
		public FootstepAudioPlayerObject( ICEWorldBehaviour _component  ) : base( _component ) {}
		public FootstepAudioPlayerObject( FootstepAudioPlayerObject _audio ) : base( _audio ) { Copy( _audio ); }

		private AudioSource m_AudioSource = null;
		public AudioSource Source{
			get{ return m_AudioSource = ( m_AudioSource == null && Owner != null && Application.isPlaying ? Owner.AddComponent<AudioSource>() : m_AudioSource );}
		}

		private FootstepAudioDataObject m_AudioData = null;
		public FootstepAudioDataObject AudioData{
			get{ return m_AudioData;}
		}

		public void SetData( FootstepAudioDataObject _data )
		{
			m_AudioData = _data;
			Prepare();
		}


		private float m_IntervalTimer = 0;
		public void Update( float _speed )
		{
			if( Source == null || m_AudioData == null || m_AudioData.Clips.Count == 0 )
				return;
			
			m_IntervalTimer += Time.deltaTime;
			float _interval = m_AudioData.Interval.Evaluate( _speed );

			if( _interval > 0 && m_IntervalTimer > _interval )
			{
				PlayOneShot();
				m_IntervalTimer = 0;
			}
			else if( _interval == 0 && Source != null )
			{
				Source.Stop();
			}
		}

		/// <summary>
		/// Plays the footstep sound.
		/// </summary>
		private void PlayOneShot()
		{
			if( Source == null || m_AudioData == null || m_AudioData.Clips.Count == 0 )
				return;

			if( ! Source.isActiveAndEnabled )
				return;
			
			Source.clip = m_AudioData.Clips[ Random.Range( 0, m_AudioData.Clips.Count ) ];
			Source.PlayOneShot( Source.clip );
		}

		private void Prepare()
		{
			if( Source == null || m_AudioData == null )
				return;

			Source.volume = m_AudioData.Volume;
			Source.pitch = Random.Range( m_AudioData.MinPitch, m_AudioData.MaxPitch ) * Time.timeScale;
			Source.rolloffMode = m_AudioData.RolloffMode;	
			Source.minDistance = m_AudioData.MinDistance;
			Source.maxDistance = m_AudioData.MaxDistance;
			Source.spatialBlend = 1.0f;		
			Source.loop = false;

			if( m_AudioData.MixerGroup != null )
				Source.outputAudioMixerGroup = m_AudioData.MixerGroup;
		}

	}
		
	[System.Serializable]
	public class FootstepAudioDataObject : ICEDataObject
	{
		public FootstepAudioDataObject(){}
		public FootstepAudioDataObject( FootstepAudioDataObject _audio ) : base( _audio ) { Copy( _audio ); }

		public void Copy( FootstepAudioDataObject _audio )
		{
			if( _audio == null )
				return;
			
			Interval = _audio.Interval;

			Enabled = _audio.Enabled;
			Foldout = _audio.Foldout;
			MaxDistance = _audio.MaxDistance;
			MinDistance =_audio.MinDistance;
			DistanceMaximum = _audio.DistanceMaximum;
			MaxPitch = _audio.MaxPitch;
			MinPitch = _audio.MinPitch;
			PitchMaximum = _audio.PitchMaximum;
			RolloffMode = _audio.RolloffMode;
			Volume = _audio.Volume;
			MixerGroup = _audio.MixerGroup;

			Clips.Clear();
			foreach( AudioClip _clip in _audio.Clips )
				Clips.Add( _clip );
		}

		[XmlIgnore]
		public List<AudioClip> Clips{
			get{ return m_Clips = ( m_Clips == null ? new List<AudioClip>(): m_Clips );}
		}

		[SerializeField]
		protected List<AudioClip> m_Clips = new List<AudioClip>(); 

		public AnimationCurve Interval = new AnimationCurve();

		public float Volume = 0.5f;	
		public float MinPitch = 1.0f;	
		public float MaxPitch = 1.5f;
		public float PitchMaximum = 10;
		public float MinDistance = 2;	
		public float MaxDistance = 7;	
		public float DistanceMaximum = 7;	
		public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;

		[XmlIgnore]
		public AudioMixerGroup MixerGroup = null;
	}

	[System.Serializable]
	public class FootstepAudioObject : FootstepAudioDataObject
	{
		public FootstepAudioObject(){}
		public FootstepAudioObject( FootstepAudioObject _audio ) : base( _audio as FootstepAudioObject ){}

		private AudioClip m_Selected = null;
		[XmlIgnore]
		public AudioClip Selected{
			get{return m_Selected = ( m_Selected == null ? GetNextClip() : m_Selected ); }
		}

		public void AddClip( AudioClip _clip )
		{
			if( _clip != null )
				Clips.Add( _clip );
		}

		public void AddClip()
		{
			m_Clips.Add(AudioClip.Create("new AudioClip",1,1,1,false));
		}

		public void DeleteClip( int _index )
		{
			if( _index >= 0 && _index < m_Clips.Count )
				m_Clips.RemoveAt( _index );
		}

		public AudioClip GetNextClip()
		{
			if( m_Clips.Count == 0 )
				return null;

			reroll:	
			AudioClip _clip = m_Clips[Random.Range(0,m_Clips.Count)];

			if( _clip != null )
			{
				if ( m_Clips.Count > 1 && _clip == m_Selected )
					goto reroll;

				m_Selected = _clip;
			}

			return m_Selected;
		}
	}
		
	[System.Serializable]
	public class AudioDataObject : ICEImpulsTimerObject
	{
		public AudioDataObject(){}
		public AudioDataObject( ICEWorldBehaviour _component ) : base( _component ){}
		public AudioDataObject( AudioDataObject _audio ) : base( _audio as ICEImpulsTimerObject ) { Copy( _audio ); }

		public void Copy( AudioObject _audio )
		{
			base.Copy( _audio );

			SetImpulseData( _audio );

			Enabled = _audio.Enabled;
			Foldout = _audio.Foldout;
			Loop = _audio.Loop;
			Break = _audio.Break;
			StopAtEnd = _audio.StopAtEnd;
			MaxDistance = _audio.MaxDistance;
			MinDistance =_audio.MinDistance;
			DistanceMaximum = _audio.DistanceMaximum;
			MaxPitch = _audio.MaxPitch;
			MinPitch = _audio.MinPitch;
			PitchMaximum = _audio.PitchMaximum;
			RolloffMode = _audio.RolloffMode;
			Volume = _audio.Volume;
			MixerGroup = _audio.MixerGroup;

			Clips.Clear();
			foreach( AudioClip _clip in _audio.Clips )
				Clips.Add( _clip );
		}

		[XmlIgnore]
		public List<AudioClip> Clips{
			get{ return m_Clips = ( m_Clips == null ? new List<AudioClip>(): m_Clips );}
		}

		[SerializeField]
		protected List<AudioClip> m_Clips = new List<AudioClip>(); 

		public float Volume = 0.5f;	
		public float MinPitch = 1.0f;	
		public float MaxPitch = 1.5f;
		public float PitchMaximum = 10;
		public float MinDistance = 2;	
		public float MaxDistance = 7;	
		public float DistanceMaximum = 7;	
		public bool Loop = false;
		public bool Break = false;
		public bool StopAtEnd = true;
		public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;

		[XmlIgnore]
		public AudioMixerGroup MixerGroup = null;
	}

	[System.Serializable]
	public class AudioObject : AudioDataObject
	{
		public AudioObject(){}
		public AudioObject( ICEWorldBehaviour _component ) : base( _component ){}
		public AudioObject( AudioObject _audio ) : base( _audio as AudioDataObject ){}

		private AudioClip m_Selected = null;
		[XmlIgnore]
		public AudioClip Selected{
			get{return m_Selected = ( m_Selected == null ? GetNextClip() : m_Selected ); }
		}
			
		public void AddClip( AudioClip _clip )
		{
			if( _clip != null )
				Clips.Add( _clip );
		}

		public void AddClip()
		{
			m_Clips.Add(AudioClip.Create("1",1,1,1,true));
            

		}

		public void DeleteClip( int _index )
		{
			if( _index >= 0 && _index < m_Clips.Count )
				m_Clips.RemoveAt( _index );
		}

		public AudioClip GetNextClip()
		{
			if( m_Clips.Count == 0 )
				return null;

			int _counter = 0;
			reroll:	
			AudioClip _clip = m_Clips[Random.Range(0,m_Clips.Count)];
			
			if( _clip != null )
			{
				_counter++;
				if( m_Clips.Count > 1 && _clip == m_Selected && _counter < 5 )
					goto reroll;

				m_Selected = _clip;
			}

			return m_Selected;
		}
	}
	
	[System.Serializable]
	public class AudioPlayerObject : ICEImpulsTimerObject
	{
		public AudioPlayerObject(){}
		public AudioPlayerObject( ICEWorldBehaviour _component  ) : base( _component )
		{ Init( _component ); }
			
		private AudioSource m_AudioSource = null;
		public AudioSource Source{
			get{ return m_AudioSource = ( m_AudioSource == null && Owner != null && Application.isPlaying ? Owner.AddComponent<AudioSource>() : m_AudioSource );}
		}

		public AudioClip CurrentClip{
			get{ return ( Source != null ? Source.clip:null ); }
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			
			if( Source != null )
			{
				Source.rolloffMode = AudioRolloffMode.Logarithmic;
				Source.volume = 0.5f;
				Source.pitch = 1f;
				Source.minDistance = 10f;
				Source.maxDistance = 100f;
				Source.spatialBlend = 1f;
			}
		}

		private AudioObject m_AudioData = null;

		private void Prepare( AudioObject _data )
		{
			if( Source == null || ! _data.Enabled || ! _data.Selected  )
				return;

			Source.clip = _data.Selected;				
			Source.volume = _data.Volume;
			Source.pitch = Random.Range( _data.MinPitch, _data.MaxPitch) * Time.timeScale;
			Source.rolloffMode = _data.RolloffMode;	
			Source.minDistance = _data.MinDistance;
			Source.maxDistance = _data.MaxDistance;
			Source.spatialBlend = 1.0f;		
			Source.loop = _data.Loop;

			if( _data.MixerGroup != null )
				Source.outputAudioMixerGroup = _data.MixerGroup;
		}

		public void PlayLoop( AudioObject _data )
		{
			if( Source == null )
				return;

			if( _data != null && _data.Enabled && _data.GetNextClip() != null )
			{
				if( Source.isPlaying )
				{
					if( _data.Break )
						Source.Stop();
					else
						return;
				}

				Prepare( _data );
				Source.Play();
			}
			else if ( m_AudioData == null || m_AudioData.StopAtEnd )
				Stop();
		}

		public void PlayOneShot( AudioObject _data )
		{
			if( Source == null )
				return;

			if( _data != null && _data.Enabled && _data.GetNextClip() != null )
			{
				if( Source.isPlaying )
				{
					if( _data.Break )
						Source.Stop();
					else
						return;
				}

				Prepare( _data );				
				Source.PlayOneShot( _data.Selected );
			}
			else if ( m_AudioData == null || m_AudioData.StopAtEnd )
				Stop();
		}

		public void Play( AudioObject _data )
		{
			if( Source == null )
				return;

			if( _data != null && _data.Enabled && _data.GetNextClip() != null )
			{
				if( m_AudioData != _data || Source.clip == null || Source.clip.name != _data.Selected.name )
				{
					m_AudioData = _data;
					SetImpulseData( m_AudioData );
					base.Start();

					if( DebugLogIsEnabled ) PrintDebugLog( this, "Play - " + m_AudioData.Selected.name );
				}
			}
			else if ( m_AudioData == null || m_AudioData.StopAtEnd )
				Stop();
		}

		/// <summary>
		/// Stop the audio playback and the impulse timer
		/// </summary>
		public override void Stop()
		{
			base.Stop();

			m_AudioData = null;
			if( Source != null )
			{
				Source.Stop();
				Source.clip = null;
			}
		}

		/// <summary>
		/// Runs the action method according to the given impulse settings
		/// </summary>
		protected override void Action()
		{
			if( m_AudioData == null || ! m_AudioData.Enabled || m_AudioData.Selected == null )
				return;
			
			if( m_AudioData.Loop )
				PlayLoop( m_AudioData );
			else
				PlayOneShot( m_AudioData );
		}
	}
		
	[System.Serializable]
	public class DirectAudioDataObject : ICEOwnerObject
	{
		public DirectAudioDataObject(){}
		public DirectAudioDataObject( ICEWorldBehaviour _component  ) : base( _component ) { Init( _component ); }
		public DirectAudioDataObject( ICEWorldBehaviour _component, DirectAudioDataObject _audio ) : base( _component ) { Copy( _audio ); }
		public DirectAudioDataObject( DirectAudioDataObject _audio ) { Copy( _audio ); }

		[SerializeField]
		private List<AudioClip> m_Clips = null; 

		[XmlIgnore]
		public List<AudioClip> Clips{
			get{ return m_Clips = ( m_Clips == null ? new List<AudioClip>() : m_Clips );}
		}

		public float Volume = 0.5f;	
		public float MinPitch = 1.0f;	
		public float MaxPitch = 1.5f;
		public float PitchMaximum = 10;
		public float MinDistance = 2;	
		public float MaxDistance = 7;	
		public float DistanceMaximum = 7;	
		public bool Loop = false;
		public bool Break = false;
		public AudioRolloffMode RolloffMode = AudioRolloffMode.Logarithmic;

		public AudioMixerGroup MixerGroup = null;

		public void Copy( DirectAudioDataObject _audio )
		{
			if( _audio == null )
				return;

			Enabled = _audio.Enabled;
			Foldout = _audio.Foldout;
			Loop = _audio.Loop;
			Break = _audio.Break;
			MaxDistance = _audio.MaxDistance;
			MinDistance =_audio.MinDistance;
			DistanceMaximum = _audio.DistanceMaximum;
			MaxPitch = _audio.MaxPitch;
			MinPitch = _audio.MinPitch;
			PitchMaximum = _audio.PitchMaximum;
			RolloffMode = _audio.RolloffMode;
			Volume = _audio.Volume;
			MixerGroup = _audio.MixerGroup;

			Clips.Clear();
			foreach( AudioClip _clip in _audio.Clips )
				Clips.Add( _clip );
		}
	}

	[System.Serializable]
	public class DirectAudioObject : DirectAudioDataObject
	{
		public DirectAudioObject(){}
		public DirectAudioObject( ICEWorldBehaviour _component  ) : base( _component ) { Init( _component ); }
		public DirectAudioObject( ICEWorldBehaviour _component, DirectAudioDataObject _audio ) : base( _component, _audio ) {}
		public DirectAudioObject( DirectAudioDataObject _audio ) : base( _audio )  {}


		private AudioClip m_Selected = null;
		[XmlIgnore]
		public AudioClip Selected{
			get{return m_Selected = ( m_Selected == null ? GetNextClip() : m_Selected ); }
		}

		public void AddClip( AudioClip _clip )
		{
			if( _clip != null )
				Clips.Add( _clip );
		}

		public void AddClip()
		{
			Clips.Add(AudioClip.Create("new AudioClip", 1, 1, 1, false));
		}

		public void DeleteClip( int _index )
		{
			if( _index >= 0 && _index < Clips.Count )
				Clips.RemoveAt( _index );
		}

		public AudioClip GetNextClip()
		{
			if( Clips.Count == 0 )
				return null;

			reroll:	
			AudioClip _clip = Clips[Random.Range(0,Clips.Count)];

			if( _clip != null )
			{
				if ( Clips.Count > 1 && _clip == m_Selected )
					goto reroll;

				m_Selected = _clip;
			}

			return m_Selected;
		}
	}

	[System.Serializable]
	public class DirectAudioPlayerObject : DirectAudioObject
	{
		public DirectAudioPlayerObject(){}
		public DirectAudioPlayerObject( ICEWorldBehaviour _component  ) : base( _component ) { Init( _component ); }
		public DirectAudioPlayerObject( ICEWorldBehaviour _component, DirectAudioDataObject _audio ) : base( _component, _audio ) { Init( _component ); }
		public DirectAudioPlayerObject( DirectAudioDataObject _audio ) : base( _audio )  {}

		private AudioSource m_AudioSource = null;
		public AudioSource Source{
			get{ return m_AudioSource = ( m_AudioSource == null && Owner != null && Application.isPlaying ? Owner.AddComponent<AudioSource>() : m_AudioSource );}
		}



		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			Prepare();
		}

		public AudioClip CurrentClip{
			get{ return ( Source != null ? Source.clip:null ); }
		}

		public void Play( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			Play();
		}

		public void Play()
		{
			if( Owner == null || ! Owner.activeInHierarchy || Source == null || ! Source.enabled )
				return;

			if( Enabled && GetNextClip() != null )
			{
				if( Source.isPlaying )
				{
					// if the source is playing a single shot or the selected sound is already running and break is false then we will return, 
					// otherwise we will stop the current sound
					if( ( ! Loop || ( Source.clip != null && Source.clip.name == Selected.name ) ) && ! Break )
						return;

					Stop();
				}

				if( Prepare() )	
				{
					if( Loop )
						Source.Play();
					else
						Source.PlayOneShot( Selected );

					if( DebugLogIsEnabled ) PrintDebugLog( this, "Play - " + Selected.name );
				}
			}
			else
				Stop();
		}

		public void Stop()
		{
			if( Source == null )
				return;

			Source.Stop();
			Source.clip = null;
		}

		private bool Prepare()
		{
			if( Owner == null || ! Owner.activeInHierarchy || Source == null || ! Source.enabled )
				return false;

			if( Selected != null )
			{
				Source.playOnAwake = false;
				Source.clip = Selected;				
				Source.volume = Volume;
				Source.pitch = Random.Range( MinPitch, MaxPitch) * Time.timeScale;
				Source.rolloffMode = RolloffMode;	
				Source.minDistance = MinDistance;
				Source.maxDistance = MaxDistance;
				Source.spatialBlend = 1.0f;		
				Source.loop = Loop;

				if( MixerGroup != null )
					Source.outputAudioMixerGroup = MixerGroup;
		
				return true;
			}
			else
			{
				Source.clip = null;
				Source.rolloffMode = AudioRolloffMode.Logarithmic;
				Source.volume = 0.5f;
				Source.pitch = 1f;
				Source.minDistance = 10f;
				Source.maxDistance = 100f;
				Source.spatialBlend = 1f;
				Source.loop = false;
				return false;
			}
		}
	}


}
