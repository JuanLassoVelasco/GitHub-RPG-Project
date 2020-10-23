using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour
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
                print(healthPoints);
            }
        }

        public void Die()
        {
            if (isDead == false)
            {
                isDead = true;
                GetComponent<Animator>().SetTrigger("die");
                transform.GetComponent<CapsuleCollider>().enabled = false;
            }
        }
    }
}
