using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;

public class LivingAreaState  {

 public int Id;                                      //编号
    public string Name;                            //名称
    public string Description;                   //说明
    public float CoolingTime;                  //冷却
    public Sprite Icon;                          //图标
    public Color sceneGizmoColor = Color.gray;      //拿来渲染eyes的Gizmos颜色
    public LivingAreaAction[] behaviors;

    public LivingAreaState() { }

    public LivingAreaState(int id, string name, string desc, float cooling, Sprite icon)
    {
        this.Id = id;
        this.Name = name;
        this.Description = desc;
        this.CoolingTime = cooling;
        this.Icon = icon;
    }

    public void UpdateBehaviors(LivingAreaNode controller)
    {
        DoBehaviors(controller);
    }

    /// <summary>
    /// 顺序执行所有效果
    /// </summary>
    /// <param name="controller"></param>
    private void DoBehaviors(LivingAreaNode controller)
    {
        for (int i = 0; i < behaviors.Length; i++)
        {
            behaviors[i].Act(controller);
        }
    }
}
