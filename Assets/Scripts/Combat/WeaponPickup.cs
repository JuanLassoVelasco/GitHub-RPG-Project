using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Control;
using RPG.Attributes;

namespace RPG.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] WeaponConfig weapon = null;
        [SerializeField] float healthToRestore;
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
                PickUp(player);
            }
        }

        private void PickUp(Fighter fighter)
        {
            if (weapon != null)
            {
                fighter.EquipWeapon(weapon);
            }

            if (healthToRestore > 0)
            {
                fighter.gameObject.GetComponent<Health>().Heal(healthToRestore);
            }

            StartCoroutine(HideForSeconds(respawnTime));
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

        public bool HandleRaycast(PlayerController callingPlayer)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PickUp(callingPlayer.GetComponent<Fighter>());
            }

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
