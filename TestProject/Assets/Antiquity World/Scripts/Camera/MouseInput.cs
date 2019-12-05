using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;

public class MouseInput : MonoBehaviour
{
    [HideInInspector]
    public Vector2 deltaPan = Vector2.zero;
    [HideInInspector]
    public float deltaScale = 0f;
    private bool holding = false;
    private float holdTime = 0f;    
    private bool _isCorss;
    private Vector3 lastMousePosition = Vector3.zero;

    public int mouseButton = 0;
    public Vector3 touchPoint = Vector3.zero;
    public GameObject touchObject;
    public bool isHideBBox = false;
    public bool IsCrossUi;

    private static MouseInput m_touch;
    public static MouseInput Instance
    {
        get
        {
            return m_touch;
        }
    }

    void Awake()
    {
        m_touch = this;
    }
     void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
      
        if (Input.mousePosition.x > Screen.width || Input.mousePosition.x <= 0 || Input.mousePosition.y > Screen.height || Input.mousePosition.y <= 0)
        {
            IsCrossUi = true;
            SignalCenter.MouseOnLeaveView.Dispatch(this);
            return;
        }
        else
        {
            IsCrossUi = false;
            SignalCenter.MouseOnEnterView.Dispatch(this);
           
        }

        Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            touchPoint = hit.point;
            touchObject = hit.collider.gameObject;
        }
        else
        {
            Debug.Log("TouchPoint为NULL");
            touchPoint=Vector3.zero;
            touchObject = null;
        }

        SignalCenter.MouseOnMove.Dispatch(this);


        //鼠标左键按下时
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            mouseButton = 0;
            Cursor.visible = true;
            SignalCenter.MouseOnLeftDown.Dispatch(this);
            //Debug.Log("左键按下");
            return;
        }

        //鼠标左键弹起
        if (Input.GetMouseButtonUp(0))
        {
            Cursor.visible = true;
            mouseButton = 0;
            SignalCenter.MouseOnLeftUp.Dispatch(this);
            //Debug.Log("左键弹起");
            return;
        }
        
        if (Input.GetMouseButton(0))
        {
            mouseButton = 0;
            //Debug.Log("左键点击");
            return;
        }

        

      
        //右键按住旋转
        if (Input.GetMouseButton(1))
        {
            //pressedMouseButton = 1;
            if (holdTime > 0.15f)
            {
                this.holding = true;
                Cursor.visible = false;
                if (lastMousePosition != Vector3.zero)
                {
                   
                    mouseButton = 1;
                    deltaPan = Input.mousePosition - lastMousePosition;
                }
                lastMousePosition = Input.mousePosition;
                SignalCenter.MouseOnRight.Dispatch(this);
            }
            else
            {
                holdTime += Time.deltaTime;
            }
            return;
        }
        //右键弹起
        if (Input.GetMouseButtonUp(1))
        {
            Cursor.visible = true;
            mouseButton = 1;
            holding = false;
            if (holdTime < 0.15f)
            {
                SignalCenter.MouseOnRightUp.Dispatch(this);
            }
            holdTime = 0f;
            lastMousePosition = Vector3.zero;
            
            return;
        }
        if (Input.GetMouseButtonDown(2))
        {
            mouseButton = 2;
            SignalCenter.MouseOnMiddleDown.Dispatch(this);
        }

        if (Input.GetMouseButtonUp(2))
        {
            mouseButton = 2;
            SignalCenter.MouseOnMiddleUp.Dispatch(this);
        }

        if (Input.GetMouseButton(2))
        {
            mouseButton = 2;
            SignalCenter.MouseOnMiddle.Dispatch(this);
        }

        //移动
        if (Input.GetMouseButton(2)) 
        {
            mouseButton = 2;
            if (lastMousePosition != Vector3.zero)
            {
                this.deltaPan = Input.mousePosition - lastMousePosition;

            }
            lastMousePosition = Input.mousePosition;
            return;
        }

        //缩放
        float value;
        if ((value=Input.GetAxis("Mouse ScrollWheel"))!=0f)
        {
            mouseButton = -1;
            this.deltaScale = value * 500;
            SignalCenter.MouseOnScrollWheel.Dispatch(this);
            return;
        }



        if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        {
            Cursor.visible = true;
            mouseButton = -1;
            this.holding = false;
            lastMousePosition = Vector3.zero;
            return;
        }





    }



}

