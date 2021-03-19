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
        BoxCollider boxCollider = null;
        int childCount = 0;

        private void Start()
        {
            boxCollider = gameObject.GetComponent<BoxCollider>();
            childCount = gameObject.transform.childCount;
        }

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
            ShowPickup(false);
            yield return new WaitForSeconds(hideTime);
            ShowPickup(true);
        }

        private void ShowPickup(bool isShown)
        {
            boxCollider.enabled = isShown;
            for (int i = 0; i < childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isShown);
            }
        }
    }
}
