using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class DayNightSystem : MonoBehaviour
{
    SFLight sun;
    void Start()
    {
        if (GetComponent<SFLight>())
        sun = GetComponent<SFLight>();
    }

    void Update()
    {
        SaveData.Save.current[SaveData.Save.current_save_file].day_coef += Time.deltaTime / 600f;
        if (SaveData.Save.current[SaveData.Save.current_save_file].day_coef > 1)
            SaveData.Save.current[SaveData.Save.current_save_file].day_coef = -1f;
        if (sun != null)
            sun.color = new Color(
                Mathf.Abs(SaveData.Save.current[SaveData.Save.current_save_file].day_coef) / 2f,
                Mathf.Abs(SaveData.Save.current[SaveData.Save.current_save_file].day_coef) / 2f,
                Mathf.Abs(SaveData.Save.current[SaveData.Save.current_save_file].day_coef) / 2f,
                0);
    }
}
