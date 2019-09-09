using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CylinderSectionExample : MonoBehaviour {

    public Vector3[] hitPoints;
    public Vector3[] AxisDir;
    public float[] radiuses;
    [Range(0, 5)]
    public int n = 5;

    private int i = 0;


	void Start () {
        hitPoints = new Vector3[n];
        AxisDir = new Vector3[n];
        radiuses = new float[n];
       // Shader.DisableKeyword("CLIP_PLANE");
        //Shader.DisableKeyword("CLIP_TWO_PLANES");
       // Shader.DisableKeyword("CLIP_SPHERE");
        Shader.SetGlobalInt("_hitCount",0);
        //we have declared: "material.EnableKeyword("CLIP_PLANE");" on all the crossSectionStandard derived materials - in the CrossSectionStdShaderGUI editor script - so we have to switch it off
        Renderer[] allrenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in allrenderers)
        {
            Material[] mats = r.sharedMaterials;
            foreach (Material m in mats) if (m.shader.name.Substring(0, 13) == "CrossSection/") m.DisableKeyword("CLIP_PLANE");
        }
    }
	
	void Update () {
        
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
                    Shader.EnableKeyword("CLIP_TUBES");
                    //Shader.SetGlobalInt("_hitCount", 0);

                    hitPoints[i % n] = hit.point;
                    AxisDir[i % n] = (hit.point - Camera.main.transform.position).normalized;
                    radiuses[i % n] = 0.1f;

                    Shader.SetGlobalVector("_hitPoint" + (i % n).ToString(), hitPoints[i % n]);
                    Shader.SetGlobalVector("_AxisDir" + (i % n).ToString(), AxisDir[i % n]);
                    Shader.SetGlobalFloat("_Rad" + (i % n).ToString(), radiuses[i % n]);

                    /*Shader.SetGlobalVectorArray("_hitPoints", hitPoints);
                    Shader.SetGlobalVectorArray("_AxisDirs", AxisDir);
                    Shader.SetGlobalFloatArray("_Radiuses", radiuses);*/

                    //Shader.SetGlobalVector("_SectionPoint", hit.point);
                    //Shader.SetGlobalVector("_AxisDir", (hit.point - Camera.main.transform.position).normalized);
                    //Shader.SetGlobalFloat("_Radius", 0.1f);

                    i++;
                    Shader.SetGlobalInt("_hitCount", (int)Mathf.Min(i, n));
                    StartCoroutine(drag());
                }
            }
        }
	}

    void OnEnable()
    {
        Shader.EnableKeyword("CLIP_TUBES");
        //Shader.EnableKeyword("CLIP_PLANE");
        //Shader.EnableKeyword("CLIP_PLANE");
    }

    void OnDisable()
    {
        Shader.DisableKeyword("CLIP_TUBES");
        Shader.SetGlobalInt("_hitCount", 0);
       // Shader.DisableKeyword("CLIP_PLANE");
    }

    void OnApplicationQuit()
    {
        //disable clipping so we could see the materials and objects in editor properly
        Shader.DisableKeyword("CLIP_TUBES");

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
            if (m > 0.1f)
            {
                //Shader.SetGlobalFloat("_Radius", m);
                radiuses[(i-1) % n] = m;
                //Shader.SetGlobalFloatArray("_Radiuses", radiuses);
                Shader.SetGlobalFloat("_Rad" + ((i-1) % n).ToString(), radiuses[(i-1) % n]);
            }
            yield return null;
        }
        Camera.main.GetComponent<maxCamera>().enabled = true;
        
    }
}
