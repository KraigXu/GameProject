﻿using UnityEngine;

namespace GameSystem.AI
{
    /// <summary>
    /// 顺序抵达巡逻点
    /// </summary>
    [CreateAssetMenu(menuName = "GameSystem/PluggableAI/Actions/OrderlyPatrol")]
    public class PatrolOrderlyAction : Action
    {
        public override void Act(StateController controller)
        {
            //controller.navMeshAgent.destination = controller.NextWaypoint.position;
            //controller.navMeshAgent.isStopped = false;

            //if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            //    controller.UpdateNextWayPoint(false);
        }
    }
}
