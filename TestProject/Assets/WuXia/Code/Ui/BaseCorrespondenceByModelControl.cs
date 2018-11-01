using UnityEngine;
using System.Collections;
using TinyFrameWork;

public class BaseCorrespondenceByModelControl : MonoBehaviour
{

    public Transform Target;
    public Vector3 Wordpos = Vector3.zero;
    public Vector3 ScencePosVector3;
    public bool IsNeedModelBlockOut = false;
    protected void LateUpdate()
    {
        Wordpos = Target == null ? Wordpos : Target.position;

        if (IsAPointInACamera(StrategySceneControl.Instance.Cur3DMainCamera, Wordpos))
        {
            transform.localScale = Vector3.one;
            Vector2 tempPos = StrategySceneControl.Instance.Cur3DMainCamera.WorldToScreenPoint(Wordpos);
            //----------非空
            if (StrategySceneControl.Instance.Main2DCamera != null)
            {
                Vector3 temppos = StrategySceneControl.Instance.Main2DCamera.ScreenToWorldPoint(tempPos);
                temppos.z = 0f;
                ((RectTransform) (gameObject.transform)).position = temppos;
                ScencePosVector3 = tempPos;
            }
            else
            {
                return;
            }
        }
        else
        {
            transform.localScale = Vector3.zero;
        }

    }


    public bool IsAPointInACamera(Camera cam, Vector3 wordPos)
    {
        // 是否在视野内
        bool result1 = false;
        Vector3 posViewport = cam.WorldToViewportPoint(wordPos);
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
            Vector3 dir = Wordpos - cam.transform.position;
            RaycastHit[] hits;
            bool isBlock = false;
            hits = Physics.RaycastAll(Wordpos, -dir);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Debug.DrawLine(Wordpos, hit.point, Color.black);
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
