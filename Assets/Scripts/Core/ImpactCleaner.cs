using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class ImpactCleaner : MonoBehaviour
    {
        [SerializeField] GameObject objectToDestroy = null;

        ParticleSystem impactFX;

        // Start is called before the first frame update
        void Start()
        {
            impactFX = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!impactFX.IsAlive())
            {
                if (objectToDestroy == null)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    Destroy(objectToDestroy);
                }
            }
        }
    }
}
