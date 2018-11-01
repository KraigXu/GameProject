using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 主相机
/// </summary>
public class StrategyMainCamera : MonoBehaviour
{
    public LayerMask IgonreLayer;

    public bool _isManual=true; //是否手动 
    private Camera _camera;
    private Transform _cameraTf;
    public Bounds _visibleRangeBounds;       //可视范围
    public Vector3 _target;
    public Transform _targetTf;
    public float _targetDistance=50f;
    public Vector3 _lastMousePosition;
    public float _originalNearPlane=0.3f;
    public float _originalFarPlane=1000f;
    public float _rotateSpeed = 3;
    public float _moveSpeed = 3;
    public float _zoomSpeed = 500f;
    public float _damping=3f;
    public bool _isHold = false;

    void Start ()
	{
	    _camera = gameObject.GetComponent<Camera>();
	    _cameraTf = gameObject.transform;
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.DrawSphere(_target, 5f);

        Gizmos.color=new Color(0f,1f,0f,0.5f);
        Gizmos.DrawCube(_visibleRangeBounds.center,_visibleRangeBounds.size);
    }

    private void LateUpdate()
    {
        if (_visibleRangeBounds.size == Vector3.zero)
        {
            return;
        }

        if (_targetTf != null)
        {
            _target = _targetTf.position;
        }

        if (Input.GetMouseButton(0))
        {
            _isManual = true;
        }
        else
        {
            _isManual = false;
        }


        if (_isManual)              //手动时以目标为中心
        {
            if (Input.GetMouseButton(1) )
            {
                if (_lastMousePosition != Vector3.zero)
                {
                    Vector2 offset = Input.mousePosition - _lastMousePosition;
                    _cameraTf.RotateAround(_target, Vector3.up, offset.x * Time.deltaTime * _rotateSpeed);
                    _cameraTf.RotateAround(_target, _cameraTf.right, -offset.y * Time.deltaTime * _rotateSpeed);
                }
                 _lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            { 
                _lastMousePosition = Vector3.zero;
            }

            if (Input.GetMouseButton(2))
            {
                if (_lastMousePosition != Vector3.zero)
                {
                   // float moveDistance = Time.deltaTime * -panSpeed * distance / 20f;
                    Vector3 offset = Input.mousePosition - _lastMousePosition;
                    _cameraTf.Translate(new Vector3(offset.x*Time.deltaTime*_moveSpeed,offset.y*Time.deltaTime*_moveSpeed,offset.z*Time.deltaTime*_moveSpeed));
                }
                _lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(2))
            {
                _lastMousePosition=Vector3.zero;
            }

            if (Input.GetAxis("Mouse ScrollWheel")!=0f)
            {
                float moveValue = Input.GetAxis("Mouse ScrollWheel");
                float moveDistance = moveValue * Time.deltaTime * _zoomSpeed;
                _cameraTf.Translate(_cameraTf.forward*moveDistance);
            }

            if (Input.GetKey(KeyCode.W))
            {
                Vector3 offset=new Vector3(0,0,Time.deltaTime*_moveSpeed);
                _cameraTf.Translate(offset, Space.World);
            }

            if (Input.GetKey(KeyCode.S))
            {
                Vector3 offset = new Vector3(0, 0, -Time.deltaTime * _moveSpeed);
                _cameraTf.Translate(offset, Space.World);
            }

            if (Input.GetKey(KeyCode.A))
            {
                Vector3 offset = new Vector3(-Time.deltaTime * _moveSpeed, 0,0);
                _cameraTf.Translate(offset, Space.World);
            }

            if (Input.GetKey(KeyCode.D))
            {
                Vector3 offset = new Vector3(Time.deltaTime * _moveSpeed, 0, 0);
                _cameraTf.Translate(offset, Space.World);
            }

        }
        else                    //跟随目标
        {
            Quaternion newrotation = Quaternion.Euler(60f,30f, 0);
            Vector3 newposition = newrotation * new Vector3(0, 0, -50)+ _target;
            _cameraTf.rotation = Quaternion.Lerp(_cameraTf.rotation, newrotation, Time.deltaTime * _damping);
            _cameraTf.position = Vector3.Lerp(_cameraTf.position, newposition, Time.deltaTime * _damping);
        }
        //ChangeCamerStatus
        _camera.nearClipPlane = _originalNearPlane;
        _camera.farClipPlane = _originalFarPlane;
    }

    /// <summary>
    /// 设置可见范围包围盒
    /// </summary>
    public void SetVisibleRangeBounds(Bounds bounds)
    {
        _visibleRangeBounds = bounds;
    }

    public void SetTarget(Transform target)
    {
        _targetTf = target;
    }
}
