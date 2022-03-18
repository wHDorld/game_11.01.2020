using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InventoryData;
using MainGameData;

public class Trigger_ItemPickUp : MonoBehaviour
{
    public ItemType created_item;
    public string load_item;

    Item pick_item;
    void Start()
    {
        if (SaveData.Save.current[SaveData.Save.current_save_file].IsTag((int)created_item + "_" + load_item))
            Destroy(transform.parent.gameObject);
        switch (created_item)
        {
            case ItemType.Weapon:
                pick_item = new InventoryData.Weapon_Item();
                pick_item.Generate(load_item);
                break;
            case ItemType.Helmet:
                pick_item = new InventoryData.Helmet_Item();
                pick_item.Generate(load_item);
                break;
            case ItemType.Body:
                pick_item = new InventoryData.Body_Item();
                pick_item.Generate(load_item);
                break;
            case ItemType.Bracer:
                pick_item = new InventoryData.Bracer_Item();
                pick_item.Generate(load_item);
                break;
            case ItemType.Scapular:
                pick_item = new InventoryData.Scapular_Item();
                pick_item.Generate(load_item);
                break;
        }
    }
    bool already_taken = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (already_taken || !collision.gameObject.GetComponentInParent<PlayerController>())
            return;
        already_taken = true;
        collision.gameObject.GetComponentInParent<PlayerController>().inventory.Add(pick_item);
        SaveData.Save.current[SaveData.Save.current_save_file].AddTag((int)created_item + "_" + load_item);
        Destroy(transform.parent.gameObject);
    }
}
