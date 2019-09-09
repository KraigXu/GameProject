using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CrossSectionObjectSetup : MonoBehaviour
{
    public Color sectionColor = Color.red;

    private List<Material> matList;
    private List<Material> clipMatList;
    private Renderer[] renderers;
    private Dictionary<Renderer, int[]> matDict;
    	
    public bool accurateBounds = true;

    [HideInInspector]
    public Bounds bounds;


    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        makeSectionMaterials();
        
        Planar_xyzClippingSection pxyz = (Planar_xyzClippingSection)Object.FindObjectOfType(typeof (Planar_xyzClippingSection));
        // this can freeze the app - in case of high poly meshes - for a moment we need it for pxyz object only
        if(pxyz) calculateBounds();
    }

	void Start () {	
        
	}


    void makeSectionMaterials()
    {
        matList = new List<Material>();
        clipMatList = new List<Material>();
        matDict = new Dictionary<Renderer, int []>();
        foreach (Renderer rend in renderers) {
            Material[] mats = rend.sharedMaterials;
            int[] idx = new int[mats.Length];
            for(int j = 0; j < mats.Length; j++) {
                int i = matList.IndexOf(mats[j]);
                if (i == -1)
                {
                    matList.Add(mats[j]);
                    i = matList.Count - 1;
                }
                idx[j] = i;
            }
            matDict.Add(rend, idx);
        }
        foreach (Material mat in matList)
        {
            string shaderName = mat.shader.name;
            Debug.Log(shaderName);
            if (shaderName.Length > 13)
            {
                if (shaderName.Substring(0, 13) == "CrossSection/")
                {
                    clipMatList.Add(mat);
                    continue;
                }
            }
            Material substitute = new Material(mat);
            //substitute.name = "subst_" + substitute.name;
            shaderName = shaderName.Replace("Legacy Shaders/", "").Replace("(", "").Replace(")", "");
            Shader replacementShader = null;

            if (replacementShader == null) replacementShader = Shader.Find("CrossSection/" + shaderName);
            if (replacementShader == null)
            {
                if (shaderName.Contains("Transparent/VertexLit"))
                {
                    replacementShader = Shader.Find("CrossSection/Transparent/Specular");
                }
                else if (shaderName.Contains("Transparent"))
                {
                    replacementShader = Shader.Find("CrossSection/Transparent/Diffuse");
                }
                else
                {
                    replacementShader = Shader.Find("CrossSection/Diffuse");
                }
            }
            substitute.shader = replacementShader;
            substitute.SetColor("_SectionColor", sectionColor);

            clipMatList.Add(substitute);
        }
        foreach (Renderer rend in renderers)
        {
            int[] idx = matDict[rend];
            Material[] mats = new Material[idx.Length];
            for (int i = 0; i < idx.Length; i++)
            {
                mats[i] = clipMatList[idx[i]];
            }
            rend.materials = mats;
        }
    }


	void calculateBounds() {
        if (accurateBounds)
        {
            bounds = calculateMeshBounds();
        }
        else
        {
            bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
                /*          This gives the accurate results only when the objects are not rotated or rotated by multiplication of 90 degrees.
                            A general way to get accurate results would be to iterate through all the mesh points, and find their positions range in the world space.
                            But this can take long in case of complex meshes*/
            }
        }
	}

    Bounds calculateMeshBounds()
    {
        Bounds accurateBounds = new Bounds();
        MeshFilter[] meshes = GetComponentsInChildren<MeshFilter>();
        for (int i = 0; i < meshes.Length; i++)
        {
            Mesh ms = meshes[i].mesh;
            int vc = ms.vertexCount;
            for (int j = 0; j < vc; j++)
            {
                if (i == 0 && j == 0)
                {
                    accurateBounds = new Bounds(meshes[i].transform.TransformPoint(ms.vertices[j]), Vector3.zero);
                }
                else
                {
                    accurateBounds.Encapsulate(meshes[i].transform.TransformPoint(ms.vertices[j]));
                }
            }
        }
        return accurateBounds;
    }

    void OnApplicationQuit()
    {
        Shader.DisableKeyword("CLIP_PLANE");
    }

    public Bounds GetBounds() 
    {
        return bounds;
    }

}
