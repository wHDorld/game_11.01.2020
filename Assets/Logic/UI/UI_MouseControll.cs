using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MouseControll : MonoBehaviour
{
    public Transform look_at;
    public Camera cam;
    public Image reload;
    public Image fire_reload;
    public PlayerController player;
    Transform t;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        Cursor.visible = false;
        t = transform;
    }
    void Update()
    {
        reload.fillAmount = player.w_rule.condition.ReloadAmount;
        fire_reload.fillAmount = player.w_rule.condition.FireReloadAmount;
        t.position = cam.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, 10);
        look();
    }
    void look()
    {
        Vector3 dir_look_at = -look_at.position + t.position;
        float angle = Mathf.Atan2(dir_look_at.y, dir_look_at.x) * Mathf.Rad2Deg;
        t.rotation = Quaternion.Euler(
            0, 0,
            angle
            );
    }
}
