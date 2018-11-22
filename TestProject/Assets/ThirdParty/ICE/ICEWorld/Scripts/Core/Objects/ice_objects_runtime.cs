// ##############################################################################
//
// ice_objects_runtime.cs | EntityRuntimeOptionsObject
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
using ICE.World.EnumTypes;

namespace ICE.World.Objects
{
	[System.Serializable]
	public class EntityRuntimeBehaviourObject : ICEOwnerObject
	{
		public EntityRuntimeBehaviourObject(){ Enabled = true; }
		public EntityRuntimeBehaviourObject( ICEWorldBehaviour _component ) : base( _component ){ Enabled = true; }
		public EntityRuntimeBehaviourObject( EntityRuntimeBehaviourObject _object ){ Copy( _object );  }

		public void Copy( EntityRuntimeBehaviourObject _object )
		{
			if( _object == null )
				return;
			
			base.Copy( _object );
			Enabled = true;

			UseDontDestroyOnLoad = _object.UseDontDestroyOnLoad;
			UseCoroutine = _object.UseCoroutine;

			UseRuntimeName = _object.UseRuntimeName;
			RuntimeName = _object.RuntimeName;

			CullingOptions = _object.CullingOptions;
		}

		public bool UseRemoveOnLost = false;
		public float LostRemovingLevel = -200;
		public float LostRemovingLevelMaximum = 1000;

		public bool UseDontDestroyOnLoad = false;
		public bool UseCoroutine = true;

		public bool UseRuntimeName = false;
		public string RuntimeName = "";

		public bool UseHierarchyManagement = true;

		public bool Pause = false;

		[SerializeField]
		private CullingOptionsObject m_CullingOptions = null;
		public CullingOptionsObject CullingOptions{
			get{ return m_CullingOptions = ( m_CullingOptions == null ? new CullingOptionsObject() : m_CullingOptions ); }
			set{ CullingOptions.Copy( value ); }
		}
			
		public void RuntimeRenaming( ICEWorldBehaviour _component ){
			RuntimeRenaming( _component.gameObject );
		}

		public void RuntimeRenaming( GameObject _object )
		{
			if( _object != null && UseRuntimeName && ! string.IsNullOrEmpty( RuntimeName ) )
				_object.name = RuntimeName;
		}

		/// <summary>
		/// Gets a value indicating whether this object is lost in space.
		/// </summary>
		/// <value><c>true</c> if this object is lost in space; otherwise, <c>false</c>.</value>
		public bool IsLost{
			get{ return ( UseRemoveOnLost && Owner != null && Owner.transform.position.y <= LostRemovingLevel ? true : false ); }
		}
	}

}
