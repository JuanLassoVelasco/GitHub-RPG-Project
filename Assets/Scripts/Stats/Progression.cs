using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;

        public float GetHealth(CharacterClass currentCharacterClass, int level)
        {
            float characterHealth = 0;

            foreach (ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.characterClass == currentCharacterClass)
                {
                    characterHealth = progressionClass.health[level - 1];
                }
            }

            return characterHealth;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            public CharacterClass characterClass = CharacterClass.Grunt;
            public float[] health;
        }
    }
}
