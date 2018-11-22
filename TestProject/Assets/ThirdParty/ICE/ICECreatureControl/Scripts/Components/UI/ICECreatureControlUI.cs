// ##############################################################################
//
// ICECreatureControlUI.cs
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
using UnityEngine.UI;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.Utilities;

namespace ICE.Creatures.UI
{
	public class ICECreatureControlUI : ICEWorldEntityStatusUI 
	{
		private ICECreatureControl m_CreatureControl = null;
		private ICECreatureControl CreatureControl{
			get{ return m_CreatureControl = ( m_CreatureControl == null ? GetComponentInParent<ICECreatureControl>() : m_CreatureControl ); }
		}
			
		public Canvas StatusCanvas = null;
		public Slider HealthSlider = null;
		public Text DamageText = null;

		public bool UseDamagePopup = true;
		public float DamagePopupTime = 5;
		public float DamagePopupTimeMaximum = 20;

		public float DamagePopupSpeedMin = 0.5f;
		public float DamagePopupSpeedMax = 0.5f;
		public float DamagePopupSpeedMaximum = 1;

		public override void Start () 
		{
			CreatureControl.OnAddDamage += DoAddDamage;

			if( DamageText != null )
			{
				DamageText.gameObject.SetActive( false );
				DamageText.enabled = true;
			}
		}

		public override void OnDisable()
		{
			StopAllCoroutines();

			Text[] _text_array = StatusCanvas.gameObject.GetComponentsInChildren<Text>();

			for( int i = 0 ; i < _text_array.Length ; i++ )
			{
				if( _text_array[i] != null && _text_array[i].name.Contains( "(Clone)" ) )
				{
					Destroy( _text_array[i] );
					--i;
				}
			}
		}

		public override void Update() 
		{			
			if( Camera.main != null ) 
			{
				transform.LookAt( Camera.main.transform.position, Vector3.up );
				transform.Rotate( 0,180,0 );
			}
		}

		/// <summary>
		/// DoAddDamage event fires whenever the creature reveives damages
		/// </summary>
		/// <param name="_damage">Damage.</param>
		/// <param name="_damage_direction">Damage direction.</param>
		/// <param name="_damage_position">Damage position.</param>
		/// <param name="_attacker">Attacker.</param>
		/// <param name="_force">Force.</param>
		private void DoAddDamage( float _damage, Vector3 _damage_direction, Vector3 _damage_position, Transform _attacker, float _force = 0 )
		{
			if( HealthSlider == null )
				return;

			HealthSlider.minValue = 0;
			HealthSlider.maxValue = 100;

			HealthSlider.value = CreatureControl.Status.DurabilityInPercent;

			if( UseDamagePopup && DamageText != null && StatusCanvas != null )
				StartCoroutine( DamageTextMover( _damage.ToString(), DamagePopupTime ) );
		}

		private IEnumerator DamageTextMover( string _value, float _runtime )
		{
			//float _start = Time.time;   
			float _speed = UnityEngine.Random.Range( DamagePopupSpeedMin, DamagePopupSpeedMax ); 

			GameObject _obj =  Instantiate( DamageText.gameObject ) as GameObject;  
			_obj.transform.SetParent( StatusCanvas.transform, false );
			_obj.SetActive (true);  
			Text _text = _obj.GetComponent<Text>();
			_text.text = _value;

			Destroy( _obj, _runtime );

			//Debug.Log( "start" );

			while( _obj != null )// _start + _runtime > Time.time )
			{           
				_text.transform.Translate( Vector3.up * _speed * Time.deltaTime );

				_text.transform.localScale = Vector3.Lerp( _text.transform.localScale, Vector3.zero, _speed * Time.deltaTime );  

				_text.color = new Color( _text.color.r, _text.color.g, _text.color.b, _text.color.a - (0.5f * Time.deltaTime) ); 
				yield return null;
			}
				
			yield return null;
		}
	}
}

