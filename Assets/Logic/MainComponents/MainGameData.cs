using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainGameData
{
    public delegate bool main_delegate(object[] args);
    public delegate float mainFloat_delegate(ref float arg);

    public class UnitRule
    {
        public Data data;
        public PreferableSystem preferableSystem;
        public RequireSystem requireSystem;
        public Abilities abilities;
        public Self self;
        public Clothes clothes;
        public CustomPreference bonus;
        public CustomPreference buffs;

        public UnitRule(GameObject mainGameObject, InitialAbilities initialAbilities, string tag)
        {
            data = new Data()
            {
                transform = mainGameObject.transform,
                gameObject = mainGameObject,
                rigidbody = mainGameObject.GetComponent<Rigidbody2D>(),
                animator = mainGameObject.GetComponentInChildren<Animator>(),
                initialAbilities = initialAbilities,
                tag = tag
            };
            preferableSystem = new PreferableSystem();
            requireSystem = new RequireSystem();
            abilities = new Abilities(data.initialAbilities);
            self = new Self(data.initialAbilities)
            {
                tag = tag
            };
            clothes = new Clothes();
            bonus = new CustomPreference();
            buffs = new CustomPreference();
        }

        public class PreferableSystem
        {
            public Vector3 preferable_move_direction;
            public Vector3 preferable_look_angle;
            public Vector3 preferable_look_pos;
        }
        public class RequireSystem
        {
            public Vector3 require_apply_force;
            public Vector3 aim_directional;
            public Vector3 aim_original;
            public Vector3 aim_original_offset;
            public Transform pick_weapon_parent;
        }
        public class Data
        {
            public Transform transform;
            public GameObject gameObject;
            public Rigidbody2D rigidbody;
            public InitialAbilities initialAbilities;
            public Animator animator;
            public string tag;
        }
        public class Abilities
        {
            public float speed;

            public Abilities (InitialAbilities initialAbilities)
            {
                speed = initialAbilities.staticValue.Speed;
            }
        }
        public class Self
        {
            public float Health;
            public float Stamina;
            public int Level;
            public string tag;
            public Unit unit;

            public Self(InitialAbilities initialAbilities)
            {
                Health = initialAbilities.variableValue.MaxHealth;
                Stamina = initialAbilities.variableValue.MaxStamina;
                Level = 1;
            }

            public UnitHitInfo Hurt
            {
                set
                {
                    if (value.same_tag == tag)
                        return;
                    if (Health <= 0)
                        return;
                    RefreshDamage(ref value.Damage, value.DamageType);
                    Health -= value.Damage *
                        (value.crit_name == "15crit" ? 1.5f : 
                        (value.crit_name == "20crit" ? 2f : 
                        (value.crit_name == "01crit" ? 0.1f : 1f)));
                    value.CritDamage = value.Damage *
                        (value.crit_name == "15crit" ? 1.5f :
                        (value.crit_name == "20crit" ? 2f :
                        (value.crit_name == "01crit" ? 0.1f : 1f)));
                    OnHit?.Invoke(new object[1] { value });
                    if (Health <= 0)
                    {
                        OnDie?.Invoke(new object[1] { value });
                        Health = 0;
                    }
                }
            }
            public float StaminaOut
            {
                set
                {
                    if (OnStaminaOut != null)
                        if (!OnStaminaOut(new object[1] { value }))
                            return;
                    Stamina -= value;
                    Stamina = Stamina < -0.1f ? -0.1f : Stamina;
                }
            }
            public float StaminaHeal
            {
                set
                {
                    if (OnStaminaHeal != null)
                        if (!OnStaminaHeal(new object[1] { value }))
                            return;
                    Stamina += value;
                    Stamina = Stamina > unit.rule.data.initialAbilities.variableValue.MaxStamina ? unit.rule.data.initialAbilities.variableValue.MaxStamina : Stamina;
                }
            }

            public void RefreshDamage(ref float Damage, int type)
            {
                float percent = 0;
                switch (type)
                {
                    case 0:
                        percent += unit.rule.clothes.Helmet.info.resist.ToxicResist;
                        percent += unit.rule.clothes.Body.info.resist.ToxicResist;
                        percent += unit.rule.clothes.Scapular.info.resist.ToxicResist;
                        percent += unit.rule.clothes.Bracer.info.resist.ToxicResist;
                        break;
                    case 2:
                        percent += unit.rule.clothes.Helmet.info.resist.EnergyResist;
                        percent += unit.rule.clothes.Body.info.resist.EnergyResist;
                        percent += unit.rule.clothes.Scapular.info.resist.EnergyResist;
                        percent += unit.rule.clothes.Bracer.info.resist.EnergyResist;
                        break;
                    case 3:
                        percent += unit.rule.clothes.Helmet.info.resist.FireResist;
                        percent += unit.rule.clothes.Body.info.resist.FireResist;
                        percent += unit.rule.clothes.Scapular.info.resist.FireResist;
                        percent += unit.rule.clothes.Bracer.info.resist.FireResist;
                        break;
                    case 4:
                        percent += unit.rule.clothes.Helmet.info.resist.LightResist;
                        percent += unit.rule.clothes.Body.info.resist.LightResist;
                        percent += unit.rule.clothes.Scapular.info.resist.LightResist;
                        percent += unit.rule.clothes.Bracer.info.resist.LightResist;
                        break;
                    case 5:
                        percent += unit.rule.clothes.Helmet.info.resist.PhysicalResist;
                        percent += unit.rule.clothes.Body.info.resist.PhysicalResist;
                        percent += unit.rule.clothes.Scapular.info.resist.PhysicalResist;
                        percent += unit.rule.clothes.Bracer.info.resist.PhysicalResist;
                        break;
                    case 6:
                        percent += unit.rule.clothes.Helmet.info.resist.DarkResist;
                        percent += unit.rule.clothes.Body.info.resist.DarkResist;
                        percent += unit.rule.clothes.Scapular.info.resist.DarkResist;
                        percent += unit.rule.clothes.Bracer.info.resist.DarkResist;
                        break;
                    case 7:
                        percent += unit.rule.clothes.Helmet.info.resist.SplashResist;
                        percent += unit.rule.clothes.Body.info.resist.SplashResist;
                        percent += unit.rule.clothes.Scapular.info.resist.SplashResist;
                        percent += unit.rule.clothes.Bracer.info.resist.SplashResist;
                        break;
                    case 8:
                        percent += unit.rule.clothes.Helmet.info.resist.WaterResist;
                        percent += unit.rule.clothes.Body.info.resist.WaterResist;
                        percent += unit.rule.clothes.Scapular.info.resist.WaterResist;
                        percent += unit.rule.clothes.Bracer.info.resist.WaterResist;
                        break;
                }
                Damage -= Damage * (percent / 100f);
                Damage = Damage <= 0 ? 0.01f : Damage;
                OnDamageRefreshing?.Invoke(ref Damage);
            }

            public event main_delegate OnDie;
            public event main_delegate OnHit;
            public event main_delegate OnStaminaOut;
            public event main_delegate OnStaminaHeal;
            public event mainFloat_delegate OnDamageRefreshing;
        }
        public class CustomPreference
        {
            public void ClearInvokes()
            {
                _AttackSpeed = null;
                _MoveSpeed = null;
                _ReloadSpeed = null;
                _RotateSpeed = null;
                _StaminaHealSpeed = null;
                _TensionSpeed = null;
            }

            public float Get_AttackSpeed()
            {
                float obj = 1;
                _AttackSpeed?.Invoke(ref obj);
                return obj;
            }
            public float Get_MoveSpeed()
            {
                float obj = 1;
                _MoveSpeed?.Invoke(ref obj);
                return obj;
            }
            public float Get_ReloadSpeed()
            {
                float obj = 1;
                _ReloadSpeed?.Invoke(ref obj);
                return obj;
            }
            public float Get_RotateSpeed()
            {
                float obj = 1;
                _RotateSpeed?.Invoke(ref obj);
                return obj;
            }
            public float Get_StaminaHealSpeed()
            {
                float obj = 1;
                _StaminaHealSpeed?.Invoke(ref obj);
                return obj;
            }
            public float Get_TensionSpeed()
            {
                float obj = 1;
                _TensionSpeed?.Invoke(ref obj);
                return obj;
            }

            public event mainFloat_delegate _AttackSpeed;
            public event mainFloat_delegate _MoveSpeed;
            public event mainFloat_delegate _ReloadSpeed;
            public event mainFloat_delegate _RotateSpeed;
            public event mainFloat_delegate _StaminaHealSpeed;
            public event mainFloat_delegate _TensionSpeed;
        }
        public class Clothes
        {
            public ItemInfoObject Helmet;
            public ItemInfoObject Body;
            public ItemInfoObject Scapular;
            public ItemInfoObject Bracer;

            public GameObject _helmet_objects;
            public GameObject _body_objects;
            public GameObject[] _scapular_objects = new GameObject[2];
            public GameObject[] _bracer_objects = new GameObject[2];

            public float Get_AidResist
            {
                get
                {
                    return
                        Helmet.info.resist.ToxicResist +
                        Body.info.resist.ToxicResist +
                        Scapular.info.resist.ToxicResist +
                        Bracer.info.resist.ToxicResist;
                }
            }
            public float Get_EnergyResist
            {
                get
                {
                    return
                        Helmet.info.resist.EnergyResist +
                        Body.info.resist.EnergyResist +
                        Scapular.info.resist.EnergyResist +
                        Bracer.info.resist.EnergyResist;
                }
            }
            public float Get_FireResist
            {
                get
                {
                    return
                        Helmet.info.resist.FireResist +
                        Body.info.resist.FireResist +
                        Scapular.info.resist.FireResist +
                        Bracer.info.resist.FireResist;
                }
            }
            public float Get_LightResist
            {
                get
                {
                    return
                        Helmet.info.resist.LightResist +
                        Body.info.resist.LightResist +
                        Scapular.info.resist.LightResist +
                        Bracer.info.resist.LightResist;
                }
            }
            public float Get_PhysicalResist
            {
                get
                {
                    return
                        Helmet.info.resist.PhysicalResist +
                        Body.info.resist.PhysicalResist +
                        Scapular.info.resist.PhysicalResist +
                        Bracer.info.resist.PhysicalResist;
                }
            }
            public float Get_DarkResist
            {
                get
                {
                    return
                        Helmet.info.resist.DarkResist +
                        Body.info.resist.DarkResist +
                        Scapular.info.resist.DarkResist +
                        Bracer.info.resist.DarkResist;
                }
            }
            public float Get_SplashResist
            {
                get
                {
                    return
                        Helmet.info.resist.SplashResist +
                        Body.info.resist.SplashResist +
                        Scapular.info.resist.SplashResist +
                        Bracer.info.resist.SplashResist;
                }
            }
            public float Get_WaterResist
            {
                get
                {
                    return
                        Helmet.info.resist.WaterResist +
                        Body.info.resist.WaterResist +
                        Scapular.info.resist.WaterResist +
                        Bracer.info.resist.WaterResist;
                }
            }
        }
    
        public event main_delegate OnSpecialShoot;
        public event main_delegate OnSpecialHit;
        public event main_delegate OnSpecialHitToUnit;
        public bool CallEvent(UnitEventType type, object[] args)
        {
            switch (type)
            {
                case UnitEventType.OnSpecialShoot:
                    if (OnSpecialShoot != null)
                        return OnSpecialShoot(args);
                    break;
                case UnitEventType.OnSpecialHit:
                    if (OnSpecialHit != null)
                        return OnSpecialHit(args);
                    break;
                case UnitEventType.OnSpecialHitToUnit:
                    if (OnSpecialHitToUnit != null)
                        return OnSpecialHitToUnit(args);
                    break;
            }
            return false;
        }
        public enum UnitEventType
        {
            OnSpecialShoot,
            OnSpecialHit,
            OnSpecialHitToUnit
        }
    }
    public class WeaponRule
    {
        public Data data;
        public PreferableSystem preferableSystem;
        public RequireSystem requireSystem;
        public Container container;
        public Condition condition;

        public WeaponRule(WeaponData.InstanceWeapon instanceWeapon, Animator animator, bool refresh_animator)
        {
            data = new Data()
            {
                transform = instanceWeapon.weapon.transform,
                gameObject = instanceWeapon.weapon,
                animator = animator,
                refresh_animator = refresh_animator,
                instanceWeapon = instanceWeapon,
                weaponObject = instanceWeapon.weaponObject
            };
            preferableSystem = new PreferableSystem();
            requireSystem = new RequireSystem();
            container = new Container();
            condition = new Condition();
        }
        public class PreferableSystem
        {
            public bool isTryShoot;
            public bool isAim;
            public bool isTension;
            public float Tension;
        }
        public class RequireSystem
        {
            public bool isReload;
            public bool isShoot;
            public bool isNeedToReload;
        }
        public class Data
        {
            public Transform transform;
            public GameObject gameObject;
            public Animator animator;
            public bool refresh_animator;
            public WeaponData.InstanceWeapon instanceWeapon;
            public WeaponObject weaponObject;
        }
        public class Container
        {
            public int Ammo = 1;
            public float Accuracy;

            public int RefreshAmmo
            {
                set
                {
                    Ammo -= value;
                    if (Ammo <= 0)
                        isOutOfAmmo?.Invoke(new object[0]);
                }
            }

            public event main_delegate isOutOfAmmo;
        }
        public class Condition
        {
            public bool ifReloading;
            public bool ifShooting;
            public bool ifIdle;
            public float ReloadAmount;
            public float FireReloadAmount;
        }

        public event main_delegate OnShoot;
        public enum WeaponCallbackType
        {
            OnShoot
        }
        public bool Callback(WeaponCallbackType type, object[] args)
        {
            switch (type)
            {
                case WeaponCallbackType.OnShoot:
                    if (OnShoot != null)
                        return OnShoot.Invoke(args);
                    break;
            }
            return false;
        }
    }
    public class ProjectileRule
    {
        public Data data;
        public Container container;

        public ProjectileRule(InitialProjectile initialProjectile)
        {
            data = new Data()
            {
                initialProjectile = initialProjectile,
                projectileObject = initialProjectile.projectileObject,
                transform = initialProjectile.projectile.transform,
                rigidbody = initialProjectile.projectile.GetComponent<Rigidbody2D>()
            };
            container = new Container()
            {
                initial_scale = initialProjectile.projectile.transform.localScale,
                lived_time = 0
            };
            container.stats = data.initialProjectile.projectileObject.stats;
        }

        public class Data
        {
            public InitialProjectile initialProjectile;
            public ProjectileData.ProjectileObject projectileObject;
            public Transform transform;
            public Rigidbody2D rigidbody;
        }
        public class Container
        {
            public Vector3 initial_scale;
            public float lived_time;
            public ProjectileData.ProjectileStats stats;
            public Projectile projectile;
        }

        public event main_delegate OnCollide;
        public enum ProjectileCallbackType
        {
            OnCollide
        }
        public bool Callback(ProjectileCallbackType type, object[] args)
        {
            switch (type)
            {
                case ProjectileCallbackType.OnCollide:
                    if (OnCollide != null)
                        return OnCollide.Invoke(args);
                    break;
            }
            return false;
        }
    }

    public abstract class Module
    {
        public UnitRule rule;

        public Module(UnitRule rule)
        {
            this.rule = rule;
        }

        public abstract void Update();
    }
    public abstract class WeaponModule
    {
        public UnitRule rule;
        public WeaponRule w_rule;

        public WeaponModule(UnitRule rule, WeaponRule w_rule)
        {
            this.rule = rule;
            this.w_rule = w_rule;
        }

        public abstract IEnumerator Shoot();
        public abstract IEnumerator Reload();
    }
    public abstract class ProjectileModule
    {
        public UnitRule rule;
        public ProjectileRule p_rule;

        public ProjectileModule(UnitRule rule, ProjectileRule p_rule)
        {
            this.rule = rule;
            this.p_rule = p_rule;
        }

        public abstract void Update();
    }
    public abstract class BonusModule
    {
        public UnitRule rule;
        public WeaponRule w_rule;

        public BonusModule(UnitRule rule, WeaponRule w_rule)
        {
            this.rule = rule;
            this.w_rule = w_rule;
        }

        public abstract void Update();
        public abstract void Destroy();
    }
    public abstract class BuffModule
    {
        public Unit to;
        public Unit from;
        public GameObject objectHit;

        public BuffModule(Unit to, Unit from, GameObject objectHit)
        {
            this.to = to;
            this.from = from;
            this.objectHit = objectHit;
        }

        public virtual void Update()
        {
            EffectUpdate();
        }
        public abstract void Destroy();

        public float effect_timer;
        public virtual void EffectUpdate()
        {

        }
    }

    public class Unit : MonoBehaviour
    {
        public List<Module> permanent_modules;
        public List<Module> temporary_modules;
        public List<BonusModule> bonus_modules;
        public List<BuffModule> buff_modules;
        public UnitRule rule;
        public WeaponRule w_rule;
        public MainWeapon weapon;
        public InventoryData.Inventory inventory;
        UnitHolder holder;

        public void Initiate(UnitRule rule, WeaponRule w_rule)
        {
            holder = gameObject.AddComponent<UnitHolder>();
            holder.ON(this);

            this.rule = rule;
            this.w_rule = w_rule;
            this.rule.self.unit = this;

            weapon = new MainWeapon(this);

            permanent_modules = new List<Module>();
            temporary_modules = new List<Module>();
            bonus_modules = new List<BonusModule>();
            buff_modules = new List<BuffModule>();

            inventory = new InventoryData.Inventory(this);

            StartCoroutine(apply_modules());
        }
        public void EquipItem(ItemInfoObject info, ItemType type)
        {
            foreach (var a in rule.data.transform.GetComponentsInChildren<Transform>())
            {
                if (a.name == "Head" && type == ItemType.Helmet)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;

                if (a.name == "Body" && type == ItemType.Body)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;

                if (a.name == "LeftArm" && type == ItemType.Scapular)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;
                if (a.name == "RightArm" && type == ItemType.Scapular)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;

                if (a.name == "LeftForeArm" && type == ItemType.Bracer)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;
                if (a.name == "RightForeArm" && type == ItemType.Bracer)
                    a.GetComponent<SpriteRenderer>().sprite = info.info.EquipSprite;
            }
            switch (type)
            {
                case ItemType.Helmet:
                    rule.clothes.Helmet = info;
                    break;
                case ItemType.Body:
                    rule.clothes.Body = info;
                    break;
                case ItemType.Scapular:
                    rule.clothes.Scapular = info;
                    break;
                case ItemType.Bracer:
                    rule.clothes.Bracer = info;
                    break;
            }
            apply_bonus_modules();
            apply_customObjects();
        }
        void apply_bonus_modules()
        {
            foreach (var a in bonus_modules)
                a.Destroy();
            bonus_modules.Clear();
            rule.bonus.ClearInvokes();

            if (rule.clothes.Helmet != null)
                bonus_modules.AddRange(Modules.BonusModulesControll.GetModulesByIDs(rule.clothes.Helmet.info.bonus, rule, w_rule));
            if (rule.clothes.Body != null)
                bonus_modules.AddRange(Modules.BonusModulesControll.GetModulesByIDs(rule.clothes.Body.info.bonus, rule, w_rule));
            if (rule.clothes.Scapular != null)
                bonus_modules.AddRange(Modules.BonusModulesControll.GetModulesByIDs(rule.clothes.Scapular.info.bonus, rule, w_rule));
            if (rule.clothes.Bracer != null)
                bonus_modules.AddRange(Modules.BonusModulesControll.GetModulesByIDs(rule.clothes.Bracer.info.bonus, rule, w_rule));
        }
        void apply_customObjects()
        {
            #region DESTROY
            if (rule.clothes._helmet_objects != null) Destroy(rule.clothes._helmet_objects);
            if (rule.clothes._body_objects != null) Destroy(rule.clothes._body_objects);

            if (rule.clothes._scapular_objects[0] != null) Destroy(rule.clothes._scapular_objects[0]);
            if (rule.clothes._scapular_objects[1] != null) Destroy(rule.clothes._scapular_objects[1]);

            if (rule.clothes._bracer_objects[0] != null) Destroy(rule.clothes._bracer_objects[0]);
            if (rule.clothes._bracer_objects[1] != null) Destroy(rule.clothes._bracer_objects[1]);
            #endregion

            foreach (var a in rule.data.transform.GetComponentsInChildren<Transform>())
            {
                if (a.name == "Head" && rule.clothes.Helmet?.info.child_object != null)
                {
                    rule.clothes._helmet_objects = Instantiate(rule.clothes.Helmet.info.child_object) as GameObject;
                    rule.clothes._helmet_objects.transform.SetParent(a, false);
                    rule.clothes._helmet_objects.transform.localPosition = Vector3.zero;
                    rule.clothes._helmet_objects.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._helmet_objects.transform.localScale = new Vector3(1, 1, 1);
                }

                if (a.name == "Body" && rule.clothes.Body?.info.child_object != null)
                {
                    rule.clothes._body_objects = Instantiate(rule.clothes.Body.info.child_object) as GameObject;
                    rule.clothes._body_objects.transform.SetParent(a, false);
                    rule.clothes._body_objects.transform.localPosition = Vector3.zero;
                    rule.clothes._body_objects.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._body_objects.transform.localScale = new Vector3(1, 1, 1);
                }

                if (a.name == "LeftArm" && rule.clothes.Scapular?.info.child_object != null)
                {
                    rule.clothes._scapular_objects[0] = Instantiate(rule.clothes.Scapular.info.child_object) as GameObject;
                    rule.clothes._scapular_objects[0].transform.SetParent(a, false);
                    rule.clothes._scapular_objects[0].transform.localPosition = Vector3.zero;
                    rule.clothes._scapular_objects[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._scapular_objects[0].transform.localScale = new Vector3(1, 1, 1);
                }
                if (a.name == "RightArm" && rule.clothes.Scapular?.info.child_object != null)
                {
                    rule.clothes._scapular_objects[1] = Instantiate(rule.clothes.Scapular.info.child_object) as GameObject;
                    rule.clothes._scapular_objects[1].transform.SetParent(a, false);
                    rule.clothes._scapular_objects[1].transform.localPosition = Vector3.zero;
                    rule.clothes._scapular_objects[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._scapular_objects[1].transform.localScale = new Vector3(1, 1, 1);
                }

                if (a.name == "LeftForeArm" && rule.clothes.Bracer?.info.child_object != null)
                {
                    rule.clothes._bracer_objects[0] = Instantiate(rule.clothes.Bracer.info.child_object) as GameObject;
                    rule.clothes._bracer_objects[0].transform.SetParent(a, false);
                    rule.clothes._bracer_objects[0].transform.localPosition = Vector3.zero;
                    rule.clothes._bracer_objects[0].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._bracer_objects[0].transform.localScale = new Vector3(1, 1, 1);
                }
                if (a.name == "RightForeArm" && rule.clothes.Bracer?.info.child_object != null)
                {
                    rule.clothes._bracer_objects[1] = Instantiate(rule.clothes.Bracer.info.child_object) as GameObject;
                    rule.clothes._bracer_objects[1].transform.SetParent(a, false);
                    rule.clothes._bracer_objects[1].transform.localPosition = Vector3.zero;
                    rule.clothes._bracer_objects[1].transform.localRotation = Quaternion.Euler(0, 0, 0);
                    rule.clothes._bracer_objects[1].transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        public virtual void Apply_Permanent_Modules()
        {
            for (int i = 0; i < permanent_modules.Count; i++)
            {
                permanent_modules[i].Update();
            }
        }
        public virtual void Apply_Temporary_Modules()
        {
            for (int i = 0; i < temporary_modules.Count; i++)
            {
                temporary_modules[i].Update();
            }
        }
        public virtual void Apply_Bonus_Modules()
        {
            for (int i = 0; i < bonus_modules.Count; i++)
            {
                bonus_modules[i].Update();
            }
        }
        public virtual void Apply_Buff_Modules()
        {
            for (int i = 0; i < buff_modules.Count; i++)
            {
                buff_modules[i].Update();
            }
        }

        public virtual IEnumerator apply_modules()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                Apply_Permanent_Modules();
                Apply_Temporary_Modules();
                Apply_Bonus_Modules();
                Apply_Buff_Modules();
            }
        }
    }
    public class UnitHolder : MonoBehaviour
    {
        public Unit unit;

        public void ON(Unit unit)
        {
            this.unit = unit;
        }
    }
    public class MainWeapon
    {
        public List<WeaponModule> permanent_modules;
        public List<WeaponModule> temporary_modules;
        public MonoBehaviour mono;
        public Unit unit;

        public MainWeapon(Unit unit)
        {
            this.unit = unit;
            PickUpWeapon();
            mono = unit.GetComponent<MonoBehaviour>();
            mono.StartCoroutine(Shoot());
            mono.StartCoroutine(Tension());
        }

        public void PickUpWeapon()
        {
            unit.w_rule = new WeaponRule(unit.w_rule.data.instanceWeapon, unit.w_rule.data.animator, unit.w_rule.data.refresh_animator);

            permanent_modules = new List<WeaponModule>();
            permanent_modules.AddRange(Modules.WeaponModulesControll.GetModulesByIDs(
                unit.w_rule.data.weaponObject.stats.listOfInitiatePermanentModules,
                unit.rule,
                unit.w_rule
                ));

            temporary_modules = new List<WeaponModule>();
            temporary_modules.AddRange(Modules.WeaponModulesControll.GetModulesByIDs(
                unit.w_rule.data.weaponObject.stats.listOfInitiateTemporaryModules,
                unit.rule,
                unit.w_rule
                ));
            unit.w_rule.container.isOutOfAmmo += (args) =>
            {
                mono.StartCoroutine(Reload());
                return true;
            };
        }
        public void PickUpWeapon(WeaponData.InstanceWeapon instanceWeapon)
        {
            Object.Destroy(unit.w_rule.data.instanceWeapon.weapon);
            instanceWeapon.weapon.transform.SetParent(unit.rule.requireSystem.pick_weapon_parent, false);
            instanceWeapon.weapon.transform.localPosition = new Vector3(0, 0, 0);

            unit.w_rule = new WeaponRule(instanceWeapon, unit.w_rule.data.animator, unit.w_rule.data.refresh_animator);

            permanent_modules = new List<WeaponModule>();
            permanent_modules.AddRange(Modules.WeaponModulesControll.GetModulesByIDs(
                unit.w_rule.data.weaponObject.stats.listOfInitiatePermanentModules,
                unit.rule,
                unit.w_rule
                ));

            temporary_modules = new List<WeaponModule>();
            temporary_modules.AddRange(Modules.WeaponModulesControll.GetModulesByIDs(
                unit.w_rule.data.weaponObject.stats.listOfInitiateTemporaryModules,
                unit.rule,
                unit.w_rule
                ));
            unit.w_rule.container.isOutOfAmmo += (args) =>
            {
                mono.StartCoroutine(Reload());
                return true;
            };
        }

        public IEnumerator Shoot()
        {
            while (true)
            {
                if (unit.w_rule.preferableSystem.isTryShoot && unit.w_rule.preferableSystem.Tension > 0)
                {
                    for (int i = 0; i < permanent_modules.Count; i++)
                        yield return mono.StartCoroutine(permanent_modules[i].Shoot());
                    for (int i = 0; i < temporary_modules.Count; i++)
                        yield return mono.StartCoroutine(temporary_modules[i].Shoot());
                    yield return new WaitForSeconds(0.1f);
                    unit.w_rule.preferableSystem.isTryShoot = false;
                    unit.w_rule.preferableSystem.Tension = 0;
                }
                else
                    yield return null;
            }
        }
        public IEnumerator Reload()
        {
            while (unit.w_rule.condition.ifShooting)
            {
                yield return null;
            }

            unit.w_rule.requireSystem.isReload = true;
            unit.w_rule.container.Ammo = 0;

            for (int i = 0; i < permanent_modules.Count; i++)
                yield return mono.StartCoroutine(permanent_modules[i].Reload());
            for (int i = 0; i < temporary_modules.Count; i++)
                yield return mono.StartCoroutine(temporary_modules[i].Reload());

            unit.w_rule.requireSystem.isReload = false;
        }
        public IEnumerator Tension()
        {
            while (true)
            {
                if (unit.w_rule.preferableSystem.isTension && unit.w_rule.condition.ifIdle)
                {
                    unit.w_rule.preferableSystem.Tension +=
                             unit.w_rule.data.weaponObject.stats.TensionPerSecond
                             * Time.deltaTime
                             * unit.rule.buffs.Get_TensionSpeed()
                             * unit.rule.bonus.Get_TensionSpeed();
                    unit.w_rule.preferableSystem.Tension =
                          unit.w_rule.preferableSystem.Tension >= 1 ? 1 : unit.w_rule.preferableSystem.Tension;
                }
                else
                {
                    unit.w_rule.preferableSystem.Tension = 0;
                    /*unit.w_rule.preferableSystem.Tension -=
                            unit.w_rule.data.weaponObject.stats.TensionPerSecond
                            * Time.deltaTime
                            * 15f;
                    unit.w_rule.preferableSystem.Tension =
                        unit.w_rule.preferableSystem.Tension <= 0 ? 0 : unit.w_rule.preferableSystem.Tension;*/
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
    public class Projectile : MonoBehaviour
    {
        public List<ProjectileModule> modules;
        public List<ProjectileModule> temporary_modules;
        public UnitRule rule;
        public ProjectileRule p_rule;

        public void Initiate(UnitRule rule, ProjectileRule p_rule)
        {
            this.rule = rule;
            this.p_rule = p_rule;
            p_rule.container.projectile = this;

            temporary_modules = new List<ProjectileModule>();
            modules = new List<ProjectileModule>();
            modules.AddRange(Modules.ProjectileModulesControll.GetModulesByIDs(
                p_rule.data.projectileObject.stats.listOfInitiateModules,
                this.rule,
                this.p_rule
                ));

            StartCoroutine(apply_modules());
            StartCoroutine(rescale());
        }

        public virtual void Apply_Permanent_Modules()
        {
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Update();
            }
        }
        public virtual void Apply_Temporary_Modules()
        {
            for (int i = 0; i < temporary_modules.Count; i++)
            {
                temporary_modules[i].Update();
            }
        }

        public virtual IEnumerator apply_modules()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();
                Apply_Permanent_Modules();
                Apply_Temporary_Modules();
            }
        }

        IEnumerator rescale()
        {
            float time = 0;

            while (time < 1)
            {
                time += (1f / p_rule.data.projectileObject.animation.rescale_time) * Time.deltaTime;
                p_rule.data.transform.localScale =
                    new Vector3(0.0001f, 0.0001f, 0.0001f) * (1f - time) + time * p_rule.container.initial_scale;
                yield return null;
            }
            p_rule.data.transform.localScale = p_rule.container.initial_scale;
        }
    }

    public class UnitHitInfo
    {
        public float Damage;
        public float CritDamage;
        public float OriginalDamage;
        public UnitRule from;
        public int DamageType;
        public Vector3 directional;
        public Vector3 from_pos;
        public Vector3 hit_pos;
        public string same_tag;
        public string crit_name;
        public GameObject hit_object;
    }
    public class WorldLevelUp
    {
        public int DamageLoss_Level;
        public int AttackSpeedLoss_Level;
        public int HealthLoss_Level;
    }

    public enum ItemType
    {
        Helmet,
        Scapular,
        Bracer,
        Pants,
        Shoes,
        Body,
        Weapon
    }
}
