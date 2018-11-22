// ##############################################################################
//
// ice_objects_corpse.cs | CorpseObject
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

using ICE;
using ICE.World;
using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World.Objects
{

	[System.Serializable]
	public class CorpseObject : ICEOwnerObject {

		public CorpseObject(){}
		public CorpseObject( CorpseObject _object ) : base( _object ){ Copy( _object );}
		public CorpseObject( ICEWorldBehaviour _component ) : base( _component )
		{
			Init( _component );
		}

		public override void Init( ICEWorldBehaviour _component )
		{
			base.Init( _component );
			m_Corpse = null;
		}

		public void Copy( CorpseObject _object  )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			CorpseReferencePrefab = _object.CorpseReferencePrefab;
			CorpseRemovingDelay = _object.CorpseRemovingDelay;
			CorpseRemovingDelayMin = _object.CorpseRemovingDelayMin;
			CorpseRemovingDelayMax = _object.CorpseRemovingDelayMax;
			CorpseRemovingDelayMaximum = _object.CorpseRemovingDelayMaximum;
			UseCorpseScaling = _object.UseCorpseScaling;
			UseRandomDelay = _object.UseRandomDelay;
		}

		public override void Reset()
		{
			m_Corpse = null;
		}

		[XmlIgnore]
		public GameObject CorpseReferencePrefab = null;
		public float CorpseRemovingDelay = 20.0f;
		public float CorpseRemovingDelayMin = 20.0f;
		public float CorpseRemovingDelayMax = 20.0f;
		public float CorpseRemovingDelayMaximum = 20.0f;
		public bool UseCorpseScaling = false;
		public bool UseRandomDelay = false;

		private GameObject m_Corpse = null;
		public void SpawnCorpse()
		{
			if( Enabled == false || IsRemoteClient || m_Corpse != null || Owner.activeInHierarchy == false || CorpseReferencePrefab == null )
				return;

			//Debug.Log( "SPAWN CORSE" );

			m_Corpse = WorldManager.Spawn( CorpseReferencePrefab, Owner.transform.position, Owner.transform.rotation );
			if( m_Corpse != null )
			{
				m_Corpse.name = CorpseReferencePrefab.name;
				SystemTools.CopyTransforms( Owner.transform, m_Corpse.transform );

				if( UseCorpseScaling )
					m_Corpse.transform.localScale = Owner.transform.localScale;

				m_Corpse.SetActive( true );

				if( CorpseRemovingDelay > 0 )
					WorldManager.Destroy( m_Corpse, ( UseRandomDelay ? UnityEngine.Random.Range( CorpseRemovingDelayMin, CorpseRemovingDelayMax ) : CorpseRemovingDelay ) ); 
			}
		}
	}
}
	
