//The purpose of this script is to manipulate the scale and position of the capped section corner gizmo object 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CappedSectionCorner
    : MonoBehaviour {

    public int layer=13;
    [Space(10)]
    public Collider xAxis;
    [Space(10)]
    public Collider yAxis;
    [Space(10)]
    public Collider zAxis;

    private enum GizmoAxis { X, Y, Z, XYRotate, XZRotate, YZRotate, none};
	private GizmoAxis selectedAxis;

	private RaycastHit hit; 
	private Ray ray, ray1;
	private Plane dragplane;
    private float rayDistance, newRotY, rayDistancePrev, distance;
	private Vector3 lookCamera, startDrag, startPos, startDragRot, lookHitPoint, startScale;
	private bool dragging;

    private float scaleM = 1;
	// Use this for initialization
	void Start () {
        layer = 1 << layer;
        scaleM = Vector3.Magnitude(transform.localScale);
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetMouseButtonDown(0)){

			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			dragplane = new Plane();

			if (Physics.Raycast(ray, out hit,1000f, layer)){
	                
				if (hit.collider == xAxis){
					selectedAxis = GizmoAxis.X;
					dragplane.SetNormalAndPosition(transform.up, transform.position);
				}
                else if (hit.collider == yAxis)
                {
                    selectedAxis = GizmoAxis.Y;
                    dragplane.SetNormalAndPosition(transform.forward, transform.position);
                }
				else if (hit.collider == zAxis){
					selectedAxis = GizmoAxis.Z;	
					dragplane.SetNormalAndPosition(transform.up, transform.position);
                }
                else
                {
                    Debug.Log(hit.collider.name);
                    return;
                }
                distance = hit.distance;
                startDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
                startPos = transform.position;
                startScale = transform.localScale;
                dragging = true;
            }
        }

        if (dragging)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 onDrag = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance));
            Vector3 translation = onDrag - startDrag;
            Vector3 projectedTranslation = Vector3.zero;

            if (dragging)
            {
                float lsx = startScale.x;
                float lsy = startScale.y;
                float lsz = startScale.z;
                
                switch (selectedAxis)
                {
                     case GizmoAxis.X:
                        {
                            projectedTranslation = Vector3.Project(translation, transform.right);
                            transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                            lsx -= translation.magnitude * Mathf.Sign(Vector3.Dot(projectedTranslation, transform.right));
                            break;
                        }
                    case GizmoAxis.Y:
                        {
                            projectedTranslation = Vector3.Project(translation, transform.up);
                            transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                            lsy -= translation.magnitude * Mathf.Sign(Vector3.Dot(projectedTranslation, transform.up));
                            break;
                        }
                    case GizmoAxis.Z:
                        {
                            projectedTranslation = Vector3.Project(translation, transform.forward);
                            transform.position = startPos + projectedTranslation.normalized * translation.magnitude;
                            lsz -= translation.magnitude * Mathf.Sign(Vector3.Dot(projectedTranslation, transform.forward));
                            break;
                        }
                }

                transform.localScale = new Vector3(Mathf.Clamp(lsx, 0.1f, Mathf.Infinity), Mathf.Clamp(lsy, 0.1f, Mathf.Infinity), Mathf.Clamp(lsz, 0.1f, Mathf.Infinity));

                
                foreach (UVScaler uvs in gameObject.GetComponentsInChildren<UVScaler>()) uvs.SetUV();
                foreach (ScaleToColor uvs in gameObject.GetComponentsInChildren<ScaleToColor>()) uvs.SetColor(Vector3.Magnitude(transform.localScale)/scaleM);
            }

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
            }
        }
	}

    public void HideCage(bool val)
    {
        foreach (PlaneHover ph in gameObject.GetComponentsInChildren<PlaneHover>()) ph.GetComponent<Renderer>().enabled = val;
    }
    public void HideCaps(bool val)
    {
        foreach (UVScaler uvs in gameObject.GetComponentsInChildren<UVScaler>()) uvs.GetComponent<Renderer>().enabled = val;
    }
}
