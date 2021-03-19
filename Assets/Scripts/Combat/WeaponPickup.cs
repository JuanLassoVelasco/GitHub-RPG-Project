using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] Weapon weapon = null;
        [SerializeField] float respawnTime = 5;

        Fighter player = null;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                player = other.GetComponent<Fighter>();
                player.EquipWeapon(weapon);
                StartCoroutine(HideForSeconds(respawnTime));
            }
        }

        private IEnumerator HideForSeconds(float hideTime)
        {
            HidePickup();
            yield return new WaitForSeconds(hideTime);
            //ShowPickup();
        }

        private void HidePickup()
        {
            BoxCollider boxCollider = gameObject.GetComponent<BoxCollider>();
            boxCollider.enabled = false;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
