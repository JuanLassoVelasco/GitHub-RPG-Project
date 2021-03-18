using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;

        Fighter player = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                print("ran into weapon");
                player = other.GetComponent<Fighter>();
                player.EquipWeapon(weapon);
                Destroy(this.gameObject);
            }
        }
    }
}
