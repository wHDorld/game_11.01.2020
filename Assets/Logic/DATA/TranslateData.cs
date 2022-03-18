using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "TranslateObject", menuName = "Create Translate Object")]
public class TranslateData : ScriptableObject
{
    [BoxGroup("Damage Types")]
    public TextElement[] DamageTypes;

    [BoxGroup("Items")]
    [Header("Name by ID's")]
    public TextElement[] Names;
    [BoxGroup("Items")]
    [Header("Description by ID's")]
    public TextElement[] Descriptions;

    [BoxGroup("Bonuses")]
    [Header("Bonus name by ID's")]
    public TextElement[] BonusName;
    [BoxGroup("Bonuses")]
    [Header("Bonus description by ID's")]
    public TextElement[] BonusDescr;

    [BoxGroup("Words")]
    public TextElement[] Words;

    public string Get_DamageType_ByID(int ID)
    {
        if (ID >= DamageTypes.Length || ID < 0)
            return "";
        for (int i = 0; i < DamageTypes[ID].elements.Length; i++)
            if (DamageTypes[ID].elements[i].KEY == GlobalData.Translate_Key)
                return DamageTypes[ID].elements[i].VALUE;
        return "";
    }
    public string Get_Name_ByID(int ID)
    {
        if (ID >= Names.Length || ID < 0)
            return "";
        for (int i = 0; i < Names[ID].elements.Length; i++)
            if (Names[ID].elements[i].KEY == GlobalData.Translate_Key)
                return Names[ID].elements[i].VALUE;
        return "";
    }
    public string Get_Description_ByID(int ID)
    {
        if (ID >= Descriptions.Length || ID < 0)
            return "";
        for (int i = 0; i < Descriptions[ID].elements.Length; i++)
            if (Descriptions[ID].elements[i].KEY == GlobalData.Translate_Key)
                return Descriptions[ID].elements[i].VALUE;
        return "";
    }
    public string Get_BonusName_ByID(int ID)
    {
        if (ID >= BonusName.Length || ID < 0)
            return "";
        for (int i = 0; i < BonusName[ID].elements.Length; i++)
            if (BonusName[ID].elements[i].KEY == GlobalData.Translate_Key)
                return BonusName[ID].elements[i].VALUE;
        return "";
    }
    public string Get_BonusDescr_ByID(int ID)
    {
        if (ID >= BonusDescr.Length || ID < 0)
            return "";
        for (int i = 0; i < BonusDescr[ID].elements.Length; i++)
            if (BonusDescr[ID].elements[i].KEY == GlobalData.Translate_Key)
                return BonusDescr[ID].elements[i].VALUE;
        return "";
    }
    public string Get_Words_ByID(int ID)
    {
        if (ID >= Words.Length || ID < 0)
            return "";
        for (int i = 0; i < Words[ID].elements.Length; i++)
            if (Words[ID].elements[i].KEY == GlobalData.Translate_Key)
                return Words[ID].elements[i].VALUE;
        return "";
    }
}
[System.Serializable]
public class TextElement
{
    public string NAME;
    public TranslateElement[] elements = new TranslateElement[2] {
        new TranslateElement() { KEY = "EN" },
        new TranslateElement() { KEY = "RU" }
        };
}
[System.Serializable]
public class TranslateElement
{
    public string KEY;
    public string VALUE;
}