using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {

        public delegate void ContactTargetEvent(int code,int targetId);

        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for

        public Vector3 TargetV3=Vector3.zero;
        public bool IsMove = false;
        public int  TargetCode;
        public int TargetId;
        private ContactTargetEvent _contactTargetEvent;
        private LineRenderer _moveLine;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();
            _moveLine = gameObject.GetComponent<LineRenderer>();
            if (_moveLine == null)
            {
                _moveLine = gameObject.AddComponent<LineRenderer>();
            }

            agent.updateRotation = false;
	        agent.updatePosition = true;
        }


        private void Update()
        {
            if (target != null)
                agent.SetDestination(target.position);

            if (TargetV3 != Vector3.zero)
            {
                agent.SetDestination(TargetV3);
            }
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                IsMove = true;
                character.Move(agent.desiredVelocity, false, false);
            }
            else
            {
                IsMove = false;
                character.Move(Vector3.zero, false, false);
                if (_contactTargetEvent != null)
                {
                    _contactTargetEvent(TargetCode,TargetId);
                    _contactTargetEvent = null;
                }
            }       
        }
        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void SetTarget(Vector3 targetV3, ContactTargetEvent contactEvent,int targetCode,int targetid)
        {
            this.TargetV3 = targetV3;
            this._contactTargetEvent = contactEvent;
            this.TargetCode = targetCode;
            this.TargetId = targetid;
            //����·���ĵ㣬
            //·��  ������
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetV3, path);
            //������Ⱦ���ùյ�ĸ������������͵ġ�
            _moveLine.positionCount = path.corners.Length;
            //������Ⱦ�Ĺյ�λ�ã��������ͣ�
            agent.SetDestination(targetV3);
            _moveLine.SetPositions(path.corners);
        }

    }
}
