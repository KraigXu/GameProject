using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/Controllers/BulletController", 10)]
	public class BulletController : MonoBehaviour {

		public float Speed = 0.1f;
		public Vector3 Dir = new Vector3();
		public float TimeToLive = 5f;
        public float DamageQty = 0.5f;

		public GameObject OnDestroyFx;
		public bool IsDestroyOnCollision = true;

		void Start()
		{
			Destroy( transform.gameObject, TimeToLive);
		}

		void Update () 
		{
			if( AutoTileMap.Instance.GetAutotileCollisionAtPosition( transform.position ) == eTileCollisionType.BLOCK )
			{
				Destroy( transform.gameObject );
			}
			transform.position += Speed * Dir * Time.deltaTime;
		}

		void OnDestroy()
		{
			if( OnDestroyFx != null )
			{
				Instantiate( OnDestroyFx, transform.position, transform.rotation );
			}
		}
		
		void OnTriggerStay(Collider other) 
		{
			if( IsDestroyOnCollision && other.attachedRigidbody && (other.gameObject.layer != gameObject.layer) )
			{
				//apply damage here
                DamageData.ApplyDamage(other.attachedRigidbody.gameObject, DamageQty, Dir);
				Destroy(gameObject);
			}
		}
	}
}
