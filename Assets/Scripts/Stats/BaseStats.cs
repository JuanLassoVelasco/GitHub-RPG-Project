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

        Experience experience;

        private void Awake()
        {
            experience = GetComponent<Experience>();
            currentLevel = CalculateLevel();
            if (experience != null)
            {
                experience.onExperienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > currentLevel)
            {
                currentLevel = newLevel;
                print("leveled up");
            }
        }

        public float GetBaseStat(Stat statToGet)
        {
            return progression.GetStat(statToGet, characterClass, currentLevel);
        }

        public int GetLevel()
        {
            return currentLevel;
        }

        public int CalculateLevel()
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
    }
}
