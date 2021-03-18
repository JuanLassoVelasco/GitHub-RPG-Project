using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;
using System;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] Transform rightHandTransform = null;
        [SerializeField] Transform leftHandTransform = null;
        [SerializeField] Weapon defaultWeapon = null;

        Health target;
        Animator animator;
        Mover mover;
        Weapon currentWeapon = null;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
            EquipWeapon(defaultWeapon);
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null)
            {
                if (target.IsDead() == false)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance > currentWeapon.GetWeaponRange())
                    {
                        animator.SetTrigger("stopAttack");
                        mover.MoveTo(target.transform.position, 1f);
                    }
                    else
                    {
                        mover.Cancel();
                        AttackBehavior();
                    }
                }
            }

        }

        public void EquipWeapon(Weapon weapon)
        {
            if (weapon == null) return;
            currentWeapon = weapon;
            weapon.Spawn(rightHandTransform, leftHandTransform, animator);
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack >= timeBetweenAttacks)
            {
                // This will trigger the Hit() event
                animator.ResetTrigger("stopAttack");
                animator.SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        // Animation Event
        void Hit()
        {
            if (target == null) return;
            if (currentWeapon.HasProjectile())
            {
                currentWeapon.LaunchProjectile(rightHandTransform, leftHandTransform, target);
            }
            else
            {
                target.TakeDamage(currentWeapon.GetWeaponDamage());
            }
        }

        // Animation Event
        void Shoot()
        {
            Hit();
        }

        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
            target = null;
            GetComponent<Mover>().Cancel();
        }
        
    }
}
