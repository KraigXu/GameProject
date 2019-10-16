using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
public enum AnchorType
{
    LeftTop,
    LeftCenter,
    LeftBottom,
    MidTop,
    MidCenter,
    MidBottom,
    RightTop,
    RightCenter,
    RightBottom
}
public class UICommonManager : MonoBehaviour {
    public static UICommonManager Instance { get; private set; } 

    private SpriteAtlas _atlas;
    private Canvas _canvas;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
#if UNITY_WEBGL
        _atlas = Resources.Load("Atlas/EngineWebgl") as SpriteAtlas;
#else
         _atlas = Resources.Load("Atlas/Ss_Atlas") as SpriteAtlas;
#endif

        _canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }


    /// <summary>
    /// 获取图片图集
    /// </summary>
    /// <param name="spName"></param>
    /// <returns></returns>
    public Sprite GetSprite(string spName)
    {
        return _atlas.GetSprite(spName);
    }

    /// <summary>
    /// 更换图片
    /// </summary>
    /// <param name="go"></param>
    /// <param name="spName"></param>
    public void SetSprite(GameObject go,string spName)
    {
        var sp = GetSprite(spName);
        var image = go.GetComponent<Image>();
        image.sprite = sp;
        //image.SetNativeSize();
    }

    public void SetSprite(Button btn, string spName)
    {
        var sp = GetSprite(spName);
        var image = btn.GetComponent<Image>();
        image.sprite = sp;
    }

    /// <summary>
    /// 更换图片(并设置原始大小)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="spName"></param>
    public void SetSpriteWithNativeSize(GameObject go, string spName)
    {
        var sp = GetSprite(spName);
        var image = go.GetComponent<Image>();
        image.sprite = sp;
        image.SetNativeSize();
    }

    public void SetBehaviourEnable<T>(T t,bool isEnable) where  T : Behaviour
    {
        t.enabled = isEnable;
    }

    /// <summary>
    /// 更换字体颜色(子物体)
    /// </summary>
    /// <param name="go"></param>
    /// <param name="col"></param>
    public void SetTextColorInChild(GameObject go, Color col)
    {
        go.GetComponentInChildren<Text>().color = col;
    }

    /// <summary>
    /// 设置字体
    /// </summary>
    /// <param name="go"></param>
    /// <param name="text"></param>
    public void SetText(GameObject go, string text)
    {
        var tt = go.GetComponent<Text>();
        tt.text = text;
    }

    /// <summary>
    /// 添加子物体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefab"></param>
    /// <returns></returns>
    public GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        if (go == null || parent == null) return go;
        var t = go.transform;
        t.SetParent(parent.transform);
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = Vector3.one;
        go.layer = parent.layer;
        return go;
    }

    public void ShowUi(GameObject go, bool isShow)
    {
        go.SetActive(isShow);
    }

    /// <summary>
    /// 设置左侧列表初始位置
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="parRect"></param>
    public void SetLeftListAnchorByEdge(RectTransform rect, RectTransform parRect)
    {
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -rect.sizeDelta.x, rect.sizeDelta.x);
        rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, Screen.height / _canvas.scaleFactor);
    }

    public void SetAnchorByEdge(RectTransform rect, AnchorType ancType)
    {
        switch (ancType)
        {
            case AnchorType.LeftTop:
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left,0f, 0f);
                rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0f, 0f);
                break;
            case AnchorType.LeftCenter:
                break;
            case AnchorType.LeftBottom:
                break;
            case AnchorType.MidTop:
                break;
            case AnchorType.MidCenter:
                break;
            case AnchorType.MidBottom:
                break;
            case AnchorType.RightTop:
                break;
            case AnchorType.RightCenter:
                break;
            case AnchorType.RightBottom:
                break;
        }
    }


}
