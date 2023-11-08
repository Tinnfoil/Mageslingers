using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public Health GetHealthData();
    public void TakeDamage(HitboxData hitBoxData, HitInfo hitInfo);
}

public class Health
{
    private float maxHealth;
    private float health;
    public NetworkPawn Pawn;
    public List<Status> EffectingStatuses;

    public bool Dead = false;
    public System.Action<Health, HitboxData, HitInfo> OnHealthZero;
    public System.Action<Status> OnStatusEffectApplied;

    public Dictionary<Status, GameObject> Effects;

    public HitboxData lastHitboxData;
    public HitInfo lastHitInfo;

    public Health(NetworkPawn pawn, float maxHealth = 100)
    {
        Pawn = pawn;
        this.maxHealth = maxHealth;
        health = maxHealth;
        Effects = new Dictionary<Status, GameObject>();
        EffectingStatuses = new List<Status>();
    }

    public void TakeDamage(HitboxData hitboxData, HitInfo hitInfo)
    {
        lastHitboxData = hitboxData;
        lastHitInfo = hitInfo;
        AddHealth(hitboxData.HealthChange);
        for (int i = 0; i < hitboxData.AppliedStatuses.Length; i++)
        {
            AddStatusEffect(hitboxData.AppliedStatuses[i]);
        }
    }

    public void AddStatusEffect(AppliedStatus status)
    {
        if (status.statusEffect == StatusEffect.NONE) return;

        Status addedStatus = null;
        switch (status.statusEffect)
        {
            case StatusEffect.FIRE:
                addedStatus = new FireStatus(status.Time, status.Value, 1);
                Effects.Add(addedStatus, GameManager.instance.SpawnGameObject(GameManager.instance.StatusEffectMap[status.statusEffect].VFX, Pawn.transform, Pawn.transform.position, Pawn.transform.rotation));
                break;
        }

        EffectingStatuses.Add(addedStatus);
        OnStatusEffectApplied?.Invoke(addedStatus);
    }
    public float GetHealth()
    {
        return health;
    }
    public void SetHealth(float value)
    {
        health = value;
    }
    public void AddHealth(float value)
    {
        health += value;
        if(!Dead && health <= 0)
        {
            Dead = true;
            OnHealthZero?.Invoke(this, lastHitboxData, lastHitInfo);
        }
    }
    public void SubstractHealth(float value)
    {
        AddHealth(-value);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
    public void SetMaxHealth(float value)
    {
        maxHealth = value;
    }
    public void AddMaxHealth(float value)
    {
        maxHealth += value;
    }
    public void SubstractMaxHealth(float value)
    {
        AddHealth(-value);
    }

    public void Tick(float deltaTime)
    {


        for (int i = 0; i < EffectingStatuses.Count; i++)
        {
            EffectingStatuses[i].Tick(deltaTime, this);

            if (EffectingStatuses[i].StatusCompleted)
            {
                GameObject VFX = Effects[EffectingStatuses[i]];
                Effects.Remove(EffectingStatuses[i]);
                GameManager.instance.DestroyGameObject(VFX);

                EffectingStatuses.RemoveAt(i);
                i--;
            }
        }
    }
}
