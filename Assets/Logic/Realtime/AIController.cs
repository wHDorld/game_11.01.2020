using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using Modules;
using WeaponData;
using ArtifitialIntelligenceData;

public class AIController : Unit
{
    public StealthSettings stealth;

    public string weapon_path;
    public string helmet_path;
    public string body_path;
    public string bracer_path;
    public string scapular_path;

    public Transform pick_weapon_parent;
    public AIRule iRule;
    public AI_Object ai_object;
    public InstanceWeapon instanceWeapon;
    public WeaponAnimatorHandler weaponAnimatorHandler;
    public Transform aim;
    public SFLight underLight;

    Transform player;
    PlayerController playerController;
    AI_Behaviour_Struct current_behaviour;
    EnemyInfoBar infoBar;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = GameObject.FindObjectOfType<PlayerController>();
        iRule = new AIRule(ai_object);
        Initiate(new UnitRule(gameObject, iRule.data.ai.initiateStat.initialAbilities, ai_object.TAG), new WeaponRule(instanceWeapon, pick_weapon_parent.GetComponentInChildren<Animator>(), true));
        rule.requireSystem.pick_weapon_parent = pick_weapon_parent;
        rule.preferableSystem.preferable_look_pos = transform.position + transform.right;
        weaponAnimatorHandler = new WeaponAnimatorHandler(this);
        permanent_modules.AddRange(ControllModulesControll.GetModulesByIDs(iRule.data.ai.initiateStat.initial_controll_modules, rule));
        current_behaviour = ai_object.default_behaviour;
        initiate_inventory();
        inventory.Equip();
        StartCoroutine(random_move());
        StartCoroutine(basic_behaviour());

        initiate_bar();
        initiate_events();

        FlashLight_Switch();
        Light_Switch();
        rule.preferableSystem.preferable_look_angle = transform.eulerAngles;

