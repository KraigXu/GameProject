﻿using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;


public delegate void EntityCallBack(Entity entity, int id);
public delegate int[] SocialDialogEvent(int resoult, int a, int b);
public delegate void SingleParameterEvent(int key);

public delegate void EntitySingleEvent(Entity a, Entity b);




/// <summary>
/// 记录整个程序运行中所有的确定 和流动数据，不应该被删除
/// </summary>
public sealed class Define
{
    public static readonly string TaskSceneName = "Start";
    public static readonly string FightingSceneName = "Demo";
    public static readonly string ManagerSceneName = "Manager";
    public static readonly string LoadingSceneName = "Loading";

    /// <summary>
    /// 当前游戏状态             -1:发生错误  0:正常界面 1正常战略 2正常战役
    /// </summary>
    public static byte GameStatus = 0;
    public static int PlayerId = 1;
    public static string CurrentSceneName = "";
    public static string NextSceneName = "";
    public static int CurStatus = 3;

    //----->按键
    public static KeyCode CofirmCode = KeyCode.Mouse0;
    public static KeyCode CancelCode = KeyCode.Mouse1;
    public static KeyCode GameRoationLeft = KeyCode.Z;
    public static KeyCode GameRoationRight = KeyCode.X;
    public static KeyCode ItemSlot1 = KeyCode.Alpha1;
    public static KeyCode ItemSlot2 = KeyCode.Alpha2;
    public static KeyCode ItemSlot3 = KeyCode.Alpha3;
    public static KeyCode ItemSlot4 = KeyCode.Alpha4;
    public static KeyCode ItemSlot5 = KeyCode.Alpha5;
    public static KeyCode ItemSlot6 = KeyCode.Alpha6;
    public static KeyCode ItemSlot7 = KeyCode.Alpha7;
    public static KeyCode ItemSlot8 = KeyCode.Alpha8;
    public static KeyCode CommandSkill1 = KeyCode.Q;
    public static KeyCode CommandSkill2 = KeyCode.W;
    public static KeyCode CommandSkill3 = KeyCode.E;
    public static KeyCode Intelligent = KeyCode.LeftShift;
    public static KeyCode LevelMap = KeyCode.M;
    public static KeyCode SkillSlot1 = KeyCode.Alpha1;
    public static KeyCode SkillSlot2 = KeyCode.Alpha2;
    public static KeyCode SkillSlot3 = KeyCode.Alpha3;
    public static KeyCode SkillSlot4 = KeyCode.Alpha4;
    public static KeyCode SkillSlot5 = KeyCode.Alpha5;
    public static KeyCode SkillSlot6 = KeyCode.Alpha6;
    public static KeyCode SkillSlot7 = KeyCode.Alpha7;
    public static KeyCode SkillSlot8 = KeyCode.Alpha8;


    public static readonly string TagTerrain = "Terrain";

    public static readonly string TagLivingArea = "LivingArea";

    public static readonly string TagBiological = "Biological";

    public static readonly string PoolName = "GeneratedPool";



    public static bool IsAPointInACamera(Camera camera ,Vector3 position,bool isNeedModelBlockOut=false)
    {
        // 是否在视野内
        bool result1 = false;
        Vector3 posViewport = camera.WorldToViewportPoint(position);
        Rect rect = new Rect(0, 0, 1, 1);
        result1 = rect.Contains(posViewport);
        // 是否在远近平面内
        bool result2 = false;
        if (posViewport.z >= camera.nearClipPlane && posViewport.z <= camera.farClipPlane)
        {
            result2 = true;
        }
        // 综合判断
        bool result = result1 && result2;
        if (isNeedModelBlockOut)
        {
            Vector3 dir = position - camera.transform.position;
            RaycastHit[] hits;
            bool isBlock = false;
            hits = Physics.RaycastAll(position, -dir);
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                Debug.DrawLine(position, hit.point, Color.black);
                if (hit.transform.tag == "Terrain")
                {
                    isBlock = true;
                    break;
                }
            }
            return result && !isBlock;
        }
        else
            return result;

    }

}
