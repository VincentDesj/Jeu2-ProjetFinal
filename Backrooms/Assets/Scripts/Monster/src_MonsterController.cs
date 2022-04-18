using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class src_MonsterController : MonoBehaviour
{
    public GameObject terrain;
    public Bounds terrainBounds;
    public NavMeshAgent navMeshAgent;

    public Vector3 nextPosition;

    void Awake()
    {
        terrainBounds = terrain.gameObject.GetComponent<Collider>().bounds;
        nextPosition = new Vector3();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        GetNextPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (!navMeshAgent.pathPending)
        {
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    GetNextPosition();
                    navMeshAgent.SetDestination(nextPosition);
                }
            }
        }
    }

    private void GetNextPosition()
    {
        nextPosition.Set(UnityEngine.Random.Range(terrainBounds.min.x, terrainBounds.max.x),
                        terrainBounds.max.y,
                        UnityEngine.Random.Range(terrainBounds.min.z, terrainBounds.max.z));
    }
}
