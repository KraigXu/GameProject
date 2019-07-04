using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
public delegate void CellFeatureEvent(Entity entity);

public class UiCellFeature : MonoBehaviour
{
    private CellFeatureEvent _event;
    private Text _lableTxt;
    private Button _featureBtn;
    private Image _featureImg;
    private Entity _entity;

    void Awake()
    {
        _lableTxt = transform.Find("Lable").GetComponent<Text>();
        _featureBtn = transform.Find("Feature").GetComponent<Button>();
        _featureImg = transform.Find("FeatureAtr").GetComponent<Image>();

        _featureBtn.onClick.AddListener(FeatureBtnClick);
    }

    void FeatureBtnClick()
    {
        if (_event == null) return;

        _event(_entity);

    }

    public void Init(string lable, Sprite featureSprite,Entity entity, CellFeatureEvent callback)
    {
        _lableTxt.text = lable;
        _featureImg.sprite = featureSprite;
        _entity = entity;
        _event = callback;

    }

}
