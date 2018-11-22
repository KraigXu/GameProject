// ##############################################################################
//
// ice_CreatureMemory.cs
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
using ICE.Creatures.Utilities;
using ICE.World.Utilities;

using ICE.World;
using ICE.World.Objects;

namespace ICE.Creatures.Objects
{
	[System.Serializable]
	public class MemoryItemObject : ICEDataObject
	{
		public MemoryItemObject(){}
		public MemoryItemObject( MemoryItemObject _memory ) : base( _memory ){ 
			Copy( _memory );
		}
		
		public void Copy( MemoryItemObject _memory )
		{
		}
	}
	
	[System.Serializable]
	public class MemoryDataObject : ICEDataObject
	{
		public MemoryDataObject(){}
		public MemoryDataObject( MemoryDataObject _memory ) : base( _memory ){ 
			Copy( _memory );
		}
		
		public void Copy( MemoryDataObject _memory )
		{
			CapacityMax = _memory.CapacityMax;
			Capacity = _memory.Capacity;

			m_Items.Clear();
			foreach( MemoryItemObject _item in _memory.Items )
				m_Items.Add( new MemoryItemObject( _item ) );
		}

		public int CapacityMax = 100;
		public int Capacity = 100;

		private List<MemoryItemObject> m_Items = new List<MemoryItemObject>();
		public List<MemoryItemObject> Items{
			get{ return m_Items; }
		}
	}
	
	[System.Serializable]
	public class SpatialMemoryObject : MemoryDataObject
	{
		public SpatialMemoryObject(){}
		public SpatialMemoryObject( SpatialMemoryObject _memory ) : base( _memory as MemoryDataObject ) { 
			Copy( _memory );
		}
		
		public void Copy( SpatialMemoryObject _memory )
		{
		}
	}

	[System.Serializable]
	public class LongTermMemoryObject : MemoryDataObject
	{
		public LongTermMemoryObject(){}
		public LongTermMemoryObject( LongTermMemoryObject _memory ) : base( _memory as MemoryDataObject ) { 
			Copy( _memory );
		}
		
		public void Copy( LongTermMemoryObject _memory )
		{
		}
	}

	[System.Serializable]
	public class ShortTermMemoryObject : MemoryDataObject
	{
		public ShortTermMemoryObject(){}
		public ShortTermMemoryObject( ShortTermMemoryObject _memory ) : base( _memory as MemoryDataObject ) { 
			Copy( _memory );
		}
		
		public void Copy( ShortTermMemoryObject _memory )
		{
		}
	}
	
	[System.Serializable]
	public class MemoryObject : ICEDataObject
	{
		public MemoryObject(){}
		public MemoryObject( MemoryObject _memory ) : base( _memory ){ 
			Copy( _memory );
		}
		
		public void Copy( MemoryObject _memory )
		{
			SpatialMemory = new SpatialMemoryObject( _memory.SpatialMemory );
			LongTermMemory = new LongTermMemoryObject( _memory.LongTermMemory );
			ShortTermMemory = new ShortTermMemoryObject( _memory.ShortTermMemory );
		}

		public SpatialMemoryObject SpatialMemory = new SpatialMemoryObject();
		public LongTermMemoryObject LongTermMemory = new LongTermMemoryObject();
		public ShortTermMemoryObject ShortTermMemory = new ShortTermMemoryObject();
	}

}