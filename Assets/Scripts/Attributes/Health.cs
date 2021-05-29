using UnityEngine;
using RPG.Saving;
using RPG.Stats;
using RPG.Core;
using System;
using GameDevTV.Utils;
using UnityEngine.Events;

namespace RPG.Attributes
{
    public class Health : MonoBehaviour, ISaveable
    {
        
        [Range(0f, 100f)]
        [SerializeField] float percentRegenOnLevelUp = 70f;
        [SerializeField] TakeDamageEvent takeDamage;
        [SerializeField] UnityEvent onDie;

        [System.Serializable]
        public class TakeDamageEvent : UnityEvent<float>
        {

        }

        BaseStats baseStats;
        bool isDead = false;
        LazyValue<float> healthPoints;

        private void Awake()
        {
            baseStats = gameObject.GetComponent<BaseStats>();
            healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return baseStats.GetCalculatedStat(Stat.Health);
        }

        private void Start()
        {
            healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            if (baseStats != null)
            {
                baseStats.onLevelUp += UpdateHealth;
            }
        }

        private void OnDisable()
        {
            if (baseStats != null)
            {
                baseStats.onLevelUp -= UpdateHealth;
            }
        }

        private void Update()
        {
            if (healthPoints.value <= 0)
            {
                Die();
            }
        }

        private void AwardExperience(GameObject instigator)
        {
            if (instigator == null) return;
            float xpReward = GetComponent<BaseStats>().GetCalculatedStat(Stat.ExperienceReward);
            instigator.GetComponent<Experience>().GainExperience(xpReward);
        }

        private void UpdateHealth()
        {
            healthPoints.value = baseStats.GetCalculatedStat(Stat.Health) * percentRegenOnLevelUp / 100f;
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetPercentHealth()
        {
            return 100 * (healthPoints.value / baseStats.GetCalculatedStat(Stat.Health));
        }

        public float GetCurrentHealth()
        {
            return healthPoints.value;
        }

        public float GetMaxHealth()
        {
            return baseStats.GetCalculatedStat(Stat.Health);
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            healthPoints.value = Mathf.Max(healthPoints.value - damage, 0);

            if (healthPoints.value <= 0)
            {
                Die();
                AwardExperience(instigator);
                onDie.Invoke();
            }
            else
            {
                takeDamage.Invoke(damage);
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

        public void Heal(float healAmount)
        {
            healthPoints.value = Mathf.Min(healthPoints.value + healAmount, GetMaxHealth());
        }

        public object CaptureState()
        {
            return healthPoints.value;
        }

        public void RestoreState(object state)
        {
            float savedHealthPoints = (float)state;
            healthPoints.value = savedHealthPoints;
        }
    }
}
