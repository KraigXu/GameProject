using UnityEngine;
using System.Collections;
using TinyFrameWork;

public class BaseCorrespondenceByModelControl : MonoBehaviour
{
    public bool IsNeedModelBlockOut = false;
    private Vector3 _wordpos = Vector3.zero;
    private Camera _camera3D;
    private Camera _camera2D;
    private bool _isInit;

    protected void LateUpdate()
    {
        if (_isInit == false) return;
        if (IsAPointInACamera(_camera3D, _wordpos))
        {
            transform.localScale = Vector3.one;
            Vector2 tempPos = _camera3D.WorldToScreenPoint(_wordpos);
            Vector3 temppos = _camera2D.ScreenToWorldPoint(tempPos);
            temppos.z = 0f;
            ((RectTransform)(gameObject.transform)).position = temppos;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }


    }

    public virtual void Init(Camera camera3D, Camera camera2D, Vector3 target)
    {
        _camera2D = camera2D;
        _camera3D = camera3D;
        _wordpos = target;
        _isInit = true;
    }



    private bool IsAPointInACamera(Camera cam, Vector3 wordPos)
    {
        // 是否在视野内
        bool result1 = false;
        Vector3 posViewport = cam.WorldToViewportPoint(_wordpos);
        Rect rect = new Rect(0, 0, 1, 1);
        result1 = rect.Contains(posViewport);
        // 是否在远近平面内
        bool result2 = false;
        if (posViewport.z >= cam.nearClipPlane && posViewport.z <= cam.farClipPlane)
        {
            result2 = true;
        }
        // 综合判断
        bool result = result1 && result2;
        if (IsNeedModelBlockOut)
        {
            Vector3 dir = _wordpos - cam.transform.position;
            RaycastHit[] hits;
            bool isBlock = false;
            hits = Physics.RaycastAll(_wordpos, -dir);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Debug.DrawLine(_wordpos, hit.point, Color.black);
                if (hit.transform.tag == "Terrain")
                {
                    isBlock = true;
                    break;
                }
            }
            return result && !isBlock;
        }
        else
            return result;
    }
}
