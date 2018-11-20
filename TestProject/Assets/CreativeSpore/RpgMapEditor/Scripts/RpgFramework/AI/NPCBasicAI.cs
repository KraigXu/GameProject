using UnityEngine;
using System.Collections;

namespace CreativeSpore.RpgMapEditor
{
    [AddComponentMenu("RpgMapEditor/AI/NPCBasicAI", 10)]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    [RequireComponent(typeof(DirectionalAnimation))]
    [RequireComponent(typeof(IsoSpriteController))]
    public class NPCBasicAI : MonoBehaviour
    {
        [Tooltip("Maximum waiting time between steps")]
        public float MaxStepDelay = 2f;
        [Tooltip("Minimum waiting time between steps")]
        public float MinStepDelay = 0.5f;
        [Tooltip("Distance covered in each step")]
        public float MovingStepDist = 1f;
        [Tooltip("Moving Speed")]
        public float MovingSpeed = 1f;

        private PhysicCharBehaviour m_physicBhv;
        private DirectionalAnimation m_dirAnim;
        void Start()
        {
            m_physicBhv = GetComponent<PhysicCharBehaviour>();
            m_dirAnim = GetComponent<DirectionalAnimation>();
            StopAllCoroutines();
            StartCoroutine(DoLogic());
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            EditorCompatibilityUtils.CircleCap(0, transform.position, transform.rotation, MovingStepDist);
        }
#endif

        void OnEnable()
        {
            Start();            
        }

        void OnDisable()
        {
            StopCoroutine(DoLogic());
        }

        void DoCollision(Vector2 normal)
        {
            Vector2 vReflect = Vector3.Reflect(m_velocity, normal);
            m_velocity = Vector2.Lerp(vReflect, m_velocity, Random.value / 2f);
        }

        void OnCollisionEnter(Collision other)
        {
            DoCollision(other.contacts[0].normal);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            DoCollision(other.contacts[0].normal);
        }

        Vector2 m_velocity;
        IEnumerator DoLogic()
        {
            m_physicBhv.Dir = Vector2.zero;
            m_dirAnim.IsPlaying = false;
            yield return new WaitForSeconds(Random.Range(MinStepDelay, MaxStepDelay));
            while (true)
            {
                m_dirAnim.IsPlaying = true;
                m_velocity = Random.rotation * Vector2.right;
                m_velocity = m_velocity.normalized * MovingStepDist;
                for (float dist = 0; dist < MovingStepDist; )
                {
                    yield return new WaitForFixedUpdate();
                    if(m_physicBhv.CollFlags != 0)
                    {
                        switch(m_physicBhv.CollFlags)
                        {
                            case PhysicCharBehaviour.eCollFlags.DOWN: DoCollision(Vector2.up); break;
                            case PhysicCharBehaviour.eCollFlags.UP: DoCollision(-Vector2.up); break;
                            case PhysicCharBehaviour.eCollFlags.RIGHT: DoCollision(-Vector2.right); break;
                            case PhysicCharBehaviour.eCollFlags.LEFT: DoCollision(Vector2.right); break;
                        }
                    }
                    Debug.DrawRay(transform.position, m_velocity);
                    Vector2 disp = m_velocity.normalized * MovingSpeed;
                    dist += disp.magnitude * Time.deltaTime;
                    m_physicBhv.MaxSpeed = MovingSpeed;
                    m_physicBhv.Dir = disp;
                    m_dirAnim.SetAnimDirection(disp);
                }
                m_physicBhv.Dir = Vector2.zero;
                m_dirAnim.IsPlaying = false;
                yield return new WaitForSeconds(Random.Range(MinStepDelay, MaxStepDelay));
            }
        }
    }
}