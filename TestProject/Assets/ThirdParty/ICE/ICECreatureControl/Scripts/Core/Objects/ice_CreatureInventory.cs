// ##############################################################################
//
// ice_CreatureInventory.cs
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
using ICE.World.Utilities;
using ICE.World.Objects;
using ICE.World.EnumTypes;

using ICE.Creatures;
using ICE.Creatures.Utilities;
using ICE.Creatures.EnumTypes;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class InventoryActionDataObject : ICETimerObject
	{
		public InventoryActionDataObject(){}
		public InventoryActionDataObject( InventoryActionDataObject _data ) : base( _data ) { Copy( _data ); }

		public void Copy( InventoryActionDataObject _data )
		{
			if( _data == null )
				return;

			base.Copy( _data );

			Type = _data.Type;
			ItemName = _data.ItemName;
			ParentName = _data.ParentName;

			DistributionOffset = _data.DistributionOffset;
			DistributionRotation = _data.DistributionRotation;
			DistributionOffsetType = _data.DistributionOffsetType;
			DistributionOffsetRadius = _data.DistributionOffsetRadius;
		}

		public InventoryActionType Type;
		public string ItemName;
		public string ParentName;

		private GameObject m_DistributionItem;

		public Vector3 DistributionOffset;
		public Quaternion DistributionRotation;
		public RandomOffsetType DistributionOffsetType;
		public float DistributionOffsetRadius;

		public bool DropItemRequired(){
			return ( Type == InventoryActionType.DropItem ? true : false );
		}

		public bool ParentUpdateRequired(){
			return ( Type == InventoryActionType.ChangeParent ? true : false );
		}

		public bool CollectActiveItemRequired(){
			return ( Type == InventoryActionType.CollectActiveItem ? true : false );
		}

		public void Stop(  InventoryObject _inventory ){

			if( base.Stop() && _inventory != null )
				_inventory.Action( this );
		}
			
		public void Update(  InventoryObject _inventory )
		{
			if( _inventory == null )
				return;
			
			if( base.Update() )
				_inventory.Action( this );
		}

		public Vector3 Offset
		{
			get{
				Vector3 _offset = Vector3.zero;

				if( DistributionOffsetType == RandomOffsetType.EXACT )
				{
					_offset = DistributionOffset;
				}
				else if( DistributionOffsetRadius > 0 )
				{
					Vector2 _pos = Random.insideUnitCircle * DistributionOffsetRadius;

					_offset.x = _pos.x;
					_offset.z = _pos.y;

					if( DistributionOffsetType == RandomOffsetType.HEMISPHERE )
						_offset.y = Random.Range(0, DistributionOffsetRadius ); 
					else if( DistributionOffsetType == RandomOffsetType.SPHERE )
						_offset.y = Random.Range( - DistributionOffsetRadius , DistributionOffsetRadius ); 
				}

				return _offset;
			}
		}
	}

	[System.Serializable]
	public class InventoryActionObject : ICEDataObject
	{
		public InventoryActionObject(){}
		public InventoryActionObject( InventoryActionObject _data ) : base( _data ) {
			Copy( _data );
		}

		public void Copy( InventoryActionObject _data )
		{
			Enabled = _data.Enabled;
			Foldout = _data.Foldout;

			m_ActionList = new List<InventoryActionDataObject>();
			foreach( InventoryActionDataObject _action in _data.ActionList )
				m_ActionList.Add( new InventoryActionDataObject( _action ) );
		}

		[SerializeField]
		private List<InventoryActionDataObject> m_ActionList = null;
		public List<InventoryActionDataObject> ActionList{
			get{ return m_ActionList = ( m_ActionList == null ? new List<InventoryActionDataObject>() : m_ActionList ); }
			set{ 
				ActionList.Clear();		
				if( value == null ) return;	
				foreach( InventoryActionDataObject _action in value )
					ActionList.Add( new InventoryActionDataObject( _action ) );			
			}

		}

		public void Start() 
		{
			if( ! Enabled )
				return;
			
			foreach( InventoryActionDataObject _action in ActionList )
				_action.Start();
		}

		public void Stop( ICEWorldBehaviour _component ) 
		{
			InventoryObject _inventory = null;
			ICECreatureControl _control = _component as ICECreatureControl;
			if( _control != null )
				_inventory = _control.Creature.Status.Inventory;

			foreach( InventoryActionDataObject _action in ActionList )
				_action.Stop( _inventory );
		}

		public void Update( InventoryObject _inventory )
		{
			if( ! Enabled || _inventory == null )
				return;

			foreach( InventoryActionDataObject _action in ActionList )
				_action.Update( _inventory );
		}
	}

	[System.Serializable]
	public class InventorySlotObject : ICEOwnerObject
	{
		public InventorySlotObject(){
			ItemName = "";
			Amount = 0;
		}
		public InventorySlotObject( ICEWorldBehaviour _component, string _name, int _amount ){
			base.Init( _component );
			m_ItemName = _name;	
			m_MaxAmount = _amount;
		}
		public InventorySlotObject( ICEWorldBehaviour _component ) : base( _component ){}
		public InventorySlotObject( ICEWorldBehaviour _component, InventorySlotObject _slot ) : base( _component ) { Copy( _slot ); }

		public void Init( GameObject _owner )
		{
			base.SetOwner( _owner );
			Reset();
		}

		public override void Reset()
		{
			if( Application.isPlaying )
			{
				if( UseRandomAmount )
					Amount = UnityEngine.Random.Range( 0, MaxAmount );
				else
					Amount = InitialAmount;
			}
		}

		public bool IsExclusive = false;
		public bool UseDetachOnDie = false;
		public float DropRange = 0;
		public bool IsTransferable = true;

		[SerializeField]
		private string m_SlotName = "";
	
		[SerializeField]
		private string m_MountPointName = "";
		public string MountPointName{
			set{
				m_MountPointName = value;
				m_SlotName = "";

				if( m_MountPointName.Trim() == "" )
					m_MountPointTransform = null;
				else if( Owner != null && ( m_MountPointTransform == null || m_MountPointTransform.name.Trim() != m_MountPointName ) )
					m_MountPointTransform = ICE.World.Utilities.SystemTools.FindChildByName( m_MountPointName, Owner.transform );

				UpdateItemObject();
			}
			get{ return ( string.IsNullOrEmpty( m_MountPointName ) ? m_SlotName : m_MountPointName ); }
		}


		private Transform m_MountPointTransform = null;
		[XmlIgnore]
		public Transform MountPointTransform{
			get{ 
				if( Owner == null || MountPointName.Trim() == "" )
					m_MountPointTransform = null;
				else if( m_MountPointTransform == null || m_MountPointTransform.name != MountPointName || ! Application.isPlaying )
					m_MountPointTransform = ICE.World.Utilities.SystemTools.FindChildByName( MountPointName, Owner.transform );

				return m_MountPointTransform;
			}
		}

		[SerializeField]
		private string m_ItemName = "";
		public string ItemName{
			set{
				m_ItemName = value;

				//if( ! IsExclusive && m_ItemName != "" && m_Amount == 0 )
				//	m_Amount = 1; 
				
				UpdateItemObject();
			}
			get{ return m_ItemName; }
		}
			
		[XmlIgnore]
		private GameObject m_ItemObject = null;
		[XmlIgnore]
		public GameObject ItemObject{
			set{ SetItemObject( value ); }
			get{ return UpdateItemObject(); }
		}

		public GameObject UpdateItemObject()
		{
			if( ! ItemNameIsValid || IsNotional || Amount == 0 || ItemUpdateRequired || ( MountPointTransform != null && m_ItemObject != null && m_ItemObject.transform.IsChildOf( MountPointTransform ) == false ) ) 
			{
				if( m_ItemObject != null )
					CreatureRegister.Remove( m_ItemObject );

				m_ItemObject = null;
			}

			if( ItemNameIsValid && ! IsNotional && Amount > 0 && m_ItemObject == null )
			{
				m_ItemObject = FindItemObject();
				if( m_ItemObject == null )
				{
					m_ItemObject = SpawnItemObject( MountPointTransform.position, MountPointTransform.rotation );
					AttachToSlot( m_ItemObject );
				}
				else
				{
					m_ItemObject.transform.position = MountPointTransform.position;
					m_ItemObject.transform.rotation = MountPointTransform.rotation;
					m_ItemObject.SetActive( true );
				}
			}

			return m_ItemObject;
		}

		public bool SetItemObject( GameObject _item )
		{
			if( _item != null && IsNotional == false && m_ItemObject == null && FreeCapacity >= 1 )
			{
				m_ItemObject = _item;

				if( ItemReferenceObject == null )
					SetItemReferenceObject( m_ItemObject );
				
				AttachToSlot( m_ItemObject );
				m_Amount++;

				return true;
			}

			return false;
		}
			
		/// <summary>
		/// Spawns a new item object, changes its name and sets it active.
		/// </summary>
		/// <returns>The item object.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public GameObject SpawnItemObject( Vector3 _position, Quaternion _rotation )
		{
			GameObject _item = CreatureRegister.Spawn( ItemReferenceObject, _position, _rotation );
			if( _item != null )
			{
				_item.name = ItemReferenceObject.name;
				_item.SetActive( true );
			}

			return _item;
		}

		/// <summary>
		/// Finds the item object within the transform hierarchy.
		/// </summary>
		/// <returns>The item object.</returns>
		public GameObject FindItemObject()
		{
			Transform _transform = ICE.World.Utilities.SystemTools.FindChildByName( m_ItemName, MountPointTransform );
			if( _transform != null )
				return _transform.gameObject;
			else
				return null;
		}

		public bool ItemUpdateRequired{
			get{ return ( m_ItemObject != null && m_ItemObject.name.Trim() != m_ItemName.Trim() ? true : false ); }
		}

		[XmlIgnore]
		public GameObject ItemReferenceObject{
			get{ return CreatureRegister.GetReferenceGameObjectByName( ItemName ); }
		}

		private void SetItemReferenceObject( GameObject _item ){
			CreatureRegister.AddReference( _item );
		}

		public bool ItemNameIsValid{
			get{ return CheckName( ItemName ); }
		}

		private bool CheckName( string _name ){
			return ( ! string.IsNullOrEmpty( _name ) && ! string.IsNullOrEmpty( _name.Trim() ) ? true : false ); 
		}

		public int InitialAmount = 0;

		[SerializeField]
		private int m_Amount = 0;
		public int Amount{
			set{ 
				m_Amount = Mathf.Clamp( ( ItemNameIsValid ? value : 0 ), 0, MaxAmount );

				if( ! IsExclusive && m_Amount == 0 )
					m_ItemName = "";
				
				UpdateItemObject();
			}
			get{ return ( ItemNameIsValid ? m_Amount : 0 ); }
		}

		public float AmountInPercent{
			get{ return ( MaxAmount > 0 ? ( 100f / MaxAmount ) * Amount : 0 ); }
		}

		[SerializeField]
		private int m_MaxAmount = 1;
		public int MaxAmount{
			set{ m_MaxAmount = Mathf.Max( 1, value ); }
			get{ return Mathf.Max( 1, m_MaxAmount ); }
		}
		public bool UseRandomAmount = false;
		public int FreeCapacity{
			get{ return Mathf.Max( 0, MaxAmount - Amount );	}
		}

		private bool m_IsChild = false;
		public bool IsChild{
			get{ return m_IsChild; }
		}

		public bool IsNotional{
			get{ return ( MountPointTransform == null ? true : false ); }
		}

		public bool IsEquipped{
			get{ return ( MountPointTransform != null && ItemObject != null && ItemObject.transform.IsChildOf( MountPointTransform ) ? true : false ); }
		}

		public bool IsEmpty{
			get{ return ( string.IsNullOrEmpty( ItemName ) ? true : false ); }
		}

		/// <summary>
		/// Attachs to slot.
		/// </summary>
		/// <returns><c>true</c>, if to slot was attached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public bool AttachToSlot( GameObject _object ){

			if( _object == null )
				return false;

			if( ! _object.transform.IsChildOf( MountPointTransform ) )
				return CreatureRegister.AttachToTransform( _object, MountPointTransform );
			else
				return true;
		}

		/// <summary>
		/// Detachs from slot.
		/// </summary>
		/// <returns><c>true</c>, if from slot was detached, <c>false</c> otherwise.</returns>
		/// <param name="_object">Object.</param>
		public bool DetachFromSlot( GameObject _object ){
			return CreatureRegister.DetachFromTransform( _object );
		}

		/// <summary>
		/// Gives a detached item.
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="_position">Position.</param>
		/// <param name="_rotation">Rotation.</param>
		public GameObject GiveItem( Vector3 _position, Quaternion _rotation )
		{
			GameObject _item = ItemObject;

			if( _item != null )
			{
				m_ItemObject = null;
				DetachFromSlot( _item );
				Amount--;

				_item.transform.position = _position;
				_item.transform.rotation = _rotation;
			}

			return _item;
		}

		/// <summary>
		/// Gives an item from slot (optional attached to the specified transform).
		/// </summary>
		/// <returns>The detached item.</returns>
		/// <param name="_transform">Transform.</param>
		/// <param name="_attach">If set to <c>true</c> attach the item to the specified transform.</param>
		public GameObject GiveItem( Transform _transform, bool _attach )
		{
			if( _transform == null )
				return null;

			GameObject _item = GiveItem( _transform.position, _transform.rotation );

			if( _attach )
				CreatureRegister.AttachToTransform( _item, _transform );

			return _item;
		}

		/// <summary>
		/// Gives an detached item.
		/// </summary>
		/// <returns>The item.</returns>
		public GameObject GiveItem(){
			return GiveItem( ( MountPointTransform != null ? MountPointTransform : Owner.transform ), false );
		}

		/// <summary>
		/// Drops an detached item.
		/// </summary>
		public void DropItem(){
			GameObject _item = GiveItem();

			if( _item == null )
			{
				Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range( 0, 360 ) , 0 );
				Vector3 _position = PositionTools.FixTransformPoint( Owner.transform, new Vector3( 0,5,3 ) );
				if( DropRange > 0 )
					_position = ICE.World.Utilities.PositionTools.GetRandomPosition( _position, DropRange );

				_item = CreatureRegister.Spawn( ItemReferenceObject, _position, _rotation );

				Amount--;
			}
		}

		public void Copy( InventorySlotObject _slot )
		{
			if( _slot == null )
				return;

			MountPointName = _slot.MountPointName;
			IsExclusive = _slot.IsExclusive;
			ItemName = _slot.ItemName;
			MaxAmount = _slot.MaxAmount;	
			Amount = _slot.Amount;
			UseRandomAmount = _slot.UseRandomAmount;
			UseDetachOnDie = _slot.UseDetachOnDie;
		}
		/*
		public int Merge( InventorySlotObject _slot )
		{
			if( _slot == null )
				return 0;

			IsExclusive = _slot.IsExclusive;
			Key = _slot.Key;
			Amount = Amount + _slot.Amount;
			MaxAmount = _slot.MaxAmount;	
			UseRandomAmount = _slot.UseRandomAmount;

			return true;
		}*/

		public int TryUpdateAmount( string _name, int _amount )
		{
			// if name is empty or unequal to the given slotname the full amount will returned
			if( ! CheckName( _name ) || ( ItemNameIsValid && ItemName != _name ) )
				return _amount;
			else
			{
				// if the slot isn't named we have to set the name before we update the amount
				if( ! ItemNameIsValid )
					m_ItemName = _name;

				// now we can update the amount and return the rest ...
				return UpdateAmount( _amount );
			}
		}

		public int UpdateAmount( int _amount )
		{
			int _rest = 0;
			int _new_amount = Amount + _amount;

			if( _new_amount < 0 ){
				_rest = _new_amount;
				Amount = 0;
			}
			else if( _new_amount > MaxAmount ){
				_rest = _new_amount - MaxAmount;
				Amount = MaxAmount;
			}
			else{
				Amount = _new_amount;
			}

			return _rest;
		}
	}

	[System.Serializable]
	public class InventoryDataObject : ICEOwnerObject
	{
		public InventoryDataObject(){}
		public InventoryDataObject( InventoryDataObject _inventory ) { Copy( _inventory ); }
		public InventoryDataObject( ICEWorldBehaviour _component ) : base( _component ){}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );

			foreach( InventorySlotObject _slot in Slots )
				_slot.Init( Owner );
		}

		public void Copy( InventoryObject _inventory )
		{
			if( _inventory == null )
				return;

			base.Copy( _inventory );

			MaxSlots = _inventory.MaxSlots;
			IgnoreInventoryOwner = _inventory.IgnoreInventoryOwner;
			DefaultDropRange = _inventory.DefaultDropRange;
			LastCollectedObjectID = _inventory.LastCollectedObjectID;
			UseDetachOnDie = _inventory.UseDetachOnDie;

			Slots.Clear();
			foreach( InventorySlotObject _slot in _inventory.Slots )
				Slots.Add( new InventorySlotObject( OwnerComponent, _slot ) );
		}

		[SerializeField]
		private List<InventorySlotObject> m_Slots = null;
		public List<InventorySlotObject> Slots{
			get{ return m_Slots = ( m_Slots == null ? new List<InventorySlotObject>() : m_Slots ); }
			set{ 
				Slots.Clear(); 
				if( value == null ) return;			
				foreach( InventorySlotObject _slot in value )
					Slots.Add( new InventorySlotObject( OwnerComponent, _slot ) );
			}
		}

		public int AvailableSlots{
			get{ return Slots.Count; }
		}
		public int MaxSlots = 9;
		public bool IgnoreInventoryOwner = false;
		public float DefaultDropRange = 0.5f;
		public int LastCollectedObjectID = 0;
		public bool UseDetachOnDie = false;
		public bool UseStaticSlots = false;

		public List<string> AvailableItems{
			get{
				List<string> _list = new List<string>();

				foreach( InventorySlotObject _slot in Slots )
				{
					if( _slot.ItemName != null && _slot.ItemName != "" && _list.Contains( _slot.ItemName ) == false )
						_list.Add( _slot.ItemName );
				}

				return _list;
			}
		}
	}

	[System.Serializable]
	public class InventoryObject : InventoryDataObject
	{
		public InventoryObject(){}
		public InventoryObject( InventoryObject _inventory ) : base( _inventory ) {}
		public InventoryObject( ICEWorldBehaviour _component ) : base( _component ) { Init( _component ); }


		public InventoryObject Copy(){
			return new InventoryObject( this );
		}

		public override void Reset()
		{
			foreach( InventorySlotObject _slot in Slots )
				_slot.Reset();
		}

		/// <summary>
		/// Drops one item from the slot with the specified _index.
		/// </summary>
		/// <param name="_index">Index.</param>
		public void Drop( int _index )
		{
			InventorySlotObject _slot = GetSlotByIndex( _index );
			if( _slot != null )
				_slot.DropItem();
		}

		public bool Insert( GameObject _object )
		{
			if( _object == null || ! _object.activeInHierarchy )
				return false;

			InventorySlotObject _slot = ForceSlotByItem( _object, 1 );
			if( _slot != null )
			{
				if( _slot.SetItemObject( _object ) )
				{
					if( DebugLogIsEnabled ) PrintDebugLog( this, "Insert - '" + _object.name + "' inserted and attached to slot (new amount : " + _slot.Amount + "/" + _slot.MaxAmount + ")" ); 
				}
				else if( _slot.FreeCapacity >= 1 )
				{
					if( _slot.IsEmpty )
						_slot.ItemName = _object.name;
					
					_slot.Amount++;
					CreatureRegister.Remove( _object );

					if( DebugLogIsEnabled ) PrintDebugLog( this, "Insert - '" + _object.name + "' amount increased of slot (new amount : " + _slot.Amount + "/" + _slot.MaxAmount + ") and original object removed" ); 
				}
				else
				{
					//if( DebugLogIsEnabled ) PrintDebugLog( this, "Insert - '" + _object.name + "' amount increased of slot (new amount : " + _slot.Amount + "/" + _slot.MaxAmount + ") and original object removed" ); 
					return false;
				}

				return true;
			}

			return false;
		}

		public void Insert( InventoryObject _inventory )
		{
			if( _inventory == null )
				return;

			foreach( InventorySlotObject _slot in _inventory.Slots )
				_slot.Amount = Insert( _slot.ItemName, _slot.Amount );
		}

		public int Insert( string _name, int _amount )
		{
			int _rest = _amount;
			InventorySlotObject _named_slot = ForceSlotByItemName( _name, _amount );

			if( _named_slot != null )
				_rest = _named_slot.TryUpdateAmount( _name, _rest );
	
			if( _rest > 0 )
			{
				foreach( InventorySlotObject _slot in Slots )
					_rest = _slot.TryUpdateAmount( _name, _rest );
			}

			return _rest;
		}

		/*
		public InventoryObject Transfer()
		{
			InventoryObject _inventory = new InventoryObject();

			foreach( InventorySlotObject _slot in _inventory.Slots )
			{
				if( _slot.IsTransferable )
				{
					_inventory.Slots.Add( new InventorySlotObject( _slot ) );
					_slot.Amount = 0;
				}
			}

			if( ! _inventory.IgnoreInventoryOwner )
				_inventory.Insert( Owner.name, 1 );

			return _inventory;
		}*/


		/// <summary>
		/// Gets the empty slot.
		/// </summary>
		/// <returns>The empty slot.</returns>
		public InventorySlotObject GetEmptySlot()
		{
			foreach( InventorySlotObject _slot in Slots )
			{
				if( _slot != null && string.IsNullOrEmpty( _slot.ItemName ) == true )
					return _slot;
			}

			return null;
		}
			
		/// <summary>
		/// Gets a slot by the specified name
		/// </summary>
		/// <returns>The slot by item name.</returns>
		/// <param name="_name">Name.</param>
		public InventorySlotObject GetSlotByItemName( string _name )
		{
			foreach( InventorySlotObject _slot in Slots )
			{
				if( _slot != null && _slot.ItemName == _name )
					return _slot;
			}

			return null;
		}

		/// <summary>
		/// Gets a slot by the specified name and a desired free capacity
		/// </summary>
		/// <returns>The slot by item name.</returns>
		/// <param name="_name">Name.</param>
		/// <param name="_amount">Amount.</param>
		public InventorySlotObject GetSlotByItemName( string _name, int _amount )
		{
			foreach( InventorySlotObject _slot in Slots )
			{
				if( _slot != null && _slot.ItemName == _name && _slot.FreeCapacity >= _amount )
					return _slot;
			}

			return null;
		}

		/// <summary>
		/// Gets a slot by the specified index.
		/// </summary>
		/// <returns>The slot by index.</returns>
		/// <param name="_index">Index.</param>
		public InventorySlotObject GetSlotByIndex( int _index )
		{
			if( _index >= 0 && _index < Slots.Count )
				return Slots[ _index ];
			else
				return null;
		}

		private InventorySlotObject ForceSlotByItem( GameObject _item, int _required_amount )
		{
			if( _item == null )
				return null;
			
			InventorySlotObject _slot = ForceSlotByItemName( _item.name, _required_amount );
			if( _slot != null && _item.transform.IsChildOf( Owner.transform ) )
				_slot.MountPointName = _item.transform.parent.name;

			return _slot;
		}

		private InventorySlotObject ForceSlotByItemName( string _name, int _required_amount )
		{
			InventorySlotObject _slot = GetSlotByItemName( _name, _required_amount );

			if( _slot == null )
				_slot = GetEmptySlot();
				
			// if there is no slot and the inventory is not static create a new slot
			if( _slot == null && ( ! UseStaticSlots || ! Application.isPlaying ) )
			{ 
				_slot = new InventorySlotObject( OwnerComponent, _name, _required_amount );
				Slots.Add( _slot );
			}

			return _slot;
		}
			
		/// <summary>
		/// Current item amount of the specified slot.
		/// </summary>
		/// <returns>The item amount.</returns>
		/// <param name="_index">Index.</param>
		public int SlotItemAmount( int _index )
		{
			if( _index >= 0 && _index < Slots.Count )
				return Slots[_index].Amount;
			else
				return 0;
		}

		/// <summary>
		/// Current item amount of the specified slot and item name
		/// </summary>
		/// <returns>The item amount.</returns>
		/// <param name="_index">Index.</param>
		/// <param name="_name">Name.</param>
		public int SlotItemAmount( int _index, string _name )
		{
			if( _index > 0 && _index < Slots.Count && Slots[ _index ].ItemName == _name )
				return Slots[ _index ].Amount;
			else
				return 0;
		}

		/// <summary>
		/// Current amount of the specified item name
		/// </summary>
		/// <returns>The item amount.</returns>
		/// <param name="_name">Name.</param>
		public int SlotItemAmount( string _name )
		{
			InventorySlotObject _slot = GetSlotByItemName( _name );
			if( _slot != null )
				return _slot.Amount;
			else
				return 0;
		}

		public int SlotItemMaxAmount( int _index )
		{
			if( _index >= 0 && _index < Slots.Count )
				return Slots[_index].MaxAmount;
			else
				return 0;
		}
			
		public string SlotItemName( int _index )
		{
			if( _index >= 0 && _index < Slots.Count )
				return Slots[_index].ItemName;
			else
				return "";
		}

		public void Detach()
		{
		}

		public void DetachOnDie()
		{
			foreach( InventorySlotObject _slot in Slots )
			{
				if( _slot != null && ( _slot.UseDetachOnDie == true || UseDetachOnDie == true ) && _slot.Amount > 0 )
				{
					GameObject _reference = _slot.ItemReferenceObject;
					if( _reference != null )
					{
						Vector3 _position = Owner.transform.TransformPoint(Vector3.zero);
						Quaternion _rotation = Owner.transform.rotation;

						while( _slot.Amount > 0 )
						{
							if( _slot.DropRange > 0 )
								_position = ICE.World.Utilities.PositionTools.GetRandomPosition( _position, _slot.DropRange );

							GameObject _item = CreatureRegister.Spawn( _reference, _position, _rotation );
							//GameObject _item = (GameObject)GameObject.Instantiate( _reference, _position, _rotation );
							if( _item != null )
							{
								_item.name = _reference.name;
					
								if( _item.GetComponent<Rigidbody>() != null )
								{
									_item.GetComponent<Rigidbody>().useGravity = true;
									_item.GetComponent<Rigidbody>().isKinematic = false;
									_item.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
								}
							}

							_slot.Amount--;
						}

						if( _slot.ItemObject )
							WorldManager.Remove( _slot.ItemObject );
					}
				}
			}
		}

		public void Action( InventoryActionDataObject _action )
		{
			if( _action == null || ! _action.Enabled )
				return;
			
			if( _action.DropItemRequired() )
			{
				InventorySlotObject _slot = GetSlotByItemName( _action.ItemName );			
				if( _slot != null && _slot.Amount > 0 )
				{
					Transform _transform = ICE.World.Utilities.SystemTools.FindChildByName( _action.ParentName, Owner.transform );
					_transform = ( _transform != null ? _transform : Owner.transform );

					Quaternion _rotation = Quaternion.Euler( 0, UnityEngine.Random.Range( 0, 360 ) , 0 );
					Vector3 _position = PositionTools.FixTransformPoint( _transform, _action.Offset );

					GameObject _item = _slot.GiveItem( _position, _rotation );
					if( _item == null )
					{
						_item = CreatureRegister.Spawn( _slot.ItemReferenceObject, _position, _rotation );
						_slot.Amount--;
					}
				}
			}
			else if( _action.ParentUpdateRequired() )
			{
				InventorySlotObject _slot = GetSlotByItemName( _action.ItemName );
				if( _slot != null && _slot.Amount > 0 )
				{
					if( _slot.ItemObject != null )
						_slot.MountPointName = _action.ParentName;
				}
			}
			else if( _action.CollectActiveItemRequired() )
			{
				ICECreatureControl _control = OwnerComponent as ICECreatureControl;
				TargetObject _target = _control.Creature.ActiveTarget;

				if( _control != null && _target != null && _target.Selectors.TotalCheckIsValid ) //&& LastCollectedObjectID != _target.TargetID  )
				{

					GameObject _item = _target.TargetGameObject;

					//LastCollectedObjectID = _target.TargetID;
				
					if( _target.EntityComponent != null && _target.EntityComponent.IsChildEntity )
					{
						ICEWorldEntity _parent = _target.EntityComponent.RootEntity;
						if( _parent != null )
						{
	
							if( DebugLogIsEnabled ) PrintDebugLog( this, "CollectActiveItem : take '" + _target.Name + "' from " + _parent.name + " (" + _parent.ObjectInstanceID + ")" );  

							InventorySlotObject _slot = GetInventorySlot( _parent.gameObject, _target.TargetName  );
							if( _slot != null )
								_item = _slot.GiveItem();
						}
					}
		
					if( Insert( _item ) )
					{
						//Debug.Log( _control.Creature.ActiveTarget.TargetGameObject.name + " - " +  _control.Creature.ActiveTarget.TargetGameObject.GetInstanceID() );
						//_target.ResetTargetGameObject();
						_control.Creature.ResetActiveTarget();

						// 
					}
						
				}
			}
		}

		public static InventorySlotObject GetInventorySlot( GameObject _object, string _slot )
		{
			if( _object == null )
				return null;

			InventoryObject _inventory = GetInventory( _object );
			if( _inventory != null )
				return _inventory.GetSlotByItemName( _slot );
			else
				return null;
		}

		public static InventoryObject GetInventory( GameObject _object )
		{
			if( _object == null )
				return null;

			ICECreatureEntity _entity = _object.GetComponent<ICECreatureEntity>();
			InventoryObject _inventory = null;
			if( _entity != null )
			{
				if( _entity as ICECreatureControl != null )
				{
					ICECreatureControl _creature = _entity as ICECreatureControl;
					if( _creature != null )
						_inventory = _creature.Creature.Status.Inventory;
				}
				else if( _entity as ICECreaturePlayer != null )
				{
					ICECreaturePlayer _player = _entity as ICECreaturePlayer;
					if( _player != null )
						_inventory = _player.Inventory;
				}
				else if( _entity as ICECreatureItem != null )
				{
					ICECreatureItem _item = _entity as ICECreatureItem;
					if( _item != null )
						_inventory = _item.Inventory;
				}
			}

			return _inventory;
		}

		public bool ItemExists( string _key ) 
		{
			if( _key == "" )
				return false;
			
			foreach( InventorySlotObject _item in Slots )
			{
				if( _item.ItemName == _key )
					return true;
			}

			return false;
		}
	}

	[System.Serializable]
	public class PlayerInventoryObject : InventoryObject
	{
		public PlayerInventoryObject(){}
		public PlayerInventoryObject( PlayerInventoryObject _inventory ) : base( _inventory ) {}
		public PlayerInventoryObject( ICEWorldBehaviour _component ) : base( _component ) { Init( _component ); }

	}
}
