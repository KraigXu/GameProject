using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UiTeamTips : MonoBehaviour
{
    public RectTransform RectTF;

    public TeamFixed Info
    {
        get { return _info; }
        set
        {
            _info = value;
            Name.text = value.Name+value.Members.Count;

        }

    }

    public Text Name;
    private TeamFixed _info;

}
