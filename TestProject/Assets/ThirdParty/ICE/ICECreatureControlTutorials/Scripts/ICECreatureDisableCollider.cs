using UnityEngine;
using System.Collections;

public class ICECreatureDisableCollider : MonoBehaviour {

	// just to buffering the initial kinematic state
	private bool m_HasKinematicRigidbody = true;

	private ICE.Creatures.ICECreatureControl m_Creature = null;
	private ICE.Creatures.ICECreatureControl m_AttachedCreature{
		get{ return m_Creature = ( m_Creature == null ? this.gameObject.GetComponent<ICE.Creatures.ICECreatureControl>() : m_Creature ); }
	}

	private Collider m_Collider = null;
	private Collider m_AttachedCollider{
		get{ return m_Collider = ( m_Collider == null ? this.gameObject.GetComponent<Collider>() : m_Collider ); }
	}

	private Rigidbody m_Rigidbody = null;
	private Rigidbody m_AttachedRigidbody{
		get{ return m_Rigidbody = ( m_Rigidbody == null ? this.gameObject.GetComponent<Rigidbody>() : m_Rigidbody ); }
	}

	void Start(){

		// we store the initial kinematic state 
		if( m_AttachedRigidbody != null )
			m_HasKinematicRigidbody = m_AttachedRigidbody.isKinematic;
		
	}
	void OnEnable(){

		// we enable the collider 
		if( m_AttachedCollider != null )
			m_AttachedCollider.enabled = true;

	}

	void OnDisable(){

		// whenever the object will be disabled ...

		// ... we restore the original kinematic state
		if( m_AttachedRigidbody != null )
			m_AttachedRigidbody.isKinematic = m_HasKinematicRigidbody;

		// ... and we enable the collider again
		if( m_AttachedCollider != null )
			m_AttachedCollider.enabled = true;

	}

	void Update () {

		// makes sure that we have all required components and the desired conditions are fullfilled - the creature must be dead and the collider must be enabled
		if( m_AttachedCreature != null && m_AttachedCollider != null && m_AttachedCollider.enabled && m_AttachedCreature.Creature.Status.IsDead )
		{
			// we have to make sure that a non-kinematic rigidbody will not carry the gameobject away 
			if( m_AttachedRigidbody != null )
				m_AttachedRigidbody.isKinematic = true;

			m_AttachedCollider.enabled = false;
		}
	}
}
