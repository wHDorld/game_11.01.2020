using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SaveData
{
    public static class Save
    {
        public static int current_save_file;
        public static SaveFile[] current;

        public static void LOAD()
        {
            if (!Directory.Exists("SAVE"))
            {
                Directory.CreateDirectory("SAVE");
            }
            if (!File.Exists("SAVE/PlayerSave.trapsAreGay"))
            {
                current = new SaveFile[3];
                for (int i = 0; i < current.Length; i++)
                    current[i] = new SaveFile(); 
                return;
            }
            FileStream f = File.Open("SAVE/PlayerSave.trapsAreGay", FileMode.Open);
            current = (SaveFile[])(new BinaryFormatter().Deserialize(f));
            f.Close();
        }
        public static void SAVE()
        {
            if (!Directory.Exists("SAVE"))
            {
                Directory.CreateDirectory("SAVE");
            }
            FileStream f = File.Create("SAVE/PlayerSave.trapsAreGay");
            new BinaryFormatter().Serialize(f, current);
            f.Close();
        }
    }

    [System.Serializable]
    public class SaveFile
    {
        public InventoryData.Inventory inventory;
        public float day_coef = 1f;
        public float game_time_global = 0;
        public float game_time_local = 0;
        public string[] tags = new string[0];
        public float[] spawn_pos = new float[2];
        public string spawn_location;
        
        public SaveFile()
        {
            tags = new string[0];
            inventory = new InventoryData.Inventory(GameObject.FindObjectOfType<PlayerController>());
            add_weapon(inventory);

            InventoryData.Helmet_Item h = new InventoryData.Helmet_Item();
            h.Generate("BasicSuit");
            h.isEquiped = true;
            inventory.Add(h);

            InventoryData.Body_Item b = new InventoryData.Body_Item();
            b.Generate("BasicSuit");
            b.isEquiped = true;
            inventory.Add(b);

            InventoryData.Bracer_Item br = new InventoryData.Bracer_Item();
            br.Generate("BasicSuit");
            br.isEquiped = true;
            inventory.Add(br);

            InventoryData.Scapular_Item s = new InventoryData.Scapular_Item();
            s.Generate("BasicSuit");
            s.isEquiped = true;
            inventory.Add(s);

        }
        void add_weapon(InventoryData.Inventory inventory)
        {
            InventoryData.Weapon_Item w = new InventoryData.Weapon_Item();
            w.Generate("KnightSuit");
            w.isEquiped = true;
            inventory.Add(w);

            w = new InventoryData.Weapon_Item();
            w.Generate("DarkBoss");
            w.isEquiped = true;
            inventory.Add(w);
        }

        public void AddTag(string tag)
        {
            List<string> ret = new List<string>();
            ret.AddRange(tags);
            ret.Add(tag);
            tags = ret.ToArray();
        }
        public bool IsTag(string tag)
        {
            foreach (var a in tags)
                if (a == tag)
                    return true;
            return false;
        }
    }
}
