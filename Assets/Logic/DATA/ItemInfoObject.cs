using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using Modules;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "Create new ItemInfo")]
public class ItemInfoObject : ScriptableObject
{
    [BoxGroup("Data by ID's")] public int Name;
    [BoxGroup("Data by ID's")] public int Description;
    public Sprite LQ_Preview;
    public Sprite HQ_Preview;

    [BoxGroup("Info")]
    public ItemInfo info;
}

[System.Serializable]
public class ItemInfo
{
    public Sprite EquipSprite;
    public int rarity;
    public Resistes resist;
    public ListOfBonusModules[] bonus;
    public Object child_object;
}
[System.Serializable]
public class Resistes
{
    public float PhysicalResist;
    public float WaterResist;
    public float FireResist;
    public float DarkResist;
    public float LightResist;
    public float EnergyResist;
    public float SplashResist;
    public float ToxicResist;
}
