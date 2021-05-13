using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.UI.DamageText
{
    public class DamageText : MonoBehaviour
    {
        public void SetValue(float damage)
        {
            GetComponentInChildren<Text>().text = damage.ToString();
        }
    }
}
