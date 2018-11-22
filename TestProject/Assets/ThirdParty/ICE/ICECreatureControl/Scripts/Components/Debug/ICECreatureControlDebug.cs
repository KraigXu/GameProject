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
using ICE.Creatures;
using ICE.Creatures.Objects;
using ICE.Creatures.EnumTypes;
using ICE.Creatures.Utilities;

namespace ICE.Creatures
{
	[RequireComponent (typeof (ICECreatureControl))]
	public class ICECreatureControlDebug : ICECreatureEntityDebug 
	{
		//--------------------------------------------------
		// Action Object
		//--------------------------------------------------
		[SerializeField]
		private DebugObject m_CreatureDebug = null;
		public DebugObject CreatureDebug{
			set{ m_CreatureDebug.Copy( value ); }
			get{ return m_CreatureDebug = ( m_CreatureDebug == null ? new DebugObject() :m_CreatureDebug ); }
		}

		public override void Awake () {

			base.Awake();

			CreatureDebug.Init( this );

			CreatureDebug.CreatureControl.Creature.Move.OnMoveComplete += OnMoveComplete;
			CreatureDebug.CreatureControl.Creature.Move.OnUpdateMovePosition += OnMoveUpdatePosition;
		}


		void OnMoveComplete( GameObject _sender, TargetObject _target  )
		{
	
			if( m_CreatureDebug.MovePointer.Enabled && m_CreatureDebug.MovePointer.Pointer != null )
				m_CreatureDebug.MovePointer.Position = m_CreatureDebug.CreatureControl.Creature.Move.MovePosition;

			if( m_CreatureDebug.TargetMovePositionPointer.Enabled && m_CreatureDebug.TargetMovePositionPointer.Pointer != null &&  _target != null )
				m_CreatureDebug.TargetMovePositionPointer.Position = _target.TargetMovePosition;

			if( m_CreatureDebug.DesiredTargetMovePositionPointer.Enabled && m_CreatureDebug.DesiredTargetMovePositionPointer.Pointer != null &&  _target != null )
				m_CreatureDebug.DesiredTargetMovePositionPointer.Position = _target.DesiredTargetMovePosition;
		}

		void OnMoveUpdatePosition(  GameObject _sender, Vector3 _origin_position, ref Vector3 _new_position )
		{
			/*if( m_CreatureDebug.DebugLogEnabled )
				Debug.Log ( _origin_position.ToString() + " - " + _new_position.ToString() );*/
		}

		/// <summary>
		/// Raises the draw gizmos selected event.
		/// </summary>
		public override void OnDrawGizmosSelected(){
			DrawGizmos( DrawSelectedOnly );
		}

		/// <summary>
		/// Raises the draw gizmos event.
		/// </summary>
		public override void OnDrawGizmos(){
			DrawGizmos( ! DrawSelectedOnly );			
		}

		/// <summary>
		/// Draws the gizmos.
		/// </summary>
		/// <param name="_enabled">If set to <c>true</c> enabled.</param>
		public override void DrawGizmos( bool _enabled )
		{
			if( ! this.enabled || ! _enabled )
				return;

			m_CreatureDebug.Init( this );
			m_CreatureDebug.Gizmos.DrawOwnerGizmos( GizmoColor );

			if( ! m_CreatureDebug.Gizmos.Enabled )
				return;
			
			m_CreatureDebug.Gizmos.DrawHome();
			m_CreatureDebug.Gizmos.DrawOutpost();
			m_CreatureDebug.Gizmos.DrawEscort();
			m_CreatureDebug.Gizmos.DrawPatrol();
			m_CreatureDebug.Gizmos.DrawInteraction();
		}
	}
}
