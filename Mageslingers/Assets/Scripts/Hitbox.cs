using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    public Action<Collider, CollisionHitType> OnHitboxEnter, OnHitboxExit;

    public HashSet<NetworkPawn> hitPawns = new HashSet<NetworkPawn>();

    public CharacterPawn Owner;

    public HitboxData hitBoxData;

    float timePassed;

    private void Awake()
    {
        GetComponent<Collider>().enabled = false;
    }

    public virtual void IntializeHitBox(CharacterPawn owner)
    {
        Owner = owner;
    }

    public virtual void ActivateHitbox()
    {
        GetComponent<Collider>().enabled = true;
    }

    public void Update()
    {
        timePassed += Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        CharacterPawn np = other.GetComponentInParent<CharacterPawn>();
        if (np && (hitBoxData.EffectsOwner ? (true) : (np != Owner)))
        {
            if (!hitPawns.Contains(np))
            {
                hitPawns.Add(np);
                OnHitboxEnter?.Invoke(other, CollisionHitType.Pawn);
            }

        }
        else if (np == null)
        {
            OnHitboxEnter?.Invoke(other, CollisionHitType.Enviroment);
        }

        Debug.Log("Trigger");
    }

    public void OnTriggerExit(Collider other)
    {
        CharacterPawn np = other.GetComponentInParent<CharacterPawn>();
        if (np && (hitBoxData.EffectsOwner ? (true) : (np != Owner)))
        {
            if (hitPawns.Contains(np))
            {
                hitPawns.Remove(np);
                OnHitboxExit?.Invoke(other, CollisionHitType.Pawn);
            }
        }
        else if (np == null)
        {
            OnHitboxExit?.Invoke(other, CollisionHitType.Enviroment);
        }
    }

    public virtual void TriggerEffect(CharacterPawn pawn)
    {
        pawn.TakeDamage(hitBoxData.HealthChange);
    }

}

[System.Serializable]
public class HitboxData
{
    public float HealthChange = 0;
    public bool EffectsOwner = false;
    public bool DestroyOnHit = true;
}

public class EOAHitboxData : HitboxData
{
    [Tooltip("If >0, will repeat this damage to whoever is in collider")]
    public float TriggerInterval = 0;
}
public class DOTHitboxData : HitboxData
{
    [Tooltip("If >0, will repeat this damage to whoever is in collider")]
    public float TriggerInterval = 0;
}



public enum CollisionHitType
{
    Pawn,
    Enviroment
}
