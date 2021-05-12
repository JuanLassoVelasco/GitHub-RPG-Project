using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.UI.DamageText
{
    public class DamageTextSpawner : MonoBehaviour
    {
        [SerializeField] DamageText damageTextPrefab;

        public void Spawn(float damage)
        {
            DamageText damageTextInstance = Instantiate<DamageText>(damageTextPrefab, transform);
        }
    }
}
