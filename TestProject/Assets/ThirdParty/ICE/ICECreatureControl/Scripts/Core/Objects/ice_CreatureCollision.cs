// ##############################################################################
//
// ice_CreatureCollision.cs
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
	public class CollisionDataObject : ICEDataObject
	{
		public CollisionDataObject(){
			Foldout = true;
			Enabled = true;
		}
		public CollisionDataObject( CollisionDataObject _object ) : base( _object ){ Copy( _object ); }

		public void Copy( CollisionDataObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			UseTag = _object.UseTag;
			UseLayer = _object.UseLayer;
			UseBodyPart = _object.UseBodyPart;
			Name = _object.Name;
			ForceInteraction = _object.ForceInteraction;
			Tag = _object.Tag;
			Layer = _object.Layer;
			BodyPart = _object.BodyPart;
			UseBehaviourModeKey = _object.UseBehaviourModeKey;
			BehaviourModeKey = _object.BehaviourModeKey;
			Influences = _object.Influences;
		}

		public bool UseTag = true;
		public bool UseLayer = true;
		public bool UseBodyPart = true;

		public string Name = "";
		public bool ForceInteraction = false;
		public string Tag = "";
		public int Layer = 0;
		public string BodyPart = "";
		public bool UseBehaviourModeKey = false;
		public string BehaviourModeKey = "";

	
		[SerializeField]
		public InfluenceObject m_Influences = null;
		public InfluenceObject Influences{
			get{ return m_Influences = ( m_Influences == null ? new InfluenceObject():m_Influences ); }
			set{ Influences.Copy( value ); }
		}

	}

	[System.Serializable]
	public class CollisionObject : ICEOwnerObject
	{
		public CollisionObject(){}
		public CollisionObject( ICEWorldBehaviour _component ) : base( _component ){}
		public CollisionObject( CollisionObject _object ) : base( _object ){ Copy( _object ); }

		public bool UseCollider = true;
		public bool UseTrigger = false;
		public bool UseCharacterController = false;

		public bool AllowChildCollisions = false;

		public override void Reset()
		{
			Collisions.Clear();	
			Enabled = false;
			Foldout = false;
		}

		public void Copy( CollisionObject _object)
		{
			if( _object == null )
				return;

			base.Copy( _object );


			UseCollider = _object.UseCollider;
			UseTrigger = _object.UseTrigger;
			UseCharacterController = _object.UseCharacterController;
			AllowChildCollisions = _object.AllowChildCollisions;

			Collisions.Clear();
			foreach( CollisionDataObject _data in _object.Collisions )
				Collisions.Add( new CollisionDataObject( _data ) );
		}

		private TargetObject m_Target = null;
		public virtual TargetObject Target{
			get{ return m_Target = ( m_Target == null ? new TargetObject( TargetType.UNDEFINED ):m_Target );  }
		}

		public List<CollisionDataObject> Collisions = new List<CollisionDataObject>();
	

		/*
		public CollisionDataObject CheckExternal( GameObject _object )
		{
			if( _object == null )
				return null;

			CollisionDataObject _current_collision = null;
			
			foreach( CollisionDataObject _collision in Collisions )
			{
				if( _collision.Enabled && _collision.Type == ImpactType.EXTERN )
					_current_collision = _collision;
			}
			
			if( _current_collision != null && _current_collision.ForceInteraction )
			{
				Target.TargetGameObject = _object;
				Target.TargetOffset.z = 2;
				Target.TargetStopDistance = 2;
				Target.TargetRandomRange = 0;
				Target.BehaviourModeKey = _current_collision.BehaviourModeKey;
			}
			
			return _current_collision;
		}*/

		public List<CollisionDataObject> CheckCollider( Collider _collider, string _contact_name )
		{
			if( _collider == null || ( ! AllowChildCollisions && _collider.transform.IsChildOf( Owner.transform ) ) )
				return new List<CollisionDataObject>();

			string _tag = _collider.tag;
			int _layer = _collider.gameObject.layer;
	
			List<CollisionDataObject> _collisions = new List<CollisionDataObject>();

			foreach( CollisionDataObject _impact in Collisions )
			{
				if( _impact.Enabled )
				{
					bool _ready = true;

					if( _impact.UseTag && _impact.Tag != _tag )
						_ready = false;

					if( _impact.UseLayer && _impact.Layer != _layer )
						_ready = false;

					if( _impact.UseBodyPart && _impact.BodyPart != _contact_name )
						_ready = false;
					
					if( _ready )
						_collisions.Add( _impact );					 
				}
			}

			/*
			if( _current_impact != null && _current_impact.ForceInteraction )
			{
				Target.OverrideTargetGameObject( _collider.gameObject );
				Target.Move.Offset.z = 2;
				Target.Move.StoppingDistance = 2;
				Target.Move.RandomRange = 0;
				Target.BehaviourModeKey = _current_impact.BehaviourModeKey;
			}*/

			return _collisions;
		}

		public bool HasTarget()
		{
			if( m_Target != null && m_Target.TargetGameObject != null ) 
				return true;
			else
				return false;
		}


	}
}