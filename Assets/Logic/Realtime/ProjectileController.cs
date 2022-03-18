using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using ProjectileData;

public class ProjectileController : Projectile
{
    public InitialProjectile initialProjectile;

    public void Awake()
    {
        initialProjectile.projectile = gameObject;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!p_rule.Callback(ProjectileRule.ProjectileCallbackType.OnCollide, new object[1] { collision }))
            return;
    }
    public void OnCollisionEnter(Collision collision)
    {

    }
}
