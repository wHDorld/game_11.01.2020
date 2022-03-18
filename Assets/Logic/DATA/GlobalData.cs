using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class GlobalData
{
    public static string Translate_Key = "EN";
    public static void Initiate()
    {
        translate = Resources.Load("TranslateObject") as TranslateData;
        GenerateRareColor();
        GenerateDamageColor();
    }
    public static void GenerateRareColor()
    {
        List<string[]> cols = new List<string[]>();
        StreamReader sr = new StreamReader("OPEN_DATA/INFO/RGB_ITEM_RARITY_LIST.txt");
        while (!sr.EndOfStream)
        {
            cols.Add(sr.ReadLine().Split(','));
        }
        rare_array = new Color[cols.Count];
        for (int i = 0; i < cols.Count; i++)
            rare_array[i] = new Color(
                int.Parse(cols[i][0]) / (float)255,
                int.Parse(cols[i][1]) / (float)255,
                int.Parse(cols[i][2]) / (float)255,
                1
                );
    }
    public static void GenerateDamageColor()
    {
        List<string> cols = new List<string>();
        StreamReader sr = new StreamReader("OPEN_DATA/INFO/RGB_DAMAGE_LIST.txt");
        while (!sr.EndOfStream)
        {
            cols.Add(sr.ReadLine());
        }
        damage_color_hex = new string[cols.Count];
        for (int i = 0; i < cols.Count; i++)
            damage_color_hex[i] = cols[i];
    }

    public static float DAMAGE_NOW(float damageMultiply, int level)
    {
        return damageMultiply * Mathf.Pow(level, 2.2f);
    }

    public static TranslateData translate;
    public static Color[] rare_array;
    
    public static string[] damage_color_hex;

    #region EFFECTS
    public static Object fire_effect;
    public static GameObject get_fire_effect
    {
        get
        {
            fire_effect = fire_effect ?? Resources.Load("Effects/FireEffect");
            return Object.Instantiate(fire_effect) as GameObject;
        }
    }

    public static Object water_effect;
    public static GameObject get_water_effect
    {
        get
        {
            water_effect = water_effect ?? Resources.Load("Effects/WaterEffect");
            return Object.Instantiate(water_effect) as GameObject;
        }
    }

    public static Object energy_effect;
    public static GameObject get_energy_effect
    {
        get
        {
            energy_effect = energy_effect ?? Resources.Load("Effects/EnergyEffect");
            return Object.Instantiate(energy_effect) as GameObject;
        }
    }

    public static Object aid_effect;
    public static GameObject get_aid_effect
    {
        get
        {
            aid_effect = aid_effect ?? Resources.Load("Effects/AidEffect");
            return Object.Instantiate(aid_effect) as GameObject;
        }
    }

    public static Object dark_effect;
    public static GameObject get_dark_effect
    {
        get
        {
            dark_effect = dark_effect ?? Resources.Load("Effects/DarkEffect");
            return Object.Instantiate(dark_effect) as GameObject;
        }
    }

    public static Object light_effect;
    public static GameObject get_light_effect
    {
        get
        {
            light_effect = light_effect ?? Resources.Load("Effects/LightEffect");
            return Object.Instantiate(light_effect) as GameObject;
        }
    }
    #endregion
}
