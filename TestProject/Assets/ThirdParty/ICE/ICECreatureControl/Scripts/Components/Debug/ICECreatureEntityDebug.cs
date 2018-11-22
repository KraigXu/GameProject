// ##############################################################################
//
// ICECreatureControlDebug.cs
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
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Utilities;

namespace ICE.Creatures
{
	[RequireComponent (typeof (ICECreatureEntity)), ExecuteInEditMode]
	public class ICECreatureEntityDebug : ICEWorldEntityDebug 
	{
		private ICECreatureEntity m_Entity = null;
		protected ICECreatureEntity Entity{
			get{ return m_Entity = ( m_Entity == null ? GetComponent<ICECreatureEntity>() : m_Entity ); }
		}

		public override void DrawGizmos( bool _draw )
		{
			if( ! _draw || ! this.enabled || Entity == null )
				return;

			CustomGizmos.Color( GizmoColor );
			Gizmos.DrawSphere( this.transform.position, GizmoSize );
			//Gizmos.DrawWireCube( this.transform.position, Vector3.one );

			if( Entity.EntityType == ICE.World.EnumTypes.EntityClassType.Waypoint )
			{
				ICECreatureWaypoint _waypoint = Entity as ICECreatureWaypoint;

				if( _waypoint != null && _waypoint.Links.Enabled )
				{
					foreach( WaypointLinkObject _link in _waypoint.Links.Links )
					{
						if( _link.Enabled && _link.Waypoint != null )
						{
							float _dist = 0.25f;

							Vector3 _pos_origin = this.transform.position;
							Vector3 _pos_target = _link.Waypoint.transform.position;			
							Vector3 _dir = ( _pos_target - _pos_origin ).normalized;
							//_dir.y = 0;
							Vector3 _rot_dir = _dir;
							_rot_dir.y = 0;
							_rot_dir = Quaternion.Euler(0, 90, 0) * _rot_dir;

							Gizmos.DrawLine( _pos_origin + ( _rot_dir * _dist ) , _pos_target + ( _rot_dir * _dist ) );
							//CustomGizmos.Arrow( _pos_origin + ( _rot_dir * _dist ) + ( _dir * 2 ),  _dir );
							CustomGizmos.ArrowHead(  _pos_origin + ( _rot_dir * _dist ) + ( _dir * 1.0f ),  _dir, _dist * 2, _dist * 100 );
							CustomGizmos.ArrowHead(  _pos_origin + ( _rot_dir * _dist ) + ( _dir * 1.5f ),  _dir, _dist * 2, _dist * 100 );
							CustomGizmos.ArrowHead(  _pos_origin + ( _rot_dir * _dist ) + ( _dir * 2.0f ),  _dir, _dist * 2, _dist * 100 );
							//CustomGizmos.Arrow( 0,  _pos_origin + ( _rot_dir * _dist ) + ( _dir * 1 ), Quaternion.LookRotation( _dir ), _dist * 10 );
						}
					}
				}


			}
		}
	}
}
