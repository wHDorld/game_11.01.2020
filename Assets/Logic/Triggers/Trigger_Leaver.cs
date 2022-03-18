using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Leaver : MonoBehaviour
{
    public Animator animator;
    public string tag_name;
    public UnityEngine.Events.UnityEvent action;

    void Start()
    {
        if (SaveData.Save.current[SaveData.Save.current_save_file].IsTag(tag_name))
            Open();
    }
    bool already_taken = false;
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (already_taken || !collision.gameObject.GetComponentInParent<PlayerController>())
            return;
        if (!Input.GetKeyDown(KeyCode.E))
            return;
        Open();

        SaveData.Save.current[SaveData.Save.current_save_file].spawn_pos = 
            new float[2] {
            GameObject.FindObjectOfType<PlayerController>().transform.position.x,
            GameObject.FindObjectOfType<PlayerController>().transform.position.y
            };
        SaveData.Save.current[SaveData.Save.current_save_file].spawn_location =
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        SaveData.Save.current[SaveData.Save.current_save_file].game_time_local = 0;
        SaveData.Save.current[SaveData.Save.current_save_file].AddTag(tag_name);
        SaveData.Save.SAVE();
    }
    public void Open()
    {
        already_taken = true;
        animator.SetBool("open", true);
        action.Invoke();
    }
}
