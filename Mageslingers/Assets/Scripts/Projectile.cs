using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Projectile : NetworkActor
{
    public Hitbox hitbox;
    public GameObject Model;

    public Vector3 Direction = new Vector3(0,0,1);
    public float Speed = 1;

    public CharacterPawn Owner;
    public float lifeTime = 3;

    public float Gravity = 0;

    Vector3 mouseTarget;

    private void Awake()
    {
        hitbox.OnHitboxEnter += HandleHit;
        hitbox.OnHitboxExit += HandleExit;
        Model.SetActive(false);
    }

    public void IntializeProjectile(CharacterPawn owner, Vector3 target)
    {
        Owner = owner;
        mouseTarget = target;
        hitbox.IntializeHitBox(owner);
    }

    public void FireProjectile(Vector3 startPosition, bool owner)
    {
        Model.SetActive(true);

        Vector3 direction = new Vector3(mouseTarget.x, 0, mouseTarget.z) - new Vector3(Owner.transform.position.x, 0, Owner.transform.position.z);
        transform.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.position = Owner.transform.position + Vector3.up + direction.normalized / 2f ;
        Direction = direction.normalized;
        if (owner) { hitbox.ActivateHitbox(); LeanTween.delayedCall(lifeTime, () => CmdDestroyMe()); }

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().velocity = Direction * Speed;

    }
    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {

    }

    public void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity += Vector3.up * Gravity * Time.fixedDeltaTime;
    }

    public virtual void HandleHit(Collider col, CollisionHitType hitType, HitInfo hitInfo)
    {
        if(hitType == CollisionHitType.Pawn)
        {
            hitbox.TriggerEffect(col.GetComponentInParent<CharacterPawn>());
        }
        else if(hitType == CollisionHitType.Destructable)
        {
            col.GetComponentInParent<IHealth>().TakeDamage(hitbox.hitBoxData, hitInfo);
        }
        if (hitbox.hitBoxData.DestroyOnHit) 
        {
            hitbox.GetComponent<Collider>().enabled = false;
            CmdTriggerHitEffect();
            CmdDestroyMe();
        }
    }

    public virtual void HandleExit(Collider col, CollisionHitType hitType)
    {

    }

    [Command(requiresAuthority = false)]
    public void CmdTriggerHitEffect()
    {
        RpcTriggerHitEffect();
    }
    [ClientRpc]
    public void RpcTriggerHitEffect()
    {

    }

    [Command(requiresAuthority = false)]
    public void CmdDestroyMe()
    {
        LeanTween.cancel(gameObject);
        NetworkServer.Destroy(gameObject);
    }
}

[System.Serializable]
public struct HitInfo
{
    public Vector3 hitpoint;
    public Vector3 hitDirection;
}