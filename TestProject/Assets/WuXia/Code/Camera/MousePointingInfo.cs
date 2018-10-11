using System.Collections;
using System.Collections.Generic;
using UnityEngine;





/// <summary>
/// 鼠标指向信息
/// </summary>
public class MousePointingInfo : MonoBehaviour
{

    public delegate void OnMousePointing(Transform tf);

    public delegate void OnMousePointingPoint(Transform tf, Vector3 point);

    public Camera _camera;

    /// <summary>
    /// 当标识的鼠标进入时
    /// </summary>
    public Dictionary<string, OnMousePointing> MouseEnterEvents = new Dictionary<string, OnMousePointing>();

    public Dictionary<string, OnMousePointing> MouseExitEvents = new Dictionary<string, OnMousePointing>();

    public Dictionary<string, OnMousePointing> MouseOverEvents = new Dictionary<string, OnMousePointing>();

    public Dictionary<string, OnMousePointingPoint> Mouse0ClickEvents = new Dictionary<string, OnMousePointingPoint>();

    public Dictionary<string, OnMousePointingPoint> Mouse1ClickEvents = new Dictionary<string, OnMousePointingPoint>();

    public Transform _curlastContact;  //当前鼠标之前指向到的物体

    public Transform _clickTf;        //点击时效果坐标

    void Start()
    {
        _camera = GetComponent<Camera>();//获取场景中摄像机对象的组件接口
      
    }

    void Update()
    {

    }

    void LateUpdate()
    {
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);    //定义一条射线，这条射线从摄像机屏幕射向鼠标所在位置
        RaycastHit hit;    //声明一个碰撞的点
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.blue);
            _clickTf.position = hit.point;
            if (Input.GetMouseButtonUp(0))
            {
                if (Mouse0ClickEvents.ContainsKey(hit.transform.tag))
                {
                    Mouse0ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                if (Mouse1ClickEvents.ContainsKey(hit.transform.tag))
                {
                    Mouse1ClickEvents[hit.transform.tag](hit.collider.transform, hit.point);
                }
            }

            if (_curlastContact == hit.collider.transform) //表示为同一个物体
            {
                if (MouseOverEvents.ContainsKey(hit.collider.tag))
                {
                    MouseOverEvents[hit.transform.tag](hit.collider.transform);
                }
            }
            else //表示为不同物体
            {
                if (MouseEnterEvents.ContainsKey(hit.transform.tag))
                {
                    MouseEnterEvents[hit.transform.tag](hit.collider.transform);
                }
                if (_curlastContact != null)
                {
                    if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                    {
                        MouseExitEvents[_curlastContact.tag](_curlastContact);
                    }
                }

            }


            _curlastContact = hit.collider.transform;
        }
        else
        {
            if (_curlastContact != null)
            {
                if (MouseExitEvents.ContainsKey(_curlastContact.tag))
                {
                    MouseExitEvents[_curlastContact.tag](_curlastContact);
                }
            }
            _curlastContact = null;
        }
    }


   


}
