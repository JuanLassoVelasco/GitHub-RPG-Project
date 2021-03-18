using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Saving;

namespace RPG.Core
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        bool isDead = false;

        public bool IsDead()
        {
            return isDead;
        }

        private void Update()
        {
            if (healthPoints <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float damage)
        {
            if (healthPoints != 0)
            {
                healthPoints = Mathf.Max(healthPoints - damage, 0);
            }
        }

        public void Die()
        {
            if (isDead == false)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                GetComponent<ActionScheduler>().CancelCurrentAction();
                transform.GetComponent<CapsuleCollider>().enabled = false;
            }
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            float savedHealthPoints = (float)state;
            healthPoints = savedHealthPoints;
        }
    }
}
