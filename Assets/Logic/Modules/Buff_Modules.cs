using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;

namespace Modules
{
    public class C_Buff_Fire : BuffModule
    {
        public C_Buff_Fire(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {

            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Fire)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Aid)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Water)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
        }

        float time = 5;
        float temporary_time = 0;
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            temporary_time -= Time.deltaTime;
            if (temporary_time <= 0)
            {
                temporary_time = 0.2f;
                to.rule.self.Hurt = new UnitHitInfo()
                {
                    Damage = GlobalData.DAMAGE_NOW(0.1f, from.rule.self.Level),
                    DamageType = 3,
                    from = from.rule,
                    directional = from.rule.requireSystem.aim_directional,
                    from_pos = from.rule.requireSystem.aim_original,
                    hit_pos = to.rule.data.transform.position,
                    OriginalDamage = 0.1f,
                    same_tag = from.rule.data.tag,
                    crit_name = "",
                    hit_object = to.rule.data.transform.gameObject
                };
            }
            if (time <= 0)
                Destroy();
        }
        public override void Destroy()
        {
            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 8;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject fireg = GlobalData.get_fire_effect;
            fireg.transform.SetParent(objectHit.transform, false);
            fireg.transform.localPosition = Vector3.zero;
            fireg.name = "FireEffect";
            Object.Destroy(fireg, 8);
            var fire = fireg.GetComponent<ParticleSystem>().shape;
            fire.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    public class C_Buff_Aid : BuffModule
    {
        public C_Buff_Aid(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Aid)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._MoveSpeed += slow_move;
            to.rule.buffs._AttackSpeed += slow_move;
        }
        float slow_move(ref float arg)
        {
            arg *= 0.5f;
            return 1;
        }

        float time = 60;
        float temporary_time = 0;
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            temporary_time -= Time.deltaTime;
            if (temporary_time <= 0)
            {
                temporary_time = 1f;
                to.rule.self.Hurt = new UnitHitInfo()
                {
                    Damage = GlobalData.DAMAGE_NOW(0.1f, from.rule.self.Level),
                    from = from.rule,
                    directional = from.rule.requireSystem.aim_directional,
                    from_pos = from.rule.requireSystem.aim_original,
                    hit_pos = to.rule.data.transform.position,
                    OriginalDamage = 0.01f,
                    same_tag = from.rule.data.tag,
                    crit_name = "",
                    hit_object = to.rule.data.transform.gameObject
                };
            }
            if (time <= 0)
                Destroy();
        }
        public override void Destroy()
        {
            to.rule.buffs._MoveSpeed -= slow_move;
            to.rule.buffs._AttackSpeed -= slow_move;

            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 3;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject aidg = GlobalData.get_aid_effect;
            aidg.transform.SetParent(objectHit.transform, false);
            aidg.transform.localPosition = Vector3.zero;
            aidg.name = "AidEffect";
            Object.Destroy(aidg, 3);
            var aid = aidg.GetComponent<ParticleSystem>().shape;
            aid.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    public class C_Buff_Light : BuffModule
    {
        public C_Buff_Light(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Light)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Dark)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._ReloadSpeed += slow_move;
        }
        float slow_move(ref float arg)
        {
            arg *= 0.5f;
            return 1;
        }

        float time = 10;
        float temporary_time = 0;
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            temporary_time -= Time.deltaTime;
            if (temporary_time <= 0)
            {
                temporary_time = 0.5f;
                //rework
                for (int i = 0; i < to.buff_modules.Count; i++)
                    if (to.buff_modules[i] is C_Buff_Aid)
                    {
                        to.buff_modules[i].Destroy();
                        break;
                    }
                for (int i = 0; i < to.buff_modules.Count; i++)
                    if (to.buff_modules[i] is C_Buff_Fire)
                    {
                        to.buff_modules[i].Destroy();
                        break;
                    }
                foreach (var a in to.rule.data.transform.GetComponentsInChildren<ParticleSystem>())
                {
                    if (a.name == "AidEffect") Object.Destroy(a.gameObject);
                    if (a.name == "FireEffect") Object.Destroy(a.gameObject);
                }
            }
            if (time <= 0)
                Destroy();
        }
        public override void Destroy()
        {
            to.rule.buffs._ReloadSpeed -= slow_move;

            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 6;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject lightg = GlobalData.get_light_effect;
            lightg.transform.SetParent(objectHit.transform, false);
            lightg.transform.localPosition = Vector3.zero;
            lightg.name = "LightEffect";
            Object.Destroy(lightg, 6);
            var light = lightg.GetComponent<ParticleSystem>().shape;
            light.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    public class C_Buff_Dark : BuffModule
    {
        public C_Buff_Dark(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Dark)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._ReloadSpeed += slow_move;
        }
        float slow_move(ref float arg)
        {
            arg *= 1.2f;
            return 1;
        }
        
        float temporary_time = 3;
        public override void Update()
        {
            base.Update();
            temporary_time -= Time.deltaTime;
            if (temporary_time <= 0)
            {
                temporary_time = 3f;
                give_random();
            }
        }
        void give_random()
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    new C_Buff_Aid(to, from, objectHit);
                    break;
                case 1:
                    new C_Buff_Fire(to, from, objectHit);
                    break;
                case 2:
                    new C_Buff_Energy(to, from, objectHit);
                    break;
            }
        }
        public override void Destroy()
        {
            to.rule.buffs._ReloadSpeed -= slow_move;

            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 15;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject darkg = GlobalData.get_dark_effect;
            darkg.transform.SetParent(objectHit.transform, false);
            darkg.transform.localPosition = Vector3.zero;
            darkg.name = "DarkEffect";
            Object.Destroy(darkg, 15);
            var dark = darkg.GetComponent<ParticleSystem>().shape;
            dark.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    public class C_Buff_Water : BuffModule
    {
        public C_Buff_Water(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Fire)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._MoveSpeed += slow_move;
            to.rule.self.OnHit += hit;
        }
        float slow_move(ref float arg)
        {
            arg *= 1f - (0.1f * (1f - to.rule.clothes.Get_WaterResist / 100f));
            return 1;
        }
        bool hit(object[] args)
        {
            var a = (UnitHitInfo)args[0];
            if (a.DamageType == 2)
                to.rule.self.Hurt = new UnitHitInfo()
                {
                    Damage = a.Damage * 0.1f,
                    DamageType = 8,
                    from = this.from.rule,
                    directional = a.directional,
                    from_pos = a.from_pos,
                    hit_pos = a.hit_pos,
                    OriginalDamage = a.OriginalDamage * 0.1f,
                    same_tag = a.same_tag,
                    crit_name = a.crit_name,
                    hit_object = a.hit_object
                };
            return true;
        }

        float time = 10;
        float temporary_time = 10;
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Destroy();
            }
        }
        public override void Destroy()
        {
            to.rule.buffs._MoveSpeed -= slow_move;
            to.rule.self.OnHit -= hit;

            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 6;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject waterg = GlobalData.get_water_effect;
            waterg.transform.SetParent(objectHit.transform, false);
            waterg.transform.localPosition = Vector3.zero;
            waterg.name = "WaterEffect";
            Object.Destroy(waterg, 6);
            var water = waterg.GetComponent<ParticleSystem>().shape;
            water.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    public class C_Buff_Energy : BuffModule
    {
        public C_Buff_Energy(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Energy)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._MoveSpeed += slow_move;
            to.rule.buffs._RotateSpeed += slow_move;
            to.rule.buffs._TensionSpeed += slow_move;
        }
        float slow_move(ref float arg)
        {
            arg *= 0f;
            return 1;
        }

        float time = 0.2f;
        float temporary_time = 10;
        public override void Update()
        {
            base.Update();
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Destroy();
            }
        }
        public override void Destroy()
        {
            to.rule.buffs._MoveSpeed -= slow_move;
            to.rule.buffs._RotateSpeed -= slow_move;
            to.rule.buffs._TensionSpeed -= slow_move;
            to.buff_modules.Remove(this);
        }

        public override void EffectUpdate()
        {
            if (effect_timer > 0)
            {
                effect_timer -= Time.deltaTime;
                return;
            }
            effect_timer = 1f;
            if (!objectHit.GetComponent<SpriteRenderer>())
                return;
            GameObject energyg = GlobalData.get_energy_effect;
            energyg.transform.SetParent(objectHit.transform, false);
            energyg.transform.localPosition = Vector3.zero;
            energyg.name = "EnergyEffect";
            Object.Destroy(energyg, 1);
            var energy = energyg.GetComponent<ParticleSystem>().shape;
            energy.spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
        }
    }
    //rework
    public class C_Buff_Bash : BuffModule
    {
        public C_Buff_Bash(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Bash)
                {
                    to.buff_modules[i].Destroy();
                    break;
                }
            to.buff_modules.Add(this);
            to.rule.buffs._MoveSpeed += slow_move;
            to.rule.buffs._RotateSpeed += slow_move;
        }
        float slow_move(ref float arg)
        {
            arg *= 0f;
            return 1;
        }

        float time = 0.2f;
        float temporary_time = 10;
        public override void Update()
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                Destroy();
            }
        }
        public override void Destroy()
        {
            to.rule.buffs._MoveSpeed -= slow_move;
            to.rule.buffs._RotateSpeed -= slow_move;
            to.buff_modules.Remove(this);
        }
    }
    //
    public class C_Buff_Aoe : BuffModule
    {
        public C_Buff_Aoe(Unit to, Unit from, GameObject objectHit) : base(to, from, objectHit)
        {
            for (int i = 0; i < to.buff_modules.Count; i++)
                if (to.buff_modules[i] is C_Buff_Aoe)
                {
                    return;
                }
            to.buff_modules.Add(this);
            to.rule.self.OnHit += hit;
        }

        bool hit(object[] args)
        {
            var a = (UnitHitInfo)args[0];
            if (a.DamageType != 7)
                return true;
            RaycastHit2D[] ret = Physics2D.CircleCastAll(to.rule.data.transform.position, 5f, Vector2.right, 0.01f, LayerMask.GetMask("Unit"));
            List<UnitHolder> holders = new List<UnitHolder>();
            foreach (var b in ret)
            {
                if (!b.collider.GetComponent<UnitHolder>())
                    continue;
                if (b.collider.GetComponent<UnitHolder>().unit == to)
                    continue;
                holders.Add(b.collider.GetComponent<UnitHolder>());
            }

            foreach (var b in holders)
            {
                b.unit.rule.self.Hurt = new UnitHitInfo()
                {
                    Damage = a.Damage / (float)ret.Length,
                    DamageType = 5,
                    from = this.from.rule,
                    directional = a.directional,
                    from_pos = a.from_pos,
                    hit_pos = a.hit_pos,
                    OriginalDamage = a.OriginalDamage / (float)ret.Length,
                    same_tag = a.same_tag,
                    crit_name = a.crit_name,
                    hit_object = a.hit_object
                };
            }
            Destroy();

            return true;
        }
        
        public override void Update()
        {
            base.Update();
            return;
        }
        public override void Destroy()
        {
            to.rule.self.OnHit -= hit;

            to.buff_modules.Remove(this);
        }
    }
}