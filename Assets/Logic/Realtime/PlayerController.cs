using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using Modules;
using WeaponData;
using UIData;

public class PlayerController : Unit
{
    public Transform pick_weapon_parent;
    public GameObject arms_go;
    public Transform aim_offset;
    public Transform laser_fire;
    public InstanceWeapon instanceWeapon;
    public InitialAbilities initialAbilities;
    public WeaponAnimatorHandler weaponAnimatorHandler;
    public Transform cam;
    public Transform aim_original;
    public UI_Manager ui_manager;

    Cinemachine.CinemachineVirtualCamera virtualCamera;
    Transform arms_transform;
    Camera cam_c;

    void Start()
    {
        GlobalData.Initiate();
        virtualCamera = GameObject.FindGameObjectWithTag("PlayerCameraS").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        cam_c = cam.GetComponent<Camera>();
        arms_transform = arms_go.transform;
        SaveData.Save.LOAD();
        if (SaveData.Save.current[SaveData.Save.current_save_file].spawn_location == UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        transform.position = new Vector3(
            SaveData.Save.current[SaveData.Save.current_save_file].spawn_pos[0],
            SaveData.Save.current[SaveData.Save.current_save_file].spawn_pos[1],
            0
            );

        ui_manager = new UI_Manager();
        ui_manager.Initiate();
        //Cursor.lockState = CursorLockMode.Locked;
        Initiate(new UnitRule(gameObject, initialAbilities, "Player"), new WeaponRule(instanceWeapon, arms_go.GetComponent<Animator>(), true));

        inventory = SaveData.Save.current[SaveData.Save.current_save_file].inventory;
        inventory.unit = this;

        rule.requireSystem.pick_weapon_parent = pick_weapon_parent;
        weaponAnimatorHandler = new WeaponAnimatorHandler(this);
        permanent_modules.Add(new C_Movement(rule));
        permanent_modules.Add(new C_LookAt(rule));
        permanent_modules.Add(new C_BasicStaminaHeal(rule));

        inventory.ReGenerate();
        inventory.Equip();

        Light_Switch();
        FlashLight_Switch();
    }

    void Update()
    {
        SaveData.Save.current[SaveData.Save.current_save_file].game_time_global += Time.deltaTime;
        SaveData.Save.current[SaveData.Save.current_save_file].game_time_local += Time.deltaTime;
        laser_fire.localScale = new Vector3(25f, 0.4f * w_rule.preferableSystem.Tension, 1);
        if (Input.GetKeyDown(KeyCode.Tab))
            ui_manager.OpenUI();
        if (Input.GetKeyDown(KeyCode.Escape))
            ui_manager.CloseUI();

        Move();
        Look();
        weaponAnimatorHandler.Update();
        Shoot();
        mouse_speed = w_rule.preferableSystem.isAim ? 1.2f : 2f;

        if (Input.GetKeyDown(KeyCode.Z))
            Light_Switch();
        if (Input.GetKeyDown(KeyCode.X))
            FlashLight_Switch();
    }

    public void Move()
    {
        rule.preferableSystem.preferable_move_direction =            
            new Vector3(
            Input.GetAxis("Horizontal"),
            Input.GetAxis("Vertical"),
            0
            );
    }
    public void Shoot()
    {
        w_rule.preferableSystem.isTension = Input.GetKey(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse1);
        if (Input.GetKeyUp(KeyCode.Mouse0))
            w_rule.preferableSystem.isTryShoot = true;
        if (Input.GetKey(KeyCode.Mouse1) && w_rule.preferableSystem.Tension >= 1)
            w_rule.preferableSystem.isTryShoot = true;
    }
    
    float mouse_speed;
    float camera_distance = 8;
    Quaternion arms_rotation;
    public void Look()
    {
        rule.preferableSystem.preferable_look_pos = cam_c.ScreenToWorldPoint(Input.mousePosition);

        rule.requireSystem.aim_directional = aim_original.right;
        rule.requireSystem.aim_original = aim_original.position;
        rule.requireSystem.aim_original_offset = aim_offset.position - aim_original.position;

        camera_distance -= Input.GetAxis("Mouse ScrollWheel") * 15f;
        camera_distance = camera_distance > 16 ? 16 : (camera_distance < 4 ? 4 : camera_distance);
        virtualCamera.m_Lens.OrthographicSize = camera_distance;

        /*cam.position = Vector3.Lerp(
            cam.position,
            new Vector3(transform.position.x, transform.position.y, -10),
            4 * Time.deltaTime
            );*/
    }

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
}
