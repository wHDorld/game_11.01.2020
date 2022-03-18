using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponData;

[CreateAssetMenu(fileName = "Weapon", menuName = "Create new Weapon")]
public class WeaponObject : ScriptableObject
{
    public int Name;
    public int Description;
    public Sprite LQ_Preview;
    public Sprite HQ_Preview;

    public WeaponStats stats;
}
