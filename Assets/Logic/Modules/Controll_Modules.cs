using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;

namespace Modules
{
    public class C_Movement : Module
    {
        public C_Movement(UnitRule rule) : base(rule) { }

        public override void Update()
        {
            Vector3 dir = rule.preferableSystem.preferable_move_direction;
            dir = dir.sqrMagnitude > 1 ? dir.normalized : dir;

            rule.data.rigidbody.MovePosition(
                rule.data.transform.position + dir * rule.abilities.speed * rule.bonus.Get_MoveSpeed() * rule.buffs.Get_MoveSpeed()
                );
        }
    }
    public class C_FPSCamera_Y : Module
    {
        public C_FPSCamera_Y(UnitRule rule) : base(rule) { }

        public override void Update()
        {
            rule.data.transform.rotation *= Quaternion.Euler(
                rule.data.transform.rotation.eulerAngles.x,
                rule.data.transform.rotation.eulerAngles.y,
                rule.preferableSystem.preferable_look_angle.z
                );
        }
    }
    public class C_SlowLook : Module
    {
        public C_SlowLook(UnitRule rule) : base(rule) { }

        public override void Update()
        {
            rule.data.transform.rotation = Quaternion.Lerp(
                rule.data.transform.rotation,
                rule.data.transform.rotation * Quaternion.Euler(
                    rule.data.transform.rotation.eulerAngles.x,
                    rule.data.transform.rotation.eulerAngles.y,
                    rule.preferableSystem.preferable_look_angle.z
                ), 
                8f * Time.deltaTime);
        }
    }
    public class C_LookAt : Module
    {
        public C_LookAt(UnitRule rule) : base(rule) { }

        public override void Update()
        {
            Vector3 dir = rule.preferableSystem.preferable_look_pos - rule.data.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rule.data.transform.rotation = Quaternion.Lerp(
                rule.data.transform.rotation,
                Quaternion.Euler(
                    0,
                    0,
                    angle
                ),
                rule.data.initialAbilities.staticValue.RotateSpeed * Time.deltaTime * rule.bonus.Get_RotateSpeed() * rule.buffs.Get_RotateSpeed());
        }
    }
    public class C_Animation : Module
    {
        public C_Animation(UnitRule rule) : base(rule) { }

        public override void Update()
        {
            Vector3 dir = rule.data.transform.InverseTransformDirection(
                rule.preferableSystem.preferable_move_direction) * rule.data.initialAbilities.staticValue.Speed
                * rule.data.animator.GetFloat("MoveSpeed");

            rule.data.animator.SetFloat("Horizontal", dir.y);
            rule.data.animator.SetFloat("Vertical", dir.x);
        }
    }
    public class C_BasicStaminaHeal : Module
    {
        public C_BasicStaminaHeal(UnitRule rule) : base(rule)
        {
            rule.self.OnStaminaOut += (args) =>
            {
                timer = 1.4f;
                return true;
            };
        }

