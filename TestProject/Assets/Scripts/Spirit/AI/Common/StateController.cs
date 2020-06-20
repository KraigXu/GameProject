
using GameSystem.AI;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace GameSystem.AI
{
    public class StateController : MonoBehaviour
    {
        public State currentState;                              // 当前状态
        public State remainState;                               // 保持当前状态
        public AIState defaultConfig;                            // 默认状态信息
        public Point eyesPoint=new Point(new Vector3(0,1,1.1f),Vector3.zero);                                 // 眼睛：拿来观察状态变化
        public Rigidbody rigidbodySelf;                         // 自己的刚体
        public Collider colliderSelf;                           // 自己的Collider
        //public NavMeshAgent navMeshAgent;                       // 导航组件
        public Points waypoints;

        public SafeDictionary<CommonCode, object> statePrefs;           // 用于每一次状态保存信息使用时（OnExitState时清除）
        public SafeDictionary<CommonCode, object> instancePrefs;        // 用于整个实例保存信息用（如ChaseEnemy）

  
        private State startState;                               // 初始状态，每次复活后重置
        private Transform target;                               // 追踪目标

        private int nextWaypointIndex;                          // 下一个巡逻点
        public Point NextWaypoint { get { return waypoints[nextWaypointIndex]; } }

     //   public TeamManager Team { get { return playerManager.Team; } }
        public Color debugColor = Color.green;          // 调试颜色
        public bool fromEyes = true;                    // 是否从眼睛放出检测射线，否则是自身
        [Range(0, 360)]
        public float angle = 90f;                       // 检测前方角度范围
        [Range(1, 50)]
        public int accuracy = 3;                        // 检测精度
        [Range(0, 100)]
        public float distance = 25f;                    // 检测距离
        [Range(0, 1800)]
        public float rotatePerSecond = 90f;             // 每秒旋转角度

        /// <summary>
        /// 获取组件
        /// </summary>
        private void Awake()
        {
            rigidbodySelf = GetComponent<Rigidbody>();
            colliderSelf = GetComponent<Collider>();
            startState = currentState;
            statePrefs = new SafeDictionary<CommonCode, object>();
            instancePrefs = new SafeDictionary<CommonCode, object>();
            
        }

        /// <summary>
        /// 配置导航信息，重置状态
        /// </summary>
        private void OnEnable()
        {
            currentState = startState;                          //复活的时候重置状态
        }

        /// <summary>
        /// 失效时同时清除状态信息
        /// </summary>
        private void OnDisable()
        {
            OnExitState();
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        private void Update()
        {
            currentState.UpdateState(this);                     //更新状态
        }


        /// <summary>
        /// 配置AI
        /// </summary>
        /// <param name="enable">是否可用</param>
        public void SetupAI(bool enable)
        {
            enabled = enable;
        //    navMeshAgent.enabled = enable;
            //UpdateNextWayPoint(true);
        }

        /// <summary>
        /// 转换到下一个状态
        /// </summary>
        /// <param name="nextState">下一个状态</param>
        public void TransitionToState(State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                OnExitState();
            }
        }

        /// <summary>
        /// 改变状态后调用
        /// </summary>
        private void OnExitState()
        {
           //statePrefs.Clear();
        }
        /// <summary>
        /// 碰撞是否是自己
        /// </summary>
        /// <param name="collider">碰撞的物体</param>
        /// <returns>是否是自己</returns>
        public bool IsMyself(Collider collider)
        {
            return collider == colliderSelf;
        }

        /// <summary>
        /// 查找敌人，并存到instancePrefs
        /// </summary>
        /// <returns>是否查找到敌人</returns>
        public bool FindEnemy(Quaternion anger, float radius, Color debugColor, bool isFromEyes = true)
        {
            RaycastHit hit;
            if (isFromEyes)
            {
                target = FindTarget.FindEnemy(out hit, eyesPoint.GetWorldPosition(transform),eyesPoint.GetWorldForward(transform), gameObject, anger, radius, debugColor);
            }
            else
            {
                target = FindTarget.FindEnemy(out hit, transform.position, transform.forward, gameObject, anger, radius, debugColor);
            }
               
            if (target != null)
            {
                instancePrefs.AddOrModifyValue(CommonCode.ChaseEnemy, target);
                return true;
            }
            return false;   
        }
            
        /// <summary>
        /// 攻击
        /// </summary>
        public void Attack()
        {
            //Property.SetNewTarget((Transform)instancePrefs[CommonCode.ChaseEnemy]);
            //Property.Attack();
        }

        public void Look()
        {
            
        }

    }
    
}



