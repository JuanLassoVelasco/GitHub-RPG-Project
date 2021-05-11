using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Resources;
using RPG.Control;

namespace RPG.Combat
{
    [RequireComponent(typeof(Health))]

    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController callingPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                callingPlayer.GetComponent<Fighter>().Attack(this.gameObject);
            }

            return true;
        }
    }
}
