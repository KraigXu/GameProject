// ##############################################################################
//
// ice_objects_layers.cs | ICE.World.Objects.LayerObject
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

namespace ICE.World.Objects
{
	[System.Serializable]
	public class LayerObject : ICEDataObject 
	{
		public LayerObject(){}
		public LayerObject( LayerMask _mask ){ SetDefaultLayerMaskByMask( _mask ); }
		public LayerObject( string _name ){ SetDefaultLayerMaskByName( _name ); }
		public LayerObject( int _index ){ SetDefaultLayerMaskByIndex( _index ); }
		public LayerObject( LayerObject _object ){ Copy( _object ); }

		public void Copy( LayerObject _object )
		{
			if( _object == null )
				return;

			base.Copy( _object );

			SetLayers( _object.Layers );

			SetDefaultLayerMaskByMask( _object.DefaultLayerMask );
		}

		private LayerMask m_DefaultLayerMask = 0;
		public LayerMask DefaultLayerMask{
			get{ return m_DefaultLayerMask; }
		}

		[SerializeField]
		private List<string> m_Layers = null;
		public List<string> Layers{
			get{ return m_Layers = ( m_Layers == null ? new List<string>() : m_Layers ); }
		}

		private LayerMask m_LayerMask = 0;
		public LayerMask Mask{
			get{ return m_LayerMask = SystemTools.GetLayerMask( Layers, m_LayerMask, m_DefaultLayerMask ); }
		}

		/// <summary>
		/// Sets the default layer Mask by a layer index.
		/// </summary>
		/// <param name="_index">Index.</param>
		public void SetDefaultLayerMaskByIndex( int _index ){
			m_DefaultLayerMask = ( 1 << _index ); 
		}

		/// <summary>
		/// Sets the default layer Mask by a layer name.
		/// </summary>
		/// <param name="_name">Name.</param>
		public void SetDefaultLayerMaskByName( string _name ){
			m_DefaultLayerMask = ( 1 << LayerMask.NameToLayer( _name ) ); 
		}

		/// <summary>
		/// Sets the default layer Mask by mask.
		/// </summary>
		/// <param name="_mask">Mask.</param>
		public void SetDefaultLayerMaskByMask( LayerMask _mask ){
			m_DefaultLayerMask = _mask; 
		}

		/// <summary>
		/// Resets the layer mask.
		/// </summary>
		public void ResetLayerMask(){
			m_LayerMask = 0;
		}

		/// <summary>
		/// Ignore the specified _mask.
		/// </summary>
		/// <param name="_mask">Mask.</param>
		public LayerMask Ignore( LayerMask _mask )
		{
			foreach( string _name in m_Layers )
			{
				int _layer = LayerMask.NameToLayer( _name );
				if( _layer != -1 && SystemTools.IsInLayerMask( _layer, _mask ) )
					_mask |= (1 >> _layer );
			}

			return _mask;
		}

		public bool Contains( int _layer ){
			return SystemTools.IsInLayerMask( _layer, Mask );
		}

		/// <summary>
		/// Sets the layers by string list.
		/// </summary>
		/// <param name="_layers">Layer string list.</param>
		public void SetLayers( List<string> _layers )
		{
			if( _layers == null )
				return;

			Layers.Clear();
			foreach( string _name in _layers )
				Layers.Add( _name );
		}
	}
}
	
