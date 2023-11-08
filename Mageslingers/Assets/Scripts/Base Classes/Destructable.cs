using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : BasePawn
{
    public GameObject destructablePrefab;
    // Start is called before the first frame update
    public override void Start()
    {
    }

    public override void HandleDeath(Health health, HitboxData hitBoxData, HitInfo hitInfo)
    {
        Break(hitBoxData, hitInfo);
        base.HandleDeath(health, hitBoxData, hitInfo);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    [Mirror.Command(requiresAuthority = false)]
    public void CmdTakeDamage(HitboxData hitboxdata, HitInfo hitInfo)
    {
        RpcTakeDamage(hitboxdata, hitInfo);
    }
    [Mirror.ClientRpc]
    public void RpcTakeDamage(HitboxData hitboxdata, HitInfo hitInfo)
    {
        HealthData.TakeDamage(hitboxdata, hitInfo);
    }

    public void Break(HitboxData hitBoxData, HitInfo hitInfo)
    {
        GameObject destructable = Instantiate(destructablePrefab, transform.position, transform.rotation);
        Rigidbody[] rigidbodies = destructable.GetComponentsInChildren<Rigidbody>();
        foreach (var item in rigidbodies)
        {
            item.transform.parent = null;
            item.AddExplosionForce(1, transform.position, 10, 1, ForceMode.Impulse);
            item.AddForce(hitInfo.hitDirection * 5 * (1 - (Mathf.Clamp(Vector3.Distance(hitInfo.hitpoint, item.GetComponent<Renderer>().bounds.center), 0, 2) / 2f)), ForceMode.Impulse);
        }
        Destroy(destructable, 1);

        this.gameObject.SetActive(false);

        if (GameManager.instance.isServer)
        {
            Mirror.NetworkServer.Destroy(this.gameObject);
        }
    }

    public override void TakeDamage(HitboxData hitboxdata, HitInfo hitInfo)
    {
        CmdTakeDamage(hitboxdata, hitInfo);
    }
}
