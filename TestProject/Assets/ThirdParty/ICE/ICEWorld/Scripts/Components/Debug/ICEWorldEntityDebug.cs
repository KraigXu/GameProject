// ##############################################################################
//
// ICE.World.ICEWorldEntityDebug.cs
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

using ICE.World.Objects;
using ICE.World.Utilities;

namespace ICE.World
{
	[RequireComponent (typeof (ICEWorldEntity)), ExecuteInEditMode]
	public class ICEWorldEntityDebug : ICEWorldBehaviour {

		public float BaseOffset = 0;
		public float BaseOffsetMaximum = 1;
		public bool GroundedInEditorMode = false;

		public bool UseCustomGizmoColor = false;
		public Color CustomGizmoColor = Color.red;
		public float GizmoSize = 0.25f;

		public Color GizmoColor{
			get{ return ( UseCustomGizmoColor ? CustomGizmoColor : WorldManager.GetDebugDefaultColor( this.gameObject ) ); }
		}

		public bool DrawSelectedOnly = false;

		public override void Update () {

			#if UNITY_EDITOR
			if( GroundedInEditorMode == true && ! Application.isPlaying && UnityEditor.Selection.activeGameObject == transform.gameObject )
				WorldManager.SetGroundLevel( transform, BaseOffset );	
			#endif
		}

		public virtual void OnDrawGizmos(){
			DrawGizmos( ! DrawSelectedOnly );
		}

		public virtual void OnDrawGizmosSelected(){
			DrawGizmos( DrawSelectedOnly );
		}

		public virtual void DrawGizmos( bool _draw )
		{
			if( ! _draw || ! this.enabled )
				return;

			Gizmos.color = GizmoColor;
			Gizmos.DrawSphere( this.transform.position, GizmoSize );
			//Gizmos.DrawWireCube( this.transform.position, Vector3.one );
		}
	}
}
