using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectileData;

[CreateAssetMenu(fileName = "Projectile", menuName = "Create Projectile")]
public class InitialProjectile : ScriptableObject
{
    [HideInInspector]
    public GameObject projectile;
    public ProjectileObject projectileObject;
}
