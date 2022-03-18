using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using WeaponData;

namespace Modules
{
    public class W_JustShoot : WeaponModule
    {
        public W_JustShoot(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule) { }

        public override IEnumerator Reload() { yield break; }

        public override IEnumerator Shoot()
        {
            rule.self.StaminaOut = w_rule.data.weaponObject.stats.Stamina_PerShot;
            w_rule.requireSystem.isShoot = true;
            float speed_multiply = rule.bonus.Get_AttackSpeed() * rule.buffs.Get_AttackSpeed();
            float time = 1f / w_rule.data.weaponObject.stats.RateOfFire;
            MakeHit();
            initiate_effect();
            w_rule.Callback(WeaponRule.WeaponCallbackType.OnShoot, new object[0]);
            while (time > 0)
            {
                w_rule.condition.FireReloadAmount = time / (1f / w_rule.data.weaponObject.stats.RateOfFire);
                time -= Time.deltaTime * speed_multiply;
                yield return null;
            }
            w_rule.condition.FireReloadAmount = 0;
            w_rule.container.RefreshAmmo = 1;
        }
        void initiate_effect()
        {
            if (w_rule.data.weaponObject.stats.shooting_effect == null)
                return;
            GameObject g = Object.Instantiate(w_rule.data.weaponObject.stats.shooting_effect) as GameObject;
            Object.Destroy(g, w_rule.data.weaponObject.stats.shooting_effect_live);
            g.transform.position = rule.requireSystem.aim_original + rule.requireSystem.aim_original_offset;
            g.transform.rotation = Quaternion.LookRotation(rule.requireSystem.aim_directional);
        }
        void MakeHit()
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(
                rule.requireSystem.aim_original, rule.requireSystem.aim_directional,
                w_rule.data.weaponObject.stats.MaxDistance
                );
            List<RaycastHit2D> ret = new List<RaycastHit2D>();
            ret.AddRange(hits);
            hits = SORT_QUEUE(ret).ToArray();
            for (int i = 0; i < hits.Length; i++)
            {
                if (i >= w_rule.data.weaponObject.stats.Penetrating)
                    break;
                controll_hit(hits[i]);
            }
        }
        void controll_hit(RaycastHit2D hit)
        {
            if (hit.collider.GetComponentInParent<UnitHolder>())
            {
                hit.collider.GetComponentInParent<UnitHolder>().unit.rule.self.Hurt =
                    new UnitHitInfo()
                    {
                        Damage = GlobalData.DAMAGE_NOW(w_rule.data.weaponObject.stats.DamageMultiply, rule.self.Level),
                        from = rule,
                        directional = rule.requireSystem.aim_directional,
                        from_pos = rule.requireSystem.aim_original,
                        hit_pos = hit.point,
                        OriginalDamage = w_rule.data.weaponObject.stats.DamageMultiply,
                        same_tag = rule.data.tag,
                        crit_name = hit.collider.tag,
                        hit_object = hit.collider.gameObject
                    };
            }
            else if (hit.collider.GetComponent<Rigidbody>())
            {
                hit.collider.GetComponent<Rigidbody>().AddForceAtPosition(
                    rule.requireSystem.aim_directional * w_rule.data.weaponObject.stats.DamageMultiply * 10,
                    rule.requireSystem.aim_original,
                    ForceMode.Impulse
                    );
            }
        }
        List<RaycastHit2D> SORT_QUEUE(List<RaycastHit2D> units)
        {
            if (units.Count <= 1) return units;

            float middle = units[units.Count / 2].distance;
            List<RaycastHit2D> left = new List<RaycastHit2D>();
            List<RaycastHit2D> right = new List<RaycastHit2D>();
            List<RaycastHit2D> mid = new List<RaycastHit2D>();
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].distance < middle)
                    left.Add(units[i]);
                else if (units[i].distance > middle)
                    right.Add(units[i]);
                else
                    mid.Add(units[i]);
            }
            left = SORT_QUEUE(left);
            right = SORT_QUEUE(right);

            List<RaycastHit2D> ret = new List<RaycastHit2D>();
            ret.AddRange(left);
            ret.AddRange(mid);
            ret.AddRange(right);

            return ret;
        }
    }
    public class W_JustReload : WeaponModule
    {
        public W_JustReload(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule) { }
        public override IEnumerator Reload()
        {
            float time = w_rule.data.weaponObject.stats.ReloadTime;
            float reload_multiply = rule.bonus.Get_ReloadSpeed() * rule.buffs.Get_ReloadSpeed();
            while (time > 0)
            {
                w_rule.condition.ReloadAmount = time / w_rule.data.weaponObject.stats.ReloadTime;
                time -= Time.deltaTime * reload_multiply;
                yield return null;
            }
            w_rule.condition.ReloadAmount = 0;
            w_rule.container.Ammo += w_rule.data.weaponObject.stats.MaxAmmo;
        }

        public override IEnumerator Shoot() { yield break; }
    }
    public class W_SpecialShoot : WeaponModule
    {
        public W_SpecialShoot(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule) { }

        public override IEnumerator Reload()
        {
            yield break;
        }
        public override IEnumerator Shoot()
        {
            rule.self.StaminaOut = w_rule.data.weaponObject.stats.Stamina_PerShot;
            w_rule.requireSystem.isShoot = true;
            float speed_multiply = rule.bonus.Get_AttackSpeed() * rule.buffs.Get_AttackSpeed();
            float time = 1f / w_rule.data.weaponObject.stats.RateOfFire;
            MakeHit();
            initiate_effect();
            w_rule.Callback(WeaponRule.WeaponCallbackType.OnShoot, new object[0]);
            while (time > 0)
            {
                w_rule.condition.FireReloadAmount = time / (1f / w_rule.data.weaponObject.stats.RateOfFire);
                time -= Time.deltaTime * speed_multiply;
                yield return null;
            }
            w_rule.condition.FireReloadAmount = 0;
            w_rule.container.RefreshAmmo = 1;
        }
        void MakeHit()
        {
            GameObject g = get_shooting_object;
            g.transform.position = rule.requireSystem.aim_original;


            g.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rule.requireSystem.aim_directional.y, rule.requireSystem.aim_directional.x) * Mathf.Rad2Deg);
            g.GetComponent<ProjectileController>().Initiate(rule, new ProjectileRule(g.GetComponent<ProjectileController>().initialProjectile));
            rule.CallEvent(UnitRule.UnitEventType.OnSpecialShoot, new object[1] { g.GetComponent<ProjectileController>() });
        }
        void initiate_effect()
        {
            if (w_rule.data.weaponObject.stats.shooting_effect == null)
                return;
            GameObject g = Object.Instantiate(w_rule.data.weaponObject.stats.shooting_effect) as GameObject;
            Object.Destroy(g, w_rule.data.weaponObject.stats.shooting_effect_live);
            g.transform.position = rule.requireSystem.aim_original;
            g.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rule.requireSystem.aim_directional.y, rule.requireSystem.aim_directional.x) * Mathf.Rad2Deg);
        }

        GameObject get_shooting_object
        {
            get
            {
                return Object.Instantiate(w_rule.data.weaponObject.stats.shooting_object) as GameObject;
            }
        }
    }

    public static class WeaponModulesControll
    {
        public static WeaponModule GetModuleByID(ListOfWeaponModules ID, UnitRule rule, WeaponRule w_rule)
        {
            switch (ID)
            {
                case ListOfWeaponModules.JustShoot:
                    return new W_JustShoot(rule, w_rule);
                case ListOfWeaponModules.JustReload:
                    return new W_JustReload(rule, w_rule);
                case ListOfWeaponModules.SpecialShoot:
                    return new W_SpecialShoot(rule, w_rule);
            }

            return null;
        }
        public static WeaponModule[] GetModulesByIDs(ListOfWeaponModules[] IDs, UnitRule rule, WeaponRule w_rule)
        {
            WeaponModule[] ret = new WeaponModule[IDs.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetModuleByID(IDs[i], rule, w_rule);
            return ret;
        }
    }
    public enum ListOfWeaponModules
    {
        JustShoot,
        JustReload,
        SpecialShoot
    }
}
