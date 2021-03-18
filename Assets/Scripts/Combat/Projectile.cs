using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

public class Projectile : MonoBehaviour
{
    [SerializeField] float projectileSpeed = 1;

    Health target;

    float damage = 0;

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;
        gameObject.transform.LookAt(GetAimLocation());
        gameObject.transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        target.TakeDamage(damage);
        Destroy(this.gameObject);
    }

    public void SetTarget(Health target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null) return target.transform.position;
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }
}
