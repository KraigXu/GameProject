using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;

public static class StrategyPlayer
{
    public static int PlayerId;
    public static string Name;
    public static string SurName;
    public static Sprite AvatarSprite;
    public static Entity Entity;
    public static HexUnit Unit;

    public static Vector3 MousePoint;

    public static Entity Entity1;
    public static Entity Entity2;
    public static Entity Entity3;
    public static Entity Entity4;
    
    public static void PlayerInit(int id, string name, string surname, Sprite avatar, Entity entity, HexUnit unit)
    {
        PlayerId = id;
        Name = name;
        SurName = surname;
        AvatarSprite = avatar;
        Entity = entity;
        Unit = unit;
    }
    public static void PlayerInit(int id, string name, string surname, Sprite avatar, Entity entity)
    {
        PlayerId = id;
        Name = name;
        SurName = surname;
        AvatarSprite = avatar;
        Entity = entity;
    }

}
