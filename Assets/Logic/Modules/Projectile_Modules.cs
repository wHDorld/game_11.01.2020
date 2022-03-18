using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using ProjectileData;

namespace Modules
{
    public class P_JustFly : ProjectileModule
    {
        public P_JustFly(UnitRule rule, ProjectileRule p_rule) : base(rule, p_rule) { }

        float[] timer = new float[5];
        public override void Update()
        {
            update_timer();

            Vector3 additional_direction =
                p_rule.data.transform.TransformDirection(
                    new Vector3(
                        p_rule.data.projectileObject.animation.x_direction.curve.Evaluate(timer[0]),
                        0
                        )
                    ) + new Vector3(
                        0,
                        p_rule.data.projectileObject.animation.y_direction.curve.Evaluate(timer[1])
                        );
            Vector3 additional_rotation =
                    new Vector3(
                        p_rule.data.projectileObject.animation.rotate_x_axis.curve.Evaluate(timer[3]),
                        p_rule.data.projectileObject.animation.rotate_y_axis.curve.Evaluate(timer[4]),
                        p_rule.data.projectileObject.animation.rotate_z_axis.curve.Evaluate(timer[2])
                        );

            p_rule.data.rigidbody.MovePosition(
                p_rule.data.transform.position +
                p_rule.data.transform.right * p_rule.container.stats.Speed + 
                additional_direction);
            p_rule.data.transform.rotation *= Quaternion.Euler(additional_rotation);
        }
        
        void update_timer()
        {
            timer[0] += Time.deltaTime * p_rule.data.projectileObject.animation.x_direction.speed;
            timer[0] = timer[0] > 1 ? 0 : timer[0];

            timer[1] += Time.deltaTime * p_rule.data.projectileObject.animation.y_direction.speed;
            timer[1] = timer[1] > 1 ? 0 : timer[1];

            timer[2] += Time.deltaTime * p_rule.data.projectileObject.animation.rotate_z_axis.speed;
            timer[2] = timer[2] > 1 ? 0 : timer[2];

            timer[3] += Time.deltaTime * p_rule.data.projectileObject.animation.rotate_x_axis.speed;
            timer[3] = timer[3] > 1 ? 0 : timer[3];

            timer[4] += Time.deltaTime * p_rule.data.projectileObject.animation.rotate_y_axis.speed;
            timer[4] = timer[4] > 1 ? 0 : timer[4];
        }
    }
    public class P_CollideWork : ProjectileModule
    {
        public P_CollideWork(UnitRule rule, ProjectileRule p_rule) : base(rule, p_rule)
        {
            p_rule.OnCollide += (args) =>
            {
                /*Collider2D a = (Collider2D)args[0];

                if (a.gameObject.GetComponentInParent<UnitHolder>())
                {
                    if (a.gameObject.GetComponentInParent<UnitHolder>().unit.rule.data.tag == rule.data.tag)
                        return false;

                    buff_apply(a.gameObject.GetComponentInParent<UnitHolder>(), a.gameObject);
                    a.gameObject.GetComponentInParent<UnitHolder>().unit.rule.self.Hurt = new UnitHitInfo()
                    {
                        Damage = GlobalData.DAMAGE_NOW(p_rule.container.stats.DamageMultiply, rule.self.Level),
                        DamageType = p_rule.container.stats.DamageType,
                        from = rule,
                        directional = rule.requireSystem.aim_directional,
                        from_pos = rule.requireSystem.aim_original,
                        hit_pos = p_rule.data.transform.position,
                        OriginalDamage = p_rule.container.stats.DamageMultiply,
                        same_tag = rule.data.tag,
                        crit_name = a.tag,
                        hit_object = a.gameObject
                    };
                    hit_type(p_rule.data.transform.position, a.gameObject.transform);
                    return true;
                }
                else if (a.gameObject.GetComponent<Rigidbody2D>())
                {
                    a.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                        rule.requireSystem.aim_directional * p_rule.container.stats.DamageMultiply * 200,
                        p_rule.data.transform.position,
                        ForceMode2D.Impulse
                        );
                }

                if (!a.gameObject.GetComponentInParent<UnitHolder>() && !p_rule.container.stats.IgnoreObstacle)
                {
                    hit_type(p_rule.data.transform.position, a.gameObject.transform);
                    return true;
                }*/

                return false;
            };
        }

