using System;
using UnityEngine;
using RPG.Attributes;
using UnityEngine.UI;

namespace RPG.Combat
{
    public class EnemyHealthDisplay : MonoBehaviour
    {
        Fighter player;
        Health enemyHealth;
        Text healthText;

        private void Awake()
        {
            healthText = GetComponent<Text>();
            player = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update()
        {
            enemyHealth = player.GetCurrentTarget();
            if (enemyHealth != null)
            {
                healthText.text = "Enemy: " + Math.Round(enemyHealth.GetCurrentHealth(), 0).ToString() + "/" + Math.Round(enemyHealth.GetMaxHealth(), 0).ToString();
            }
            else
            {
                healthText.text = "";
            }
        }
    }
}
