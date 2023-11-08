using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectDatabase", menuName = "StatusEffects/StatusEffectDatabase")]
public class StatusEffectDatabase : ScriptableObject
{
    public StatusEffectData[] StatusEffectData;
}
[System.Serializable]
public class StatusEffectData
{
    public StatusEffect StatusEffect;
    public GameObject VFX;
}
