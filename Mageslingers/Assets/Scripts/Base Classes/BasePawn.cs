using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BasePawn : NetworkPawn, IHealth
{
    [SerializeField]
    private StartingHealthData StartingHealthData = new StartingHealthData()
    {
        StartingHealth = 20
    };


    public Health HealthData;

    public virtual void Awake()
    {
        InitializeHealth(StartingHealthData.StartingHealth);
        HealthData.OnHealthZero += HandleDeath;
        HealthData.OnStatusEffectApplied += HandleStatusApplied;
    }

    public virtual void InitializeHealth(float health = 100)
    {
        HealthData = new Health(this, health);
    }

    public override void Start()
    {
        base.Start();
    }

    public virtual void HandleDeath(Health health, HitboxData hitBoxData, HitInfo hitInfo)
    {
        HealthData.OnHealthZero -= HandleDeath;
    }
    public virtual void HandleStatusApplied(Status status)
    {
       
    }

    public override void Update()
    {
        HealthData.Tick(Time.deltaTime);
        base.Update();
    }

    public virtual Health GetHealthData()
    {
        return HealthData;
    }

    public virtual void TakeDamage(HitboxData hitboxData, HitInfo hitInfo)
    {
        throw new System.NotImplementedException();
    }
}

[System.Serializable]
public struct StartingHealthData
{
    public float StartingHealth;
}
