using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Stats;

public class LevelDisplay : MonoBehaviour
{
    BaseStats playerStats;
    Text levelText;

    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        levelText = GetComponent<Text>();
    }

    void Update()
    {
        levelText.text = "Level: " + playerStats.CalculateLevel().ToString();
    }
}
