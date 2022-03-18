using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;

namespace Modules
{
    //SPEED EFFECTS
    public class AttackSpeed2x : BonusModule
    {
        public AttackSpeed2x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._TensionSpeed += (ref float arg) =>
            {
                arg *= 2;
                return 2f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class AttackSpeed11x : BonusModule
    {
        public AttackSpeed11x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._TensionSpeed += (ref float arg) =>
            {
                arg *= 1.1f;
                return 2f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class StaminaSpeed15x : BonusModule
    {
        public StaminaSpeed15x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._StaminaHealSpeed += (ref float arg) =>
            {
                arg *= 1.5f;
                return 2f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class MoveSpeed14x : BonusModule
    {
        public MoveSpeed14x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._MoveSpeed += (ref float arg) =>
            {
                arg *= 1.4f;
                return 2f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class MoveSpeed05x : BonusModule
    {
        public MoveSpeed05x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._MoveSpeed += (ref float arg) =>
            {
                arg *= 0.5f;
                return 0.5f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class AttackSpeedDiv2x : BonusModule
    {
        public AttackSpeedDiv2x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._TensionSpeed += (ref float arg) =>
            {
                arg /= 2;
                return 0.5f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class ArrowSpeed_Increase : BonusModule
    {
        public ArrowSpeed_Increase(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }
        bool e(object[] args)
        {
            var a = (ProjectileController)args[0];
            a.p_rule.container.stats.Speed *= 4;
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
        }
    }
    public class AttackSpeed3x : BonusModule
    {
        public AttackSpeed3x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._TensionSpeed += (ref float arg) =>
            {
                arg *= 3;
                return 0.5f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class RotateSpeed03x : BonusModule
    {
        public RotateSpeed03x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.bonus._RotateSpeed += (ref float arg) =>
            {
                arg /= 3;
                return 0.33f;
            };
        }

        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }

    //PER MOMENT EFFECTS
    public class PermomentWater : BonusModule
    {
        public PermomentWater(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {

        }

        float timer = 0;
        public override void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }
            timer = 5;
            foreach (var a in rule.data.gameObject.GetComponentsInChildren<Transform>())
                if (a.name == "Body")
                {
                    new Modules.C_Buff_Water(rule.self.unit, rule.self.unit, a.gameObject);
                    break;
                }
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class PermomentFire : BonusModule
    {
        public PermomentFire(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {

        }

        float timer = 0;
        public override void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }
            timer = 6;
            foreach (var a in rule.data.gameObject.GetComponentsInChildren<Transform>())
                if (a.name == "Body")
                {
                    new Modules.C_Buff_Fire(rule.self.unit, rule.self.unit, a.gameObject);
                    break;
                }
        }
        public override void Destroy()
        {
            return;
        }
    }
    public class FreezeOnHit : BonusModule
    {
        public FreezeOnHit(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.self.OnHit += e;
        }
        bool e(object[] args)
        {
            if (Random.value >= 0.15f)
                return true;
            if (current_stun != null)
            {
                rule.self.unit.StopCoroutine(current_stun);
                if (current_enemy != null)
                {
                    current_enemy.buffs._MoveSpeed -= slow_move;
                    current_enemy.buffs._RotateSpeed -= slow_move;
                    current_enemy.buffs._TensionSpeed -= slow_move;
                }
            }
            current_enemy = ((UnitHitInfo)args[0]).from;
            current_stun = rule.self.unit.StartCoroutine(stun(((UnitHitInfo)args[0]).from));
            return true;
        }
        public override void Update()
        {

        }
        public override void Destroy()
        {
            if (current_stun != null)
            {
                rule.self.unit.StopCoroutine(current_stun);
                if (current_enemy != null)
                {
                    current_enemy.buffs._MoveSpeed -= slow_move;
                    current_enemy.buffs._RotateSpeed -= slow_move;
                    current_enemy.buffs._TensionSpeed -= slow_move;
                }
            }
            rule.self.OnHit -= e;
        }

        float slow_move(ref float arg)
        {
            arg *= 0f;
            return 1;
        }
        Coroutine current_stun;
        UnitRule current_enemy;
        IEnumerator stun(UnitRule enemy)
        {
            enemy.buffs._MoveSpeed += slow_move;
            enemy.buffs._RotateSpeed += slow_move;
            enemy.buffs._TensionSpeed += slow_move;
            yield return new WaitForSeconds(1.5f);
            enemy.buffs._MoveSpeed -= slow_move;
            enemy.buffs._RotateSpeed -= slow_move;
            enemy.buffs._TensionSpeed -= slow_move;

            current_stun = null;
        }
    }

    //ARROW EFFECTS
    public class RandomFire_Energy : BonusModule
    {
        public RandomFire_Energy(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }
        bool e(object[] args)
        {
            if (Random.value > 0.2f)
                return true;
            var a = (ProjectileController)args[0];
            a.p_rule.container.stats.DamageType = 2;
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
        }
    }
    public class Fire_ArrowPush : BonusModule
    {
        public Fire_ArrowPush(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialHitToUnit += e;
        }
        bool e(object[] args)
        {
            rule.self.unit.StartCoroutine(push((UnitRule)args[0], rule.data.transform.right));
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            rule.OnSpecialHitToUnit -= e;
        }

        public IEnumerator push(UnitRule enemy, Vector2 direction)
        {
            direction = direction.normalized;
            float timer = 1f;
            while (timer > 0)
            {
                timer -= Time.deltaTime * 8;
                enemy.data.rigidbody.MovePosition(enemy.data.rigidbody.position + direction * Time.deltaTime * 10);
                yield return null;
            }
        }
    }
    public class RandomFire_ShellShock : BonusModule
    {
        public RandomFire_ShellShock(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialHitToUnit += e;
        }
        bool e(object[] args)
        {
            if (Random.value > 0.06f)
                return true;
            if (current_stun != null)
            {
                rule.self.unit.StopCoroutine(current_stun);
                if (current_enemy != null)
                {
                    current_enemy.OnSpecialShoot -= random_fire;
                }
            }
            current_enemy = ((UnitRule)args[0]);
            current_stun = rule.self.unit.StartCoroutine(stun((UnitRule)args[0]));
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            if (current_stun != null)
            {
                rule.self.unit.StopCoroutine(current_stun);
                if (current_enemy != null)
                {
                    current_enemy.OnSpecialShoot -= random_fire;
                }
            }
            rule.OnSpecialHitToUnit -= e;
        }

        bool random_fire(object[] args)
        {
            var a = (ProjectileController)args[0];
            a.transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-60, 60));
            return true;
        }
        Coroutine current_stun;
        UnitRule current_enemy;
        IEnumerator stun(UnitRule enemy)
        {
            enemy.OnSpecialShoot += random_fire;
            yield return new WaitForSeconds(5f);
            enemy.OnSpecialShoot -= random_fire;

            current_stun = null;
        }
    }
    public class Fire_ArrowDamage05x : BonusModule
    {
        public Fire_ArrowDamage05x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }
        bool e(object[] args)
        {
            var a = (ProjectileController)args[0];
            a.p_rule.container.stats.DamageMultiply /= 2;
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
        }
    }

    //DAMAGE EFFECTS
    public class DamageBuff_Energy : BonusModule
    {
        public DamageBuff_Energy(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialHit += e;
        }

        bool e(object[] args)
        {
            var a = (ProjectileRule)args[0];
            if (a.container.stats.DamageType != 2)
                return true;
            a.container.stats.DamageMultiply *= 2;
            return true;
        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            rule.OnSpecialHit -= e;
        }
    }

    //GAMEPLAY EFFECTS
    public class OnShootRandom_AttackSpeed10x : BonusModule
    {
        public OnShootRandom_AttackSpeed10x(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }

        float speed_up(ref float arg)
        {
            arg *= 60;
            return 60;
        }
        bool e(object[] args)
        {
            if (timer > 0)
            {
                var a = (ProjectileController)args[0];
                a.p_rule.container.stats.DamageMultiply /= 2;
                a.p_rule.container.stats.Speed *= Random.Range(0.6f, 0.8f);
                a.transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-20, 20));
                return true;
            }
            if (Random.value < 0.7f)
            {
                return true;
            }
            timer = 2;
            return true;
        }
        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
            if (isApply)
                rule.bonus._TensionSpeed -= speed_up;
        }

        float timer = 0;
        bool isApply = false;
        public override void Update()
        {
            if (timer <= 0)
            {
                if (isApply)
                {
                    rule.bonus._TensionSpeed -= speed_up;
                    isApply = false;
                }
                return;
            }
            if (!isApply)
            {
                rule.bonus._TensionSpeed += speed_up;
                isApply = true;
            }
            timer -= Time.deltaTime;
            return;
        }
    }
    public class OnShoot_ArrowLookAt : BonusModule
    {
        public OnShoot_ArrowLookAt(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }
        
        bool e(object[] args)
        {
            var a = (ProjectileController)args[0];
            a.p_rule.container.projectile.modules.Add(new Modules.P_LookAtAim(rule, a.p_rule));

            return true;
        }

        public override void Update()
        {
            return;
        }

        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
        }
    }
    public class OnShootRandom_ShootAgain : BonusModule
    {
        public OnShootRandom_ShootAgain(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {
            rule.OnSpecialShoot += e;
        }
        bool e(object[] args)
        {
            if (!can_shoot)
                return true;
            if (Random.value < 0.7f)
            {
                var a = (ProjectileController)args[0];
                a.p_rule.container.stats.DamageMultiply /= 2;
                rule.self.unit.StartCoroutine(shoot());
            }
            return true;
        }
        public override void Update()
        {
            return;
        }

        bool can_shoot = true;
        public IEnumerator shoot()
        {
            can_shoot = false;
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < rule.self.unit.weapon.permanent_modules.Count; i++)
                yield return rule.self.unit.StartCoroutine(rule.self.unit.weapon.permanent_modules[i].Shoot());
            for (int i = 0; i < rule.self.unit.weapon.temporary_modules.Count; i++)
                yield return rule.self.unit.StartCoroutine(rule.self.unit.weapon.temporary_modules[i].Shoot());
            yield return new WaitForSeconds(0.1f);
            can_shoot = true;
        }
        public override void Destroy()
        {
            rule.OnSpecialShoot -= e;
        }
    }

    //NO LOGIC EFFECTS
    public class ArmShield : BonusModule
    {
        public ArmShield(UnitRule rule, WeaponRule w_rule) : base(rule, w_rule)
        {

        }
        public override void Update()
        {
            return;
        }
        public override void Destroy()
        {
            return;
        }
    }

    //INFO
    public static class BonusModulesControll
    {
        public static BonusModule GetModuleByID(ListOfBonusModules ID, UnitRule rule, WeaponRule w_rule)
        {
            switch (ID)
            {
                case ListOfBonusModules.ArmShield:
                    return new ArmShield(rule, w_rule);
                case ListOfBonusModules.RotateSpeed03x:
                    return new RotateSpeed03x(rule, w_rule);
                case ListOfBonusModules.AttackSpeed3x:
                    return new AttackSpeed3x(rule, w_rule);
                case ListOfBonusModules.Fire_ArrowDamage05x:
                    return new Fire_ArrowDamage05x(rule, w_rule);
                case ListOfBonusModules.RandomFire_ShellShock:
                    return new RandomFire_ShellShock(rule, w_rule);
                case ListOfBonusModules.Fire_ArrowPush:
                    return new Fire_ArrowPush(rule, w_rule);
                case ListOfBonusModules.MoveSpeed05x:
                    return new MoveSpeed05x(rule, w_rule);
                case ListOfBonusModules.FreezeOnHit:
                    return new FreezeOnHit(rule, w_rule);
                case ListOfBonusModules.ArrowSpeed_Increase:
                    return new ArrowSpeed_Increase(rule, w_rule);
                case ListOfBonusModules.OnShootRandom_ShootAgain:
                    return new OnShootRandom_ShootAgain(rule, w_rule);
                case ListOfBonusModules.AttackSpeed2x:
                    return new AttackSpeed2x(rule, w_rule);
                case ListOfBonusModules.AttackSpeed11x:
                    return new AttackSpeed11x(rule, w_rule);
                case ListOfBonusModules.PermomentWater:
                    return new PermomentWater(rule, w_rule);
                case ListOfBonusModules.AttackSpeedDiv2x:
                    return new AttackSpeedDiv2x(rule, w_rule);
                case ListOfBonusModules.StaminaSpeed15x:
                    return new StaminaSpeed15x(rule, w_rule);
                case ListOfBonusModules.RandomFire_Energy:
                    return new RandomFire_Energy(rule, w_rule);
                case ListOfBonusModules.DamageBuff_Energy:
                    return new DamageBuff_Energy(rule, w_rule);
                case ListOfBonusModules.PermomentFire:
                    return new PermomentFire(rule, w_rule);
                case ListOfBonusModules.OnShootRandom_AttackSpeed10x:
                    return new OnShootRandom_AttackSpeed10x(rule, w_rule);
                case ListOfBonusModules.OnShoot_ArrowLookAt:
                    return new OnShoot_ArrowLookAt(rule, w_rule);
            }

            return null;
        }
        public static BonusModule[] GetModulesByIDs(ListOfBonusModules[] IDs, UnitRule rule, WeaponRule w_rule)
        {
            if (IDs == null)
                return new BonusModule[0];
            BonusModule[] ret = new BonusModule[IDs.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetModuleByID(IDs[i], rule, w_rule);
            return ret;
        }
        public static string[] GetModulesDescriptionByIDs(ListOfBonusModules[] IDs)
        {
            List<string> ret = new List<string>();
            foreach (var a in IDs)
            {
                switch (a)
                {
                    case ListOfBonusModules.ArmShield:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(28))
                            );
                        break;
                    case ListOfBonusModules.RotateSpeed03x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(27))
                            );
                        break;
                    case ListOfBonusModules.AttackSpeed3x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(26))
                            );
                        break;
                    case ListOfBonusModules.Fire_ArrowDamage05x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(25))
                            );
                        break;
                    case ListOfBonusModules.RandomFire_ShellShock:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(24))
                            );
                        break;
                    case ListOfBonusModules.Fire_ArrowPush:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(23))
                            );
                        break;
                    case ListOfBonusModules.MoveSpeed05x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(22))
                            );
                        break;
                    case ListOfBonusModules.FreezeOnHit:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(21))
                            );
                        break;
                    case ListOfBonusModules.ArrowSpeed_Increase:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(20))
                            );
                        break;
                    case ListOfBonusModules.OnShootRandom_ShootAgain:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(19))
                            );
                        break;
                    case ListOfBonusModules.OnShoot_ArrowLookAt:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(18))
                            );
                        break;
                    case ListOfBonusModules.OnShootRandom_AttackSpeed10x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(17))
                            );
                        break;
                    case ListOfBonusModules.PermomentFire:
                        ret.Add(
                            string.Format("{0}<sprite=3>",
                            GlobalData.translate.Get_Words_ByID(16))
                            );
                        break;
                    case ListOfBonusModules.DamageBuff_Energy:
                        ret.Add(
                            string.Format("{0}<sprite=2>",
                            GlobalData.translate.Get_Words_ByID(15))
                            );
                        break;
                    case ListOfBonusModules.RandomFire_Energy:
                        ret.Add(
                            string.Format("{0}<sprite=2>",
                            GlobalData.translate.Get_Words_ByID(14))
                            );
                        break;
                    case ListOfBonusModules.StaminaSpeed15x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(13))
                            );
                        break;
                    case ListOfBonusModules.AttackSpeed2x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(10))
                            );
                        break;
                    case ListOfBonusModules.AttackSpeed11x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(3))
                            );
                        break;
                    case ListOfBonusModules.PermomentWater:
                        ret.Add(
                            string.Format("{0}<sprite=8>",
                            GlobalData.translate.Get_Words_ByID(11))
                            );
                        break;
                    case ListOfBonusModules.AttackSpeedDiv2x:
                        ret.Add(
                            string.Format("{0}",
                            GlobalData.translate.Get_Words_ByID(12))
                            );
                        break;
                }
            }
            return ret.ToArray();
        }
    }
    public enum ListOfBonusModules
    {
        AttackSpeed2x,
        AttackSpeed11x,
        PermomentWater,
        AttackSpeedDiv2x,
        StaminaSpeed15x,
        RandomFire_Energy,
        DamageBuff_Energy,
        PermomentFire,
        OnShootRandom_AttackSpeed10x,
        OnShoot_ArrowLookAt,
        OnShootRandom_ShootAgain,
        ArrowSpeed_Increase,
        FreezeOnHit,
        MoveSpeed05x,
        Fire_ArrowPush,
        RandomFire_ShellShock,
        Fire_ArrowDamage05x,
        AttackSpeed3x,
        RotateSpeed03x,
        ArmShield
    }
}
