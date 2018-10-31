using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using LivingArea;
using UnityEngine;

/// <summary>
/// 区节点
/// </summary>
public class DistrictNode : MonoBehaviour
{
    public int Id;
    public string Name;
    public string Description;
    public int FactionId;
    public int GrowingModulus;
    public int SecurityModulus;
    public int Traffic;

    public List<LivingAreaNode> LivingAreaChilds=new List<LivingAreaNode>();

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
