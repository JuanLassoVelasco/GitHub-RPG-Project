using UnityEngine;
using RPG.Core;

namespace RPG.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {

        [SerializeField] float weaponDamage = 5f;
        [SerializeField] float weaponRange = 2.0f;
        [SerializeField] AnimatorOverrideController animatorOveride = null;
        [SerializeField] GameObject equippedPrefab = null;
        [SerializeField] Projectile projectile = null;
        [SerializeField] bool isRightHanded = true;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (equippedPrefab != null)
            {
                Transform wieldingHand = GetWieldingHand(rightHand, leftHand);
                Instantiate(equippedPrefab, wieldingHand);
            }
            if (animatorOveride != null)
            {
                animator.runtimeAnimatorController = animatorOveride;
            }
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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            Projectile projectileInstance = Instantiate(projectile, GetWieldingHand(rightHand, leftHand).position, Quaternion.identity);
            projectileInstance.SetTarget(target, weaponDamage);
        }

        public float GetWeaponDamage()
        {
            return weaponDamage;
        }

        public float GetWeaponRange()
        {
            return weaponRange;
        }
    }
}
