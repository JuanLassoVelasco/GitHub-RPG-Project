using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ImpactCleaner : MonoBehaviour
    {
        ParticleSystem impactFX;

        // Start is called before the first frame update
        void Start()
        {
            impactFX = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (impactFX.isStopped)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
