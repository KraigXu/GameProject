using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(LineRenderer))]
public class PlayerControl : MonoBehaviour
{
    public LayerMask RayLayerId;
    public Camera CurCamera;
    public GameObject TargetGo;

    private Ray _ray;
    private RaycastHit _hit;
    private AICharacterControl _characterControl;
    private NavMeshAgent _agent;
    private LineRenderer _moveLine;

    // Use this for initialization
    void Start () {
	    if (CurCamera == null)
	    {
            CurCamera=Camera.main;
	    }

	    if (TargetGo == null)
	    {
	        TargetGo =new GameObject("Target");
          //  TargetGo.transform.SetParent(this.transform);
	    }

        _characterControl = transform.GetComponent<AICharacterControl>();
        _agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        _moveLine = GetComponent<LineRenderer>();
        _moveLine.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {
        _ray = CurCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(_ray, out _hit, RayLayerId))
        {
            if (Input.GetMouseButtonUp(0))
            {
                TargetGo.transform.position = _hit.point;
                Move();
            }
        }
        Debug.DrawLine(_ray.origin,_hit.point,Color.red);
    }

    private void Move()
    {
        _characterControl.SetTarget(TargetGo.transform);
        //设置路径的点，
        //路径  导航。
        NavMeshPath path = new NavMeshPath();
        _agent.CalculatePath(TargetGo.transform.position, path);
        //线性渲染设置拐点的个数。数组类型的。
        _moveLine.positionCount = path.corners.Length;
        //线性渲染的拐点位置，数组类型，
        _agent.SetDestination(TargetGo.transform.position);
        _moveLine.SetPositions(path.corners);
    }

    public void Setting(float moveSpeed)
    {
        _agent.speed = moveSpeed;
    }


    



}
