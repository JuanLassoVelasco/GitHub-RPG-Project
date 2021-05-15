using UnityEngine;
using RPG.Attributes;
using System;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {

        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float percentageBonus = 0f;
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] AnimatorOverrideController animatorOveride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isRightHanded = true;

        const string WEAPON_NAME = "Weapon";

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            var overideController = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (equippedPrefab != null)
            {
                Transform wieldingHand = GetWieldingHand(rightHand, leftHand);
                GameObject weapon = Instantiate(equippedPrefab, wieldingHand);
                weapon.name = WEAPON_NAME;
            }
            if (animatorOveride != null)
            {
                animator.runtimeAnimatorController = animatorOveride;
            }
            else if(overideController != null)
            {
                animator.runtimeAnimatorController = overideController.runtimeAnimatorController;
            }
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            Transform oldWeapon = rightHand.Find(WEAPON_NAME);
            if (oldWeapon == null)
            {
                oldWeapon = leftHand.Find(WEAPON_NAME);
            }
            if (oldWeapon == null) return;

            oldWeapon.name = "OLDNEWSDESTROY";

            Destroy(oldWeapon.gameObject);
        }

        private Transform GetWieldingHand(Transform rightHand, Transform leftHand)
        {
            Transform wieldingHand;
            if (isRightHanded)
            {
                wieldingHand = rightHand;
            }
            else
            {
                wieldingHand = leftHand;
            }

            return wieldingHand;
        }

        public bool HasProjectile()
        {
            return projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, GameObject instigator, Health target, float calculatedDamage)
        {
            Projectile projectileInstance = Instantiate(projectile, GetWieldingHand(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, instigator, calculatedDamage);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }

        public float GetPercentageBonus()
        {
            return percentageBonus;
        }
    }
}
