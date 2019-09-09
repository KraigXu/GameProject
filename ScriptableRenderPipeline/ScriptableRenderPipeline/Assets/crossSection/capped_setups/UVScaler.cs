using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UVScaler : MonoBehaviour {
    private Mesh m;
    private Vector2[] muv;
	// Use this for initialization
	void Start () {
        m = GetComponent<MeshFilter>().sharedMesh;
        muv = m.uv;
        SetUV();
	}
	
	// Update is called once per frame
	void Update () {

	}
    public void SetUV()
    {
        muv[0] = new Vector2(0, 0);
        muv[2] = new Vector2(transform.right.magnitude * transform.lossyScale.x, 0);
        muv[3] = new Vector2(0, transform.up.magnitude * transform.lossyScale.y);
        muv[1] = new Vector2(transform.right.magnitude * transform.lossyScale.x, transform.up.magnitude * transform.lossyScale.y);
        m.uv = muv;
    }
}
