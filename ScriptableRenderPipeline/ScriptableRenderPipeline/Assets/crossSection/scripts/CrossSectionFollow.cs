using UnityEngine;
using System.Collections;

public class CrossSectionFollow : MonoBehaviour {
    private static int m_referenceCount = 0;

    private static CrossSectionFollow m_instance;
    private Vector3 tempPos;
	private Quaternion tempRot;

    public bool followPosition = true;
    public bool followRotation = true;

    public static CrossSectionFollow Instance
    {
        get
        {
            return m_instance;
        }
    }

    void Awake()
    {
        m_referenceCount++;
        if (m_referenceCount > 1)
        {
            DestroyImmediate(this.gameObject);
            return;
        }

        m_instance = this;
        // Use this line if you need the object to persist across scenes
        //DontDestroyOnLoad(this.gameObject);
    }


    void Update () {
	
		if(tempPos!=transform.position || tempRot != transform.rotation){

			tempPos = transform.position;
			tempRot = transform.rotation;
			SetSection();
		}
	}

    void OnDisable() {

		//Shader.DisableKeyword("CLIP_PLANE");
	}

    void OnEnable()
    {
        //Shader.EnableKeyword("CLIP_PLANE");
        SetSection();
    }

    void OnDestroy()
    {
        m_referenceCount--;
        if (m_referenceCount == 0)
        {
            m_instance = null;
        }

    }

    void OnApplicationQuit()
    {
        //Shader.DisableKeyword("CLIP_PLANE");
    }

    void SetSection(){

        if (followPosition) Shader.SetGlobalVector("_SectionPoint", transform.position);
        if (followRotation)
        {
            Shader.SetGlobalVector("_SectionPlane", transform.forward);
            Shader.SetGlobalVector("_SectionPlane2", transform.right);
        }
	}

}