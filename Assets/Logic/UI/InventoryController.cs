using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InventoryData;
using TMPro;

public class InventoryController : MonoBehaviour
{
    PlayerController player;
    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
        foreach (var a in GetComponentsInChildren<Transform>())
        {
            if (a.name == "_HELMET_EQUIP") _helmet_equip = a.GetComponent<Image>();
            if (a.name == "_BODY_EQUIP") _body_equip = a.GetComponent<Image>();
            if (a.name == "_PREARM_EQUIP") _prearm_equip = a.GetComponent<Image>();
            if (a.name == "_FOREARM_EQUIP") _forearm_equip = a.GetComponent<Image>();
            if (a.name == "_PRELEG_EQUIP") _preleg_equip = a.GetComponent<Image>();
            if (a.name == "_FORELEG_EQUIP") _foreleg_equip = a.GetComponent<Image>();
            if (a.name == "_BOW_EQUIP") _bow_equip = a.GetComponent<Image>();

            if (a.name == "_LARM") _LARM = a.GetComponent<Image>();
            if (a.name == "_RARM") _RARM = a.GetComponent<Image>();
            if (a.name == "_LSHOULDER") _LSHOULDER = a.GetComponent<Image>();
            if (a.name == "_RSHOULDER") _RSHOULDER = a.GetComponent<Image>();
            if (a.name == "_BODY") _BODY = a.GetComponent<Image>();
            if (a.name == "_HEAD") _HEAD = a.GetComponent<Image>();
            if (a.name == "_BOW") _BOW = a.GetComponent<Image>();

            if (a.name == "SHOW_ITEM") show_item = a.GetComponent<Image>();
            if (a.name == "SHOW_ITEM_IMAGE") show_item_image = a.GetComponent<Image>();
            if (a.name == "SHOW_ITEM_BETTER") show_item_better = a.GetComponent<Image>();

            if (a.name == "Inventory_Content") _inventory_parent = a;
            if (a.name == "Bonus_Content") _bonus_parent = a;

            if (a.name == "ITEM_NAME") ITEM_NAME = a.GetComponent<TextMeshProUGUI>();
            if (a.name == "ITEM_DESCR") ITEM_DESCR = a.GetComponent<TextMeshProUGUI>();

            if (a.name == "_EQUIP") _EQUIP = a.GetComponent<Button>();
            if (a.name == "_USE") _USE = a.GetComponent<Button>();
            if (a.name == "_TRASH") _TRASH = a.GetComponent<Button>();
        }

