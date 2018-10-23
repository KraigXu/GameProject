using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class CacheBoxControl : MonoBehaviour
{

    public RectTransform image;
    private Sequence _sequence;
	// Use this for initialization
	void Start () {
        _sequence = DOTween.Sequence();
        _sequence.Append(image.transform.DORotate(new Vector3(0, 0, 180), 2));
        _sequence.Append(image.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 1));
        _sequence.Append(image.transform.DORotate(new Vector3(0, 0, 360), 2));
        _sequence.Append(image.transform.DOScale(new Vector3(1f, 1f, 1f), 1));
        _sequence.SetLoops(-1);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
