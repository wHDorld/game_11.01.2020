using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponData
{
    [System.Serializable]
    public class WeaponStats
    {
        public float RateOfFire;
        public float ReloadTime; //depricated
        public float TensionPerSecond;
        public int MaxAmmo;
        public float DamageMultiply;
        public float MaxDistance;
        public int Penetrating;
        public int Rarity;
        public float Stamina_PerShot;
        public float StaminaSpeed_WhileTension;

        public Object shooting_object;
        public InitialProjectile shooting_object_info;
        public Object shooting_effect;
        public float shooting_effect_live;

        public Modules.ListOfWeaponModules[] listOfInitiatePermanentModules;
        public Modules.ListOfWeaponModules[] listOfInitiateTemporaryModules;
    }

    [System.Serializable]
    public class InstanceWeapon
    {
        public GameObject weapon;
        public WeaponObject weaponObject;
    }

    public class WeaponAnimatorHandler
    {
        public MainGameData.Unit unit;

        public WeaponAnimatorHandler(MainGameData.Unit unit)
        {
            this.unit = unit;
            ShootType = Animator.StringToHash("ShootType");
            isShoot_Hash = Animator.StringToHash("isShoot");
            isReload_Hash = Animator.StringToHash("isReload");
            Tension = Animator.StringToHash("Tension");
        }

        public void Update()
        {
            ConditionUpdate();
            BooleanUpdate();
        }

        public void ConditionUpdate()
        {
            unit.w_rule.condition.ifShooting =
                unit.w_rule.data.animator.GetCurrentAnimatorStateInfo(0).IsTag("SHOOT")
                || unit.w_rule.requireSystem.isShoot;
            unit.w_rule.condition.ifReloading =
                unit.w_rule.data.animator.GetCurrentAnimatorStateInfo(0).IsTag("RELOAD")
                || unit.w_rule.requireSystem.isReload;
            unit.w_rule.condition.ifIdle =
                unit.w_rule.data.animator.GetCurrentAnimatorStateInfo(0).IsTag("IDLE");
        }

        public void BooleanUpdate()
        {
            unit.w_rule.data.animator.SetBool(isShoot_Hash,
                unit.w_rule.requireSystem.isShoot);
            unit.w_rule.requireSystem.isShoot = false;

            unit.w_rule.data.animator.SetBool(isReload_Hash,
                unit.w_rule.requireSystem.isReload);

            unit.w_rule.data.animator.SetFloat(Tension,
                unit.w_rule.preferableSystem.Tension);
        }

        #region HASH
        public int isShoot_Hash;
        public int isReload_Hash;
        public int ShootType;
        public int Tension;
        #endregion
    }
}