        iRule.container.ApplyViewGauge(this);
    }

    #region INITIATE
    void initiate_inventory()
    {
        InventoryData.Weapon_Item weapon = new InventoryData.Weapon_Item();
        weapon.Generate(weapon_path);
        weapon.isEquiped = true;
        inventory.Add(weapon);

        InventoryData.Helmet_Item helmet = new InventoryData.Helmet_Item();
        helmet.Generate(helmet_path);
        helmet.isEquiped = true;
        inventory.Add(helmet);

        InventoryData.Body_Item body = new InventoryData.Body_Item();
        body.Generate(body_path);
        body.isEquiped = true;
        inventory.Add(body);

        InventoryData.Bracer_Item bracer = new InventoryData.Bracer_Item();
        bracer.Generate(bracer_path);
        bracer.isEquiped = true;
        inventory.Add(bracer);

        InventoryData.Scapular_Item scapular = new InventoryData.Scapular_Item();
        scapular.Generate(scapular_path);
        scapular.isEquiped = true;
        inventory.Add(scapular);
    }
    void initiate_events()
    {
        w_rule.OnShoot += (args) =>
        {
            iRule.container.RefreshActualShoot = 1;

            return true;
        };
        iRule.container.isOutOfActualShoot += (args) =>
        {
            StartCoroutine(pause_reload());

            return true;
        };
        rule.self.OnDie += (args) =>
        {
            StopAllCoroutines();
            OnAngry -= Event_OnAngry;
            this.enabled = false;
            return true;
        };
        rule.self.OnHit += (args) =>
        {
            if (rule.self.Health <= 0)
                return true;

            iRule.container.last_player_pos = player.position;
            if (iRule.self.isAngry)
                return true;
            iRule.self.view_gauge = 1;
            iRule.container.stealth.UPD();
            iRule.self.isAngry = true;
            stealth_timer = 1;

            group_alarm();
            return true;
        };
        rule.self.OnDamageRefreshing += (ref float arg) =>
        {
            if (iRule.self.isAngry)
                return arg;
            arg *= 2f;
            return arg;
        };

        OnAngry += Event_OnAngry;
    }
    void Event_OnAngry(ref List<AIController> arg)
    {
        if (!iRule.self.isAngry)
            return;
        arg.Add(this);
    }
    void initiate_bar()
    {
        infoBar = (Instantiate(Resources.Load("EnemyInfoBar")) as GameObject).GetComponent<EnemyInfoBar>();
        infoBar.ON(this, iRule);
    }
    #endregion

    void Update()
    {
        distance_to_player = Vector2.Distance(transform.position, player.position);
        isPlayerInFOV = check_isPlayerInFOV;
        if (isPlayerInFOV)
            iRule.container.last_player_pos = player.position;
        distance_to_player_phantom = Vector2.Distance(transform.position, iRule.container.last_player_pos);
        weaponAnimatorHandler.Update();

        rule.requireSystem.aim_original = aim.position;
        rule.requireSystem.aim_directional = aim.right;
        S_Update();
        TryShoot();
        if (iRule.self.isAngry || iRule.self.isSuspicion)
        {
            RotateAim();
            Controll();
        }
        underLight_Controll();
    }

    #region CONTROLL
    void TryShoot()
    {
        w_rule.preferableSystem.isTryShoot = false;

        if (iRule.self.isAngry || iRule.self.isSuspicion)
            w_rule.preferableSystem.isTension = true;
        else
            w_rule.preferableSystem.isTension = false;

        if (!iRule.self.isAngry) return;
        if (isPauseReload) return;

        w_rule.preferableSystem.isTension = true;
        if (w_rule.preferableSystem.Tension >= 1)
            w_rule.preferableSystem.isTryShoot = true;
    }
    void RotateAim()
    {
        Vector3 local_dir = transform.InverseTransformDirection(iRule.container.last_player_pos - aim.position);
        float local_angle = Mathf.Atan2(local_dir.y, local_dir.x) * Mathf.Rad2Deg;
        local_angle = local_angle > 15 ? 15 : (local_angle < -15 ? -15 : local_angle);

        aim.localRotation =
            Quaternion.Euler(
                0,
                0,
                local_angle +
                    Random.Range(-iRule.data.ai.initiateStat.random_aim_angle, iRule.data.ai.initiateStat.random_aim_angle)
            );
    }
    void Controll()
    {
        if (w_rule.condition.ifIdle)
            rule.preferableSystem.preferable_look_pos = iRule.container.last_player_pos;
        else
            rule.preferableSystem.preferable_look_pos = rule.preferableSystem.preferable_look_pos;

        move = Vector3.Lerp(move,
            new Vector3(
            distance_to_player_phantom < current_behaviour.actual_distance[0] ? -1 :
                (distance_to_player_phantom > current_behaviour.actual_distance[1] ? 1 : random_move_dir.x),
            random_move_dir.y,
            0
            ),
            current_behaviour.actual_speed_use * Time.deltaTime * 3f);
        rule.preferableSystem.preferable_move_direction = transform.TransformDirection(move);
    }

    void underLight_Controll()
    {
        return;
        float to_p_dist = Mathf.Clamp(Vector2.Distance(transform.position, player.position) - 3, 0f, 6f) / 6f;

        if (iRule.self.isSuspicion)
            underLight.intensity = Mathf.Lerp(underLight.intensity, 0.5f * to_p_dist, 4f * Time.deltaTime);
        else if (iRule.self.isAngry)
            underLight.intensity = Mathf.Lerp(underLight.intensity, 0.8f * to_p_dist, 4f * Time.deltaTime);
        else
            underLight.intensity = Mathf.Lerp(underLight.intensity, 0f * to_p_dist, 4f * Time.deltaTime);
    }

    IEnumerator basic_behaviour()
    {
        while (true)
        {
            yield return null;
            while (iRule.self.isAngry || iRule.self.isSuspicion)
                yield return null;

            yield return StartCoroutine(basic_move_realize());
            yield return StartCoroutine(basic_rotate_realize());
        }
    }
    IEnumerator basic_move_realize()
    {
        rule.preferableSystem.preferable_move_direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
            ).normalized / 6f;
        float t = 0;
        float max_t = Random.Range(0.1f, 1f);
        while (t < max_t && !iRule.self.isAngry && !iRule.self.isSuspicion)
        {
            yield return null;
            t += Time.deltaTime;
            rule.preferableSystem.preferable_look_pos = transform.position + rule.preferableSystem.preferable_move_direction;
        }
        rule.preferableSystem.preferable_move_direction = new Vector3(0, 0, 0);
    }
    IEnumerator basic_rotate_realize()
    {
        float t = 0;
        float max_t = Random.Range(0.1f, 1f);
        int max_rotates = Random.Range(3, 20);

        for (int i = 0; i < max_rotates; i++)
        {
            if (iRule.self.isAngry || iRule.self.isSuspicion)
                break;
            rule.preferableSystem.preferable_look_pos = transform.position + transform.right + new Vector3(
                Random.Range(-0.8f, 0.8f),
                Random.Range(-0.8f, 0.8f)
                );

            t = 0;
            max_t = Random.Range(0.5f, 2.5f);
            while (t < max_t && !iRule.self.isAngry && !iRule.self.isSuspicion)
            {
                yield return null;
                t += Time.deltaTime;
            }
        }
        t = 0;
        max_t = Random.Range(0.5f, 2f);
        while (t < max_t && !iRule.self.isAngry && !iRule.self.isSuspicion)
        {
            yield return null;
            t += Time.deltaTime;
        }
    }
    #endregion
    #region LIGHT
    public void Light_Switch()
    {
        foreach (var a in gameObject.GetComponentsInChildren<Light>(true))
            if (a.tag == "W_Light") a.enabled = !a.enabled;
    }
    public void FlashLight_Switch()
    {
        foreach (var a in gameObject.GetComponentsInChildren<Light>(true))
            if (a.tag == "W_FlashLight") a.enabled = !a.enabled;
    }
    #endregion
    #region MOVE AND SHOOT
    float distance_to_player;
    float distance_to_player_phantom;
    Vector3 move;
    Vector3 random_move_dir;
    bool isPauseReload;
    IEnumerator pause_reload()
    {
        isPauseReload = true;
        float time = current_behaviour.fire_reload + Random.Range(current_behaviour.fire_reload / -3f, current_behaviour.fire_reload / 3f);
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
        }
        isPauseReload = false;
        iRule.container.ActualShootCount = current_behaviour.shoot_count_per_fire;
    }
    IEnumerator random_move()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(current_behaviour.random_move_timing[0], current_behaviour.random_move_timing[1]));
            random_move_dir = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0
                ); 
        }
    }
    #endregion
    #region STEALTH
    Vector2 last_player_pos;

    float stealth_timer;
    public void S_Update()
    {
        if (iRule.self.isAngry)
        {
            angry_update();
            return;
        }
        if (iRule.self.isSuspicion)
        {
            suspicion_update();
            return;
        }
        basic_update();
    }

    void angry_update()
    {
        stealth_timer -= Time.deltaTime;
        if (isPlayerInFOV)
        {
            stealth_timer = stealth.angry_time;
            if (iRule.self.view_gauge != 1)
                iRule.container.stealth.UPD();
            iRule.self.view_gauge = 1;
        }
        if (stealth_timer <= 0)
        {
            iRule.self.isAngry = false;
            iRule.self.isSuspicion = true;
            iRule.self.view_gauge = 0.51f;
            stealth_timer = stealth.suspicion_time;
            iRule.container.stealth.UPD();
        }
    }
    void suspicion_update()
    {
        stealth_timer -= Time.deltaTime;
        if (isPlayerInFOV)
        {
            stealth_timer = stealth.suspicion_time;
            iRule.self.view_gauge += Time.deltaTime * stealth.reaction_speed;
            iRule.container.stealth.UPD();
        }
        if (iRule.self.view_gauge >= 1)
        {
            iRule.self.isAngry = true;
            iRule.self.isSuspicion = false;
            stealth_timer = stealth.angry_time;
            iRule.self.view_gauge = 1;
            group_alarm();
            iRule.container.stealth.UPD();
            return;
        }
        if (stealth_timer <= 0)
        {
            iRule.self.isSuspicion = false;
            stealth_timer = 0;
            iRule.self.view_gauge = 0;
        }
    }
    void basic_update()
    {
        stealth_timer -= Time.deltaTime;
        if (isPlayerInFOV)
        {
            stealth_timer = stealth.basic_time;
            iRule.self.view_gauge += Time.deltaTime * stealth.reaction_speed;

            iRule.container.stealth.UPD();
        }
        if (iRule.self.view_gauge >= 0.5f)
        {
            iRule.self.isAngry = false;
            iRule.self.isSuspicion = true;
            stealth_timer = stealth.suspicion_time;
            iRule.self.view_gauge = 0.5f;
            return;
        }
        if (stealth_timer <= 0)
        {
            stealth_timer = 0;
            iRule.self.view_gauge = 0;
        }
    }

    bool isPlayerInFOV = false;
    bool check_isPlayerInFOV
    {
        get
        {
            if (distance_to_player > stealth.view_distance)
                return false;

            var lc = Physics2D.Linecast(transform.position, player.position, LayerMask.GetMask("Obstacle"));
            if (lc.collider != null)
                Debug.DrawLine(transform.position, lc.point);
            else
                Debug.DrawLine(transform.position, player.position, Color.red);
            if (lc.collider != null)
                return false;

            Vector2 dir = player.position - transform.position;
            float cos =
                (dir.x * transform.right.x + dir.y * transform.right.y)
            / //_________________________________________________________________________
                Mathf.Sqrt(dir.x * dir.x + dir.y * dir.y) *
                Mathf.Sqrt(transform.right.x * transform.right.x + transform.right.y * transform.right.y);

            if (stealth.Cos_AngleOfView > cos)
                return false;

            return true;
        }
    }

    void group_alarm()
    {
        Transform parent = transform;
        bool isCoop = false;
        foreach (var a in GetComponentsInParent<Transform>())
            if (a.tag == "AI_Coop")
            {
                parent = a;
                isCoop = true;
                break;
            }
        if (!isCoop)
            return;
        foreach (var a in parent.GetComponentsInChildren<AIController>())
        {
            a.iRule.self.view_gauge = 1;
            a.iRule.container.stealth.UPD();
            a.iRule.self.isAngry = true;
            a.iRule.container.last_player_pos = player.position;
        }
    }
    #endregion
    #region EVENT
    public delegate void ai_Delegate(ref List<AIController> arg);
    public static event ai_Delegate OnAngry;
    public static void CallEvent(AiEvents type, ref List<AIController> arg)
    {
        switch (type)
        {
            case AiEvents.OnAngry:
                OnAngry?.Invoke(ref arg);
                break;
        }
    }
    #endregion

    public enum AiEvents
    {
        OnAngry
    }
}
