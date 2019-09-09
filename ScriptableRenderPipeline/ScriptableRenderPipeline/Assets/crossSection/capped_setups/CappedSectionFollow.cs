//The purpose of this script is to setup and control the properties of materials on the model GameObject 

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class CappedSectionFollow : MonoBehaviour {
    public GameObject model;
    private enum Mode { box, corner };
    private Mode sectionMode;

    private Vector3 tempPos;
	private Quaternion tempRot;

    public bool followPosition = true;
    //public bool followRotation = true;

    private List<Material> matList;
    private List<Material> sectionMaterials;
    private Dictionary<Renderer, int[]> matDict;

    //public Material[] materials;

    void Awake()
    {
        sectionMaterials = new List<Material>();
        if (gameObject.GetComponent<CappedSectionBox>()) sectionMode = Mode.box;
        if (gameObject.GetComponent<CappedSectionCorner>()) sectionMode = Mode.corner;
        if (model) makeSectionMaterials();
    }
    void Start()
    {
        foreach (Material m in sectionMaterials)
        {
            if (m.HasProperty("_SectionDirX")) m.SetVector("_SectionDirX", transform.right);
            if (m.HasProperty("_SectionDirY")) m.SetVector("_SectionDirY", transform.up);
            if (m.HasProperty("_SectionDirZ")) m.SetVector("_SectionDirZ", transform.forward);
        }
        Shader.SetGlobalVector("_SectionDirX", transform.right);
        Shader.SetGlobalVector("_SectionDirY", transform.up);
        Shader.SetGlobalVector("_SectionDirZ", transform.forward);
        Shader.SetGlobalColor("_SectionColor", Color.black);
        SetSection();
    }

    void Update () {
	
		if(tempPos!=transform.position || tempRot != transform.rotation){

			tempPos = transform.position;
			tempRot = transform.rotation;
			SetSection();
		}
	}

    void makeSectionMaterials()
    {
        Renderer[] renderers = model.GetComponentsInChildren<Renderer>();
        matList = new List<Material>();
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
                if (((sectionMode == Mode.box)&&(shaderName.Substring(0, 13) == "Clipping/Box/"))||((sectionMode == Mode.corner) && (shaderName.Substring(0, 9) == "Clipping/")))
                {
                    sectionMaterials.Add(mat);
                    continue;
                }
            }
            Material substitute = new Material(mat);
            Shader replacementShader = null;

            if (sectionMode == Mode.box) replacementShader = Shader.Find("Clipping/Box/" + shaderName);
            if (sectionMode == Mode.corner) replacementShader = Shader.Find("Clipping/Corner/" + shaderName);

            if (replacementShader == null)
            {
                if (sectionMode == Mode.box) replacementShader = Shader.Find("Clipping/Box/Standard");
                if (sectionMode == Mode.corner) replacementShader = Shader.Find("Clipping/Corner/Standard");
            }

            substitute.shader = replacementShader;

            sectionMaterials.Add(substitute);
        }
        foreach (Renderer rend in renderers)
        {
            int[] idx = matDict[rend];
            Material[] mats = new Material[idx.Length];
            for (int i = 0; i < idx.Length; i++)
            {
                mats[i] = sectionMaterials[idx[i]];
            }
            rend.materials = mats;
        }
    }

    void OnDisable() {

        Shader.DisableKeyword("CLIP_BOX");
        Shader.DisableKeyword("CLIP_CORNER");
	}

    void OnEnable()
    {
        if (sectionMode == Mode.box)  Shader.EnableKeyword("CLIP_BOX");
        if (sectionMode == Mode.corner) Shader.EnableKeyword("CLIP_CORNER");
        SetSection();
    }


    void OnApplicationQuit()
    {
        Shader.DisableKeyword("CLIP_BOX");
        Shader.DisableKeyword("CLIP_CORNER");
    }

    void SetSection()
    {
        if (followPosition)
        {
            foreach (Material m in sectionMaterials)
            {
                if (m.HasProperty("_SectionCentre")) m.SetVector("_SectionCentre", transform.position);
                if (m.HasProperty("_SectionScale")) m.SetVector("_SectionScale", transform.localScale);
            }
            Shader.SetGlobalVector("_SectionCentre", transform.position);
            Shader.SetGlobalVector("_SectionScale", transform.localScale);
        }
	}

}