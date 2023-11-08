using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status
{
    public float TimeLeft;
    public bool StatusCompleted = false;

    public System.Action OnStatusEffectApplied;

    public Status(float time)
    {
        TimeLeft = time;
    }

    public virtual void ApplyEffect(Health health)
    {
        OnStatusEffectApplied?.Invoke();
    }
    public virtual void Tick(float DeltaTime, Health health)
    {
        TimeLeft -= DeltaTime;

        if(TimeLeft <= 0)
        {
            StatusCompleted = true;
        }
    }
}

public class FloatStatus : Status
{
    public float Value;
    public FloatStatus(float time, float value) : base(time)
    {
        this.Value = value;
    }
    public override void ApplyEffect(Health health)
    {
        base.ApplyEffect(health);
    }
    public override void Tick(float DeltaTime, Health health)
    {
        base.Tick(DeltaTime, health);
    }
}

public class TimeIntervalStatus : FloatStatus
{
    public float timeInterval;
    public float currentIntervalTime;
    public TimeIntervalStatus(float time, float Damage, float timeInterval) : base(time, Damage)
    {
        this.timeInterval = timeInterval;
        currentIntervalTime = timeInterval;
    }
    public override void ApplyEffect(Health health)
    {
        base.ApplyEffect(health);
    }
    public override void Tick(float DeltaTime, Health health)
    {
        currentIntervalTime -= DeltaTime;
        if (currentIntervalTime <= 0)
        {
            currentIntervalTime += timeInterval;
            ApplyEffect(health);
        }
        base.Tick(DeltaTime, health);
    }
}

public class FireStatus : TimeIntervalStatus
{
    public FireStatus(float time, float Damage, float timeInterval) : base(time, Damage, timeInterval)
    {

    }
    public override void ApplyEffect(Health health)
    {
        health.lastHitInfo = new HitInfo { hitDirection = Vector3.down };
        health.SubstractHealth(Value);
        base.ApplyEffect(health);
    }
    public override void Tick(float DeltaTime, Health health)
    {
        base.Tick(DeltaTime, health);
    }
}


[System.Serializable]
public class AppliedStatus
{
    public StatusEffect statusEffect = StatusEffect.NONE;
    public float Time;
    public float Value;
}

public enum StatusEffect
{
    NONE = 0,
    FIRE = 1 << 1
}