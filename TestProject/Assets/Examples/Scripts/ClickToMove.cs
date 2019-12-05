using UnityEngine;
using UnityEngine.AI;

// Use physics raycast hit from mouse click to set agent destination
[RequireComponent(typeof(NavMeshAgent))]
public class ClickToMove : MonoBehaviour
{
    NavMeshAgent m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();

    void Start()
    {
        m_Agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                m_Agent.destination = m_HitInfo.point;
        }
    }
    void OnDrawGizmos()
    {
        var path = m_Agent.path;
        // color depends on status
        Color c = Color.white;
        switch (path.status)
        {
            case UnityEngine.AI.NavMeshPathStatus.PathComplete: c = Color.white; break;
            case UnityEngine.AI.NavMeshPathStatus.PathInvalid: c = Color.red; break;
            case UnityEngine.AI.NavMeshPathStatus.PathPartial: c = Color.yellow; break;
        }
        // draw the path
        for (int i = 1; i < path.corners.Length; ++i)
            Debug.DrawLine(path.corners[i - 1], path.corners[i], c);
    }

}
