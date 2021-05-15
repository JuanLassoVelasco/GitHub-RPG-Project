using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using RPG.Movement;
using System;
using RPG.Attributes;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {

        Mover mover;
        Health playerHealth;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] float navMeshPointingTolerance = 0.2f;
        [SerializeField] float maxTravelDistance = 40f;
        [SerializeField] CursorMapping[] cursorMappings = null;

        // Start is called before the first frame update
        void Awake()
        {
            mover = GetComponent<Mover>();
            playerHealth = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithUI()) return;
            if (playerHealth.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            SetCursor(CursorType.Movement);
            return MoveToCursor();
        }

        private bool MoveToCursor()
        {
            Vector3 target;
            bool hasHit = RaycastNavmesh(out target);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMovement(target, 1f);
                }
            }
            return hasHit;
        }

        private bool RaycastNavmesh(out Vector3 target)
        {
            RaycastHit rayHit;
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out rayHit);
            if (!hasHit)
            {
                return false;
            }

            NavMeshHit navHit;

            bool targetFound = NavMesh.SamplePosition(rayHit.point, out navHit, navMeshPointingTolerance, NavMesh.AllAreas);
            if (targetFound)
            {
                target = navHit.position;
            }
            else
            {
                return false;
            }

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);

            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > maxTravelDistance) return false;
            
            return targetFound;
        }

        private float GetPathLength(NavMeshPath path)
        {
            float pathLength = Vector3.Distance(this.transform.position, path.corners[0]);

            for (int i = 1; i < path.corners.Length; i++)
            {
                pathLength += Vector3.Distance(path.corners[i - 1], path.corners[i]);
            }

            return pathLength;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if (mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[cursorMappings.Length - 1];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
