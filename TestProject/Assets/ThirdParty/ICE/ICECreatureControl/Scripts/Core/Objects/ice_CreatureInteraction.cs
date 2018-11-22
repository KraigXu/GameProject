// ##############################################################################
//
// ice_CreatureInteraction.cs
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{

	[System.Serializable]
	public class InteractorRuleObject : TargetObject
	{
		public InteractorRuleObject() : base() { 
			Enabled = true;
			Foldout = true;
			Selectors.UseDefaultPriority = true;
		}
		public InteractorRuleObject( InteractorRuleObject _rule ) : base( _rule as TargetObject ) { Copy( _rule );  }

		public InteractorRuleObject( InteractorRuleObject _rule, bool _default ) : base( _rule ) 
		{ 
			Copy( _rule ); 

			Enabled = true;
			Foldout = true;
			Selectors.UseDefaultPriority = _default;
		}

		public void Copy( InteractorRuleObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			OverrideTargetMovePosition = _object.OverrideTargetMovePosition;
			OverrideInfluences = _object.OverrideInfluences;
		}
			
		public bool OverrideTargetMovePosition = false;
		public bool OverrideInfluences = false;

		public bool PrepareRuleTarget( ICEWorldBehaviour _component, TargetObject _target )
		{
			if( ! Enabled || ! OwnerIsReady( _component ) )
				return false;

			// By default an additional interactor rule have the same TargetGameObject as the initial rule but while 
			// Selectors.Preselection.Enabled is true the rule will select its own best target of the same type according 
			// to the specifier pre-selection criteria
			if( Selectors.Preselection.Enabled )
			{
				if( PrepareTargetGameObject( _component ) == null || ! IsValidAndReady )
					return false;
			}
			else
			{					
				if( _target == null || _target.IsValidAndReady == false )
					return false;
				
				OverrideTargetGameObject( _target.TargetGameObject );
			}

			if( OverrideInfluences == false )
				Influences.Copy( _target.Influences );

			//if( Selectors.UseDefaultPriority )
			//	Selectors.Priority = _target.Selectors.Priority;

			if( OverrideTargetMovePosition == false )
				Move.Copy( _target.Move );

			Behaviour.CurrentBehaviourModeKey = GetBestBehaviourModeKey( Owner );

			return true;
		}

		/*
		public string BehaviourModeKey;	

		public Vector3 Offset;
		public float StoppingDistance;
		public float RandomRange;
		public bool IgnoreLevelDifference;
		public bool UpdateOffsetOnActivateTarget;
		public bool UpdateOffsetOnMovePositionReached;
		public bool UpdateOffsetOnRandomizedTimer;
		public float UpdateOffsetUpdateTimeMax = 0;
		public float UpdateOffsetUpdateTimeMin = 0;

		public float SmoothingMultiplier;*/
/*
		[SerializeField]
		private TargetSelectorsObject m_Selectors = new TargetSelectorsObject( TargetType.INTERACTOR );
		public TargetSelectorsObject Selectors{
			set{ m_Selectors = value;}
			get{ return m_Selectors;}
		}
*/


/*
		public Vector3 GetOffsetPosition( Transform _transform )
		{
			if( _transform == null )
				return Vector3.zero;

			Vector3 _local_offset = TargetOffset;
			
			_local_offset.x = _local_offset.x/_transform.lossyScale.x;
			_local_offset.y = _local_offset.y/_transform.lossyScale.y;
			_local_offset.z = _local_offset.z/_transform.lossyScale.z;
			
			return _transform.TransformPoint( _local_offset );
		}*/
	}

	//--------------------------------------------------
	// InteractorObject
	//--------------------------------------------------
	[System.Serializable]
	public class InteractorObject : TargetObject
	{
		public InteractorObject() : base( TargetType.INTERACTOR ){}
		public InteractorObject( InteractorObject _interactor ) : base( _interactor ) { Copy( _interactor ); }

		public void Copy( InteractorObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Enabled = _object.Enabled;
			InteractorFoldout = _object.InteractorFoldout;
			ShowInteractorInfoText = _object.ShowInteractorInfoText;
			InteractorInfoText = _object.InteractorInfoText;

			Rules.Clear();
			foreach( InteractorRuleObject _rule in _object.Rules )
				Rules.Add( new InteractorRuleObject( _rule ) );
		}

		public bool InteractorFoldout = true;
		public bool ShowInteractorInfoText = false;
		public string InteractorInfoText = "";

		public int AveragePriority{
			get{
				int _priority = SelectionPriority;
	
				foreach( InteractorRuleObject _rule in m_Rules )
					_priority += _rule.SelectionPriority;

				_priority = _priority / (m_Rules.Count + 1);

				return _priority;
			}
		}



		[SerializeField]
		private List<InteractorRuleObject> m_Rules = new List<InteractorRuleObject>();
		public List<InteractorRuleObject> Rules
		{
			set{ m_Rules = value; }
			get{ return m_Rules; }
		}
	
		private InteractorRuleObject m_PreviousRule = null;
		public InteractorRuleObject PreviousRule{
			get{ return m_PreviousRule; }
		}

		private InteractorRuleObject m_ActiveRule = null;
		public InteractorRuleObject ActiveRule{
			get{ return m_ActiveRule; }
		}

		/*
		public InteractorRuleObject GetRuleByDistanceAndSituation( float _distance )
		{
		}*/

		public InteractorRuleObject GetRuleByIndexOffset( int _offset )
		{
			int _index = m_Rules.IndexOf( m_ActiveRule );
			int _new_index = _index + _offset;

			if( _new_index < 0 )
				return null;
			else if( _new_index >= m_Rules.Count )
				return null;
			else
				return m_Rules[_new_index];
		}
		/*
		/// <summary>
		/// Returns the nearest rule by distance.
		/// </summary>
		/// <returns>The rule by distance.</returns>
		/// <param name="_distance">_distance.</param>
		public InteractorRuleObject GetRuleByDistance( float _distance )
		{
			InteractorRuleObject _selected = null;

			foreach( InteractorRuleObject _rule in m_Rules )
			{
				if( _distance <= _rule.Selectors.SelectionRange && _rule.Enabled )
					_selected = _rule;
			}

			return _selected;
		}
		*/
		public List<InteractorRuleObject> GetRulesByDistance( float _distance )
		{
			List<InteractorRuleObject> _rules = new List<InteractorRuleObject>();			
			foreach( InteractorRuleObject _rule in m_Rules )
			{
				_rule.Selectors.ResetStatus();
				if( _rule.Enabled && ( _rule.Selectors.UseAdvanced || _distance <= _rule.Selectors.FixedSelectionRange ) )
					_rules.Add( _rule );

			}
			
			return _rules;
		}

		public bool IsValidAndEnabled
		{
			get{
				if( Enabled && IsValidAndReady )
					return true;
				else
					return false;
			}
		}

		public void LateUpdates( GameObject _owner, float _speed )
		{
			LateUpdate( _owner, _speed );
			foreach( InteractorRuleObject _rule in Rules )
				_rule.LateUpdate( _owner, _speed );
		}

		public void FixedUpdates()
		{
			FixedUpdate();

			foreach( InteractorRuleObject _rule in Rules )
				_rule.FixedUpdate();
		}



		private List<TargetObject> m_PreparedTargets = null;
		public List<TargetObject> PreparedTargets{
			get{
				if( m_PreparedTargets == null )
					m_PreparedTargets = new List<TargetObject>();

				return m_PreparedTargets; 
			}
		}

		public int PreparedTargetsCount{
			get{ 
				if( m_PreparedTargets != null )
					return m_PreparedTargets.Count;
				else
					return 0;
			}
		}
		
		public void PrepareTargets( ICEWorldBehaviour _component )
		{
			if( ! Enabled || ! OwnerIsReady( _component ) || PrepareTargetGameObject( _component ) == null || ! IsValidAndReady )
				return;

			PreparedTargets.Clear();

			Selectors.ResetStatus();

			Behaviour.CurrentBehaviourModeKey = GetBestBehaviourModeKey( Owner );

			PreparedTargets.Add( this as TargetObject );

			foreach( InteractorRuleObject _rule in m_Rules )
			{
				_rule.Selectors.ResetStatus();
				if( _rule.PrepareRuleTarget( OwnerComponent, this as TargetObject ) )
					PreparedTargets.Add( _rule as TargetObject );
			}
		}

		public bool AutoCreate( GameObject _object , StatusObject _status )
		{
			if( _object == null || _status == null )
				return false;

			Enabled = true;
			AccessType = TargetAccessType.NAME;
			OverrideTargetGameObject( _object );

			if( EntityCreature != null )
			{
				return CreateAutoCreatureRules( _status, EntityCreature.Creature.Status );
			}
			else if( EntityPlayer != null )
			{
				return CreateAutoPlayerRules( _status );
			}				
			else
				return false;
		}

		public bool CreateAutoCreatureRules( StatusObject _own_status, StatusObject _target_status )
		{
			if( Rules.Count > 0 || _own_status == null || _target_status == null  )
				return false;
			
			Selectors.Priority = (int)Random.Range( 50, 70 );
			Selectors.UseSelectionRange = true;
			Selectors.SelectionRange = (int)Random.Range( 30, 50 );
			Selectors.UseSelectionAngle = false;
			Selectors.SelectionAngle = 0;

			Behaviour.UseAdvancedBehaviour = false;
			Behaviour.UseSelectiveBehaviour = false;
			Behaviour.BehaviourModeKey = "SENSE";

			// TARGET IS PREDATOR
			if( _target_status.TrophicLevel == TrophicLevelType.CARNIVORE || _target_status.TrophicLevel == TrophicLevelType.OMNIVORES || 
				_own_status.TrophicLevel == TrophicLevelType.CARNIVORE || _own_status.TrophicLevel == TrophicLevelType.OMNIVORES )
			{
				Rules.Add( CreateAutoRule( "HUNT", Selectors.SelectionRange - 5, Selectors.Priority + 5 ) );
				Rules.Add( CreateAutoRule( "ATTACK", Move.StoppingDistance, Selectors.Priority + 10 ) );
			}
			else
			{
				Rules.Add( CreateAutoRule( "ESCAPE", Selectors.SelectionRange - 5, Selectors.Priority + 5 ) );
				Rules.Add( CreateAutoRule( "DEFEND", Move.StoppingDistance, Selectors.Priority + 10 ) );
			}

			return true;
		}

		public bool CreateAutoPlayerRules( StatusObject _own_status )
		{
			if( Rules.Count > 0 || _own_status == null  )
				return false;

			Selectors.Priority = (int)Random.Range( 50, 70 );
			Selectors.UseSelectionRange = true;
			Selectors.SelectionRange = (int)Random.Range( 30, 50 );
			Selectors.UseSelectionAngle = false;
			Selectors.SelectionAngle = 0;

			Behaviour.UseAdvancedBehaviour = false;
			Behaviour.UseSelectiveBehaviour = false;
			Behaviour.BehaviourModeKey = "SENSE";

			// TARGET IS PREDATOR
			if( _own_status.TrophicLevel == TrophicLevelType.CARNIVORE || _own_status.TrophicLevel == TrophicLevelType.OMNIVORES )
			{
				Rules.Add( CreateAutoRule( "HUNT", Selectors.SelectionRange - 5, Selectors.Priority + 5 ) );
				Rules.Add( CreateAutoRule( "ATTACK", Move.StoppingDistance, Selectors.Priority + 10 ) );
			}
			else
			{
				Rules.Add( CreateAutoRule( "ESCAPE", Selectors.SelectionRange - 5, Selectors.Priority + 5 ) );
				Rules.Add( CreateAutoRule( "DEFEND", Move.StoppingDistance, Selectors.Priority + 10 ) );
			}

			return true;
		}

		public InteractorRuleObject CreateAutoRule( string _key, float _range, int _priority  )
		{
			InteractorRuleObject _rule = new InteractorRuleObject();
			_rule.Enabled = true;
			_rule.AccessType = AccessType;
			//_rule.UseChildObjects = UseChildObjects;
			_rule.UseTargetAttributes = UseTargetAttributes;
			_rule.OverrideTargetGameObject( TargetGameObject );
			_rule.Selectors.UseDefaultPriority = false;
			_rule.Selectors.Priority = _priority;
			_rule.Selectors.UseSelectionRange = true;
			_rule.Selectors.SelectionRange = _range;
			_rule.Selectors.UseSelectionAngle= false;
			_rule.Selectors.SelectionAngle = 0;

			_rule.Behaviour.UseAdvancedBehaviour = false;
			_rule.Behaviour.UseSelectiveBehaviour = false;
			_rule.Behaviour.BehaviourModeKey = "SENSE";
			_rule.Behaviour.BehaviourModeKey = _key;

			return _rule;
		}
	}

	[System.Serializable]
	public class InteractionObject : ICEOwnerObject
	{
		public InteractionObject(){}
		public InteractionObject( ICEWorldBehaviour _component ) : base( _component ){}
		public InteractionObject( InteractionObject _object ) : base( _object ){ Copy( _object ); }

		public void Copy( InteractionObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			Interactors.Clear();
			foreach( InteractorObject _interactor in _object.Interactors )
				Interactors.Add( new InteractorObject( _interactor ) );
		}

		[SerializeField]
		private List<InteractorObject> m_Interactors = null;
		public List<InteractorObject> Interactors{
			get{ return m_Interactors = ( m_Interactors == null ? new List<InteractorObject>() : m_Interactors ); }
			set{ 			
				Interactors.Clear();
				if( value == null ) return;	
				foreach( InteractorObject _interactor in value )
					Interactors.Add( new InteractorObject( _interactor ) );}
		}

		public override void Reset()
		{
			m_Interactors.Clear();
		}
	
		public List<InteractorObject> GetValidInteractors()
		{
			List<InteractorObject> _interactors = new List<InteractorObject> ();
			foreach( InteractorObject _interactor in Interactors )
			{
				if( _interactor.IsValidAndEnabled )
					_interactors.Add( _interactor );
			}

			return _interactors;
		}

		public bool TargetsReady()
		{
			if( GetValidInteractors().Count > 0 )
				return true;
			else
				return false;
		}

		public bool CheckExternalTarget( GameObject _target )
		{
			if( _target == null )
				return false;

			return false;
		}

		public bool UseAutoInteractorDetection = false;
		public void AutoInteractorDetection()
		{
		}

		public InteractorObject AddInteractor( GameObject _object, StatusObject _status )
		{
			if( _object == null || _status == null )
				return null;
			
			InteractorObject _interactor = GetInteractor( _object );

			if( _interactor == null )
			{
				_interactor = new InteractorObject();
				if( _interactor.AutoCreate( _object, _status ) )
					Interactors.Add( _interactor );
			}

			return _interactor;
		}

		public InteractorObject GetInteractor( GameObject _object )
		{
			if( _object == null )
				return null;

			InteractorObject _interactor_obj = null;
			
			foreach( InteractorObject _interactor in Interactors )
			{
				if( _interactor.TargetGameObject == null ) 
					continue;					
				else if( ( _interactor.AccessType == TargetAccessType.NAME && _interactor.TargetName == _object.name ) ||
					( _interactor.AccessType == TargetAccessType.TAG && _interactor.TargetTag == _object.tag ) ||
					( _interactor.AccessType == TargetAccessType.OBJECT && _interactor.TargetGameObject.GetInstanceID() == _object.GetInstanceID() ) )
				{
					_interactor_obj = _interactor;
					break;
				}
			}

			return _interactor_obj;
		}

		public void Sense()
		{
			/*
			foreach( InteractorObject _interactor in Interactors )
			{
				if( _interactor.IsValidAndEnabled )
					_interactor.PrepareTargets( Owner );
			}*/
		}

		public void LateUpdate( GameObject _owner, float _speed  )
		{
			foreach( InteractorObject _interactor in Interactors )
			{
				if( _interactor.Enabled )
					_interactor.LateUpdates( _owner, _speed );
			}

		}

		public void FixedUpdate()
		{
			foreach( InteractorObject _interactor in Interactors )
			{
				if( _interactor.IsValidAndEnabled )
					_interactor.FixedUpdates();
			}
				
		}

	}

}

