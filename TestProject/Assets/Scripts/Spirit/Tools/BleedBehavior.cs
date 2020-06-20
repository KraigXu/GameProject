using System;
using UnityEngine;

public class BleedBehavior : MonoBehaviour
{
    public float BloodAmount = 0; //0-1 //Set this at runtime
    public float interval = 1f;
    public float EdgeSharpness = 1; //>=1 //defines how sharp the resulting alpha map will be
    public bool startFadeOut = false; //automatically fades out the blood effect (by lowering the BloodAmount value over time)
    public float autoFadeOutRelReduc = 0.5f; //relative reduction per seconde

    public Action FadeInFinished;
    public Action FadeOutFinished;
    [SerializeField]
    private Material _material;

     void Start()
     {
         _material = Resources.Load<Material>("Materials/BlendEffect");
     }

    public void Update()
    {
        if (startFadeOut)
        {
            BloodAmount -= Time.deltaTime / interval;
            if (BloodAmount <= 0)
            {
                startFadeOut = true;
                BloodAmount = 1;
                GameObject.Destroy(this);
            }
        }
        else
        {
            BloodAmount += Time.deltaTime / interval;
            if (BloodAmount >= 1)
            {
                startFadeOut = false;
                BloodAmount = 0;
                GameObject.Destroy(this);
            }
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        _material.SetFloat("_BlendAmount", BloodAmount);
        _material.SetFloat("_EdgeSharpness", EdgeSharpness);

        Graphics.Blit(source, destination, _material);
    }

    public void FadeOut()
    {
        startFadeOut = true;
        BloodAmount = 1;
    }

    public void FadeIn()
    {
        startFadeOut = false;
        BloodAmount = 0;
    }
}