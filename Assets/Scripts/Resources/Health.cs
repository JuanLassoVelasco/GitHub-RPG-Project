using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;

namespace RPG.Resources
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] float healthPoints = 100f;

        GameObject attacker;
        float maxHealth;
        bool isDead = false;

        private void Awake()
        {
            healthPoints = gameObject.GetComponent<BaseStats>().GetHealth();
            maxHealth = healthPoints;
        }

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

        public float GetPercentHealth()
        {
            return 100 * (healthPoints / maxHealth);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            attacker = instigator;

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

                float xpReward = GetComponent<BaseStats>().GetExperienceReward();
                attacker.GetComponent<Experience>().GainExperience(xpReward);
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