        float timer = 0;
        public override void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                return;
            }
            if (!rule.self.unit.w_rule.preferableSystem.isTension)
                rule.self.StaminaHeal = 0.006f * rule.bonus.Get_StaminaHealSpeed() * rule.buffs.Get_StaminaHealSpeed();
            else
                rule.self.StaminaHeal = 0.006f * rule.bonus.Get_StaminaHealSpeed() * rule.buffs.Get_StaminaHealSpeed() * rule.self.unit.w_rule.data.weaponObject.stats.StaminaSpeed_WhileTension;
        }
    }
    public class C_PhysicalHitReact : Module
    {
        Transform _head;
        Transform _body;
        Transform _leftArm;
        Transform _rightArm;
        Transform _leftForeArm;
        Transform _rightForeArm;

        Quaternion _head_original_rotate;
        Quaternion _body_original_rotate;
        Quaternion _leftArm_original_rotate;
        Quaternion _rightArm_original_rotate;
        Quaternion _leftForeArm_original_rotate;
        Quaternion _rightForeArm_original_rotate;

        Vector3 _head_original_addDirection;
        Vector3 _body_original_addDirection;
        Vector3 _leftArm_original_addDirection;
        Vector3 _rightArm_original_addDirection;
        Vector3 _leftForeArm_original_addDirection;
        Vector3 _rightForeArm_original_addDirection;

        public C_PhysicalHitReact(UnitRule rule) : base(rule) 
        {
            //initiate();
            //rule.self.unit.StartCoroutine(apply_additionalDirection());
            //rule.self.unit.StartCoroutine(compile_originalRotation());
            //rule.self.OnHit += e;
        }
        void initiate()
        {
            foreach (var a in rule.data.transform.GetComponentsInChildren<Transform>())
            {
                if (a.name == "Head") _head = a;

                if (a.name == "Body") _body = a;

                if (a.name == "LeftArm") _leftArm = a;
                if (a.name == "RightArm") _rightArm = a;

                if (a.name == "LeftForeArm") _leftForeArm = a;
                if (a.name == "RightForeArm") _rightForeArm = a;
            }
        }
        bool e(object[] args)
        {
            rotateBone((UnitHitInfo)args[0]);

            return true;
        }

        public override void Update()
        {
        }

        IEnumerator compile_originalRotation()
        {
            while (true)
            {
                yield return null;
                refresh_additionalDirectional();

                _head_original_rotate = _head.rotation;
                _body_original_rotate = _body.rotation;
                _leftArm_original_rotate = _leftArm.rotation;
                _rightArm_original_rotate = _rightArm.rotation;
                _leftForeArm_original_rotate = _leftForeArm.rotation;
                _rightForeArm_original_rotate = _rightForeArm.rotation;
                
            }
        }
        void refresh_additionalDirectional()
        {
            _head_original_addDirection = Vector3.Lerp(_head_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
            _body_original_addDirection = Vector3.Lerp(_body_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
            _leftArm_original_addDirection = Vector3.Lerp(_leftArm_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
            _rightArm_original_addDirection = Vector3.Lerp(_rightArm_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
            _leftForeArm_original_addDirection = Vector3.Lerp(_leftForeArm_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
            _rightForeArm_original_addDirection = Vector3.Lerp(_rightForeArm_original_addDirection, Vector3.zero, 0.05f * Time.deltaTime);
        }
        IEnumerator apply_additionalDirection()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                _head.rotation = Quaternion.Lerp(
                    _head.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_head_original_addDirection.y, _head_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
                _body.rotation = Quaternion.Lerp(
                    _body.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_body_original_addDirection.y, _body_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
                _leftArm.rotation = Quaternion.Lerp(
                    _leftArm.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_leftArm_original_addDirection.y, _leftArm_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
                _rightArm.rotation = Quaternion.Lerp(
                    _rightArm.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_rightArm_original_addDirection.y, _rightArm_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
                _leftForeArm.rotation = Quaternion.Lerp(
                    _leftForeArm.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_leftForeArm_original_addDirection.y, _leftForeArm_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
                _rightForeArm.rotation = Quaternion.Lerp(
                    _rightForeArm.rotation,
                    Quaternion.Euler(0, 0, Mathf.Atan2(_rightForeArm_original_addDirection.y, _rightForeArm_original_addDirection.x) * Mathf.Rad2Deg),
                    12f * Time.deltaTime);
            }
        }

        void rotateBone(UnitHitInfo info)
        {
            if (info.hit_object == _head.gameObject)
                _head_original_addDirection = (info.directional + _head.right).normalized;

            if (info.hit_object == _body.gameObject)
                _body_original_addDirection = (info.directional + _body.right).normalized;

            if (info.hit_object == _leftArm.gameObject)
                _leftArm_original_addDirection = (info.directional + _leftArm.right).normalized;

            if (info.hit_object == _rightArm.gameObject)
                _rightArm_original_addDirection = (info.directional + _rightArm.right).normalized;

            if (info.hit_object == _leftForeArm.gameObject)
                _leftForeArm_original_addDirection = (info.directional + _leftForeArm.right).normalized;

            if (info.hit_object == _rightForeArm.gameObject)
                _rightForeArm_original_addDirection = (info.directional + _rightForeArm.right).normalized;
        }
    }

    //rework
    public class C_AnimatorHitWork : Module
    {
        public C_AnimatorHitWork(UnitRule rule) : base(rule)
        {
            rule.self.OnHit += (args) =>
            {
                if (timer > 0)
                    return true;
                timer = Random.Range(0.3f, 1.5f);
                var hitInfo = (UnitHitInfo)args[0];
                Vector3 rH = new Vector3();
                Vector3 lH = new Vector3();
                foreach (var a in rule.data.gameObject.GetComponentsInChildren<Transform>())
                {
                    if (a.name == "_RightHitWork") rH = a.position;
                    if (a.name == "_LeftHitWork") lH = a.position;
                }
                rule.data.animator.SetTrigger("isHit");
                float rD = Vector3.Distance(rH, hitInfo.hit_pos);
                float lD = Vector3.Distance(lH, hitInfo.hit_pos);
                if (Mathf.Abs(rD - lD) < 0.1f)
                {
                    rule.data.animator.SetFloat("HitType", 0);
                    return true;
                }
                if (rD < lD)
                    rule.data.animator.SetFloat("HitType", 1);
                else
                    rule.data.animator.SetFloat("HitType", 2);
                return true;
            };
        }

        public float timer = 0;
        public override void Update()
        {
            timer -= timer > 0 ? Time.deltaTime : 0;
        }
    }

    public static class ControllModulesControll
    {
        public static Module GetModuleByID(ListOfControllModules ID, UnitRule rule)
        {
            switch (ID)
            {
                case ListOfControllModules.Movement:
                    return new C_Movement(rule);
                case ListOfControllModules.FPSCamera_Y:
                    return new C_FPSCamera_Y(rule);
                case ListOfControllModules.Animation:
                    return new C_Animation(rule);
                case ListOfControllModules.AnimatorHitWork:
                    return new C_AnimatorHitWork(rule);
                case ListOfControllModules.SlowLook:
                    return new C_SlowLook(rule);
                case ListOfControllModules.LookAt:
                    return new C_LookAt(rule);
                case ListOfControllModules.BasicStaminaHeal:
                    return new C_BasicStaminaHeal(rule);
                case ListOfControllModules.PhysicalHitReact:
                    return new C_PhysicalHitReact(rule);
            }

            return null;
        }
        public static Module[] GetModulesByIDs(ListOfControllModules[] IDs, UnitRule rule)
        {
            Module[] ret = new Module[IDs.Length];
            for (int i = 0; i < ret.Length; i++)
                ret[i] = GetModuleByID(IDs[i], rule);
            return ret;
        }
    }
    public enum ListOfControllModules
    {
        Movement,
        FPSCamera_Y,
        Animation,
        AnimatorHitWork,
        SlowLook,
        LookAt,
        BasicStaminaHeal,
        PhysicalHitReact
    }
}
