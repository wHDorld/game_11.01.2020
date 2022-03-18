using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectileData
{
    [System.Serializable]
    public class ProjectileObject
    {
        public ProjectileStats stats;
        public ProjectileAnimation animation;
    }
    [System.Serializable]
    public struct ProjectileStats
    {
        public float Speed;
        public float LifeTime;
        public float LifeTimeAfterStock;
        public float DamageMultiply;
        public HitType HitType;
        public bool IgnoreObstacle;
        public int Rarity;
        public int DamageType;

        public Modules.ListOfProjectileModules[] listOfInitiateModules;
    }
    [System.Serializable]
    public class ProjectileAnimation
    {
        public float rescale_time;
        public _projectileAnimation_element x_direction;
        public _projectileAnimation_element y_direction;
        public _projectileAnimation_element rotate_z_axis;
        public _projectileAnimation_element rotate_x_axis;
        public _projectileAnimation_element rotate_y_axis;
    }

    [System.Serializable]
    public class _projectileAnimation_element
    {
        public AnimationCurve curve;
        public float speed;
    }

    public enum HitType
    {
        Destroy,
        Stock,
        Bounce,
        Pierce
    }
}
