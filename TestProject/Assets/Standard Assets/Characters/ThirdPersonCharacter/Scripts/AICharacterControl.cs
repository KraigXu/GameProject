using System;
using UnityEngine;
using UnityEngine.AI;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public enum FollowType
        {
            None,
            Transform,
            Vector
        }
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling

        public bool IsMove = false;
        [SerializeField]
        private FollowType _followType;
        [SerializeField]
        private Transform _targetrTransform;
        [SerializeField]
        private Vector3 _targetVector3;

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

            _moveLine.startWidth = 0.1f;
            _moveLine.endWidth = 0.1f;
            

            agent.updateRotation = false;
	        agent.updatePosition = true;
        }


         void Update()
        {
            switch (_followType)
            {
                case FollowType.None:

                    break;
                case FollowType.Transform:
                {
                    agent.SetDestination(_targetrTransform.position);
                }
                    
                    break;
                case FollowType.Vector:
                {
                    agent.SetDestination(_targetVector3);
                }
                   
                    break;
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
                _followType = FollowType.None;
            }
                
        }

        void FixedUpdate()
        {
            if (agent.path.corners.Length > 1)
            {
                _moveLine.positionCount = agent.path.corners.Length;
                _moveLine.SetPositions(agent.path.corners);
            }

        }


        public void SetTarget(Transform target)
        {
            this._targetrTransform = target;
            _followType = FollowType.Transform;


            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target.position, path);
            //线性渲染设置拐点的个数。数组类型的。
            _moveLine.positionCount = path.corners.Length;
            //线性渲染的拐点位置，数组类型，
            agent.SetDestination(target.position);
            _moveLine.SetPositions(path.corners);

        }

        public void SetTarget(Vector3 target)
        {
            this._targetVector3 = target;
            _followType = FollowType.Vector;

            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(target, path);
            //线性渲染设置拐点的个数。数组类型的。
            _moveLine.positionCount = path.corners.Length;
            //线性渲染的拐点位置，数组类型，
            agent.SetDestination(target);
            _moveLine.SetPositions(path.corners);
        }
    }
}
