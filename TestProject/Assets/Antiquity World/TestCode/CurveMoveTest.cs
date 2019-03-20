using System.Collections;
using System.Collections.Generic;
using MapMagic;
using UnityEngine;

public class CurveMoveTest : MonoBehaviour
{

    [SerializeField]
    private AnimationCurve X = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0,0),
        new Keyframe(1,-1),
        new Keyframe(1.5f,-2),
        new Keyframe(2.5f,0), 
        new Keyframe(3,1),
        new Keyframe(4,2), 
        new Keyframe(5,0), 
    });
    private AnimationCurve Y = new AnimationCurve();
    [SerializeField]
    private AnimationCurve Z = new AnimationCurve(new Keyframe[]
    {
        new Keyframe(0,0),
        new Keyframe(1,1),
        new Keyframe(2,1.5f), 
        new Keyframe(2.5f,2),
        new Keyframe(3f,-1), 
        new Keyframe(3.5f,-1.5f),
        new Keyframe(5,-2),
    });
    public float time=0.0f;

    

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time <= 1)
        {
            time += Time.deltaTime;
            float x = X.Evaluate(time);
            float y = Y.Evaluate(time);
            float z = Z.Evaluate(time);
            transform.position = transform.position + new Vector3(x*Time.deltaTime, y * Time.deltaTime, z * Time.deltaTime);
        }


    }
}
