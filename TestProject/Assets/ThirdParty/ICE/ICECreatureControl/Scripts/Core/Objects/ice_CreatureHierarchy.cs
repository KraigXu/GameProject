// ##############################################################################
//
// ice_CreatureHierarchy.cs | HierarchyGroupObject | HierarchyManagementObject
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
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Attributes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class HierarchyGroupObject : ICEDataObject
	{
		public HierarchyGroupObject() : base(){}
		public HierarchyGroupObject( EntityClassType _type ) : base(){ 
			EntityType = _type; 
		}

		public EntityClassType EntityType = EntityClassType.Undefined;
		public Transform GroupTransform = null;
		public string GroupName{
			get{ return ( GroupTransform != null ? GroupTransform.name : "" ); }
			set{ 
				if( string.IsNullOrEmpty( value ) )
					GroupTransform = null;
				else if( GroupTransform != null ) 
					GroupTransform.name = value; 			
			}
		}
		public Color GroupColor = Color.red;

		public void UpdateGroupName( string _suffix )
		{
			string _entity = EntityType.ToString();
			string _default_name = _entity+_suffix;

			if( GroupTransform != null && GroupTransform.name.Contains( _entity ) )
				GroupTransform.name = _default_name;
		}

		public override void Reset()
		{
			if( GroupTransform == null )
				return;


			GroupTransform.DetachChildren();
			GameObject.DestroyImmediate( GroupTransform.gameObject );

			if( GroupTransform == null )
				ICEDebug.Log( GroupName + " (" + EntityType.ToString() + ") removed!" );

			GroupName = "";
		}
	}

	[System.Serializable]
	public class HierarchyManagementObject : ICEOwnerObject
	{
		public HierarchyManagementObject() : base(){}
		public HierarchyManagementObject( ICEWorldBehaviour _component) : base( _component ){}

		public override void Init( ICEWorldBehaviour _component  )
		{
			base.Init( _component );
		}

		public string GroupSuffix = "(Group)";

		[SerializeField]
		private HierarchyGroupObject m_HierarchyRootGroup = null;
		public HierarchyGroupObject HierarchyRootGroup{
			get{ return m_HierarchyRootGroup = ( m_HierarchyRootGroup == null ? new HierarchyGroupObject():m_HierarchyRootGroup ); }
			set{ m_HierarchyRootGroup = value; }
		}
			
		[SerializeField]
		private List<HierarchyGroupObject> m_HierarchyGroups = null;
		public List<HierarchyGroupObject> HierarchyGroups{
			get{ return m_HierarchyGroups = ( m_HierarchyGroups == null ? new List<HierarchyGroupObject>():m_HierarchyGroups ); }
			set{ m_HierarchyGroups = value; }
		}

		/// <summary>
		/// Gets all available entity class types.
		/// </summary>
		/// <value>The entity class types.</value>
		public EntityClassType[] EntityClassTypes{
			get{ return System.Enum.GetValues( typeof(EntityClassType) ) as EntityClassType[]; }
		}

		/// <summary>
		/// Gets the hierarchy group by using the EntityClassType.
		/// </summary>
		/// <returns>The hierarchy group.</returns>
		/// <param name="_type">Type.</param>
		public HierarchyGroupObject GetHierarchyGroup( EntityClassType _type )
		{
			foreach( HierarchyGroupObject _group in HierarchyGroups )
			{
				if( _group.EntityType == _type )
					return _group;
			}

			return null;
		}

		public HierarchyGroupObject GetHierarchyGroup( EntityClassType _type, bool _forced )
		{
			HierarchyGroupObject _group = GetHierarchyGroup( _type );
			if( _group == null && _forced )
			{
				_group = new HierarchyGroupObject( _type );
				HierarchyGroups.Add( _group );
			}
			return _group;	
		}

		public HierarchyGroupObject AddHierarchyGroup( EntityClassType _type, bool _enabled )
		{		
			HierarchyGroupObject _group = null;
			if( GetHierarchyGroup( _type ) == null )
			{
				_group = new HierarchyGroupObject( _type );
				_group.Enabled = _enabled;
				HierarchyGroups.Add( _group );
			}
			return _group;	
		}

		/// <summary>
		/// Adds the new hierarchy group by using the EntityClassType.
		/// </summary>
		/// <returns>The hierarchy group.</returns>
		/// <param name="_type">Type.</param>
		public HierarchyGroupObject AddHierarchyGroup( EntityClassType _type )
		{		
			HierarchyGroupObject _group = null;
			if( GetHierarchyGroup( _type ) == null )
			{
				_group = new HierarchyGroupObject( _type );
				HierarchyGroups.Add( _group );
			}
			return _group;	
		}

		/// <summary>
		/// Gets the hierarchy root group transform.
		/// </summary>
		/// <returns>The hierarchy root group transform.</returns>
		public Transform GetHierarchyRootGroupTransform()
		{
			if( Enabled && HierarchyRootGroup.Enabled )
			{
				if( HierarchyRootGroup.GroupTransform == null )
					HierarchyRootGroup.GroupTransform = Owner.transform;

				return HierarchyRootGroup.GroupTransform;
			}
			else
				return null;
		}

		public HierarchyGroupObject UpdateHierarchyGroup( HierarchyGroupObject _group )
		{			
			if( ! Enabled || _group == null || ! _group.Enabled )
				return null;

			// this will be the name of the group object
			string _default_group_name = _group.EntityType.ToString()+GroupSuffix;

			// if the group have no transform we have to find a suitable object or we have 
			// to create a new one
			if( _group.GroupTransform == null )
			{
				GameObject _obj = GameObject.Find( _default_group_name );
				if( _obj == null )
				{
					_group.GroupTransform = new GameObject().transform;
					_group.GroupTransform.SetParent( GetHierarchyRootGroupTransform(), true );
					_group.GroupTransform.name = _default_group_name;
					_group.GroupTransform.position = Vector3.zero;
				}
				else
				{
					_group.GroupTransform = _obj.transform;
				}
			}

			// if the group have a transform we have to set the correct parent
			// and update the name if required
			if( _group.GroupTransform != null )
				_group.GroupTransform.SetParent( GetHierarchyRootGroupTransform(), true );

			return _group;
		}

		/// <summary>
		/// Gets the hierarchy group transform.
		/// </summary>
		/// <returns>The hierarchy group transform.</returns>
		/// <param name="_type">Type.</param>
		public Transform GetHierarchyGroupTransform( EntityClassType _type )
		{			
			if( ! Enabled )
				return null;
			
			HierarchyGroupObject _group = UpdateHierarchyGroup( GetHierarchyGroup( _type, true ) );
			if( _group != null && _group.Enabled && _group.GroupTransform != null )
				return _group.GroupTransform;
			else
				return GetHierarchyRootGroupTransform();
		}

		public void ResetHierarchyGroups()
		{
			foreach( HierarchyGroupObject _group in HierarchyGroups ) 
			{
				if( _group != null )
					_group.Reset();
			}

			HierarchyGroups.Clear();
		}

		public void RemoveHierarchyGroup( EntityClassType _type )
		{
			HierarchyGroupObject _group = GetHierarchyGroup( _type );

			if( _group != null && _group.GroupTransform != null )
				GameObject.DestroyImmediate( _group.GroupTransform.gameObject );
		}

		/// <summary>
		/// Updates the suffix.
		/// </summary>
		/// <param name="_suffix">Suffix.</param>
		public void UpdateSuffix( string _suffix )
		{
			if( _suffix == GroupSuffix )
				return;

			GroupSuffix = _suffix;

			foreach( EntityClassType _type in EntityClassTypes )
			{
				HierarchyGroupObject _group = UpdateHierarchyGroup( GetHierarchyGroup( _type, true ) );
				if( _group != null )
					_group.UpdateGroupName( GroupSuffix );
			}
		}

		public void UpdateHierarchyGroups( bool _modify_enabled, bool _enabled )
		{
			if( HierarchyRootGroup.GroupTransform == null )
			{
				HierarchyRootGroup.EntityType = EntityClassType.Undefined;
				HierarchyRootGroup.Enabled = _enabled;
				HierarchyRootGroup.GroupTransform = GetHierarchyRootGroupTransform();
			}
				
			// this loop will get or create a group for each EntityClassType, updates all
			// data and acording to the _modify_enabled flag enable or disable the groups
			foreach( EntityClassType _type in EntityClassTypes )// System.Enum.GetValues( typeof(EntityClassType) ) )
			{
				HierarchyGroupObject _group = UpdateHierarchyGroup( GetHierarchyGroup( _type, true ) );
				if( _group != null && _modify_enabled )
					_group.Enabled = _enabled; 
			}
		}

		public void ReorganizeSceneObjects()
		{
			ICECreatureEntity[] _entities = GameObject.FindObjectsOfType<ICECreatureEntity>();
			foreach( ICECreatureEntity _entity in _entities )
			{
				if( _entity != null && _entity.UseHierarchyManagement )
					_entity.transform.SetParent( GetHierarchyGroupTransform( _entity.EntityType ), true );
			}
		}
	}


}
