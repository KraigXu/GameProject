using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform TargetTF;


    public GameObject PlayerGo;
    public NavMeshAgent agent;

    private static PlayerController _instance;

    public static PlayerController Instance
    {
        get { return _instance; }
    }


    void Awake()
    {
        _instance = this;
    }
	void Start () {
		
        SignalCenter.MouseOnMove.AddListener(PlayerOnMouseMove);
	    SignalCenter.MouseOnRightUp.AddListener(PlayerOnRightClick);
        SignalCenter.MouseOnLeftUp.AddListener(PlayerOnLeftClick);

	}


    void Update()
    {

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyTeamTipsWindow);
        }

        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            //队伍信息
              UICenterMasterManager.Instance.DestroyWindow(WindowID.StrategyTeamTipsWindow);  


        }
        


    }


    void OnDestroy()
    {
        SignalCenter.MouseOnMove.RemoveListener(PlayerOnMouseMove);
        SignalCenter.MouseOnMove.RemoveListener(PlayerOnLeftClick);
    }

    private void PlayerOnMouseMove(MouseInput input)
    {
        TargetTF.position = input.touchPoint;
    }


    private void PlayerOnLeftClick(MouseInput input)
    {
        if (PlayerGo != null)
        {
            agent = PlayerGo.GetComponent<NavMeshAgent>();
            agent.destination = input.touchPoint;
        }
    }

    private void PlayerOnRightClick(MouseInput input)
    {
        if (PlayerGo != null)
        {
            PlayerGo.gameObject.transform.position = input.touchPoint;
            //   NavMeshAgent agent = PlayerGo.GetComponent<NavMeshAgent>();
            //   agent.destination = input.touchPoint;
        }
    }





    void OnDrawGizmos()
    {
        if (agent == null) return;
        var path = agent.path;
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
