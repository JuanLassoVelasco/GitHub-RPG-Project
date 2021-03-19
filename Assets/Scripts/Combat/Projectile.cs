using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;

namespace RPG.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] GameObject hitFX = null;
        [SerializeField] GameObject[] itemsToDestroyOnHit = null;
        [SerializeField] float projectileSpeed = 1f;
        [SerializeField] float lifeTimeAfterDestroy = 1f;
        [SerializeField] float maxLifeTime = 6f;
        [SerializeField] bool isHoming = false;

        Health target;
        GameObject instigator;
        BoxCollider boxCollider;

        float damage = 0;

        private void Start()
        {
            gameObject.transform.LookAt(GetAimLocation());
            boxCollider = GetComponent<BoxCollider>();
        }

        // Update is called once per frame
        void Update()
        {
            if (target != null & isHoming)
            {
                gameObject.transform.LookAt(GetAimLocation());
            }

            gameObject.transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);

            if (target.IsDead())
            {
                target = null;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != target) return;
            target.TakeDamage(instigator, damage);
            if (hitFX != null)
            {
                Instantiate(hitFX, this.transform.position, Quaternion.identity);
            }

            boxCollider.enabled = false;
            projectileSpeed = 0f;

            foreach (GameObject toDestroy in itemsToDestroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(this.gameObject, lifeTimeAfterDestroy);
        }

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            this.target = target;
            this.damage = damage;
            this.instigator = instigator;

            Destroy(this.gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
            if (targetCapsule == null) return target.transform.position;
            return target.transform.position + Vector3.up * targetCapsule.height / 2;
        }
    }
}
