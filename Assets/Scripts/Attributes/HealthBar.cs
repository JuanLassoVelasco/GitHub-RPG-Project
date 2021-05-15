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

        void Update()
        {
            currentHealthPercent = entityHealth.GetPercentHealth();
            healthBarForeground.localScale = new Vector3(currentHealthPercent/100, 1f, 1f);
        }
    }
}
