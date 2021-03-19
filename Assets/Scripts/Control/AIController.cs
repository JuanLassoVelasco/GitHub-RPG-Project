using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Resources;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float susWaitTime = 5f;
        [SerializeField] ControlPath controlPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] float patrolSpeedFraction = 0.2f;

        GameObject player;
        Fighter enemyFighter;
        Health enemyHealth;
        Mover enemyMover;
        ActionScheduler actionScheduler;

        Vector3 guardPosition;
        int currentWaypoint = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSpentAtWaypoint = 0;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindWithTag("Player");
            enemyFighter = GetComponent<Fighter>();
            enemyHealth = GetComponent<Health>();
            enemyMover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();

            guardPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

            if (enemyHealth.IsDead()) return;

            AttackPlayer(distanceFromPlayer);
        }

        private void AttackPlayer(float distanceToPlayer)
        {
            if (distanceToPlayer < chaseDistance)
            {
                timeSinceLastSawPlayer = 0;
                enemyFighter.Attack(player);
            }
            else if (timeSinceLastSawPlayer < susWaitTime)
            {
                actionScheduler.CancelCurrentAction();
            }
            else
            {
                PatrolBehavior();
            }

            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPostion = guardPosition;

            if (controlPath != null)
            {
                if (AtWaypoint())
                {
                    if (timeSpentAtWaypoint > waypointDwellTime)
                    {
                        CycleWaypoint();
                        timeSpentAtWaypoint = 0;
                    }
                    timeSpentAtWaypoint += Time.deltaTime;
                }
                nextPostion = GetCurrentWaypoint();
            }

            enemyMover.StartMovement(nextPostion, patrolSpeedFraction);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return controlPath.GetWaypoint(currentWaypoint);
        }

        private void CycleWaypoint()
        {
            currentWaypoint = controlPath.GetNextWaypoint(currentWaypoint);
        }

        private bool AtWaypoint()
        {
            float distaceFromWaypoint = Vector3.Distance(GetCurrentWaypoint(), transform.position);
            return distaceFromWaypoint < waypointTolerance;
        }

        // Called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