        update_main_window();
        update_human_model(null);
    }

    void update_human_model(Item item)
    {
        Helmet_Item helmet = player.inventory.GetUsesItem<Helmet_Item>();
        if (helmet != null)
            _HEAD.sprite = (helmet.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;

        Body_Item body = player.inventory.GetUsesItem<Body_Item>();
        if (body != null)
            _BODY.sprite = (body.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;

        Scapular_Item scapular = player.inventory.GetUsesItem<Scapular_Item>();
        if (scapular != null)
        {
            _RSHOULDER.sprite = (scapular.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
            _LSHOULDER.sprite = (scapular.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        }

        Bracer_Item bracer = player.inventory.GetUsesItem<Bracer_Item>();
        if (bracer != null)
        {
            _RARM.sprite = (bracer.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
            _LARM.sprite = (bracer.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        }

        Weapon_Item bow = player.inventory.GetUsesItem<Weapon_Item>();
        if (bow != null)
            _BOW.sprite = (bow.item_file.current_objects[0] as WeaponObject).HQ_Preview;

        //RECLOTH
        if (item == null) return;

        if (item is Helmet_Item)
            _HEAD.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        if (item is Body_Item)
            _BODY.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        if (item is Scapular_Item)
        {
            _RSHOULDER.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
            _LSHOULDER.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        }
        if (item is Bracer_Item)
        {
            _RARM.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
            _LARM.sprite = (item.item_file.current_objects[0] as ItemInfoObject).info.EquipSprite;
        }
        if (item is Weapon_Item)
            _BOW.sprite = (item.item_file.current_objects[0] as WeaponObject).HQ_Preview;
    }
    void update_main_window()
    {
        Helmet_Item helmet_item = player.inventory.GetUsesItem<Helmet_Item>();
        if (helmet_item != null)
            _helmet_equip.sprite = helmet_item.item_file.HQ_Image;

        Body_Item body_item = player.inventory.GetUsesItem<Body_Item>();
        if (body_item != null)
            _body_equip.sprite = body_item.item_file.HQ_Image;

        Scapular_Item scapular_item = player.inventory.GetUsesItem<Scapular_Item>();
        if (scapular_item != null)
            _prearm_equip.sprite = scapular_item.item_file.HQ_Image;

        Bracer_Item bracer_item = player.inventory.GetUsesItem<Bracer_Item>();
        if (bracer_item != null)
            _forearm_equip.sprite = bracer_item.item_file.HQ_Image;

        Weapon_Item weapon_item = player.inventory.GetUsesItem<Weapon_Item>();
        if (weapon_item != null)
            _bow_equip.sprite = weapon_item.item_file.HQ_Image;
        //rework
    }
    void show_cloth(Item info)
    {
        _EQUIP.onClick.RemoveAllListeners();
        _EQUIP.interactable = true;
        _EQUIP.onClick.AddListener(() =>
        {
            info.Equip();
            update_main_window();
            show_cloth(info);
        });

        _USE.onClick.RemoveAllListeners();
        _USE.interactable = false;

        _TRASH.onClick.RemoveAllListeners();
        _TRASH.onClick.AddListener(() =>
        {
            if (info.isEquiped)
                return;
            clear_inventory();
            player.inventory.Remove(info);
            _TRASH.onClick.RemoveAllListeners();
            _EQUIP.onClick.RemoveAllListeners();
            _USE.onClick.RemoveAllListeners();
            _TRASH.interactable = false;
            _EQUIP.interactable = true;
            _USE.interactable = false;
        });
        _TRASH.interactable = !info.isEquiped;

        var b = info.item_file.current_objects[0] as ItemInfoObject;
        bool isBetter = true; //rework

        show_item.color = GlobalData.rare_array[b.info.rarity];
        show_item_image.sprite = b.HQ_Preview;
        show_item_better.color = new Color(show_item_better.color.r, show_item_better.color.g, show_item_better.color.b, isBetter ? 1 : 0);

        ITEM_NAME.text = GlobalData.translate.Get_Name_ByID(b.Name);
        ITEM_DESCR.text = GlobalData.translate.Get_Description_ByID(b.Description);
    }
    void show_weapon(Item info)
    {
        _EQUIP.onClick.RemoveAllListeners();
        _EQUIP.interactable = true;
        _EQUIP.onClick.AddListener(() =>
        {
            info.Equip();
            update_main_window();
            show_weapon(info);
        });

        _USE.onClick.RemoveAllListeners();
        _USE.interactable = false;

        _TRASH.onClick.RemoveAllListeners();
        _TRASH.onClick.AddListener(() =>
        {
            if (info.isEquiped)
                return;
            clear_inventory();
            player.inventory.Remove(info);
            _TRASH.onClick.RemoveAllListeners();
            _EQUIP.onClick.RemoveAllListeners();
            _USE.onClick.RemoveAllListeners();
            _TRASH.interactable = false;
            _EQUIP.interactable = true;
            _USE.interactable = false;
        });
        _TRASH.interactable = !info.isEquiped;

        var b = info.item_file.current_objects[0] as WeaponObject;
        bool isBetter = true; //rework

        show_item.color = GlobalData.rare_array[b.stats.Rarity];
        show_item_image.sprite = b.HQ_Preview;
        show_item_better.color = new Color(show_item_better.color.r, show_item_better.color.g, show_item_better.color.b, isBetter ? 1 : 0);

        ITEM_NAME.text = GlobalData.translate.Get_Name_ByID(b.Name);
        ITEM_DESCR.text = GlobalData.translate.Get_Description_ByID(b.Description);
    }
    void bonus_cloth(Item info)
    {
        clear_bonus();
        default_cloth_bonus(info);

        var item = info.item_file.current_objects[0] as ItemInfoObject;
        AddBonus(Modules.BonusModulesControll.GetModulesDescriptionByIDs(item.info.bonus));
    }
    void bonus_weapon(Item info)
    {
        clear_bonus();
        default_weapon_bonus(info);
    }
    void clear_inventory()
    {
        foreach (var a in _inventory_parent.GetComponentsInChildren<Transform>())
            if (a != _inventory_parent && a.gameObject != null)
                Destroy(a.gameObject);
    }
    void clear_bonus()
    {
        foreach (var a in _bonus_parent.GetComponentsInChildren<Transform>())
            if (a != _bonus_parent && a.gameObject != null)
                Destroy(a.gameObject);
    }

    public void Show_Item_Inventory(int type)
    {
        clear_inventory();
        foreach (var a in player.inventory.current)
        {
            if (a is Weapon_Item && type != 10) continue;
            if ((a is Helmet_Item && type != 0)) continue;
            if ((a is Body_Item && type != 1)) continue;
            if ((a is Scapular_Item && type != 2)) continue;
            if ((a is Bracer_Item && type != 3)) continue;

            GameObject g = Instantiate(Resources.Load("ITEM")) as GameObject;
            g.transform.SetParent(_inventory_parent, false);
            var b = a.item_file.current_objects[0] as ItemInfoObject;
            bool isBetter = true; //rework
            foreach (var c in g.GetComponentsInChildren<Image>())
            {
                if (c.name == "ITEM_IMAGE") c.sprite = b.HQ_Preview;
                if (c.name == "BETTER") c.color = new Color(c.color.r, c.color.g, c.color.b, isBetter ? 1 : 0);
            }
            var d = a;
            g.GetComponent<Image>().color = GlobalData.rare_array[b.info.rarity];
            g.GetComponent<Button>().onClick.AddListener(() =>
            {
                show_cloth(d);
                bonus_cloth(d);
                update_human_model(d);
            });
        }
    }
    public void Show_Weapon_Inventory()
    {
        clear_inventory();
        foreach (var a in player.inventory.current)
        {
            if (!(a is Weapon_Item)) continue;

            GameObject g = Instantiate(Resources.Load("ITEM")) as GameObject;
            g.transform.SetParent(_inventory_parent, false);
            var b = a.item_file.current_objects[0] as WeaponObject;
            bool isBetter = true; //rework
            foreach (var c in g.GetComponentsInChildren<Image>())
            {
                if (c.name == "ITEM_IMAGE") c.sprite = b.HQ_Preview;
                if (c.name == "BETTER") c.color = new Color(c.color.r, c.color.g, c.color.b, isBetter ? 1 : 0);
            }
            var d = a;
            g.GetComponent<Image>().color = GlobalData.rare_array[b.stats.Rarity];
            g.GetComponent<Button>().onClick.AddListener(() =>
            {
                show_weapon(d);
                bonus_weapon(d);
                update_human_model(d);
            });
        }
    }
    public void AddBonus(string text)
    {
        GameObject g = Instantiate(Resources.Load("ITEM_BONUS")) as GameObject;
        g.transform.SetParent(_bonus_parent, false);
        g.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    public void AddBonus(string[] text)
    {
        foreach (var a in text) {
            GameObject g = Instantiate(Resources.Load("ITEM_BONUS")) as GameObject;
            g.transform.SetParent(_bonus_parent, false);
            g.GetComponentInChildren<TextMeshProUGUI>().text = a;
        }
    }

    void default_cloth_bonus(Item info)
    {
        var item = info.item_file.current_objects[0] as ItemInfoObject;
        if (item.info.resist.PhysicalResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.PhysicalResist,
                "<sprite=5>", GlobalData.damage_color_hex[5])
                );
        if (item.info.resist.WaterResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.WaterResist,
                "<sprite=8>", GlobalData.damage_color_hex[8])
                );
        if (item.info.resist.FireResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.FireResist,
                "<sprite=3>", GlobalData.damage_color_hex[3])
                );
        if (item.info.resist.DarkResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.DarkResist,
                "<sprite=6>", GlobalData.damage_color_hex[6])
                );
        if (item.info.resist.LightResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.LightResist,
                "<sprite=4>", GlobalData.damage_color_hex[4])
                );
        if (item.info.resist.EnergyResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.EnergyResist,
                "<sprite=2>", GlobalData.damage_color_hex[1])
                );
        if (item.info.resist.SplashResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.SplashResist,
                "<sprite=7>", GlobalData.damage_color_hex[7])
                );
        if (item.info.resist.ToxicResist != 0)
            AddBonus(
                string.Format("{0} <{3}>{1:F0}%</color> {2}",
                GlobalData.translate.Get_Words_ByID(2),
                item.info.resist.ToxicResist,
                "<sprite=0>", GlobalData.damage_color_hex[0])
                );
    }
    void default_weapon_bonus(Item info)
    {
        var weapon = info.item_file.current_objects[0] as WeaponObject;
        if (weapon.stats.DamageMultiply != 0)
            AddBonus(
                GlobalData.translate.Get_Words_ByID(0) + " = " +
                "<" + GlobalData.damage_color_hex[5] + ">" +
                weapon.stats.DamageMultiply
                + "</color><sprite=" + 5 + ">"
                );
        if (weapon.stats.shooting_object_info != null)
        {
            AddBonus(
                GlobalData.translate.Get_Words_ByID(1) + " = " +
                "<" + GlobalData.damage_color_hex[weapon.stats.shooting_object_info.projectileObject.stats.DamageType] + ">" +
                Mathf.RoundToInt(weapon.stats.shooting_object_info.projectileObject.stats.DamageMultiply * 10f)
                + "</color><sprite=" + weapon.stats.shooting_object_info.projectileObject.stats.DamageType + ">"
                );
        }
        AddBonus(
           string.Format("{0} <color=yellow>{1:F1}</color> {2}",
           GlobalData.translate.Get_Words_ByID(5),
           weapon.stats.RateOfFire,
           GlobalData.translate.Get_Words_ByID(6))
           );
        AddBonus(
           string.Format("{0} <color=yellow>{1:F1}</color> {2}",
           GlobalData.translate.Get_Words_ByID(7),
           weapon.stats.ReloadTime,
           GlobalData.translate.Get_Words_ByID(8))
           );
        AddBonus(
           string.Format("{0} : <color=yellow>{1:F0}</color>",
           GlobalData.translate.Get_Words_ByID(9),
           weapon.stats.MaxAmmo)
           );
    }

    #region PARENTS
    public Transform _inventory_parent;
    public Transform _bonus_parent;
    #endregion

    #region ITEM INFO
    public TextMeshProUGUI ITEM_NAME;
    public TextMeshProUGUI ITEM_DESCR;
    public Button _EQUIP;
    public Button _USE;
    public Button _TRASH;
    #endregion

    #region SLOTS
    public Image _helmet_equip;
    public Image _body_equip;
    public Image _prearm_equip;
    public Image _forearm_equip;
    public Image _preleg_equip;
    public Image _foreleg_equip;
    public Image _bow_equip;
    #endregion
    #region HUMAN
    public Image _LARM;
    public Image _RARM;
    public Image _LSHOULDER;
    public Image _RSHOULDER;
    public Image _BODY;
    public Image _HEAD;
    public Image _BOW;
    #endregion
    #region SHOW ITEM
    public Image show_item;
    public Image show_item_image;
    public Image show_item_better;
    #endregion
}
