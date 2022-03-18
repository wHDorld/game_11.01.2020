using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArtifitialIntelligenceData;

[CreateAssetMenu(fileName = "AI", menuName = "Create new AI")]
public class AI_Object : ScriptableObject
{
    public string NAME;
    public string TAG;
    public MobType mobType;
    public InitiateStat initiateStat;

    public AI_Behaviour_Struct default_behaviour;
    public AI_Behaviour_Struct[] all_behaviours;
}
