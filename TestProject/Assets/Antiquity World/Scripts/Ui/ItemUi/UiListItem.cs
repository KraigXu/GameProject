using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 只显示名字的按钮
/// 包含点击时间
/// </summary>
public class UiListItem : MonoBehaviour
{
    public delegate void OnClick(int id);
    public Text Text;
    public int Id;
    public OnClick ClickCallback;
    public Button Button;

    void Start()
    {
        Button.onClick.AddListener(delegate ()
        {
            if (ClickCallback != null)
            {
                ClickCallback(Id);
            }
        });
    }

    public void Destroy()
    {
        WXPoolManager.Pools[Define.GeneratedPool].Despawn(transform);
    }


}
