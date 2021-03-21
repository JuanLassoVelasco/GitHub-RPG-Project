using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

        public float GetStat(Stat statToGet, CharacterClass currentCharacterClass, int level)
        {
            BuildLookup();

            float[] levels = lookupTable[currentCharacterClass][statToGet];

            if (levels.Length < level) return 0;

            return levels[level - 1];
        }

        private void BuildLookup()
        {
            if (lookupTable != null) return;

            lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                Dictionary<Stat, float[]> statsTable = new Dictionary<Stat, float[]>();

                foreach (ProgressionStats progressionStat in progressionClass.stats)
                {
                    statsTable[progressionStat.stat] = progressionStat.levelValues;
                }
                lookupTable[progressionClass.characterClass] = statsTable;
            }
        }

        public int GetMaxLevel(Stat stat, CharacterClass characterClass)
        {
            BuildLookup();

            return lookupTable[characterClass][stat].Length;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass;
            public ProgressionStats[] stats;
        }

        [System.Serializable]
        class ProgressionStats
        {
            public Stat stat;
            public float[] levelValues;
        }
    }
}
