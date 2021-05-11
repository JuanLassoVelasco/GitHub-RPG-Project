using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] int startingLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        [SerializeField] GameObject levelUpFX = null;
        [SerializeField] bool shouldUseModifiers = false;

        Experience experience;
        IModifierProvider[] modifierProviders;

        LazyValue<int> currentLevel;

        public event Action onLevelUp;

        private void Awake()
        {
            modifierProviders = GetComponents<IModifierProvider>();
            experience = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }

        private void Start()
        {
            currentLevel.ForceInit();
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
            if (newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
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
            return progression.GetStat(statToGet, characterClass, currentLevel.value);
        }


        public int GetLevel()
        {
            return currentLevel.value;
        }

        private int CalculateLevel()
        {
            if (experience == null) return startingLevel;

            float currentXP = experience.GetExperiencePoints();
            int maxLevel = progression.GetMaxLevel(Stat.ExperienceToLevelUp, characterClass);

            for (int level = 1; level <= maxLevel; level++)
            {
                if (currentXP < progression.GetStat(Stat.ExperienceToLevelUp, characterClass, level))
                {
                    return level;
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