        float time = 0;
        public override void Update()
        {
            raycast_detect();
            time += Time.deltaTime;
            if (time >= p_rule.container.stats.LifeTime)
                Object.Destroy(p_rule.data.transform.gameObject);
        }

        void raycast_detect()
        {
            RaycastHit2D hit = Physics2D.Raycast(p_rule.data.rigidbody.position, p_rule.data.transform.right, p_rule.container.stats.Speed + 0.01f, 
                ~(LayerMask.GetMask("Projectile") + LayerMask.GetMask("Trigger")));
            if (hit.collider == null)
            {
                hit = Physics2D.Raycast(p_rule.data.rigidbody.position, -p_rule.data.transform.right, p_rule.container.stats.Speed + 0.01f, 
                    ~(LayerMask.GetMask("Projectile") + LayerMask.GetMask("Trigger")));
                if (hit.collider == null)
                    return;
            }
            var a = hit.collider;

            if (a.gameObject.GetComponentInParent<UnitHolder>())
            {
                if (a.gameObject.GetComponentInParent<UnitHolder>().unit.rule.data.tag == rule.data.tag)
                    return;
                buff_apply(a.gameObject.GetComponentInParent<UnitHolder>(), a.gameObject);
                rule.CallEvent(UnitRule.UnitEventType.OnSpecialHit, new object[1] { p_rule });
                rule.CallEvent(UnitRule.UnitEventType.OnSpecialHitToUnit, new object[1] { a.gameObject.GetComponentInParent<UnitHolder>().unit.rule });
                a.gameObject.GetComponentInParent<UnitHolder>().unit.rule.self.Hurt = new UnitHitInfo()
                {
                    Damage = GlobalData.DAMAGE_NOW(p_rule.container.stats.DamageMultiply, rule.self.Level),
                    DamageType = p_rule.container.stats.DamageType,
                    from = rule,
                    directional = rule.requireSystem.aim_directional,
                    from_pos = rule.requireSystem.aim_original,
                    hit_pos = hit.point,
                    OriginalDamage = p_rule.container.stats.DamageMultiply,
                    same_tag = rule.data.tag,
                    crit_name = a.tag,
                    hit_object = a.gameObject
                };
                hit_type(hit, true);
                return;
            }
            else if (a.gameObject.GetComponent<Rigidbody2D>())
            {
                a.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                    rule.requireSystem.aim_directional * p_rule.container.stats.DamageMultiply * 200,
                    hit.point,
                    ForceMode2D.Impulse
                    );
                if (!p_rule.container.stats.IgnoreObstacle)
                    hit_type(hit, false);
                return;
            }
            if (!p_rule.container.stats.IgnoreObstacle)
                hit_type(hit, false);
        }

