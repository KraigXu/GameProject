// ##############################################################################
//
// ICE.World.ICEObject.cs
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

namespace ICE.World
{
	public class ICEWorldObject : ICEWorldBehaviour {

		protected Rigidbody m_Rigidbody = null;
		public Rigidbody ObjectRigidbody{
			get{ return m_Rigidbody = ( m_Rigidbody == null?GetComponent<Rigidbody>():m_Rigidbody ); }
		}

		protected Collider m_ObjectCollider = null;
		public Collider ObjectCollider{
			get{ return m_ObjectCollider = ( m_ObjectCollider == null?GetComponent<Collider>():m_ObjectCollider ); }
		}

		protected Renderer[] m_ObjectRenderer = null;
		public Renderer[] ObjectRenderer{
			get{ return m_ObjectRenderer = ( m_ObjectRenderer == null?GetComponentsInChildren<Renderer>():m_ObjectRenderer ); }
		}

		protected Mesh[] m_ObjectMeshes = null;
		public Mesh[] ObjectMeshes{
			get{ return m_ObjectMeshes = ( m_ObjectMeshes == null?GetComponentsInChildren<Mesh>():m_ObjectMeshes ); }
		}

		protected Collider[] m_ObjectColliders = null;
		public Collider[] ObjectColliders{
			get{ return m_ObjectColliders = ( m_ObjectColliders == null?GetComponentsInChildren<Collider>():m_ObjectColliders ); }
		}

		protected Transform[] m_ObjectTransforms = null;
		public Transform[] ObjectTransforms {
			get{ return m_ObjectTransforms  = ( m_ObjectTransforms == null?GetComponentsInChildren<Transform>():m_ObjectTransforms ); }
		}

		public override void Start () {
			base.Start();
		}

		public override void Update () {

		}
	}
}