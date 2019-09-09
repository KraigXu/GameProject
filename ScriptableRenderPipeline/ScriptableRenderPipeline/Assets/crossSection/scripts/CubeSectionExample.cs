using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CubeSectionExample : MonoBehaviour {

	void Start () {

        Shader.DisableKeyword("CLIP_PLANE");
        Shader.DisableKeyword("CLIP_CUBE");
        Renderer[] allrenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allrenderers)
        {
            Material[] mats = r.sharedMaterials;
            foreach (Material m in mats) if (m.shader.name.Substring(0, 13) == "CrossSection/") m.DisableKeyword("CLIP_PLANE");
        }
    }
	
	void Update () {
        //Shader.SetGlobalFloat("_Radius", 0.2f);
        //return;
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 10000f))
            {
                if (hit.transform.IsChildOf(transform))
                {
                    Debug.Log("hit");
                    Shader.EnableKeyword("CLIP_CUBE");
                    Shader.SetGlobalVector("_SectionPoint", hit.point);
                    Shader.SetGlobalVector("_SectionPlane", hit.normal);
                    Shader.SetGlobalVector("_SectionPlane2", Vector3.Cross(hit.transform.up, hit.normal).normalized);
                    Shader.SetGlobalFloat("_Radius", 0.05f);
                    StartCoroutine(drag());
                }
            }
        }
	}

    void OnEnable()
    {
        Shader.EnableKeyword("CLIP_CUBE");
        //Shader.EnableKeyword("CLIP_PLANE");
    }

    void OnDisable()
    {
        Shader.DisableKeyword("CLIP_CUBE");
        //Shader.DisableKeyword("CLIP_PLANE");
    }

    void OnApplicationQuit()
    {
        //disable clipping so we could see the materials and objects in editor properly
        Shader.DisableKeyword("CLIP_CUBE");

    }


    IEnumerator drag()
    {
        float cameraDistance = Vector3.Distance(transform.position, Camera.main.transform.position);
        Vector3 startPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance));
        Vector3 translation = Vector3.zero;
        Camera.main.GetComponent<maxCamera>().enabled = false;
        while (Input.GetMouseButton(0))
        {
            translation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance)) - startPoint;
            float m = translation.magnitude;
            if(m>0.05f) Shader.SetGlobalFloat("_Radius", m);
            yield return null;
        }
        Camera.main.GetComponent<maxCamera>().enabled = true;
        
    }
}
