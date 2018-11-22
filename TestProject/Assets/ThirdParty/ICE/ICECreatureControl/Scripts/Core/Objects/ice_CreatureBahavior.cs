// ##############################################################################
//
// ice_CreatureBehavior.cs
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
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{

	/// <summary>
	/// Behaviour mode rule link object.
	/// </summary>
	[System.Serializable]
	public class BehaviourModeRuleLinkObject : ICEOwnerObject
	{
		public BehaviourModeRuleLinkObject(){}
		public BehaviourModeRuleLinkObject( LinkType _type, string _key, int _index )
		{
			Type = _type;
			BehaviourModeKey = _key;
			RuleIndex = _index;
		}
		public BehaviourModeRuleLinkObject( ICEWorldBehaviour _component ) : base( _component ){}
		public BehaviourModeRuleLinkObject( BehaviourModeRuleLinkObject _link ) : base( _link as ICEOwnerObject ){ Copy( _link ); }

		public void Copy( BehaviourModeRuleLinkObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Type = _object.Type;
			BehaviourModeKey = _object.BehaviourModeKey;
			RuleIndex = _object.RuleIndex;

		}

		public LinkType Type;
		public string BehaviourModeKey;
		public int RuleIndex;
	}
		

	[System.Serializable]
	public class BehaviourModeFavouredObject : ICEDataObject
	{
		public BehaviourModeFavouredObject(){}
		public BehaviourModeFavouredObject( BehaviourModeFavouredObject _object ) : base( _object )
		{ Copy( _object ); }

		public void Copy( BehaviourModeFavouredObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			MinRuntime = _object.MinRuntime;
			MaxRuntime = _object.MaxRuntime;
			MaxRuntimeMaximum = _object.MaxRuntimeMaximum;
			FavouredPriority = _object.FavouredPriority;
			FavouredUntilNextMovePositionReached = _object.FavouredUntilNextMovePositionReached;
			FavouredUntilTargetMovePositionReached = _object.FavouredUntilTargetMovePositionReached;
			FavouredUntilDetourPositionReached = _object.FavouredUntilDetourPositionReached;
			FavouredTarget = _object.FavouredTarget;
			FavouredTargetName = _object.FavouredTargetName;
			FavouredTargetRange = _object.FavouredTargetRange;

		}

		private float m_Runtime; 
		public float Runtime{
			get{ return m_Runtime;}
		}
		public float MinRuntime = 0;
		public float MaxRuntime = 0;
		public float MaxRuntimeMaximum = 60;

		public float FavouredPriority = 0; 

		public bool  FavouredUntilNextMovePositionReached = false; 
		public bool FavouredUntilTargetMovePositionReached = false; 
		public bool FavouredUntilDetourPositionReached = false; 
		public bool FavouredTarget = false;
		public string FavouredTargetName = "";
		public float FavouredTargetRange = 0;

		public float InitRuntime(){
			return m_Runtime = ( Enabled == true?UnityEngine.Random.Range( MinRuntime, MaxRuntime ):0 );
		}

		public bool FavouredUntilNewTargetInRange( TargetObject _target, float _distance )
		{
			if( _target == null || _target.TargetGameObject == null || FavouredTarget == false || FavouredTargetRange <= 0 )
				return false;
			
			if( _distance <= FavouredTargetRange && ( string.IsNullOrEmpty( FavouredTargetName ) || _target.TargetGameObject.name == FavouredTargetName ) )
				return false;
			else
				return true;
		}
	}

	//--------------------------------------------------
	// Animations
	//--------------------------------------------------
	[System.Serializable]
	public class BehaviourModeObject : ICEOwnerObject 
	{
		public BehaviourModeObject(){}
		public BehaviourModeObject( string _key )
		{
			if( string.IsNullOrEmpty( _key ) )
				return;
			
			Key = _key;
			Rules.Add( new BehaviourModeRuleObject() );
		}
		public BehaviourModeObject( ICEWorldBehaviour _component ) : base( _component ){}
		public BehaviourModeObject( BehaviourModeObject _mode ) : base( _mode ){ Copy( _mode ); }

		public void Copy( BehaviourModeObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Favoured.Copy( _object.Favoured );
			Key = _object.Key;

			Rules.Clear();
			foreach( BehaviourModeRuleObject _rule in _object.Rules )
				Rules.Add( new BehaviourModeRuleObject( _rule ) );

		}
			
		public string Key = "";
		public SequenceOrderType RulesOrderType;
		public bool RulesOrderInverse;

		[SerializeField]
		public BehaviourModeFavouredObject m_Favoured = null;
		public BehaviourModeFavouredObject Favoured{
			get{ return m_Favoured = ( m_Favoured == null ? new BehaviourModeFavouredObject():m_Favoured ); }
			set{ Favoured.Copy( value ); }
		}

		private bool m_Active = false;
		public bool Active{
			get{return m_Active;}
		}

		/// <summary>
		/// Starts the behaviour mode.
		/// </summary>
		/// <param name="_component">Component.</param>
		public void Start( ICEWorldBehaviour _component )
		{
			Init( _component );
			Favoured.InitRuntime();
			m_Active = true;
		}

		/// <summary>
		/// Stops the behaviour mode include its rules.
		/// </summary>
		public void Stop()
		{
			if( Rule != null )
				Rule.Stop();

			if( LastRule != null )
				LastRule.Stop();

			m_Active = false;
		}


		/// <summary>
		/// Update the behaviour mode include its active rule.
		/// </summary>
		public void Update()
		{
			if( Rule != null )
				Rule.Update();
		}
			
		/// <summary>
		/// Gets a value indicating whether this instance is valid.
		/// </summary>
		/// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
		public bool IsValid{
			get{ return ( ValidRules.Count > 0 ? true : false ); }
		}

		public bool IsReady{
			get{ return ( Rule != null ? true : false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this instance has active detour rule.
		/// </summary>
		/// <value><c>true</c> if this instance has active detour rule; otherwise, <c>false</c>.</value>
		public bool HasActiveDetourRule{
			get{
				if( Rule.Move.Type == MoveType.DETOUR ) 
					return true;
				else
					return false;			
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance has detour rules.
		/// </summary>
		/// <value><c>true</c> if this instance has detour rules; otherwise, <c>false</c>.</value>
		public bool HasDetourRules{
			get{
				foreach( BehaviourModeRuleObject _rule in Rules )
				{
					if( _rule.Move.Type == MoveType.DETOUR ) 
						return true;
				}
	
				Favoured.FavouredUntilDetourPositionReached = false;

				return false;
			}
		}

		private bool m_RuleChanged = false;
		public bool RuleChanged{
			get{ return m_RuleChanged; }
		}
			


		private BehaviourModeRuleObject m_Rule = null;
		public BehaviourModeRuleObject Rule{
			get{ return m_Rule; }
		}

		private BehaviourModeRuleObject m_LastRule = null;
		public BehaviourModeRuleObject LastRule{
			get{ return m_LastRule; }
		}

		/// <summary>
		/// Gets the index of the active rule.
		/// </summary>
		/// <value>The index of the active rule.</value>
		public int ActiveRuleIndex{
			get{ return ( m_Rule != null ? m_Rule.Index : 0 ); }
		}

		/// <summary>
		/// Gets the index of the last rule.
		/// </summary>
		/// <value>The last index of the rule.</value>
		public int LastRuleIndex{
			get{ return ( m_LastRule != null ? m_LastRule.Index : 0 ); }
		}

		public float RuleLength()
		{
			if( m_Rule != null )
			{ 
				if( m_Rule.UseCustomLength && Mathf.Max( m_Rule.LengthMin, m_Rule.LengthMax ) > 0 )
					return UnityEngine.Random.Range ( Mathf.Min ( m_Rule.LengthMin, m_Rule.LengthMax ), Mathf.Max ( m_Rule.LengthMin, m_Rule.LengthMax ) );
				else 
					return m_Rule.Animation.GetAnimationLength();
			}
			else
				return 0.0f;
		}




		[SerializeField]
		private List<BehaviourModeRuleObject> m_Rules = new List<BehaviourModeRuleObject>();

		[XmlArray("Rules"),XmlArrayItem("BehaviourModeRuleObject")]
		public List<BehaviourModeRuleObject> Rules{
			set{ m_Rules = value; }
			get{ return m_Rules; }
		}

		//private List<BehaviourModeRuleObject> m_ValidRules = null;
		public List<BehaviourModeRuleObject> ValidRules{
			get{ 
				List<BehaviourModeRuleObject> _rules = new List<BehaviourModeRuleObject>();

				if( Rules.Count == 1 )
					Rules[0].Enabled = true;
						
				foreach( BehaviourModeRuleObject _rule in Rules )
				{
					if( _rule != null && _rule.Enabled == true )
						_rules.Add( _rule );	
				}

				return _rules; 			
			}
		}

		public bool NextRule()
		{
			if( ValidRules.Count == 0 )
			{
				if( DebugLogIsEnabled ) PrintDebugLog( this, "NextRule : The behaviour mode '" + Key + "' don't contains valid rules, please check this mode and its rules!");
				return false;
			}

			return SetRuleByIndex( GetNextRuleIndex() );
		}

		/// <summary>
		/// Gets the index of the next rule.
		/// </summary>
		/// <returns>The next rule index.</returns>
		private int GetNextRuleIndex()
		{
			List<BehaviourModeRuleObject> _rules = ValidRules;
			int _index = ActiveRuleIndex;

			if( _rules.Count == 1 )
			{
				_index = 0;
			}
			else if( Rule != null && Rule.Link.Enabled && Rule.Link.Type == LinkType.RULE && Rule.Link.RuleIndex < _rules.Count )
			{
				_index = Rule.Link.RuleIndex;
			}
			else if( RulesOrderType == SequenceOrderType.RANDOM )
			{
				_index = UnityEngine.Random.Range( 0, _rules.Count );
			}
			else if( RulesOrderType == SequenceOrderType.WEIGHTEDRANDOM )
			{
				float _weight = 0f;
				for( int i = 0; i < _rules.Count; i++ ){
					_weight += _rules[i].Weight;
				}

				float _random = UnityEngine.Random.Range( 0, _weight );
				for( int i = 0; i < _rules.Count; i++ )
				{
					if( _random < _rules[i].Weight )
					{
						_index = i;
						break;
					}
					_random -= _rules[i].Weight;
				}            
			}
			else 
			{
				if( RulesOrderType == SequenceOrderType.PINGPONG )
				{
					if( ! RulesOrderInverse && _index + 1 >= _rules.Count )
						RulesOrderInverse = true;
					else if( RulesOrderInverse && _index - 1 < 0 )
						RulesOrderInverse = false;
				}

				if( ! RulesOrderInverse )
				{
					_index++;

					if( _index >= _rules.Count )
						_index = 0;
				}
				else
				{
					_index--;

					if( _index < 0 )
						_index = _rules.Count - 1;
				}
			}

			return _index;
		}

		/// <summary>
		/// Sets a rule by the specified index.
		/// </summary>
		/// <returns><c>true</c>, if rule was set by index, <c>false</c> otherwise.</returns>
		/// <param name="_index">Index.</param>
		private bool SetRuleByIndex( int _index )
		{
			List<BehaviourModeRuleObject> _rules = ValidRules;

			if( _index < 0 || _index >= _rules.Count )
			{
				if( DebugLogIsEnabled ) PrintDebugLog( this, "SetRuleByIndex : Behaviour mode rule index '" + _index + "' out of range, please check this mode and its rules!");
				return false;
			}

			BehaviourModeRuleObject _new_rule = _rules[ _index ];

			if( m_Rule != _new_rule || m_Rule == null )
			{
				//if( m_Rule != null )
				//	m_Rule.Stop();
				
				m_LastRule = m_Rule;
				m_Rule = _new_rule;
				m_Rule.Index = _index;

				//if( m_Rule != null )
				//	m_Rule.Start( OwnerComponent );

				m_RuleChanged = true;
			}
			else
				m_RuleChanged = false;

			return m_RuleChanged;
		}


	}

	[System.Serializable]
	public class BehaviourModeRuleLengthObject : ICEDataObject
	{
	}

	//--------------------------------------------------
	// Animations
	//--------------------------------------------------
	[System.Serializable]
	public class BehaviourModeRuleObject : ICEOwnerObject
	{
		public BehaviourModeRuleObject(){}
		public BehaviourModeRuleObject( ICEWorldBehaviour _component ) : base( _component ) {}
		public BehaviourModeRuleObject( BehaviourModeRuleObject _rule ) : base( _rule as ICEOwnerObject ) { Copy( _rule ); }

		public void Copy( BehaviourModeRuleObject _rule )
		{
			if( _rule == null )
				return;

			base.Copy( _rule );

			Animation.Copy( _rule.Animation );
			Look.Copy( _rule.Look );
			Audio.Copy( _rule.Audio );
			Inventory.Copy( _rule.Inventory );
			Events.Copy( _rule.Events );
			Effect.Copy( _rule.Effect );
			Link.Copy( _rule.Link );
			Move.Copy( _rule.Move );
			Body.Copy( _rule.Body );
			Influences.Copy( _rule.Influences );

			Key = _rule.Key;
			LengthMax = _rule.LengthMax;
			LengthMin = _rule.LengthMin;
			LengthMaximum = _rule.LengthMaximum;
			Weight = _rule.Weight;
			UseCustomLength = _rule.UseCustomLength;
			FoldoutCustomLength = _rule.FoldoutCustomLength;

		}

		private float m_Runtime = 0;
		public float Runtime{
			get{ return m_Runtime; }
		}

		private bool m_Active = false;
		public bool Active{
			get{ return m_Active; }
		}

		public void SetActive( bool _value )
		{
			if( _value == true && m_Active == false && Enabled )
			{
				m_Active = true;

				Move.Altitude.Init();
				Effect.Start( OwnerComponent );
				Events.Start( OwnerComponent );
				Inventory.Start();
				Influences.Start();
			}
			else if( _value == false )
			{
				m_Active = false;

				m_Runtime = 0;

				Effect.Stop();
				Events.Stop();
				Inventory.Stop( OwnerComponent );
				Influences.Stop( OwnerComponent );
			}
		}

		public void Start( ICEWorldBehaviour _component )
		{
			Init( _component );
			SetActive( true );
		}

		public void Stop()
		{
			SetActive( false );
		}

		public void Update(){

			if( m_Active )
				m_Runtime += Time.deltaTime;
			else
				m_Runtime = 0;

			Effect.Update();
			Events.Update();
		}

		public string Key = "";

		public bool FoldoutCustomLength = true;
		public bool UseCustomLength = true;
		public float LengthMin = 0f;
		public float LengthMax = 0f;
		public float LengthMaximum = 20;
		public float Weight = 1f;
		public int Index = 0;

		[SerializeField]
		private BehaviourInfluenceObject m_Influences = null;
		public BehaviourInfluenceObject Influences{
			set{ m_Influences = value; }
			get{ return m_Influences = ( m_Influences == null ? new BehaviourInfluenceObject() : m_Influences ); }
		}

		[SerializeField]
		private AnimationDataObject m_Animation = null;
		public AnimationDataObject Animation{
			set{ m_Animation = value; }
			get{ return m_Animation = ( m_Animation == null ? new AnimationDataObject() : m_Animation ); }
		}

		[SerializeField]
		private BodyDataObject m_Body = null;
		public BodyDataObject Body{
			set{ m_Body = value; }
			get{ return m_Body = ( m_Body == null ? new BodyDataObject() : m_Body ); }
		}

		[SerializeField]
		private MoveDataObject m_Move = null;
		public MoveDataObject Move{
			set{ m_Move = value; }
			get{ return m_Move = ( m_Move == null ? new MoveDataObject() : m_Move ); }
		}

		[SerializeField]
		private BehaviourModeRuleLinkObject m_Link = null;
		public BehaviourModeRuleLinkObject Link{
			set{ m_Link = value; }
			get{ return m_Link = ( m_Link == null ? new BehaviourModeRuleLinkObject() : m_Link ); }
		}

		[SerializeField]
		private LookDataObject m_Look = null;
		public LookDataObject Look{
			set{ m_Look = value; }
			get{ return m_Look = ( m_Look == null ? new LookDataObject() : m_Look ); }
		}


		[SerializeField]
		private AudioObject m_Audio = null;
		public AudioObject Audio{
			set{ m_Audio = value; }
			get{ return m_Audio = ( m_Audio == null ? new AudioObject() : m_Audio ); }
		}

		[SerializeField]
		private BehaviourEventsObject m_Events = null;
		public BehaviourEventsObject Events{
			set{ m_Events = value; }
			get{ return m_Events = ( m_Events == null ? new BehaviourEventsObject() : m_Events ); }
		}

		[SerializeField]
		private EffectObject m_Effect = null;
		public EffectObject Effect{
			set{ m_Effect = value; }
			get{ return m_Effect = ( m_Effect == null ? new EffectObject() : m_Effect ); }
		}

		[SerializeField]
		private InventoryActionObject m_Inventory = null;
		public InventoryActionObject Inventory{
			set{ m_Inventory = value; }
			get{ return m_Inventory = ( m_Inventory == null ? new InventoryActionObject() : m_Inventory ); }
		}

		/// <summary>
		/// Gets a value indicating whether a move is required by this rule.
		/// </summary>
		/// <value><c>true</c> if a move is required; otherwise, <c>false</c>.</value>
		public bool MoveRequired{
			get{ return( Move.Enabled && ( Move.Motion.Velocity != Vector3.zero || Move.Motion.AngularVelocity != Vector3.zero ) ? true : false ); }
		}

		public bool UseRootMotion{
			get{ return Animation.UseRootMotion; }
	   	}

		/// <summary>
		/// Gets a value indicating whether this instance has mode link.
		/// </summary>
		/// <value><c>true</c> if this instance has mode link; otherwise, <c>false</c>.</value>
		public bool HasModeLink{
			get{
				if( Link.Enabled && 
					Link.Type == LinkType.MODE &&
					Link.BehaviourModeKey != "" ) 
					return true;
				else
					return false;
			}
		}

		/*
		private GameObject m_Effect = null;
		public void StartEffect( GameObject _owner ) 
		{
			if( _owner == null )
				return;

			if( Effect.ReferenceObject != null && Effect.Enabled == true )
			{
				Vector3 position = _owner.transform.position;

				if( Effect.OffsetType == RandomOffsetType.EXACT )
					position = PositionTools.GetOffsetPosition( _owner.transform, Effect.Offset );
				else if( Effect.OffsetRadius > 0 )
				{
					Vector2 pos = Random.insideUnitCircle * Effect.OffsetRadius;

					position.x += pos.x;
					position.z += pos.y;

					if( Effect.OffsetType == RandomOffsetType.HEMISPHERE )
						position.y += Random.Range(0,Effect.OffsetRadius ); 
					else if( Effect.OffsetType == RandomOffsetType.SPHERE )
						position.y += Random.Range( - Effect.OffsetRadius , Effect.OffsetRadius ); 
				}


				if( m_Effect == null || Effect.Detach )
				{
					m_Effect = (GameObject)Object.Instantiate( Effect.ReferenceObject, position, _owner.transform.rotation);

					if( ! Effect.Detach )
						m_Effect.transform.parent = _owner.transform;
				}

				if( m_Effect != null )
					m_Effect.SetActive( true );
			}
		}

		public void StopEffect() 
		{
			if( m_Effect != null && Effect.Detach == false )
				m_Effect.SetActive( false );
		}

		public void UpdateEffect( GameObject _owner )
		{
			Effect.UpdateEffect( _owner );
		}*/
	}


	//--------------------------------------------------
	// Animations
	//--------------------------------------------------
	[System.Serializable]
	[XmlRoot("BehaviourObject")]
	public class BehaviourObject  : ICEOwnerObject
	{
		public BehaviourObject(){}
		public BehaviourObject( ICEWorldBehaviour _component ) : base( _component ){ Init( _component );}
		public BehaviourObject( BehaviourObject _object ) : base( _object ){ Copy( _object );}

		public bool UseEditingMode = false;
		public string NewBehaviourModeKey = "";
		public string OldBehaviourModeKey = "";

		// Event Handler
		public delegate void OnBehaviourModeChangedEvent( GameObject _sender, BehaviourModeObject _new_mode, BehaviourModeObject _last_mode );
		public event OnBehaviourModeChangedEvent OnBehaviourModeChanged;

		public delegate void OnBehaviourModeRuleChangedEvent( GameObject _sender, BehaviourModeRuleObject _new_rule, BehaviourModeRuleObject _last_rule );
		public event OnBehaviourModeRuleChangedEvent OnBehaviourModeRuleChanged;

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
	
			BehaviourAnimation.Init( _component );

			for( int i = 0 ; i < BehaviourModes.Count ; i++ )
				m_BehaviourModesKeys.Add( BehaviourModes[i].Key, i );

		}

		public void Copy( BehaviourObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			BehaviourModes.Clear();
			foreach( BehaviourModeObject _mode in _object.BehaviourModes )
				BehaviourModes.Add( new BehaviourModeObject( _mode ) );
		}

		public override void Reset()
		{
			if( Application.isPlaying )
			{
				m_LastBehaviourMode = null;
				m_ActiveBehaviourMode = null;
				m_DesignatedBehaviourMode = null;
				m_DesignatedBehaviourModeKey = "";
			}
			else
			{
				BehaviourModes.Clear();
			}
		}

		public BehaviourModeObject CopyBehaviourMode( BehaviourModeObject _mode )
		{
			string _key = _mode.Key;

			_key = Regex.Replace( _key, "COPY_OF_", "" );

			_key = "COPY_OF_" + _key;

			if( BehaviourModeExists( _key ) )
			{
				int _index = 1;
				while( BehaviourModeExists( _key + "_" + _index ) )
				      _index++;

				_key += "_" + _index;
			}

			BehaviourModeObject _copy = new BehaviourModeObject( _mode );
			_copy.Key = _key;

			m_BehaviourModes.Add( _copy );

			return _copy;
		}

		public string GetFixedBehaviourModeKey( string _key )
		{
			return Regex.Replace( _key, "( )+", "").ToUpper(); 
		}

		public string AddBehaviourMode( string _key )
		{
			_key = GetFixedBehaviourModeKey( _key ); 

			if( SystemTools.IsValid( _key ) && ! BehaviourModeExists( _key ) )
				m_BehaviourModes.Add( new BehaviourModeObject( _key ) );
	
			return _key;
		}

		public string AddBehaviourModeNumbered( string _key )
		{
			_key = GetFixedBehaviourModeKey( _key ); 

			string _index_key = _key;
			int _index = 0;
			while( BehaviourModeExists( _index_key ) )
			{
				_index++;

				_index_key = _key + "_" + _index; 
			}

			_key = _index_key;

			if( SystemTools.IsValid( _key ) && ! BehaviourModeExists( _key ) )
				m_BehaviourModes.Add( new BehaviourModeObject( _key ) );

			return _key;
		}

		public BehaviourModeObject CreateBehaviourMode( string _key )
		{
			BehaviourModeObject _mode = null;
			_key = GetFixedBehaviourModeKey( _key ); 

			if( _key != "" && ! BehaviourModeExists( _key ) )
				_mode = new BehaviourModeObject( _key );

			return _mode;
		}

		public bool BehaviourModeExists( string _key )
		{
			foreach( BehaviourModeObject  _mode in m_BehaviourModes )
				if( _mode.Key.Equals( _key ) ) return true;
			
			return false;
		}

		//--------------------------------------------------
		// Animation
		//--------------------------------------------------
		private AnimationPlayerObject m_BehaviourAnimation = null;
		public AnimationPlayerObject BehaviourAnimation{
			get{ return m_BehaviourAnimation = ( m_BehaviourAnimation == null ? new AnimationPlayerObject( OwnerComponent ):m_BehaviourAnimation ); }
		}

		private AudioPlayerObject m_BehaviourAudio = null;
		public AudioPlayerObject BehaviourAudioPlayer{
			get{ return m_BehaviourAudio = ( m_BehaviourAudio == null ? new AudioPlayerObject( OwnerComponent ):m_BehaviourAudio ); }
		}

		private LookObject m_BehaviourLook = null;
		public LookObject BehaviourLook{
			get{
				if( m_BehaviourLook == null )
					m_BehaviourLook = new LookObject( Owner );

				return m_BehaviourLook;
			}
		}

		private float m_BehaviourTimer = 0.0f;

		//private int m_LastBehaviourModeIndex = 0;
		private int m_ActiveBehaviourModeIndex = 0;

		private BehaviourModeObject m_LastBehaviourMode = null;
		private BehaviourModeObject m_ActiveBehaviourMode = null;
		private BehaviourModeObject m_DesignatedBehaviourMode = null;
		private string m_DesignatedBehaviourModeKey = "";
		/*
		public int[] ActiveBehaviourModeData{
			get{
				int[] _data = new int[2];
				if( m_ActiveBehaviourMode != null )
				{
					_data[0] = m_ActiveBehaviourModeIndex;
					_data[1] = m_ActiveBehaviourMode.ActiveRuleIndex;
				}
		
				return _data;			
			}
			set{
				if( value != null && value.Length == 2 )
				{
					SetBehaviourModeByKey
				}
			}
		}*/

		/// <summary>
		/// Gets the active behaviour mode.
		/// </summary>
		/// <value>The active behaviour mode.</value>
		public BehaviourModeObject ActiveBehaviourMode{
			get{return m_ActiveBehaviourMode;}
		}

		/// <summary>
		/// Gets the last behaviour mode.
		/// </summary>
		/// <value>The last behaviour mode.</value>
		public BehaviourModeObject LastBehaviourMode{
			get{return m_LastBehaviourMode;}
		}

		/// <summary>
		/// Returns the key of the active behaviour mode.
		/// </summary>
		/// <value>The behaviour mode key.</value>
		public string ActiveBehaviourModeKey{
			get{ return ( ActiveBehaviourMode != null ? ActiveBehaviourMode.Key : m_DesignatedBehaviourModeKey ); }
		} 

		/// <summary>
		/// Returns the key of the last behaviour mode.
		/// </summary>
		/// <value>The last behaviour mode key.</value>
		public string LastBehaviourModeKey{
			get{ return ( LastBehaviourMode != null ? LastBehaviourMode.Key : "" ); }
		}

		/// <summary>
		/// Gets the active behaviour mode rule or null.
		/// </summary>
		/// <value>The active behaviour mode rule.</value>
		public BehaviourModeRuleObject ActiveBehaviourModeRule{
			get{ return ( ActiveBehaviourMode != null ?  ActiveBehaviourMode.Rule : null ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.BehaviourObject"/> active behaviour mode
		/// rule has mode link.
		/// </summary>
		/// <value><c>true</c> if active behaviour mode rule has mode link; otherwise, <c>false</c>.</value>
		private bool ActiveBehaviourModeRuleHasModeLink{
			get{ return ( ActiveBehaviourModeRule != null ? ActiveBehaviourModeRule.HasModeLink:false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.BehaviourObject"/> active behaviour is valid.
		/// </summary>
		/// <value><c>true</c> if active behaviour is valid; otherwise, <c>false</c>.</value>
		public bool ActiveBehaviourIsValid{
			get{ return ( ActiveBehaviourMode != null && ActiveBehaviourMode.IsValid ? true : false ); }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="ICE.Creatures.Objects.BehaviourObject"/> active behaviour is ready.
		/// </summary>
		/// <value><c>true</c> if active behaviour is ready; otherwise, <c>false</c>.</value>
		public bool ActiveBehaviourIsReady{
			get{ return ( ActiveBehaviourMode != null && ActiveBehaviourMode.IsReady ? true : false ); }
		}


		//private BehaviourModeRuleObject m_LastBehaviourModeRule = null;
		//private int m_LastBehaviourModeRuleIndex = 0;

		//private BehaviourModeRuleObject m_BehaviourModeRule = null;
		//private int m_BehaviourModeRuleIndex = 0;


		private float m_BehaviourModeRuleTimer = 0.0f;
		private float m_BehaviourModeRuleLength = 0.0f;

		private bool m_BehaviourModeChanged = false;
		private bool m_BehaviourModeRulesChanged = false;
		private bool m_BehaviourModeRuleChanged = false;

		private bool m_BehaviourModeKeyIsValid = true;
		private bool BehaviourModeKeyIsValid{
			get{return m_BehaviourModeKeyIsValid; }
		}

		[SerializeField]
		private List<BehaviourModeObject > m_BehaviourModes = new List<BehaviourModeObject >();
		private Dictionary<string, int> m_BehaviourModesKeys = new Dictionary<string, int>();

		//--------------------------------------------------
		// BehaviourModes
		//--------------------------------------------------

		[XmlArray("BehaviourModes"),XmlArrayItem("BehaviourModeObject")]
		public List<BehaviourModeObject> BehaviourModes{
			get{ return m_BehaviourModes = ( m_BehaviourModes == null ? new List<BehaviourModeObject >() : m_BehaviourModes ); }
			set{ 
				BehaviourModes.Clear();
				if( value == null ) return;	
				foreach( BehaviourModeObject _mode in value )
					BehaviourModes.Add( new BehaviourModeObject( _mode ) );
			}
		}


		//--------------------------------------------------
		public bool BehaviourModeRulesChanged{
			get{ return m_BehaviourModeRulesChanged; }
		}

		//--------------------------------------------------
		// BehaviourMode
		//--------------------------------------------------

		/// <summary>
		/// Gets the behaviour mode by key or null.
		/// </summary>
		/// <returns>The behaviour mode by key.</returns>
		/// <param name="key">Key.</param>
		public BehaviourModeObject GetBehaviourModeByKey( string key )
		{
			if( key == null || key.Trim() == "" )
				return null;

			foreach( BehaviourModeObject _mode in BehaviourModes ){
				if( _mode.Key == key )
					return _mode;
			}

			return null;
		}

		protected BehaviourModeObject SetDesignatedBehaviourModeByKey( string _key )
		{
			if( string.IsNullOrEmpty( _key ) )
				return null;

			m_DesignatedBehaviourModeKey = _key; 
			m_DesignatedBehaviourMode = null;

			if( m_BehaviourModesKeys.TryGetValue( m_DesignatedBehaviourModeKey, out m_ActiveBehaviourModeIndex ) )
				m_DesignatedBehaviourMode = BehaviourModes[m_ActiveBehaviourModeIndex];

			return m_DesignatedBehaviourMode;
		}


		/// <summary>
		/// Sets the behaviour mode by key.
		/// </summary>
		///<description>Use this function to change the behaviour of your creature.</description>
		/// <param name="key">Key.</param>
		public bool SetBehaviourModeByKey( string _key )
		{
			bool _behaviour_mode_changed = false;
			if( string.IsNullOrEmpty( _key ) )
			{
				if( m_BehaviourModeKeyIsValid )
					PrintDebugLog( this, "SetBehaviourModeByKey : Invalid behaviour mode key, please check the behaviour modes!");

				m_BehaviourModeKeyIsValid = false;
			}
			else
			{
				m_BehaviourModeKeyIsValid = true;

				if( ActiveBehaviourModeKey != _key || ! ActiveBehaviourIsReady )
				{
					if( SetDesignatedBehaviourModeByKey( _key ) == null )
						PrintDebugLog( this, "SetBehaviourModeByKey : Behaviour Mode '" + m_DesignatedBehaviourModeKey + "' not exists, please check this behaviour mode!");
					else if( m_DesignatedBehaviourMode == null || m_DesignatedBehaviourMode.ValidRules.Count == 0 )
						PrintDebugLog( this, "SetBehaviourModeByKey : Behaviour Mode '" + m_DesignatedBehaviourModeKey + "' have no valid rules, please check its behaviour modes rules!");
					else
					{
						// stops the current behaviour mode include its active rules
						if( m_ActiveBehaviourMode != null )
						{
							BehaviourAnimation.Stop( m_ActiveBehaviourMode.Rule.Animation );
							m_ActiveBehaviourMode.Stop();
						}

						//BehaviourAudioPlayer.Stop();
						
						m_LastBehaviourMode = m_ActiveBehaviourMode;	
						m_ActiveBehaviourMode = m_DesignatedBehaviourMode;
		
						// initialize the active behaviour mode
						m_ActiveBehaviourMode.Start( OwnerComponent );

						// initialize the active behaviour mode rule
						NextBehaviorModeRule( true );

						m_BehaviourTimer = 0;

						m_BehaviourModeRulesChanged = true;
						m_BehaviourModeChanged = true;
						_behaviour_mode_changed = true;

						if( OnBehaviourModeChanged != null )
							OnBehaviourModeChanged( Owner, m_ActiveBehaviourMode, m_LastBehaviourMode );
					}
				}
			}

			return _behaviour_mode_changed;
		}

		public void NextBehaviorModeRule( bool _update = false )
		{
			if( ! ActiveBehaviourIsValid )
				PrintDebugLog( this, "NextBehaviorModeRule : invalid Behaviour '" + ActiveBehaviourModeKey + "', please check this mode and its rules!");
			else
			{
				// NextRule will be false if there are no rules but also no new rule, the first 
				// case should never be true so we can use _forced to run a simulated rule change
				// to refresh the parameter and/or start animation and audio files, effects and events 
				// etc.
				if( m_ActiveBehaviourMode.NextRule() || _update )
				{
					if( m_ActiveBehaviourMode.LastRule != null )
					{
						BehaviourAnimation.Stop( m_ActiveBehaviourMode.Rule.Animation );
						m_ActiveBehaviourMode.LastRule.Stop();
					}
					
					if( m_ActiveBehaviourMode.Rule != null )
					{
						if( DebugLogIsEnabled ) PrintDebugLog( this, "NextBehaviorModeRule : Start rule #" + m_ActiveBehaviourMode.Rule.Index + " of Behaviour Mode " + ActiveBehaviourModeKey + "!" );

						m_ActiveBehaviourMode.Rule.Start( OwnerComponent );

						BehaviourAnimation.Play( m_ActiveBehaviourMode.Rule.Animation );
						BehaviourAudioPlayer.Play( m_ActiveBehaviourMode.Rule.Audio );
						BehaviourLook.Adapt( m_ActiveBehaviourMode.Rule.Look );
						//BehaviourMessage.Send( m_ActiveBehaviourMode.Rule.Message );
					}
					
					m_BehaviourModeRuleLength = m_ActiveBehaviourMode.RuleLength();
					m_BehaviourModeRuleTimer = 0;

					m_BehaviourModeRuleChanged = m_ActiveBehaviourMode.RuleChanged;
					if( OnBehaviourModeRuleChanged != null && m_BehaviourModeRuleChanged == true )
						OnBehaviourModeRuleChanged( Owner, m_ActiveBehaviourMode.Rule, m_ActiveBehaviourMode.LastRule );
				}
			}
				
		}


		//--------------------------------------------------
		public bool BehaviourModeChanged{
			get{ return m_BehaviourModeChanged; }
		}

		//--------------------------------------------------
		public bool BehaviourModeRuleChanged{
			get{ return m_BehaviourModeRuleChanged; }
		}

		//--------------------------------------------------
		// Timer
		//--------------------------------------------------
		public float BehaviourTimer{
			get{ return m_BehaviourTimer; }
		}

		/// <summary>
		/// Updates the behaviour (include the active mode and the active rule).
		/// </summary>
		public void EarlyUpdate( StatusObject _status )
		{
			m_BehaviourModeChanged = false;					
			m_BehaviourModeRulesChanged = false;
			m_BehaviourModeRuleChanged = false;

			m_BehaviourTimer += Time.deltaTime;
			m_BehaviourModeRuleTimer += Time.deltaTime;

			BehaviourAudioPlayer.Update();

			//if( ActiveBehaviourModeRule != null )
			//	ActiveBehaviourModeRule.Inventory.Update( _status.Inventory );

		}

		public bool LateUpdate( StatusObject _status )
		{
			if( _status == null || ActiveBehaviourMode == null || ActiveBehaviourModeRule == null )
				return false;

			m_ActiveBehaviourMode.Update();

			if( m_BehaviourModeRuleTimer >= m_BehaviourModeRuleLength )
			{
				if( m_ActiveBehaviourMode.Rule != null && m_ActiveBehaviourMode.Rule.HasModeLink )
					SetBehaviourModeByKey( ActiveBehaviourMode.Rule.Link.BehaviourModeKey );
				else if( ActiveBehaviourMode.ValidRules.Count > 1 )
					NextBehaviorModeRule();
			}

			BehaviourAnimation.Update( ActiveBehaviourModeRule.Animation );

			ActiveBehaviourModeRule.Inventory.Update( _status.Inventory );

			return true;
		}
	}
}
