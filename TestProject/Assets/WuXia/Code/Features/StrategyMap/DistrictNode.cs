using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;

/// <summary>
/// 区节点
/// </summary>
public class DistrictNode : MonoBehaviour
{
    public int Id;
    public string Name;
    public string Description;
    public int DistricName;
    public DistrictData Value;

    public Projector Projector;
    public Transform Model;
    public Material Material;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void LateUpdate()
    {

    }
}
