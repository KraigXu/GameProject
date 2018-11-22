using UnityEngine;

using ICE.Creatures;
using ICE.Creatures.EnumTypes;

public class ICECreatureCustomMovements : MonoBehaviour {

	private ICECreatureControl m_Controller = null;
	protected ICECreatureControl AttachedCreatureController{
		get{ return m_Controller = ( m_Controller == null ? GetComponent<ICECreatureControl>() : m_Controller ); }
	}

	private Rigidbody m_Rigidbody = null;
	protected Rigidbody AttachedRigidbody{
		get{ return m_Rigidbody = ( m_Rigidbody == null ? GetComponent<Rigidbody>() : m_Rigidbody ); }
	}

	private MotionControlType m_OriginalMotionControlType;
	private bool m_OriginalIsKinematic = false;
	private float m_Velocity = 0;
		
	void OnDisable() 
	{
		// restore the original MotionControlType
		if( AttachedCreatureController != null )
			AttachedCreatureController.Creature.Move.MotionControl = m_OriginalMotionControlType;

		// restore the original IsKinematic value
		if( AttachedRigidbody != null )
			AttachedRigidbody.isKinematic = m_OriginalIsKinematic;

		// reset velocity
		m_Velocity = 0;
	}

	void Update () {

		if( AttachedCreatureController == null )
			return;

		// check conditions
		if( AttachedCreatureController.Creature.Status.IsDead || AttachedCreatureController.Creature.Status.DurabilityInPercent < 5 )
		{
			// optional buffer for the original MotionControlType
			m_OriginalMotionControlType = AttachedCreatureController.Creature.Move.MotionControl;

			// enable custom moves
			AttachedCreatureController.Creature.Move.MotionControl = MotionControlType.CUSTOM;

			// deactivate the internal gravity
			AttachedCreatureController.Creature.Move.UseInternalGravity = false;

			if( AttachedRigidbody )
			{
				// optional buffer for the original isKinematic value
				m_OriginalIsKinematic = AttachedRigidbody.isKinematic;

				AttachedRigidbody.isKinematic = true;
			}

			// handle your custom moves ... example ...

			m_Velocity += 0.025f;

			if( transform.position.y < 200 )
				transform.Translate( transform.up * (m_Velocity * m_Velocity) * Time.deltaTime );
		}
	}
}
