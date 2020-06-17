using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] Transform target;


    NavMeshAgent playerNav;

    // Start is called before the first frame update
    void Start()
    {
        playerNav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            playerNav.SetDestination(target.position);
        }
    }
}
