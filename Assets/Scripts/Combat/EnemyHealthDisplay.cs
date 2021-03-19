using System;
using UnityEngine;
using RPG.Resources;
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

        void Update()
        {
            enemyHealth = player.GetCurrentTarget();
            if (enemyHealth != null)
            {
                healthText.text = "Enemy: " + Math.Round(enemyHealth.GetPercentHealth(), 0).ToString() + "%";
            }
            else
            {
                healthText.text = "";
            }
        }
    }
}
