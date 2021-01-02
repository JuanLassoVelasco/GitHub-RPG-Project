using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float weaponDamage = 5f;

        Health target;
        Animator animator;
        Mover mover;

        float timeSinceLastAttack = Mathf.Infinity;

        private void Start()
        {
            animator = GetComponent<Animator>();
            mover = GetComponent<Mover>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target != null)
            {
                if (target.IsDead() == false)
                {
                    float distance = Vector3.Distance(transform.position, target.transform.position);
                    if (distance > weaponRange)
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
            if (target != null) {
                target.TakeDamage(weaponDamage);
            }
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
        }

        
    }
}
