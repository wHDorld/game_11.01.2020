using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InitialUnitAbilities", menuName = "Create Initiate Abilities")]
public class InitialAbilities : ScriptableObject
{
    public StaticValue staticValue;
    public VariableValue variableValue;

    [System.Serializable]
    public class StaticValue
    {
        public float Speed;
        public float RotateSpeed;
    }
    [System.Serializable]
    public class VariableValue
    {
        public float MaxHealth;
        public float MaxStamina;
    }
}
