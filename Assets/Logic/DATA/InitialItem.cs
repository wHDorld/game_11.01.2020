using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create new Item")]
public class InitialItem : ScriptableObject
{
    public int NAME;
    public int DESCRIPTION;
    public Sprite HQ_Image;
    public Sprite LQ_Image;

    public Object[] current_objects;
}
