using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Stats
{
    public class ExperienceDisplay : MonoBehaviour
    {
        Experience experience;
        Text experienceText;

        private void Awake()
        {
            experience = GameObject.FindWithTag("Player").GetComponent<Experience>();
            experienceText = GetComponent<Text>();
        }

        void Update()
        {
            experienceText.text = "XP: " + experience.GetExperiencePoints().ToString();
        }
    }
}
