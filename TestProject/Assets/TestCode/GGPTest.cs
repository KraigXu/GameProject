using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GGPTest : MonoBehaviour
{

    public List<Renderer> Renderers;

    public Bounds Bounds;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
	    if (Renderers.Count > 0)
	    {
	        Bounds = Renderers[0].bounds;

	        for (int i = 1; i <Renderers.Count; i++)
	        {
	            Bounds.Encapsulate(Renderers[i].bounds);

	        }
	    }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.3f, 1f, 0.3f);
        Gizmos.DrawCube(Bounds.center, Bounds.size);

    }

    void OnGUI()
    {
        
        Vector3 point2D = Camera.main.WorldToScreenPoint(Bounds.min);

        GUI.TextField(new Rect(0, 0, 10, 10), "MM");

        GUI.TextField(new Rect(point2D.x,Screen.height-point2D.y, 20, 10), "这是最小点");

    }



}
