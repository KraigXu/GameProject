// ##############################################################################
//
// ICECreatureObjectSelect.cs
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

using ICE.World.EnumTypes;

//using ICE.Creatures;
//using ICE.Creatures.EnumTypes;

namespace ICE.World
{
	public class ICEWorldSelectableEntity : MonoBehaviour {

		public static class SelectedObject 
		{
			private static GameObject m_SelectedObject = null;
			public static GameObject SelectOject{
				get{return m_SelectedObject; }
				set{
					if( value == null || m_SelectedObject == null )
						m_SelectedObject = value; 		    
				}
			}
		}

		public struct RenderData 
		{
			public Renderer renderer;
			public int ID;
			public Material[] materials;
			public Shader shader;
		}



		public bool IsSelected = false;

		public ObjectSelectType SelectType = ObjectSelectType.OVER;
		public ObjectSelectVisibilityType VisibilityType = ObjectSelectVisibilityType.COLOR;

		public Color SelectionColor = Color.red;
		public Material SelectionMaterial = null;
		public Shader SelectionShader = null;
		public GameObject SelectionEffect = null;

		public bool FreezeCreature = true;
		public float TimeScale = 1;
		public float SelectDelay = 0.5f;


		private GameObject m_SelectionEffect = null;
		private float m_DefaultTimeScale = 1;
		private float m_SelectTimer = 0;
		private Renderer[] m_Renderer;
		private List<RenderData> m_RenderDatas = new List<RenderData>();

		// Use this for initialization
		void Start () {
		
			
			m_DefaultTimeScale = Time.timeScale;

			// cache all renderer data
			m_Renderer = GetComponentsInChildren<Renderer>();			
			foreach( Renderer _renderer in m_Renderer )
			{
				RenderData _data = new RenderData();
				
				_data.renderer = _renderer;
				_data.ID = _renderer.GetInstanceID();
				_data.materials = _renderer.materials;
				
				m_RenderDatas.Add( _data );
			}
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		void OnMouseUpAsButton() {
			
			if( SelectType == ObjectSelectType.CLICK )
				ChangeSelection( ! IsSelected );
		}

		void OnMouseEnter(){

			m_SelectTimer += Time.deltaTime;
			if( SelectType == ObjectSelectType.OVER && m_SelectTimer > SelectDelay )		
				ChangeSelection( true );

		}
		// Called when mouse exit this object
		void OnMouseExit () {

			m_SelectTimer = 0;
			if( SelectType == ObjectSelectType.OVER )
				ChangeSelection( false );
			
		}
		
		// Called when mouse click on this object
		void OnMouseDown () {

		}

		public void ChangeSelection( bool _selected )
		{
			if( _selected )
				Select();
			else
				Deselect();
		}

		public void Select()
		{
			if( SelectedObject.SelectOject != null )
				return;

			SelectedObject.SelectOject = gameObject;
			m_SelectTimer = 0;
			IsSelected = true;
			ChangeSelectionMaterials( IsSelected );

			/*
			if( FreezeCreature )
			{
				ICECreatureControl _control = GetComponent<ICECreatureControl>();
				if( _control != null )
				{
					_control.Creature.Behaviour.BehaviourAnimation.Break();
					_control.enabled = false;
				}
			}*/
			
			if( TimeScale != m_DefaultTimeScale )
				Time.timeScale = TimeScale;

			if( m_SelectionEffect == null && SelectionEffect != null )
			{
				m_SelectionEffect = (GameObject)Instantiate( SelectionEffect, Vector3.zero, SelectionEffect.transform.rotation ); 
				m_SelectionEffect.transform.SetParent( transform, false );
			}
		}

		public void Deselect()
		{
			SelectedObject.SelectOject = null;
			IsSelected = false;
			ChangeSelectionMaterials( IsSelected );
			/*
			if( FreezeCreature )
			{
				ICECreatureControl _control = GetComponent<ICECreatureControl>();
				if( _control != null )
				{
					_control.enabled = true;
					_control.Creature.Behaviour.BehaviourAnimation.Play();
				}
			}*/
			
			if( TimeScale != m_DefaultTimeScale )
				Time.timeScale = m_DefaultTimeScale;

			if( m_SelectionEffect != null )
			{
				GameObject.DestroyImmediate( m_SelectionEffect );
				m_SelectionEffect = null;
			}


			m_SelectTimer = 0;


		}


		/// <summary>
		/// Changes the color of the selection.
		/// </summary>
		/// <param name="_selected">If set to <c>true</c> _selected.</param>
		public void ChangeSelectionMaterials( bool _selected )
		{
			if( VisibilityType == ObjectSelectVisibilityType.NONE )
				return;

			if( _selected )
			{
				foreach( Renderer _renderer in m_Renderer )
				{
					Material[] _temp = new Material[_renderer.materials.Length];
					for( int i = 0 ; i < _renderer.materials.Length ; i++ )
					{
						if( VisibilityType == ObjectSelectVisibilityType.MATERIAL && SelectionMaterial != null )
							_temp[i] = new Material( SelectionMaterial );
									
						if( VisibilityType == ObjectSelectVisibilityType.SHADER && SelectionShader != null )
						{
							_temp[i] = new Material( _renderer.materials[i] );
							_temp[i].shader = SelectionShader;
						}

						if( VisibilityType == ObjectSelectVisibilityType.COLOR )
						{
							_temp[i] = new Material( _renderer.materials[i] );
							_temp[i].color = SelectionColor;
						}
					}
					
					_renderer.materials = _temp;
				}
			}
			else
			{
				foreach( Renderer _renderer in m_Renderer )
					foreach( RenderData _data in m_RenderDatas )
						if( _data.ID == _renderer.GetInstanceID() )
							_renderer.materials = _data.materials;
				
			}
		}
	}
}