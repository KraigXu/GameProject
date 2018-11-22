// ##############################################################################
//
// ICECreatureRegisterDebug.cs
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

using ICE.World.Utilities;

using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Utilities;


namespace ICE.Creatures
{
	[RequireComponent (typeof (ICECreatureRegister))]
	public class ICECreatureRegisterDebug : MonoBehaviour {

		private void OnDrawGizmosSelected()
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			if( _register == null )
				return;
			
			if( _register.RegisterDebug.UseDrawSelected )
				DrawRegisterGizmos();
		}

		private void OnDrawGizmos()
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			if( _register == null )
				return;
			
			if( ! _register.RegisterDebug.UseDrawSelected )
				DrawRegisterGizmos();
		}

		private void DrawRegisterGizmos()
		{
			ICECreatureRegister _register = ICECreatureRegister.Instance;
			if( _register == null )
				return;
			
			if( ! _register.UseDebug )
				return;

			Gizmos.color = new Color( Color.blue.r,Color.blue.g,Color.blue.b, 0.75f );
			Vector3 _s = new Vector3( 0.5f, 0.15f, 0.5f );
			Gizmos.DrawCube( transform.position + ( Vector3.up * 0.25f ), _s );
			Gizmos.DrawCube( transform.position + ( Vector3.up * 0.50f ), _s );
			Gizmos.DrawCube( transform.position + ( Vector3.up * 0.75f ), _s );
			Gizmos.color = new Color( Color.blue.r,Color.blue.g,Color.blue.b, 1f);
			CustomGizmos.Text( "CREATURE REGISTER", transform.position, Gizmos.color, 14, FontStyle.Bold );

			foreach( ReferenceGroupObject _group in _register.ReferenceGroupObjects )
			{
				if( _group.ReferenceGameObject == null )
					continue;

				if( _register.RegisterDebug.ShowReferenceGizmos && ! _group.Status.isPrefab )
				{
					Gizmos.color = new Color( _register.RegisterDebug.ColorReferences.r, _register.RegisterDebug.ColorReferences.g, _register.RegisterDebug.ColorReferences.b, 0.25f );
					Vector3 _pos = _group.ReferenceGameObject.transform.position;
					float _size = Mathf.Clamp( _group.ReferenceGameObject.transform.lossyScale.magnitude, 0.25f, 1 ) * 0.25f;
					Gizmos.DrawSphere( _pos, _size );
					_pos.y += 2;

					if(_register.RegisterDebug.ShowReferenceGizmosText )
					{
						Gizmos.color = new Color( _register.RegisterDebug.ColorReferences.r, _register.RegisterDebug.ColorReferences.g, _register.RegisterDebug.ColorReferences.b, 1f );
						CustomGizmos.Text( _group.ReferenceGameObject.name + " (SCENE REFERENCE)", _pos , Gizmos.color, 12, FontStyle.Italic );
					}
				}

				if( _register.RegisterDebug.ShowCloneGizmos )
				{
					foreach( GameObject _item in _group.ActiveObjects )
					{
						if( _group.ReferenceGameObject == _item )
							continue;

						Gizmos.color = new Color( _register.RegisterDebug.ColorClones.r, _register.RegisterDebug.ColorClones.g, _register.RegisterDebug.ColorClones.b, 0.25f );
						Vector3 _pos = _item.transform.position;
						float _size = Mathf.Clamp( _item.transform.lossyScale.magnitude, 0.25f, 1 ) * 0.25f;
						Gizmos.DrawSphere( _pos, _size );

						if(_register.RegisterDebug.ShowCloneGizmosText )
						{
							_pos.y += 2;
							Gizmos.color = new Color( _register.RegisterDebug.ColorClones.r, _register.RegisterDebug.ColorClones.g, _register.RegisterDebug.ColorClones.b, 1f );
							CustomGizmos.Text( _item.name + " (CLONE)", _pos , Gizmos.color, 12, FontStyle.Italic );
						}
					}
				}

				if(_register.RegisterDebug.ShowSpawnPointGizmos && _group.PoolManagementEnabled )
				{
					foreach( SpawnPointObject _point in _group.ValidSpawnPoints )
					{
						if( _point.SpawnPointGameObject == null )
							continue;

						GameObject[] _objects = _point.GetAllSpawnPointGameObjects();

						foreach( GameObject _object in _objects )
						{
							Gizmos.color = new Color( _register.RegisterDebug.ColorSpawnPoints.r, _register.RegisterDebug.ColorSpawnPoints.g, _register.RegisterDebug.ColorSpawnPoints.b, 0.25f );
							Vector3 _pos = _object.transform.position;
							float _size = Mathf.Clamp( _object.transform.lossyScale.magnitude, 0.25f, 1 ) * 0.25f;
							Gizmos.DrawSphere( _pos, _size );

							if( _point.UseRandomRect )
							{
								CustomGizmos.Box( _object.transform, _point.RandomRect, new Vector3( 0,_point.RandomRect.y * 0.5f,0 ) );
							}
							else
							{
								CustomGizmos.Circle( _pos,_point.SpawningRangeMin,CustomGizmos.GetBestDegrees(_point.SpawningRangeMin, 360), false );
								CustomGizmos.BeamCircle( _pos,_point.SpawningRangeMax,CustomGizmos.GetBestDegrees(_point.SpawningRangeMax, 360), false, _point.SpawningRangeMax - _point.SpawningRangeMin, "", false, true );
							}

							if(_register.RegisterDebug.ShowSpawnPointGizmosText )
							{
								_pos.z += _point.SpawningRangeMax;
								_pos.y += 4;
								Gizmos.color = new Color( _register.RegisterDebug.ColorSpawnPoints.r, _register.RegisterDebug.ColorSpawnPoints.g, _register.RegisterDebug.ColorSpawnPoints.b, 1f );
								CustomGizmos.Text( _point.SpawnPointGameObject.name + " (SP)", _pos , Gizmos.color, 12, FontStyle.Italic );
							}

						}
					}
				}
			}
		}
	}
}
