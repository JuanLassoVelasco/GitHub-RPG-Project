using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int currentLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpFX = null;
        [SerializeField] bool shouldUseModifiers = false;

        Experience experience;
        IModifierProvider[] modifierProviders;

        public event Action onLevelUp;

        private void Awake()
        {
            modifierProviders = GetComponents<IModifierProvider>();
            experience = GetComponent<Experience>();
        }

        private void Start()
        {
            currentLevel = CalculateLevel();
        }

        private void OnEnable()
        {
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void OnDisable()
        {
            if (experience != null)
            {
                experience.onExperienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                onLevelUp();
                Instantiate(levelUpFX, this.transform);
            }
        }

        public float GetCalculatedStat(Stat statToGet)
        {
            return (GetBaseStat(statToGet) + GetAdditiveModifier(statToGet)) * (1f + GetPercentageModifier(statToGet));
        }

        private float GetBaseStat(Stat statToGet)
        {
            return progression.GetStat(statToGet, characterClass, currentLevel);
        }


        public int GetLevel()
        {
            return currentLevel;
        }

        private int CalculateLevel()
        {
            if (experience == null) return currentLevel;

            float currentXP = experience.GetExperiencePoints();
            int maxLevel = progression.GetMaxLevel(Stat.ExperienceToLevelUp, characterClass);

            for (int characterLevel = currentLevel; characterLevel <= maxLevel; characterLevel++)
            {
                if (currentXP < progression.GetStat(Stat.ExperienceToLevelUp, characterClass, characterLevel))
                {
                    return characterLevel;
                }
            }

            return maxLevel;
        }

        private float GetAdditiveModifier(Stat statToGet)
        {
            if (!shouldUseModifiers) return 0;

            float modifierTotal = 0f;

            foreach (IModifierProvider modifierProvider in modifierProviders)
            {
                foreach (float modifier in modifierProvider.GetAdditiveModifiers(statToGet))
                {
                    modifierTotal += modifier;
                }
            }

            return modifierTotal;
        }

        private float GetPercentageModifier(Stat statToGet)
        {
            if (!shouldUseModifiers) return 0;

            float percentBonusTotal = 0f;

            foreach (IModifierProvider percentModProvider in modifierProviders)
            {
                foreach (float percentBonus in percentModProvider.GetPercentageModifiers(statToGet))
                {
                    percentBonusTotal += percentBonus;
                }
            }

            return percentBonusTotal / 100f;
        }
    }
}
