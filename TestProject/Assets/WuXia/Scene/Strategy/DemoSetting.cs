using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoSetting : MonoBehaviour
{
    public int PlayerId=1;
    public int StartType;           //开启类型 0是正常 1是观看
    public float playerMoveSpeed = 10f;

    public GameObject Biological;
    public GameObject DistrictPrefab;
    public GameObject PlayerBiological;
    public GameObject LivingAreaPrefab;
    public GameObject LivingAreaModelPrefab;

    public DateTime curTime = DateTime.Now;
    public Camera MainCamera;

    public GameObject Biological1;
    public GameObject Biological2;
    public GameObject Biological3;

    public Material LineMat;
    public Material BiologicalMat;

    public Shader Shader;


}
