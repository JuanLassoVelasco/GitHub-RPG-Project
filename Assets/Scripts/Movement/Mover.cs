using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;
using RPG.Saving;
using RPG.Resources;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable
    {

        [SerializeField] float maxSpeed = 6f;

        NavMeshAgent navMeshAgent;
        Health health;

        // Start is called before the first frame update
        void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }

        public void StartMovement(Vector3 destination, float speedMultiplier)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedMultiplier);
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead())
            {
                navMeshAgent.enabled = false;
            }

            UpdateAnimator();
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }

        public void MoveTo(Vector3 destination, float speedMultiplier)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * speedMultiplier;
            navMeshAgent.isStopped = false;
        }

        [System.Serializable]
        struct MoverSaveData
        {
            public SerializableVector3 position;
            public SerializableVector3 rotation;
        }

        public object CaptureState()
        {
            MoverSaveData data = new MoverSaveData();
            data.position = new SerializableVector3(transform.position);
            data.rotation = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            MoverSaveData saveData = (MoverSaveData)state;
            navMeshAgent.enabled = false;
            transform.position = saveData.position.ToVector();
            transform.eulerAngles = saveData.rotation.ToVector();
            navMeshAgent.enabled = true;
        }
    }
}
