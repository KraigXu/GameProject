using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using UnityEngine;  

public class StrategyTeamTipsWindow : UIWindowBase
{

    [SerializeField]
    private List<UiTeamTips> _tipses;


    protected override void InitWindowData()
    {
        this.ID = WindowID.StrategyTeamTipsWindow;

        windowData.windowType = UIWindowType.NormalLayer;
        windowData.closeModel = UIWindowCloseModel.Destory;
    }
    public override void InitWindowOnAwake()
    {

    }
    void Update()
    {

        if (GameStaticData.TeamRunDic.Count == 0)
        {
            for (int i = 0; i < _tipses.Count; i++)
            {
                _tipses[i].gameObject.SetActive(false);
            }

            return;
        }
        if (_tipses.Count>= GameStaticData.TeamRunDic.Count)
        {
            IEnumerator<TeamFixed> iEnumerator= GameStaticData.TeamRunDic.Values.GetEnumerator();

            for (int i = 0; i < _tipses.Count; i++)
            {
                if (i == 0)
                {
                    var item = iEnumerator.Current;
                    _tipses[i].Info = item;
                    Vector3 point = item.Transform.position  + new Vector3(0, 3, 0);
                    if (Define.IsAPointInACamera(Camera.main, point))
                    {
                        Vector2 tempPos = Camera.main.WorldToScreenPoint(point);
                        _tipses[i].RectTF.position = tempPos;
                        _tipses[i].RectTF.localScale = Vector3.one;
                    }
                    else
                    {
                        _tipses[i].RectTF.localScale = Vector3.zero;
                    }
                    _tipses[i].gameObject.SetActive(true);

                }
                else
                {
                    if (iEnumerator.MoveNext())
                    {
                        var item = iEnumerator.Current;
                        _tipses[i].Info = item;
                        Vector3 point = item.Transform.position + new Vector3(0, 3, 0);
                        if (Define.IsAPointInACamera(Camera.main, point))
                        {
                            Vector2 tempPos = Camera.main.WorldToScreenPoint(point);
                            _tipses[i].RectTF.position = tempPos;
                            _tipses[i].RectTF.localScale = Vector3.one;
                        }
                        else
                        {
                            _tipses[i].RectTF.localScale = Vector3.zero;
                        }
                        _tipses[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        _tipses[i].gameObject.SetActive(false);
                    }


                }

            }

        }
        else
        {

        }
    }
}


