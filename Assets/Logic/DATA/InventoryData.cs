using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using WeaponData;

namespace InventoryData
{
    [System.Serializable]
    public class Inventory
    {
        public Item[] current;
        [System.NonSerialized] public Unit unit;
        public Inventory(Unit unit)
        {
            this.unit = unit;
            current = new Item[0];
        }

        public Item this[int index]
        {
            get
            {
                if (index < 0 || index >= current.Length)
                    return new Item();
                return current[index];
            }
        }

        public void Add(Item item)
        {
            List<Item> ret = new List<Item>();
            ret.AddRange(current);
            item.inventory = this;
            ret.Add(item);
            current = ret.ToArray();
        }
        public void Remove(int index)
        {
            List<Item> ret = new List<Item>();
            ret.AddRange(current);
            ret.RemoveAt(index);
            current = ret.ToArray();
        }
        public void Remove(Item item)
        {
            List<Item> ret = new List<Item>();
            ret.AddRange(current);
            ret.Remove(item);
            current = ret.ToArray();
        }
        public T GetUsesItem<T>() where T: Item
        {
            foreach (var a in current)
                if (a is T && a.isEquiped)
                    return a as T;
            return null; //rework
        }
        
        public void Equip()
        {
            for (int i = 0; i < current.Length; i++)
                if (current[i].isEquiped)
                    current[i].Equip();
        }
        public void ReGenerate()
        {
            foreach (var a in current)
                a.ReGenerate();
        }
    }

    [System.Serializable]
    public class Item
    {
        [System.NonSerialized] public InitialItem item_file;

        public Inventory inventory;
        public string original_filePath;
        public bool isEquiped;

        public virtual void Equip()
        {

        }
        public virtual void Unequip()
        {

        }
        public virtual void Use()
        {

        }

        public void PathFind()
        {
            item_file = (InitialItem)Resources.Load("Items/" + original_filePath + "/" + OnType?.Invoke());
        }
        public void Generate(string filePath)
        {
            original_filePath = filePath;
            PathFind();
        }
        public void ReGenerate()
        {
            PathFind();
        }

        public delegate string item_delegate_string();

        public event item_delegate_string OnType;
    }

    [System.Serializable]
    public class Weapon_Item : Item
    {
        public Weapon_Item()
        {
            OnType += () => { return "BOW"; };
        }

        public override void Equip()
        {
            for (int i = 0; i < inventory.current.Length; i++)
                if (inventory[i] is Weapon_Item)
                    inventory[i].isEquiped = false;
            isEquiped = true;
            GameObject g = Object.Instantiate(item_file.current_objects[1]) as GameObject;
            g.name = item_file.current_objects[1].name;

            inventory.unit.weapon.PickUpWeapon(
                new InstanceWeapon()
                {
                    weaponObject = item_file.current_objects[0] as WeaponObject,
                    weapon = g
                });
            if (inventory.unit.w_rule.data.refresh_animator)
            {
                inventory.unit.w_rule.data.animator.runtimeAnimatorController = item_file.current_objects[2] as AnimatorOverrideController;
            }
            inventory.unit.StartCoroutine(apply());
        }

        IEnumerator apply()
        {
            yield return null;
            inventory.unit.w_rule.data.animator.Rebind();
        }
    }
    [System.Serializable]
    public class Helmet_Item : Item
    {
        public Helmet_Item()
        {
            OnType += () => { return "HELMET"; };
        }
        public override void Equip()
        {
            for (int i = 0; i < inventory.current.Length; i++)
                if (inventory[i] is Helmet_Item)
                    inventory[i].isEquiped = false;
            isEquiped = true;

            inventory.unit.EquipItem(item_file.current_objects[0] as ItemInfoObject, ItemType.Helmet);
        }
    }
    [System.Serializable]
    public class Body_Item : Item
    {
        public Body_Item()
        {
            OnType += () => { return "BODY"; };
        }
        public override void Equip()
        {
            for (int i = 0; i < inventory.current.Length; i++)
                if (inventory[i] is Body_Item)
                    inventory[i].isEquiped = false;
            isEquiped = true;

            inventory.unit.EquipItem(item_file.current_objects[0] as ItemInfoObject, ItemType.Body);
        }
    }
    [System.Serializable]
    public class Bracer_Item : Item
    {
        public Bracer_Item()
        {
            OnType += () => { return "BRACER"; };
        }
        public override void Equip()
        {
            for (int i = 0; i < inventory.current.Length; i++)
                if (inventory[i] is Bracer_Item)
                    inventory[i].isEquiped = false;
            isEquiped = true;

            inventory.unit.EquipItem(item_file.current_objects[0] as ItemInfoObject, ItemType.Bracer);
        }
    }
    [System.Serializable]
    public class Scapular_Item : Item
    {
        public Scapular_Item()
        {
            OnType += () => { return "SCAPULAR"; };
        }
        public override void Equip()
        {
            for (int i = 0; i < inventory.current.Length; i++)
                if (inventory[i] is Scapular_Item)
                    inventory[i].isEquiped = false;
            isEquiped = true;

            inventory.unit.EquipItem(item_file.current_objects[0] as ItemInfoObject, ItemType.Scapular);
        }
    }
}
