using UnityEngine;
using System.Collections;

public class PlaneHover : MonoBehaviour {

	public Color hovercolor;
	private Color original;
	private Renderer rend;
	private bool selected;


	void Start(){

		rend = transform.GetComponent<Renderer>();
		original = rend.material.color;
	}

	void OnMouseEnter () {

        //GetComponent<Renderer>().enabled = true;
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

	void Update(){

		if(selected && Input.GetMouseButtonUp(0)){
			SetOriginal();
			selected = false;
		}
	}

}
