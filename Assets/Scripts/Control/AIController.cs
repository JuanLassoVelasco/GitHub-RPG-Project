using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using System;
using GameDevTV.Utils;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float susWaitTime = 5f;
        [SerializeField] float agroCooldownTime = 5f;
        [SerializeField] ControlPath controlPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 5f;
        [SerializeField] float patrolSpeedFraction = 0.2f;
        [SerializeField] float shoutDistance = 5f;

        GameObject player;
        Fighter enemyFighter;
        Health enemyHealth;
        Mover enemyMover;
        ActionScheduler actionScheduler;

        LazyValue<Vector3> guardPosition;
        int currentWaypoint = 0;
        float timeSinceLastSawPlayer = Mathf.Infinity;
        float timeSinceLastAgroed = Mathf.Infinity;
        float timeSpentAtWaypoint = 0;

        private void Awake()
        {
            player = GameObject.FindWithTag("Player");
            enemyFighter = GetComponent<Fighter>();
            enemyHealth = GetComponent<Health>();
            enemyMover = GetComponent<Mover>();
            actionScheduler = GetComponent<ActionScheduler>();
            guardPosition = new LazyValue<Vector3>(GetInitialGuardPosition);
        }

        private Vector3 GetInitialGuardPosition()
        {
            return transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {
            guardPosition.ForceInit();
        }

        // Update is called once per frame
        void Update()
        {


            if (enemyHealth.IsDead()) return;

            AttackPlayer();
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastAgroed += Time.deltaTime;
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSpentAtWaypoint += Time.deltaTime;
        }

        public void Aggrevate()
        {
            timeSinceLastAgroed = 0;
        }

        private void AttackPlayer()
        {
            if (IsAggrevated())
            {
                timeSinceLastSawPlayer = 0;
                enemyFighter.Attack(player);
                AggrevateNearbyEnemies();
            }
            else if (timeSinceLastSawPlayer < susWaitTime)
            {
                actionScheduler.CancelCurrentAction();
            }
            else
            {
                PatrolBehavior();
            }
        }

        private void AggrevateNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up);

            foreach (RaycastHit hit in hits)
            {
                AIController enemy = hit.transform.GetComponent<AIController>();

                if (enemy == null) continue;

                enemy.Aggrevate();
            }
        }

        private bool IsAggrevated()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

            return distanceToPlayer < chaseDistance || timeSinceLastAgroed < agroCooldownTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPostion = guardPosition.value;

            if (controlPath != null)
            {
                if (AtWaypoint())
                {
                    if (timeSpentAtWaypoint > waypointDwellTime)
                    {
                        CycleWaypoint();
                        timeSpentAtWaypoint = 0;
                    }
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
