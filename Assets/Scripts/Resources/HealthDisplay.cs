using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Resources
{
    public class HealthDisplay : MonoBehaviour
    {
        Health health;
        Text healthText;

        private void Awake()
        {
            health = GameObject.FindWithTag("Player").GetComponent<Health>();
            healthText = GetComponent<Text>();
        }

        void Update()
        {
            healthText.text = "Health: " + Math.Round(health.GetCurrentHealth(), 0).ToString() + "/" + Math.Round(health.GetMaxHealth(), 0).ToString();
        }
    }
}
