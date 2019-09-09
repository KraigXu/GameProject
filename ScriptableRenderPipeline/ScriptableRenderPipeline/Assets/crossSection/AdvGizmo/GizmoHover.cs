using UnityEngine;
using System.Collections;

public class GizmoHover : MonoBehaviour {

	public Color hovercolor;
	private Color original;
	private Renderer rend;
	private bool selected;
    private float t = 0;


	void Start(){

		rend = transform.GetComponent<Renderer>();
		original = rend.material.color;
	}

	void OnMouseEnter () {
	
		SetHovered();
	}

	void OnMouseExit () {
	
		if(!selected)
			SetOriginal();
	}

	void SetHovered(){

		rend.material.color = hovercolor;
	}

	void SetOriginal(){

		rend.material.color = original;
	}

	void OnMouseDown(){

		selected=true;
        if (Time.time - t < 0.3f)
        {
            SendMessageUpwards("ChangeMode");
            SetOriginal();
        }
        t = Time.time;
	}

	void Update(){

		if(selected && Input.GetMouseButtonUp(0)){
			SetOriginal();
			selected = false;
		}
	}

}
