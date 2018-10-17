using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;
/// <summary>
/// 主相机控制
/// </summary>
public class MainCameraControl : MonoBehaviour
{

    public static MainCameraControl Instance
    {
        get { return _instance; }
    }
    private static MainCameraControl _instance;

    public Camera Camera;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        this.Camera = GetComponent<Camera>();
    }



    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UICenterMasterManager.Instance.ShowWindow(WindowID.ExtendedMenuWindow);
        }
    }
}
