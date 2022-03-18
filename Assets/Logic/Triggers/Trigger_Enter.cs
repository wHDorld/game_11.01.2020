using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Trigger_Enter : MonoBehaviour
{
    public string Level_Name;
    public float[] spawn_pos = new float[2]
    {
        0, 0
    };

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (SaveData.Save.current == null)
            return;
        if (SaveData.Save.current[SaveData.Save.current_save_file].game_time_local < 1)
            return;
        if (!collision.GetComponentInParent<PlayerController>())
            return;
        SaveData.Save.current[SaveData.Save.current_save_file].spawn_pos = spawn_pos;
        SaveData.Save.current[SaveData.Save.current_save_file].spawn_location = Level_Name;
        SaveData.Save.current[SaveData.Save.current_save_file].game_time_local = 0;
        SaveData.Save.SAVE();
        SceneManager.LoadScene(Level_Name, LoadSceneMode.Single);
    }
}
