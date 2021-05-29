using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Attributes
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] RectTransform healthBarForeground;
        [SerializeField] Health entityHealth;

        float currentHealthPercent;
        bool isEnabled;

        void Update()
        {
            currentHealthPercent = entityHealth.GetPercentHealth();

            if (Mathf.Approximately(currentHealthPercent, 100f) || entityHealth.IsDead())
            {
                isEnabled = false;
                ShowHealthBar(isEnabled);
                return;
            }
            else
            {
                isEnabled = true;
                ShowHealthBar(isEnabled);
            }

            healthBarForeground.localScale = new Vector3(currentHealthPercent/100, 1f, 1f);
        }

        private void ShowHealthBar(bool isEnabled)
        {
            GetComponentInChildren<Canvas>().enabled = isEnabled;
        }
    }
}
