using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] GameObject targetToDestroy;

    // Animation event to destroy text
    public void DestroyTarget()
    {
        Destroy(targetToDestroy);
    }

}