        void hit_type(RaycastHit2D hit, bool isUnitHit)
        {
            switch (p_rule.container.stats.HitType)
            {
                case HitType.Destroy:
                    hitType_destroy();
                    break;
                case HitType.Stock:
                    hitType_stock(hit.point, hit.collider.transform);
                    break;
                case HitType.Bounce:
                    hitType_bounce(hit);
                    break;
                case HitType.Pierce:
                    hitType_bounce(hit);
                    break;
            }
        }
        void hitType_destroy()
        {
            Object.Destroy(p_rule.data.transform.gameObject);
        }
        void hitType_stock(Vector3 hit_pos, Transform hit_to)
        {
            p_rule.data.transform.gameObject.GetComponent<ProjectileController>().StopAllCoroutines();
            p_rule.data.transform.localScale = p_rule.container.initial_scale;
            p_rule.data.transform.position = hit_pos;
            p_rule.data.transform.SetParent(hit_to, true);
            p_rule.data.transform.localPosition = new Vector3(
                p_rule.data.transform.localPosition.x,
                p_rule.data.transform.localPosition.y,
                0
                );
            Object.Destroy(p_rule.data.rigidbody);
            Object.Destroy(p_rule.data.transform.gameObject.GetComponent<ProjectileController>());
            Object.Destroy(p_rule.data.transform.gameObject, p_rule.container.stats.LifeTimeAfterStock);
        }
        void hitType_bounce(RaycastHit2D hit)
        {
            Vector2 v_a = (hit.normal + new Vector2(p_rule.data.transform.right.x, p_rule.data.transform.right.y));

            p_rule.data.transform.gameObject.GetComponent<ProjectileController>().StopAllCoroutines();
            p_rule.data.transform.localScale = p_rule.container.initial_scale;
            p_rule.data.transform.position = hit.point;

            p_rule.data.rigidbody.velocity = ((hit.normal + v_a) * 10f);
            p_rule.data.rigidbody.angularVelocity = Random.Range(-900, 900f);

            p_rule.data.transform.gameObject.GetComponent<Collider2D>().enabled = true;
            Object.Destroy(p_rule.data.transform.gameObject.GetComponent<ProjectileController>());
            Object.Destroy(p_rule.data.transform.gameObject, p_rule.container.stats.LifeTimeAfterStock);
        }

        void buff_apply(UnitHolder holder, GameObject hitObj)
        {
            switch (p_rule.container.stats.DamageType)
            {
                case 0:
                    new C_Buff_Aid(holder.unit, rule.self.unit, hitObj);
                    break;
                case 2:
                    new C_Buff_Energy(holder.unit, rule.self.unit, hitObj);
                    break;
                case 3:
                    new C_Buff_Fire(holder.unit, rule.self.unit, hitObj);
                    break;
                case 4:
                    new C_Buff_Light(holder.unit, rule.self.unit, hitObj);
                    break;
                case 6:
                    new C_Buff_Dark(holder.unit, rule.self.unit, hitObj);
                    break;
                case 7:
                    new C_Buff_Aoe(holder.unit, rule.self.unit, hitObj);
                    break;
                case 8:
                    new C_Buff_Water(holder.unit, rule.self.unit, hitObj);
                    break;
            }
        }
    }
    public class P_LookAtAim : ProjectileModule
    {
        public P_LookAtAim(UnitRule rule, ProjectileRule p_rule) : base(rule, p_rule) { }
        
        public override void Update()
        {
            Vector2 dir = rule.preferableSystem.preferable_look_pos - p_rule.data.transform.position;
            p_rule.data.transform.rotation = 
                Quaternion.Lerp(
                    p_rule.data.transform.rotation,
                    Quaternion.Euler(
                        0, 
                        0,
                        Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg), 
                    9f * Time.deltaTime
                );
        }
    }

    public static class ProjectileModulesControll
    {
        public static ProjectileModule GetModuleByID(ListOfProjectileModules ID, UnitRule rule, ProjectileRule p_rule)
        {
            switch (ID)
            {
                case ListOfProjectileModules.JustFly:
                    return new P_JustFly(rule, p_rule);
                case ListOfProjectileModules.CollideWork:
                    return new P_CollideWork(rule, p_rule);
                case ListOfProjectileModules.LookAtAim:
                    return new P_LookAtAim(rule, p_rule);
            }

            return null;
        }
        public static ProjectileModule[] GetModulesByIDs(ListOfProjectileModules[] IDs, UnitRule rule, ProjectileRule p_rule)
        {
            ProjectileModule[] ret = new ProjectileModule[IDs.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetModuleByID(IDs[i], rule, p_rule);
            return ret;
        }
    }
    public enum ListOfProjectileModules
    {
        JustFly,
        CollideWork,
        LookAtAim
    }
}
