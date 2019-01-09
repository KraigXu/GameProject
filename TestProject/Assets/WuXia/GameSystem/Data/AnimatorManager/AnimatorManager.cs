using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;



public class AnimatorManager : MonoBehaviour
{

    public Animator Animator;
    public Avatar Avatar;
    public AnimatorController AnimatorController;

	// Use this for initialization
	void Start ()
	{

	    Animator.avatar = Avatar;
	    Animator.runtimeAnimatorController = AnimatorController;
	    
        Debug.Log(Animator.parameters.Length);
      

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 200, 30), "播放"))
        {
            Animator.SetBool("OnGround",false);
        }
    }
}
